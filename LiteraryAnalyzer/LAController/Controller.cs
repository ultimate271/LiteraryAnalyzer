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
		private LitOptions LO { get; set; } = new LitOptions();
		
		public Controller() : this (new LiteraryAnalyzerContext()) { }
		public Controller(LiteraryAnalyzerContext db) { this.db = db; }

		public LitElm CreateElm(String Header) {
			return new LitElm { Header = Header };
		}
		public void SeparateNovel(MDAnnSourceInfo infoIn, MDAnnSourceInfo infoOut) {
			this.SeparateNovel(infoIn, infoOut, LitOptionsFactory.CreateSourceNovel());
		}
		public void SeparateNovel(MDAnnSourceInfo infoIn, MDAnnSourceInfo infoOut, LitOptions LO) {
			var source = LO.BuildAnnSource(infoIn);
			var novel = LO.ParseAnnSource(source);
			var sourceOut = LO.WriteAnnSource(novel);
			sourceOut.TagFile = LO.WriteTagFile(novel, infoOut);
			LO.WriteToFileSystem(sourceOut, infoOut);
		}
		public MDAnnSource DeveloperDebugTwo() {
			LO = LitOptionsFactory.CreateDefault();
			var sourceInfo = new MDAnnSourceInfo() {
				BaseDir = @"C:\Users\brett\Source\Repos\notes\достоевский\possessed",
				Prefix = "possessed" };
			return LO.BuildAnnSource(sourceInfo);
		}
		public LitNovel DeveloperDebug () {
			LO = LitOptionsFactory.CreateDefault();
			var sourceInfo = new MDAnnSourceInfo() {
				BaseDir = @"C:\Users\brett\Source\Repos\notes\достоевский\possessed",
				Prefix = "possessed" };
			var source = LO.BuildAnnSource(sourceInfo);
			return LO.ParseAnnSource(source);

			//var sourceInfo = new MDAnnSourceInfo() { BaseDir = @"C:\Users\brett\Source\Repos\notes\test", Prefix = "sil" };
			//var sourceInfo = new MDAnnSourceInfo() {
			//	BaseDir = @"C:\Users\bwebster\Source\Repos\notes\source",
			//	Prefix = "plato euthyphro.txt" };
			//LO = LitOptionsFactory.CreateSourceNovel();
			//var source = LO.BuildAnnSource(sourceInfo);
			//var novel = LO.ParseAnnSource(source);
			//var sourceOut = LO.WriteAnnSource(novel);
			//var writeInfo = new MDAnnSourceInfo() {
			//	BaseDir = @"C:\Users\bwebster\Source\Repos\notes\plato",
			//	Prefix = "Euthyphro"
			//};
			//sourceOut.TagFile = LO.WriteTagFile(novel, writeInfo);
			//LO.WriteToFileSystem(sourceOut, writeInfo);

			//System.Console.WriteLine("Done");

		}
		public String ToRawTextbox(IEnumerable<String> s) {
			return LO.ToRawSourceLinesDefault(s);
		}
		public void GenerateTags(MDAnnSourceInfo info) {
			LO = LitOptionsFactory.CreateDefault();
			var source = LO.BuildAnnSource(info);
			var novel = LO.ParseAnnSource(source);
			var sourceOut = LO.WriteAnnSource(novel);
			sourceOut.TagFile = LO.WriteTagFile(novel, info);
			LO.WriteToFileSystem(sourceOut, info);
		}
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
