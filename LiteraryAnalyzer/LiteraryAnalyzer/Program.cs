﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	//struct LitFile {
	//	public String Filename { get { return String.Format(@"C:\Users\Brett\Source\Repos\notes\martin\storm{0:D2}{1}{2:D2}.md", ChapterNumber, Title, Iteration); } }
	//	public String Title { get; set; }
	//	public int Iteration { get; set; }
	//	public int ChapterNumber { get; set; }
	//	public String Contents { get; set; }

	//}
	class Program {
		static void Main(string[] args) {
			//Instructions for use.
			//Replace Filename with the source txt, annotated with markdown
			//Replace Prefix with where you want the output to be saved
			//BaseDir most likely stays intact as is
			var source = new MarkdownFile {
				Filename = "source\\1984 - George Orwel.txt",
				BaseDir = @"C:\Users\Brett\Source\Repos\notes",
				Prefix = "orwell\\nineteen",
				Count = 0
			};
			//Handcraft a contents file, with one line for each markdown header
			//In the source itself, each header should have a newline follow by some number of # symbols, then a filename identifier
			//The contents file should be a list of these identifiers, exactly as they appear on the line in source
			//If shit doesn't match, exceptions will get thrown
			//Replace the contentsURI with your contents file
			//If the file can be parsed for markdown style headers, set this to null
			IEnumerable<String> contents = null;//System.IO.File.ReadLines(@"C:\Users\Brett\Source\Repos\notes\cline\rpo00Contents.md");

			//********* DO NOT EDIT ANYTHING BELOW THIS LINE ***********
			foreach (var mdfile in source.ParseMarkdown(contents)) {
				mdfile.PrintFile();
			}
			////************************ A Storm of Swords : Specialized Parse *****************************
			//String source = System.IO.File.ReadAllText(@"C:\Users\Brett\Source\Repos\notes\source\StormOfSwords.txt");
			//List<String> toc = new List<string>(new String[]{ "Prologue", "Jaime", "Catelyn", "Arya", "Tyrion", "Davos", "Sansa", "Jon", "Daenerys", "Bran", "Davos", "Jaime", "Tyrion", "Arya", "Catelyn", "Jon", "Sansa", "Arya", "Samwell", "Tyrion", "Catelyn", "Jaime", "Arya", "Daenerys", "Bran", "Davos", "Jon", "Daenerys", "Sansa", "Arya", "Jon", "Jaime", "Tyrion", "Samwell", "Arya", "Catelyn", "Davos", "Jaime", "Tyrion", "Arya", "Bran", "Jon", "Daenerys", "Arya", "Jaime", "Catelyn", "Samwell", "Arya", "Jon", "Catelyn", "Arya", "Catelyn", "Arya", "Tyrion", "Davos", "Jon", "Bran", "Daenerys", "Tyrion", "Sansa", "Tyrion", "Sansa", "Jaime", "Davos", "Jon", "Arya", "Tyrion", "Jaime", "Sansa", "Jon", "Tyrion", "Daenerys", "Jaime", "Jon", "Arya", "Samwell", "Jon", "Tyrion", "Samwell", "Jon", "Sansa", "Epilogue", "Appendix" });
			//List<LitFile> files = new List<LitFile>();

			//int fromIndex = 2475;
			//int toIndex = fromIndex;

			//String prev = null;
			//foreach (String title in toc) {
			//	if (prev != null) {
			//		fromIndex = source.IndexOf("\r\n" + prev + "\r\n", fromIndex);
			//		toIndex = source.IndexOf("\r\n" + title + "\r\n", fromIndex);
			//		String contents = source.Substring(fromIndex, toIndex - fromIndex);
			//		contents = contents.Replace('\x201c', '"');
			//		contents = contents.Replace('\x201d', '"');
			//		contents = contents.Replace('\x2014', '-');
			//		contents = contents.Replace("\x2026", "...");
			//		files.Add(new LitFile { Title = prev, Contents = contents, Iteration = files.Count(l => l.Title == prev) + 1, ChapterNumber = files.Count + 1 });
			//	}
			//	prev = title;
			//}
			//foreach (LitFile file in files) {
			//	System.IO.File.WriteAllText(file.Filename, file.Contents);
			//}
			//********************************** Fun with db stuff *************************************
			//System.Console.WriteLine("Some words go here");
			//using (var db = new LiteraryAnalyzerContext()) {
			//	foreach (Excerpt e in db.Excerpts.OrderBy(e => e.ExcerptID)) {
			//		System.Console.WriteLine(e.ExcerptText);
			//	}
			//	System.Console.ReadLine();
			//}
			//using (var db = new LiteraryAnalyzerContext()) {
			//	var root = new Excerpt { ExcerptText = "Root" };
			//	db.Excerpts.Add(root);
			//	db.SaveChanges();
			//}
		}
	}
}
