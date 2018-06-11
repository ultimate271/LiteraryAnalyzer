using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// This class is a wrapper around the abstract Litelm class, used to represent an authors source material. This material will be strictly
	/// something that has been written by the author, and not by me or any other annotator. Footnotes, headers, and threads all get parsed seperately
	/// </summary>
	public class LitSource : Litelm{
	}
}
