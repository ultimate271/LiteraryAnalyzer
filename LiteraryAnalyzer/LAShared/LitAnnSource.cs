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
			string pattern = String.Format(@"{0}([^\.]+)\.md", info.Prefix);
			var query = files.Select(s => new { File = s, Match = System.Text.RegularExpressions.Regex.Match(s, pattern) })
				.Where(a => a.Match.Success);
			MDSourceFile SourceObj;
			foreach (var file in query) {
				SourceObj = new MDSourceFile();
				SourceObj.Lines = new List<String>(System.IO.File.ReadAllLines(file.File));
				SourceObj.Descriptor = file.Match.Groups[1].Value;
				retVal.Sources.Add(SourceObj);
			}

			//Insert the tags file
			pattern = String.Format(@"{0}notes.md", info.Prefix);
			var NotesFile = files.Where(s => System.Text.RegularExpressions.Regex.IsMatch(s, pattern));
			if (NotesFile.Count() > 0) {
				var TagFile = new MDTagFile();
				TagFile.Lines = new List<string>(System.IO.File.ReadAllLines(NotesFile.First()));
				retVal.TagFile = TagFile;
			}

			return retVal;
		}
		public static void TagAllSourceFiles(this LitAnnSource source) {
			foreach (var sourceFile in source.Sources) {
				sourceFile.TagLines();
			}
		}
	}
}
