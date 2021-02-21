namespace UDP
{
    partial class tclsTuningTargetDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsTuningTargetDisplay));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.trackBarStability = new System.Windows.Forms.TrackBar();
            this.trackBarSpeed = new System.Windows.Forms.TrackBar();
            this.labelStability = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.TwinLEDDisplay = new UDP.tclsTwinLEDDisplay();
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarStability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pictureBox.Location = new System.Drawing.Point(12, 59);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(80, 80);
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            // 
            // trackBarStability
            // 
            this.trackBarStability.LargeChange = 1;
            this.trackBarStability.Location = new System.Drawing.Point(133, 59);
            this.trackBarStability.Maximum = 5;
            this.trackBarStability.Name = "trackBarStability";
            this.trackBarStability.Size = new System.Drawing.Size(104, 45);
            this.trackBarStability.TabIndex = 4;
            this.trackBarStability.Scroll += new System.EventHandler(this.trackBarStability_Scroll);
            // 
            // trackBarSpeed
            // 
            this.trackBarSpeed.LargeChange = 1;
            this.trackBarSpeed.Location = new System.Drawing.Point(133, 109);
            this.trackBarSpeed.Maximum = 5;
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new System.Drawing.Size(104, 45);
            this.trackBarSpeed.TabIndex = 5;
            this.trackBarSpeed.Scroll += new System.EventHandler(this.trackBarSpeed_Scroll);
            // 
            // labelStability
            // 
            this.labelStability.AutoSize = true;
            this.labelStability.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStability.ForeColor = System.Drawing.Color.Red;
            this.labelStability.Location = new System.Drawing.Point(243, 59);
            this.labelStability.Name = "labelStability";
            this.labelStability.Size = new System.Drawing.Size(109, 16);
            this.labelStability.TabIndex = 6;
            this.labelStability.Text = "Auto-Tune Delay";
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSpeed.ForeColor = System.Drawing.Color.Red;
            this.labelSpeed.Location = new System.Drawing.Point(243, 109);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(102, 16);
            this.labelSpeed.TabIndex = 7;
            this.labelSpeed.Text = "Auto-Tune Rate";
            // 
            // TwinLEDDisplay
            // 
            this.TwinLEDDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.TwinLEDDisplay.Location = new System.Drawing.Point(12, 10);
            this.TwinLEDDisplay.Name = "TwinLEDDisplay";
            this.TwinLEDDisplay.Size = new System.Drawing.Size(320, 40);
            this.TwinLEDDisplay.TabIndex = 2;
            // 
            // checkBoxEnable
            // 
            this.checkBoxEnable.AutoSize = true;
            this.checkBoxEnable.Location = new System.Drawing.Point(112, 125);
            this.checkBoxEnable.Name = "checkBoxEnable";
            this.checkBoxEnable.Size = new System.Drawing.Size(15, 14);
            this.checkBoxEnable.TabIndex = 8;
            this.checkBoxEnable.UseVisualStyleBackColor = true;
            // 
            // tclsTuningTargetDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(364, 156);
            this.Controls.Add(this.checkBoxEnable);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.labelStability);
            this.Controls.Add(this.trackBarSpeed);
            this.Controls.Add(this.trackBarStability);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.TwinLEDDisplay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tclsTuningTargetDisplay";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tuning Target";
            this.Load += new System.EventHandler(this.tclsTuningTargetDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarStability)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private tclsTwinLEDDisplay TwinLEDDisplay;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TrackBar trackBarStability;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.Label labelStability;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.CheckBox checkBoxEnable;
    }
}