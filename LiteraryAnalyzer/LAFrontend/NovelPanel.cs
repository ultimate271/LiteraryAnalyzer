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
		private LAShared.LitNovel Novel { get; set; }
		public NovelPanel() {
			InitializeComponent();
		}

		private void NovelPanel_Load(object sender, EventArgs e) {
			Novel = c.DeveloperDebug();
			RefreshNovel();
		}

		private void RefreshNovel() {
			var NovelNode = new TreeNode(this.Novel.Title);
			NovelNode.Nodes.AddRange(
				this.Novel.Scenes.Select(
					s => BuildTreeNode(s)
				).ToArray()
			);
			treeView1.Nodes.Clear();
			treeView1.Nodes.Add(NovelNode);

			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(this.Novel.Authors.Select(a => a.Author).ToArray());
			comboBox1.SelectedIndex = 0;
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
			var elm = treeView1.SelectedNode.Tag as LAShared.LitElm;
			textBox1.Text = elm?.AllText(Novel.Authors.First());
		}
	}
}
