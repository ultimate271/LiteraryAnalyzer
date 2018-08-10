using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDSourceFile : MDFile {
		public String Descriptor { get; set; } = "";
		public String Author { get; set; } = "";
	}
	public static partial class ParsingTools {
		public static string ToLongFilename(this MDSourceFile source, LitAnnSourceInfo info) {
			return String.Format("{0}\\{1}", info.BaseDir, source.ToShortFilename(info));
		}
		public static string ToShortFilename(this MDSourceFile source, LitAnnSourceInfo info) {
			return ToShortFilename(info.Prefix, source.Descriptor, source.Author);
		}
		public static string ToShortFilename (LitAnnSourceInfo info, LitSourceInfo author, LitSceneMetadata metadata) {
			return ToShortFilename(info.Prefix, metadata.Descriptor, author.Author);
		}
		public static string ToShortFilename (String Prefix,  String descriptor,String author) {
			return String.Format("{0}{1}.{2}.md", Prefix, descriptor, author);
		}
		public static void TagLines(this MDSourceFile sourcefile) {
			sourcefile.Lines = new List<string>(ParsingTools.TagLines(sourcefile.Lines, sourcefile.Descriptor, sourcefile.Author));
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
