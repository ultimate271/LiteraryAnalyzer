using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// This class is a wrapper around the abstract Litelm class, used to represent an authors source material. This material will be strictly
	/// something that has been written by the author, and not by me or any other annotator. Footnotes, headers, and threads all get parsed seperately
	/// </summary>
	public class LitSource : Litelm{
	}
	public static partial class LitExtensions {
		public static LitSource ParseSource(this String s) {
			if (String.IsNullOrWhiteSpace(s)) {
				return null;
			}
			return new LitSource { Text = s };
		}
		public static IEnumerable<LitFootnote> ExtractFootnotes(this LitSource source) {
			List<LitFootnote> retVal = new List<LitFootnote>();
			var matches = Regex.Matches(source.Text, @"\[([^\[\]]*)\]");
			LitFootnote temp;
			int index;
			String text;
			foreach (Match m in matches) {
				index = source.Text.IndexOf(m.Groups[0].Value);
				text = m.Groups[1].Value.Trim();
				temp = new LitFootnote { Source = source, Index = index, Text = text };
				retVal.Add(temp);
			}
			return retVal;
		}
	}
}
