using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitRef {
		public LitRef() { }
		public LitRef(String tag) : this(new LitTag(tag)) { }
		public LitRef(LitTag tag) { Tags.Add(tag); }
		public List<LitTag> Tags { get; set; } = new List<LitTag>();
		public String Commentary { get; set; } = "";
	}
	public static partial class LitExtensions {
		public static bool IsReferenceIntersection(this LitRef ref1, LitRef ref2) {
			return ref1.Tags.Intersect(ref2.Tags, new LitTag()).Count() > 0;
		}
		public static void CombineRef(this LitRef ref1, LitRef ref2) {
			ref1.Tags.AddRange(ref2.Tags.Except(ref1.Tags, new LitTag()));
			if (String.IsNullOrWhiteSpace(ref1.Commentary)) {
				ref1.Commentary = ref2.Commentary;
			}
		}
		public static void AddTag(this LitRef litRef, LitTag tag) {
			if (!litRef.Tags.Contains(tag, new LitTag())) {
				litRef.Tags.Add(tag);
			}
		}
	}
	public static partial class ParsingTools {
		public static LitRef ParseLitRef(IEnumerable<String> lines) {
			if (lines.Count() == 0) { return null; }
			var PartitionedLines = ParsingTools.PartitionLines(lines, l => System.Text.RegularExpressions.Regex.IsMatch(l, @"^##[^#]"));
			var link = PartitionedLines.First().Select(s => ParseLink(s)).Where(l => l != null).First();

			var retVal = new LitRef();
			//Do the specific things for this style of reference
			if (link.Link.Equals("Reference")) {
				if (link.Tag.Equals("Character")) {
					retVal = new LitChar();
					(retVal as LitChar).ParseLitChar(PartitionedLines);
				}
			}

			//Get the first tag of the reference
			string pattern = @"^# (.+)";
			var match = System.Text.RegularExpressions.Regex.Match(lines.First(), pattern);
			retVal.Tags.Add(new LitTag(match.Groups[1].Value));

			//Save the commentary
			retVal.Commentary = SourceLinesToString(PartitionedLines.First());

			//Save the tags
			pattern = "^## Tags$";
			var tagsList = PartitionedLines.Where(list => System.Text.RegularExpressions.Regex.IsMatch(list.First(), pattern)).First();
			foreach (var tagline in tagsList.Where(s => IsSourceLine(s))) {
				retVal.AddTag(new LitTag(tagline));
			}

			return retVal;
		}
	}
}
