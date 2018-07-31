using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDSourceFile : MDFile {
		public String Descriptor { get; set; }
	}
	public static partial class ParsingTools {
		public static void TagLines(this MDSourceFile sourcefile) {
			ParsingTools.TagLines(sourcefile.Lines, sourcefile.Descriptor);
		}
	}
}
