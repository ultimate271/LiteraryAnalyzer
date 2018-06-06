using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer.LAShared {
	public class Descriptor {
		private String _DescriptorID;
		public String DescriptorID { get { return _DescriptorID; } set { _DescriptorID = value.Trim(); } }
	}
}
