using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents the highest level of a section of a novel. A novel will be composed of a list of these.
	/// </summary>
	public class LitScene : Litelm {
		/// <summary>
		/// Represents the location or locations that the scene is taking place.
		/// Empty list indicates that this is simple exposition
		/// Usually a change in location would indicate a change of LitScene, but not always. An example would be
		/// # Julie and Susan discuss politics as they walk from Point A to Point B
		/// In this example, the scene is the discussion of politics.
		/// </summary>
		public List<LitPlace> Location { get; set; }
		/// <summary>
		/// This contains the list of events that happen in this scene
		/// </summary>
		public List<LitEvent> Events { get; set; }
		/// <summary>
		/// This list will contain all of the characters that are involved in the scene.
		/// This will also include all of the speakers from the events, if they are not included in the annotated markdown.
		/// </summary>
		public List<LitChar> Actors { get; set; }
		/// <summary>
		/// Will include any other references to anything that the reader might want to know while reading this scene.
		/// This should be distinct from all of the other lit tools here.
		/// </summary>
		public List<LitRef> References { get; set; }

	}
}
