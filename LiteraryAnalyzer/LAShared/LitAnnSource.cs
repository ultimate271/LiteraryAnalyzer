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
	}
	public static partial class LitExtensions {
		public static LitNovel ParseAnnSource(this LitAnnSource source) {
			var retVal = new LitNovel();

			//Aggregate the source
			var files = System.IO.Directory.GetFiles(source.BaseDir, source.Prefix + "*.md");
			Array.Sort(files);
			var query = files.Where(s => !s.Contains("notes.md"));
			var sb = new StringBuilder();
			foreach (var file in query) {
				sb.Append(System.IO.File.ReadAllText(file));
			}
			string sourceText = sb.ToString();

			//Some decisions
			//The very first # does not represent a scene, but instead represents metadata.
			//The user can define a tag manually. This is done by adding a [Tag Element] tag under the header

			return retVal;
		}
	}
}
