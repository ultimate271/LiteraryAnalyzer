using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDNotesFile : MDFile {
	}
	public static partial class ParsingTools {
		/// <summary>
		/// Takes the lines of the notes, and populates the novel References and such appropiately
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="lines"></param>
		public static void ParseNotesFileDefault(this LitOptions LO, 
			LitNovel novel, 
			MDNotesFile notesfile
		){
			var PartitionedLines = LO.ExtractNotesRefs(notesfile);
			LitRef litref = null;
			foreach (var refLines in PartitionedLines) {
				litref = LO.ParseToLitRef(novel, refLines);
				novel.AddReferenceDistinct(litref, false);
			}
		}

		public static IEnumerable<IEnumerable<String>> ExtractNotesRefsDefault(
			this LitOptions LO, 
			MDNotesFile notesfile
		){  
			string pattern = @"^#[^#]";
			return ParsingTools.PartitionLines(
				notesfile.Lines, 
				(s => System.Text.RegularExpressions.Regex.IsMatch(s, pattern))
			);
		}
		public static String ToShortNotesFilenameDefault(
			this LitOptions LO, 
			MDAnnSourceInfo info
		){
			return String.Format("{0}notes.md", info.Prefix);
		}
		public static String ToLongNotesFilenameDefault(
			this LitOptions LO, 
			MDAnnSourceInfo info
		){
			return String.Format(
				"{0}\\{1}",
				info.BaseDir,
				LO.ToShortNotesFilename(info)
			);
		}
		public static MDNotesFile WriteNotesFileDefault(this LitOptions LO, LitNovel novel) {
			var retVal = new MDNotesFile();
			foreach (var reference in novel.References) {
				retVal.Lines.AddRange(LO.WriteNotesLines(novel, reference));
			}
			return retVal;
		}
	}
}
