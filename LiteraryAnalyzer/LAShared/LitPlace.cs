using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitPlace : LitRef {
		public LitPlace() : base() { }
		public LitPlace(String tag) : base(tag) { }
		public LitPlace(LitTag tag) : base(tag) { }
	}
	public static partial class ParsingTools {
		public static List<String> WriteNotesPlaceLinesDefault(
			this LitOptions LO,
			LitNovel novel,
			LitPlace place
		){
			var retVal = new List<String>();
			var AllElms = novel.AllElms();

			//Show actor instances
			var locationHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Location of"
			};
			retVal.Add(locationHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(e => e is LitScene)
				.Select(e => e as LitScene)
				.Where(s => s.Locations.Contains(place))
				.Select(s => s.TreeTag.ToHyperlink())
			);

			//Show mentions
			var placeHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Mentioned in"
			};
			retVal.Add(placeHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(e => e.References.Contains(place))
				.Select(e => e.TreeTag.ToHyperlink())
			);

			return retVal;
		}
	}
}
