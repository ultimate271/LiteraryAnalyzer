using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitRef {
		public LitRef() { }
		public LitRef(String tag) : this(new LitTag(tag)) { }
		public LitRef(LitTag tag) { Tags.Add(tag); }
		public List<LitTag> Tags { get; set; } = new List<LitTag>();
		public String Commentary { get; set; } = "";
	}
	public static partial class LitExtensions {
		public static bool IsReferenceIntersection(this LitRef ref1, LitRef ref2) {
			return ref1.Tags.Intersect(ref2.Tags, new LitTag()).Count() > 0;
		}
		public static void CombineRef(this LitRef ref1, LitRef ref2) {
			ref1.Tags.AddRange(ref2.Tags.Except(ref1.Tags, new LitTag()));
			if (String.IsNullOrWhiteSpace(ref1.Commentary)) {
				ref1.Commentary = ref2.Commentary;
			}
		}
	}
}
