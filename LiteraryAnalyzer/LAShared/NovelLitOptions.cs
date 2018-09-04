using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public static partial class ParsingTools {
		public static IEnumerable<String> BuildSourceFilenamesNovel(
			this LitOptions LO,
			MDAnnSourceInfo info
		) {
			return new String[] { String.Format("{0}\\{1}", info.BaseDir, info.Prefix) };
		}
		public static List<MDSourceFile> BuildSourceFilesNovel(
			this LitOptions LO,
			MDAnnSourceInfo info,
			IEnumerable<String> filenames
		) {
			return new List<MDSourceFile>(new MDSourceFile[] {
				new MDSourceFile() {
					Lines = new List<String>(
						System.IO.File.ReadAllLines(filenames.First())
					)
				}
			});
		}
		public static MDAnnSource WriteAnnSourceNovel(
			this LitOptions LO,
			LitNovel novel
		){
			var retVal = new MDAnnSource();
			var list = new List<MDSourceFile>();
			foreach (var scene in novel.Scenes) {
				list.AddRange(
					WriteSourceFileNovel(
						LO,
						new[] { scene.Header },
						new String [] { },
						scene,
						novel.Authors.First()
					)
				);
			}
			retVal.Sources = list;
			return retVal;
		}

		//TODO
		//Make a LitOptionsFactory
		public static List<MDSourceFile> WriteSourceFileNovel(
			this LitOptions LO,
			IEnumerable<String> HeaderAcc,
			IEnumerable<String> MetadataAcc,
			LitElm sourceElm,
			LitAuthor author
		){
			var retVal = new List<MDSourceFile>();
			//Base Case
			if (sourceElm.Children.Count == 0) {
				var sourcefile = new MDSourceFile();
				sourcefile.Metadata = new LitSceneMetadata() {
					Text = MetadataAcc.ToList(),
					Descriptor = sourceElm.TreeTag.Tag.TrimStart('.'),
					Header = String.Join(" - ", HeaderAcc)
				};
				sourcefile.Author = author;

				//This is an inelegent way to force the treetag to be the first scene of the novel
				sourceElm.TreeTag.Tag = String.Format("{0}.01", sourceElm.TreeTag.Tag);
				sourcefile.Lines = new List<String>(
					LO.WriteMetadata(sourcefile.Metadata, sourcefile.Author)
					.Concat(LO.WriteElmSourceLines(sourceElm, author))
				);
				retVal.Add(sourcefile);
			}
			//Inductive Case
			else {
				foreach (var child in sourceElm.Children) {
					var newHeaderAcc = HeaderAcc.Concat(new [] {child.Header});
					var newMetadataAcc = MetadataAcc.Concat(
						LO.WriteElmTextDefault(sourceElm.Source.Text[author])
					);
					retVal.AddRange(WriteSourceFileNovel(
						LO, 
						newHeaderAcc, 
						newMetadataAcc,
						child, 
						author
					));
				}
			}
			return retVal;
		}
	}
}
