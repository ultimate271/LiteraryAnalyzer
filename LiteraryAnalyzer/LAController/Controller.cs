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
		public MarkdownOption MarkdownOption { get; set; } = new MarkdownOption();
		
		public Controller() : this (new LiteraryAnalyzerContext()) { }
		public Controller(LiteraryAnalyzerContext db) { this.db = db; }

		//public void DeveloperDebug (String markdown) {
		//	String[] sentences = markdown.Split('.','?','!');
		//	var lines = markdown.Replace("\r", "").Split('\n')
		//		.Where(l => !l.StartsWith("#"));
		//	System.Console.WriteLine(sentences.Length);

		//	var paras = new List<String>();
		//	var sb = new StringBuilder();
		//	foreach (String line in lines) {
		//		if (String.IsNullOrEmpty(line)) {
		//			paras.Add(sb.ToString());
		//			sb.Clear();
		//		}
		//		else { 
		//			sb.Append(line).Append(" ");
		//		}
		//	}
		//	foreach (String para in paras) {
		//		System.Console.Write("{0}: ", para.Count(c => c == '.' || c == '?' || c == '!'));
		//		foreach (String sen in para.Split('.', '?', '!')) {
		//			System.Console.Write("{0}, ", sen.Count(c => c == ' '));
		//		}
		//		System.Console.WriteLine();
		//	}
		//	System.Console.WriteLine(paras.Count);
		//}
		/// <summary>
		/// Takes a markdown file and inserts records into the db for 
		/// </summary>
		public void ParseMarkdownToFileSystem() {
			var file = new MarkdownFile(this.MarkdownOption, this.db);
			file.ParseMarkdownToFileSystem();
		}

		//public String ConvertRussianToEnglishPhonetic(String inRussian) {
		//	var dict = Helper.BuildDictionaryFromFile(@"C:\Users\bwebster\Source\Repos\notes\russian\characterPronounciationDict");
		//	StringBuilder s = new StringBuilder(inRussian);
		//	foreach (string c in dict.Keys) {
		//		s.Replace(c, dict[c] + "_");
		//	}
		//	return s.ToString();
		//}

		//public IEnumerable<String> ConvertRussianToEnglishVerbosePhonetics(String inRussian, Dictionary<String, String> dict) {
		//	List<String> retVal = new List<String>();
		//	String outword = "";
		//	StringBuilder paddedRusWord = new StringBuilder();
		//	StringBuilder phoneticWord = new StringBuilder();
		//	int padding = 1;
		//	String chunk = "";
		//	foreach (string rusWord in inRussian.Split()) {
		//		paddedRusWord = new StringBuilder();
		//		phoneticWord = new StringBuilder();
		//		foreach (string russianChar in rusWord.ToCharArray().Select(c => c.ToString())) {
		//			try {
		//				chunk = dict[russianChar] + " ";
		//			}
		//			catch (KeyNotFoundException) {
		//				chunk = russianChar + " ";
		//			}
		//			padding = Math.Max(russianChar.Length, chunk.Length);
		//			paddedRusWord.Append(russianChar.PadRight(padding));
		//			phoneticWord.Append(chunk.PadRight(padding));
		//		}
		//		outword = String.Format("{2}\n{0}\n{1}\n", paddedRusWord.ToString(), phoneticWord.ToString(), rusWord);
		//		retVal.Add(outword);
		//	}
		//	return retVal;
		//}
	}
}
