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
		public LitSource Source { get; set; }
	}
	public static partial class LitExtensions {
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
}
