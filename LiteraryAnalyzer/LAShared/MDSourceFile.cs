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
		public static String ToLongFilenameDefault(this LitOptions LO, MDAnnSourceInfo info, MDSourceFile source ) {
			return String.Format("{0}\\{1}", info.BaseDir, ToShortFilenameDefault(LO, info, source));
		}
		public static String ToShortFilenameDefault(this LitOptions LO, MDAnnSourceInfo info, MDSourceFile source) {
			return ToShortFilenameDefault(LO, info.Prefix, source.Descriptor, source.Author);
		}
		public static String ToShortFilenameDefault (this LitOptions LO, MDAnnSourceInfo info, LitAuthor author, LitSceneMetadata metadata) {
			return ToShortFilenameDefault(LO, info.Prefix, metadata.Descriptor, author.Author);
		}
		public static String ToShortFilenameDefault (this LitOptions LO, String Prefix, String descriptor,String author) {
			return String.Format("{0}{1}.{2}.md", Prefix, descriptor, author);
		}
		public static void ParseSourceFileDefault(this LitOptions LO, LitNovel novel, MDSourceFile sourceFile) {
			var PartitionedScenes = LO.ExtractFromSourceFile(sourceFile);

			//Extract and add the metadata
			var MetadataLines = LO.ExtractMetadata(PartitionedScenes);
			var LitSceneMetadata = LO.ParseMetadata(novel, MetadataLines);
			var Author = LO.ParseAuthor(novel, MetadataLines);

			//Extract and add the scenes
			var PartitionedSceneLines = LO.ExtractScenes(PartitionedScenes);
			foreach (var Scenelines in PartitionedSceneLines) {
				var scene = LO.ParseToScene(novel, LitSceneMetadata, Author, Scenelines);
				novel.AddScene(scene);
			}
		}
		public static IEnumerable<IEnumerable<String>> ExtractFromSourceFileDefault(this LitOptions LO, MDSourceFile sourcefile) {
			return ParsingTools.PartitionLines(
				sourcefile.Lines, 
				line => System.Text.RegularExpressions.Regex.IsMatch(line, @"^#[^#]")
			);
		}
		public static void TagSourceFileDefault(this LitOptions LO, MDSourceFile sourcefile) {
			sourcefile.Lines = new List<string>(TagSourceFileDefault(LO, sourcefile.Lines, sourcefile.Descriptor, sourcefile.Author));
		}
		public static List<String> TagSourceFileDefault(this LitOptions LO, IEnumerable<String> lines, String tag, String author) {
			return TagSourceFileDefault(LO, lines, tag, author, 1);
		}
		public static List<String> TagSourceFileDefault (this LitOptions LO, IEnumerable<String> lines, String tag, String author, int headerLevel) {
			var retVal = new List<String>();
			var arg = new List<String>();
			//First remove the existing tags
			var query = lines.Where(s => {
				var linkLine = LO.ParseLink(s);
				return linkLine == null || !ParsingTools.GenereratedLinks.Contains(linkLine.Link);
			} );
			int i = headerLevel == 1 ? -1 : 0;
			bool adding = true;
			foreach (var line in query) {
				var pattern = @"^(#+)[^#]";
				var match = System.Text.RegularExpressions.Regex.Match(line, pattern);
				if (match.Success) {
					int lineHeaderLevel = match.Groups[1].Length;
					if (i < 0) { //i should only ever equal 0 if this is the metadata scene, and it's the first scene of the file
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[Metadata]: # {{{0}}}", tag));
						retVal.Add(String.Format(@"[Descriptor]: # {{{0}}}", tag));
						retVal.Add(String.Format(@"[Author]: # {{{0}}}", author));
					}
					else if (lineHeaderLevel == headerLevel && adding) {
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[TreeTag]: # {{{0}.{1:00}}}", tag, i));
					}
					else if (lineHeaderLevel > headerLevel) {
						adding = false;
						arg.Add(line);
					}
					else if (lineHeaderLevel == headerLevel && !adding) {
						//Recursively call the lines we've gathered together, tag them, and add the range
						retVal.AddRange(TagSourceFileDefault(LO, arg, String.Format(@"{0}.{1:00}", tag, i), author, headerLevel + 1));

						//Begin anew
						arg = new List<string>();
						adding = true;

						//Start with this header line
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[TreeTag]: # {{{0}.{1:00}}}", tag, i));
					}
				}
				else { //If this is not a header line, and just a regular line
					if (adding) {
						retVal.Add(line);
					}
					else {
						arg.Add(line);
					}
				}
			}
			if (arg.Count > 0) {
				retVal.AddRange(TagSourceFileDefault(LO, arg, String.Format(@"{0}.{1:00}", tag, i), author, headerLevel + 1));
			}
			return retVal;
		}
		/// <summary>
		/// Depricated
		/// </summary>
		/// <param name="source"></param>
		/// <param name="novel"></param>
		//public static void ParseLitSourceInfo(this MDSourceFile source, LitNovel novel) {
		//	var litSourceInfo = new LitSourceInfo();
		//	var query = source.Lines.Select(s => ParsingTools.ParseLink(s))
		//		.Where(l => l != null && l.Link.Equals("Author"));
		//	if (query.Count() > 0) {
		//		litSourceInfo.Author = query.First().Tag;
		//	}
		//	//source.LitSourceInfo = novel.AddSourceInfoDistinct(litSourceInfo);
		//}
	}
}
