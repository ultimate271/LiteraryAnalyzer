﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public static partial class ParsingTools {
		public static readonly string[] GenereratedLinks = { "Metadata", "TreeTag", "Descriptor" };
		/// <summary>
		/// Breaks up a list of lines into a list of list of lines, where each sublist starts with a single header hash
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static List<List<String>> PartitionLines(IEnumerable<String> lines, Func<String, bool> Breaker) {
			var PartitionedLines = new List<List<String>>();
			var currentLines = new List<String>();
			foreach (var line in lines) {
				if (currentLines.Count > 0 && Breaker(line)) {
					PartitionedLines.Add(currentLines);
					currentLines = new List<String>();
				}
				currentLines.Add(line);
			}
			if (currentLines.Count > 0) {
				PartitionedLines.Add(currentLines);
			}
			return PartitionedLines;
		}
		public static List<String> TagLines(IEnumerable<String> lines, String tag) {
			return TagLines(lines, tag, 1);
		}
		public static List<String> TagLines (IEnumerable<String> lines, String tag, int headerLevel) {
			var retVal = new List<String>();
			var arg = new List<String>();
			//First remove the existing tags
			var query = lines.Where(s => {
				var linkLine = ParsingTools.ParseLink(s);
				return linkLine == null || !ParsingTools.GenereratedLinks.Contains(linkLine.Link);
			} );
			int i = headerLevel == 1 ? -1 : 0;
			bool adding = true;
			foreach (var line in query) {
				var pattern = @"^(#+)[^#]";
				var match = System.Text.RegularExpressions.Regex.Match(line, pattern);
				if (match.Success) {
					int lineHeaderLevel = match.Groups[1].Length;
					if (i < 0) { //i should only ever equal 0 if this is the metadata scene, and it's the first scene of the file
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[Metadata]: # {{{0}}}", tag));
						retVal.Add(String.Format(@"[Descriptor]: # {{{0}}}", tag));
					}
					else if (lineHeaderLevel == headerLevel && adding) {
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[TreeTag]: # {{{0}.{1:00}}}", tag, i));
					}
					else if (lineHeaderLevel > headerLevel) {
						adding = false;
						arg.Add(line);
					}
					else if (lineHeaderLevel == headerLevel && !adding) {
						//Recursively call the lines we've gathered together, tag them, and add the range
						retVal.AddRange(TagLines(arg, String.Format(@"{0}.{1:00}", tag, i), headerLevel + 1));

						//Begin anew
						arg = new List<string>();
						adding = true;

						//Start with this header line
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[TreeTag]: # {{{0}.{1:00}}}", tag, i));
					}
				}
				else { //If this is not a header line, and just a regular line
					if (adding) {
						retVal.Add(line);
					}
					else {
						arg.Add(line);
					}
				}
			}
			if (arg.Count > 0) {
				retVal.AddRange(TagLines(arg, String.Format(@"{0}.{1:00}", tag, i), headerLevel + 1));
			}
			return retVal;
		}
		/// <summary>
		/// Takes all of the source lines of a list of arbitrary strings and turns them into a string
		/// </summary>
		public static String SourceLinesToString(IEnumerable<string> lines) {
			var paragraphs = ParsingTools.PartitionLines(lines.Where(s => IsSourceLine(s)), line => String.IsNullOrWhiteSpace(line));
			StringBuilder sb = new StringBuilder();
			foreach (var paralist in paragraphs) {
				foreach (var paraline in paralist.Where(l => !String.IsNullOrEmpty(l))) {
					sb.Append(paraline.TrimEnd('\r', '\n'));
					sb.Append(" ");
				}
				sb.Append("\r\n");
			}
			return sb.ToString();
		}
	}
}
