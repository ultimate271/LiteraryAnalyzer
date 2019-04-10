using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiteraryAnalyzer.LAShared;

namespace LiteraryAnalyzer.LAFrontend {
	public partial class SourceWriterPanel : LiteraryAnalyzer.LAFrontend.BasePanel {
		public Controller c { get; set; } = new Controller();
		private String BaseDir { get; set; } = "";
		private String Filename { get; set; } = "";
		private String WriteDir { get; set; } = "";
		public SourceWriterPanel() {
			InitializeComponent();
		}
		private void SourceWriterPanel_Load(object sender, EventArgs e) {
			if (System.IO.Directory.Exists(@"C:\users\bwebster\source\repos\notes")) {
				c.MarkdownOption.BaseDir = @"C:\users\bwebster\source\repos\notes";
			}
			else if (System.IO.Directory.Exists(@"C:\Users\Brett\Source\Repos\notes")) {
				c.MarkdownOption.BaseDir = @"C:\Users\Brett\Source\Repos\notes";
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
		}
		private void setBaseDirToolStripMenuItem_Click(object sender, EventArgs e) {
			var browser = new FolderBrowserDialog();
			browser.RootFolder = Environment.SpecialFolder.UserProfile;
			browser.SelectedPath = c.MarkdownOption.BaseDir;
			if (browser.ShowDialog() == DialogResult.OK) {
				c.MarkdownOption.BaseDir = browser.SelectedPath;
				this.BaseDir = browser.SelectedPath;
				this.label1.Text = c.MarkdownOption.BaseDir;
			}
		}

		private void setRawSourceToolStripMenuItem_Click(object sender, EventArgs e) {
			var browser = new OpenFileDialog();
			browser.InitialDirectory = c.MarkdownOption.BaseDir;
			if (browser.ShowDialog() == DialogResult.OK) {
				String result = browser.FileName;
				if (result.StartsWith(c.MarkdownOption.BaseDir, StringComparison.OrdinalIgnoreCase)) {
					c.MarkdownOption.Filename = result.Substring(c.MarkdownOption.BaseDir.Length).TrimStart('\\');
					this.Filename = result.Substring(c.MarkdownOption.BaseDir.Length).TrimStart('\\');
					this.label2.Text = c.MarkdownOption.Filename;
				}
			}
		}
		private void button5_Click(object sender, EventArgs e) {
			var browser = new FolderBrowserDialog();
			browser.RootFolder = Environment.SpecialFolder.UserProfile;
			browser.SelectedPath = c.MarkdownOption.BaseDir;
			if (browser.ShowDialog() == DialogResult.OK) {
				this.WriteDir = browser.SelectedPath;
				this.label7.Text = this.WriteDir;
			}
		}

		private void button3_Click(object sender, EventArgs e) {
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

		private void button4_Click(object sender, EventArgs e) {
			var infoIn = new MDAnnSourceInfo() {
				BaseDir = this.BaseDir,
				Prefix = this.Filename
			};
			var infoOut = new MDAnnSourceInfo() {
				BaseDir = this.WriteDir,
				Prefix = this.textBox1.Text
			};
			try {
				c.SeparateNovel(infoIn, infoOut);
			}
			catch {
				MessageBox.Show("Something got fuckarooed");
				return;
			}
			MessageBox.Show("By golly it worked");
		}

		private void button6_Click(object sender, EventArgs e) {
			var infoIn = new MDAnnSourceInfo() {
				BaseDir = this.BaseDir,
				Prefix = this.Filename
			};
			var infoOut = new MDAnnSourceInfo() {
				BaseDir = this.WriteDir,
				Prefix = this.textBox1.Text
			};
			try {
				c.SeparateNovel(infoIn, infoOut, LitOptionsFactory.CreateShakespearePlay());
			}
			catch {
				MessageBox.Show("Something got fuckarooed");
				return;
			}
			MessageBox.Show("By golly it worked");

		}
	}
}
