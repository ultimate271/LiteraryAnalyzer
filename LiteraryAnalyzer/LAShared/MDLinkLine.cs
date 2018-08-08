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
		/// <summary>
		/// Will look through all of the generic list types of the element,
		/// and based upon the name of the link, return a new LitRef with
		/// the appropiate type, with the correct tag, so that it can be added
		/// to the element.
		/// It does not add the reference to the element, it merely creates the correct
		/// kind of reference
		/// </summary>
		/// <param name="link"></param>
		/// <param name="elm"></param>
		/// <returns></returns>
		public static LitRef LinkToRef(this MDLinkLine link, LitNovel novel, LitElm elm) {
			throw new NotImplementedException();

		}

		public static String MakeLinkLine(String link, String tag) {
			return new MDLinkLine() { Link = link, Tag = tag }.ToString();
		}
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
