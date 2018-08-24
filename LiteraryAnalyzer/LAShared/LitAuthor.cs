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
		public static LitAuthor ParseAuthorDefault(
			this LitOptions LO, 
			LitNovel novel, 
			IEnumerable<String> metadatalines
		){
			var retVal = new LitAuthor();
			var links = metadatalines.Select(l => LO.ParseLink(l)).Where(link => link != null);
			retVal.Author = links.Where(link => link.Link.Equals("Author")).Select(link => link.Tag).FirstOrDefault();
			return novel.AddAuthorDistinct(retVal);
		} 

		public static void WriteToFileSystemDefault(this LitOptions LO, 
			MDAnnSource sourceObjects, 
			MDAnnSourceInfo info 
		){
			foreach (var source in sourceObjects.Sources) {
				System.IO.File.WriteAllLines(LO.ToLongSourceFilename(info, source), source.Lines);
			}
			System.IO.File.WriteAllLines(LO.ToLongNotesFilename(info), sourceObjects.Notes.Lines);
			System.IO.File.WriteAllLines(LO.ToLongTagFilename(info), sourceObjects.TagFile.Lines);
		}
		public static void WriteToFileSystemRaw(
			this LitOptions LO,
			MDAnnSource sourceObjects,
			MDAnnSourceInfo info
		){
			foreach (var source in sourceObjects.Sources) {
				String dir = String.Format("{0}\\{1}", info.BaseDir, info.Prefix);
				if (!System.IO.Directory.Exists(dir)) {
					System.IO.Directory.CreateDirectory(dir);
				}
				System.IO.File.WriteAllLines(
					LO.ToLongSourceFilename(info, source),
					source.Lines
				);
			}
		}

		public static String ToSourceLine(this LitAuthor sourceinfo) {
			return ParsingTools.MakeLinkLine("Author", sourceinfo.Author);
		}
	}
}
