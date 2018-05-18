using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	public class Controller {
		private LiteraryAnalyzerContext db;
		public Controller() {
			db = new LiteraryAnalyzerContext();
		}

		public void DeveloperDebug (String markdown) {
			String[] sentences = markdown.Split('.','?','!');
			var lines = markdown.Replace("\r", "").Split('\n')
				.Where(l => !l.StartsWith("#"));
			System.Console.WriteLine(sentences.Length);

			var paras = new List<String>();
			var sb = new StringBuilder();
			foreach (String line in lines) {
				if (String.IsNullOrEmpty(line)) {
					paras.Add(sb.ToString());
					sb.Clear();
				}
				else { 
					sb.Append(line).Append(" ");
				}
			}
			foreach (String para in paras) {
				System.Console.Write("{0}: ", para.Count(c => c == '.' || c == '?' || c == '!'));
				foreach (String sen in para.Split('.', '?', '!')) {
					System.Console.Write("{0}, ", sen.Count(c => c == ' '));
				}
				System.Console.WriteLine();
			}
			System.Console.WriteLine(paras.Count);
		}
		/// <summary>
		/// Takes a markdown file and inserts records into the db for 
		/// </summary>
		/// <param name="markdownFile"></param>
		public void ParseMarkdownToDatabase(MarkdownFile file) {
			file.ParseMarkdownToDatabase(this.db);
			this.db.SaveChanges();
		}
		public void ParseMarkdownToFileSystem(MarkdownFile file) {
			file.ParseMarkdownToFileSystem();
		}
	}
}
