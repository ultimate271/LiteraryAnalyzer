using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared{
	public static class Helper {
		public static Dictionary<String, String> BuildDictionaryFromFile(String URI) {
			//This function does no verification
			var retVal = new Dictionary<String, String>();
			try {
				string contents = System.IO.File.ReadAllText(URI);
				var list = contents.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string s in list) {
					var match = System.Text.RegularExpressions.Regex.Match(s, @"(.),(.*)");
					string key = match.Groups[1].Value;
					string value = match.Groups[2].Value;
					retVal.Add(key, value);
				}
			}
			catch {
				throw;
			}
			return retVal;
		}

		public static int HeaderLevel(String s) {
			var matches = System.Text.RegularExpressions.Regex.Matches(s, "^#*", 0);
			return matches[0].Length;
		}

	}
}
