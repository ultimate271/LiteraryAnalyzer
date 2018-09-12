namespace LiteraryAnalyzer.LAFrontend {
	partial class NovelPanel {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.treeView2 = new System.Windows.Forms.TreeView();
			this.btnAddElm = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point(762, 31);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(435, 562);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyUp);
			this.treeView1.Leave += new System.EventHandler(this.treeView1_Leave);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(4, 31);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(496, 566);
			this.textBox1.TabIndex = 1;
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(4, 4);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(321, 21);
			this.comboBox1.TabIndex = 2;
			this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
			// 
			// treeView2
			// 
			this.treeView2.Location = new System.Drawing.Point(506, 31);
			this.treeView2.Name = "treeView2";
			this.treeView2.Size = new System.Drawing.Size(250, 562);
			this.treeView2.TabIndex = 3;
			this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
			this.treeView2.Leave += new System.EventHandler(this.treeView2_Leave);
			// 
			// btnAddElm
			// 
			this.btnAddElm.Location = new System.Drawing.Point(762, 2);
			this.btnAddElm.Name = "btnAddElm";
			this.btnAddElm.Size = new System.Drawing.Size(75, 23);
			this.btnAddElm.TabIndex = 4;
			this.btnAddElm.Text = "Add Elm";
			this.btnAddElm.UseVisualStyleBackColor = true;
			this.btnAddElm.Click += new System.EventHandler(this.btnAddElm_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(843, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "label1";
			// 
			// NovelPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnAddElm);
			this.Controls.Add(this.treeView2);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.treeView1);
			this.Name = "NovelPanel";
			this.Size = new System.Drawing.Size(1200, 600);
			this.Load += new System.EventHandler(this.NovelPanel_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TreeView treeView2;
		private System.Windows.Forms.Button btnAddElm;
		private System.Windows.Forms.Label label1;
	}
}
