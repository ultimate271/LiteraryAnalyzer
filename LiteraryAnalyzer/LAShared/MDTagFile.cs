using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDTagFile : MDFile {
	}
	public static partial class ParsingTools {
		public static String ToLongTagFilenameDefault(
			this LitOptions LO, 
			MDAnnSourceInfo info
		) {
			return String.Format("{0}\\{1}", info.BaseDir, LO.ToShortTagFilename(info));
		}
		public static String ToShortTagFilenameDefault(
			this LitOptions LO, 
			MDAnnSourceInfo info
		) {
			return "tags";
		}
		public static MDTagFile WriteTagFileDefault(
			this LitOptions LO, 
			LitNovel novel, 
			MDAnnSourceInfo info
		){ 
			var retVal = new MDTagFile();
			var Tags = new List<MDTag>();
			string Filename;

			foreach (var author in novel.Authors) {
				foreach (var metadata in novel.SceneMetadata) {
					Filename = LO.ToShourtSourceFilename(info, metadata, author);
					var query = novel.Scenes.Where(s => s.Metadata == metadata);
					foreach (var scene in query) {
						Tags.AddRange(LO.GetAllTags(scene, Filename));
					}
				}
			}
			Filename = LO.ToShortNotesFilename(info);
			foreach (var litRef in novel.References) {
				foreach (var Tag in litRef.Tags) {
					Tags.Add(new MDTag() {
						TagName = Tag.Tag,
						TagFile = Filename,
						TagLine = LO.WriteRefTagEX(litRef)
					});
				}
			}

			retVal.Lines = Tags.Select(t => LO.WriteTagLine(t)).ToList();
			return retVal;
		}
		public static String WriteElmTagEXDefault(
			this LitOptions LO,
			LitElm elm
		) {
			var link = new MDLinkLine {
				Link = "TreeTag",
				Tag = elm.TreeTag.Tag
			};
			var linkString = LO.WriteLink(link);
			return String.Format(@"/\v^#.*(\n*{0})@=", SanitizeForRegex(linkString));
		}
		public static String WriteRefTagEXDefault(
			this LitOptions LO,
			LitRef reference
		) {
			return String.Format(@"/\v^# {0}$", SanitizeForRegex(reference.Tags.First().Tag));
		}
		public static String SanitizeForRegex(String s) {
			String[] escape = { "\\\\", "\\{", "\\}", "\\(", "\\)", "\\[", "\\]" };
			foreach (var c in escape) {
				s = System.Text.RegularExpressions.Regex.Replace(s, c, c);
			}
			return s;
		}
	}
}
