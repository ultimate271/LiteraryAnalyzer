using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitOptionsFactory {
		public static LitOptions CreateDefault() {
			return new LitOptions();
		}
		public static LitOptions CreateSourceNovel() {
			var LO = new LitOptions();
			LO.BuildSourceFilenames = LO.BuildSourceFilenamesNovel;
			LO.BuildSourceFiles = LO.BuildSourceFilesNovel;
			LO.BuildNotesFile = (info, files) => new MDNotesFile();
			//LO.WriteElmText = LO.WriteElmTextDefault;
			LO.WriteElmText = (text) => LO.WriteTextGQQ(text, 80);
			LO.WriteAnnSource = LO.WriteAnnSourceNovel;
			LO.WriteElmLinks = (e) => { return new List<String>(); };
			LO.WriteToFileSystem = LO.WriteToFileSystemRaw;
			LO.ToLongSourceFilename = LO.ToLongFilenameRaw;
			return LO;
		}
		public static LitOptions CreateShakespearePlay() {
			var LO = LitOptionsFactory.CreateSourceNovel();
			LO.WriteElmText = LO.WriteTextIdentity;
			LO.SourceLinesToString = LO.SourceLinesToStringIdentity;
			return LO;
		}
	}
}
