using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteraryAnalyzer.LAShared;

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
			Controller c = new Controller();
			c.DeveloperDebug();
			if (args.Length > 0) {
				var info = new MDAnnSourceInfo() {
					BaseDir = System.IO.Directory.GetCurrentDirectory(),
					Prefix = args[0]
				};
				c.GenerateTags(info);
			}
			//Instructions for use.
			//Replace Filename with the source txt, annotated with markdown
			//Replace Prefix with where you want the output to be saved
			//BaseDir most likely stays intact as is

			//var option = new MarkdownOption {
			//	ContentsOption = MarkdownOption.ContentsOptions.Default,
			//	ParserOption = MarkdownOption.ParserOptions.Default,
			//	URIOption = MarkdownOption.URIOptions.ShortStory,
			//	Filename = "source\\shakespeare complete.txt",
			//	BaseDir = @"C:\Users\bwebster\Source\Repos\notes",
			//	Prefix = "shakespeare\\",
			//};

			//LitHeader myHeader = new LitHeader { Text = "Brothers" };
			//var ret = myHeader.ParseHeaderToModel(System.IO.File.ReadAllText(@"C:\Users\bwebster\Source\Repos\notes\dastoyevsky\brothers02.08.md"));
			//var db = new LiteraryAnalyzerContext();
			//db.Litelms.Add(myHeader);
			//db.Litelms.AddRange(ret.Footnotes);
			//db.SaveChanges();

			//c.MarkdownOption = option;
			//c.ParseMarkdownToFileSystem();
			//var myDict = Helper.BuildDictionaryFromFile(@"C:\Users\bwebster\Source\Repos\notes\russian\characterPronounciationDict");
			//foreach (string ch in myDict.Keys) {
			//	System.Console.WriteLine("{0},{1}", ch, myDict[ch]);
			//}
			//string infile = @"C:\Users\bwebster\Source\Repos\notes\toy\in";
			//string outfile = @"C:\Users\bwebster\Source\Repos\notes\toy\out";
			//string inRussian = System.IO.File.ReadAllText(infile);
			//var outList = c.ConvertRussianToEnglishVerbosePhonetics(inRussian, myDict);
			//System.IO.File.WriteAllText(outfile, String.Join("\n", outList));

			//string phonetic = c.ConvertRussianToEnglishPhonetic(inRussian);
			//StringBuilder sb = new StringBuilder();
			//var query = inRussian.Split().Zip(phonetic.Split(), (a, b) => String.Format("{0,20} | {1}", a, b));
			//foreach (String s in query) {
			//	sb.AppendLine(s.Replace('_', ' '));
			//}
			//System.IO.File.WriteAllText(outfile, sb.ToString());
			//return;
			//Handcraft a contents file, with one line for each markdown header
			//In the source itself, each header should have a newline follow by some number of # symbols, then a filename identifier
			//The contents file should be a list of these identifiers, exactly as they appear on the line in source
			//If shit doesn't match, exceptions will get thrown
			//Replace the contentsURI with your contents file
			//If the file can be parsed for markdown style headers, set this to null
			//IEnumerable<String> contents = null;//System.IO.File.ReadLines(@"C:\Users\Brett\Source\Repos\notes\cline\rpo00Contents.md");

			//********* DO NOT EDIT ANYTHING BELOW THIS LINE ***********
			//foreach (var mdfile in source.ParseMarkdown(contents)) {
			//	mdfile.PrintFile();
			//}
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
