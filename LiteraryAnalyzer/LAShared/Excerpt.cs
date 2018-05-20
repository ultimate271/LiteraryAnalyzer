using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	public class Excerpt {
		public int ExcerptID { get; set; }
		public string ExcerptText { get; set; }
		public String TokenID { get; set; }

		public virtual List<Excerpt> Children { get; set; } = new List<Excerpt>();
		public virtual Token Token { get; set; }
	}
}
