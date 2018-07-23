using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LiteraryAnalyzer.LAShared {
	public abstract class LitRef {
		public List<LitTag> Tags { get; set; }
		public String Commentary { get; set; }
	}
}
