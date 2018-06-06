using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class ExcerptHeader : Excerpt{

		public virtual List<Excerpt> Children { get; set; } = new List<Excerpt>();
	}
}
