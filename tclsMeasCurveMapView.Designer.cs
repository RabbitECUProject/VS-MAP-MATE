namespace UDP
{
    partial class tclsMeasCurveMapView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsMeasCurveMapView));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CurveMapToolstrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRotLeft = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRotRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSelectCol = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSelectRow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSelectTable = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonColorPicker = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPlusMinus = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMultiply = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEquals = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.timerCellColour = new System.Windows.Forms.Timer(this.components);
            this.timerAutoTune = new System.Windows.Forms.Timer(this.components);
            this.toolStripButtonAxes = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.CurveMapToolstrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.CurveMapToolstrip, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(798, 351);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // CurveMapToolstrip
            // 
            this.CurveMapToolstrip.AutoSize = false;
            this.CurveMapToolstrip.GripMargin = new System.Windows.Forms.Padding(1);
            this.CurveMapToolstrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.CurveMapToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonZoomIn,
            this.toolStripButtonZoomOut,
            this.toolStripButtonRotLeft,
            this.toolStripButtonRotRight,
            this.toolStripSeparator2,
            this.toolStripButtonSelectCol,
            this.toolStripButtonSelectRow,
            this.toolStripButtonSelectTable,
            this.toolStripButtonColorPicker,
            this.toolStripButtonAxes,
            this.toolStripSeparator1,
            this.toolStripButtonPrevious,
            this.toolStripButtonNext,
            this.toolStripSeparator3,
            this.toolStripButtonUp,
            this.toolStripButtonDown,
            this.toolStripSeparator4,
            this.toolStripButtonPlusMinus,
            this.toolStripButtonMultiply,
            this.toolStripButtonEquals,
            this.toolStripSeparator5,
            this.toolStripButtonHelp});
            this.CurveMapToolstrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.CurveMapToolstrip.Location = new System.Drawing.Point(0, 0);
            this.CurveMapToolstrip.Name = "CurveMapToolstrip";
            this.CurveMapToolstrip.Size = new System.Drawing.Size(798, 36);
            this.CurveMapToolstrip.Stretch = true;
            this.CurveMapToolstrip.TabIndex = 1;
            this.CurveMapToolstrip.Text = "toolStrip1";
            this.CurveMapToolstrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CurveMapToolstrip_ItemClicked_1);
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomIn.Image")));
            this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonZoomIn.Text = "+";
            this.toolStripButtonZoomIn.ToolTipText = "Zoom In";
            this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomOut.Image")));
            this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonZoomOut.Text = "-";
            this.toolStripButtonZoomOut.ToolTipText = "Zoom Out";
            this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
            // 
            // toolStripButtonRotLeft
            // 
            this.toolStripButtonRotLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRotLeft.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRotLeft.Image")));
            this.toolStripButtonRotLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRotLeft.Name = "toolStripButtonRotLeft";
            this.toolStripButtonRotLeft.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonRotLeft.Text = "<";
            this.toolStripButtonRotLeft.ToolTipText = "Rotate Left";
            this.toolStripButtonRotLeft.Click += new System.EventHandler(this.toolStripButtonRotLeft_Click);
            // 
            // toolStripButtonRotRight
            // 
            this.toolStripButtonRotRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRotRight.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRotRight.Image")));
            this.toolStripButtonRotRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRotRight.Name = "toolStripButtonRotRight";
            this.toolStripButtonRotRight.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonRotRight.Text = ">";
            this.toolStripButtonRotRight.ToolTipText = "Rotate Right";
            this.toolStripButtonRotRight.Click += new System.EventHandler(this.toolStripButtonRotRight_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonSelectCol
            // 
            this.toolStripButtonSelectCol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectCol.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectCol.Image")));
            this.toolStripButtonSelectCol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectCol.Name = "toolStripButtonSelectCol";
            this.toolStripButtonSelectCol.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSelectCol.ToolTipText = "Select Column";
            this.toolStripButtonSelectCol.Click += new System.EventHandler(this.toolStripButtonSelectCol_Click);
            // 
            // toolStripButtonSelectRow
            // 
            this.toolStripButtonSelectRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectRow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectRow.Image")));
            this.toolStripButtonSelectRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectRow.Name = "toolStripButtonSelectRow";
            this.toolStripButtonSelectRow.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSelectRow.ToolTipText = "Select Row";
            this.toolStripButtonSelectRow.Click += new System.EventHandler(this.toolStripButtonSelectRow_Click);
            // 
            // toolStripButtonSelectTable
            // 
            this.toolStripButtonSelectTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectTable.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectTable.Image")));
            this.toolStripButtonSelectTable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectTable.Name = "toolStripButtonSelectTable";
            this.toolStripButtonSelectTable.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSelectTable.ToolTipText = "Select Table";
            this.toolStripButtonSelectTable.Click += new System.EventHandler(this.toolStripButtonSelectTable_Click);
            // 
            // toolStripButtonColorPicker
            // 
            this.toolStripButtonColorPicker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonColorPicker.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonColorPicker.Image")));
            this.toolStripButtonColorPicker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonColorPicker.Name = "toolStripButtonColorPicker";
            this.toolStripButtonColorPicker.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonColorPicker.ToolTipText = "Colour Settings";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonPrevious
            // 
            this.toolStripButtonPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrevious.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrevious.Image")));
            this.toolStripButtonPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrevious.Name = "toolStripButtonPrevious";
            this.toolStripButtonPrevious.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonPrevious.Text = "Previous";
            this.toolStripButtonPrevious.Click += new System.EventHandler(this.toolStripButtonPrevious_Click);
            // 
            // toolStripButtonNext
            // 
            this.toolStripButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNext.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNext.Image")));
            this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNext.Name = "toolStripButtonNext";
            this.toolStripButtonNext.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonNext.Text = "toolStripButtonNext";
            this.toolStripButtonNext.ToolTipText = "Next";
            this.toolStripButtonNext.Click += new System.EventHandler(this.toolStripButtonNext_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonUp
            // 
            this.toolStripButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUp.Image")));
            this.toolStripButtonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUp.Name = "toolStripButtonUp";
            this.toolStripButtonUp.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonUp.Text = "toolStripButtonUp";
            this.toolStripButtonUp.ToolTipText = "Divider Up";
            this.toolStripButtonUp.Click += new System.EventHandler(this.toolStripButtonUp_Click);
            // 
            // toolStripButtonDown
            // 
            this.toolStripButtonDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDown.Image")));
            this.toolStripButtonDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDown.Name = "toolStripButtonDown";
            this.toolStripButtonDown.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonDown.Text = "toolStripButtonDown";
            this.toolStripButtonDown.ToolTipText = "Divider Down";
            this.toolStripButtonDown.Click += new System.EventHandler(this.toolStripButtonDown_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonPlusMinus
            // 
            this.toolStripButtonPlusMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPlusMinus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPlusMinus.Image")));
            this.toolStripButtonPlusMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPlusMinus.Name = "toolStripButtonPlusMinus";
            this.toolStripButtonPlusMinus.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonPlusMinus.Text = "toolStripButtonPlusMinus";
            this.toolStripButtonPlusMinus.ToolTipText = "Add/Subtract";
            this.toolStripButtonPlusMinus.Click += new System.EventHandler(this.toolStripButtonPlusMinus_Click_1);
            // 
            // toolStripButtonMultiply
            // 
            this.toolStripButtonMultiply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMultiply.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMultiply.Image")));
            this.toolStripButtonMultiply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMultiply.Name = "toolStripButtonMultiply";
            this.toolStripButtonMultiply.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonMultiply.Text = "toolStripButtonMultiply";
            this.toolStripButtonMultiply.ToolTipText = "Multiply/Divide";
            this.toolStripButtonMultiply.Click += new System.EventHandler(this.toolStripButtonMultiply_Click_1);
            // 
            // toolStripButtonEquals
            // 
            this.toolStripButtonEquals.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEquals.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEquals.Image")));
            this.toolStripButtonEquals.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEquals.Name = "toolStripButtonEquals";
            this.toolStripButtonEquals.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonEquals.Text = "toolStripButtonEquals";
            this.toolStripButtonEquals.ToolTipText = "Enter Value";
            this.toolStripButtonEquals.Click += new System.EventHandler(this.toolStripButtonEquals_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonHelp
            // 
            this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHelp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHelp.Image")));
            this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            this.toolStripButtonHelp.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonHelp.Text = "toolStripButton1";
            this.toolStripButtonHelp.ToolTipText = "Help";
            this.toolStripButtonHelp.Click += new System.EventHandler(this.toolStripButtonHelp_Click);
            // 
            // timerCellColour
            // 
            this.timerCellColour.Enabled = true;
            this.timerCellColour.Interval = 200;
            this.timerCellColour.Tick += new System.EventHandler(this.timerCellColour_Tick);
            // 
            // timerAutoTune
            // 
            this.timerAutoTune.Enabled = true;
            this.timerAutoTune.Interval = 1000;
            this.timerAutoTune.Tick += new System.EventHandler(this.timerAutoTune_Tick);
            // 
            // toolStripButtonAxes
            // 
            this.toolStripButtonAxes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAxes.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAxes.Image")));
            this.toolStripButtonAxes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAxes.Name = "toolStripButtonAxes";
            this.toolStripButtonAxes.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonAxes.Text = "toolStripButtonaxes";
            this.toolStripButtonAxes.Click += new System.EventHandler(this.toolStripButtonAxes_Click);
            // 
            // tclsMeasCurveMapView
            // 
            this.ClientSize = new System.Drawing.Size(798, 351);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "tclsMeasCurveMapView";
            this.Activated += new System.EventHandler(this.FocusActivated);
            this.Deactivate += new System.EventHandler(this.tclsMeasCurveMapView_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.form_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.form_keyUp);
            this.Resize += new System.EventHandler(this.tclsMeasCurveMapView_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.CurveMapToolstrip.ResumeLayout(false);
            this.CurveMapToolstrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip CurveMapToolstrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotLeft;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotRight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectCol;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectRow;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectTable;
        private System.Windows.Forms.ToolStripButton toolStripButtonColorPicker;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrevious;
        private System.Windows.Forms.ToolStripButton toolStripButtonNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonPlusMinus;
        private System.Windows.Forms.ToolStripButton toolStripButtonMultiply;
        private System.Windows.Forms.ToolStripButton toolStripButtonEquals;
        private System.Windows.Forms.Timer timerCellColour;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonHelp;
		private System.Windows.Forms.Timer timerAutoTune;
        private System.Windows.Forms.ToolStripButton toolStripButtonAxes;
    }
}