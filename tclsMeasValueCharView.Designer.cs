namespace UDP
{
    partial class tclsMeasValueCharView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsMeasValueCharView));
            this.toolStripValueCharView = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLeft = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripValueCharView.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripValueCharView
            // 
            this.toolStripValueCharView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonLeft,
            this.toolStripButtonRight});
            this.toolStripValueCharView.Location = new System.Drawing.Point(0, 0);
            this.toolStripValueCharView.Name = "toolStripValueCharView";
            this.toolStripValueCharView.Size = new System.Drawing.Size(686, 25);
            this.toolStripValueCharView.TabIndex = 0;
            this.toolStripValueCharView.Text = "toolStrip1";
            // 
            // toolStripButtonLeft
            // 
            this.toolStripButtonLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLeft.Image = global::UDP.Properties.Resources.GlyphLeft;
            this.toolStripButtonLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLeft.Name = "toolStripButtonLeft";
            this.toolStripButtonLeft.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLeft.Text = "toolStripPaneLeft";
            this.toolStripButtonLeft.ToolTipText = "Previous ";
            this.toolStripButtonLeft.Click += new System.EventHandler(this.toolStripButtonLeft_Click);
            // 
            // toolStripButtonRight
            // 
            this.toolStripButtonRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRight.Image = global::UDP.Properties.Resources.GlyphRight;
            this.toolStripButtonRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRight.Name = "toolStripButtonRight";
            this.toolStripButtonRight.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRight.Text = "toolStripButtonRight";
            this.toolStripButtonRight.ToolTipText = "Next";
            this.toolStripButtonRight.Click += new System.EventHandler(this.toolStripButtonRight_Click);
            // 
            // tclsMeasValueCharView
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(686, 500);
            this.Controls.Add(this.toolStripValueCharView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "tclsMeasValueCharView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.tclsMeasCharView_Load);
            this.Resize += new System.EventHandler(this.tclsMeasValueCharView_Resize);
            this.toolStripValueCharView.ResumeLayout(false);
            this.toolStripValueCharView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripValueCharView;
        private System.Windows.Forms.ToolStripButton toolStripButtonLeft;
        private System.Windows.Forms.ToolStripButton toolStripButtonRight;
    }
}