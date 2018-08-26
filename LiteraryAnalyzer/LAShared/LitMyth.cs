using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// This will contain any background material that might be relevent to the reader
	/// </summary>
	public class LitMyth : LitRef {
		public LitMyth() : base() { }
		public LitMyth(String tag) : base(tag) { }
		public LitMyth(LitTag tag) : base(tag) { }
	}
	public static partial class ParsingTools {
		public static List<String> WriteNotesMythLinesDefault(
			this LitOptions LO,
			LitNovel novel,
			LitMyth myth
		){
			var retVal = new List<String>();
			var AllElms = novel.AllElms();

			//Show actor instances
			var eventHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Event of"
			};
			retVal.Add(eventHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(e => e.Events.Contains(myth))
				.Select(e => e.TreeTag.ToHyperlink())
			);

			//Show mentions
			var mythHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Mentioned in"
			};
			retVal.Add(mythHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(e => e.References.Contains(myth))
				.Select(e => e.TreeTag.ToHyperlink())
			);

			return retVal;
		}

	}
}
