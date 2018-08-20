using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	/// <summary>
	/// Represents the highest level of a section of a novel. A novel will be composed of a list of these.
	/// </summary>
	public class LitScene : LitElm {
		/// <summary>
		/// Represents the location or locations that the scene is taking place.
		/// Empty list indicates that this is simple exposition
		/// Usually a change in location would indicate a change of LitScene, but not always. An example would be
		/// # Julie and Susan discuss politics as they walk from Point A to Point B
		/// In this example, the scene is the discussion of politics.
		/// </summary>
		public List<LitPlace> Locations { get; set; } = new List<LitPlace>();
		/// <summary>
		/// This list will contain all of the characters that are involved in the scene.
		/// This will also include all of the speakers from the events, if they are not included in the annotated markdown.
		/// </summary>
		public List<LitChar> Actors { get; set; } = new List<LitChar>();
		/// <summary>
		/// Used by the writer to know which scenes should go in which files.
		/// </summary>
		public LitSceneMetadata Metadata { get; set; } = new LitSceneMetadata();
	}
	public static partial class LitExtensions {
		/// <summary>
		/// Takes a scene and returns all of the treetags for the speaker
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="speaker"></param>
		/// <returns></returns>
		public static IEnumerable<LitTag> SpeakerTags(this LitScene scene, LitChar speaker) {
			var retVal = new List<LitTag>();
			foreach (var litevent in scene.Children) {
				retVal.AddRange(litevent.SpeakerTags(speaker));
			}
			return retVal;
		}
	}
	public static partial class ParsingTools {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static LitScene ParseToSceneDefault(this LitOptions LO, LitNovel novel, LitSceneMetadata metadata, LitAuthor author, IEnumerable<String> lines) {
			var retVal = new LitScene();

			//Some checks
			if (!novel.Authors.Contains(author)) { throw new Exception(String.Format("Novel does not contain source info. {0}", author.Author)); }
			if (!novel.SceneMetadata.Contains(metadata)) { throw new Exception(String.Format("Novel does not contain metadata. {0}", metadata.Descriptor)); }

			//Parse the header
			LO.ParseSceneHeader(novel, retVal, lines);

			var PartitionedLines = LO.ExtractEvents(lines, 2);
			//if (PartitionedLines.Count <= 1) {
			//	throw new Exception("A scene must have at least one event in it");
			//}

			LO.ParseElmLinks(
				novel,
				retVal, 
				LO.ExtractElmLinkLines(PartitionedLines.First())
			);

			foreach (var eventLines in PartitionedLines.Skip(1)) {
				var litEvent = LO.ParseToEvent(novel, author, eventLines);
				retVal.Children.Add(litEvent);
			}

			retVal.Metadata = metadata;
			return retVal;
		}
		public static void ParseSceneHeaderDefault(this LitOptions LO, LitNovel novel, LitScene scene, IEnumerable<String> SceneLines) {
			var headerInfo = LO.ParseHeader(SceneLines.First());
			if (headerInfo.HeaderLevel != 1) {
				throw new Exception("The first line of a scene must have header level 1, " + SceneLines.First());
			}
			scene.Header = headerInfo.Text;
		}
		public static void ParseSceneLinksDefault(
			this LitOptions LO,
			LitNovel novel,
			LitScene scene,
			IEnumerable<MDLinkLine> links
		){
			foreach (var link in links) {
				LitRef novelRef;
				if (link.Link.Equals("Actor")) {
					novelRef = novel.AddReferenceDistinct(new LitChar(link.Tag));
					scene.Actors.Add(novelRef as LitChar);
				}
				else if (link.Link.Equals("Location")) {
					novelRef = novel.AddReferenceDistinct(new LitPlace(link.Tag));
					scene.Locations.Add(novelRef as LitPlace);
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="LO"></param>
		/// <param name="PartitionedScenes"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<String>> ExtractScenesDefault(this LitOptions LO, IEnumerable<IEnumerable<String>> PartitionedScenes) {
			return PartitionedScenes.Where(lines =>
				lines.Select(l => LO.ParseLink(l))
					.Where(link => link != null)
					.Where(link => link.Link.Equals("TreeTag"))
					.Count() > 0
				);
		}
		public static void MergeScene(this LitScene scene1, LitScene scene2) {
			scene1.Actors = new List<LitChar>(scene1.Actors.Union(scene2.Actors));
			scene1.Locations = new List<LitPlace>(scene1.Locations.Union(scene2.Locations)); 
			scene1.References = new List<LitRef>(scene1.References.Union(scene2.References));
			scene1.Children = new List<LitEvent>(scene1.Children.Zip(scene2.Children, (e1, e2) => e1.MergeEvent(e2)));
		}

		public static List<String> WriteSceneLinksDefault(
			this LitOptions LO,
			LitScene scene
		){
			var retVal = new List<String>();
			retVal.AddRange(scene.Actors.Select(a => 
				MakeLinkLine("Actor", a.Tags.First().Tag)
			));
			retVal.AddRange(scene.Locations.Select(p => 
				MakeLinkLine("Location", p.Tags.First().Tag)
			));
			return retVal;
		}

		//public static List<String> ToSourceLines(this LitScene scene, LitSourceInfo sourceinfo, LitOptions LO) {
		//	var retVal = new List<String>();
		//	retVal.Add(scene.WriteHeader(1));
		//	retVal.AddRange(LO.WriteElmLinks(scene));
		//	foreach (var child in scene.Children) {
		//		retVal.AddRange(child.ToSourceLines(sourceinfo, 2));
		//	}
		//	return retVal;
		//}
	}
}
