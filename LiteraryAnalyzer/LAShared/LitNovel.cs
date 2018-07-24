using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitNovel {
		public List<LitScene> Scenes { get; set; }
		public String Title { get; set; }
	}
	public static partial class LitExtensions {
		public static LitAnnSource GenerateMarkdown(this LitNovel novel) {
			throw new NotImplementedException();
		}
	}
}
