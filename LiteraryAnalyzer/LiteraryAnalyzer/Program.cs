using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	class Program {
		static void Main(string[] args) {
			using (var db = new LiteraryAnalyzerContext()) {
				foreach (Excerpt e in db.Excerpts.OrderBy(e => e.ExcerptID)) {
					System.Console.WriteLine(e.ExcerptText);
				}
				System.Console.ReadLine();
			}
			//using (var db = new LiteraryAnalyzerContext()) {
			//	var root = new Excerpt { ExcerptText = "Root" };
			//	db.Excerpts.Add(root);
			//	db.SaveChanges();
			//}
		}
	}
}
