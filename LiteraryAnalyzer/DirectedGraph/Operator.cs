using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraph {
	public class Operator <V> {
		public int Dimension { get; set; } = 0;
		private List<List<V>> _Entries = new List<List<V>>();
		public V nil { get; set; }

		public static Operator<V> GetBlank (int Dimension, V nil) {
			return new Operator<V>(Dimension, nil);
		}
		public Operator(int Dimension, V nil) {
			this.Dimension = Dimension;
			this.nil = nil;
			this.Build(nil);
		}

		public void Build() {
			try {
				this.Build(this.nil);
			}
			catch { }
		}
		/// <summary>
		/// Will clear this operator and start from scratch
		/// </summary>
		/// <param name="nil"></param>
		public void Build(V nil) {
			this.nil = nil;
			_Entries.Clear();
			for (int i = 0; i < this.Dimension; i++) {
				_Entries.Add(new List<V>());
				for (int j = 0; j < this.Dimension; j++) {
					_Entries[i].Add(nil);
				}
			}
		}
		/// <summary>
		/// If the set fails for any reason, this function does nothing.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="i"></param>
		/// <param name="j"></param>
		public void Set(V val, int i, int j) {
			try {
				_Entries[i][j] = val;
			} catch {
			}
		}

		public V Retrieve(int i, int j) {
			try {
				return _Entries[i][j];
			} catch {
				return this.nil;
			}
		}

		public List<V> GetRow(int i) {
			return new List<V>(_Entries[i]);
		}
		public List<V> GetColumn(int j) {
			var retVal = new List<V>();
			foreach(var row in _Entries) {
				retVal.Add(row[j]);
			}
			return retVal;
		}

	}
}
