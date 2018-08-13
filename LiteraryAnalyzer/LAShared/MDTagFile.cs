using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDTagFile : MDFile {
	}
	public static partial class ParsingTools {
		public static String ToLongFilename(this MDTagFile tagfile, MDAnnSourceInfo info) {
			return String.Format("{0}\\{1}", info.BaseDir, tagfile.ToShortFilename(info));
		}
		public static String ToShortFilename(this MDTagFile tagfile, MDAnnSourceInfo info) {
			return "tags";
		}
		public static MDTagFile CreateTagsFile(this LitNovel novel, MDAnnSourceInfo info) {
			var retVal = new MDTagFile();
			var Tags = new List<MDTag>();
			string Filename;

			foreach (var LitSourceInfo in novel.Authors) {
				foreach (var Metadata in novel.SceneMetadata) {
					Filename = ParsingTools.ToShortFilename(info, LitSourceInfo, Metadata);
					var query = novel.Scenes.Where(s => s.Metadata == Metadata);
					foreach (var scene in query) {
						Tags.AddRange(novel.LO.GetAllTags(scene, Filename));
					}
				}
			}
			Filename = ToNotesShortFilename(info);
			foreach (var litRef in novel.References) {
				foreach (var Tag in litRef.Tags) {
					Tags.Add(new MDTag() { TagName = Tag.Tag, TagFile = Filename, TagLine = novel.LO.WriteNotesHeader(litRef) });
				}
			}

			retVal.Lines = Tags.Select(t => t.ToString()).ToList();
			return retVal;
		}
	}
}
