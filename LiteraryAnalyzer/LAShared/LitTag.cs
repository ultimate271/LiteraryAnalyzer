using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitTag : IEqualityComparer<LitTag>{
		public String Tag { get; set; } = "";
		public LitTag() { }
		public LitTag(String Tag) {
			this.Tag = Tag;
		}

		public String ToHyperlink() {
			return String.Format("{{{0}}}", this.Tag);
		}

		public bool Equals(LitTag x, LitTag y) {
			return x.Tag.Equals(y.Tag);
		}

		public int GetHashCode(LitTag obj) {
			return obj.Tag.GetHashCode();
		}
	}
}
