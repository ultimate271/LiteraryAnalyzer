using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents the information about where a LitSource came from
	/// </summary>
	public class LitSourceInfo {
		public String Author { get; set; } = "Original";
	}
	public static partial class LitExtensions {
		public static bool IsSourceInfoIntersection(this LitSourceInfo info1, LitSourceInfo info2) {
			return info1.Author.Equals(info2.Author);
		}
	}
}
