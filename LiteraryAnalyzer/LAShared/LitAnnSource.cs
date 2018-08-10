using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitAnnSource {
		public List<MDSourceFile> Sources { get; set; } = new List<MDSourceFile>();
		public MDNotesFile Notes { get; set; } = new MDNotesFile();
		public MDTagFile TagFile { get; set; } = new MDTagFile();
	}
	public static partial class ParsingTools {
		/// <summary>
		/// Takes the source info, and compiles it all together into a LitAnnSource object
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static LitAnnSource BuildSource(this LitAnnSourceInfo info) {
			var retVal = new LitAnnSource();

			//Get the filenames
			var files = System.IO.Directory.GetFiles(info.BaseDir, info.Prefix + "*.md");
			Array.Sort(files);
			
			//Insert the source files
			string pattern = String.Format(@"{0}(\d[\d\.]+)\.([^\.]+)\.md", info.Prefix);
			var query = files.Select(s => new { File = s, Match = System.Text.RegularExpressions.Regex.Match(s, pattern) })
				.Where(a => a.Match.Success && !a.Match.Groups[1].Value.Equals("notes"));
			MDSourceFile SourceObj;
			foreach (var file in query) {
				SourceObj = new MDSourceFile();
				SourceObj.Lines = new List<String>(System.IO.File.ReadAllLines(file.File));
				SourceObj.Descriptor = file.Match.Groups[1].Value;
				SourceObj.Author = file.Match.Groups[2].Value;
				retVal.Sources.Add(SourceObj);
			}

			//Insert the notes file
			pattern = String.Format(@"{0}notes.md", info.Prefix);
			var NotesFileLines = files.Where(s => System.Text.RegularExpressions.Regex.IsMatch(s, pattern));
			if (NotesFileLines.Count() > 0) {
				var NotesFile = new MDNotesFile();
				NotesFile.Lines = new List<string>(System.IO.File.ReadAllLines(NotesFileLines.First()));
				retVal.Notes = NotesFile;
			}

			return retVal;
		}
		public static void TagAllSourceFiles(this LitAnnSource source) {
			foreach (var sourceFile in source.Sources) {
				sourceFile.TagLines();
			}
		}
		/// <summary>
		/// Depricated
		/// </summary>
		/// <param name="source"></param>
		/// <param name="novel"></param>
		public static void SetAllLitSourceInfo(this LitAnnSource source, LitNovel novel) {
			foreach (var sourcefile in source.Sources) {
				sourcefile.ParseLitSourceInfo(novel);
			}
		}
		/// <summary>
		/// I shouldn't ever use this function
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static List<String> AllLines(this LitAnnSource source) {
			var retVal = new List<String>();
			foreach (var sourcefile in source.Sources) {
				retVal.AddRange(sourcefile.Lines);
			}
			return retVal;
		}
	}
}
