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
}
