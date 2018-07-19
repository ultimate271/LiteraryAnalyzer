using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MarkdownOption {
		public int MarkdownOptionID { get; set; }
		public MarkdownOption.URIOptions URIOption { get; set; } = MarkdownOption.URIOptions.Default;
		public MarkdownOption.ContentsOptions ContentsOption { get; set; } = MarkdownOption.ContentsOptions.Default;
		public MarkdownOption.ParserOptions ParserOption { get; set; } = MarkdownOption.ParserOptions.Default;
		public MarkdownOption.ExcerptOptions ExcerptOption { get; set; } = MarkdownOption.ExcerptOptions.Default;
		public String BaseDir { get; set; } = ""; //This is the root of the directory, and should be the same directory the .git file is in
		public String Filename { get; set; } = ""; //This should include a string to indicate which file this is
		public String Prefix { get; set; } = "";//This is the directory that the parsed markdown will go into
		public enum URIOptions {
			Default,
			Standard,
			Novel,
			Full,
			ShortStory,
			Chapter
		}
		public enum ContentsOptions {
			Default,
			Novel,
			Markdown
		}
		public enum ParserOptions {
			Default,
			Novel,
		}
		public enum ExcerptOptions {
			Default,
			Markdown
		}
	}
}
