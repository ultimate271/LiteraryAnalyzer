using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitSceneMetadata {
		public String Descriptor { get; set; } = "";
		public String Header { get; set; } = "";
	}
	public static partial class ParsingTools {
		public static LitSceneMetadata ParseMetadata(this LitNovel novel, MDSourceFile sourceFile) {
			//TODO There are a few things here that need to be done.
			//First, I think that the scenes 
		}
	}
}
