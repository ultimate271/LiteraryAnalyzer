﻿using System;
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
		public List<LitMyth> Events { get; set; } = new List<LitMyth>();
		public List<LitObject> Items { get; set; } = new List<LitObject>();
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
		/// <summary>
		/// Takes two events, and if they are mergable, merges them together, returning the merged event
		/// </summary>
		/// <param name="event1"></param>
		/// <param name="event2"></param>
		/// <returns></returns>
		public static LitEvent MergeEvent(this LitEvent event1, LitEvent event2) {
			if (!event1.IsElmMergeable(event2)) { throw new Exception(String.Format("Event {0} not mergeable with Event {1}", event1.TreeTag, event2.TreeTag)); }
			event1.Speakers = new List<LitChar>(event1.Speakers.Union(event2.Speakers));
			event1.Events = new List<LitMyth>(event1.Events.Union(event2.Events));
			event1.Items = new List<LitObject>(event1.Items.Union(event2.Items));
			event1.References = new List<LitRef>(event1.References.Union(event2.References));
			var query = event2.Source.Text.Keys.Where(k => !event1.Source.Text.Keys.Contains(k));
			foreach (var litSourceInfo in query) {
				event1.Source.Text[litSourceInfo] = event2.Source.Text[litSourceInfo];
			}
			event1.Children = new List<LitEvent>(event1.Children.Zip(event2.Children, (e1, e2) => e1.MergeEvent(e2)));
			return event1;
		}
		/// <summary>
		/// Takes some event lines, determined to be syntactically correct, and creates an event object
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="lines"></param>
		/// <param name="author"></param>
		/// <returns></returns>
		public static LitEvent ParseToEventDefault(this LitOptions LO, LitNovel novel, LitAuthor author, IEnumerable<string> lines) {
			var retVal = new LitEvent();

			//Some checks
			if (!novel.Authors.Contains(author)) { throw new Exception(String.Format("Novel does not contain source info. {0}", author.Author)); }
			if (lines.Count() == 0) { throw new Exception("Event must have lines"); }

			LO.ParseEventHeader(novel, retVal, lines);

			//TODO this is a bit ugly
			var PartitionedLines = LO.ExtractEvents(lines, LO.ParseHeader(lines.First()).HeaderLevel + 1);

			LO.ParseElmLinks(
				novel, 
				retVal, 
				LO.ExtractElmLinkLines(PartitionedLines.First())
			);

			//Extract the source lines
			LO.ParseEventText(novel, retVal, author, PartitionedLines.First());

			//Parse the subevents
			foreach (var subEventLines in PartitionedLines.Skip(1)) {
				var litEvent = LO.ParseToEvent(novel, author, subEventLines);
				retVal.Children.Add(litEvent);
			}

			return retVal;
		}
		public static void ParseEventHeaderDefault(this LitOptions LO, LitNovel novel, LitEvent litevent, IEnumerable<String> lines) {
			//Parse the header
			var header = LO.ParseHeader(lines.First());
			try {
				litevent.Header = header.Text;
			} catch (NullReferenceException e) {
				throw new Exception("The first line of an event must me a header", e);
			}
		}
		public static void ParseEventLinksDefault(
			this LitOptions LO,
			LitNovel novel,
			LitEvent litevent,
			IEnumerable<MDLinkLine> links
		){
			LitRef novelRef;
			foreach (var link in links) {
				if (link.Link.Equals("Speaker")) {
					novelRef = novel.AddReferenceDistinct(new LitChar(link.Tag));
					litevent.Speakers.Add(novelRef as LitChar);
				}	
				else if (link.Link.Equals("Event")) {
					novelRef = novel.AddReferenceDistinct(new LitMyth(link.Tag));
					litevent.Events.Add(novelRef as LitMyth);
				}
				else if (link.Link.Equals("Item")) {
					novelRef = novel.AddReferenceDistinct(new LitObject(link.Tag));
					litevent.Items.Add(novelRef as LitObject);
				}
			}
		}
		public static void ParseEventTextDefault(this LitOptions LO, LitNovel novel, LitEvent litevent, LitAuthor author, IEnumerable<String> lines) {
			litevent.Source.Text[author] = LO.SourceLinesToString(lines);
		}
		/// <summary>
		/// Takes a LitEvent and returns all of the speakers in this event or the child events.
		/// </summary>
		/// <param name="litevent"></param>
		/// <param name="speaker"></param>
		/// <returns></returns>
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

		public static IEnumerable<IEnumerable<String>> ExtractEventsDefault(this LitOptions LO, IEnumerable<String> lines, int headerLevel) {
			//Partition the events
			var pattern = String.Format(@"^{0}[^#]", new String('#', headerLevel));
			return ParsingTools.PartitionLines(lines, line => System.Text.RegularExpressions.Regex.IsMatch(line, pattern));
		}
		public static List<String> WriteEventLinksDefault(
			this LitOptions LO,
			LitEvent litevent
		){
			var retVal = new List<String>();
			retVal.AddRange(litevent.Speakers.Select(a => 
				MakeLinkLine("Speaker", a.Tags.First().Tag)
			));
			retVal.AddRange(litevent.Events.Select(a =>
				MakeLinkLine("Event", a.Tags.First().Tag)
			));
			retVal.AddRange(litevent.Items.Select(a =>
				MakeLinkLine("Item", a.Tags.First().Tag)
			));
			return retVal;
		}

		//public static List<String> ToSourceLines(this LitEvent litevent, LitSourceInfo sourceinfo, int headerlevel) {
		//	var retVal = new List<String>();
		//	retVal.Add(litevent.WriteHeader(headerlevel));
		//	retVal.AddRange(litevent.WriteEventLinks());
		//	try {
		//		retVal.Add(litevent.Source.Text[sourceinfo]);
		//	}
		//	catch { }
		//	foreach (var child in litevent.Children) {
		//		retVal.AddRange(child.ToSourceLines(sourceinfo, headerlevel + 1));
		//	}
		//	return retVal;
		//}
	}
}
