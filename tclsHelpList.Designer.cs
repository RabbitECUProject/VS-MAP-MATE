namespace UDP
{
    partial class tclsHelpList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsHelpList));
            this.tableLayoutPanelCharHelp = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tableLayoutPanelCharHelp
            // 
            this.tableLayoutPanelCharHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tableLayoutPanelCharHelp.ColumnCount = 3;
            this.tableLayoutPanelCharHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanelCharHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanelCharHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelCharHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCharHelp.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanelCharHelp.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelCharHelp.Name = "tableLayoutPanelCharHelp";
            this.tableLayoutPanelCharHelp.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanelCharHelp.RowCount = 1;
            this.tableLayoutPanelCharHelp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 440F));
            this.tableLayoutPanelCharHelp.Size = new System.Drawing.Size(790, 440);
            this.tableLayoutPanelCharHelp.TabIndex = 0;
            // 
            // tclsHelpList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanelCharHelp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tclsHelpList";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Help List";
            this.Load += new System.EventHandler(this.tclsHelpList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCharHelp;
    }
}