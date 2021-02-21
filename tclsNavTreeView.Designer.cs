namespace UDP
{
    partial class tclsNavTreeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsNavTreeView));
            this.navTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // navTreeView
            // 
            this.navTreeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.navTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.navTreeView.ForeColor = System.Drawing.Color.Aquamarine;
            this.navTreeView.Location = new System.Drawing.Point(5, 5);
            this.navTreeView.Name = "navTreeView";
            this.navTreeView.Size = new System.Drawing.Size(132, 255);
            this.navTreeView.TabIndex = 0;
            this.navTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.navTreeView_AfterSelect);
            // 
            // tclsNavTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(142, 266);
            this.Controls.Add(this.navTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tclsNavTreeView";
            this.Text = "Workspace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.tclsNavTreeView_Load);
            this.Resize += new System.EventHandler(this.tclsNavTreeView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView navTreeView;
    }
}