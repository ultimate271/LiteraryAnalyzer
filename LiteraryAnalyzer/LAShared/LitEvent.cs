using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents the tree hiearchy of structure that can go into a scene
	/// </summary>
	public class LitEvent : Litelm {
		/// <summary>
		/// Represents the child nodes of this node.
		/// 
		/// Note that either Children or Source should be Null.
		/// That is, if a LitEvent has children, it should not have source, 
		/// and if it has source, it should not have children
		/// </summary>
		public List<LitEvent> Children { get; set; }
		/// <summary>
		/// Represents the source of this node.
		/// 
		/// Note that either Children or Source should be Null.
		/// That is, if a LitEvent has children, it should not have source, 
		/// and if it has source, it should not have children
		/// </summary>
		public LitSource Source { get; set; }
	}
}
