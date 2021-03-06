namespace UDP
{
    partial class tclsMeasureSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsMeasureSelect));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGaugeAdd = new System.Windows.Forms.Button();
            this.comboBoxGaugeVars = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.94033F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.05967F));
            this.tableLayoutPanel.Controls.Add(this.buttonGaugeAdd, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxGaugeVars, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(553, 67);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttonGaugeAdd
            // 
            this.buttonGaugeAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGaugeAdd.Location = new System.Drawing.Point(340, 15);
            this.buttonGaugeAdd.Name = "buttonGaugeAdd";
            this.buttonGaugeAdd.Size = new System.Drawing.Size(210, 36);
            this.buttonGaugeAdd.TabIndex = 7;
            this.buttonGaugeAdd.Text = "Add/Remove";
            this.buttonGaugeAdd.UseVisualStyleBackColor = true;
            this.buttonGaugeAdd.Click += new System.EventHandler(this.buttonGaugeAdd_Click);
            // 
            // comboBoxGaugeVars
            // 
            this.comboBoxGaugeVars.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxGaugeVars.BackColor = System.Drawing.Color.Black;
            this.comboBoxGaugeVars.ForeColor = System.Drawing.Color.Cyan;
            this.comboBoxGaugeVars.FormattingEnabled = true;
            this.comboBoxGaugeVars.Location = new System.Drawing.Point(3, 23);
            this.comboBoxGaugeVars.Name = "comboBoxGaugeVars";
            this.comboBoxGaugeVars.Size = new System.Drawing.Size(331, 21);
            this.comboBoxGaugeVars.TabIndex = 8;
            // 
            // tclsMeasureSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(553, 67);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tclsMeasureSelect";
            this.Text = "Select Gauge Data";
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonGaugeAdd;
        private System.Windows.Forms.ComboBox comboBoxGaugeVars;
    }
}