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
	public abstract class LitElm {
		public int LitelmID { get; set; }
		public string Header { get; set; }
		public List<LitTag> UserTags { get; set; } = new List<LitTag>();
		public LitTag TreeTag { get; set; } = new LitTag();
		public List<LitEvent> Children { get; set; } = new List<LitEvent>();
		public List<LitRef> References { get; set; } = new List<LitRef>();
	}
	public static partial class LitExtensions {
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
			}
			if (elm is LitScene) {
				LO.ParseSceneLinks(novel, elm as LitScene, links);
			}
			if (elm is LitEvent) {
				LO.ParseEventLinks(novel, elm as LitEvent, links);
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
		public static List<String> WriteSourceLinesDefault(this LitOptions LO, LitElm litElm, LitAuthor author, int headerlevel) {
			var retVal = new List<String>();
			retVal.Add(LO.WriteElmHeader(litElm, headerlevel));
			retVal.AddRange(LO.WriteElmLinks(litElm));
			if (litElm is LitEvent) { 
				try {
					retVal.AddRange(LO.WriteElmText((litElm as LitEvent).Source.Text[author]));
				}
				catch (KeyNotFoundException) { }
			}
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
			retVal.AddRange(litelm.UserTags.Select(t => MakeLinkLine("UserTag", t.Tag) ));
			foreach (var reference in litelm.References) {
				retVal.Add( LO.WriteReferenceLink(reference) );
			}
			if (litelm is LitEvent) {
				retVal.AddRange( LO.WriteEventLinks(litelm as LitEvent) );
			}
			if (litelm is LitScene) {
				retVal.AddRange( LO.WriteSceneLinks(litelm as LitScene) );
			}
			return retVal;
		}
		public static List<String> WriteElmTextDefault(this LitOptions LO, String Text) {
			var retVal = new List<String>();
			retVal.Add(Text);
			return retVal;
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
