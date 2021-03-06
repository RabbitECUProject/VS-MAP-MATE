namespace UDP
{
    partial class tclsDSG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsDSG));
            this.pictureBoxDSG = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDSG)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxDSG
            // 
            this.pictureBoxDSG.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxDSG.Name = "pictureBoxDSG";
            this.pictureBoxDSG.Size = new System.Drawing.Size(324, 429);
            this.pictureBoxDSG.TabIndex = 0;
            this.pictureBoxDSG.TabStop = false;
            // 
            // tclsDSG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(558, 429);
            this.Controls.Add(this.pictureBoxDSG);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "tclsDSG";
            this.Text = "tclsDSG";
            this.Resize += new System.EventHandler(this.tclsDSG_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDSG)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxDSG;
    }
}