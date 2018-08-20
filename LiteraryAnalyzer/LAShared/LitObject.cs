using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitObject : LitRef {
		public LitObject() : base() { }
		public LitObject(String tag) : base(tag) { }
		public LitObject(LitTag tag) : base(tag) { }
	}
	public static partial class ParsingTools {
		public static List<String> WriteNotesObjectLinesDefault(
			this LitOptions LO,
			LitNovel novel,
			LitObject obj
		){
			var retVal = new List<String>();
			var AllElms = novel.AllElms();

			//Show speaker instances
			var itemHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Speaker in"
			};
			retVal.Add(itemHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(e => e is LitEvent)
				.Select(e => e as LitEvent)
				.Where(e => e.Items.Contains(obj))
				.Select(e => e.TreeTag.ToHyperlink())
			);

			//Show mentions
			var objectHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Mentioned in"
			};
			retVal.Add(objectHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(e => e.References.Contains(obj))
				.Select(e => e.TreeTag.ToHyperlink())
			);

			return retVal;
		}

	}
}
