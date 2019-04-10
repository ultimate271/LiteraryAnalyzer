using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents a scene or event in the novel.
	/// </summary>
	public class LitElm {
		public int LitelmID { get; set; }
		public string Header { get; set; }
		public List<LitTag> UserTags { get; set; } = new List<LitTag>();
		public LitTag TreeTag { get; set; } = new LitTag();
		public List<LitElm> Children { get; set; } = new List<LitElm>();
		
		public List<LitChar> Actors { get; set; } = new List<LitChar>();
		public List<LitChar> Speakers { get; set; } = new List<LitChar>();
		public List<LitPlace> Locations { get; set; } = new List<LitPlace>();
		public List<LitMyth> Events { get; set; } = new List<LitMyth>();
		public List<LitObject> Items { get; set; } = new List<LitObject>();
		public List<LitRef> References { get; set; } = new List<LitRef>();

		public LitSceneMetadata Metadata { get; set; } = new LitSceneMetadata();
		public LitSource Source { get; set; } = new LitSource();
	}
	public static partial class LitExtensions {
		public static String AllText(
			this LitElm elm,
			LitAuthor author
		) {
			var retVal = new StringBuilder();
			retVal.AppendLine(elm.Source[author]);
			foreach (var child in elm.Children) {
				retVal.AppendLine(child.AllText(author));
			}
			return retVal.ToString();
		} 
		public static IEnumerable<LitElm> AllElms(
			this LitElm elm
		) {
			var retVal = new List<LitElm>();
			retVal.Add(elm);
			foreach (var child in elm.Children) {
				retVal.AddRange(child.AllElms());
			}
			return retVal;
		}
		public static void AddElm(
			this LitElm parent,
			LitElm child
		) {
			parent.Children.Add(child);
		}
		//public static List<MDTag> GetAllTags(this LitElm elm, String Filename, int HeaderLevel) {
		//	var retVal = new List<MDTag>();
		//	var tempList = new List<LitTag>();

		//	tempList.Add(elm.TreeTag);
		//	tempList.AddRange(elm.UserTags);
		//	retVal = tempList.Select(t => new MDTag() { TagName = t.Tag, TagFile = Filename, TagLine = elm.WriteHeader(HeaderLevel) }).ToList();

		//	foreach (var child in elm.Children) {
		//		retVal.AddRange(child.GetAllTags(Filename, HeaderLevel + 1));
		//	}
		//	return retVal;
		//}
		public static IEnumerable<LitRef> GetAllReferences(this LitElm elm) {
			var retVal = new List<LitRef>();
			//USES REFLECTION
			var watch1 = elm.GetType().GetProperties()[0];
			var query = elm.GetType().GetProperties().Where(prop => {
				if (prop.PropertyType.IsGenericType) {
					if (prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) {
						var genericTypes = prop.PropertyType.GetGenericArguments();
						if (genericTypes.Count() == 1) {
							return typeof(LitRef).IsAssignableFrom(genericTypes[0]);
						}
					}
				}
				return false;
			});
			foreach (var prop in query) {
				var reflist = prop.GetValue(elm);
				if (reflist != null) {
					var castedList = (reflist as IEnumerable<object>).Cast<LitRef>();
					retVal.AddRange(castedList);
				}
			}
			//REFLECTION OVER
			//Recursive call to the children
			foreach (var child in elm.Children) {
				retVal.AddRange(child.GetAllReferences());
			}
			return retVal.Distinct();
		}
		public static bool IsElmMergeable(this LitElm elm1, LitElm elm2) {
			if (!elm1.GetType().Equals(elm2.GetType())) { return false; }
			if (!elm1.TreeTag.Tag.Equals(elm2.TreeTag.Tag)) { return false; }
			if (elm1.Children.Count != elm2.Children.Count) { return false; }
			if (elm1.Children.Count == 0 && elm2.Children.Count == 0) { return true; }
			return elm1.Children.Zip(elm2.Children, (c1, c2) => IsElmMergeable(c1, c2)).Aggregate((b1, b2) => b1 && b2);
		}
	}
	public static partial class ParsingTools { 
		public static IEnumerable<MDLinkLine> ExtractElmLinkLinesDefault(
			this LitOptions LO,
			IEnumerable<String> lines
		){
			return lines
				.Select(l => LO.ParseLink(l))
				.Where(l => l != null);
		}
		public static IEnumerable<IEnumerable<String>> ExtractSubElmsDefault(
			this LitOptions LO,
			IEnumerable<String> lines
		) {
			//Partition the events
			int headerLevel = LO.ParseHeader(lines.First()).HeaderLevel + 1;
			var pattern = String.Format(@"^{0}[^#]", new String('#', headerLevel));
			return ParsingTools.PartitionLines(lines, line => System.Text.RegularExpressions.Regex.IsMatch(line, pattern));
		}
		public static void ParseElmTextDefault(
			this LitOptions LO,
			LitNovel novel,
			LitElm elm,
			LitAuthor author,
			IEnumerable<String> lines
		){
			elm.Source[author] = LO.SourceLinesToString(lines);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static LitElm ParseToElmDefault(
			this LitOptions LO,
			LitNovel novel,
			LitSceneMetadata metadata,
			LitAuthor author,
			IEnumerable<String> lines
		){
			var retVal = new LitElm();

			//Some checks
			if (!novel.Authors.Contains(author)) { throw new Exception(String.Format("Novel does not contain source info. {0}", author.Author)); }
			if (!novel.SceneMetadata.Contains(metadata)) { throw new Exception(String.Format("Novel does not contain metadata. {0}", metadata.Descriptor)); }

			//Parse the header
			LO.ParseElmHeader(novel, retVal, lines);

			var PartitionedLines = LO.ExtractSubElms(lines);

			LO.ParseElmLinks(
				novel,
				retVal, 
				LO.ExtractElmLinkLines(PartitionedLines.First())
			);

			LO.ParseElmText(novel, retVal, author, PartitionedLines.First());

			foreach (var eventLines in PartitionedLines.Skip(1)) {
				var litEvent = LO.ParseToElm(novel, metadata, author, eventLines);
				retVal.AddElm(litEvent);
			}

			retVal.Metadata = metadata;
			return retVal;
		}
		public static void ParseElmHeaderDefault(
			this LitOptions LO,
			LitNovel novel,
			LitElm elm,
			IEnumerable<String> SceneLines
		) {
			var headerInfo = LO.ParseHeader(SceneLines.First());
			elm.Header = headerInfo.Text;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="LO"></param>
		/// <param name="PartitionedScenes"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<String>> ExtractElmsDefault(
			this LitOptions LO,
			IEnumerable<IEnumerable<String>> PartitionedScenes
		) {
			return PartitionedScenes.Where(lines =>
				lines.Select(l => LO.ParseLink(l))
					.Where(link => link != null)
					.Where(link => link.Link.Equals("TreeTag"))
					.Count() > 0
				);
		}
		public static LitElm MergeElm(this LitElm elm1, LitElm elm2) {
			elm1.Actors = new List<LitChar>(elm1.Actors.Union(elm2.Actors));
			elm1.Locations = new List<LitPlace>(elm1.Locations.Union(elm2.Locations)); 
			elm1.References = new List<LitRef>(elm1.References.Union(elm2.References));
			elm1.Speakers = new List<LitChar>(elm1.Speakers.Union(elm2.Speakers));
			elm1.Events = new List<LitMyth>(elm1.Events.Union(elm2.Events));
			elm1.Items = new List<LitObject>(elm1.Items.Union(elm2.Items));
			elm1.References = new List<LitRef>(elm1.References.Union(elm2.References));
			var query = elm2.Source.Text.Keys.Where(k => !elm1.Source.Text.Keys.Contains(k));
			foreach (var litSourceInfo in query) {
				elm1.Source[litSourceInfo] = elm2.Source[litSourceInfo];
			}
			elm1.Children = new List<LitElm>(
				elm1.Children.Zip(
					elm2.Children, (e1, e2) => e1.MergeElm(e2)
				)
			);
			return elm1;
		}

		public static void ParseElmLinksDefault(
			this LitOptions LO,
			LitNovel novel,
			LitElm elm,
			IEnumerable<MDLinkLine> links
		){
			foreach (var link in links) {
				//I feel as though there is a way to use reflection to be super clever here,
				//But upon thinking about it, I think it would only create more confusion than it
				//would help, since the actual properties of the scene are not that numerous,
				//and to be honest there would probably me more exceptional cases than I am willing
				//To admit, so at this juncture, I will use a elseif chain to do what I want.
				//I don't like it, but at the same time I sort of do because it is more explicit and easier
				//To work with, and an if else chain makes sense.
				//I want to use reflection so bad, but it's probably for the best that I do this in
				//The concrete way for now, and if at some point down the road, I want to change this to use reflection,
				//It will be not terribly difficult to do (at least, only as difficult as reflection is)
				LitRef novelRef;
				if (link.Link.Equals("TreeTag")) {
					elm.TreeTag = new LitTag(link.Tag);
				}
				else if (link.Link.Equals("UserTag")) {
					//TODO UserTags must be unique, not only and that should be checked somewhere here
					elm.UserTags.Add(new LitTag(link.Tag));
				}
				else if (link.Link.Equals("Character")) {
					novelRef = novel.AddReferenceDistinct(new LitChar(link.Tag));
					elm.References.Add(novelRef as LitChar);
				}
				else if (link.Link.Equals("Place")) {
					novelRef = novel.AddReferenceDistinct(new LitPlace(link.Tag));
					elm.References.Add(novelRef as LitPlace);
				}
				else if (link.Link.Equals("Myth")) {
					novelRef = novel.AddReferenceDistinct(new LitMyth(link.Tag));
					elm.References.Add(novelRef as LitMyth);
				}
				else if (link.Link.Equals("Object")) {
					novelRef = novel.AddReferenceDistinct(new LitObject(link.Tag));
					elm.References.Add(novelRef as LitObject);
				}
				else if (link.Link.Equals("Actor")) {
					novelRef = novel.AddReferenceDistinct(new LitChar(link.Tag));
					elm.Actors.Add(novelRef as LitChar);
				}
				else if (link.Link.Equals("Location")) {
					novelRef = novel.AddReferenceDistinct(new LitPlace(link.Tag));
					elm.Locations.Add(novelRef as LitPlace);
				}
				else if (link.Link.Equals("Speaker")) {
					novelRef = novel.AddReferenceDistinct(new LitChar(link.Tag));
					elm.Speakers.Add(novelRef as LitChar);
				}	
				else if (link.Link.Equals("Event")) {
					novelRef = novel.AddReferenceDistinct(new LitMyth(link.Tag));
					elm.Events.Add(novelRef as LitMyth);
				}
				else if (link.Link.Equals("Item")) {
					novelRef = novel.AddReferenceDistinct(new LitObject(link.Tag));
					elm.Items.Add(novelRef as LitObject);
				}
			}
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
		/// <summary>
		/// Takes a litelm and writes all of the lines for that elm that go into the source for a particular Author
		/// </summary>
		/// <param name="LO"></param>
		/// <param name="litElm"></param>
		/// <param name="author"></param>
		/// <returns></returns>
		public static List<String> WriteSourceLinesDefault(this LitOptions LO, LitElm litElm, LitAuthor author) {
			return ParsingTools.WriteSourceLinesDefault(LO, litElm, author, 1);
		}
		/// <summary>
		/// Takes a litelm and writes all of the lines for that elm that go into the source for a particular Author
		/// </summary>
		/// <param name="LO"></param>
		/// <param name="litElm"></param>
		/// <param name="author"></param>
		/// <param name="headerlevel"></param>
		/// <returns></returns>
		public static List<String> WriteSourceLinesDefault(
			this LitOptions LO, 
			LitElm litElm, 
			LitAuthor author, 
			int headerlevel
		) {
			var retVal = new List<String>();
			retVal.Add(LO.WriteElmHeader(litElm, headerlevel));
			retVal.AddRange(LO.WriteElmLinks(litElm));
			try {
				retVal.AddRange(LO.WriteElmText(litElm.Source[author]));
			}
			catch (KeyNotFoundException) { }
			foreach (var child in litElm.Children) {
				retVal.AddRange(WriteSourceLinesDefault(LO, child, author, headerlevel + 1));
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
			retVal.Add( MakeLinkLine("TreeTag", litelm.TreeTag.Tag) );
			retVal.AddRange(litelm.UserTags.Select(t => 
				MakeLinkLine("UserTag", t.Tag) 
			));
			retVal.AddRange(litelm.Actors.Select(a => 
				MakeLinkLine("Actor", a.Tags.First().Tag)
			));
			retVal.AddRange(litelm.Speakers.Select(a => 
				MakeLinkLine("Speaker", a.Tags.First().Tag)
			));
			retVal.AddRange(litelm.Locations.Select(p => 
				MakeLinkLine("Location", p.Tags.First().Tag)
			));
			retVal.AddRange(litelm.Events.Select(a =>
				MakeLinkLine("Event", a.Tags.First().Tag)
			));
			retVal.AddRange(litelm.Items.Select(a =>
				MakeLinkLine("Item", a.Tags.First().Tag)
			));
			foreach (var reference in litelm.References) {
				retVal.Add( LO.WriteReferenceLink(reference) );
			}
			return retVal;
		}
		public static List<String> WriteElmTextDefault(this LitOptions LO, String Text) {
			return LO.WriteTextGQQ(Text, 80);
		}
		public static List<String> WriteTextIdentity(this LitOptions LO, String Text) {
			return new List<String>(new String[] { Text });
		}
		public static List<String> WriteTextGQQ(this LitOptions LO, String Text, int LineLength) {
			var retVal = new List<String>();
			var paragraphs = System.Text.RegularExpressions.Regex.Split(Text, "\r\n");
			int fromIndex, toIndex, seekIndex;
			foreach (var paragraph in paragraphs) {
				fromIndex = 0;
				toIndex = 0;
				seekIndex = 1;
				while (fromIndex < paragraph.Length) {
					while (toIndex + seekIndex < paragraph.Length && toIndex + seekIndex - fromIndex < LineLength) {
						if (paragraph[toIndex + seekIndex] == ' ') {
							toIndex = toIndex + seekIndex;
							seekIndex = 0;
						}
						seekIndex++;
					}
					if (toIndex + seekIndex >= paragraph.Length) { toIndex = toIndex + seekIndex; }
					retVal.Add(paragraph.Substring(fromIndex, toIndex - fromIndex).Trim());
					toIndex++; //Skip the space
					fromIndex = toIndex;
					seekIndex = 1;
				}
				retVal.Add("");
			}
			return retVal;
		}
		//public static List<String> WriteOutline(this LitElm elm, int headerlevel) {
		//	var retVal = new List<String>();
		//	retVal.Add(elm.WriteHeader(headerlevel));
		//	foreach (var child in elm.Children) {
		//		retVal.AddRange(child.WriteOutline(headerlevel + 1));
		//	}
		//	return retVal;
		//}
			
	}
}
