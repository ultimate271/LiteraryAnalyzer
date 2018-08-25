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

		public delegate IEnumerable<String> BuildSourceFilenamesDelegate(MDAnnSourceInfo info);
		public BuildSourceFilenamesDelegate BuildSourceFilenames { get; set; }

		public delegate List<MDSourceFile> BuildSourceFilesDelegate(MDAnnSourceInfo AnnSourceInfo, IEnumerable<String> files);
		public BuildSourceFilesDelegate BuildSourceFiles { get; set; }

		public delegate MDNotesFile BuildNotesFileDelegate(MDAnnSourceInfo AnnSourceInfo, IEnumerable<String> files);
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

		public delegate void ParseElmLinksDelegate(
			LitNovel novel, LitElm litelm, IEnumerable<MDLinkLine> elmlines);
		public ParseElmLinksDelegate ParseElmLinks { get; set; }

		public delegate IEnumerable<MDLinkLine> ExtractElmLinkLinesDelegate(
			IEnumerable<String> lines);
		public ExtractElmLinkLinesDelegate ExtractElmLinkLines { get; set; }

		#region "Scene Parsing"
		public delegate IEnumerable<IEnumerable<String>> ExtractScenesDelegate(IEnumerable<IEnumerable<String>> PartitionedScenes);
		public ExtractScenesDelegate ExtractScenes { get; set; }

		public delegate LitScene ParseToSceneDelegate(LitNovel novel, LitSceneMetadata metadata, LitAuthor author, IEnumerable<String> scenelines);
		public ParseToSceneDelegate ParseToScene { get; set; }

		public delegate void ParseSceneHeaderDelegate(LitNovel novel, LitScene scene, IEnumerable<String> scenelines);
		public ParseSceneHeaderDelegate ParseSceneHeader { get; set; }

		public delegate void ParseSceneLinksDelegate(LitNovel novel, LitScene scene, IEnumerable<MDLinkLine> scenelines);
		public ParseSceneLinksDelegate ParseSceneLinks { get; set; }

		#endregion

		#region "Event Parsing"
		public delegate IEnumerable<IEnumerable<String>> ExtractEventsDelegate(IEnumerable<String> lines, int HeaderLevel);
		public ExtractEventsDelegate ExtractEvents { get; set; }

		public delegate LitEvent ParseToEventDelegate(LitNovel novel, LitAuthor author, IEnumerable<String> eventlines);
		public ParseToEventDelegate ParseToEvent { get; set; }

		public delegate void ParseEventHeaderDelegate(LitNovel novel, LitEvent litevent, IEnumerable<String> eventlines);
		public ParseEventHeaderDelegate ParseEventHeader { get; set; }

		public delegate void ParseEventLinksDelegate(
			LitNovel novel, LitEvent litevent, IEnumerable<MDLinkLine> eventlines);
		public ParseEventLinksDelegate ParseEventLinks { get; set; }

		public delegate void ParseEventTextDelegate(
			LitNovel novel, LitEvent litevent, LitAuthor author, IEnumerable<String> eventlines);
		public ParseEventTextDelegate ParseEventText { get; set; }

		#endregion

		#endregion

		#endregion

		#region "Writing"

		public delegate String WriteLinkDelegate(MDLinkLine link);
		public WriteLinkDelegate WriteLink { get; set; }

		#region "Filename Writers"
		public delegate String ToShortSourceFilenameDelegate(
			MDAnnSourceInfo info, LitSceneMetadata metadata, LitAuthor author);
		public ToShortSourceFilenameDelegate ToShourtSourceFilename { get; set; }

		public delegate String ToLongSourceFilenameDelegate(
			MDAnnSourceInfo info, MDSourceFile source);
		public ToLongSourceFilenameDelegate ToLongSourceFilename { get; set; }

		public delegate String ToShortNotesFilenameDelegate(
			MDAnnSourceInfo info);
		public ToShortNotesFilenameDelegate ToShortNotesFilename { get; set; }

		public delegate String ToLongNotesFilenameDelegate(
			MDAnnSourceInfo info);
		public ToLongNotesFilenameDelegate ToLongNotesFilename { get; set; }

		public delegate String ToShortTagFilenameDelegate(
			MDAnnSourceInfo info);
		public ToShortTagFilenameDelegate ToShortTagFilename { get; set; }

		public delegate String ToLongTagFilenameDelegate(
			MDAnnSourceInfo info);
		public ToLongTagFilenameDelegate ToLongTagFilename { get; set; }

		#endregion

		#region "Source Writing"
		public delegate MDAnnSource WriteAnnSourceDelegate(LitNovel novel);
		public WriteAnnSourceDelegate WriteAnnSource { get; set; }

		public delegate MDSourceFile WriteSoruceFileDelegate(
			LitNovel novel, LitSceneMetadata metadata, LitAuthor author);
		public WriteSoruceFileDelegate WriteSourceFile { get; set; }

		public delegate List<String> WriteMetadataDelegate(LitSceneMetadata metadata, LitAuthor author);
		public WriteMetadataDelegate WriteMetadata { get; set; }

		public delegate List<String> WriteElmSourceLinesDelegate(LitElm litelm, LitAuthor sourceInfo);
		public WriteElmSourceLinesDelegate WriteElmSourceLines { get; set; }

		public delegate String WriteElmHeaderDelegate(LitElm litelm, int HeaderLevel);
		public WriteElmHeaderDelegate WriteElmHeader { get; set; }

		public delegate List<String> WriteElmLinksDelegate(LitElm litelm);
		public WriteElmLinksDelegate WriteElmLinks { get; set; }

		public delegate String WriteReferenceLinkDelegate(LitRef reference);
		public WriteReferenceLinkDelegate WriteReferenceLink { get; set; }

		public delegate List<String> WriteSceneLinksDelegate(LitScene litscene);
		public WriteSceneLinksDelegate WriteSceneLinks { get; set; }

		public delegate List<String> WriteEventLinksDelegate(LitEvent litevent);
		public WriteEventLinksDelegate WriteEventLinks { get; set; }

		public delegate List<String> WriteElmTextDelegate(String Text);
		public WriteElmTextDelegate WriteElmText { get; set; }

		#endregion

		#region "Notes Writing"
		public delegate MDNotesFile WriteNotesFileDelegate(LitNovel novel);
		public WriteNotesFileDelegate WriteNotesFile { get; set; }

		public delegate String WriteNotesHeaderDelegate(LitNovel novel, LitRef reference);
		public WriteNotesHeaderDelegate WriteNotesHeader { get; set; }

		public delegate String WriteNotesLinkDelegate(LitNovel novel, LitRef reference);
		public WriteNotesLinkDelegate WriteNotesLink { get; set; }

		public delegate List<String> WriteNotesCommentaryDelegate(LitNovel novel, LitRef reference);
		public WriteNotesCommentaryDelegate WriteNotesCommentary { get; set; }

		public delegate List<String> WriteNotesTagsDelegate(LitNovel novel, LitRef reference);
		public WriteNotesTagsDelegate WriteNotesTags { get; set; }

		public delegate List<String> WriteNotesLinesDelegate(LitNovel novel, LitRef reference);
		public WriteNotesLinesDelegate WriteNotesLines { get; set; }

		public delegate List<String> WriteNotesCharLinesDelegate(LitNovel novel, LitChar character);
		public WriteNotesCharLinesDelegate WriteNotesCharLines { get; set; }

		public delegate List<String> WriteNotesPlaceLinesDelegate(LitNovel novel, LitPlace place);
		public WriteNotesPlaceLinesDelegate WriteNotesPlaceLines { get; set; }

		public delegate List<String> WriteNotesMythLinesDelegate(LitNovel novel, LitMyth myth);
		public WriteNotesMythLinesDelegate WriteNotesMythLines { get; set; }

		public delegate List<String> WriteNotesObjectLinesDelegate(LitNovel novel, LitObject obj);
		public WriteNotesObjectLinesDelegate WriteNotesObjectLines { get; set; }

		public delegate List<MDTag> GetAllTagsDelegate(LitElm elm, String Filename);
		public GetAllTagsDelegate GetAllTags { get; set; }

		#endregion

		#region "Tag File Writing"
		public delegate MDTagFile WriteTagFileDelegate(LitNovel novel, MDAnnSourceInfo info);
		public WriteTagFileDelegate WriteTagFile { get; set; }

		public delegate String WriteElmTagEXDelegate(LitElm elm);
		public WriteElmTagEXDelegate WriteElmTagEX { get; set; }

		public delegate String WriteTagLineDelegate(MDTag tag);
		public WriteTagLineDelegate WriteTagLine { get; set; }
		#endregion

		public delegate void WriteToFileSystemDelegate(MDAnnSource source, MDAnnSourceInfo info);
		public WriteToFileSystemDelegate WriteToFileSystem { get; set; }

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
			this.BuildSourceFilenames = this.BuildSourceFilenamesDefault;
			this.BuildSourceFiles = this.BuildSourceFilesDefault;
			this.BuildNotesFile = this.BuildNotesFileDefault;

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
			this.ExtractElmLinkLines = this.ExtractElmLinkLinesDefault;
			this.ParseElmLinks = this.ParseElmLinksDefault;
			this.ParseSceneHeader = this.ParseSceneHeaderDefault;
			this.ParseSceneLinks = this.ParseSceneLinksDefault;

			this.ExtractEvents = this.ExtractEventsDefault;
			this.ParseToEvent = this.ParseToEventDefault;
			this.ParseEventHeader = this.ParseEventHeaderDefault;
			this.ParseEventLinks = this.ParseEventLinksDefault;
			this.ParseEventText = this.ParseEventTextDefault;

			this.WriteAnnSource = this.WriteAnnSourceDefault;
			this.WriteSourceFile = this.WriteSourceFileDefault;
			this.WriteMetadata = this.WriteMetadataDefault;
			this.WriteElmSourceLines = this.WriteSourceLinesDefault;
			this.WriteElmHeader = this.WriteElmHeaderDefault;
			this.WriteElmLinks = this.WriteElmLinksDefault;
			this.WriteLink = this.WriteLinkDefault;
			this.WriteReferenceLink = this.WriteReferenceLinkDefault;
			this.WriteSceneLinks = this.WriteSceneLinksDefault;
			this.WriteEventLinks = this.WriteEventLinksDefault;
			this.WriteElmText = this.WriteElmTextDefault;

			this.WriteNotesFile = this.WriteNotesFileDefault;
			this.WriteNotesLines = this.WriteNotesLinesDefault;
			this.WriteNotesHeader = this.WriteNotesHeaderDefault;
			this.WriteNotesLink = this.WriteNotesLinkDefault;
			this.WriteNotesCommentary = this.WriteNotesCommentaryDefault;
			this.WriteNotesTags = this.WriteNotesTagsDefault;
			this.WriteNotesCharLines = this.WriteNotesCharLinesDefault;
			this.WriteNotesPlaceLines = this.WriteNotesPlaceLinesDefault;
			this.WriteNotesMythLines = this.WriteNotesMythLinesDefault;
			this.WriteNotesObjectLines = this.WriteNotesObjectLinesDefault;
			this.GetAllTags = this.GetAllTagsDefault;

			this.WriteTagFile = this.WriteTagFileDefault;
			this.WriteElmTagEX = this.WriteElmTagEXDefault;
			this.WriteTagLine = this.WriteTagLineDefault;
			this.ToShourtSourceFilename = this.ToShortFilenameDefault;
			this.ToLongSourceFilename = this.ToLongFilenameDefault;
			this.ToShortNotesFilename = this.ToShortNotesFilenameDefault;
			this.ToLongNotesFilename = this.ToLongNotesFilenameDefault;
			this.ToShortTagFilename = this.ToShortTagFilenameDefault;
			this.ToLongTagFilename = this.ToLongTagFilenameDefault;
			this.WriteToFileSystem = this.WriteToFileSystemDefault;


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
		public static bool IsSourceLineDefault(this LitOptions LO, String line) {
			return LO.ParseHeader(line) == null && LO.ParseLink(line) == null;
		}

		public static List<MDTag> GetAllTagsDefault(this LitOptions LO, LitElm elm, String Filename) {
			return ParsingTools.GetAllTagsDefault(LO, elm, Filename, 1);
		}
		public static List<MDTag> GetAllTagsDefault(this LitOptions LO, LitElm elm, String Filename, int HeaderLevel) {
			var retVal = new List<MDTag>();
			var tempList = new List<LitTag>();

			tempList.Add(elm.TreeTag);
			tempList.AddRange(elm.UserTags);
			retVal = tempList.Select(t => new MDTag() {
				TagName = t.Tag,
				TagFile = Filename,
				TagLine = LO.WriteElmTagEX(elm)
			}).ToList();

			foreach (var child in elm.Children) {
				retVal.AddRange(ParsingTools.GetAllTagsDefault(LO, child, Filename, HeaderLevel + 1));
			}
			return retVal;
		}
	}
}
