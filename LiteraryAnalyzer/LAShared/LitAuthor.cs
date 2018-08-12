using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents the information about where a LitSource came from
	/// </summary>
	public class LitAuthor {
		public String Author { get; set; } = "Original";
	}
	public static partial class LitExtensions {
		public static bool IsSourceInfoIntersection(this LitAuthor info1, LitAuthor info2) {
			return info1.Author.Equals(info2.Author);
		}
	}
	public static partial class ParsingTools {
		public static void WriteToFilesystem(this MDAnnSource sourceObjects, MDAnnSourceInfo info ) {
			foreach (var source in sourceObjects.Sources) {
				System.IO.File.WriteAllLines(source.ToLongFilename(info), source.Lines);
			}
			System.IO.File.WriteAllLines(sourceObjects.Notes.ToLongFilename(info), sourceObjects.Notes.Lines);
			System.IO.File.WriteAllLines(sourceObjects.TagFile.ToLongFilename(info), sourceObjects.TagFile.Lines);
		}
		public static String ToSourceLine(this LitAuthor sourceinfo) {
			return ParsingTools.MakeLinkLine("Author", sourceinfo.Author);
		}
	}
}
