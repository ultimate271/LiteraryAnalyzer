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
			RefreshScreen();
		}
		private void RefreshScreen() {
			label1.Text = "Base Dir: " + c.MarkdownOption.BaseDir;
		}


		private void setBaseDirToolStripMenuItem_Click(object sender, EventArgs e) {
			var browser = new FolderBrowserDialog();
			browser.RootFolder = Environment.SpecialFolder.UserProfile;
			browser.SelectedPath = c.MarkdownOption.BaseDir;
			if (browser.ShowDialog() == DialogResult.OK) {
				c.MarkdownOption.BaseDir = browser.SelectedPath;
				RefreshScreen();
			}
		}
	}
}
