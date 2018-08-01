using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitNovel {
		public List<LitScene> Scenes { get; set; } = new List<LitScene>();
		public List<LitRef> References { get; set; } = new List<LitRef>();
		public String Title { get; set; }
		public List<LitRef> GeneratedReference { get; set; } = new List<LitRef>();
		public List<LitSourceInfo> SourceInfo { get; set; } = new List<LitSourceInfo>();
	}
	public static partial class LitExtensions {
		/// <summary>
		/// UPON FURTHER CONSIDERATION, I DONT THINK THIS IS THE PROPER WAY TO DO THINGS
		/// Will take every reference in the scenes of the novel, and make sure that
		/// it corresponds to a reference in the references of the novel
		/// </summary>
		/// <param name="novel"></param>
		public static void SanatizeRefs(this LitNovel novel) {
			
		}
		/// <summary>
		/// UPON FURTHER CONSIDERATION, I DONT THINK THIS IS THE PROPER WAY TO DO THINGS
		/// Takes all the reference objects in the scenes and the references, and combines them all
		/// into the references list of the novel
		/// </summary>
		/// <param name="novel"></param>
		public static void CombineRefs(this LitNovel novel) {
			var currentRefs = novel.GetAllReferences();
			novel.References = new List<LitRef>();
			foreach (var litRef in currentRefs) {
				novel.AddReferenceDistinct(litRef);
			}
		}
		public static LitRef AddReferenceDistinct(this LitNovel novel, LitRef reference) {
			return novel.AddReferenceDistinct(reference, true);
		}
		/// <summary>
		/// Will add a new reference to the list of references of the novel, or,
		/// if the novel has the reference already, will add any new tags that the
		/// current reference might not have.
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="reference"></param>
		public static LitRef AddReferenceDistinct(this LitNovel novel, LitRef reference, bool generated) {
			foreach (var currentRef in novel.References) {
				if (currentRef.IsReferenceIntersection(reference)) {
					currentRef.CombineRef(reference);
					return currentRef;
				}
			}
			novel.References.Add(reference);
			if (generated) {
				novel.GeneratedReference.Add(reference);
			}
			return reference;
		}
		/// <summary>
		/// Will add a new source info to the novel, and return the current source info 
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		public static LitSourceInfo AddSourceInfoDistinct(this LitNovel novel, LitSourceInfo info) {
			foreach (var currentSourceInfo in novel.SourceInfo) {
				if (currentSourceInfo.IsSourceInfoIntersection(info)) {
					return currentSourceInfo;
				}
			}
			novel.SourceInfo.Add(info);
			return info;
		}
		/// <summary>
		/// Adds the scene to the novel
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="scene"></param>
		public static void AddScene(this LitNovel novel, LitScene scene) {
			foreach (var NovelScene in novel.Scenes) {
				if (NovelScene.IsElmMergeable(scene)) {
					NovelScene.MergeScene(scene);
					return;
				}
			}
			novel.Scenes.Add(scene);
		}
		/// <summary>
		/// THIS FUNCTION MIGHT BE USEFUL FOR "CHECKING MY WORK" BUT NOT FOR ANYTHING ELSE
		/// </summary>
		/// <param name="novel"></param>
		/// <returns></returns>
		public static List<LitRef> GetAllReferences(this LitNovel novel) {
			var retVal = novel.References;
			foreach (var scene in novel.Scenes) {
				retVal.AddRange(scene.GetAllReferences());
			}
			return retVal;
		}
	}
	public static partial class ParsingTools {
		public static LitNovel ParseAnnSource(LitAnnSource source) {
			var retVal = new LitNovel();

			//Aggregate the source
			//var files = System.IO.Directory.GetFiles(source.BaseDir, source.Prefix + "*.md");
			//Array.Sort(files);
			//var query = files.Where(s => !s.Contains("notes.md"));
			//List<String> allLines = new List<String>();
			//foreach (var file in query) {
			//	var lines = System.IO.File.ReadAllLines(file);
			//	var shortfilename = Helper.ExtractFilename(file);
			//	var taggedLines = ParsingTools.TagLines(lines, shortfilename);
			//	allLines.AddRange(taggedLines);
			//}
			//var notesFile = files.Where(s => s.Contains(String.Format("{0}notes.md", source.Prefix)));
			//if (notesFile.Count() > 0) {
			//	retVal.ParseNotesFile(System.IO.File.ReadAllLines(notesFile.First()));
			//}

			//Preliminary tagging
			source.SetAllLitSourceInfo(retVal);
			source.TagAllSourceFiles();

			//Parse the current notes file
			retVal.ParseNotesFile(source.Notes.Lines);

			foreach (var sourceFile in source.Sources) {
				var PartitionedScenes = ParsingTools.PartitionLines(
					sourceFile.Lines, 
					line => System.Text.RegularExpressions.Regex.IsMatch(line, @"^#[^#]")
				);

				foreach (var Scenelines in PartitionedScenes) {
					var scene = retVal.ParseScene(Scenelines, sourceFile.LitSourceInfo);
					retVal.AddScene(scene);
				}

			}

			return retVal;
		}
		public static void ParseNotesFile(this LitNovel novel, IEnumerable<String> lines) {
			string pattern = @"^#[^#]";
			var PartitionedLines = ParsingTools.PartitionLines(lines, (s => System.Text.RegularExpressions.Regex.IsMatch(s, pattern)));
			LitRef litref = null;
			foreach (var refLines in PartitionedLines) {
				litref = ParsingTools.ParseLitRef(refLines);
				novel.AddReferenceDistinct(litref, false);
			}
		}
		public static LitAnnSource GenerateMarkdown(this LitNovel novel) {
			throw new NotImplementedException();
		}
	}
}
