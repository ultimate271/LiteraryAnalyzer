using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDTag {
		public String TagName { get; set; } = "";
		public String TagFile { get; set; } = "";
		public String TagLine { get; set; } = "";

		public static String TagLineToRegex(String TagLine) {
			return String.Format(@"/\v^{0}$", TagLine);
		}
		public override string ToString() {
			return String.Format("{0}\t{1}\t{2}", this.TagName, this.TagFile, TagLineToRegex(this.TagLine));
		}
	}
	public static partial class ParsingTools {
		public static MDTag ToMDTag(this LitTag tag) {
			return new MDTag() { TagName = tag.Tag };
		}
	}
}
