namespace UDP
{
    partial class tclsNotify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsNotify));
            this.NotifyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NotifyTextBox
            // 
            this.NotifyTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(224)))));
            this.NotifyTextBox.Location = new System.Drawing.Point(12, 9);
            this.NotifyTextBox.Multiline = true;
            this.NotifyTextBox.Name = "NotifyTextBox";
            this.NotifyTextBox.ReadOnly = true;
            this.NotifyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.NotifyTextBox.Size = new System.Drawing.Size(450, 62);
            this.NotifyTextBox.TabIndex = 0;
            this.NotifyTextBox.TabStop = false;
            // 
            // tclsNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 83);
            this.Controls.Add(this.NotifyTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "tclsNotify";
            this.Text = "Program Status";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.tclsNotify_Load);
            this.Resize += new System.EventHandler(this.tclsNotify_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NotifyTextBox;
    }
}