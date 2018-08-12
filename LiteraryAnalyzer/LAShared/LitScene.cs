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
		public List<LitPlace> Location { get; set; } = new List<LitPlace>();
		/// <summary>
		/// This list will contain all of the characters that are involved in the scene.
		/// This will also include all of the speakers from the events, if they are not included in the annotated markdown.
		/// </summary>
		public List<LitChar> Actors { get; set; } = new List<LitChar>();
		/// <summary>
		/// Will include any other references to anything that the reader might want to know while reading this scene.
		/// This should be distinct from all of the other lit tools here.
		/// </summary>
		public List<LitRef> References { get; set; } = new List<LitRef>();
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
		public static LitScene ParseScene(this LitNovel novel, IEnumerable<String> lines, LitAuthor sourceInfo, LitSceneMetadata metadata) {
			var retVal = new LitScene();

			//Some checks
			if (!novel.SourceInfo.Contains(sourceInfo)) { throw new Exception(String.Format("Novel does not contain source info. {0}", sourceInfo.Author)); }
			if (!novel.SceneMetadata.Contains(metadata)) { throw new Exception(String.Format("Novel does not contain metadata. {0}", metadata.Descriptor)); }

			//Parse the header
			var headerInfo = novel.LO.ParseHeader(lines.First());
			if (headerInfo.HeaderLevel != 1) {
				throw new Exception("The first line of a scene must have header level 1, " + lines.First());
			}
			retVal.Header = headerInfo.Text;

			//Partition the events
			var pattern = @"^##[^#]";
			var PartitionedLines = ParsingTools.PartitionLines(lines, line => System.Text.RegularExpressions.Regex.IsMatch(line, pattern));
			//if (PartitionedLines.Count <= 1) {
			//	throw new Exception("A scene must have at least one event in it");
			//}

			//Parse the links
			var query = PartitionedLines.First()
				.Select(l => novel.LO.ParseLink(l))
				.Where(l => l != null);
			foreach (var link in query) {
				//I feel as though there is a way to use reflection to be super clever here,
				//But upon thinking about it, I think it would only create more confusion than it
				//would help, since the actual properties of the scene are not that numerous,
				//and to be honest there would probably me more exceptional cases than I am willing
				//To admit, so at this juncture, I will use a elseif chain to do what I want.
				//I don't like it, but at the same time I sort of do because it is more explicit and easier
				//To work with, and an if else chain makes sense.
				//I want to use reflection so bad, but it's probably for the best that I do this in
				//The concrete way for now, and if at some point down the road, I want to change this to use reflection,
				//It will be not terribly difficult to do (at least, only as difficult as reflection is)
				LitRef novelRef;
				if (link.Link.Equals("TreeTag")) {
					retVal.TreeTag = new LitTag(link.Tag);
				}
				else if (link.Link.Equals("UserTag")) {
					//TODO UserTags must be unique, not only and that should be checked somewhere here
					retVal.UserTags.Add(new LitTag(link.Tag));
				}
				else if (link.Link.Equals("Actor")) {
					novelRef = novel.AddReferenceDistinct(new LitChar(link.Tag));
					retVal.Actors.Add(novelRef as LitChar);
				}
				else if (link.Link.Equals("Location")) {
					novelRef = novel.AddReferenceDistinct(new LitPlace(link.Tag));
					retVal.Location.Add(novelRef as LitPlace);
				}
				else if (link.Link.Equals("Reference")) {
					novelRef = novel.AddReferenceDistinct(new LitRef(link.Tag));
					retVal.References.Add(novelRef);
				}
			}
			foreach (var eventLines in PartitionedLines.Skip(1)) {
				var litEvent = novel.ParseEvent(eventLines, sourceInfo);
				retVal.Children.Add(litEvent);
			}

			retVal.Metadata = metadata;
			return retVal;
		}
		public static void MergeScene(this LitScene scene1, LitScene scene2) {
			scene1.Actors = new List<LitChar>(scene1.Actors.Union(scene2.Actors));
			scene1.Location = new List<LitPlace>(scene1.Location.Union(scene2.Location)); 
			scene1.References = new List<LitRef>(scene1.References.Union(scene2.References));
			scene1.Children = new List<LitEvent>(scene1.Children.Zip(scene2.Children, (e1, e2) => e1.MergeEvent(e2)));
		}

		//public static List<String> WriteSceneLinks(this LitScene scene) {
		//	var retVal = scene.WriteElmLinks();
		//	retVal.AddRange(scene.Actors.Select(a => MakeLinkLine("Actor", a.Tags.First().Tag)));
		//	retVal.AddRange(scene.Location.Select(p => MakeLinkLine("Location", p.Tags.First().Tag)));
		//	retVal.AddRange(scene.References.Select(r => MakeLinkLine("Reference", r.Tags.First().Tag)));
		//	return retVal;
		//}

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
