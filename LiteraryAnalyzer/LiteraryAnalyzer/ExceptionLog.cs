using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	public class ExceptionLog {
		public ExceptionLog() { }
		public ExceptionLog(Exception e) {
			this.StackTrace = e.StackTrace;
			this.Message = e.Message;
			this.Source = e.Source;
			this.ExToString = e.ToString();
		}
		public ExceptionLog(Exception e, String Commentary) : this(e) {
			this.Commentary = Commentary;
		}
		public int ExceptionLogID { get; set; }
		public String StackTrace { get; set; }
		public String Message { get; set;}
		public String Source { get; set;}
		public String ExToString { get; set;}
		public String Commentary { get; set; }
		public DateTime OccuranceTime { get; set; } = DateTime.Now;

		private Exception Ex;


	}
}
