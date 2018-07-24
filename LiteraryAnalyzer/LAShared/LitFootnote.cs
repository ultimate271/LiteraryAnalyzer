using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// This is a rather cool lit element that I like and would like to start using more.
	/// 
	/// This element contains a reference to a source literary element, and an index, the spot where the footnote is placed.
	/// If the index is greater than the length of the source string, then the foot note will be considered to be placed at the end of the source string
	/// The base.text field represents the actual note itself, e.g., what the annotator actually wrote in the footer of the page, (or at the back of the book, or end of the markdown, or wherever)
	///
	/// Something else to note, the concept of "Identifier" is not present, this is because the identifier (be it a star, cross mark, numbered mark, or etc.) 
	/// is a notion that is requried only because the reader requires it, in fact, in some epic poems, authors will not use footnotes and
	/// will instead provide commentary in the form of line number references with notes and remarks. In this way we will think about all 
	/// footnotes, even if not provided in the text this way. It would be quite an eccentric work indeed that used exact markings to indicate
	/// something about the nature of their footnotes, and if that ever comes up I will have to restructure this.
	/// </summary>
	public class LitFootnote : LitElm {
		public virtual LitSource Source { get; set; }
		public int Index { get; set; }
	}
}
