using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	public class MarkdownFile {
		public String GeneratedURI { get { return String.Format("{0}\\{1}{2:D2}{3}.md", this.BaseDir, this.Prefix, this.Count, this.Filename); } }
		public String FullURI { get { return this.BaseDir.Trim('\\') + "\\" + this.Filename.Trim('\\'); } }
		//Minimum Complete Constructor
		public String Filename { get; set; }
		public String Markdown { get; set; }
		public String Prefix { get; set; }
		public String BaseDir { get; set; }
		public int Count { get; set; } //Used for the generated URI, and nothing else

		private void FetchMarkdown() {
			this.Markdown = System.IO.File.ReadAllText(this.FullURI);
		}
		

		public IEnumerable<String> ParseMarkdownForContents() {
			return Markdown
				.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Where(line => line.StartsWith("#"));
		}
		public IEnumerable<MarkdownFile> ParseMarkdown() {
			return this.ParseMarkdown(null);
		}
		public IEnumerable<MarkdownFile> ParseMarkdown (IEnumerable<String> contents) {
			try {
				this.FetchMarkdown();
			}
			catch {
				return null;
			}
			if (contents == null) {
				contents = this.ParseMarkdownForContents();
			}
			var retVal = new List<MarkdownFile>();
			String prev = null;
			foreach(String s in contents) {
				if (!String.IsNullOrEmpty(prev)) {
					int fromIndex = this.Markdown.IndexOf(prev);
					int toIndex = this.Markdown.IndexOf(s);
					var tmp = new MarkdownFile {
						Filename = prev.Trim('#', ' '),
						Markdown = this.Markdown.Substring(fromIndex, toIndex - fromIndex),
						BaseDir = this.BaseDir,
						Prefix = this.Prefix,
						Count = retVal.Count + 1
					};
					retVal.Add(tmp);
				}
				prev = s;
			}
			return retVal;
		}
		public void ParseMarkdownToDatabase(LiteraryAnalyzerContext db, String Title) {
			Excerpt Root;
			var query = db.Excerpts.Where(e => e.ExcerptText.Equals(Title));
			if (query.Count() == 0) {
				Root = new Excerpt { ExcerptText = Title, Token = db.GetTokenWithWrite("Title") };
				db.Excerpts.Add(Root);
			}
			else {
				Root = query.First();
			}

			var parents = new Stack<Excerpt>();
			parents.Push(Root);
			int currentHeaderLevel = 0;
			Excerpt currentNode = null;
			foreach (var subfile in this.ParseMarkdown()) {
				//If the ParseMarkdown command parsed correctly, every subfile should be a header with some number of # symbols, 
				//followed by text, a colon (:), and more text to a new line.
				//The rest should be content.

				//Create the excerpt with the new content, along with all of its subnodes
				currentNode = subfile.ParseExcerpt(db);
				//Figure out the header level of the current excerpt
				int hl = subfile.HeaderLevel;
				//If the current node is shallower or sibling to the current parent, pop the current parent because it will no longer parent anything from here on out
				if (hl <= currentHeaderLevel) {
					parents.Pop();
				}
				//Add the current node as child to the top parent in the stack
				//This is where the database object is very steathily written to
				parents.Peek().Children.Add(currentNode);
				//The current node is the new parent for the next iteration
				parents.Push(currentNode);
				currentHeaderLevel = hl;
			}
		}
	
		public String ParseContent() {
			var textLines = this.Markdown.Split(new String[] { "\r\n" }, 0).Where(s => !s.StartsWith("#"));
			return String.Join("\r\n", textLines);
		}
		public Excerpt ParseExcerpt(LiteraryAnalyzerContext db) {
			String headerLine = this.Markdown.Split(new String[] { "\r\n" }, 0).FirstOrDefault();
			var matches = System.Text.RegularExpressions.Regex.Matches(headerLine, "^#*([^:]*):([^\n]*)");
			bool hasColon = true;
			if (matches.Count == 0) {
				hasColon = false;
				matches = System.Text.RegularExpressions.Regex.Matches(headerLine, "^#*([^\n]*)");
			}
			//Get the token from the header
			String TokenKey = hasColon ? matches[0].Groups[1].Value.Trim() : "Section";
			//Get the Token object from the database
			Token excerptToken = db.GetTokenWithWrite(TokenKey);

			String Text = hasColon ? matches[0].Groups[2].Value.Trim() : matches[0].Groups[1].Value.Trim();
			String contentText = this.ParseContent().Trim();
			Excerpt contentExcerpt =
				contentText.Length > 0
				? new Excerpt {
					ExcerptText = this.ParseContent(),
					Token = db.GetTokenWithWrite("Content")
				}
				: null;
			return new Excerpt {
				ExcerptText = Text,
				Token = excerptToken,
				Children = contentExcerpt == null ? new List<Excerpt>() : new List<Excerpt>(new Excerpt[] { contentExcerpt })
			};
		}
		public int HeaderLevel {
			get {
				//This voodoo magic line should return the number of hashes at the start of the markdown
				var matches = System.Text.RegularExpressions.Regex.Matches(this.Markdown, "^(#*)", 0);
				return matches[0].Groups[0].Length;
			}
		}
		public void PrintFile() {
			System.IO.File.WriteAllText(this.GeneratedURI, this.Markdown);
		}
	}
}
