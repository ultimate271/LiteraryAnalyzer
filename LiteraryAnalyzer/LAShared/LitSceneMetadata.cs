using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitSceneMetadata {
		public String Descriptor { get; set; } = "";
		public String Header { get; set; } = "";
	}
	public static partial class ParsingTools {
		public static LitSceneMetadata ParseMetadataDefault(this LitOptions LO, LitNovel novel, IEnumerable<String> sourceLines) {
			var retVal = new LitSceneMetadata();
			var links = sourceLines.Select(l => LO.ParseLink(l)).Where(link => link != null);
			retVal.Descriptor = links.Where(link => link.Link.Equals("Descriptor")).Select(link => link.Tag).FirstOrDefault();

			var pattern = @"^# (.*)$";
			var match = System.Text.RegularExpressions.Regex.Match(sourceLines.First(), pattern);
			retVal.Header = match.Groups[1].Value;

			return novel.AddMetadataDistinct(retVal);
		}
		public static IEnumerable<String> ExtractMetadataDefault (this LitOptions LO, IEnumerable<IEnumerable<String>> PartitionedScenes){
			return PartitionedScenes.Where(lines => 
				lines.Select(l => LO.ParseLink(l))
					.Where(link => link != null)
					.Where(link => link.Link.Equals("Metadata"))
					.Count() > 0
				).FirstOrDefault();
		}
		public static List<String> WriteMetadataDefault(this LitOptions LO, LitSceneMetadata metadata, LitAuthor sourceinfo) {
			var retVal = new List<String>();
			retVal.Add(String.Format("# {0}", metadata.Header));
			retVal.Add(MakeLinkLine("Metadata", metadata.Descriptor));
			retVal.Add(MakeLinkLine("Descriptor", metadata.Descriptor));
			retVal.Add(sourceinfo.ToSourceLine());
			return retVal;
		}
	}
}
