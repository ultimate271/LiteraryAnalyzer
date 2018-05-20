﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	public class MarkdownFile {
		public LiteraryAnalyzerContext db { get; set; } = null;
		public MarkdownOption MarkdownOptions {
			get { return _MarkdownOptions; }
			set {
				_MarkdownOptions = value;
				this.CopyOptions(value);
			}
		}
		private MarkdownOption _MarkdownOptions = new MarkdownOption();

		/// <summary>
		/// This is not a copy constructor, it only copies the settings and such
		/// </summary>
		/// <param name="other"></param>
		public MarkdownFile(MarkdownFile other) : this(other.MarkdownOptions, other.db) { }
		public MarkdownFile() : this(null, null) { }
		public MarkdownFile(MarkdownOption options) : this(options, null) { }
		public MarkdownFile(MarkdownOption options, LiteraryAnalyzerContext db) {
			this.db = db ?? new LiteraryAnalyzerContext();
			this.MarkdownOptions = options ?? new MarkdownOption();
		}
		//Guide to use
		//This function uses reflection to make the process of adding a new Write Option
		//as seamless as possible, as long as everything is named correctly,
		//And for every Proptery and Function that is named correctly,
		//A new write option is automatically created through the magic of reflection.

		//As such, these are the naming conventions, with items in <brackets> indicating
		//A tag for my little reflection parser here to decode.
		/********************************************************************************/
		//There will be two types for each option, which will correspond to the return
		//types on the delegate for the options. Call these

		//<returntype> and <fromtype>
		//<Tag> and <Option> coorespond to what is being generated (e.g. "URI") and an Enum token (e.g. Default, Standard, etc.)

		//For each generated option, create the following delegates, methods and properties.

		//public <returntype> Generate<Tag> { get {return this.<Tag>Generator(this); } }
		//public <Tag>Options <Tag>Option { get {return this.MarkdownOptions.<Tag>Option; } set { this.ReflectionMadness(value, "<Tag>"); this.MarkdownOptions.<Tag>Option = value; } }
		//public enum <Tag>Options { Default,<Option>,<Option>,...}
		//private delegate <returntype> <Tag>GeneratorDelegate(<fromtype> x);
		//private <Tag>GeneratorDelegate <Tag>Generator { get; set; } = Default<Tag>Generator
		//private static <Option><Tag>Generator = (m) => {return ...}

		//Also, make sure for each new tag you add to add a property called <Tag>Option in MarkdownOptions

#region "URI GENERATION"
		public String GenerateURI { get { return this.URIGenerator(this); } }
		public URIOptions URIOption { get { return this.MarkdownOptions.URIOption; } set { this.ReflectionMadness(value, "URI"); this.MarkdownOptions.URIOption = value; } }

		public enum URIOptions {
			Default,
			Standard,
			Novel,
			Full
		}

		private delegate String URIGeneratorDelegate(MarkdownFile file);
		private URIGeneratorDelegate URIGenerator { get; set; } = DefaultURIGenerator;

		private static URIGeneratorDelegate DefaultURIGenerator = (m) => {
			return String.Format("{0}\\{1}", m.BaseDir, m.Filename);
		};
		private static URIGeneratorDelegate StandardURIGenerator = (m) => { 
			return String.Format("{0}\\{1}{2:D2}{3}.md", m.BaseDir, m.Prefix, m.Count, m.Filename);
		};
		private static URIGeneratorDelegate NovelURIGenerator = (m) => {
			return String.Format("{0}\\{1}{2:D2}.{3:D2}.md", m.BaseDir, m.Prefix, m.Section, m.Chapter);
		};
		private static URIGeneratorDelegate FullURIGenerator = (m) => {
			return String.Format("{0}\\{1}", m.BaseDir, m.Filename);
		};


		//Fields used in URI Generation
		public String BaseDir { get; set; } //This is the root of the directory, and should be the same directory the .git file is in
		public String Filename { get; set; } //This should include a string to indicate which file this is
		public String Prefix { get; set; } //This is the directory that the parsed markdown will go into
		public String Markdown { get; set; } //This is the actual text of the file.
		public int Count { get; set; } //Used to increment the count
		public int Section { get; set; } //Used for novel URI Generation
		public int Chapter { get; set; } //Used in novel URI Generation
#endregion

#region "CONTENTS GENERATION"
		public IEnumerable<String> GenerateContents { get { return this.ContentsGenerator(this); } }
		public ContentsOptions ContentsOption { set { this.ReflectionMadness(value, "Contents"); } }

		private delegate IEnumerable<String> ContentsGeneratorDelegate(MarkdownFile file);
		private ContentsGeneratorDelegate ContentsGenerator { get; set; } = DefaultContentsGenerator;

		public enum ContentsOptions {
			Default,
			Novel
		}

		private static ContentsGeneratorDelegate DefaultContentsGenerator = (m) => {
			return m.Markdown
				.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Where(line => line.StartsWith("#"));
		};
		private static ContentsGeneratorDelegate NovelContentsGenerator = (m) => {
			return m.Markdown
				.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Where(line => line.StartsWith("#"));
		};
#endregion

#region "PARSER GENERATION"
		public IEnumerable<MarkdownFile> GenerateParser { get { return this.ParserGenerator(this); } }
		public ParserOptions ParserOption { set { this.ReflectionMadness(value, "Parser"); } }

		private delegate IEnumerable<MarkdownFile> ParserGeneratorDelegate(MarkdownFile file);
		private ParserGeneratorDelegate ParserGenerator { get; set; } = DefaultParserGenerator;

		public enum ParserOptions {
			Default,
			Novel
		}

		private static ParserGeneratorDelegate DefaultParserGenerator = (m) => {
			var retVal = new List<MarkdownFile>();
			m.FetchMarkdown();
			var contents = m.GenerateContents;
			String prev = null;
			int fromIndex = 0;
			int toIndex = 0;
			foreach (String s in contents) {
				if (!String.IsNullOrEmpty(prev)) {
					fromIndex = m.Markdown.IndexOf(prev, toIndex);
					toIndex = m.Markdown.IndexOf(s, fromIndex);
					var tmp = new MarkdownFile(m) {
						Filename = "",
						Markdown = m.Markdown.Substring(fromIndex, toIndex - fromIndex),
						BaseDir = m.BaseDir,
						Prefix = m.Prefix,
						Count = retVal.Count + 1
					};
					retVal.Add(tmp);
				}
				prev = s;
			}
			return retVal;
		};

		private static ParserGeneratorDelegate NovelParserGenerator = (m) => {
			var retVal = new List<MarkdownFile>();
			m.FetchMarkdown();
			var contents = m.GenerateContents;
			if (contents.Select(s => MarkdownFile.HeaderLevel(s)).Where(i => i < 1 || i > 2).Count() > 0) {
				throw new Exception("Invalid Novel Markdown, contains invalid headers");
			}
			String prev = null;
			int Section = 0;
			int Chapter = 0;
			int fromIndex = 0;
			int toIndex = 0;
			foreach (String s in contents) {
				if (!String.IsNullOrEmpty(prev) && MarkdownFile.HeaderLevel(prev) == 2) {
					fromIndex = m.Markdown.IndexOf(prev, toIndex);
					toIndex = m.Markdown.IndexOf(s, fromIndex);
					var tmp = new MarkdownFile(m) {
						Filename = "",
						Markdown = m.Markdown.Substring(fromIndex, toIndex - fromIndex),
						BaseDir = m.BaseDir,
						Prefix = m.Prefix,
						Section = Section,
						Chapter = Chapter
					};
					retVal.Add(tmp);
				}
				prev = s;
				if (MarkdownFile.HeaderLevel(s) == 1) {
					Section++;
					Chapter = 0;
				}
				else if (MarkdownFile.HeaderLevel(s) == 2) {
					Chapter++;
				}
			}
			return retVal;
		};

#endregion

#region "PUBLIC METHODS"
		public void ParseMarkdownToFileSystem() {
			foreach (var mdFile in this.GenerateParser) {
				mdFile.PrintFile();
			}
		}
		public void ParseMarkdownToDatabase() {
			Excerpt Root;
			var query = db.Excerpts.Where(e => e.ExcerptText.Equals(this.Prefix));
			if (query.Count() == 0) {
				Root = new Excerpt { ExcerptText = this.Prefix, Token = db.GetTokenWithWrite("Title") };
				db.Excerpts.Add(Root);
			}
			else {
				Root = query.First();
			}

			var parents = new Stack<Excerpt>();
			parents.Push(Root);
			int currentHeaderLevel = 0;
			Excerpt currentNode = null;
			foreach (var subfile in this.GenerateParser) {
				//If the ParseMarkdown command parsed correctly, every subfile should be a header with some number of # symbols, 
				//followed by text, a colon (:), and more text to a new line.
				//The rest should be content.

				//Create the excerpt with the new content, along with all of its subnodes
				currentNode = subfile.ParseExcerpt(db);
				//Figure out the header level of the current excerpt
				int hl = subfile.dep_HeaderLevel;
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
#endregion

#region "PRIVATE METHODS"
		private void PrintFile() {
			System.IO.File.WriteAllText(this.GenerateURI, this.Markdown);
		}
		private void FetchMarkdown() {
			try {
				this.Markdown = System.IO.File.ReadAllText(MarkdownFile.FullURIGenerator(this));
			}
			catch (Exception e) {
				this.db?.ExceptionLogs.Add(new ExceptionLog(e));
				this.Markdown = "";
			}
		}
		private String ParseContent() {
			var textLines = this.Markdown.Split(new String[] { "\r\n" }, 0).Where(s => !s.StartsWith("#"));
			return String.Join("\r\n", textLines);
		}
		private Excerpt ParseExcerpt(LiteraryAnalyzerContext db) {
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
		private static int HeaderLevel(String s) {
			var matches = System.Text.RegularExpressions.Regex.Matches(s, "^#*", 0);
			return matches[0].Length;
		}
		private int dep_HeaderLevel {
			get {
				var matches = System.Text.RegularExpressions.Regex.Matches(this.Markdown, "^(#*)", 0);
				return matches[0].Groups[0].Length;
			}
		}

		/// <summary>
		/// As the name of the method may suggest, there is some reflection madness that happens here.
		/// Edit at your own risk.
		/// You have been warned.
		/// 
		/// Seriously, why are you still reading this?
		/// This shit is some voodoo magic okay,
		/// Unless you know how to play with voodoo, don't touch this.
		/// Like, just don't.
		/// 
		/// Leave.
		/// 
		/// Stop reading.
		/// 
		/// If you've made it this far, it means you don't know what you are doing and shouldn't edit this function.
		/// Boom
		/// Shots fired.
		/// 
		/// Get fukd nerd
		/// 
		/// So leave now and leave the real programming to the programmers
		/// </summary>
		/// <param name="value"></param>
		/// <param name="s"></param>
		private void ReflectionMadness(Object value, String OptionName) {
			String Tag = value.ToString();
			String lsGeneratedProperty = "Generated" + OptionName;
			String lsDelegateDeclaration = OptionName + "GeneratorDelegate";
			String lsInstanceDelegate = OptionName + "Generator";
			String lsEnumType = OptionName + "Options";
			String lsOptionLambda = Tag + OptionName + "Generator";
			String lsOptionProperty = OptionName + "Option";

			var Default =  (this.GetType().GetMember(lsEnumType).First() as System.Reflection.TypeInfo).DeclaredMembers.Where(m => m.Name == "Default");
            var InstanceDelegate = this.GetType().GetProperty(lsInstanceDelegate,
				System.Reflection.BindingFlags.Instance
				| System.Reflection.BindingFlags.NonPublic
			);
			var OptionLambda = this.GetType().GetField(lsOptionLambda,
				System.Reflection.BindingFlags.Static
				| System.Reflection.BindingFlags.NonPublic
			);
			if (InstanceDelegate != null) {
				if (OptionLambda != null) {
					InstanceDelegate.SetMethod.Invoke(this, new Object[] { OptionLambda.GetValue(this) } );
				}
				else {
					try {
						if (Default == null) { throw new Exception(String.Format("No Default for {0}", OptionName)); }
						ReflectionMadness(Default, OptionName);
					}
					catch (Exception e) {
						this.db?.ExceptionLogs.Add(new ExceptionLog(e, "No Default"));
					}
				}
			}
			else {
				try {
					throw new Exception(String.Format("No Instance Delegate for {0}", OptionName));
				}
				catch (Exception e) {
					this.db?.ExceptionLogs.Add(new ExceptionLog(e, "Instance Delegate not Found"));
				}
			}
		}

		private void CopyOptions(object value) {
			foreach (var m in value.GetType().GetProperties()) {
				try {
					this.GetType().GetProperty(m.Name).SetMethod.Invoke(this, new Object[] { m.GetValue(_MarkdownOptions, null) });
				}
				catch (Exception e){
					this.db?.ExceptionLogs.Add(new ExceptionLog(e, "Option not stored in MarkdownOptions"));
				}
			}
		}
#endregion
	}
}
		