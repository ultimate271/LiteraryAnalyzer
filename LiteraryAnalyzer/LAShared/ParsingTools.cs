using System;
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
	}
}
