using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Is the head node of the three which represents the novel.
	/// 
	/// All of the refernces and sourceinfo of the scenes and events in the Scenes should be object references found in 
	/// the References or SourceInfo of this class. 
	/// (That's a programmer enforcement. I should make a test function which checks this requirement.)
	/// </summary>
	public class LitNovel {
		public List<LitScene> Scenes { get; set; } = new List<LitScene>();
		public List<LitRef> References { get; set; } = new List<LitRef>();
		public String Title { get; set; }
		public List<LitRef> GeneratedReference { get; set; } = new List<LitRef>();
		public List<LitSceneMetadata> SceneMetadata { get; set; } = new List<LitSceneMetadata>();
		public List<LitAuthor> SourceInfo { get; set; } = new List<LitAuthor>();
		public LitOptions LO { get; set; }
		public LitNovel(LitOptions LO) {
			this.LO = LO;
		}
	}
	public static partial class LitExtensions {
		/// <summary>
		/// Will add a new reference to the list of references of the novel, or,
		/// if the novel has the reference already, will add any new tags that the
		/// current reference might not have.
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="reference"></param>
		/// <returns></returns>
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
		public static LitAuthor AddSourceInfoDistinct(this LitNovel novel, LitAuthor info) {
			foreach (var currentSourceInfo in novel.SourceInfo) {
				if (currentSourceInfo.IsSourceInfoIntersection(info)) {
					return currentSourceInfo;
				}
			}
			novel.SourceInfo.Add(info);
			return info;
		}
		/// <summary>
		/// Will add a new SceneMetadata to the novel, or if this metadata already exists, will return the one that already does
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="metadata"></param>
		/// <returns></returns>
		public static LitSceneMetadata AddMetadataDistinct(this LitNovel novel, LitSceneMetadata metadata) {
			foreach (var currentSceneMetadata in novel.SceneMetadata) {
				if (currentSceneMetadata.Descriptor.Equals(metadata.Descriptor)) {
					return currentSceneMetadata;
				}
			}
			novel.SceneMetadata.Add(metadata);
			return metadata;
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
		/// <summary>
		/// Handy little tool that turns out not to be very useful at all
		/// </summary>
		/// <param name="novel"></param>
		/// <returns></returns>
		//public static List<String> WriteNovelOutline(this LitNovel novel) {
		//	List<String> retVal = new List<String>();
		//	foreach (var scene in novel.Scenes) {
		//		retVal.AddRange(scene.WriteOutline(1));
		//	}
		//	return retVal;
		//}
	}
	public static partial class ParsingTools {
		/// <summary>
		/// This is the kickoff point for where the magic happens
		/// </summary>
		/// <param name="source"></param>
		/// <param name="LO"></param>
		/// <returns></returns>
		public static LitNovel ParseAnnSource(MDAnnSource source, LitOptions LO) {
			var retVal = new LitNovel(LO);

			//Preliminary tagging
			LO.TagAllSourceFiles(source);

			//Parse the current notes file and fill the novel with current references
			retVal.ParseNotesFile(source.Notes.Lines);

			foreach (var sourceFile in source.Sources) {
				retVal.ParseSourceToNovel(sourceFile);
			}

			return retVal;
		}
		/// <summary>
		/// Takes the lines of the notes, and populates the novel References and such appropiately
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="lines"></param>
		public static void ParseNotesFile(this LitNovel novel, IEnumerable<String> lines) {
			string pattern = @"^#[^#]";
			var PartitionedLines = ParsingTools.PartitionLines(lines, (s => System.Text.RegularExpressions.Regex.IsMatch(s, pattern)));
			LitRef litref = null;
			foreach (var refLines in PartitionedLines) {
				litref = novel.LO.ParseLitRef(refLines);
				novel.AddReferenceDistinct(litref, false);
			}
		}
		/// <summary>
		/// Returns every tag for the scenes that this actor is contained in
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="actor"></param>
		/// <returns></returns>
		public static IEnumerable<LitTag> ActorTags(this LitNovel novel, LitChar actor) {
			return novel.Scenes.Where(s => s.Actors.Contains(actor)).Select(s => s.TreeTag);
		}
		/// <summary>
		/// Returns every tag for the events that this speaker is contained in
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="speaker"></param>
		/// <returns></returns>
		public static IEnumerable<LitTag> SpeakerTags(this LitNovel novel, LitChar speaker) {
			var retVal = new List<LitTag>();
			foreach (var scene in novel.Scenes) {
				retVal.AddRange(scene.SpeakerTags(speaker));
			}
			return retVal;
		}
		/// <summary>
		/// Kick off point for creating the source objects out of the novel
		/// </summary>
		/// <param name="novel"></param>
		/// <returns></returns>
		public static MDAnnSource CreateSource(this LitNovel novel) {
			var retVal = new MDAnnSource();
			foreach (var LitSourceInfo in novel.SourceInfo) {
				foreach (var Metadata in novel.SceneMetadata) {
					var lines = Metadata.ToSourceLines(LitSourceInfo);
					var query = novel.Scenes
						.Where(s => s.Metadata == Metadata)
						.Select(s => novel.LO.WriteElmSourceLines(s, LitSourceInfo));
					foreach (var scenelines in query) {
						lines.AddRange(scenelines);
					}
					var SourceFile = new MDSourceFile() {
						Descriptor = Metadata.Descriptor,
						Author = LitSourceInfo.Author,
						Lines = lines
					};
					retVal.Sources.Add(SourceFile);
				}
			}
			retVal.Notes = novel.CreateNotesFile();

			return retVal;
		}
	}
}
