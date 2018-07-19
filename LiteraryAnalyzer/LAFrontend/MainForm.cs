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
		private void RefreshScreen() {
			label1.Text = "Base Dir: " + c.MarkdownOption.BaseDir;
			label2.Text = "Source File: " + c.MarkdownOption.Filename;
		}
		private void MainForm_Load(object sender, EventArgs e) {
			if (System.IO.Directory.Exists(@"C:\users\bwebster\source\repos\notes")) {
				c.MarkdownOption.BaseDir = @"C:\users\bwebster\source\repos\notes";
			}
			else if (System.IO.Directory.Exists(@"C:\users\brett\source\repos\notes")) {
				c.MarkdownOption.BaseDir = @"C:\users\brett\source\repos\notes";
			}
			
			foreach (var item in Enum.GetValues(typeof(MarkdownOption.ContentsOptions))) {
				comboBox1.Items.Add(item);
				comboBox1.SelectedIndex = 0;
			}
			foreach (var item in Enum.GetValues(typeof(MarkdownOption.ParserOptions))) {
				comboBox2.Items.Add(item);
				comboBox2.SelectedIndex = 0;
			}
			foreach (var item in Enum.GetValues(typeof(MarkdownOption.URIOptions))) {
				comboBox3.Items.Add(item);
				comboBox3.SelectedIndex = 0;
			}
			RefreshScreen();
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

		private void setRawSourceToolStripMenuItem_Click(object sender, EventArgs e) {
			var browser = new OpenFileDialog();
			browser.InitialDirectory = c.MarkdownOption.BaseDir;
			if (browser.ShowDialog() == DialogResult.OK) {
				String result = browser.FileName;
				if (result.StartsWith(c.MarkdownOption.BaseDir, StringComparison.OrdinalIgnoreCase)) {
					c.MarkdownOption.Filename = result.Substring(c.MarkdownOption.BaseDir.Length).TrimStart('\\');
				}
			}
			RefreshScreen();
		}

		private void generateMarkdownToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
				c.ParseMarkdownToFileSystem();
			}
			catch {
				MessageBox.Show("Something got fuckarooed");
				return;
			}
			MessageBox.Show("By golly it worked");
		}

		private void textBox1_TextChanged(object sender, EventArgs e) {
			c.MarkdownOption.Prefix = textBox1.Text;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
			var x = MarkdownOption.ContentsOptions.Default;
			if (Enum.TryParse(comboBox1.Text, out x)) {
				c.MarkdownOption.ContentsOption = x;
			}
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
			var x = MarkdownOption.ParserOptions.Default;
			if (Enum.TryParse(comboBox2.Text, out x)) {
				c.MarkdownOption.ParserOption = x;
			}
		}

		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) {
			var x = MarkdownOption.URIOptions.Default;
			if (Enum.TryParse(comboBox3.Text, out x)) {
				c.MarkdownOption.URIOption = x;
			}
		}
	}
}
