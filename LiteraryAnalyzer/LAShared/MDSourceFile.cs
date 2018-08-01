using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDSourceFile : MDFile {
		public String Descriptor { get; set; }
		public LitSourceInfo LitSourceInfo { get; set; } = new LitSourceInfo();
	}
	public static partial class ParsingTools {
		public static void TagLines(this MDSourceFile sourcefile) {
			sourcefile.Lines = new List<string>(ParsingTools.TagLines(sourcefile.Lines, sourcefile.Descriptor));
		}
		public static void ParseLitSourceInfo(this MDSourceFile source, LitNovel novel) {
			var litSourceInfo = new LitSourceInfo();
			var query = source.Lines.Select(s => ParsingTools.ParseLink(s))
				.Where(l => l != null && l.Link.Equals("Author"));
			if (query.Count() > 0) {
				litSourceInfo.Author = query.First().Tag;
			}
			source.LitSourceInfo = novel.AddSourceInfoDistinct(litSourceInfo);
		}
	}
}
