namespace UDP
{
    partial class tclsNavGraphicalTreeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsNavGraphicalTreeView));
            this.imageListNodes = new System.Windows.Forms.ImageList(this.components);
            this.navTreeViewGraphicalNodes = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // imageListNodes
            // 
            this.imageListNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListNodes.ImageStream")));
            this.imageListNodes.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListNodes.Images.SetKeyName(0, "SETTING_36.bmp");
            this.imageListNodes.Images.SetKeyName(1, "CURVE_36.bmp");
            this.imageListNodes.Images.SetKeyName(2, "MAP_36.bmp");
            this.imageListNodes.Images.SetKeyName(3, "LOGGING_36.bmp");
            this.imageListNodes.Images.SetKeyName(4, "GAUGE_36.bmp");
            this.imageListNodes.Images.SetKeyName(5, "BLOB_36.bmp");
            this.imageListNodes.Images.SetKeyName(6, "SEGMENTS_36.bmp");
            this.imageListNodes.Images.SetKeyName(7, "LOGIC_36.bmp");
            this.imageListNodes.Images.SetKeyName(8, "DSG_36.bmp");
            this.imageListNodes.Images.SetKeyName(9, "SETTING_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(10, "CURVE_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(11, "MAP_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(12, "LOGGING_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(13, "GAUGE_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(14, "BLOB_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(15, "SEGMENTS_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(16, "LOGIC_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(17, "DSG_36_MONO.bmp");
            this.imageListNodes.Images.SetKeyName(18, "ECU_24.bmp");
            // 
            // navTreeViewGraphicalNodes
            // 
            this.navTreeViewGraphicalNodes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.navTreeViewGraphicalNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navTreeViewGraphicalNodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navTreeViewGraphicalNodes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.navTreeViewGraphicalNodes.ImageIndex = 0;
            this.navTreeViewGraphicalNodes.ImageList = this.imageListNodes;
            this.navTreeViewGraphicalNodes.Location = new System.Drawing.Point(0, 0);
            this.navTreeViewGraphicalNodes.Name = "navTreeViewGraphicalNodes";
            this.navTreeViewGraphicalNodes.SelectedImageIndex = 0;
            this.navTreeViewGraphicalNodes.Size = new System.Drawing.Size(438, 450);
            this.navTreeViewGraphicalNodes.TabIndex = 0;
            this.navTreeViewGraphicalNodes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.navTreeView_AfterSelect);
            // 
            // tclsNavGraphicalTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 450);
            this.Controls.Add(this.navTreeViewGraphicalNodes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tclsNavGraphicalTreeView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Navigation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.tclsNavGraphicalTreeView_Load);
            this.LocationChanged += new System.EventHandler(this.tclsNavTree_LocationChanged);
            this.Resize += new System.EventHandler(this.tclsNavTreeView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListNodes;
        private System.Windows.Forms.TreeView navTreeViewGraphicalNodes;
    }
}