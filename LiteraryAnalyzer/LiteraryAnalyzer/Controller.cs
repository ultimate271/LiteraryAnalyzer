using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteraryAnalyzer.LAShared;
using LiteraryAnalyzer.LAModel;


namespace LiteraryAnalyzer {
	public class Controller {
		private LiteraryAnalyzerContext db { get; set; } = new LiteraryAnalyzerContext();
		
		public Controller() : this (new LiteraryAnalyzerContext()) { }
		public Controller(LiteraryAnalyzerContext db) { this.db = db; }

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
		public void ParseMarkdownToFileSystem(MarkdownOption options) {
			var file = new MarkdownFile(options, this.db);
			file.ParseMarkdownToFileSystem();
		}
		public void ParseMarkdownToModel(MarkdownOption otions) {
			var file = new MarkdownFile(options, this.db);
			var myList = file.GenerateContents.ToList();
		}
	}
}
