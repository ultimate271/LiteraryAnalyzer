using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	public class Excerpt {
		public int ExcerptID { get; set; }
		public string ExcerptText { get; set; }
		public virtual Token Token { get; set; }
		public virtual List<Descriptor> Descriptors { get; set; } = new List<Descriptor>();

		public virtual Excerpt ParseString(String s) { return new Excerpt(); }

	}
}

//So far I have in mind three types of Excerpts
//Here is what each might look at in an example

//# Chapter 1: The boy and the dog go fishing
//_Character:The boy_  
//Once upon a time there was a boy. This boy was twelve years old, and was the most common type of boy you ...

//In other words, HeaderExcerpts, AnnotationExcerpts, and ContentExcterpts

//Each one should have its own parsing function.
//This parsing function is a function from String -> Excerpt
