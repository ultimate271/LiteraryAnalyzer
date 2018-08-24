﻿using System;
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
		public LitOptions LO { get; set; } = new LitOptions();
		
		public Controller() : this (new LiteraryAnalyzerContext()) { }
		public Controller(LiteraryAnalyzerContext db) { this.db = db; }

		public void DeveloperDebug () {
			//var sourceInfo = new MDAnnSourceInfo() { BaseDir = @"C:\Users\brett\Source\Repos\notes\test", Prefix = "sil" };
			var sourceInfo = new MDAnnSourceInfo() {
				BaseDir = @"C:\Users\bwebster\Source\Repos\notes\source",
				Prefix = "dostoyevsky possessed.txt" };
			LO.BuildSourceFilenames = LO.BuildSourceFilenamesNovel;
			LO.BuildSourceFiles = LO.BuildSourceFilesNovel;
			LO.WriteElmText = (text) => LO.WriteTextGQQ(text, 80);
			LO.WriteAnnSource = LO.WriteAnnSourceNovel;
			LO.WriteElmLinks = (e) => { return new List<String>(); };
			LO.WriteToFileSystem = LO.WriteToFileSystemRaw;
			LO.ToLongSourceFilename = LO.ToLongFilenameRaw;
			var source = LO.BuildAnnSource(sourceInfo);
			var novel = LO.ParseAnnSource(source);
			var sourceOut = LO.WriteAnnSource(novel);
			var writeInfo = new MDAnnSourceInfo() {
				BaseDir = @"C:\Users\bwebster\Source\Repos\notes\достоевский",
				Prefix = "possessed"
			};
			sourceOut.TagFile = LO.WriteTagFile(novel, writeInfo);
			LO.WriteToFileSystem(sourceOut, writeInfo);

			System.Console.WriteLine("Done");

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
