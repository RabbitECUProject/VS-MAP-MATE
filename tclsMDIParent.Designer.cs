namespace UDP
{
    partial class tclsMDIParent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsMDIParent));
            this.DataPageTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MDIParentStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButtonApplication = new System.Windows.Forms.ToolStripDropDownButton();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButtonTransfer = new System.Windows.Forms.ToolStripSplitButton();
            this.StripMenuItemUploadCal = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuItemDownloadCal = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuItemfreezeCalToNVM = new System.Windows.Forms.ToolStripMenuItem();
            this.clearNVMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonFile = new System.Windows.Forms.ToolStripSplitButton();
            this.openCalImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonSettings = new System.Windows.Forms.ToolStripSplitButton();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonUserControls = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonTools = new System.Windows.Forms.ToolStripSplitButton();
            this.replayScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitView = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.MDIStatusStrip = new System.Windows.Forms.StatusStrip();
            this.ConnectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.ProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ConnectionImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStripSplitHelp = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.MDIParentStrip.SuspendLayout();
            this.MDIStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataPageTimer
            // 
            this.DataPageTimer.Enabled = true;
            this.DataPageTimer.Interval = 1000;
            this.DataPageTimer.Tick += new System.EventHandler(this.DataPageTimer_Tick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MDIParentStrip
            // 
            this.MDIParentStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonApplication,
            this.toolStripSeparator1,
            this.toolStripSplitButtonTransfer,
            this.toolStripSeparator2,
            this.toolStripSplitButtonFile,
            this.toolStripSeparator3,
            this.toolStripSplitButtonSettings,
            this.toolStripSeparator4,
            this.toolStripSplitButtonUserControls,
            this.toolStripSeparator5,
            this.toolStripSplitButtonTools,
            this.toolStripSeparator6,
            this.toolStripSplitView,
            this.toolStripSplitHelp});
            this.MDIParentStrip.Location = new System.Drawing.Point(0, 0);
            this.MDIParentStrip.Name = "MDIParentStrip";
            this.MDIParentStrip.Size = new System.Drawing.Size(848, 25);
            this.MDIParentStrip.TabIndex = 1;
            this.MDIParentStrip.Text = "User Controls";
            // 
            // toolStripDropDownButtonApplication
            // 
            this.toolStripDropDownButtonApplication.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonApplication.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.toolStripDropDownButtonApplication.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonApplication.Image")));
            this.toolStripDropDownButtonApplication.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonApplication.Name = "toolStripDropDownButtonApplication";
            this.toolStripDropDownButtonApplication.Size = new System.Drawing.Size(81, 22);
            this.toolStripDropDownButtonApplication.Text = "Application";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripSplitButtonTransfer
            // 
            this.toolStripSplitButtonTransfer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButtonTransfer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenuItemUploadCal,
            this.StripMenuItemDownloadCal,
            this.StripMenuItemfreezeCalToNVM,
            this.clearNVMToolStripMenuItem});
            this.toolStripSplitButtonTransfer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonTransfer.Image")));
            this.toolStripSplitButtonTransfer.ImageTransparentColor = System.Drawing.Color.Gainsboro;
            this.toolStripSplitButtonTransfer.Name = "toolStripSplitButtonTransfer";
            this.toolStripSplitButtonTransfer.Size = new System.Drawing.Size(117, 22);
            this.toolStripSplitButtonTransfer.Text = "Calibration Image";
            // 
            // StripMenuItemUploadCal
            // 
            this.StripMenuItemUploadCal.Name = "StripMenuItemUploadCal";
            this.StripMenuItemUploadCal.Size = new System.Drawing.Size(171, 22);
            this.StripMenuItemUploadCal.Text = "Upload Cal";
            this.StripMenuItemUploadCal.Click += new System.EventHandler(this.StripMenuItemUploadCal_Click);
            // 
            // StripMenuItemDownloadCal
            // 
            this.StripMenuItemDownloadCal.Name = "StripMenuItemDownloadCal";
            this.StripMenuItemDownloadCal.Size = new System.Drawing.Size(171, 22);
            this.StripMenuItemDownloadCal.Text = "Download Cal";
            this.StripMenuItemDownloadCal.Click += new System.EventHandler(this.StripMenuItemDownloadCal_Click);
            // 
            // StripMenuItemfreezeCalToNVM
            // 
            this.StripMenuItemfreezeCalToNVM.Name = "StripMenuItemfreezeCalToNVM";
            this.StripMenuItemfreezeCalToNVM.Size = new System.Drawing.Size(171, 22);
            this.StripMenuItemfreezeCalToNVM.Text = "Freeze Cal to NVM";
            this.StripMenuItemfreezeCalToNVM.Click += new System.EventHandler(this.StripMenuItemFreezeCalToNVMTool_Click);
            // 
            // clearNVMToolStripMenuItem
            // 
            this.clearNVMToolStripMenuItem.Name = "clearNVMToolStripMenuItem";
            this.clearNVMToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.clearNVMToolStripMenuItem.Text = "Clear NVM";
            this.clearNVMToolStripMenuItem.Click += new System.EventHandler(this.clearNVMToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButtonFile
            // 
            this.toolStripSplitButtonFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButtonFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCalImageToolStripMenuItem,
            this.saveCalToolStripMenuItem});
            this.toolStripSplitButtonFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonFile.Image")));
            this.toolStripSplitButtonFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonFile.Name = "toolStripSplitButtonFile";
            this.toolStripSplitButtonFile.Size = new System.Drawing.Size(102, 22);
            this.toolStripSplitButtonFile.Text = "Calibration File";
            // 
            // openCalImageToolStripMenuItem
            // 
            this.openCalImageToolStripMenuItem.Name = "openCalImageToolStripMenuItem";
            this.openCalImageToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openCalImageToolStripMenuItem.Text = "Open Cal";
            this.openCalImageToolStripMenuItem.Click += new System.EventHandler(this.openCalImageToolStripMenuItem_Click);
            // 
            // saveCalToolStripMenuItem
            // 
            this.saveCalToolStripMenuItem.Name = "saveCalToolStripMenuItem";
            this.saveCalToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveCalToolStripMenuItem.Text = "Save Cal";
            this.saveCalToolStripMenuItem.Click += new System.EventHandler(this.saveCalToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButtonSettings
            // 
            this.toolStripSplitButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButtonSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem,
            this.configurationToolStripMenuItem});
            this.toolStripSplitButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonSettings.Image")));
            this.toolStripSplitButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonSettings.Name = "toolStripSplitButtonSettings";
            this.toolStripSplitButtonSettings.Size = new System.Drawing.Size(65, 22);
            this.toolStripSplitButtonSettings.Text = "Settings";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.programToolStripMenuItem.Text = "Program";
            this.programToolStripMenuItem.Click += new System.EventHandler(this.programToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButtonUserControls
            // 
            this.toolStripSplitButtonUserControls.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButtonUserControls.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.toolStripSplitButtonUserControls.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonUserControls.Name = "toolStripSplitButtonUserControls";
            this.toolStripSplitButtonUserControls.Size = new System.Drawing.Size(94, 22);
            this.toolStripSplitButtonUserControls.Text = "User Controls";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem1.Text = "Routine Control";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButtonTools
            // 
            this.toolStripSplitButtonTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButtonTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replayScriptToolStripMenuItem,
            this.clearMessagesToolStripMenuItem});
            this.toolStripSplitButtonTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonTools.Name = "toolStripSplitButtonTools";
            this.toolStripSplitButtonTools.Size = new System.Drawing.Size(51, 22);
            this.toolStripSplitButtonTools.Text = "Tools";
            // 
            // replayScriptToolStripMenuItem
            // 
            this.replayScriptToolStripMenuItem.Name = "replayScriptToolStripMenuItem";
            this.replayScriptToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.replayScriptToolStripMenuItem.Text = "Replay Script";
            this.replayScriptToolStripMenuItem.Click += new System.EventHandler(this.replayScriptToolStripMenuItem_Click);
            // 
            // clearMessagesToolStripMenuItem
            // 
            this.clearMessagesToolStripMenuItem.Name = "clearMessagesToolStripMenuItem";
            this.clearMessagesToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.clearMessagesToolStripMenuItem.Text = "Clear Messages";
            this.clearMessagesToolStripMenuItem.Click += new System.EventHandler(this.clearMessagesToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitView
            // 
            this.toolStripSplitView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.toolStripSplitView.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitView.Image")));
            this.toolStripSplitView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitView.Name = "toolStripSplitView";
            this.toolStripSplitView.Size = new System.Drawing.Size(48, 22);
            this.toolStripSplitView.Text = "View";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem2.Text = "Arrange Default";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // MDIStatusStrip
            // 
            this.MDIStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectionStatusLabel,
            this.StatusLabel,
            this.ProgressBar,
            this.ProgressLabel});
            this.MDIStatusStrip.Location = new System.Drawing.Point(0, 350);
            this.MDIStatusStrip.Name = "MDIStatusStrip";
            this.MDIStatusStrip.Size = new System.Drawing.Size(848, 22);
            this.MDIStatusStrip.TabIndex = 3;
            this.MDIStatusStrip.Text = "statusStrip1";
            this.MDIStatusStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MDIStatusStrip_ItemClicked);
            // 
            // ConnectionStatusLabel
            // 
            this.ConnectionStatusLabel.AutoSize = false;
            this.ConnectionStatusLabel.Name = "ConnectionStatusLabel";
            this.ConnectionStatusLabel.Size = new System.Drawing.Size(80, 17);
            this.ConnectionStatusLabel.Text = "ONLINE";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = false;
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(160, 17);
            this.StatusLabel.Text = "Status ...";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.StatusLabel.Click += new System.EventHandler(this.StatusLabel_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.AutoSize = false;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(180, 16);
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = false;
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(240, 17);
            this.ProgressLabel.Text = "Progress ...";
            this.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConnectionImageList
            // 
            this.ConnectionImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ConnectionImageList.ImageStream")));
            this.ConnectionImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ConnectionImageList.Images.SetKeyName(0, "CONNECTED_ICON.bmp");
            this.ConnectionImageList.Images.SetKeyName(1, "DISCONNECTED_ICON.bmp");
            // 
            // toolStripSplitHelp
            // 
            this.toolStripSplitHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.toolStripSplitHelp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitHelp.Image")));
            this.toolStripSplitHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitHelp.Name = "toolStripSplitHelp";
            this.toolStripSplitHelp.Size = new System.Drawing.Size(48, 22);
            this.toolStripSplitHelp.Text = "&Help";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem3.Text = "MAP-MATE Manual";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // tclsMDIParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::UDP.Properties.Resources.GlyphRight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(848, 372);
            this.Controls.Add(this.MDIStatusStrip);
            this.Controls.Add(this.MDIParentStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "tclsMDIParent";
            this.Text = "MDAC MAP-MATE Calibration";
            this.Load += new System.EventHandler(this.FormUDP_Load);
            this.Resize += new System.EventHandler(this.tclsMDIParent_Resize);
            this.MDIParentStrip.ResumeLayout(false);
            this.MDIParentStrip.PerformLayout();
            this.MDIStatusStrip.ResumeLayout(false);
            this.MDIStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer DataPageTimer;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonApplication;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonTransfer;
        private System.Windows.Forms.ToolStripMenuItem StripMenuItemUploadCal;
        private System.Windows.Forms.ToolStripMenuItem StripMenuItemDownloadCal;
        private System.Windows.Forms.ToolStripMenuItem StripMenuItemfreezeCalToNVM;
        private System.Windows.Forms.ToolStripMenuItem clearNVMToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonFile;
        private System.Windows.Forms.ToolStripMenuItem openCalImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCalToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonSettings;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStrip MDIParentStrip;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonUserControls;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonTools;
        private System.Windows.Forms.ToolStripMenuItem replayScriptToolStripMenuItem;
        private System.Windows.Forms.StatusStrip MDIStatusStrip;
        private System.Windows.Forms.ImageList ConnectionImageList;
        private System.Windows.Forms.ToolStripStatusLabel ConnectionStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel ProgressLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem clearMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
    }
}

