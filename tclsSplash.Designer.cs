namespace UDP
{
    partial class tclsSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsSplash));
            this.labelSplash = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelSplash
            // 
            this.labelSplash.BackColor = System.Drawing.Color.Transparent;
            this.labelSplash.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSplash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.labelSplash.Location = new System.Drawing.Point(12, 489);
            this.labelSplash.Name = "labelSplash";
            this.labelSplash.Size = new System.Drawing.Size(948, 35);
            this.labelSplash.TabIndex = 0;
            this.labelSplash.Text = "Loading Application...";
            // 
            // tclsSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(972, 533);
            this.Controls.Add(this.labelSplash);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tclsSplash";
            this.Text = "MDAC MAP-MATE [Rabbit ECU Version]";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSplash;
    }
}