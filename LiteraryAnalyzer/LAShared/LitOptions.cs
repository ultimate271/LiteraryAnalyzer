using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitOptions {
		#region "MDParsing"
		public delegate MDHeader ParseHeaderDelegate(String line);
		public ParseHeaderDelegate ParseHeader { get; set; }

		public delegate MDLinkLine ParseLinkDelegate(String line);
		public ParseLinkDelegate ParseLink { get; set; }

		public delegate bool IsSourceLineDelegate(String line);
		public IsSourceLineDelegate IsSourceLine { get; set; }

		public delegate String SourceLinesToStringDelegate(IEnumerable<String> lines);
		public SourceLinesToStringDelegate SourceLinesToString { get; set; }

		#endregion

		#region "Build MDAnnSource"
		public delegate MDAnnSource BuildAnnSourceDelegate(MDAnnSourceInfo AnnSourceInfo);
		public BuildAnnSourceDelegate BuildAnnSource { get; set; }

		public delegate IEnumerable<MDSourceFile> BuildSourceFilesDelegate(MDAnnSourceInfo AnnSourceInfo);
		public BuildSourceFilesDelegate BuildSourceFiles { get; set; }

		public delegate MDNotesFile BuildNotesFileDelegate(MDAnnSourceInfo AnnSourceInfo);
		public BuildNotesFileDelegate BuildNotesFile { get; set; }
		#endregion

		#region "Source Parsing"
		public delegate LitNovel ParseAnnSourceDelegate(MDAnnSource AnnSource);
		public ParseAnnSourceDelegate ParseAnnSource { get; set; }

		#region "Tagging"
		public delegate void TagAnnSourceDelegate(MDAnnSource AnnSource);
		public TagAnnSourceDelegate TagAnnSource { get; set; }

		public delegate void TagSourceFileDelegate(MDSourceFile SourceFile);
		public TagSourceFileDelegate TagSourceFile { get; set; }

		#endregion

		#region "Notes File Parsing"
		public delegate void ParseNotesFileDelegate(LitNovel novel, MDNotesFile notesFile);
		public ParseNotesFileDelegate ParseNotesFile { get; set; }

		public delegate IEnumerable<IEnumerable<String>> ExtractNotesRefsDelegate(MDNotesFile notesfile);
		public ExtractNotesRefsDelegate ExtractNotesRefs { get; set; }

		public delegate LitRef ParseToLitRefDelegate(LitNovel novel, IEnumerable<String> lines);
		public ParseToLitRefDelegate ParseToLitRef { get; set; }

		#endregion

		#region "Source File Parsing"
		public delegate void ParseSourceFileDelegate(LitNovel novel, MDSourceFile sourcefile);
		public ParseSourceFileDelegate ParseSourceFile { get; set; }

		public delegate IEnumerable<IEnumerable<String>> ExtractFromSourceFileDelegate(MDSourceFile sourceFile);
		public ExtractFromSourceFileDelegate ExtractFromSourceFile { get; set; }

		#region "Metadata Parsing"
		public delegate IEnumerable<String> ExtractMetadataDelegate(IEnumerable<IEnumerable<String>> PartitionedScenes);
		public ExtractMetadataDelegate ExtractMetadata { get; set; }

		public delegate LitSceneMetadata ParseMetadataDelegate(LitNovel novel, IEnumerable<String> metadataLines);
		public ParseMetadataDelegate ParseMetadata { get; set; }

		public delegate LitAuthor ParseAuthorDelegate(LitNovel novel, IEnumerable<String> metadatalines);
		public ParseAuthorDelegate ParseAuthor { get; set; }

		#endregion

		#region "Scene Parsing"
		public delegate IEnumerable<IEnumerable<String>> ExtractScenesDelegate(IEnumerable<IEnumerable<String>> PartitionedScenes);
		public ExtractScenesDelegate ExtractScenes { get; set; }

		public delegate LitScene ParseToSceneDelegate(LitNovel novel, LitSceneMetadata metadata, LitAuthor author, IEnumerable<String> scenelines);
		public ParseToSceneDelegate ParseToScene { get; set; }

		public delegate void ParseSceneHeaderDelegate(LitNovel novel, LitScene scene, IEnumerable<String> scenelines);
		public ParseSceneHeaderDelegate ParseSceneHeader { get; set; }

		public delegate void ParseSceneLinksDelegate(LitNovel novel, LitScene scene, IEnumerable<String> scenelines);
		public ParseSceneLinksDelegate ParseSceneLinks { get; set; }

		#endregion

		#region "Event Parsing"
		public delegate IEnumerable<IEnumerable<String>> ExtractEventsDelegate(IEnumerable<String> lines, int HeaderLevel);
		public ExtractEventsDelegate ExtractEvents { get; set; }

		public delegate LitEvent ParseToEventDelegate(LitNovel novel, LitAuthor author, IEnumerable<String> eventlines);
		public ParseToEventDelegate ParseToEvent { get; set; }

		public delegate void ParseEventHeaderDelegate(LitNovel novel, LitEvent litevent, IEnumerable<String> eventlines);
		public ParseEventHeaderDelegate ParseEventHeader { get; set; }

		public delegate void ParseEventLinksDelegate(LitNovel novel, LitEvent litevent, IEnumerable<String> eventlines);
		public ParseEventLinksDelegate ParseEventLinks { get; set; }

		public delegate void ParseEventTextDelegate(LitNovel novel, LitEvent litevent, LitAuthor author, IEnumerable<String> eventlines);
		public ParseEventTextDelegate ParseEventText { get; set; }

		#endregion

		#endregion

		#endregion

		#region "Source Writing"
		public delegate MDAnnSource WriteAnnSourceDelegate(LitNovel novel);
		public WriteAnnSourceDelegate WriteAnnSource { get; set; }

		public delegate List<String> WriteMetadataDelegate(LitSceneMetadata metadata, LitAuthor author);
		public WriteMetadataDelegate WriteMetadata { get; set; }

		public delegate List<String> WriteElmSourceLinesDelegate(LitElm litelm, LitAuthor sourceInfo);
		public WriteElmSourceLinesDelegate WriteElmSourceLines { get; set; }

		public delegate String WriteElmHeaderDelegate(LitElm litelm, int HeaderLevel);
		public WriteElmHeaderDelegate WriteElmHeader { get; set; }

		public delegate List<String> WriteElmLinksDelegate(LitElm litelm);
		public WriteElmLinksDelegate WriteElmLinks { get; set; }

		public delegate List<String> WriteElmTextDelegate(String Text);
		public WriteElmTextDelegate WriteElmText { get; set; }

		#endregion

		#region "Notes Writing"
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

		public delegate List<String> WriteNotesLitcharLinesDelegate(LitChar character);
		public WriteNotesLitcharLinesDelegate WriteNotesLitcharLines { get; set; }

		public delegate List<MDTag> GetAllTagsDelegate(LitElm elm, String Filename);
		public GetAllTagsDelegate GetAllTags { get; set; }

		#endregion


		//BuildSource
		//Parse event links (litevent.cs::ParseEvent)
		//ToNotesLines (LitRef)
		//WriteMetadataSourceLines
		public LitOptions() {
			this.ParseHeader = this.ParseHeaderDefault;
			this.ParseLink = this.ParseLinkDefault;
			this.IsSourceLine = this.IsSourceLineDefault;
			this.SourceLinesToString = this.SourceLinesToStringDefault;

			this.BuildAnnSource = this.BuildAnnSourceDefault;
			//this.BuildSourceFiles = this.BuildSourceFilesDefault;
			//this.BuildNotesFile = this.BuildNotesFileDefault;

			this.ParseAnnSource = this.ParseAnnSourceDefault;

			this.TagAnnSource = this.TagAnnSourceDefault;
			this.TagSourceFile = this.TagSourceFileDefault;

			this.ParseNotesFile = this.ParseNotesFileDefault;
			this.ExtractNotesRefs = this.ExtractNotesRefsDefault;
			this.ParseToLitRef = this.ParseToLitRefDefault;

			this.ParseSourceFile = this.ParseSourceFileDefault;
			this.ExtractFromSourceFile = this.ExtractFromSourceFileDefault;

			this.ExtractMetadata = this.ExtractMetadataDefault;
			this.ParseMetadata = this.ParseMetadataDefault;
			this.ParseAuthor = this.ParseAuthorDefault;

			this.ExtractScenes = this.ExtractScenesDefault;
			this.ParseToScene = this.ParseToSceneDefault;
			this.ParseSceneHeader = this.ParseSceneHeaderDefault;
			this.ParseSceneLinks = this.ParseSceneLinksDefault;

			this.ExtractEvents = this.ExtractEventsDefault;
			this.ParseToEvent = this.ParseToEventDefault;
			this.ParseEventHeader = this.ParseEventHeaderDefault;
			this.ParseEventLinks = this.ParseEventLinksDefault;
			this.ParseEventText = this.ParseEventTextDefault;

			this.WriteAnnSource = this.WriteAnnSourceDefault;
			this.WriteMetadata = this.WriteMetadataDefault;
			//
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
		/// Default implentation to takes all of the source lines of a list of arbitrary strings and turns them into a string
		/// GOOD!
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
		public static IEnumerable<String> ExtractMetadataDefault (this LitOptions LO, IEnumerable<IEnumerable<String>> PartitionedScenes){
			return PartitionedScenes.Where(lines => 
				lines.Select(l => LO.ParseLink(l))
					.Where(link => link != null)
					.Where(link => link.Link.Equals("Metadata"))
					.Count() > 0
				).FirstOrDefault();
		}
		public static bool IsSourceLineDefault(this LitOptions LO, String line) {
			return LO.ParseHeader(line) == null && LO.ParseLink(line) == null;
		}
		public static List<String> WriteNotesLinesDefault(this LitOptions LO, LitRef reference, LitNovel novel) {
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
		public static List<String> WriteNotesLitcharLinesDelegate(this LitOptions LO, LitChar character) {

		}
	}
}
