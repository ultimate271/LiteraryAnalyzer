using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDNotesFile : MDFile {
	}
	public static partial class ParsingTools {
		public static String ToNotesShortFilename(LitAnnSourceInfo info) {
			return new MDNotesFile().ToShortFilename(info);
		}
		public static String ToLongFilename(this MDNotesFile notesfile, LitAnnSourceInfo info) {
			return String.Format("{0}\\{1}", info.BaseDir, notesfile.ToShortFilename(info));
		}
		public static String ToShortFilename(this MDNotesFile notesfile, LitAnnSourceInfo info) {
			return String.Format("{0}notes.md", info.Prefix);
		}
		public static MDNotesFile CreateNotesFile(this LitNovel novel) {
			var retVal = new MDNotesFile();
			foreach (var reference in novel.References) {
				retVal.Lines.AddRange(reference.ToNotesLines(novel));
			}
			return retVal;
		}
	}
}
