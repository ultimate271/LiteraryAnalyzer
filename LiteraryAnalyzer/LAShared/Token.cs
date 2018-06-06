using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class Token {
		private String _TokenID;
		public String TokenID { get { return _TokenID.Trim(); } set { _TokenID = value.Trim(); } }
		public String Description { get; set; }
	}
}
