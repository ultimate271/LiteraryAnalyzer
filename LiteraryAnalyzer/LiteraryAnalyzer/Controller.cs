using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LiteraryAnalyzer {
	class Controller {
		private LiteraryAnalyzerContext db;
		private String rootDir;
		Controller() {
			db = new LiteraryAnalyzerContext();
		}
		Controller(String rootDir) {
			this.rootDir = rootDir;
		}
	}
}
