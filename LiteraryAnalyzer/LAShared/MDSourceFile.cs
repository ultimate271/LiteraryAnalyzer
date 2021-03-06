﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class MDSourceFile : MDFile {
		public LitSceneMetadata Metadata { get; set; } = new LitSceneMetadata();
		public LitAuthor Author { get; set; } = new LitAuthor();
	}
	public static partial class ParsingTools {
		public static String ToRawSourceLinesDefault(
			this LitOptions LO,
			IEnumerable<String> Lines
		) {
			var retVal = new StringBuilder();
			foreach (var line in Lines) {
				if (LO.IsSourceLine(line)) {
					retVal.AppendLine(line);
				}
			}
			return retVal.ToString();
		}
		public static String ToLongFilenameDefault(
			this LitOptions LO, 
			MDAnnSourceInfo info,
			MDSourceFile source
		) {
			return String.Format("{0}\\{1}", info.BaseDir, ToShortFilenameDefault(LO, info, source));
		}
		public static String ToLongFilenameRaw(
			this LitOptions LO,
			MDAnnSourceInfo info,
			MDSourceFile source
		) {
			return String.Format("{0}\\{1}\\{2}",
				info.BaseDir,
				info.Prefix,
				LO.ToShortFilenameDefault(info, source)
			);
		}
		public static String ToShortFilenameDefault(
			this LitOptions LO, 
			MDAnnSourceInfo info, 
			MDSourceFile source
		){
			return ToShortFilenameDefault(
				LO,
				info, 
				source.Metadata, 
				source.Author
			);
		}
		public static String ToShortFilenameDefault (
			this LitOptions LO,
			MDAnnSourceInfo info,
			LitSceneMetadata metadata,
			LitAuthor author
		){
			return ToShortFilenameDefault(
				LO, 
				info.Prefix, 
				metadata.Descriptor, 
				author.Author
			);
		}
		public static String ToShortFilenameDefault (
			this LitOptions LO,
			String Prefix,
			String descriptor,
			String author
		){
			return String.Format("{0}{1}.{2}.md", Prefix, descriptor, author);
		}
		public static void ParseSourceFileDefault(
			this LitOptions LO,
			LitNovel novel, 
			MDSourceFile sourceFile
		){
			var PartitionedElms = LO.ExtractFromSourceFile(sourceFile);

			//Extract and add the metadata
			var MetadataLines = LO.ExtractMetadata(PartitionedElms);
			var LitSceneMetadata = LO.ParseMetadata(novel, MetadataLines);
			var Author = LO.ParseAuthor(novel, MetadataLines);

			//Extract and add the scenes
			var PartitionedElmLines = LO.ExtractElms(PartitionedElms);
			foreach (var ElmLines in PartitionedElmLines) {
				var scene = LO.ParseToElm(novel, LitSceneMetadata, Author, ElmLines);
				novel.AddScene(scene);
			}
		}
		public static MDSourceFile WriteSourceFileDefault(
			this LitOptions LO,
			LitNovel novel,
			LitSceneMetadata metadata,
			LitAuthor author
		) {
					//Write all of the lines of the file
			var lines = LO.WriteMetadata(metadata, author);
			var query = novel.Scenes
				.Where(s => s.Metadata == metadata)
				.Select(s => LO.WriteElmSourceLines(s, author));
			foreach (var scenelines in query) {
				lines.AddRange(scenelines);
			}
			//Create the file
			var SourceFile = new MDSourceFile() {
				Metadata = metadata,
				Author = author,
				Lines = lines
			};
			return SourceFile;
		}
		public static IEnumerable<IEnumerable<String>> ExtractFromSourceFileDefault(
			this LitOptions LO, 
			MDSourceFile sourcefile
		){
			return ParsingTools.PartitionLines(
				sourcefile.Lines, 
				line => System.Text.RegularExpressions.Regex.IsMatch(line, @"^#[^#]")
			);
		}
		public static void TagSourceFileDefault(
			this LitOptions LO,
			MDSourceFile sourcefile
		){
			sourcefile.Lines = new List<string>(
				TagSourceFileDefault(
					LO,
					sourcefile.Lines, 
					sourcefile.Metadata.Descriptor,
					sourcefile.Author.Author
				)
			);
		}
		public static List<String> TagSourceFileDefault(this LitOptions LO, IEnumerable<String> lines, String tag, String author) {
			return TagSourceFileDefault(LO, lines, tag, author, 1);
		}
		public static List<String> TagSourceFileDefault (this LitOptions LO, IEnumerable<String> lines, String tag, String author, int headerLevel) {
			var retVal = new List<String>();
			var arg = new List<String>();
			//First remove the existing tags
			var query = lines.Where(s => {
				var linkLine = LO.ParseLink(s);
				return linkLine == null || !ParsingTools.GenereratedLinks.Contains(linkLine.Link);
			} );
			int i = headerLevel == 1 ? -1 : 0;
			bool adding = true;
			foreach (var line in query) {
				var pattern = @"^(#+)[^#]";
				var match = System.Text.RegularExpressions.Regex.Match(line, pattern);
				if (match.Success) {
					int lineHeaderLevel = match.Groups[1].Length;
					if (i < 0) { //i should only ever equal 0 if this is the metadata scene, and it's the first scene of the file
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[Metadata]: # {{{0}}}", tag));
						retVal.Add(String.Format(@"[Descriptor]: # {{{0}}}", tag));
						//TODO: Perhaps make this more elegant
						if (lines
							.Select(l => LO.ParseLink(l))
							.Where(l => l != null)
							.Where(l => l.Link.Equals("Author"))
							.Count() == 0
						){
							retVal.Add(String.Format(@"[Author]: # {{{0}}}", author));
						}
					}
					else if (lineHeaderLevel == headerLevel && adding) {
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[TreeTag]: # {{{0}.{1:00}}}", tag, i).TrimStart('.'));
					}
					else if (lineHeaderLevel > headerLevel) {
						adding = false;
						arg.Add(line);
					}
					else if (lineHeaderLevel == headerLevel && !adding) {
						//Recursively call the lines we've gathered together, tag them, and add the range
						retVal.AddRange(TagSourceFileDefault(LO, arg, String.Format(@"{0}.{1:00}", tag, i).TrimStart('.'), author, headerLevel + 1));

						//Begin anew
						arg = new List<string>();
						adding = true;

						//Start with this header line
						i++;
						retVal.Add(line);
						retVal.Add(String.Format(@"[TreeTag]: # {{{0}.{1:00}}}", tag, i).TrimStart('.'));
					}
				}
				else { //If this is not a header line, and just a regular line
					if (adding) {
						retVal.Add(line);
					}
					else {
						arg.Add(line);
					}
				}
			}
			if (arg.Count > 0) {
				retVal.AddRange(TagSourceFileDefault(LO, arg, String.Format(@"{0}.{1:00}", tag, i), author, headerLevel + 1));
			}
			return retVal;
		}
		/// <summary>
		/// Depricated
		/// </summary>
		/// <param name="source"></param>
		/// <param name="novel"></param>
		//public static void ParseLitSourceInfo(this MDSourceFile source, LitNovel novel) {
		//	var litSourceInfo = new LitSourceInfo();
		//	var query = source.Lines.Select(s => ParsingTools.ParseLink(s))
		//		.Where(l => l != null && l.Link.Equals("Author"));
		//	if (query.Count() > 0) {
		//		litSourceInfo.Author = query.First().Tag;
		//	}
		//	//source.LitSourceInfo = novel.AddSourceInfoDistinct(litSourceInfo);
		//}
	}
}
