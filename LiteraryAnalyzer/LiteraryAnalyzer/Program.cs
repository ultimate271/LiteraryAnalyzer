using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	class Program {
		static void Main(string[] args) {
			var action = new Controller();
			action.DeveloperDebug(System.IO.File.ReadAllText(@"C:\Users\bwebster\Source\Repos\notes\tolkien\sil22.md"));
			System.Console.ReadLine();
			//using (var db = new LiteraryAnalyzerContext()) {
			//	var root = db.Excerpts.Where(e => e.ExcerptID == 1).First();
			//	//var newChild = new Excerpt { ExcerptText = "A child of root" };
			//	//root.Children.Add(newChild);
			//	//root.Children.Remove(root.Children.Where(e => e.ExcerptID == 3).First());
			//	//db.Excerpts.Remove(db.Excerpts.Where(e => e.ExcerptID == 3).First());
			//	db.SaveChanges();
			//}
			//using (var db = new LiteraryAnalyzerContext()) {
			//	foreach (Excerpt e in db.Excerpts.OrderBy(e => e.ExcerptID)) {
			//		System.Console.WriteLine(e.ExcerptText);
			//	}
			//	System.Console.ReadLine();
			//}
		}
	}
}
