using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDLinkLine {
		public String Link { get; set; }
		public String Tag { get; set; }
		public override string ToString() {
			return String.Format(@"[{0}]: # {{{1}}}", this.Link, this.Tag);
		}
	}
	public static partial class ParsingTools {
		public static MDLinkLine ParseLink(String s) {
			var retVal = new MDLinkLine();
			var match = System.Text.RegularExpressions.Regex.Match(s, @"^\[([^\]]*)\]: # {([^}]*)}$");
			if (!match.Success) {
				return null;
			}
			else {
				try {
					retVal.Link = match.Groups[1].Value;
					retVal.Tag = match.Groups[2].Value;
				}
				catch {
					return null;
				}
			}
			return retVal;
		}
	}
}
