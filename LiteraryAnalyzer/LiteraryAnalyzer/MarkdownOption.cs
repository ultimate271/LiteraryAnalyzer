using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	public class MarkdownOption {
		public int MarkdownOptionID { get; set; }
		public MarkdownFile.URIOptions URIOption { get; set; } = MarkdownFile.URIOptions.Default;
		public MarkdownFile.ContentsOptions ContentsOption { get; set; } = MarkdownFile.ContentsOptions.Default;
		public MarkdownFile.ParserOptions ParserOption { get; set; } = MarkdownFile.ParserOptions.Default;
		public String BaseDir { get; set; } = ""; //This is the root of the directory, and should be the same directory the .git file is in
		public String Filename { get; set; } = ""; //This should include a string to indicate which file this is
		public String Prefix { get; set; } = "";//This is the directory that the parsed markdown will go into
	}
}
