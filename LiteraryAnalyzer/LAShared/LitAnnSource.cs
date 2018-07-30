using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents the set of source files that I have annotated
	/// </summary>
	public class LitAnnSource {
		public string BaseDir { get; set; }
		public string Prefix { get; set; }
		public LitSourceInfo LitSourceInfo { get; set; } = new LitSourceInfo() { Author = "Original" };
	}
	public static partial class LitExtensions {
	}
}
