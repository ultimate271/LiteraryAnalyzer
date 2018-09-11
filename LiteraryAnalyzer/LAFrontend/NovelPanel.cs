using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using LiteraryAnalyzer.LAShared;

namespace LiteraryAnalyzer.LAFrontend {
	public partial class NovelPanel : LiteraryAnalyzer.LAFrontend.BasePanel {
		public Controller c { get; set; } = new Controller();
		private LAShared.MDAnnSource AnnSource { get; set; }
		private LAShared.LitNovel Novel { get; set; }
		private LAShared.LitElm SelectedNode { get; set; }
		public NovelPanel() {
			InitializeComponent();

			Novel = c.DeveloperDebug();
			AnnSource = c.DeveloperDebugTwo();
		}

		private void NovelPanel_Load(object sender, EventArgs e) {
			comboBox1.Items.Clear();
			comboBox1.DisplayMember = "Author";
			comboBox1.Items.AddRange(this.Novel.Authors.ToArray());
			comboBox1.SelectedIndex = 0;

			var NovelNode = new TreeNode(this.Novel.Title);
			NovelNode.Nodes.AddRange(
				this.Novel.Scenes.Select(
					s => BuildTreeNode(s)
				).ToArray()
			);
			treeView1.Nodes.Clear();
			treeView1.Nodes.Add(NovelNode);

			treeView2.Nodes.Clear();
			foreach (var source in this.AnnSource.Sources) {
				var SourceNode = new TreeNode(source.Metadata.Descriptor);
				SourceNode.Tag = source;
				treeView2.Nodes.Add(SourceNode);
			}

			RefreshNovel();
		}

		private void RefreshNovel() {
		}
		private TreeNode BuildTreeNode(LAShared.LitElm elm) {
			var retVal = new TreeNode(elm.Header);
			retVal.Tag = elm;
			foreach (var child in elm.Children) {
				retVal.Nodes.Add(BuildTreeNode(child));
			}
			return retVal;
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
			this.SelectedNode = treeView1.SelectedNode.Tag as LAShared.LitElm;
			this.textBox1.Text = SelectedNode?.AllText(comboBox1.SelectedItem as LitAuthor);
		}
		private void treeView2_AfterSelect(object sender, TreeViewEventArgs e) {
			var source = treeView2.SelectedNode.Tag as LAShared.MDSourceFile;
			this.textBox1.Text = c.ToRawTextbox(source.Lines);
		}
		private void treeView1_Leave(object sender, EventArgs e) {
			var tv = sender as TreeView;
			tv.SelectedNode = null;
		}

		private void comboBox1_SelectedValueChanged(object sender, EventArgs e) {
		}

		private void treeView1_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.F2) {
				System.Console.WriteLine((int)e.KeyCode);
			}
			else if (e.KeyCode == Keys.H && e.Modifiers == Keys.Control) {
				treeView2.Focus();
			}
			else {
				System.Console.WriteLine((int)e.KeyCode);
			}
		}
	}
}
