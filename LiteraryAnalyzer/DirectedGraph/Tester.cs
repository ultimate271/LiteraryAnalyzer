using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraph {
	public class Tester {
		public static void Main() {

			var dg = new DirectedGraph<int, bool>(
				new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 
				false
			);

			for (int i = 0; i < dg.Vertex.Count; i++) {
				dg.AddEdge(true, i, i);
			}
			dg.AddEdge(true, 0, 1);
			dg.AddEdge(true, 0, 2);
			dg.AddEdge(true, 2, 4);
			dg.AddEdge(true, 3, 5);
			dg.AddEdge(true, 4, 6);
			dg.AddEdge(true, 5, 7);
			dg.AddEdge(true, 5, 8);
			dg.AddEdge(true, 6, 7);

			for (int i = 0; i < dg.Vertex.Count; i++) {
				for (int j = 0; j < dg.Vertex.Count; j++) {
					System.Console.Write(dg.Edge.Retrieve(i, j));
				}
				System.Console.WriteLine();
			}

		}
	}
}
