using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraph {
	public class DirectedGraph<V, E> {
		public List<V> Vertex { get; set; } = new List<V>();
		public Operator<E> Edge { get; set; }
		E nil;

		public DirectedGraph(IEnumerable<V> Vertex, E nil) {
			this.nil = nil;
			this.Vertex = new List<V>();
			foreach(var v in Vertex){
				this.Vertex.Add(v);
			}
			this.Edge = new Operator<E>(this.Vertex.Count, nil);
		}

		public void AddEdge(E e, int i, int j) {
			try {
				this.Edge.Set(e, i, j);
			}
			catch { }
		}

		public void CloseIdentity(E closure) {
			for (int i = 0; i < this.Vertex.Count; i++) {
				this.AddEdge(closure, i, i);
			}
		}
    }
}
