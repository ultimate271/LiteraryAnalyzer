using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitPlace : LitRef {
		public LitPlace() : base() { }
		public LitPlace(String tag) : base(tag) { }
		public LitPlace(LitTag tag) : base(tag) { }
	}
}
