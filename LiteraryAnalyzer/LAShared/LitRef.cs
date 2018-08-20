using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Base class for References throughout the novel
	/// <remarks>
	/// For my reference, here is the sort of heirachy for links and references
	/// LitChar
	///		Actor (Scene level)
	///		Speaker (Event level)
	///		Character (Character reference)
	///	LitPlace
	///		Location (Scene level)
	///		Place (Location Reference)
	///	LitMyth
	///		Event (Event level)
	///		Myth (Myth Reference)
	/// LitObject
	///		Item (Event level)
	///		Object (Object Reference)
	///		
	/// </remarks>
	/// </summary>
	public class LitRef {
		public LitRef() { }
		public LitRef(String tag) : this(new LitTag(tag)) { }
		public LitRef(LitTag tag) { Tags.Add(tag); }
		public List<LitTag> Tags { get; set; } = new List<LitTag>();
		public String Commentary { get; set; } = "";

	}
	public static partial class LitExtensions {
		/// <summary>
		/// Will return true if the references contains tags that are equal to eachother
		/// </summary>
		/// <param name="ref1"></param>
		/// <param name="ref2"></param>
		/// <returns></returns>
		public static bool IsReferenceIntersection(this LitRef ref1, LitRef ref2) {
			return ref1.Tags.Intersect(ref2.Tags, new LitTag()).Count() > 0;
		}
		/// <summary>
		/// Will insert the contents (tags mostly) of the second reference into the first
		/// </summary>
		/// <param name="ref1"></param>
		/// <param name="ref2"></param>
		public static void CombineRef(this LitRef ref1, LitRef ref2) {
			ref1.Tags.AddRange(ref2.Tags.Except(ref1.Tags, new LitTag()));
			if (String.IsNullOrWhiteSpace(ref1.Commentary)) {
				ref1.Commentary = ref2.Commentary;
			}
		}
		/// <summary>
		/// Adds a tag to the reference (only if it doesn't contain that tag)
		/// </summary>
		/// <param name="litRef"></param>
		/// <param name="tag"></param>
		public static void AddTag(this LitRef litRef, LitTag tag) {
			if (!litRef.Tags.Contains(tag, new LitTag())) {
				litRef.Tags.Add(tag);
			}
		}
	}
	public static partial class ParsingTools {
		public static String WriteReferenceLinkDefault(
			this LitOptions LO,
			LitRef reference
		){
			string link = "";
			if (reference is LitChar) { link = "Character"; }
			else if (reference is LitPlace) { link = "Place"; }
			else if (reference is LitMyth) { link = "Myth"; }
			else if (reference is LitObject) { link = "Object"; }
			return LO.WriteLink(new MDLinkLine() {
				Tag = reference.Tags.First().Tag,
				Link = link
			});

		}
		public static List<String> WriteNotesLinesDefault(
			this LitOptions LO,
			LitNovel novel,
			LitRef reference
		){
			var retVal = new List<String>();

			retVal.Add(LO.WriteNotesHeader(novel, reference));
			retVal.Add(LO.WriteNotesLink(novel, reference));
			retVal.AddRange(LO.WriteNotesCommentary(novel, reference));
			retVal.AddRange(LO.WriteNotesTags(novel, reference));

			if (reference is LitChar) {
				retVal.AddRange(LO.WriteNotesCharLines(novel, reference as LitChar));
			}
			if (reference is LitPlace) {
				retVal.AddRange(LO.WriteNotesPlaceLines(novel, reference as LitPlace));
			}
			if (reference is LitMyth) {
				retVal.AddRange(LO.WriteNotesMythLines(novel, reference as LitMyth));
			}
			if (reference is LitObject) {
				retVal.AddRange(LO.WriteNotesObjectLines(novel, reference as LitObject));
			}

			return retVal;
		}
		public static String WriteNotesHeaderDefault(
			this LitOptions LO, 
			LitNovel novel, 
			LitRef reference
		){
			var TagHeader = new MDHeader() {
				HeaderLevel = 1,
				Text = reference.Tags.First().Tag
			};
			return TagHeader.ToString();
		}
		public static String WriteNotesLinkDefault(
			this LitOptions LO,
			LitNovel novel,
			LitRef reference
		){
			var retVal = new MDLinkLine();
			retVal.Link = "Reference";
			if (reference is LitChar) {
				retVal.Tag = "Character";
			}
			else if (reference is LitPlace) {
				retVal.Tag = "Place";
			}
			else if (reference is LitMyth) {
				retVal.Tag = "Myth";
			}
			else if (reference is LitObject) {
				retVal.Tag = "Object";
			}
			else {
				retVal.Tag = "Reference";
			}
			return retVal.ToString();
		}
		public static List<String> WriteNotesCommentaryDefault(
			this LitOptions LO,
			LitNovel novel,
			LitRef reference
		){
			return new List<string>(new String[] { reference.Commentary });
		}
		public static List<String> WriteNotesTagsDefault(
			this LitOptions LO,
			LitNovel novel,
			LitRef reference
		){
			var retVal = new List<string>();

			var tagsHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Tags"
			};
			retVal.Add(tagsHeader.ToString());
		
			//Place the tags in the header
			foreach (var tag in reference.Tags) {
				retVal.Add(tag.Tag);
			}

			return retVal;
		}
		public static LitRef ParseToLitRefDefault(this LitOptions LO, LitNovel novel, IEnumerable<String> lines) {
			if (lines.Count() == 0) { return null; }
			var PartitionedLines = ParsingTools.PartitionLines(lines, l => System.Text.RegularExpressions.Regex.IsMatch(l, @"^##[^#]"));
			var link = PartitionedLines.First().Select(s => LO.ParseLink(s)).Where(l => l != null).First();

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
			retVal.Commentary = LO.SourceLinesToString(PartitionedLines.First());

			//Save the tags
			pattern = "^## Tags$";
			var tagsList = PartitionedLines.Where(list => System.Text.RegularExpressions.Regex.IsMatch(list.First(), pattern)).First();
			foreach (var tagline in tagsList.Where(s => LO.IsSourceLine(s))) {
				retVal.AddTag(new LitTag(tagline));
			}

			return novel.AddReferenceDistinct(retVal);
		}
		public static String ReferenceHeader(this LitRef reference) {
			var TagHeader = new MDHeader() {
				HeaderLevel = 1,
				Text = reference.Tags.First().Tag
			};
			return TagHeader.ToString();
		}
	}
}
