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

		public static MDHeader ParseHeaderOther(this LitOptions LO, String line) {

			return new MDHeader() { HeaderLevel = 12, Text = LO.SourceLinesToString(new String[] { line }) };
		}
	}
}
