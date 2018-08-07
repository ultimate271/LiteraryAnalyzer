using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents the tree hiearchy of structure that can go into a scene
	/// </summary>
	public class LitEvent : LitElm {
		/// <summary>
		///Represents a particular node
		/// </summary>
		public LitSource Source { get; set; } = new LitSource();
		public List<LitChar> Speakers { get; set; } = new List<LitChar>();
	}
	public static partial class LitExtensions {
		/// <summary>
		/// I HAVE DECIDED THAT THIS IS PROBABLY NOT THE BEST WAY TO DO THINGS
		/// </summary>
		/// <param name="elm"></param>
		public static void TagChildren(this LitElm elm) {
			if (elm.Children != null) {
				int i = 1;
				foreach (var child in elm.Children) {
					child.TreeTag.Tag = String.Format("{0}.{1}", elm.TreeTag.Tag, i);
					child.TagChildren();
					i++;
				}
			}
		}
	}
	public static partial class ParsingTools {
		public static LitEvent MergeEvent(this LitEvent event1, LitEvent event2) {
			if (!event1.IsElmMergeable(event2)) { throw new Exception(String.Format("Event {0} not mergeable with Event {1}", event1.TreeTag, event2.TreeTag)); }
			event1.Speakers = new List<LitChar>(event1.Speakers.Union(event2.Speakers));
			var query = event2.Source.Text.Keys.Where(k => !event1.Source.Text.Keys.Contains(k));
			foreach (var litSourceInfo in query) {
				event1.Source.Text[litSourceInfo] = event2.Source.Text[litSourceInfo];
			}
			event1.Children = new List<LitEvent>(event1.Children.Zip(event2.Children, (e1, e2) => e1.MergeEvent(e2)));
			return event1;
		}
		public static LitEvent ParseEvent(this LitNovel novel, IEnumerable<string> lines, LitSourceInfo sourceInfo) {
			var retVal = new LitEvent();

			//Some checks
			if (!novel.SourceInfo.Contains(sourceInfo)) { throw new Exception(String.Format("Novel does not contain source info. {0}", sourceInfo.Author)); }

			//Check for lines
			if (lines.Count() == 0) {
				throw new Exception("Event must have lines");
			}

			//Parse the header
			var header = ParseHeader(lines.First());
			try {
				retVal.Header = header.Text;
			} catch (NullReferenceException e) {
				throw new Exception("The first line of an event must me a header", e);
			}

			//Partition the subevents
			var PartitionedLines = PartitionLines(lines, line => {
				var subheader = ParseHeader(line);
				if (subheader != null) {
					return subheader.HeaderLevel == header.HeaderLevel + 1;
				}
				return false;
			});

			//Extract the links from the first partition
			var links = PartitionedLines.First().Select(line => ParseLink(line)).Where(link => link != null);
			LitRef novelRef;
			foreach (var link in links) {
				if (link.Link.Equals("TreeTag")) {
					retVal.TreeTag = new LitTag(link.Tag);
				}
				else if (link.Link.Equals("Speaker")) {
					novelRef = novel.AddReferenceDistinct(new LitChar(link.Tag));
					retVal.Speakers.Add(novelRef as LitChar);
				}	
			}

			//Extract the source lines
			retVal.Source.Text[sourceInfo] = SourceLinesToString(PartitionedLines.First());

			//Parse the subevents
			foreach (var subEventLines in PartitionedLines.Skip(1)) {
				var litEvent = novel.ParseEvent(subEventLines, sourceInfo);
				retVal.Children.Add(litEvent);
			}

			return retVal;
		}
		public static IEnumerable<LitTag> SpeakerTags(this LitEvent litevent, LitChar speaker) {
			var retVal = new List<LitTag>();
			if (litevent.Speakers.Contains(speaker)) {
				retVal.Add(litevent.TreeTag);
			}
			foreach (var child in litevent.Children) {
				retVal.AddRange(child.SpeakerTags(speaker));
			}
			return retVal;
		}
	}
}
