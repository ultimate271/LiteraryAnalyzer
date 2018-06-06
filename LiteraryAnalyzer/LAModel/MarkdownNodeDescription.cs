using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAModel {
    public class MarkdownNodeDescription : MarkdownFile {
		public String Tag { get; set; }
		public IEnumerable<String> Descriptors { get; set; }
		public String Content { get; set; }
		public override void ParseMarkdownToDatabase() {
			base.ParseMarkdownToDatabase();
		}
	}
}
