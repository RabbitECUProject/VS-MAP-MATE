namespace UDP
{
    partial class tclsAxesEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsAxesEdit));
            this.buttonEnter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonEnter
            // 
            this.buttonEnter.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonEnter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonEnter.Location = new System.Drawing.Point(609, 226);
            this.buttonEnter.Name = "buttonEnter";
            this.buttonEnter.Size = new System.Drawing.Size(101, 32);
            this.buttonEnter.TabIndex = 0;
            this.buttonEnter.Text = "Enter";
            this.buttonEnter.UseVisualStyleBackColor = false;
            this.buttonEnter.Click += new System.EventHandler(this.buttonEnter_Click);
            // 
            // tclsAxesEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(722, 270);
            this.Controls.Add(this.buttonEnter);
            this.ForeColor = System.Drawing.Color.Aquamarine;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tclsAxesEdit";
            this.Text = "Edit Axes Points";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonEnter;
    }
}