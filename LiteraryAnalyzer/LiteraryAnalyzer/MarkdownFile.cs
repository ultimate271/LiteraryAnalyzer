using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	public class MarkdownFile {
		public String GeneratedURI { get { return String.Format("{0}\\{1}{2:D2}{3}.md", BaseDir, Prefix, Count, Filename); } }
		public String FullURI { get { return BaseDir + "\\" + Filename; } }
		public String Filename { get; set; }
		public String Markdown { get; set; }
		public String Prefix { get; set; }
		public String BaseDir { get; set; }
		public int Count { get; set; }

		private void FetchMarkdown() {
			this.Markdown = System.IO.File.ReadAllText(this.FullURI);
		}
		

		public IEnumerable<String> ParseMarkdownForContents() {
			return Markdown
				.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Where(line => line.StartsWith("#"));
		}
		public IEnumerable<MarkdownFile> ParseMarkdown (IEnumerable<String> contents) {
			try {
				this.FetchMarkdown();
			}
			catch {
				return null;
			}
			if (contents == null) {
				contents = this.ParseMarkdownForContents();
			}
			var retVal = new List<MarkdownFile>();
			String prev = null;
			foreach(String s in contents) {
				if (!String.IsNullOrEmpty(prev)) {
					int fromIndex = this.Markdown.IndexOf("\r\n" + prev + "\r\n") + 2;
					int toIndex = this.Markdown.IndexOf("\r\n" + s + "\r\n") + 2;
					var tmp = new MarkdownFile {
						Filename = prev.Trim('#', ' '),
						Markdown = this.Markdown.Substring(fromIndex, toIndex - fromIndex),
						BaseDir = this.BaseDir,
						Prefix = this.Prefix,
						Count = retVal.Count + 1
					};
					retVal.Add(tmp);
				}
				prev = s;
			}
			return retVal;
		}
		public void PrintFile() {
			System.IO.File.WriteAllText(this.GeneratedURI, this.Markdown);
		}
	}
}
