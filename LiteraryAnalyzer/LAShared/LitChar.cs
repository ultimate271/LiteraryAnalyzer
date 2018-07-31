using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitChar : LitRef {
		public LitChar() : base() { }
		public LitChar(String tag) : base(tag) { }
		public LitChar(LitTag tag) : base(tag) { }
	}
	public static partial class ParsingTools {
		/// <summary>
		/// To save on processing, this takes a set of partitioned lines and parses out the character from them
		/// </summary>
		/// <param name="retVal"></param>
		/// <param name="lines"></param>
		public static void ParseLitChar(this LitChar retVal, IEnumerable<IEnumerable<String>> lines) {

		}
	}
}
