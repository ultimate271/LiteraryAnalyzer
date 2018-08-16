﻿using System;
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
		public static MDTagFile WriteTagFileDefault(this LitOptions LO, LitNovel novel, MDAnnSourceInfo info) {
			var retVal = new MDTagFile();
			var Tags = new List<MDTag>();
			string Filename;

			foreach (var author in novel.Authors) {
				foreach (var metadata in novel.SceneMetadata) {
					Filename = LO.ToShortFilename(info, author, metadata);
					var query = novel.Scenes.Where(s => s.Metadata == metadata);
					foreach (var scene in query) {
						Tags.AddRange(LO.GetAllTags(scene, Filename));
					}
				}
			}
			Filename = ToNotesShortFilename(info);
			foreach (var litRef in novel.References) {
				foreach (var Tag in litRef.Tags) {
					Tags.Add(new MDTag() { TagName = Tag.Tag, TagFile = Filename, TagLine = LO.WriteNotesHeader(novel, litRef) });
				}
			}

			retVal.Lines = Tags.Select(t => t.ToString()).ToList();
			return retVal;
		}
	}
}
