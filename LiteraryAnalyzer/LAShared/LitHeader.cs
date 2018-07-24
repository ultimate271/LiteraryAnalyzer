using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// The ID of this class is irrelevent, but the text will represent the text given after the space after the last hash sign at the beginning of the line
	/// Children will of course be parsed recursively
	/// A note about header levels, there is no such thing. The only thing header level is used for is to determine if a header is above or below the current header in the hiarchy, nothing more
	/// So a header level like 2324 would translate to 1212 if you understand what I am meaning
	/// </summary>
	public class LitHeader : LitElm{
		public List<LitElm> Children { get; set; } = new List<LitElm>();
	}
	public static partial class LitExtensions {
		public class ParseHeaderRetVal {
			public LitHeader Parent { get; set; } = null;
			public List<LitFootnote> Footnotes { get; set; } = new List<LitFootnote>();
		}
		//public static ParseHeaderRetVal ParseHeaderToModel(this LitHeader parent, String s) {
		//	if (String.IsNullOrEmpty(s)) {
		//		return null;
		//	}
		//	if (parent == null) {
		//		return null;
		//	}
		//	Dictionary<string, string> footers = new Dictionary<string, string>();
		//	string pattern = @"\[([^\[\]:]*):([^\[\]]*)\]";
		//	var matches = Regex.Matches(s, pattern);
		//	foreach (Match m in matches) {
		//		try {
		//			footers.Add(m.Groups[1].Value.Trim(), m.Groups[2].Value.Trim());
		//		}
		//		catch (ArgumentException) {
		//		}
		//	}
		//	s = Regex.Replace(s, pattern, "");

		//	return parent.ParseHeaderToModel(s, footers);
		//}
		///// <summary>
		///// Parses the markdown in s and places the results as children of parent
		///// </summary>
		///// <param name="parent"></param>
		///// <param name="s"></param>
		//public static ParseHeaderRetVal ParseHeaderToModel(this LitHeader parent, String s, Dictionary<string, string> footers) {
		//	if (String.IsNullOrEmpty(s)) {
		//		return null;
		//	}
		//	if (parent == null) {
		//		return null;
		//	}

		//	var retVal = new ParseHeaderRetVal();
		//	var temp = new List<string>();
		//	var subStrings = new List<string>();
		//	int currentLevel = 0;
		//	bool subHeader = false;

		//	//Split the string into lines
		//	var lines = s
		//		.Split(new String[] { "\r\n" }, StringSplitOptions.None)
		//		.Select(line => new {
		//			Line = line,
		//			HeaderLevel = Helper.HeaderLevel(line),
		//			IsLitThread = line.StartsWith("_")
		//		});

		//	//Determine the partitions (substrings) of the current string
		//	foreach (var line in lines) {
		//		if (line.HeaderLevel > 0 && line.HeaderLevel <= currentLevel && subHeader) {
		//			subStrings.Add(String.Join("\r\n", temp));
		//			temp = new List<string>();
		//			temp.Add(line.Line);
		//			currentLevel = line.HeaderLevel;
		//		}
		//		else if (line.HeaderLevel > 0 && line.HeaderLevel > currentLevel && !subHeader) {
		//			subStrings.Add(String.Join("\r\n", temp));
		//			temp = new List<string>();
		//			temp.Add(line.Line);
		//			currentLevel = line.HeaderLevel;
		//			subHeader = true;
		//		}
		//		else if (line.HeaderLevel == 0 || line.HeaderLevel > currentLevel) {
		//			if (!line.IsLitThread) {
		//				temp.Add(line.Line);
		//			}
		//		}
		//	}
		//	subStrings.Add(String.Join("\r\n", temp));

		//	//Create new objects for each substring and add them to the children of the parent node
		//	subHeader = false;
		//	LitElm child;
		//	List<LitFootnote> footnotes = new List<LitFootnote>();
		//	foreach (String subString in subStrings) {
		//		child = null;
		//		if (!subHeader) {
		//			child = subString.ParseSource();
		//			subHeader = true;

		//			//Footnote stuff
		//			if (child != null) {
		//				var footerRefs = (child as LitSource).ExtractFootnotes();
		//				foreach (LitFootnote footnote in footerRefs) {
		//					try {
		//						footnote.Text = footers[footnote.Text];
		//						footnotes.Add(footnote);
		//					}
		//					catch (KeyNotFoundException) {
		//					}
		//				}
		//			}
		//		}
		//		else {
		//			var query = subString.Split(new String[] { "\r\n" }, 0);
		//			if (query.Length > 0) {
		//				child = new LitHeader { Text = query.First().Trim('#', ' ') };
		//				var ret = (child as LitHeader).ParseHeaderToModel(String.Join("\r\n", query.Skip(1)), footers);
		//				footnotes.AddRange(ret.Footnotes);
		//			}
		//		}
		//		if (child != null) {
		//			parent.Children.Add(child);
		//		}
		//	}
		//	return new ParseHeaderRetVal() { Parent = parent, Footnotes = footnotes };
		//}
	}
}
