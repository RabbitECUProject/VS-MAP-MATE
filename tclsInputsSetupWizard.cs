/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Inputs Setup Wizard                                    */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsInputsSetupWizard.cs                               */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP
{
    public partial class tclsInputsSetupWizard : Form
    {
        byte mu8CrankSensorType;
        byte mu8CamSensorType;
        byte mu8CrankSensorStrength;
        byte mu8CamSensorStrength;
        byte mu8CTSRes;
        byte mu8ATSRes;
        byte mu8TPSRes;
        byte mu8MAPRes;
        byte mu8MAFRes;
        bool mboTPSCAN;
        bool mboMAPCAN;
        bool mboATSCAN;
        bool mboCTSCAN;
        bool mboPPSCAN;


        public tclsInputsSetupWizard(byte u8CrankSensorType, byte u8CamSensorType, byte u8CrankSensorStrength, byte u8CamSensorStrength, byte u8CTSRes, byte u8ATSRes, byte u8TPSRes, byte u8MAPRes, byte u8MAFRes, byte u8TPSCAN, byte u8MAPCAN, byte u8ATSCAN, byte u8CTSCAN, byte u8PPSCAN)
        {
            mu8CrankSensorType = u8CrankSensorType;
            mu8CamSensorType = u8CamSensorType;
            mu8CrankSensorStrength = u8CrankSensorStrength;
            mu8CamSensorStrength = u8CamSensorStrength;
            mu8CTSRes = u8CTSRes;
            mu8ATSRes = u8ATSRes;
            mu8TPSRes = u8TPSRes;
            mu8MAPRes = u8MAPRes;
            mu8MAFRes = u8MAFRes;
            mboTPSCAN = u8TPSCAN == 0 ? false : true;
            mboMAPCAN = u8MAPCAN == 0 ? false : true;
            mboATSCAN = u8ATSCAN == 0 ? false : true;
            mboCTSCAN = u8CTSCAN == 0 ? false : true;
            mboPPSCAN = u8PPSCAN == 0 ? false : true;

            InitializeComponent();
        }

        private void tclsInputsSetupWizard_Load(object sender, EventArgs e)
        {
            PopulateCombos();
        }

        private void PopulateCombos()
        {
            comboBoxAFM.Items.Add("IO#4, Arduino Due A4, ECU AA4, TEMP SENSOR");
            comboBoxAFM.Items.Add("IO#5, Arduino Due A5, ECU AA3, DEFAULT CTS");
            comboBoxAFM.Items.Add("IO#6, Arduino Due A6, ECU AA2, DEFAULT ATS");
            comboBoxAFM.Items.Add("IO#7, Arduino Due A7, ECU AA1, TEMP SENSOR");
            comboBoxAFM.Items.Add("IO#8, Arduino Due A8, ECU BA4, DEFAULT TPS");
            comboBoxAFM.Items.Add("IO#9, Arduino Due A9, ECU BA3, DEFAULT MAP/MAF");
            comboBoxAFM.Items.Add("IO#10, Arduino Due A10, ECU BA2, DO NOT USE DUE");
            comboBoxAFM.Items.Add("IO#11, Arduino Due A11, ECU BA1, NON-TEMP SENSOR");
            comboBoxAFM.Items.Add("IO#103, Disabled");

            foreach (object ComboItem in comboBoxAFM.Items)
            {
                string szComboString = ComboItem.ToString();
                string[] szComboStrings = szComboString.Split(',');
                string szNumber = szComboStrings[0].Substring(3);
                if (Convert.ToByte(szNumber) == mu8MAFRes)
                {
                    comboBoxAFM.SelectedItem = ComboItem;
                }
            }

            if (comboBoxAFM.SelectedIndex < 0)
            {
                comboBoxAFM.SelectedIndex = comboBoxAFM.Items.Count - 1;
            }

            comboBoxMAP.Items.Add("IO#4, Arduino Due A4, ECU AA4, TEMP SENSOR");
            comboBoxMAP.Items.Add("IO#5, Arduino Due A5, ECU AA3, DEFAULT CTS");
            comboBoxMAP.Items.Add("IO#6, Arduino Due A6, ECU AA2, DEFAULT ATS");
            comboBoxMAP.Items.Add("IO#7, Arduino Due A7, ECU AA1, TEMP SENSOR");
            comboBoxMAP.Items.Add("IO#8, Arduino Due A8, ECU BA4, DEFAULT TPS");
            comboBoxMAP.Items.Add("IO#9, Arduino Due A9, ECU BA3, DEFAULT MAP/MAF");
            comboBoxMAP.Items.Add("IO#10, Arduino Due A10, ECU BA2, DO NOT USE DUE");
            comboBoxMAP.Items.Add("IO#11, Arduino Due A11, ECU BA1, NON-TEMP SENSOR");
            comboBoxMAP.Items.Add("IO#103, Disabled");

            foreach (object ComboItem in comboBoxMAP.Items)
            {
                string szComboString = ComboItem.ToString();
                string[] szComboStrings = szComboString.Split(',');
                string szNumber = szComboStrings[0].Substring(3);
                if (Convert.ToByte(szNumber) == mu8MAPRes)
                {
                    comboBoxMAP.SelectedItem = ComboItem;
                }
            }

            if (comboBoxMAP.SelectedIndex < 0)
            {
                comboBoxMAP.SelectedIndex = comboBoxMAP.Items.Count - 1;
            }

            comboBoxCTS.Items.Add("IO#4, Arduino Due A4, ECU AA4, TEMP SENSOR");
            comboBoxCTS.Items.Add("IO#5, Arduino Due A5, ECU AA3, DEFAULT CTS");
            comboBoxCTS.Items.Add("IO#6, Arduino Due A6, ECU AA2, DEFAULT ATS");
            comboBoxCTS.Items.Add("IO#7, Arduino Due A7, ECU AA1, TEMP SENSOR");
            comboBoxCTS.Items.Add("IO#8, Arduino Due A8, ECU BA4, DEFAULT TPS");
            comboBoxCTS.Items.Add("IO#9, Arduino Due A9, ECU BA3, DEFAULT MAP/MAF");
            comboBoxCTS.Items.Add("IO#10, Arduino Due A10, ECU BA2, DO NOT USE DUE");
            comboBoxCTS.Items.Add("IO#11, Arduino Due A11, ECU BA1, NON-TEMP SENSOR");
            comboBoxCTS.Items.Add("IO#103, Disabled");

            foreach (object ComboItem in comboBoxCTS.Items)
            {
                string szComboString = ComboItem.ToString();
                string[] szComboStrings = szComboString.Split(',');
                string szNumber = szComboStrings[0].Substring(3);
                if (Convert.ToByte(szNumber) == mu8CTSRes)
                {
                    comboBoxCTS.SelectedItem = ComboItem;
                }
            }

            if (comboBoxCTS.SelectedIndex < 0)
            {
                comboBoxCTS.SelectedIndex = comboBoxCTS.Items.Count - 1;
            }

            comboBoxATS.Items.Add("IO#4, Arduino Due A4, ECU AA4, TEMP SENSOR");
            comboBoxATS.Items.Add("IO#5, Arduino Due A5, ECU AA3, DEFAULT CTS");
            comboBoxATS.Items.Add("IO#6, Arduino Due A6, ECU AA2, DEFAULT ATS");
            comboBoxATS.Items.Add("IO#7, Arduino Due A7, ECU AA1, TEMP SENSOR");
            comboBoxATS.Items.Add("IO#8, Arduino Due A8, ECU BA4, DEFAULT TPS");
            comboBoxATS.Items.Add("IO#9, Arduino Due A9, ECU BA3, DEFAULT MAP/MAF");
            comboBoxATS.Items.Add("IO#10, Arduino Due A10, ECU BA2, DO NOT USE DUE");
            comboBoxATS.Items.Add("IO#11, Arduino Due A11, ECU BA1, NON-TEMP SENSOR");
            comboBoxATS.Items.Add("IO#103, Disabled");

            foreach (object ComboItem in comboBoxATS.Items)
            {
                string szComboString = ComboItem.ToString();
                string[] szComboStrings = szComboString.Split(',');
                string szNumber = szComboStrings[0].Substring(3);
                if (Convert.ToByte(szNumber) == mu8ATSRes)
                {
                    comboBoxATS.SelectedItem = ComboItem;
                }
            }

            if (comboBoxATS.SelectedIndex < 0)
            {
                comboBoxATS.SelectedIndex = comboBoxATS.Items.Count - 1;
            }

            comboBoxTPS.Items.Add("IO#4, Arduino Due A4, ECU AA4, TEMP SENSOR");
            comboBoxTPS.Items.Add("IO#5, Arduino Due A5, ECU AA3, DEFAULT CTS");
            comboBoxTPS.Items.Add("IO#6, Arduino Due A6, ECU AA2, DEFAULT ATS");
            comboBoxTPS.Items.Add("IO#7, Arduino Due A7, ECU AA1, TEMP SENSOR");
            comboBoxTPS.Items.Add("IO#8, Arduino Due A8, ECU BA4, DEFAULT TPS");
            comboBoxTPS.Items.Add("IO#9, Arduino Due A9, ECU BA3, DEFAULT MAP/MAF");
            comboBoxTPS.Items.Add("IO#10, Arduino Due A10, ECU BA2, DO NOT USE DUE");
            comboBoxTPS.Items.Add("IO#11, Arduino Due A11, ECU BA1, NON-TEMP SENSOR");
            comboBoxTPS.Items.Add("IO#103, Disabled");

            foreach (object ComboItem in comboBoxTPS.Items)
            {
                string szComboString = ComboItem.ToString();
                string[] szComboStrings = szComboString.Split(',');
                string szNumber = szComboStrings[0].Substring(3);
                if (Convert.ToByte(szNumber) == mu8TPSRes)
                {
                    comboBoxTPS.SelectedItem = ComboItem;
                }
            }

            if (comboBoxTPS.SelectedIndex < 0)
            {
                comboBoxTPS.SelectedIndex = comboBoxTPS.Items.Count - 1;
            }

            CheckBoxes();

            comboBoxTriggerSensorStrength.Items.Add("Low Noise");
            comboBoxTriggerSensorStrength.Items.Add("Medium Noise");
            comboBoxTriggerSensorStrength.Items.Add("High Noise");

            switch (mu8CrankSensorStrength)
            {
                case 0:
                default:
                    {
                        comboBoxTriggerSensorStrength.SelectedIndex = 0;
                        break;
                    }
                case 1:
                    {
                        comboBoxTriggerSensorStrength.SelectedIndex = 1;
                        break;
                    }
                case 2:
                    {
                        comboBoxTriggerSensorStrength.SelectedIndex = 2;
                        break;
                    }
            }

            comboBoxCamSensorStrength.Items.Add("Low Noise");
            comboBoxCamSensorStrength.Items.Add("Medium Noise");
            comboBoxCamSensorStrength.Items.Add("High Noise");

            switch (mu8CamSensorStrength)
            {
                case 0:
                default:
                    {
                        comboBoxCamSensorStrength.SelectedIndex = 0;
                        break;
                    }
                case 1:
                    {
                        comboBoxCamSensorStrength.SelectedIndex = 1;
                        break;
                    }
                case 2:
                    {
                        comboBoxCamSensorStrength.SelectedIndex = 2;
                        break;
                    }
            }

            comboBoxTriggerSensorType.Items.Add("Hall Effect/Optical");
            comboBoxTriggerSensorType.Items.Add("Magnetic Reluctor");

            switch (mu8CrankSensorType)
            {
                default:
                case 0:
                    {
                        comboBoxTriggerSensorType.SelectedIndex = 0;
                        break;
                    }
                case 1:
                    {
                        comboBoxTriggerSensorType.SelectedIndex = 1;
                        break;
                    }
            }

            comboBoxSyncSensorType.Items.Add("Hall Effect/Optical");
            comboBoxSyncSensorType.Items.Add("Magnetic Reluctor");

            switch (mu8CamSensorType)
            {
                default:
                case 0:
                    {
                        comboBoxSyncSensorType.SelectedIndex = 0;
                        break;
                    }
                case 1:
                    {
                        comboBoxSyncSensorType.SelectedIndex = 1;
                        break;
                    }
            }
        }

        private void CheckBoxes()
        {
            if (mboATSCAN == true)
            {
                checkBoxATSCAN.Checked = true;
                comboBoxATS.Enabled = false;
            }
            else
            {
                checkBoxATSCAN.Checked = false;
                comboBoxATS.Enabled = true;
            }

            if (mboCTSCAN == true)
            {
                checkBoxCTSCAN.Checked = true;
                comboBoxCTS.Enabled = false;
            }
            else
            {
                checkBoxCTSCAN.Checked = false;
                comboBoxCTS.Enabled = true;
            }

            if (mboTPSCAN == true)
            {
                checkBoxTPSCAN.Checked = true;
                comboBoxTPS.Enabled = false;
            }
            else
            {
                checkBoxTPSCAN.Checked = false;
                comboBoxTPS.Enabled = true;
            }

            if (mboPPSCAN == true)
            {
                checkBoxPPSCAN.Checked = true;
            }
            else
            {
                checkBoxPPSCAN.Checked = false;
            }

            if (mboMAPCAN == true)
            {
                checkBoxMAPCAN.Checked = true;
                comboBoxMAP.Enabled = false;
            }
            else
            {
                checkBoxMAPCAN.Checked = false;
                comboBoxMAP.Enabled = true;
            }
        }

        private void checkBoxCTSCAN_CheckedChanged(object sender, EventArgs e)
        {
            mboCTSCAN = checkBoxCTSCAN.Checked;
            CheckBoxes();
        }

        private void checkBoxATSCAN_CheckedChanged(object sender, EventArgs e)
        {
            mboATSCAN = checkBoxATSCAN.Checked;
            CheckBoxes();
        }

        private void checkBoxTPSCAN_CheckedChanged(object sender, EventArgs e)
        {
            mboTPSCAN = checkBoxTPSCAN.Checked;
            CheckBoxes();
        }

        private void checkBoxMAPCAN_CheckedChanged(object sender, EventArgs e)
        {
            mboMAPCAN = checkBoxMAPCAN.Checked;
            CheckBoxes();
        }

        private void checkBoxPPSCAN_CheckedChanged(object sender, EventArgs e)
        {
            mboPPSCAN = checkBoxPPSCAN.Checked;
        }

        private void comboBoxTriggerSensorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            mu8CrankSensorType = (byte)comboBoxTriggerSensorType.SelectedIndex;
        }

        private void comboBoxTriggerSensorStrength_SelectedIndexChanged(object sender, EventArgs e)
        {
            mu8CrankSensorStrength = (byte)comboBoxTriggerSensorType.SelectedIndex;
        }

        private void comboBoxSyncSensorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            mu8CamSensorType = (byte)comboBoxSyncSensorType.SelectedIndex;
        }

        private void comboBoxCamSensorStrength_SelectedIndexChanged(object sender, EventArgs e)
        {
            mu8CamSensorStrength = (byte)comboBoxSyncSensorType.SelectedIndex;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool boErr = true;

            int iCrankTypeIDX = tclsASAM.iGetCharIndex("Primary VR Enable");
            int iCamTypeIDX = tclsASAM.iGetCharIndex("Secondary VR Enable");
            int iTriggerPullStrengthIDX = tclsASAM.iGetCharIndex("Trigger Pull Strength");
            int iCamPullStrengthIDX = tclsASAM.iGetCharIndex("Sync Pull Strength");
            int iCTSSourceIDX = tclsASAM.iGetCharIndex("CTS AD Resource");
            int iATSSourceIDX = tclsASAM.iGetCharIndex("ATS AD Resource");
            int iTPSSourceIDX = tclsASAM.iGetCharIndex("TPS AD Resource");
            int iMAPSourceIDX = tclsASAM.iGetCharIndex("MAP AD Resource");
            int iMAFSourceIDX = tclsASAM.iGetCharIndex("AFM AD Resource");
            int iCTSCANIDX = tclsASAM.iGetCharIndex("CTS CAN Primary");
            int iATSCANIDX = tclsASAM.iGetCharIndex("ATS CAN Primary");
            int iTPSCANIDX = tclsASAM.iGetCharIndex("TPS CAN Primary");
            int iMAPCANIDX = tclsASAM.iGetCharIndex("MAP CAN Primary");
            int iPPSCANIDX = tclsASAM.iGetCharIndex("PPS CAN Primary");

            if ((iCTSSourceIDX != -1) &&
            (iATSSourceIDX != -1) &&
            (iTPSSourceIDX != -1) &&
            (iMAPSourceIDX != -1) &&
            (iMAFSourceIDX != -1) &&
            (iCTSCANIDX != -1) &&
            (iATSCANIDX != -1) &&
            (iTPSCANIDX != -1) &&
            (iMAPCANIDX != -1) &&
            (iPPSCANIDX != -1) &&
            (iCrankTypeIDX != -1) &&
            (iCamTypeIDX != -1) &&
            (iTriggerPullStrengthIDX != -1) &&
            (iCamPullStrengthIDX != -1))
            {
                try
                {
                    string szComboString = comboBoxCTS.SelectedItem.ToString();
                    string[] szComboStrings = szComboString.Split(',');
                    string szNumber = szComboStrings[0].Substring(3);
                    mu8CTSRes = Convert.ToByte(szNumber);

                    szComboString = comboBoxATS.SelectedItem.ToString();
                    szComboStrings = szComboString.Split(',');
                    szNumber = szComboStrings[0].Substring(3);
                    mu8ATSRes = Convert.ToByte(szNumber);

                    szComboString = comboBoxTPS.SelectedItem.ToString();
                    szComboStrings = szComboString.Split(',');
                    szNumber = szComboStrings[0].Substring(3);
                    mu8TPSRes = Convert.ToByte(szNumber);

                    szComboString = comboBoxMAP.SelectedItem.ToString();
                    szComboStrings = szComboString.Split(',');
                    szNumber = szComboStrings[0].Substring(3);
                    mu8MAPRes = Convert.ToByte(szNumber);

                    szComboString = comboBoxAFM.SelectedItem.ToString();
                    szComboStrings = szComboString.Split(',');
                    szNumber = szComboStrings[0].Substring(3);
                    mu8MAFRes = Convert.ToByte(szNumber);

                    mu8CrankSensorStrength = (byte)comboBoxTriggerSensorStrength.SelectedIndex;
                    mu8CamSensorStrength = (byte)comboBoxCamSensorStrength.SelectedIndex;
                    mu8CrankSensorType = (byte)comboBoxTriggerSensorType.SelectedIndex;
                    mu8CamSensorType = (byte)comboBoxSyncSensorType.SelectedIndex;

                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iCrankTypeIDX].u32Address, mu8CrankSensorType);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iCamTypeIDX].u32Address, mu8CamSensorType);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iTriggerPullStrengthIDX].u32Address, mu8CrankSensorStrength);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iCamPullStrengthIDX].u32Address, mu8CamSensorStrength);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iCTSSourceIDX].u32Address, mu8CTSRes);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iATSSourceIDX].u32Address, mu8ATSRes);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iTPSSourceIDX].u32Address, mu8TPSRes);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iMAPSourceIDX].u32Address, mu8MAPRes);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iCTSCANIDX].u32Address, mboCTSCAN == true ? (byte)1 : (byte)0);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iATSCANIDX].u32Address, mboATSCAN == true ? (byte)1 : (byte)0);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iTPSCANIDX].u32Address, mboTPSCAN == true ? (byte)1 : (byte)0);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iMAPCANIDX].u32Address, mboMAPCAN == true ? (byte)1 : (byte)0);
                    tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iPPSCANIDX].u32Address, mboPPSCAN == true ? (byte)1 : (byte)0);
                    boErr = false;
                }
                catch
                {
                    MessageBox.Show("An error occurred applying the new settings");
                    boErr = true;
                }
            }

            if (false == boErr)
            {
                Program.vNotifyProgramEvent(tenProgramEvent.enLoadCalibrationComplete, 0, "");
                this.Close();
            }
        }

        private void buttonSetDefault_Click(object sender, EventArgs e)
        {
            checkBoxATSCAN.Checked = false;
            checkBoxCTSCAN.Checked = false;
            checkBoxPPSCAN.Checked = false;
            checkBoxTPSCAN.Checked = false;
            checkBoxMAPCAN.Checked = false;

            comboBoxAFM.SelectedIndex = 5; comboBoxAFM.Enabled = true;
            comboBoxATS.SelectedIndex = 2; comboBoxATS.Enabled = true;
            comboBoxCTS.SelectedIndex = 1; comboBoxCTS.Enabled = true;
            comboBoxMAP.SelectedIndex = 5; comboBoxMAP.Enabled = true;
            comboBoxTPS.SelectedIndex = 4; comboBoxTPS.Enabled = true;
        }
    }
}
