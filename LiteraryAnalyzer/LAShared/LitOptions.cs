using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitOptions {
		public delegate MDHeader ParseHeaderDelegate(String line);
		public ParseHeaderDelegate ParseHeader { get; set; }

		public delegate MDLinkLine ParseLinkDelegate(String line);
		public ParseLinkDelegate ParseLink { get; set; }

		public delegate bool IsSourceLineDelegate(String line);
		public IsSourceLineDelegate IsSourceLine { get; set; }

		public delegate String SourceLinesToStringDelegate(IEnumerable<String> lines);
		public SourceLinesToStringDelegate SourceLinesToString { get; set; }

		public delegate void TagLinesDelegate(MDSourceFile sourceFile);
		public TagLinesDelegate TagLines { get; set; }

		public delegate LitAuthor ParseLitSourceInfoDelegate(IEnumerable<String> metadatalines);
		public ParseLitSourceInfoDelegate ParseLitSourceInfo { get; set; }

		public delegate LitRef ParseLitRefDelegate(IEnumerable<String> lines);
		public ParseLitRefDelegate ParseLitRef { get; set; }

		public delegate IEnumerable<IEnumerable<String>> ExtractScenesDelegate(IEnumerable<IEnumerable<String>> PartitionedScenes);
		public ExtractScenesDelegate ExtractScenes { get; set; }

		public delegate IEnumerable<String> ExtractMetadataDelegate(IEnumerable<IEnumerable<String>> PartitionedScenes);
		public ExtractMetadataDelegate ExtractMetadata { get; set; }

		public delegate LitSceneMetadata ParseMetadataDelegate(IEnumerable<String> sourceLines);
		public ParseMetadataDelegate ParseMetadata { get; set; }

		public delegate LitAuthor ParseLitAuthorDelegate(IEnumerable<String> metadatalines);
		public ParseLitAuthorDelegate ParseLitAuthor { get; set; }

		public delegate List<String> WriteElmSourceLinesDelegate(LitElm litelm, LitAuthor sourceInfo);
		public WriteElmSourceLinesDelegate WriteElmSourceLines { get; set; }

		public delegate String WriteElmHeaderDelegate(LitElm litelm, int HeaderLevel);
		public WriteElmHeaderDelegate WriteElmHeader { get; set; }

		public delegate List<String> WriteElmLinksDelegate(LitElm litelm);
		public WriteElmLinksDelegate WriteElmLinks { get; set; }

		public delegate List<String> WriteElmTextDelegate(String Text);
		public WriteElmTextDelegate WriteElmText { get; set; }

		public delegate String WriteNotesHeaderDelegate(LitRef reference);
		public WriteNotesHeaderDelegate WriteNotesHeader { get; set; }

		public delegate String WriteNotesLinkDelegate(LitRef reference);
		public WriteNotesLinkDelegate WriteNotesLink { get; set; }

		public delegate List<String> WriteNotesCommentaryDelegate(LitRef reference);
		public WriteNotesCommentaryDelegate WriteNotesCommentary { get; set; }

		public delegate List<String> WriteNotesTagsDelegate(LitRef reference);
		public WriteNotesTagsDelegate WriteNotesTags { get; set; }

		public delegate List<String> WriteNotesLinesDelegate(LitRef reference);
		public WriteNotesLinesDelegate WriteNotesLines { get; set; }

		public delegate List<MDTag> GetAllTagsDelegate(LitElm elm, String Filename);
		public GetAllTagsDelegate GetAllTags { get; set; }


		//BuildSource
		//Parse event links (litevent.cs::ParseEvent)
		//ToNotesLines (LitRef)
		//WriteMetadataSourceLines
		public LitOptions() {
			this.ParseHeader = this.ParseHeaderDefault;
			this.ParseLink = this.ParseLinkDefault;
			this.IsSourceLine = this.IsSourceLineDefault;
			this.SourceLinesToString = this.SourceLinesToStringDefault;
			this.TagLines = this.TagLinesDefault;
			this.ParseLitSourceInfo = this.ParseLitSourceInfoDefault;
			this.ParseLitRef = this.ParseLitRefDefault;
			this.ExtractScenes = this.ExtractScenesDefault;
			this.ExtractMetadata = this.ExtractMetadataDefault;
			this.ParseMetadata = this.ParseMetadataDefault;
			this.ParseLitAuthor = this.ParseLitAuthorDefault;
			this.WriteElmSourceLines = this.WriteSourceLinesDefault;
			this.WriteElmHeader = this.WriteElmHeaderDefault;
			this.WriteElmLinks = this.WriteElmLinksDefault;
			this.WriteElmText = this.WriteElmTextDefault;
			this.WriteNotesHeader = this.WriteNotesHeaderDefault;
			this.WriteNotesLink = this.WriteNotesLinkDefault;
			this.WriteNotesCommentary = this.WriteNotesCommentaryDefault;
			this.WriteNotesTags = this.WriteNotesTagsDefault;
			this.WriteNotesLines = this.WriteNotesLinesDefault;
			this.GetAllTags = this.GetAllTagsDefault;
		}
	}
	public static partial class ParsingTools {
		/// <summary>
		/// Default implementaiton for parsing a line into a MDHeader object.
		/// </summary>
		/// <param name="line"></param>
		/// <returns>The MDHeader object, or null if the parse failed</returns>
		public static MDHeader ParseHeaderDefault(this LitOptions LO, String line) {
			var retVal = new MDHeader();
			LO.ParseLink(line);
			var match = System.Text.RegularExpressions.Regex.Match(line, @"^(#+)([^#].*)$");
			if (!match.Success) {
				return null;
			}
			else {
				try {
					retVal.HeaderLevel = match.Groups[1].Value.Length;
					retVal.Text = match.Groups[2].Value.Trim();
				}
				catch {
					return null;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Default implentation to takes all of the source lines of a list of arbitrary strings and turns them into a string
		/// </summary>
		public static String SourceLinesToStringDefault(this LitOptions LO, IEnumerable<string> lines) {
			var paragraphs = ParsingTools.PartitionLines(lines.Where(s => LO.IsSourceLine(s)), line => String.IsNullOrWhiteSpace(line));
			StringBuilder sb = new StringBuilder();
			foreach (var paralist in paragraphs) {
				foreach (var paraline in paralist.Where(l => !String.IsNullOrEmpty(l))) {
					sb.Append(paraline.TrimEnd('\r', '\n'));
					sb.Append(" ");
				}
				sb.Append("\r\n");
			}
			return sb.ToString().Trim();
		}
		/// <summary>
		/// Default implentation to parse a link line into a MDLink
		/// </summary>
		/// <param name="LO"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static MDLinkLine ParseLinkDefault(this LitOptions LO, String s) {
			var retVal = new MDLinkLine();
			var match = System.Text.RegularExpressions.Regex.Match(s, @"^\[([^\]]*)\]: # {([^}]*)}$");
			if (!match.Success) {
				return null;
			}
			else {
				try {
					retVal.Link = match.Groups[1].Value;
					retVal.Tag = match.Groups[2].Value;
				}
				catch {
					return null;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Takes a litelm and writes it's header at a particular level
		/// </summary>
		/// <param name="LO"></param>
		/// <param name="elm"></param>
		/// <param name="headerlevel"></param>
		/// <returns></returns>
		public static String WriteElmHeaderDefault(this LitOptions LO, LitElm elm, int headerlevel) {
			return String.Format("{0} {1}", new String('#', headerlevel), elm.Header);
		}
		public static IEnumerable<IEnumerable<String>> ExtractScenesDefault(this LitOptions LO, IEnumerable<IEnumerable<String>> PartitionedScenes) {
			return PartitionedScenes.Where(lines =>
				lines.Select(l => LO.ParseLink(l))
					.Where(link => link != null)
					.Where(link => link.Link.Equals("TreeTag"))
					.Count() > 0
				);
		}
		/// <summary>
		/// Takes a litelm and writes all of the lines for that elm that go into the source for a particular Author
		/// </summary>
		/// <param name="LO"></param>
		/// <param name="litElm"></param>
		/// <param name="sourceinfo"></param>
		/// <returns></returns>
		public static List<String> WriteSourceLinesDefault(this LitOptions LO, LitElm litElm, LitAuthor sourceinfo) {
			return ParsingTools.WriteSourceLinesDefault(LO, litElm, sourceinfo, 1);
		}
		/// <summary>
		/// Takes a litelm and writes all of the lines for that elm that go into the source for a particular Author
		/// </summary>
		/// <param name="LO"></param>
		/// <param name="litElm"></param>
		/// <param name="sourceinfo"></param>
		/// <param name="headerlevel"></param>
		/// <returns></returns>
		public static List<String> WriteSourceLinesDefault(this LitOptions LO, LitElm litElm, LitAuthor sourceinfo, int headerlevel) {
			var retVal = new List<String>();
			retVal.Add(LO.WriteElmHeader(litElm, headerlevel));
			retVal.AddRange(LO.WriteElmLinks(litElm));
			if (litElm is LitEvent) { 
				try {
					retVal.AddRange(LO.WriteElmText((litElm as LitEvent).Source.Text[sourceinfo]));
				}
				catch { }
			}
			foreach (var child in litElm.Children) {
				retVal.AddRange(WriteSourceLinesDefault(LO, child, sourceinfo, headerlevel + 1));
			}
			return retVal;
		}
		/// <summary>
		/// Takes a litelm and writes the links for it that go under the header
		/// </summary>
		/// <param name="litelm"></param>
		/// <returns></returns>
		public static List<String> WriteElmLinksDefault(this LitOptions LO, LitElm litelm) {
			var retVal = new List<String>();
			retVal.Add(MakeLinkLine("TreeTag", litelm.TreeTag.Tag));
			retVal.AddRange(litelm.UserTags.Select(t => MakeLinkLine("UserTag", t.Tag)));
			if (litelm is LitEvent) {
				retVal.AddRange((litelm as LitEvent).Speakers.Select(a => MakeLinkLine("Speaker", a.Tags.First().Tag)));
			}
			if (litelm is LitScene) {
				retVal.AddRange((litelm as LitScene).Actors.Select(a => MakeLinkLine("Actor", a.Tags.First().Tag)));
				retVal.AddRange((litelm as LitScene).Location.Select(p => MakeLinkLine("Location", p.Tags.First().Tag)));
				retVal.AddRange((litelm as LitScene).References.Select(r => MakeLinkLine("Reference", r.Tags.First().Tag)));
			}
			return retVal;
		}
		public static List<String> WriteElmTextDefault(this LitOptions LO, String Text) {
			var retVal = new List<String>();
			retVal.Add(Text);
			return retVal;
		}
		public static String WriteNotesHeaderDefault(this LitOptions LO, LitRef reference) {
			var TagHeader = new MDHeader() {
				HeaderLevel = 1,
				Text = reference.Tags.First().Tag
			};
			return TagHeader.ToString();
		}
		public static String WriteNotesLinkDefault(this LitOptions LO, LitRef reference) {
			var retVal = new MDLinkLine();
			retVal.Link = "Reference";
			if (reference is LitChar) {
				retVal.Tag = "Character";
			}
			else if (reference is LitPlace) {
				retVal.Tag = "Place";
			}
			else {
				retVal.Tag = "Reference";
			}
			return retVal.ToString();
		}
		public static List<String> WriteNotesCommentaryDefault(this LitOptions LO, LitRef reference) {
			return new List<string>(new String[] { reference.Commentary });
		}
		public static List<String> WriteNotesTagsDefault(this LitOptions LO, LitRef reference) {
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
		public static LitRef ParseLitRefDefault(this LitOptions LO, IEnumerable<String> lines) {
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

			return retVal;
		}
		public static IEnumerable<String> ExtractMetadataDefault (this LitOptions LO, IEnumerable<IEnumerable<String>> PartitionedScenes){
			return PartitionedScenes.Where(lines => 
				lines.Select(l => LO.ParseLink(l))
					.Where(link => link != null)
					.Where(link => link.Link.Equals("Metadata"))
					.Count() > 0
				).FirstOrDefault();
		}
		public static LitSceneMetadata ParseMetadataDefault(this LitOptions LO, IEnumerable<String> sourceLines) {
			var retVal = new LitSceneMetadata();
			var links = sourceLines.Select(l => LO.ParseLink(l)).Where(link => link != null);
			retVal.Descriptor = links.Where(link => link.Link.Equals("Descriptor")).Select(link => link.Tag).FirstOrDefault();

			var pattern = @"^# (.*)$";
			var match = System.Text.RegularExpressions.Regex.Match(sourceLines.First(), pattern);
			retVal.Header = match.Groups[1].Value;

			return retVal;
		}
		public static LitAuthor ParseLitAuthorDefault(this LitOptions LO, IEnumerable<String> metadatalines) {
			var retVal = new LitAuthor();
			var links = metadatalines.Select(l => LO.ParseLink(l)).Where(link => link != null);
			retVal.Author = links.Where(link => link.Link.Equals("Author")).Select(link => link.Tag).FirstOrDefault();
			return retVal;
		}
		public static bool IsSourceLineDefault(this LitOptions LO, String line) {
			return LO.ParseHeader(line) == null && LO.ParseLink(line) == null;
		}
		public static LitAuthor ParseLitSourceInfoDefault(this LitOptions LO, IEnumerable<String> metadatalines) {
			var retVal = new LitAuthor();
			var links = metadatalines.Select(l => LO.ParseLink(l)).Where(link => link != null);
			retVal.Author = links.Where(link => link.Link.Equals("Author")).Select(link => link.Tag).FirstOrDefault();
			return retVal;
		}
		public static List<String> WriteNotesLinesDefault(this LitOptions LO, LitRef reference) {
			var retVal = new List<String>();

			retVal.Add(LO.WriteNotesHeader(reference));
			retVal.Add(LO.WriteNotesLink(reference));
			retVal.AddRange(LO.WriteNotesCommentary(reference));
			retVal.AddRange(LO.WriteNotesTags(reference));

			return retVal;
		}

		public static List<MDTag> GetAllTagsDefault(this LitOptions LO, LitElm elm, String Filename) {
			return ParsingTools.GetAllTagsDefault(LO, elm, Filename, 1);
		}
		public static List<MDTag> GetAllTagsDefault(this LitOptions LO, LitElm elm, String Filename, int HeaderLevel) {
			var retVal = new List<MDTag>();
			var tempList = new List<LitTag>();

			tempList.Add(elm.TreeTag);
			tempList.AddRange(elm.UserTags);
			retVal = tempList.Select(t => new MDTag() { TagName = t.Tag, TagFile = Filename, TagLine = LO.WriteElmHeader(elm, HeaderLevel) }).ToList();

			foreach (var child in elm.Children) {
				retVal.AddRange(ParsingTools.GetAllTagsDefault(LO, child, Filename, HeaderLevel + 1));
			}
			return retVal;
		}
		public static void TagLinesDefault(this LitOptions LO, MDSourceFile sourcefile) {
			sourcefile.Lines = new List<string>(TagLinesDefault(LO, sourcefile.Lines, sourcefile.Descriptor, sourcefile.Author));
		}
		public static List<String> TagLinesDefault(this LitOptions LO, IEnumerable<String> lines, String tag, String author) {
			return TagLinesDefault(LO, lines, tag, author, 1);
		}
		public static List<String> TagLinesDefault (this LitOptions LO, IEnumerable<String> lines, String tag, String author, int headerLevel) {
			var retVal = new List<String>();
			var arg = new List<String>();
			//First remove the existing tags
			var query = lines.Where(s => {
				var linkLine = LO.ParseLink(s);
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
						retVal.Add(String.Format(@"[Author]: # {{{0}}}", author));
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
						retVal.AddRange(TagLinesDefault(LO, arg, String.Format(@"{0}.{1:00}", tag, i), author, headerLevel + 1));

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
				retVal.AddRange(TagLinesDefault(LO, arg, String.Format(@"{0}.{1:00}", tag, i), author, headerLevel + 1));
			}
			return retVal;
		}
	}
}
