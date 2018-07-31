﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class LitNovel {
		public List<LitScene> Scenes { get; set; } = new List<LitScene>();
		public List<LitRef> References { get; set; } = new List<LitRef>();
		public String Title { get; set; }
		public List<LitRef> GeneratedReference { get; set; } = new List<LitRef>();
	}
	public static partial class LitExtensions {
		/// <summary>
		/// UPON FURTHER CONSIDERATION, I DONT THINK THIS IS THE PROPER WAY TO DO THINGS
		/// Will take every reference in the scenes of the novel, and make sure that
		/// it corresponds to a reference in the references of the novel
		/// </summary>
		/// <param name="novel"></param>
		public static void SanatizeRefs(this LitNovel novel) {
			
		}
		/// <summary>
		/// UPON FURTHER CONSIDERATION, I DONT THINK THIS IS THE PROPER WAY TO DO THINGS
		/// Takes all the reference objects in the scenes and the references, and combines them all
		/// into the references list of the novel
		/// </summary>
		/// <param name="novel"></param>
		public static void CombineRefs(this LitNovel novel) {
			var currentRefs = novel.GetAllReferences();
			novel.References = new List<LitRef>();
			foreach (var litRef in currentRefs) {
				novel.AddReferenceDistinct(litRef);
			}
		}
		/// <summary>
		/// Will add a new reference to the list of references of the novel, or,
		/// if the novel has the reference already, will add any new tags that the
		/// current reference might not have.
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="reference"></param>
		public static LitRef AddReferenceDistinct(this LitNovel novel, LitRef reference) {
			foreach (var currentRef in novel.References) {
				if (currentRef.IsReferenceIntersection(reference)) {
					currentRef.CombineRef(reference);
					return currentRef;
				}
			}
			novel.References.Add(reference);
			novel.GeneratedReference.Add(reference);
			return reference;
		}
		/// <summary>
		/// I DON'T THINK I WANT TO USE THIS FUNCTION
		/// </summary>
		/// <param name="novel"></param>
		/// <param name="scene"></param>
		public static void AddScene(this LitNovel novel, LitScene scene) {
			foreach (var reference in scene.GetAllReferences()) {
				novel.AddReferenceDistinct(reference);
			}
			novel.Scenes.Add(scene);
		}
		/// <summary>
		/// THIS FUNCTION MIGHT BE USEFUL FOR "CHECKING MY WORK" BUT NOT FOR ANYTHING ELSE
		/// </summary>
		/// <param name="novel"></param>
		/// <returns></returns>
		public static List<LitRef> GetAllReferences(this LitNovel novel) {
			var retVal = novel.References;
			foreach (var scene in novel.Scenes) {
				retVal.AddRange(scene.GetAllReferences());
			}
			return retVal;
		}
	}
	public static partial class ParsingTools {
		public static LitNovel ParseAnnSource(this LitAnnSourceInfo source) {
			var retVal = new LitNovel();

			//Aggregate the source
			var files = System.IO.Directory.GetFiles(source.BaseDir, source.Prefix + "*.md");
			Array.Sort(files);
			var query = files.Where(s => !s.Contains("notes.md"));
			List<String> allLines = new List<String>();
			foreach (var file in query) {
				var lines = System.IO.File.ReadAllLines(file);
				var shortfilename = Helper.ExtractFilename(file);
				var taggedLines = ParsingTools.TagLines(lines, shortfilename);
				allLines.AddRange(taggedLines);
			}

			var PartitionedScenes = ParsingTools.PartitionLines(allLines, line => System.Text.RegularExpressions.Regex.IsMatch(line, @"^#[^#]"));

			foreach (var Scenelines in PartitionedScenes) {
				var scene = retVal.ParseScene(Scenelines, source.LitSourceInfo);
				retVal.Scenes.Add(scene);
			}

			//Some decisions
			//The very first # does not represent a scene, but instead represents metadata.
			//The user can define a tag manually. This is done by adding a [Tag Element] tag under the header

			return retVal;
		}
		public static LitAnnSourceInfo GenerateMarkdown(this LitNovel novel) {
			throw new NotImplementedException();
		}
	}
}
