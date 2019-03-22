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
			for (int i = 0; i < dg.Vertex.Count; i++) {
				for (int j = 0; j < dg.Vertex.Count; j++) {
					System.Console.Write(dg.Edge.Retrieve(i, j));
				}
				System.Console.WriteLine();
			}
		}
	}
}
