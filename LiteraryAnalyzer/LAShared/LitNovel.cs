using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitNovel : Litelm {
		public LitAnnSource LitAnnSource { get; set; }
		public List<LitScene> Scenes { get; set; }
		public String Title { get; set; }
	}
}
