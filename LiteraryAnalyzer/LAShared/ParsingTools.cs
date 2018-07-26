using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public static partial class ParsingTools {
		public static readonly string[] GenereratedLinks = { "Metadata", "TreeTag" };
		/// <summary>
		/// Breaks up a list of lines into a list of list of lines, where each sublist starts with a single header hash
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static List<List<String>> PartitionLines(IEnumerable<String> lines) {
			var PartitionedScenes = new List<List<String>>();
			var currentSceneLines = new List<String>();
			foreach (var line in lines) {
				currentSceneLines.Add(line);
				if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^#[^#]")) {
					PartitionedScenes.Add(currentSceneLines);
					currentSceneLines = new List<String>();
				}
			}
			return PartitionedScenes;
		}
		public static List<String> TagLines (IEnumerable<String> lines, String tag) {
			var retVal = new List<String>();
			int i = 0;
			//First remove the existing tags
			var query = lines.Where(s => {
				var linkLine = ParsingTools.ParseLink(s);
				return linkLine == null || !ParsingTools.GenereratedLinks.Contains(linkLine.Link);
			} );
			foreach (var line in query) {
				retVal.Add(line);
				if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^#[^#]")) {
					if (i == 0) {
						retVal.Add(String.Format(@"[Metadata]: # {{{0}}}", tag));
					}
					else {
						retVal.Add(String.Format(@"[TreeTag]: # {{{0}.{1:00}}}", tag, i));
					}
					i++;
				}
			}
			return retVal;
		}
	}
}
