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
		public static List<String> ToSourceLines(this LitSceneMetadata metadata, LitAuthor sourceinfo) {
			var retVal = new List<String>();
			retVal.Add(String.Format("# {0}", metadata.Header));
			retVal.Add(MakeLinkLine("Metadata", metadata.Descriptor));
			retVal.Add(MakeLinkLine("Descriptor", metadata.Descriptor));
			retVal.Add(sourceinfo.ToSourceLine());
			return retVal;
		}
	}
}
