using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDHeader {
		public int HeaderLevel { get; set; }
		public string Text { get; set; }
		public override string ToString() {
			return String.Format(@"{0} {1}", new String('#', this.HeaderLevel), this.Text);
		}
	}
	public static partial class ParsingTools {
		/// <summary>
		/// Default implementaiton for parsing a line into a MDHeader object.
		/// </summary>
		/// <param name="line"></param>
		/// <returns>The MDHeader object, or null if the parse failed</returns>
		public static MDHeader ParseHeaderDefault(
			this LitOptions LO, 
			String line
		){
			var retVal = new MDHeader();
			var match = System.Text.RegularExpressions.Regex.Match(line, @"^(#+)([^#].*)$");
			if (!match.Success) {
				return null;
			}
			else {
				try {
					retVal.HeaderLevel = match.Groups[1].Value.Length;
					retVal.Text = match.Groups[2].Value.Trim();
				}
				catch {
					return null;
				}
			}
			return retVal;
		}
	}
}
