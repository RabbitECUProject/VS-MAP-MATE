namespace UDP
{
    partial class tclsLogicBlockView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsLogicBlockView));
            this.menuStripLogic = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemIOPDF = new System.Windows.Forms.ToolStripMenuItem();
            this.iOHelpPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripLogic.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripLogic
            // 
            this.menuStripLogic.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemIOPDF});
            this.menuStripLogic.Location = new System.Drawing.Point(0, 0);
            this.menuStripLogic.Name = "menuStripLogic";
            this.menuStripLogic.Size = new System.Drawing.Size(392, 24);
            this.menuStripLogic.TabIndex = 0;
            this.menuStripLogic.Text = "menuStrip1";
            // 
            // toolStripMenuItemIOPDF
            // 
            this.toolStripMenuItemIOPDF.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iOHelpPDFToolStripMenuItem});
            this.toolStripMenuItemIOPDF.Name = "toolStripMenuItemIOPDF";
            this.toolStripMenuItemIOPDF.Size = new System.Drawing.Size(44, 20);
            this.toolStripMenuItemIOPDF.Text = "Help";
            this.toolStripMenuItemIOPDF.Click += new System.EventHandler(this.toolStripMenuItemIOPDF_Click);
            // 
            // iOHelpPDFToolStripMenuItem
            // 
            this.iOHelpPDFToolStripMenuItem.Name = "iOHelpPDFToolStripMenuItem";
            this.iOHelpPDFToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.iOHelpPDFToolStripMenuItem.Text = "I/O Help PDF";
            this.iOHelpPDFToolStripMenuItem.Click += new System.EventHandler(this.iOHelpPDFToolStripMenuItem_Click);
            // 
            // tclsLogicBlockView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(392, 261);
            this.Controls.Add(this.menuStripLogic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripLogic;
            this.Name = "tclsLogicBlockView";
            this.Text = "tclsLogicBlockView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Resize += new System.EventHandler(this.tclsLogicView_Resize);
            this.menuStripLogic.ResumeLayout(false);
            this.menuStripLogic.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripLogic;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemIOPDF;
        private System.Windows.Forms.ToolStripMenuItem iOHelpPDFToolStripMenuItem;
    }
}