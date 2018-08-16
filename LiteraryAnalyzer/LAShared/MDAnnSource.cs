using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Object represenation of the files that get written to/read from the file system
	/// </summary>
	public class MDAnnSource {
		public List<MDSourceFile> Sources { get; set; } = new List<MDSourceFile>();
		public MDNotesFile Notes { get; set; } = new MDNotesFile();
		public MDTagFile TagFile { get; set; } = new MDTagFile();
	}
	public static partial class ParsingTools {
		/// <summary>
		/// Kick off point for creating the source objects out of the novel
		/// </summary>
		/// <param name="novel"></param>
		/// <returns></returns>
		public static MDAnnSource WriteAnnSourceDefault(this LitOptions LO, LitNovel novel) {
			var retVal = new MDAnnSource();
			foreach (var author in novel.Authors) {
				foreach (var metadata in novel.SceneMetadata) {
					var lines = LO.WriteMetadata(metadata, author);
					var query = novel.Scenes
						.Where(s => s.Metadata == metadata)
						.Select(s => LO.WriteElmSourceLines(s, author));
					foreach (var scenelines in query) {
						lines.AddRange(scenelines);
					}
					var SourceFile = new MDSourceFile() {
						Descriptor = metadata.Descriptor,
						Author = author.Author,
						Lines = lines
					};
					retVal.Sources.Add(SourceFile);
				}
			}
			retVal.Notes = LO.WriteNotesFile(novel);

			return retVal;
		}
		/// <summary>
		/// Takes the source info, and compiles it all together into a LitAnnSource object
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static MDAnnSource BuildAnnSourceDefault(this LitOptions LO, MDAnnSourceInfo info) {
			//TODO modularize this function
			var retVal = new MDAnnSource();

			//Get the filenames
			var files = LO.BuildSourceFilenames(info);

			//Insert the source files
			retVal.Sources = LO.BuildSourceFiles(info, files);

			//Insert the notes file
			retVal.Notes = LO.BuildNotesFile(info, files);

			return retVal;
		}
		public static IEnumerable<String> BuildSourceFilenamesDefault(this LitOptions LO, MDAnnSourceInfo info) {
			var files = System.IO.Directory.GetFiles(info.BaseDir, info.Prefix + "*.md");
			Array.Sort(files);
			return files;
		}
		public static List<MDSourceFile> BuildSourceFilesDefault(this LitOptions LO, MDAnnSourceInfo info, IEnumerable<String> files) {
			var retVal = new List<MDSourceFile>();
			string pattern = String.Format(@"{0}(\d[\d\.]+)\.([^\.]+)\.md", info.Prefix);
			var query = files.Select(s => new { File = s, Match = System.Text.RegularExpressions.Regex.Match(s, pattern) });
			MDSourceFile SourceObj;
			foreach (var file in query) {
				SourceObj = new MDSourceFile();
				SourceObj.Lines = new List<String>(System.IO.File.ReadAllLines(file.File));
				SourceObj.Descriptor = file.Match.Groups[1].Value;
				SourceObj.Author = file.Match.Groups[2].Value;
				retVal.Add(SourceObj);
			}
			return retVal;
		}
		public static MDNotesFile BuildNotesFileDefault(this LitOptions LO, MDAnnSourceInfo info, IEnumerable<String> files) {
			var retVal = new MDNotesFile();
			var pattern = String.Format(@"{0}notes.md", info.Prefix);
			var NotesFileLines = files.Where(s => System.Text.RegularExpressions.Regex.IsMatch(s, pattern));
			if (NotesFileLines.Count() > 0) {
				var NotesFile = new MDNotesFile();
				NotesFile.Lines = new List<string>(System.IO.File.ReadAllLines(NotesFileLines.First()));
				retVal = NotesFile;
			}
			return retVal;
		}
		public static void TagAnnSourceDefault(this LitOptions LO, MDAnnSource source) {
			foreach (var sourceFile in source.Sources) {
				LO.TagSourceFile(sourceFile);
			}
		}
		/// <summary>
		/// Depricated
		/// </summary>
		/// <param name="source"></param>
		/// <param name="novel"></param>
		//public static void SetAllLitSourceInfo(this LitAnnSource source, LitNovel novel) {
		//	foreach (var sourcefile in source.Sources) {
		//		sourcefile.ParseLitSourceInfo(novel);
		//	}
		//}
		/// <summary>
		/// I shouldn't ever use this function
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static List<String> AllLines(this MDAnnSource source) {
			var retVal = new List<String>();
			foreach (var sourcefile in source.Sources) {
				retVal.AddRange(sourcefile.Lines);
			}
			return retVal;
		}
	}
}
