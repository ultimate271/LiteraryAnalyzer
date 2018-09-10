namespace LiteraryAnalyzer.LAFrontend {
	partial class MainForm {
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
			LiteraryAnalyzer.Controller controller1 = new LiteraryAnalyzer.Controller();
			LiteraryAnalyzer.LAShared.MarkdownOption markdownOption1 = new LiteraryAnalyzer.LAShared.MarkdownOption();
			LiteraryAnalyzer.Controller controller2 = new LiteraryAnalyzer.Controller();
			LiteraryAnalyzer.LAShared.MarkdownOption markdownOption2 = new LiteraryAnalyzer.LAShared.MarkdownOption();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.sourceWriterPanel1 = new LiteraryAnalyzer.LAFrontend.SourceWriterPanel();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.novelPanel1 = new LiteraryAnalyzer.LAFrontend.NovelPanel();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1240, 657);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.sourceWriterPanel1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1232, 631);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "SourceWriter";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// sourceWriterPanel1
			// 
			this.sourceWriterPanel1.BackColor = System.Drawing.SystemColors.Control;
			markdownOption1.BaseDir = "C:\\users\\bwebster\\source\\repos\\notes";
			markdownOption1.ContentsOption = LiteraryAnalyzer.LAShared.MarkdownOption.ContentsOptions.Default;
			markdownOption1.ExcerptOption = LiteraryAnalyzer.LAShared.MarkdownOption.ExcerptOptions.Default;
			markdownOption1.Filename = "";
			markdownOption1.MarkdownOptionID = 0;
			markdownOption1.ParserOption = LiteraryAnalyzer.LAShared.MarkdownOption.ParserOptions.Default;
			markdownOption1.Prefix = "";
			markdownOption1.URIOption = LiteraryAnalyzer.LAShared.MarkdownOption.URIOptions.Default;
			controller1.MarkdownOption = markdownOption1;
			this.sourceWriterPanel1.c = controller1;
			this.sourceWriterPanel1.Location = new System.Drawing.Point(7, 7);
			this.sourceWriterPanel1.Name = "sourceWriterPanel1";
			this.sourceWriterPanel1.Size = new System.Drawing.Size(450, 600);
			this.sourceWriterPanel1.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.novelPanel1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1232, 631);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// novelPanel1
			// 
			this.novelPanel1.BackColor = System.Drawing.SystemColors.Control;
			markdownOption2.BaseDir = "";
			markdownOption2.ContentsOption = LiteraryAnalyzer.LAShared.MarkdownOption.ContentsOptions.Default;
			markdownOption2.ExcerptOption = LiteraryAnalyzer.LAShared.MarkdownOption.ExcerptOptions.Default;
			markdownOption2.Filename = "";
			markdownOption2.MarkdownOptionID = 0;
			markdownOption2.ParserOption = LiteraryAnalyzer.LAShared.MarkdownOption.ParserOptions.Default;
			markdownOption2.Prefix = "";
			markdownOption2.URIOption = LiteraryAnalyzer.LAShared.MarkdownOption.URIOptions.Default;
			controller2.MarkdownOption = markdownOption2;
			this.novelPanel1.c = controller2;
			this.novelPanel1.Location = new System.Drawing.Point(7, 7);
			this.novelPanel1.Name = "novelPanel1";
			this.novelPanel1.Size = new System.Drawing.Size(1200, 600);
			this.novelPanel1.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 681);
			this.Controls.Add(this.tabControl1);
			this.Name = "MainForm";
			this.Text = "Lit Analyzer";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private SourceWriterPanel sourceWriterPanel1;
		private System.Windows.Forms.TabPage tabPage2;
		private NovelPanel novelPanel1;
	}
}

