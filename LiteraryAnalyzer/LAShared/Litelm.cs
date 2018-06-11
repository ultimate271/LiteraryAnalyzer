using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	public abstract class Litelm {
		public int LitelmID { get; set; }
		public string Text { get; set; }
	}
}
