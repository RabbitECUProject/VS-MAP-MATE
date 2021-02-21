namespace UDP
{
    partial class tclsICMeasSegmentView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsICMeasSegmentView));
            this.toolStripMeasSegment = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPrev = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripMeasSegment.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMeasSegment
            // 
            this.toolStripMeasSegment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPrev,
            this.toolStripButtonNext});
            this.toolStripMeasSegment.Location = new System.Drawing.Point(0, 0);
            this.toolStripMeasSegment.Name = "toolStripMeasSegment";
            this.toolStripMeasSegment.Size = new System.Drawing.Size(284, 25);
            this.toolStripMeasSegment.TabIndex = 0;
            this.toolStripMeasSegment.Text = "toolStripMeasSegment";
            this.toolStripMeasSegment.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButtonPrev
            // 
            this.toolStripButtonPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrev.Image = global::UDP.Properties.Resources.GlyphLeft;
            this.toolStripButtonPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrev.Name = "toolStripButtonPrev";
            this.toolStripButtonPrev.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPrev.Text = "toolStripButton1";
            this.toolStripButtonPrev.ToolTipText = "Previous";
            this.toolStripButtonPrev.Click += new System.EventHandler(this.toolStripButtonPrev_Click);
            // 
            // toolStripButtonNext
            // 
            this.toolStripButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNext.Image = global::UDP.Properties.Resources.GlyphRight;
            this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNext.Name = "toolStripButtonNext";
            this.toolStripButtonNext.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNext.Text = "toolStripButton1";
            this.toolStripButtonNext.ToolTipText = "Next";
            this.toolStripButtonNext.Click += new System.EventHandler(this.toolStripButtonNext_Click);
            // 
            // tclsICMeasSegmentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.toolStripMeasSegment);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "tclsICMeasSegmentView";
            this.Text = "tclsICMeasSegmentView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Resize += new System.EventHandler(this.tclsICMeasSegmentView_Resize);
            this.toolStripMeasSegment.ResumeLayout(false);
            this.toolStripMeasSegment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMeasSegment;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrev;
        private System.Windows.Forms.ToolStripButton toolStripButtonNext;
    }
}