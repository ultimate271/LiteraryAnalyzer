using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitOptions {
		public delegate String SourceLinesToStringDelegate(IEnumerable<String> lines);
		public SourceLinesToStringDelegate SourceLinesToString { get; set; }

		public delegate MDHeader ParseHeaderDelegate(String line);
		public ParseHeaderDelegate ParseHeader { get; set; }

		public delegate MDLinkLine ParseLinkDelegate(String line);
		public ParseLinkDelegate ParseLink { get; set; }

		public delegate List<String> WriteSourceLinesDelegate(LitElm litelm, LitSourceInfo sourceInfo);
		public WriteSourceLinesDelegate WriteSourceLines { get; set; }

		//ParseLitRef
		//ParseMetadata
		//IsSourceLine (might want to rework this)
		//ExtractScenes
		//ExtractMetadata
		public LitOptions() {
			this.ParseHeader = this.ParseHeaderDefault;
			this.SourceLinesToString = this.SourceLinesToStringDefault;
			this.ParseLink = this.ParseLinkDefault;
			this.WriteSourceLines = this.WriteSourceLinesDefault;
		}
	}
	public static partial class ParsingTools {
		/// <summary>
		/// Parses a line into a MDHeader object.
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
		/// Takes all of the source lines of a list of arbitrary strings and turns them into a string
		/// </summary>
		public static String SourceLinesToStringDefault(this LitOptions LO, IEnumerable<string> lines) {
			var paragraphs = ParsingTools.PartitionLines(lines.Where(s => IsSourceLine(s)), line => String.IsNullOrWhiteSpace(line));
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
		public static List<String> WriteSourceLinesDefault(this LitOptions LO, LitElm litElm, LitSourceInfo sourceinfo) {
			return ParsingTools.WriteSourceLinesDefault(LO, litElm, sourceinfo, 1);
		}
		public static List<String> WriteSourceLinesDefault(this LitOptions LO, LitElm litElm, LitSourceInfo sourceinfo, int headerlevel) {
			var retVal = new List<String>();
			retVal.Add(litElm.WriteHeader(headerlevel));
			retVal.AddRange(WriteElmLinks(litElm));
			if (litElm is LitEvent) { 
				try {
					retVal.Add((litElm as LitEvent).Source.Text[sourceinfo]);
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
		public static List<String> WriteElmLinks(LitElm litelm) {
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
	}
}
