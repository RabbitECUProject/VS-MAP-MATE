namespace UDP
{
    partial class tclsMeasValueGaugeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsMeasValueGaugeView));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gaugesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRemoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gaugesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gaugesToolStripMenuItem
            // 
            this.gaugesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRemoveToolStripMenuItem});
            this.gaugesToolStripMenuItem.Name = "gaugesToolStripMenuItem";
            this.gaugesToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.gaugesToolStripMenuItem.Text = "Gauges";
            this.gaugesToolStripMenuItem.Click += new System.EventHandler(this.gaugesToolStripMenuItem_Click);
            // 
            // addRemoveToolStripMenuItem
            // 
            this.addRemoveToolStripMenuItem.Name = "addRemoveToolStripMenuItem";
            this.addRemoveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addRemoveToolStripMenuItem.Text = "Add/Remove";
            this.addRemoveToolStripMenuItem.Click += new System.EventHandler(this.addRemoveToolStripMenuItem_Click);
            // 
            // tclsMeasValueGaugeView
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "tclsMeasValueGaugeView";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.tclsMeasValueGaugeView_Load_1);
            this.Resize += new System.EventHandler(this.GaugeView_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gaugesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRemoveToolStripMenuItem;
    }
}