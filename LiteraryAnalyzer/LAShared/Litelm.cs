using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	public abstract class LitElm {
		public int LitelmID { get; set; }
		public string Header { get; set; }
		public List<LitTag> UserTags { get; set; }
		public LitTag TreeTag { get; set; }
		public List<LitEvent> Children { get; set; }
	}
}
