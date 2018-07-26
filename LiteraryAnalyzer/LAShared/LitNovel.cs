using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitNovel {
		public List<LitScene> Scenes { get; set; }
		public String Title { get; set; }
	}
	public static partial class ParsingTools {
		public static LitNovel ParseAnnSource(this LitAnnSource source) {
			var retVal = new LitNovel();

			//Aggregate the source
			var files = System.IO.Directory.GetFiles(source.BaseDir, source.Prefix + "*.md");
			Array.Sort(files);
			var query = files.Where(s => !s.Contains("notes.md"));
			List<String> allLines = new List<String>();
			foreach (var file in query) {
				var lines = System.IO.File.ReadAllLines(file);
				var shortfilename = Helper.ExtractFilename(file);
				var taggedLines = ParsingTools.TagLines(lines, shortfilename);
				allLines.AddRange(taggedLines);
			}

			var PartitionedScenes = ParsingTools.PartitionLines(allLines, line => System.Text.RegularExpressions.Regex.IsMatch(line, @"^#[^#]"));

			foreach (var Scenelines in PartitionedScenes) {
				var Scene = ParsingTools.ParseScene(Scenelines);
				retVal.Scenes.Add(Scene);
			}

			//Some decisions
			//The very first # does not represent a scene, but instead represents metadata.
			//The user can define a tag manually. This is done by adding a [Tag Element] tag under the header

			return retVal;
		}
		public static LitAnnSource GenerateMarkdown(this LitNovel novel) {
			throw new NotImplementedException();
		}
	}
}
