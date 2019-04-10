using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiteraryAnalyzer.LAShared;

namespace LiteraryAnalyzer.LAFrontend {
	public partial class MainForm : Form {
		private Controller c { get; set; } = new Controller();
		public MainForm() {
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e) {
			sourceWriterPanel1.c = this.c;
			novelPanel1.c = this.c;
		}

		private void sourceWriterPanel1_Load(object sender, EventArgs e) {

		}
	}
}
