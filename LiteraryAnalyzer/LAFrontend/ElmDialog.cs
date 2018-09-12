using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiteraryAnalyzer.LAFrontend {
	public partial class ElmDialog : Form {
		public String HeaderText { get; set; }
		public ElmDialog() {
			InitializeComponent();
		}

		private void ElmDialog_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				this.HeaderText = this.textBox1.Text;
				this.Close();
			}
		}
	}
}
