namespace UDP
{
    partial class tclsMeasValueLoggingView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsMeasValueLoggingView));
            this.timerGridUpdate = new System.Windows.Forms.Timer(this.components);
            this.buttonLogAdd = new System.Windows.Forms.Button();
            this.tableLayoutPanelLogging = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLogSave = new System.Windows.Forms.Button();
            this.progressBarLogBuffer = new System.Windows.Forms.ProgressBar();
            this.comboBoxLoggingVars = new System.Windows.Forms.ComboBox();
            this.dataGridViewLogging = new System.Windows.Forms.DataGridView();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonSplitterUp = new System.Windows.Forms.Button();
            this.buttonSplitterDown = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.tableLayoutPanelLogging.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLogging)).BeginInit();
            this.SuspendLayout();
            // 
            // timerGridUpdate
            // 
            this.timerGridUpdate.Enabled = true;
            this.timerGridUpdate.Interval = 500;
            this.timerGridUpdate.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonLogAdd
            // 
            this.buttonLogAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogAdd.Location = new System.Drawing.Point(533, 3);
            this.buttonLogAdd.Name = "buttonLogAdd";
            this.buttonLogAdd.Size = new System.Drawing.Size(88, 36);
            this.buttonLogAdd.TabIndex = 2;
            this.buttonLogAdd.Text = "Add/Remove";
            this.buttonLogAdd.UseVisualStyleBackColor = true;
            this.buttonLogAdd.Click += new System.EventHandler(this.buttonLogAdd_Click);
            // 
            // tableLayoutPanelLogging
            // 
            this.tableLayoutPanelLogging.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanelLogging.ColumnCount = 9;
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogging.Controls.Add(this.buttonLogSave, 0, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.buttonLogAdd, 3, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.progressBarLogBuffer, 1, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.comboBoxLoggingVars, 2, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.dataGridViewLogging, 0, 2);
            this.tableLayoutPanelLogging.Controls.Add(this.buttonZoomIn, 4, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.button1, 5, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.buttonSplitterUp, 6, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.buttonSplitterDown, 7, 0);
            this.tableLayoutPanelLogging.Controls.Add(this.buttonReset, 8, 0);
            this.tableLayoutPanelLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLogging.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLogging.Name = "tableLayoutPanelLogging";
            this.tableLayoutPanelLogging.RowCount = 3;
            this.tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelLogging.Size = new System.Drawing.Size(1005, 359);
            this.tableLayoutPanelLogging.TabIndex = 4;
            // 
            // buttonLogSave
            // 
            this.buttonLogSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonLogSave.BackgroundImage")));
            this.buttonLogSave.Location = new System.Drawing.Point(3, 3);
            this.buttonLogSave.Name = "buttonLogSave";
            this.buttonLogSave.Size = new System.Drawing.Size(36, 36);
            this.buttonLogSave.TabIndex = 4;
            this.buttonLogSave.UseVisualStyleBackColor = true;
            this.buttonLogSave.Click += new System.EventHandler(this.buttonLogSave_Click);
            // 
            // progressBarLogBuffer
            // 
            this.progressBarLogBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarLogBuffer.Location = new System.Drawing.Point(45, 11);
            this.progressBarLogBuffer.Name = "progressBarLogBuffer";
            this.progressBarLogBuffer.Size = new System.Drawing.Size(216, 20);
            this.progressBarLogBuffer.TabIndex = 5;
            // 
            // comboBoxLoggingVars
            // 
            this.comboBoxLoggingVars.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLoggingVars.BackColor = System.Drawing.Color.Black;
            this.comboBoxLoggingVars.ForeColor = System.Drawing.Color.Cyan;
            this.comboBoxLoggingVars.FormattingEnabled = true;
            this.comboBoxLoggingVars.Location = new System.Drawing.Point(267, 10);
            this.comboBoxLoggingVars.Name = "comboBoxLoggingVars";
            this.comboBoxLoggingVars.Size = new System.Drawing.Size(260, 21);
            this.comboBoxLoggingVars.TabIndex = 6;
            this.comboBoxLoggingVars.SelectedIndexChanged += new System.EventHandler(this.comboBoxLoggingVars_SelectedIndexChanged);
            // 
            // dataGridViewLogging
            // 
            this.dataGridViewLogging.AllowUserToAddRows = false;
            this.dataGridViewLogging.AllowUserToDeleteRows = false;
            this.dataGridViewLogging.AllowUserToResizeColumns = false;
            this.dataGridViewLogging.AllowUserToResizeRows = false;
            this.dataGridViewLogging.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLogging.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewLogging.BackgroundColor = System.Drawing.Color.DarkSlateGray;
            this.dataGridViewLogging.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelLogging.SetColumnSpan(this.dataGridViewLogging, 9);
            this.dataGridViewLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLogging.Location = new System.Drawing.Point(3, 235);
            this.dataGridViewLogging.Name = "dataGridViewLogging";
            this.dataGridViewLogging.Size = new System.Drawing.Size(999, 121);
            this.dataGridViewLogging.TabIndex = 7;
            this.dataGridViewLogging.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewLogging_DataBindingComplete);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomIn.AutoSize = true;
            this.buttonZoomIn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonZoomIn.BackgroundImage")));
            this.buttonZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonZoomIn.Location = new System.Drawing.Point(627, 3);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(36, 36);
            this.buttonZoomIn.TabIndex = 8;
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.Location = new System.Drawing.Point(669, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 36);
            this.button1.TabIndex = 9;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonSplitterUp
            // 
            this.buttonSplitterUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSplitterUp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonSplitterUp.BackgroundImage")));
            this.buttonSplitterUp.Location = new System.Drawing.Point(711, 3);
            this.buttonSplitterUp.Name = "buttonSplitterUp";
            this.buttonSplitterUp.Size = new System.Drawing.Size(36, 36);
            this.buttonSplitterUp.TabIndex = 10;
            this.buttonSplitterUp.UseVisualStyleBackColor = true;
            this.buttonSplitterUp.Click += new System.EventHandler(this.buttonSplitterUp_Click);
            // 
            // buttonSplitterDown
            // 
            this.buttonSplitterDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSplitterDown.AutoSize = true;
            this.buttonSplitterDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonSplitterDown.BackgroundImage")));
            this.buttonSplitterDown.Location = new System.Drawing.Point(753, 3);
            this.buttonSplitterDown.Name = "buttonSplitterDown";
            this.buttonSplitterDown.Size = new System.Drawing.Size(36, 36);
            this.buttonSplitterDown.TabIndex = 11;
            this.buttonSplitterDown.UseVisualStyleBackColor = true;
            this.buttonSplitterDown.Click += new System.EventHandler(this.buttonSplitterDown_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Image = ((System.Drawing.Image)(resources.GetObject("buttonReset.Image")));
            this.buttonReset.Location = new System.Drawing.Point(795, 3);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(36, 36);
            this.buttonReset.TabIndex = 12;
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // tclsMeasValueLoggingView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 359);
            this.Controls.Add(this.tableLayoutPanelLogging);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "tclsMeasValueLoggingView";
            this.Text = "tclsMeasValueLoggingView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.tclsMeasValueLoggingView_Load);
            this.Resize += new System.EventHandler(this.tclsMeasValueLoggingView_Resize);
            this.tableLayoutPanelLogging.ResumeLayout(false);
            this.tableLayoutPanelLogging.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLogging)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerGridUpdate;
        private System.Windows.Forms.Button buttonLogAdd;
        private System.Windows.Forms.Button buttonSplitterUp;
        private System.Windows.Forms.Button buttonSplitterDown;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogging;
        private System.Windows.Forms.Button buttonLogSave;
        private System.Windows.Forms.ProgressBar progressBarLogBuffer;
        private System.Windows.Forms.ComboBox comboBoxLoggingVars;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridViewLogging;
    }
}