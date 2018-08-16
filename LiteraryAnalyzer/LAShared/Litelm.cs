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
	}
	public static partial class LitExtensions {
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
				catch (KeyNotFoundException) { }
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
		public static List<String> WriteElmTextGQQ(this LitOptions LO, String Text, int LineLength) {
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
					retVal.Add(paragraph.Substring(fromIndex, toIndex - fromIndex));
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
