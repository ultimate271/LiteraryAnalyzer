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
		/// Represents the source of this node.
		/// 
		/// Note that either Children or Source should be Null.
		/// That is, if a LitEvent has children, it should not have source, 
		/// and if it has source, it should not have children
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
		public static LitEvent ParseEvent(this LitNovel novel, IEnumerable<string> lines, LitSourceInfo sourceInfo) {
			var retVal = new LitEvent();

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
	}
}
