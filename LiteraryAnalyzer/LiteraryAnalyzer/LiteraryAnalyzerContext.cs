using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	public class LiteraryAnalyzerContext : DbContext {
		public DbSet<Excerpt> Excerpts { get; set; }
	}
}
