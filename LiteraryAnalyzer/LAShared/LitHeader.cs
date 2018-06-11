using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// The ID of this class is irrelevent, but the text will represent the text given after the space after the last hash sign at the beginning of the line
	/// Children will of course be parsed recursively
	/// A note about header levels, there is no such thing. The only thing header level is used for is to determine if a header is above or below the current header in the hiarchy, nothing more
	/// So a header level like 2324 would translate to 1212 if you understand what I am meaning
	/// </summary>
	public class LitHeader : Litelm{
		public List<Litelm> Children { get; set; } = new List<Litelm>();
	}
	public static partial class LitExtensions {
		public static void ParseHeader(this LitHeader parent, String s) {
			if (String.IsNullOrEmpty(s)) {
				return;
			}
			if (parent == null) {
				return;
			}

			var lines = s
				.Split(new String[] { "\r\n" }, StringSplitOptions.None)
				.Select(line => new {
					Line = line,
					HeaderLevel = Helper.HeaderLevel(line)
				});

			var Source = new List<string>();
			int currentLevel = 0;
			LitHeader subHeader = null;

			foreach (var line in lines) {
				if (line.HeaderLevel > 0 && line.HeaderLevel <= currentLevel && subHeader != null) {
					subHeader.ParseHeader(String.Join("\r\n", Source));
					parent.Children.Add(subHeader);
					subHeader = new LitHeader { Text = line.Line.Trim('#', ' ') };
					Source = new List<string>();
					currentLevel = line.HeaderLevel;
				}
				else if (line.HeaderLevel == 0 || line.HeaderLevel > currentLevel) {
					Source.Add(line.Line);
				}
				else if (line.HeaderLevel > 0 && line.HeaderLevel > currentLevel) {
					if (subHeader == null) { 
						var sourceObj = String.Join("\r\n", Source).ParseSource();
						if (sourceObj != null) {
							parent.Children.Add(sourceObj);
						}
						subHeader = new LitHeader { Text = line.Line.Trim('#', ' ') };
						Source = new List<string>();
						currentLevel = line.HeaderLevel;
					}
				}
			}
			if (subHeader != null) {
				subHeader.ParseHeader(String.Join("\r\n", Source));
				parent.Children.Add(subHeader);
			}
			else {
				var source = String.Join("\r\n", Source).ParseSource();
				if (source != null) {
					parent.Children.Add(source);
				}
			}
		}
		public static LitSource ParseSource(this String s) {
			if (String.IsNullOrWhiteSpace(s)) {
				return null;
			}
			return new LitSource { Text = s };
		}
	}
}
