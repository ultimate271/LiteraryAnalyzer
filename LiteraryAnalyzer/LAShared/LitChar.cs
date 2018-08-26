using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Reference to a character in the novel (speaker, actor, etc)
	/// </summary>
	public class LitChar : LitRef {
		public LitChar() : base() { }
		public LitChar(String tag) : base(tag) { }
		public LitChar(LitTag tag) : base(tag) { }
	}
	public static partial class ParsingTools {
		/// <summary>
		/// To save on processing, this takes a set of partitioned lines and parses out the character from them
		/// </summary>
		/// <param name="retVal"></param>
		/// <param name="lines"></param>
		public static void ParseLitChar(this LitChar retVal, IEnumerable<IEnumerable<String>> lines) {

		}
		public static List<String> WriteNotesCharLinesDefault(
			this LitOptions LO,
			LitNovel novel,
			LitChar character
		){
			var retVal = new List<String>();
			var AllElms = novel.AllElms();

			//Show actor instances
			var actorHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Actor in"
			};
			retVal.Add(actorHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(s => s.Actors.Contains(character))
				.Select(s => s.TreeTag.ToHyperlink())
			);

			//Show speaker instances
			var speakerHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Speaker in"
			};
			retVal.Add(speakerHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(e => e.Speakers.Contains(character))
				.Select(e => e.TreeTag.ToHyperlink())
			);

			//Show mentions
			var characterHeader = new MDHeader() {
				HeaderLevel = 2,
				Text = "Mentioned in"
			};
			retVal.Add(characterHeader.ToString());
			retVal.AddRange(
				AllElms
				.Where(e => e.References.Contains(character))
				.Select(e => e.TreeTag.ToHyperlink())
			);

			return retVal;
		}
	}
}
