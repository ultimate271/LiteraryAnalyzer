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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setBaseDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setWorkingDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.generateMarkdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setRawSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.executeToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setBaseDirToolStripMenuItem,
            this.setWorkingDirToolStripMenuItem,
            this.setRawSourceToolStripMenuItem,
            this.toolStripMenuItem1});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// setBaseDirToolStripMenuItem
			// 
			this.setBaseDirToolStripMenuItem.Name = "setBaseDirToolStripMenuItem";
			this.setBaseDirToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.setBaseDirToolStripMenuItem.Text = "Set Base Dir";
			this.setBaseDirToolStripMenuItem.Click += new System.EventHandler(this.setBaseDirToolStripMenuItem_Click);
			// 
			// setWorkingDirToolStripMenuItem
			// 
			this.setWorkingDirToolStripMenuItem.Name = "setWorkingDirToolStripMenuItem";
			this.setWorkingDirToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.setWorkingDirToolStripMenuItem.Text = "Set Prefix";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// executeToolStripMenuItem
			// 
			this.executeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateMarkdownToolStripMenuItem});
			this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
			this.executeToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.executeToolStripMenuItem.Text = "Execute";
			// 
			// generateMarkdownToolStripMenuItem
			// 
			this.generateMarkdownToolStripMenuItem.Name = "generateMarkdownToolStripMenuItem";
			this.generateMarkdownToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.generateMarkdownToolStripMenuItem.Text = "Generate Markdown";
			// 
			// setRawSourceToolStripMenuItem
			// 
			this.setRawSourceToolStripMenuItem.Name = "setRawSourceToolStripMenuItem";
			this.setRawSourceToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.setRawSourceToolStripMenuItem.Text = "Set Raw Source";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 681);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "Form1";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setBaseDirToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setWorkingDirToolStripMenuItem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem generateMarkdownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setRawSourceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
	}
}

