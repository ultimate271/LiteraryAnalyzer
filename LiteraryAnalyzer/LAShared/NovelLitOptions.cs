using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public static partial class ParsingTools {
		public static IEnumerable<String> BuildSourceFilenamesNovel(
			this LitOptions LO,
			MDAnnSourceInfo info
		) {
			return new String[] { String.Format("{0}\\{1}", info.BaseDir, info.Prefix) };
		}
		public static List<MDSourceFile> BuildSourceFilesNovel(
			this LitOptions LO,
			MDAnnSourceInfo info,
			IEnumerable<String> filenames
		) {
			return new List<MDSourceFile>(new MDSourceFile[] {
				new MDSourceFile() {
					Lines = new List<String>(
						System.IO.File.ReadAllLines(filenames.First())
					)
				}
			});
		}
	}
}
