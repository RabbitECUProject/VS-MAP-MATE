using MDACFirmwareUpdateInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP
{
    public partial class tclsFirmwareJTAG : Form
    {
        MDACFirmwareUpdateInterface.MDACFirmwareUpdate mclsFirmwareUpdateInterface;
        bool mboCombosLoaded;
        float mfMCU1ProgressValidate = 100;
        float mfMCU2ProgressValidate = 100;
        float mfMCU3ProgressValidate = 100;
        float mfMCU1ProgressProgram = 100;
        float mfMCU2ProgressProgram = 100;
        float mfMCU3ProgressProgram = 100;

        public tclsFirmwareJTAG()
        {
            String szSetting;
            int iSelectedIDX;

            mboCombosLoaded = false;

            InitializeComponent();
            mclsFirmwareUpdateInterface = new MDACFirmwareUpdateInterface.MDACFirmwareUpdate("Firmware update");

            comboBoxMCU1FirmwareHexPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU1FW"));
            comboBoxMCU1USBCfgPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU1ToolInterfaceCfgPath"));
            comboBoxMCU1HWCfgPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU1ToolDeviceCfgPath"));
            comboBoxMCU1ToolPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU1ToolPath"));

            comboBoxMCU1FirmwareHexPath.SelectedIndex = comboBoxMCU1FirmwareHexPath.Items.Count - 1;
            comboBoxMCU1USBCfgPath.SelectedIndex = comboBoxMCU1USBCfgPath.Items.Count - 1;
            comboBoxMCU1HWCfgPath.SelectedIndex = comboBoxMCU1HWCfgPath.Items.Count - 1;
            comboBoxMCU1ToolPath.SelectedIndex = comboBoxMCU1ToolPath.Items.Count - 1;

            comboBoxMCU2FirmwareHexPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU2FW"));
            comboBoxMCU2USBCfgPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU2ToolInterfaceCfgPath"));
            comboBoxMCU2HWCfgPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU2ToolDeviceCfgPath"));
            comboBoxMCU2ToolPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU2ToolPath"));

            comboBoxMCU2FirmwareHexPath.SelectedIndex = comboBoxMCU2FirmwareHexPath.Items.Count - 1;
            comboBoxMCU2USBCfgPath.SelectedIndex = comboBoxMCU2USBCfgPath.Items.Count - 1;
            comboBoxMCU2HWCfgPath.SelectedIndex = comboBoxMCU2HWCfgPath.Items.Count - 1;
            comboBoxMCU2ToolPath.SelectedIndex = comboBoxMCU2ToolPath.Items.Count - 1;

            comboBoxMCU3FirmwareHexPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU3FW"));
            comboBoxMCU3USBCfgPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU3ToolInterfaceCfgPath"));
            comboBoxMCU3HWCfgPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU3ToolDeviceCfgPath"));
            comboBoxMCU3ToolPath.Items.Add(Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU3ToolPath"));

            comboBoxMCU3FirmwareHexPath.SelectedIndex = comboBoxMCU3FirmwareHexPath.Items.Count - 1;
            comboBoxMCU3USBCfgPath.SelectedIndex = comboBoxMCU3USBCfgPath.Items.Count - 1;
            comboBoxMCU3HWCfgPath.SelectedIndex = comboBoxMCU3HWCfgPath.Items.Count - 1;
            comboBoxMCU3ToolPath.SelectedIndex = comboBoxMCU3ToolPath.Items.Count - 1;

            szSetting = Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU1Tool");
            iSelectedIDX = 0;

            foreach (String szToolOption in comboBoxMCU1ToolType.Items)
            {
                if (szToolOption.Contains(szSetting))
                {
                    comboBoxMCU1ToolType.SelectedIndex = iSelectedIDX;
                }

                iSelectedIDX++;
            }

            szSetting = Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU2Tool");
            iSelectedIDX = 0;

            foreach (String szToolOption in comboBoxMCU2ToolType.Items)
            {
                if (szToolOption.Contains(szSetting))
                {
                    comboBoxMCU2ToolType.SelectedIndex = iSelectedIDX;
                }

                iSelectedIDX++;
            }

            szSetting = Program.mAPP_mclsIniParser.GetSetting("FirmwareUpdate", "MCU3Tool");
            iSelectedIDX = 0;

            foreach (String szToolOption in comboBoxMCU3ToolType.Items)
            {
                if (szToolOption.Contains(szSetting))
                {
                    comboBoxMCU3ToolType.SelectedIndex = iSelectedIDX;
                }

                iSelectedIDX++;
            }

            if (comboBoxMCU1ToolType.SelectedIndex == ~0)
            {
                comboBoxMCU1ToolType.SelectedIndex = comboBoxMCU1ToolType.Items.Count - 1;
            }

            if (comboBoxMCU2ToolType.SelectedIndex == ~0)
            {
                comboBoxMCU2ToolType.SelectedIndex = comboBoxMCU2ToolType.Items.Count - 1;
            }

            if (comboBoxMCU3ToolType.SelectedIndex == ~0)
            {
                comboBoxMCU3ToolType.SelectedIndex = comboBoxMCU3ToolType.Items.Count - 1;
            }

            if (comboBoxMCU1ToolType.SelectedIndex != 1)
            {
                buttonMCU1Validate.Enabled = true;
            }
            else
            {
                buttonMCU1Validate.Enabled = false;
            }

            if (comboBoxMCU2ToolType.SelectedIndex != 1)
            {
                buttonMCU2Validate.Enabled = true;
            }
            else
            {
                buttonMCU2Validate.Enabled = false;
            }

            if (comboBoxMCU3ToolType.SelectedIndex != 1)
            {
                buttonMCU3Validate.Enabled = true;
            }
            else
            {
                buttonMCU3Validate.Enabled = false;
            }

            mboCombosLoaded = true;
        }

        private void TableLayoutPanelMCU1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FirmwareLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tclsFirmwareJTAG_Load(object sender, EventArgs e)
        {

        }

        private void buttonMCU1Program_Click(object sender, EventArgs ev)
        {
            MDACFirmwareUpdate.InterfaceType ToolType;

            switch (comboBoxMCU1ToolType.SelectedIndex)
            {
                case ConstantData.TOOLTYPE.u16OpenOCD: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareOpenOCD; break;
                case ConstantData.TOOLTYPE.u16KeilUVision: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareKeilUVision4; break;
                case ConstantData.TOOLTYPE.u16USBDM: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareUSBDM; break;
                default: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareInterfaceCount; break;
            }

            mclsFirmwareUpdateInterface.FirmwareLoad(ToolType, comboBoxMCU1ToolPath.SelectedItem.ToString(),
               comboBoxMCU1USBCfgPath.SelectedItem.ToString(),
               comboBoxMCU1HWCfgPath.SelectedItem.ToString(),
               AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + comboBoxMCU1FirmwareHexPath.SelectedItem.ToString(), 1000, true);
        }

        private void buttonMCU1Validate_Click(object sender, EventArgs e)
        {
            MDACFirmwareUpdate.InterfaceType ToolType;

            switch (comboBoxMCU1ToolType.SelectedIndex)
            {
                case ConstantData.TOOLTYPE.u16OpenOCD: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareOpenOCD; break;
                case ConstantData.TOOLTYPE.u16KeilUVision: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareKeilUVision4; break;
                case ConstantData.TOOLTYPE.u16USBDM: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareUSBDM; break;
                default: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareInterfaceCount; break;
            }

            bool verify = mclsFirmwareUpdateInterface.FirmwareVerify(MDACFirmwareUpdate.InterfaceType.MDACFirmwareOpenOCD, comboBoxMCU1ToolPath.SelectedItem.ToString(),
               comboBoxMCU1USBCfgPath.SelectedItem.ToString(),
               comboBoxMCU1HWCfgPath.SelectedItem.ToString(),
               AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + comboBoxMCU1FirmwareHexPath.SelectedItem.ToString(), 5000, false, 3000);

            timerProgress.Enabled = true;
            mfMCU1ProgressValidate = 0;
        }

        private void buttonMUC1LoadToolPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFolder = comboBoxMCU1ToolPath.SelectedItem.ToString();
            String newSelectedFolder = GetSelectedFolder(oldSelectedFolder);

            newSelectedFolder = newSelectedFolder.Replace("\\", "/");

            if (oldSelectedFolder.Equals(newSelectedFolder))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU1OpenOCDPath", newSelectedFolder);
                comboBoxMCU1ToolPath.Items.Add(newSelectedFolder);
                comboBoxMCU1ToolPath.SelectedIndex = comboBoxMCU1ToolPath.Items.Count - 1;
            }
        }

        private String GetSelectedFolder(String initial_folder)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = initial_folder;
            fbd.ShowDialog();

            return fbd.SelectedPath;
        }

        private String GetSelectedFile(String initial_path, String default_extension)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = Path.GetDirectoryName(initial_path);
            ofd.DefaultExt = default_extension;
            ofd.ShowDialog();

            return ofd.FileName;
        }

        private void comboBoxMCU1ToolPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU1OpenOCDPath", comboBoxMCU1ToolPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU1USBCfgPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU1OpenOCDInterfaceCfgPath", comboBoxMCU1USBCfgPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU1HWCfgPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU1OpenOCDDeviceCfgPath", comboBoxMCU1HWCfgPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU1FirmwareHexPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU1FW", comboBoxMCU1FirmwareHexPath.SelectedItem.ToString());
            }
        }

        private void buttonLoadMCU1USBCfgPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU1USBCfgPath.SelectedItem.ToString();
            String openOCDfolder = comboBoxMCU1ToolPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(openOCDfolder + "/share/" + oldSelectedFile, "cfg");

            if (newSelectedFile.Length == 0) {return;}

            newSelectedFile = newSelectedFile.Replace("\\", "/");
            newSelectedFile = newSelectedFile.Replace(openOCDfolder, "");
            newSelectedFile = newSelectedFile.Replace("/share/", "");
            newSelectedFile = newSelectedFile.Replace("/Share/", "");
            newSelectedFile = newSelectedFile.Replace("/SHARE/", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU1OpenOCDInterfaceCfgPath", newSelectedFile);
                comboBoxMCU1USBCfgPath.Items.Add(newSelectedFile);
                comboBoxMCU1USBCfgPath.SelectedIndex = comboBoxMCU1USBCfgPath.Items.Count - 1;
            }
        }

        private void buttonMCU2LoadToolPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFolder = comboBoxMCU2ToolPath.SelectedItem.ToString();
            String newSelectedFolder = GetSelectedFolder(oldSelectedFolder);

            newSelectedFolder = newSelectedFolder.Replace("\\", "/");

            if (oldSelectedFolder.Equals(newSelectedFolder))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU2OpenOCDPath", newSelectedFolder);
                comboBoxMCU2ToolPath.Items.Add(newSelectedFolder);
                comboBoxMCU2ToolPath.SelectedIndex = comboBoxMCU2ToolPath.Items.Count - 1;
            }
        }

        private void buttonMCU1LoadHWCfgPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU1HWCfgPath.SelectedItem.ToString();
            String openOCDfolder = comboBoxMCU1ToolPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(openOCDfolder + "/share/" + oldSelectedFile, "cfg");

            if (newSelectedFile.Length == 0) { return; }

            newSelectedFile = newSelectedFile.Replace("\\", "/");
            newSelectedFile = newSelectedFile.Replace(openOCDfolder, "");
            newSelectedFile = newSelectedFile.Replace("/share/", "");
            newSelectedFile = newSelectedFile.Replace("/Share/", "");
            newSelectedFile = newSelectedFile.Replace("/SHARE/", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU1OpenOCDInterfaceHWPath", newSelectedFile);
                comboBoxMCU1HWCfgPath.Items.Add(newSelectedFile);
                comboBoxMCU1HWCfgPath.SelectedIndex = comboBoxMCU1HWCfgPath.Items.Count - 1;
            }
        }

        private void buttonMCU2LoadUSBCfgPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU2USBCfgPath.SelectedItem.ToString();
            String openOCDfolder = comboBoxMCU2ToolPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(openOCDfolder + "/share/" + oldSelectedFile, "cfg");

            if (newSelectedFile.Length == 0) { return; }

            newSelectedFile = newSelectedFile.Replace("\\", "/");
            newSelectedFile = newSelectedFile.Replace(openOCDfolder, "");
            newSelectedFile = newSelectedFile.Replace("/share/", "");
            newSelectedFile = newSelectedFile.Replace("/Share/", "");
            newSelectedFile = newSelectedFile.Replace("/SHARE/", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU2OpenOCDInterfaceCfgPath", newSelectedFile);
                comboBoxMCU2USBCfgPath.Items.Add(newSelectedFile);
                comboBoxMCU2USBCfgPath.SelectedIndex = comboBoxMCU2USBCfgPath.Items.Count - 1;
            }
        }

        private void buttonMCU2LoadHWCfgPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU2HWCfgPath.SelectedItem.ToString();
            String openOCDfolder = comboBoxMCU2ToolPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(openOCDfolder + "/share/" + oldSelectedFile, "cfg");

            if (newSelectedFile.Length == 0) { return; }

            newSelectedFile = newSelectedFile.Replace("\\", "/");
            newSelectedFile = newSelectedFile.Replace(openOCDfolder, "");
            newSelectedFile = newSelectedFile.Replace("/share/", "");
            newSelectedFile = newSelectedFile.Replace("/Share/", "");
            newSelectedFile = newSelectedFile.Replace("/SHARE/", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU2OpenOCDInterfaceHWPath", newSelectedFile);
                comboBoxMCU2HWCfgPath.Items.Add(newSelectedFile);
                comboBoxMCU2HWCfgPath.SelectedIndex = comboBoxMCU2HWCfgPath.Items.Count - 1;
            }
        }

        private void buttonMCU1LoadFWPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU1FirmwareHexPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + oldSelectedFile, "hex");

            if (newSelectedFile.Length == 0) { return; }

            newSelectedFile = newSelectedFile.Replace(AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU1FW", newSelectedFile);
                comboBoxMCU1FirmwareHexPath.Items.Add(newSelectedFile);
                comboBoxMCU1FirmwareHexPath.SelectedIndex = comboBoxMCU1FirmwareHexPath.Items.Count - 1;
            }
        }

        private void comboBoxMCU2FirmwareHexPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU2FW", comboBoxMCU2FirmwareHexPath.SelectedItem.ToString());
            }
        }

        private void buttonMCU2LoadFWPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU2FirmwareHexPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + oldSelectedFile, "hex");

            if (newSelectedFile.Length == 0) { return; }

            newSelectedFile = newSelectedFile.Replace(AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU2FW", newSelectedFile);
                comboBoxMCU2FirmwareHexPath.Items.Add(newSelectedFile);
                comboBoxMCU2FirmwareHexPath.SelectedIndex = comboBoxMCU2FirmwareHexPath.Items.Count - 1;
            }
        }

        private void buttonMCU2Program_Click(object sender, EventArgs e)
        {
            MDACFirmwareUpdate.InterfaceType ToolType;

            switch (comboBoxMCU2ToolType.SelectedIndex)
            {
                case ConstantData.TOOLTYPE.u16OpenOCD: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareOpenOCD; break;
                case ConstantData.TOOLTYPE.u16KeilUVision: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareKeilUVision4; break;
                case ConstantData.TOOLTYPE.u16USBDM: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareUSBDM; break;
                default: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareInterfaceCount; break;
            }

            mclsFirmwareUpdateInterface.FirmwareLoad(ToolType, comboBoxMCU2ToolPath.SelectedItem.ToString(),
               comboBoxMCU2USBCfgPath.SelectedItem.ToString(),
               comboBoxMCU2HWCfgPath.SelectedItem.ToString(),
               AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + comboBoxMCU2FirmwareHexPath.SelectedItem.ToString(), 1000, true);
        }

        private void comboBoxMCU2ToolPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU21OpenOCDPath", comboBoxMCU2ToolPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU2USBCfgPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU2OpenOCDInterfaceCfgPath", comboBoxMCU2USBCfgPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU3ToolPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU3OpenOCDPath", comboBoxMCU3ToolPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU3USBCfgPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU3OpenOCDInterfaceCfgPath", comboBoxMCU3USBCfgPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU2HWCfgPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU2OpenOCDDeviceCfgPath", comboBoxMCU2HWCfgPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU3HWCfgPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU3OpenOCDDeviceCfgPath", comboBoxMCU3HWCfgPath.SelectedItem.ToString());
            }
        }

        private void comboBoxMCU3FirmwareHexPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mboCombosLoaded == true)
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU3FW", comboBoxMCU3FirmwareHexPath.SelectedItem.ToString());
            }
        }

        private void buttonMCU3LoadFWPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU3FirmwareHexPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + oldSelectedFile, "hex");

            if (newSelectedFile.Length == 0) { return; }

            newSelectedFile = newSelectedFile.Replace(AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU3FW", newSelectedFile);
                comboBoxMCU3FirmwareHexPath.Items.Add(newSelectedFile);
                comboBoxMCU3FirmwareHexPath.SelectedIndex = comboBoxMCU3FirmwareHexPath.Items.Count - 1;
            }
        }

        private void buttonMCU3LoadHWCfgPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU3HWCfgPath.SelectedItem.ToString();
            String openOCDfolder = comboBoxMCU3ToolPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(openOCDfolder + "/share/" + oldSelectedFile, "cfg");

            if (newSelectedFile.Length == 0) { return; }

            newSelectedFile = newSelectedFile.Replace("\\", "/");
            newSelectedFile = newSelectedFile.Replace(openOCDfolder, "");
            newSelectedFile = newSelectedFile.Replace("/share/", "");
            newSelectedFile = newSelectedFile.Replace("/Share/", "");
            newSelectedFile = newSelectedFile.Replace("/SHARE/", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU3OpenOCDInterfaceHWPath", newSelectedFile);
                comboBoxMCU3HWCfgPath.Items.Add(newSelectedFile);
                comboBoxMCU3HWCfgPath.SelectedIndex = comboBoxMCU3HWCfgPath.Items.Count - 1;
            }
        }

        private void buttonMCU3Program_Click(object sender, EventArgs e)
        {
            MDACFirmwareUpdate.InterfaceType ToolType;

            switch (comboBoxMCU3ToolType.SelectedIndex)
            {
                case ConstantData.TOOLTYPE.u16OpenOCD: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareOpenOCD; break;
                case ConstantData.TOOLTYPE.u16KeilUVision: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareKeilUVision4; break;
                case ConstantData.TOOLTYPE.u16USBDM: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareUSBDM; break;
                default: ToolType = MDACFirmwareUpdate.InterfaceType.MDACFirmwareInterfaceCount; break;
            }

            mclsFirmwareUpdateInterface.FirmwareLoad(ToolType, comboBoxMCU3ToolPath.SelectedItem.ToString(),
               comboBoxMCU3USBCfgPath.SelectedItem.ToString(),
               comboBoxMCU3HWCfgPath.SelectedItem.ToString(),
               AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + comboBoxMCU3FirmwareHexPath.SelectedItem.ToString(), 1000, true);
        }

        private void buttonMCU3LoadToolPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFolder = comboBoxMCU3ToolPath.SelectedItem.ToString();
            String newSelectedFolder = GetSelectedFolder(oldSelectedFolder);

            newSelectedFolder = newSelectedFolder.Replace("\\", "/");

            if (oldSelectedFolder.Equals(newSelectedFolder))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU3OpenOCDPath", newSelectedFolder);
                comboBoxMCU3ToolPath.Items.Add(newSelectedFolder);
                comboBoxMCU3ToolPath.SelectedIndex = comboBoxMCU3ToolPath.Items.Count - 1;
            }
        }

        private void buttonMCU3LoadUSBCfgPath_Click(object sender, EventArgs e)
        {
            String oldSelectedFile = comboBoxMCU3USBCfgPath.SelectedItem.ToString();
            String openOCDfolder = comboBoxMCU3ToolPath.SelectedItem.ToString();
            String newSelectedFile = GetSelectedFile(openOCDfolder + "/share/" + oldSelectedFile, "cfg");

            if (newSelectedFile.Length == 0) { return; }

            newSelectedFile = newSelectedFile.Replace("\\", "/");
            newSelectedFile = newSelectedFile.Replace(openOCDfolder, "");
            newSelectedFile = newSelectedFile.Replace("/share/", "");
            newSelectedFile = newSelectedFile.Replace("/Share/", "");
            newSelectedFile = newSelectedFile.Replace("/SHARE/", "");

            if (oldSelectedFile.Equals(newSelectedFile))
            {
                //no change
            }
            else
            {
                Program.mAPP_mclsIniParser.AddSetting("FirmwareUpdate", "MCU3OpenOCDInterfaceCfgPath", newSelectedFile);
                comboBoxMCU3USBCfgPath.Items.Add(newSelectedFile);
                comboBoxMCU3USBCfgPath.SelectedIndex = comboBoxMCU3USBCfgPath.Items.Count - 1;
            }
        }

        private void buttonMCU3Validate_Click(object sender, EventArgs e)
        {
            bool verify = mclsFirmwareUpdateInterface.FirmwareVerify(MDACFirmwareUpdate.InterfaceType.MDACFirmwareOpenOCD, comboBoxMCU3ToolPath.SelectedItem.ToString(),
               comboBoxMCU3USBCfgPath.SelectedItem.ToString(),
               comboBoxMCU3HWCfgPath.SelectedItem.ToString(),
               AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + comboBoxMCU3FirmwareHexPath.SelectedItem.ToString(), 1000, false, 3000);

            timerProgress.Enabled = true;
            mfMCU3ProgressValidate = 0;
        }

        private void timerProgress_Tick(object sender, EventArgs e)
        {
            bool status;

            if (mfMCU1ProgressValidate < 100)
            {
                mfMCU1ProgressValidate += 5;

                progressBarMCU1Validate.Value = (int)mfMCU1ProgressValidate;

                if (mfMCU1ProgressValidate > 99)
                {
                    String szResponse = mclsFirmwareUpdateInterface.GetResponse();
                    status = ValidateParse(1, szResponse);

                    if (status == true)
                    {
                        progressBarMCU1Validate.Value = 100;
                    }
                    else
                    {
                        progressBarMCU1Validate.Value = 0;
                    }

                    timerProgress.Enabled = false;
                }
            }

            if (mfMCU3ProgressValidate < 100)
            {
                mfMCU3ProgressValidate += 5;

                progressBarMCU3Validate.Value = (int)mfMCU3ProgressValidate;

                if (mfMCU3ProgressValidate > 99)
                {
                    String szResponse = mclsFirmwareUpdateInterface.GetResponse();
                    status = ValidateParse(3, szResponse);

                    if (status == true)
                    {
                        progressBarMCU3Validate.Value = 100;
                    }
                    else
                    {
                        progressBarMCU3Validate.Value = 0;
                    }

                    timerProgress.Enabled = false;
                }
            }
        }

        private void buttonMCU2Validate_Click(object sender, EventArgs e)
        {

        }

        private bool ValidateParse(int iMCU, String szResponse)
        {
            bool statusVerified = false;
            bool statusVerifyFailed = false;
            String szMessageString = "NUL";
            String[] aszResponseLines = szResponse.Split('\r', '\n');

            foreach (String szResponseLine in aszResponseLines)
            {
                if (szResponseLine.IndexOf("verified", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    szMessageString = szResponseLine;
                    statusVerified = true;
                }

                if (szResponseLine.IndexOf("verify failed", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    szMessageString = szResponseLine;
                    statusVerifyFailed = true;
                }
            }

            if (statusVerified == true)
            {
                szMessageString = szMessageString.Replace("verified", "Verified");
                MessageBox.Show(szMessageString, "MCU" + iMCU + " Success", MessageBoxButtons.OK);
            }
            else if (statusVerifyFailed == true)
            {
                MessageBox.Show(szMessageString, "MCU" + iMCU + " Firmware different", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Verify action failed - unknown reason", "Failed", MessageBoxButtons.OK);
            }

            return statusVerified;
        }

        private void comboBoxMCU1ToolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMCU1ToolType.SelectedIndex != 1)
            {
                buttonMCU1Validate.Enabled = true;
            }
            else
            {
                buttonMCU1Validate.Enabled = false;
            }
        }

        private void comboBoxMCU2ToolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMCU2ToolType.SelectedIndex != 1)
            {
                buttonMCU2Validate.Enabled = true;
            }
            else
            {
                buttonMCU2Validate.Enabled = false;
            }
        }

        private void comboBoxMCU3ToolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMCU3ToolType.SelectedIndex != 1)
            {
                buttonMCU3Validate.Enabled = true;
            }
            else
            {
                buttonMCU3Validate.Enabled = false;
            }
        }
    }
}
    
