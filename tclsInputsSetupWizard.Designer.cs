namespace UDP
{
    partial class tclsInputsSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsInputsSetupWizard));
            this.groupBoxCrank = new System.Windows.Forms.GroupBox();
            this.comboBoxTriggerSensorType = new System.Windows.Forms.ComboBox();
            this.comboBoxTriggerSensorStrength = new System.Windows.Forms.ComboBox();
            this.comboBoxCamSensorStrength = new System.Windows.Forms.ComboBox();
            this.comboBoxSyncSensorType = new System.Windows.Forms.ComboBox();
            this.groupBoxCam = new System.Windows.Forms.GroupBox();
            this.groupBoxAnalog = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelCTS = new System.Windows.Forms.Label();
            this.labelATS = new System.Windows.Forms.Label();
            this.labelTPS = new System.Windows.Forms.Label();
            this.labelMAP = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxCTS = new System.Windows.Forms.ComboBox();
            this.comboBoxATS = new System.Windows.Forms.ComboBox();
            this.comboBoxTPS = new System.Windows.Forms.ComboBox();
            this.comboBoxMAP = new System.Windows.Forms.ComboBox();
            this.comboBoxAFM = new System.Windows.Forms.ComboBox();
            this.checkBoxCTSCAN = new System.Windows.Forms.CheckBox();
            this.checkBoxATSCAN = new System.Windows.Forms.CheckBox();
            this.checkBoxTPSCAN = new System.Windows.Forms.CheckBox();
            this.checkBoxMAPCAN = new System.Windows.Forms.CheckBox();
            this.checkBoxPPSCAN = new System.Windows.Forms.CheckBox();
            this.groupBoxCrank.SuspendLayout();
            this.groupBoxCam.SuspendLayout();
            this.groupBoxAnalog.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCrank
            // 
            this.groupBoxCrank.Controls.Add(this.comboBoxTriggerSensorStrength);
            this.groupBoxCrank.Controls.Add(this.comboBoxTriggerSensorType);
            this.groupBoxCrank.Location = new System.Drawing.Point(12, 12);
            this.groupBoxCrank.Name = "groupBoxCrank";
            this.groupBoxCrank.Size = new System.Drawing.Size(619, 60);
            this.groupBoxCrank.TabIndex = 0;
            this.groupBoxCrank.TabStop = false;
            this.groupBoxCrank.Text = "Crank Trigger";
            // 
            // comboBoxTriggerSensorType
            // 
            this.comboBoxTriggerSensorType.FormattingEnabled = true;
            this.comboBoxTriggerSensorType.Location = new System.Drawing.Point(6, 19);
            this.comboBoxTriggerSensorType.Name = "comboBoxTriggerSensorType";
            this.comboBoxTriggerSensorType.Size = new System.Drawing.Size(172, 21);
            this.comboBoxTriggerSensorType.TabIndex = 0;
            this.comboBoxTriggerSensorType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTriggerSensorType_SelectedIndexChanged);
            // 
            // comboBoxTriggerSensorStrength
            // 
            this.comboBoxTriggerSensorStrength.FormattingEnabled = true;
            this.comboBoxTriggerSensorStrength.Location = new System.Drawing.Point(184, 19);
            this.comboBoxTriggerSensorStrength.Name = "comboBoxTriggerSensorStrength";
            this.comboBoxTriggerSensorStrength.Size = new System.Drawing.Size(169, 21);
            this.comboBoxTriggerSensorStrength.TabIndex = 1;
            this.comboBoxTriggerSensorStrength.SelectedIndexChanged += new System.EventHandler(this.comboBoxTriggerSensorStrength_SelectedIndexChanged);
            // 
            // comboBoxCamSensorStrength
            // 
            this.comboBoxCamSensorStrength.FormattingEnabled = true;
            this.comboBoxCamSensorStrength.Location = new System.Drawing.Point(184, 19);
            this.comboBoxCamSensorStrength.Name = "comboBoxCamSensorStrength";
            this.comboBoxCamSensorStrength.Size = new System.Drawing.Size(169, 21);
            this.comboBoxCamSensorStrength.TabIndex = 1;
            this.comboBoxCamSensorStrength.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamSensorStrength_SelectedIndexChanged);
            // 
            // comboBoxSyncSensorType
            // 
            this.comboBoxSyncSensorType.FormattingEnabled = true;
            this.comboBoxSyncSensorType.Location = new System.Drawing.Point(6, 19);
            this.comboBoxSyncSensorType.Name = "comboBoxSyncSensorType";
            this.comboBoxSyncSensorType.Size = new System.Drawing.Size(172, 21);
            this.comboBoxSyncSensorType.TabIndex = 0;
            this.comboBoxSyncSensorType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSyncSensorType_SelectedIndexChanged);
            // 
            // groupBoxCam
            // 
            this.groupBoxCam.Controls.Add(this.comboBoxCamSensorStrength);
            this.groupBoxCam.Controls.Add(this.comboBoxSyncSensorType);
            this.groupBoxCam.Location = new System.Drawing.Point(12, 78);
            this.groupBoxCam.Name = "groupBoxCam";
            this.groupBoxCam.Size = new System.Drawing.Size(619, 60);
            this.groupBoxCam.TabIndex = 1;
            this.groupBoxCam.TabStop = false;
            this.groupBoxCam.Text = "Cam Sync";
            // 
            // groupBoxAnalog
            // 
            this.groupBoxAnalog.Controls.Add(this.checkBoxPPSCAN);
            this.groupBoxAnalog.Controls.Add(this.checkBoxMAPCAN);
            this.groupBoxAnalog.Controls.Add(this.checkBoxTPSCAN);
            this.groupBoxAnalog.Controls.Add(this.checkBoxATSCAN);
            this.groupBoxAnalog.Controls.Add(this.checkBoxCTSCAN);
            this.groupBoxAnalog.Controls.Add(this.comboBoxAFM);
            this.groupBoxAnalog.Controls.Add(this.comboBoxMAP);
            this.groupBoxAnalog.Controls.Add(this.comboBoxTPS);
            this.groupBoxAnalog.Controls.Add(this.comboBoxATS);
            this.groupBoxAnalog.Controls.Add(this.comboBoxCTS);
            this.groupBoxAnalog.Controls.Add(this.label1);
            this.groupBoxAnalog.Controls.Add(this.labelMAP);
            this.groupBoxAnalog.Controls.Add(this.labelTPS);
            this.groupBoxAnalog.Controls.Add(this.labelATS);
            this.groupBoxAnalog.Controls.Add(this.labelCTS);
            this.groupBoxAnalog.Location = new System.Drawing.Point(12, 144);
            this.groupBoxAnalog.Name = "groupBoxAnalog";
            this.groupBoxAnalog.Size = new System.Drawing.Size(619, 178);
            this.groupBoxAnalog.TabIndex = 2;
            this.groupBoxAnalog.TabStop = false;
            this.groupBoxAnalog.Text = "Analog Inputs";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(529, 342);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(88, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "Apply";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelCTS
            // 
            this.labelCTS.AutoSize = true;
            this.labelCTS.Location = new System.Drawing.Point(13, 35);
            this.labelCTS.Name = "labelCTS";
            this.labelCTS.Size = new System.Drawing.Size(136, 13);
            this.labelCTS.TabIndex = 0;
            this.labelCTS.Text = "Coolant Temp Sensor Input";
            // 
            // labelATS
            // 
            this.labelATS.AutoSize = true;
            this.labelATS.Location = new System.Drawing.Point(13, 62);
            this.labelATS.Name = "labelATS";
            this.labelATS.Size = new System.Drawing.Size(112, 13);
            this.labelATS.TabIndex = 1;
            this.labelATS.Text = "Air Temp Sensor Input";
            // 
            // labelTPS
            // 
            this.labelTPS.AutoSize = true;
            this.labelTPS.Location = new System.Drawing.Point(13, 89);
            this.labelTPS.Name = "labelTPS";
            this.labelTPS.Size = new System.Drawing.Size(146, 13);
            this.labelTPS.TabIndex = 2;
            this.labelTPS.Text = "Throttle Position Sensor Input";
            // 
            // labelMAP
            // 
            this.labelMAP.AutoSize = true;
            this.labelMAP.Location = new System.Drawing.Point(13, 116);
            this.labelMAP.Name = "labelMAP";
            this.labelMAP.Size = new System.Drawing.Size(154, 13);
            this.labelMAP.TabIndex = 3;
            this.labelMAP.Text = "Manifold Pressure Sensor Input";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Air Flow Sensor Input";
            // 
            // comboBoxCTS
            // 
            this.comboBoxCTS.FormattingEnabled = true;
            this.comboBoxCTS.Location = new System.Drawing.Point(184, 27);
            this.comboBoxCTS.Name = "comboBoxCTS";
            this.comboBoxCTS.Size = new System.Drawing.Size(324, 21);
            this.comboBoxCTS.TabIndex = 5;
            // 
            // comboBoxATS
            // 
            this.comboBoxATS.FormattingEnabled = true;
            this.comboBoxATS.Location = new System.Drawing.Point(184, 54);
            this.comboBoxATS.Name = "comboBoxATS";
            this.comboBoxATS.Size = new System.Drawing.Size(324, 21);
            this.comboBoxATS.TabIndex = 6;
            // 
            // comboBoxTPS
            // 
            this.comboBoxTPS.FormattingEnabled = true;
            this.comboBoxTPS.Location = new System.Drawing.Point(184, 81);
            this.comboBoxTPS.Name = "comboBoxTPS";
            this.comboBoxTPS.Size = new System.Drawing.Size(324, 21);
            this.comboBoxTPS.TabIndex = 7;
            // 
            // comboBoxMAP
            // 
            this.comboBoxMAP.FormattingEnabled = true;
            this.comboBoxMAP.Location = new System.Drawing.Point(184, 108);
            this.comboBoxMAP.Name = "comboBoxMAP";
            this.comboBoxMAP.Size = new System.Drawing.Size(324, 21);
            this.comboBoxMAP.TabIndex = 8;
            // 
            // comboBoxAFM
            // 
            this.comboBoxAFM.FormattingEnabled = true;
            this.comboBoxAFM.Location = new System.Drawing.Point(184, 135);
            this.comboBoxAFM.Name = "comboBoxAFM";
            this.comboBoxAFM.Size = new System.Drawing.Size(324, 21);
            this.comboBoxAFM.TabIndex = 9;
            // 
            // checkBoxCTSCAN
            // 
            this.checkBoxCTSCAN.AutoSize = true;
            this.checkBoxCTSCAN.Location = new System.Drawing.Point(533, 29);
            this.checkBoxCTSCAN.Name = "checkBoxCTSCAN";
            this.checkBoxCTSCAN.Size = new System.Drawing.Size(72, 17);
            this.checkBoxCTSCAN.TabIndex = 10;
            this.checkBoxCTSCAN.Text = "CTS CAN";
            this.checkBoxCTSCAN.UseVisualStyleBackColor = true;
            this.checkBoxCTSCAN.CheckedChanged += new System.EventHandler(this.checkBoxCTSCAN_CheckedChanged);
            // 
            // checkBoxATSCAN
            // 
            this.checkBoxATSCAN.AutoSize = true;
            this.checkBoxATSCAN.Location = new System.Drawing.Point(533, 58);
            this.checkBoxATSCAN.Name = "checkBoxATSCAN";
            this.checkBoxATSCAN.Size = new System.Drawing.Size(72, 17);
            this.checkBoxATSCAN.TabIndex = 11;
            this.checkBoxATSCAN.Text = "ATS CAN";
            this.checkBoxATSCAN.UseVisualStyleBackColor = true;
            this.checkBoxATSCAN.CheckedChanged += new System.EventHandler(this.checkBoxATSCAN_CheckedChanged);
            // 
            // checkBoxTPSCAN
            // 
            this.checkBoxTPSCAN.AutoSize = true;
            this.checkBoxTPSCAN.Location = new System.Drawing.Point(533, 85);
            this.checkBoxTPSCAN.Name = "checkBoxTPSCAN";
            this.checkBoxTPSCAN.Size = new System.Drawing.Size(72, 17);
            this.checkBoxTPSCAN.TabIndex = 12;
            this.checkBoxTPSCAN.Text = "TPS CAN";
            this.checkBoxTPSCAN.UseVisualStyleBackColor = true;
            this.checkBoxTPSCAN.CheckedChanged += new System.EventHandler(this.checkBoxTPSCAN_CheckedChanged);
            // 
            // checkBoxMAPCAN
            // 
            this.checkBoxMAPCAN.AutoSize = true;
            this.checkBoxMAPCAN.Location = new System.Drawing.Point(533, 112);
            this.checkBoxMAPCAN.Name = "checkBoxMAPCAN";
            this.checkBoxMAPCAN.Size = new System.Drawing.Size(74, 17);
            this.checkBoxMAPCAN.TabIndex = 13;
            this.checkBoxMAPCAN.Text = "MAP CAN";
            this.checkBoxMAPCAN.UseVisualStyleBackColor = true;
            this.checkBoxMAPCAN.CheckedChanged += new System.EventHandler(this.checkBoxMAPCAN_CheckedChanged);
            // 
            // checkBoxPPSCAN
            // 
            this.checkBoxPPSCAN.AutoSize = true;
            this.checkBoxPPSCAN.Location = new System.Drawing.Point(533, 137);
            this.checkBoxPPSCAN.Name = "checkBoxPPSCAN";
            this.checkBoxPPSCAN.Size = new System.Drawing.Size(72, 17);
            this.checkBoxPPSCAN.TabIndex = 14;
            this.checkBoxPPSCAN.Text = "PPS CAN";
            this.checkBoxPPSCAN.UseVisualStyleBackColor = true;
            this.checkBoxPPSCAN.CheckedChanged += new System.EventHandler(this.checkBoxPPSCAN_CheckedChanged);
            // 
            // tclsInputsSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 377);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxAnalog);
            this.Controls.Add(this.groupBoxCam);
            this.Controls.Add(this.groupBoxCrank);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tclsInputsSetupWizard";
            this.Text = "Inputs Wizard";
            this.Load += new System.EventHandler(this.tclsInputsSetupWizard_Load);
            this.groupBoxCrank.ResumeLayout(false);
            this.groupBoxCam.ResumeLayout(false);
            this.groupBoxAnalog.ResumeLayout(false);
            this.groupBoxAnalog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCrank;
        private System.Windows.Forms.ComboBox comboBoxTriggerSensorStrength;
        private System.Windows.Forms.ComboBox comboBoxTriggerSensorType;
        private System.Windows.Forms.ComboBox comboBoxCamSensorStrength;
        private System.Windows.Forms.ComboBox comboBoxSyncSensorType;
        private System.Windows.Forms.GroupBox groupBoxCam;
        private System.Windows.Forms.GroupBox groupBoxAnalog;
        private System.Windows.Forms.ComboBox comboBoxAFM;
        private System.Windows.Forms.ComboBox comboBoxMAP;
        private System.Windows.Forms.ComboBox comboBoxTPS;
        private System.Windows.Forms.ComboBox comboBoxATS;
        private System.Windows.Forms.ComboBox comboBoxCTS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelMAP;
        private System.Windows.Forms.Label labelTPS;
        private System.Windows.Forms.Label labelATS;
        private System.Windows.Forms.Label labelCTS;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxPPSCAN;
        private System.Windows.Forms.CheckBox checkBoxMAPCAN;
        private System.Windows.Forms.CheckBox checkBoxTPSCAN;
        private System.Windows.Forms.CheckBox checkBoxATSCAN;
        private System.Windows.Forms.CheckBox checkBoxCTSCAN;
    }
}