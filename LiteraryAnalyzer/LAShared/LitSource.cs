﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents what the author actually wrote
	/// </summary>
	public class LitSource {
		/// <summary>
		/// This is a dictionary to allow for concurrent source
		/// </summary>
		public Dictionary<LitAuthor, String> Text { get; set; } = new Dictionary<LitAuthor, string>();
		public String this[LitAuthor author] {
			get {
				return this.Text[author] != null ? Text[author] : "";
			}
			set {
				this.Text[author] = value;
			}
		}
	}
	public static partial class ParsingTools {
	}
	public static partial class LitExtensions {
		//public static LitSource ParseSource(this String s) {
		//	if (String.IsNullOrWhiteSpace(s)) {
		//		return null;
		//	}
		//	return new LitSource { Text = s };
		//}
		//public static IEnumerable<LitFootnote> ExtractFootnotes(this LitSource source) {
		//	List<LitFootnote> retVal = new List<LitFootnote>();
		//	var matches = Regex.Matches(source.Text, @"\[([^\[\]]*)\]");
		//	LitFootnote temp;
		//	int index;
		//	String text;
		//	foreach (Match m in matches) {
		//		index = source.Text.IndexOf(m.Groups[0].Value);
		//		text = m.Groups[1].Value.Trim();
		//		temp = new LitFootnote { Source = source, Index = index, Text = text };
		//		retVal.Add(temp);
		//	}
		//	return retVal;
		//}
	}
}
