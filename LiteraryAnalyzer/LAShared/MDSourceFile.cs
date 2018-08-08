using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDSourceFile : MDFile {
		public String Descriptor { get; set; } = "";
		public String LitSourceInfo { get; set; } = "";
	}
	public static partial class ParsingTools {
		public static void TagLines(this MDSourceFile sourcefile) {
			sourcefile.Lines = new List<string>(ParsingTools.TagLines(sourcefile.Lines, sourcefile.Descriptor, sourcefile.LitSourceInfo));
		}
		public static void ParseSource(this LitNovel novel, MDSourceFile sourceFile) {
			var PartitionedScenes = ParsingTools.PartitionLines(
				sourceFile.Lines, 
				line => System.Text.RegularExpressions.Regex.IsMatch(line, @"^#[^#]")
			);
			//Extract and add the metadata
			var MetadataLines = ParsingTools.ExtractMetadata(PartitionedScenes);
			var LitSceneMetadata = ParsingTools.ParseMetadata(MetadataLines);
			LitSceneMetadata = novel.AddMetadataDistinct(LitSceneMetadata);

			var LitSourceInfo = ParsingTools.ParseLitSourceInfo(MetadataLines);
			LitSourceInfo = novel.AddSourceInfoDistinct(LitSourceInfo);

			//Extract and add the scenes
			var PartitionedSceneLines = ParsingTools.ExtractScenes(PartitionedScenes);
			foreach (var Scenelines in PartitionedSceneLines) {
				var scene = novel.ParseScene(Scenelines, LitSourceInfo, LitSceneMetadata);
				novel.AddScene(scene);
			}
		}
		public static IEnumerable<String> ExtractMetadata (IEnumerable<IEnumerable<String>> PartitionedScenes){
			return PartitionedScenes.Where(lines => 
				lines.Select(l => ParsingTools.ParseLink(l))
					.Where(link => link != null)
					.Where(link => link.Link.Equals("Metadata"))
					.Count() > 0
				).FirstOrDefault();
		}
		public static IEnumerable<IEnumerable<String>> ExtractScenes(IEnumerable<IEnumerable<String>> PartitionedScenes) {
			return PartitionedScenes.Where(lines =>
				lines.Select(l => ParsingTools.ParseLink(l))
					.Where(link => link != null)
					.Where(link => link.Link.Equals("TreeTag"))
					.Count() > 0
				);
		}
		/// <summary>
		/// Depricated
		/// </summary>
		/// <param name="source"></param>
		/// <param name="novel"></param>
		public static void ParseLitSourceInfo(this MDSourceFile source, LitNovel novel) {
			var litSourceInfo = new LitSourceInfo();
			var query = source.Lines.Select(s => ParsingTools.ParseLink(s))
				.Where(l => l != null && l.Link.Equals("Author"));
			if (query.Count() > 0) {
				litSourceInfo.Author = query.First().Tag;
			}
			//source.LitSourceInfo = novel.AddSourceInfoDistinct(litSourceInfo);
		}
	}
}
