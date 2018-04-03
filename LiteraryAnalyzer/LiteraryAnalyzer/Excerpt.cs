using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer {
	public class Excerpt {
		public int ExcerptID { get; set; }
		public string ExcerptText { get; set; }

		public virtual List<Excerpt> Children { get; set; }
	}
	public class LiteraryAnalyzerContext : DbContext {
		public DbSet<Excerpt> Excerpts { get; set; }
	}
}
