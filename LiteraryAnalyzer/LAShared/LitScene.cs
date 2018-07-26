using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents the highest level of a section of a novel. A novel will be composed of a list of these.
	/// </summary>
	public class LitScene : LitElm {
		/// <summary>
		/// Represents the location or locations that the scene is taking place.
		/// Empty list indicates that this is simple exposition
		/// Usually a change in location would indicate a change of LitScene, but not always. An example would be
		/// # Julie and Susan discuss politics as they walk from Point A to Point B
		/// In this example, the scene is the discussion of politics.
		/// </summary>
		public List<LitPlace> Location { get; set; }
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
	public static partial class ParsingTools {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static LitScene ParseScene(IEnumerable<String> lines) {
			var retVal = new LitScene();
			var headerInfo = ParsingTools.ParseHeader(lines.First());
			if (headerInfo.HeaderLevel != 1) {
				throw new Exception("The first line of a scene must have header level 1, " + lines.First());
			}
			retVal.Header = headerInfo.Text;
			var pattern = @"^##[^#]";
			var PartitionedLines = ParsingTools.PartitionLines(lines, line => System.Text.RegularExpressions.Regex.IsMatch(line, pattern));
			if (PartitionedLines.Count <= 1) {
				throw new Exception("A scene must have at least one event in it");
			}
			var query = PartitionedLines.First()
				.Select(l => ParsingTools.ParseLink(l))
				.Where(l => l != null);

			foreach (var link in query) {

			}

			return retVal;
		}
	}
}
