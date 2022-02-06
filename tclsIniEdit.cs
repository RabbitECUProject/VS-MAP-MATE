/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      INI Edit                                               */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsIniEdit.cs                                         */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
#if BUILD_WIFI
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.AirPcap;
using SharpPcap.WinPcap;
#endif
using System.IO;
using System.Net.NetworkInformation;

namespace UDP
{
    public partial class tclsIniEdit : Form
    {        
        Label[] maclsLabelSection;
        Label[] maclsLabelSetting;
        string[] maszSettings;
        ComboBox[] maclsSettingComboBox;
        int miSelectedInterfaceIDX = -1;

        public tclsIniEdit()
        {
            InitializeComponent();
        }

        private void tclsIniEdit_Load(object sender, EventArgs e)
        {
            string[] aszSections = Program.mAPP_mclsIniParser.EnumAll();
            int iSettingCount = 0;
            int iYOffset = 10;
            maclsLabelSection = new Label[Program.mAPP_mclsIniParser.GetSettingsCount()];
            maclsLabelSetting = new Label[Program.mAPP_mclsIniParser.GetSettingsCount()];
            maclsSettingComboBox = new ComboBox[Program.mAPP_mclsIniParser.GetSettingsCount()];
            maszSettings = new string[Program.mAPP_mclsIniParser.GetSettingsCount()];

            foreach (string szSection in aszSections)
            {
                string[] aszSettings = Program.mAPP_mclsIniParser.EnumSection(szSection);

                foreach (string szSetting in aszSettings)
                {
                    string[] aszSettingOptions;

                    Label sectionLabel = new System.Windows.Forms.Label();
                    Label settingLabel = new System.Windows.Forms.Label();
                    ComboBox comboBox = new System.Windows.Forms.ComboBox();

                    sectionLabel.Location = new System.Drawing.Point(10, iYOffset);
                    sectionLabel.Size = new System.Drawing.Size(45, 15);
                    sectionLabel.TabIndex = 2;
                    sectionLabel.Text = szSection;
                    maclsLabelSection[iSettingCount] = sectionLabel;

                    settingLabel.Location = new System.Drawing.Point(170, iYOffset);
                    settingLabel.Size = new System.Drawing.Size(40, 15);
                    settingLabel.TabIndex = 3;
                    settingLabel.Text = szSetting;
                    maclsLabelSetting[iSettingCount] = settingLabel;
                    maszSettings[iSettingCount] = szSetting;

                    comboBox.FormattingEnabled = true;
                    comboBox.Location = new System.Drawing.Point(290, iYOffset - 5);
                    comboBox.Size = new System.Drawing.Size(440, 20);
                    comboBox.TabIndex = 4;
                    maclsSettingComboBox[iSettingCount] = comboBox;

                    aszSettingOptions = aszGetSettingOptions(szSetting);

                    foreach(string szSettingOption in aszSettingOptions)
                    {
                        if (null != szSettingOption)
                        {
                            comboBox.Items.Add(szSettingOption);

                            if (szSettingOption.Equals(Program.mAPP_mclsIniParser.GetSetting(szSection, szSetting)))
                            {
                                comboBox.SelectedIndex = comboBox.Items.Count - 1;
                                if ("SelectedDevice" == szSetting)
                                {
                                    miSelectedInterfaceIDX = comboBox.SelectedIndex;
                                }
                            }
                        }
                    }

                    comboBox.SelectedIndex = 
                        comboBox.SelectedIndex == -1 ? 0 : comboBox.SelectedIndex;

                    comboBox.SelectedValueChanged += new System.EventHandler(ComboClickHandler);
                    this.SettingsPanel.Controls.Add(sectionLabel);
                    this.SettingsPanel.Controls.Add(settingLabel);
                    this.SettingsPanel.Controls.Add(comboBox);
                    iSettingCount++;
                    iYOffset += 25;
                }
            }

            vValidateNetworkSettings();
            this.Height = 100 + 25 * iSettingCount;
            this.SettingsPanel.Height = 10 + 25 * iSettingCount;
            this.SaveButton.Top = 30 + 25 * iSettingCount;
            this.ExitButton.Top = 30 + 25 * iSettingCount;
        }

        private void vValidateNetworkSettings()
        {
#if BUILD_WIFI
            string[] aszSections = Program.mAPP_mclsIniParser.EnumAll();
            int iComboPrimaryIDX;
            int iComboSecondaryIDX;

            try
            {
                /* retrieve the network adapter device list */
                CaptureDeviceList devices = CaptureDeviceList.Instance;
            
                if ((miSelectedInterfaceIDX >= 0) && (miSelectedInterfaceIDX < devices.Count))
                {
                    foreach (string szSection in aszSections)
                    {
                        if ("NetworkConnection" == szSection)
                        {
                            string[] aszSettings = Program.mAPP_mclsIniParser.EnumSection(szSection);

                            foreach (string szSetting in aszSettings)
                            {
                                switch (szSetting)
                                {
                                    case "NetworkAdapterMAC":
                                        {
                                            string szDeviceMacAddress;
                                            string szDeviceMacAddressDelimited;
                                            
                                            try
                                            {
                                                szDeviceMacAddress = devices[miSelectedInterfaceIDX].MacAddress.ToString();
                                                szDeviceMacAddressDelimited = szDeviceMacAddress.Substring(0, 2) + "-";
                                                szDeviceMacAddressDelimited += szDeviceMacAddress.Substring(2, 2) + "-";
                                                szDeviceMacAddressDelimited += szDeviceMacAddress.Substring(4, 2) + "-";
                                                szDeviceMacAddressDelimited += szDeviceMacAddress.Substring(6, 2) + "-";
                                                szDeviceMacAddressDelimited += szDeviceMacAddress.Substring(8, 2) + "-";
                                                szDeviceMacAddressDelimited += szDeviceMacAddress.Substring(10, 2);
                                            }
                                            catch
                                            {
                                                szDeviceMacAddressDelimited = "Unavailable";
                                            }

                                            iComboPrimaryIDX = Array.IndexOf(maszSettings, "NetworkAdapterMAC");
                                            
                                            if (0 != String.Compare(szDeviceMacAddressDelimited, maclsSettingComboBox[iComboPrimaryIDX].SelectedText))
                                            {
                                                maclsSettingComboBox[iComboPrimaryIDX].Items.Clear();
                                                maclsSettingComboBox[iComboPrimaryIDX].Items.Add(szDeviceMacAddressDelimited);
                                                maclsSettingComboBox[iComboPrimaryIDX].SelectedIndex = 0;
                                            }
                                            break;
                                        }
                                    case "LocalIPAddress":
                                        {
                                            List<string> lstszLocalIPAddress = new List<string>();
                                            string szOldLocalIPAddress;
                                            string szOldRemoteIPAddress;
                                            bool boOldLocalIPAddressAdded = false;
                                            bool boOldRemoteIPAddressAdded = false;
                                            string[] aszLocalIPAddress;
                                            string szRemoteIPAddress;
                                            int iAddressIDX;

                                            foreach (NetworkInterface clsNetwork in NetworkInterface.GetAllNetworkInterfaces())
                                            {
                                                foreach(UnicastIPAddressInformation clsIP in clsNetwork.GetIPProperties().UnicastAddresses)
                                                {
                                                    lstszLocalIPAddress.Add(clsIP.Address.ToString());
                                                }
                                            }

                                            iComboPrimaryIDX = Array.IndexOf(maszSettings, "LocalIPAddress");
                                            iComboSecondaryIDX = Array.IndexOf(maszSettings, "RemoteIPAddress");

                                            szOldLocalIPAddress = maclsSettingComboBox[iComboPrimaryIDX].SelectedItem.ToString();
                                            szOldRemoteIPAddress = maclsSettingComboBox[iComboSecondaryIDX].SelectedItem.ToString();

                                            maclsSettingComboBox[iComboPrimaryIDX].Items.Clear();
                                            maclsSettingComboBox[iComboSecondaryIDX].Items.Clear();

                                            foreach(string szIPAddress in lstszLocalIPAddress)
                                            {
                                                aszLocalIPAddress = szIPAddress.Split('.');

                                                if (4 == aszLocalIPAddress.Length)
                                                {
                                                    maclsSettingComboBox[iComboPrimaryIDX].Items.Add(szIPAddress);
                                                    if (szIPAddress == szOldLocalIPAddress) boOldLocalIPAddressAdded = true;


                                                    for (iAddressIDX = 0; iAddressIDX < 15; iAddressIDX++)
                                                    {
                                                        szRemoteIPAddress = aszLocalIPAddress[0] + '.';
                                                        szRemoteIPAddress += aszLocalIPAddress[1] + '.';
                                                        szRemoteIPAddress += aszLocalIPAddress[2] + '.';
                                                        szRemoteIPAddress += iAddressIDX.ToString();

                                                        if (szRemoteIPAddress == szOldRemoteIPAddress) boOldRemoteIPAddressAdded = true;
                                                        maclsSettingComboBox[iComboSecondaryIDX].Items.Add(szRemoteIPAddress);
                                                    }
                                                }
                                            }

                                            if ((false == boOldLocalIPAddressAdded) && ("NULL" != szOldLocalIPAddress))
                                            {
                                                maclsSettingComboBox[iComboPrimaryIDX].Items.Add(szOldLocalIPAddress);
                                                maclsSettingComboBox[iComboPrimaryIDX].SelectedIndex = maclsSettingComboBox[iComboPrimaryIDX].Items.Count - 1;
                                            }
                                            else
                                            {
                                                maclsSettingComboBox[iComboPrimaryIDX].SelectedIndex = 0;
                                            }

                                            if ((false == boOldRemoteIPAddressAdded) && ("NULL" != szOldRemoteIPAddress))
                                            {
                                                maclsSettingComboBox[iComboSecondaryIDX].Items.Add(szOldRemoteIPAddress);
                                                maclsSettingComboBox[iComboSecondaryIDX].SelectedIndex = maclsSettingComboBox[iComboSecondaryIDX].Items.Count - 1;
                                            }
                                            else
                                            {
                                                maclsSettingComboBox[iComboSecondaryIDX].SelectedIndex = 0;
                                            }
 
                                            break;
                                        }
                                    case "RemoteIPAddress":
                                        {
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                            }
                        }
                    } 
                }
                else
                {
                    MessageBox.Show("Unable to validate Network Settings");
                }
            }
            catch
            {
                MessageBox.Show("Unable to validate Network Settings");
            }
#endif
        }

        private string[] aszGetSettingOptions(string szSettingName)
        {
            ArrayList lstSettingOptions = new ArrayList();

            switch (szSettingName)
            {
                case "RP1210Lib":
                    {
                        string[] aszDLL = { "kv121032.dll" };

                        lstSettingOptions.AddRange(aszDLL);
                        break;
                    }
                case "ComsDeviceSerial":
                    {
                        string[] aszDeviceSerials = { "ED000200" };

                        lstSettingOptions.AddRange(aszDeviceSerials);
                        break;
                    }
                case "ComsBaud":
                    {
                        string[] aszDeviceSerials = { "9600", "19200", "56000", "133", "250", "500", "10", "100" };

                        lstSettingOptions.AddRange(aszDeviceSerials);
                        break;
                    }
                case "ComsPort":
                    {
                        string[] aszComPortTypes = new string[30];

                        int iPort;

                        for (iPort = 1; iPort < 31; iPort++)
                        {
                            aszComPortTypes[iPort - 1] = "COM" + iPort.ToString();
                        }

                        lstSettingOptions.AddRange(aszComPortTypes);
                        break;
                    }
                case "ComsDiagIDTX":
                    {
                        string[] aszConnectionTypes = new string[32];

                        int iIDTX;

                        for (iIDTX = 1800; iIDTX < 2048; iIDTX += 8)
                        {
                            aszConnectionTypes[(iIDTX - 1800) / 8] = iIDTX.ToString();
                        }

                        lstSettingOptions.AddRange(aszConnectionTypes);
                        break;
                    }
                case "ComsDiagIDRX":
                    {
                        string[] aszConnectionTypes = new string[32];

                        int iIDRX;

                        for (iIDRX = 1808; iIDRX < 2048; iIDRX += 8)
                        {
                            aszConnectionTypes[(iIDRX - 1808) / 8] = iIDRX.ToString();
                        }

                        lstSettingOptions.AddRange(aszConnectionTypes);
                        break;
                    }
                case "ConnectionType":
                    {
                        string[] aszConnectionTypes = { "CANAL", "CANRP1210", "USBCDC", "Serial", "CANLIBKVASER" };
                        lstSettingOptions.AddRange(aszConnectionTypes);
                        break;
                    }
                case "ComsTickus":
                    {
                        string[] aszComsTickRates = { "5000", "10000", "20000", "50000", "100000" };
                        lstSettingOptions.AddRange(aszComsTickRates);
                        break;
                    }
                case "SelectedDevice":
                    {
#if BUILD_WIFI
                        try
                        {
                            /* retrieve the network adapter device list */
                            CaptureDeviceList devices = CaptureDeviceList.Instance;

                            /* if no devices were found print an error */
                            if (devices.Count < 1)
                            {
                                throw new Exception("No network adapters found");
                            }

                            foreach (WinPcapDevice dev in devices)
                            {
                                string szDetectedDevice = null;

                                if (null != dev.Interface.FriendlyName)
                                {
                                    szDetectedDevice = dev.Interface.FriendlyName;
                                    lstSettingOptions.Add(szDetectedDevice);
                                }
                                else
                                {
                                    szDetectedDevice = dev.Description;
                                    lstSettingOptions.Add(szDetectedDevice);
                                }
                            }
                        }
                        catch
                        {
                            lstSettingOptions.Add("No Adapters Found");
                        }
#else
                        lstSettingOptions.Add("No Adapters Found");
#endif

                        break;
                    }

                case "SelectedA2L":
                    {
                        string[] aszA2LFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Database\\A2L Databases", "*.A2L");
                        string[] aszRelativeA2LFiles = new string[aszA2LFiles.Length];
                        int iIDX = 0;

                        foreach (string szFileString in aszA2LFiles)
                        {
                            string szFile = szFileString.Replace("Database\\A2L Databases", "^");
                            int iCharPos = szFile.IndexOf('^') + 2;
                            szFile = szFile.Substring(iCharPos);
                            aszRelativeA2LFiles[iIDX] = szFile;
                            iIDX++;
                        }

                        lstSettingOptions.AddRange(aszRelativeA2LFiles);
                        break;
                    }
                case "SelectedLayoutXML":
                    {
                        string[] aszGUILayoutXMLFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Database\\GUI Layout Databases", "*.XML");
                        string[] aszRelativeGUILayoutXMLFiles = new string[aszGUILayoutXMLFiles.Length];
                        int iIDX = 0;

                        foreach (string szFileString in aszGUILayoutXMLFiles)
                        {
                            string szFile = szFileString.Replace("Database\\GUI Layout Databases", "^");
                            int iCharPos = szFile.IndexOf('^') + 2;
                            szFile = szFile.Substring(iCharPos);
                            aszRelativeGUILayoutXMLFiles[iIDX] = szFile;
                            iIDX++;
                        }                        
                        
                        lstSettingOptions.AddRange(aszRelativeGUILayoutXMLFiles);
                        break;
                    }
                case "TransferBlockSize":
                    {
                        string[] aszBlockSizes = { "4", "8", "16", "32", "64", "128", "256", "512", "1024" };
                        lstSettingOptions.AddRange(aszBlockSizes);
                        break;
                    }
                case "MapViewCountMax":
                    {
                        string[] aszViewCounts = { "1", "2", "3", "4", "5", "10", "15", "20", "40" };
                        lstSettingOptions.AddRange(aszViewCounts);
                        break;
                    }
                case "TableViewCountMax":
                    {
                        string[] aszViewCounts = { "1", "2", "3", "4", "5", "10", "15", "20", "40" };
                        lstSettingOptions.AddRange(aszViewCounts);
                        break;
                    }
                case "CharViewCountMax":
                    {
                        string[] aszViewCounts = { "1", "2", "3", "4", "5", "10", "15", "20", "40" };
                        lstSettingOptions.AddRange(aszViewCounts);
                        break;
                    }
                case "GaugeViewCountMax":
                    {
                        string[] aszViewCounts = { "1", "2", "3", "4", "5", "10", "15", "20", "40" };
                        lstSettingOptions.AddRange(aszViewCounts);
                        break;
                    }
                default:
                    {
                        lstSettingOptions.Add("Setting unavailable");
                        break;
                    }
            }


            return (String[])lstSettingOptions.ToArray(typeof(String));
        }

        private void ComboClickHandler(object sender, System.EventArgs e)
        {
            int iSelectedComboBox = Array.IndexOf(maclsSettingComboBox, sender);
            string szSettingOption;

            szSettingOption = maclsSettingComboBox[iSelectedComboBox].SelectedItem.ToString();

            Program.mAPP_mclsIniParser.AddSetting(maclsLabelSection[iSelectedComboBox].Text,
                maclsLabelSetting[iSelectedComboBox].Text, szSettingOption);

            if (0 == string.Compare(maszSettings[iSelectedComboBox], "SelectedDevice"))
            {
                miSelectedInterfaceIDX = maclsSettingComboBox[iSelectedComboBox].SelectedIndex;
                vValidateNetworkSettings();
                MessageBox.Show("After changing the selected device please save and re-start the application with the new device connected", "Re-start required");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Program.mAPP_mclsIniParser.SaveSettings();
            MessageBox.Show("For changes to take effect the application must be restarted", "Restart pending");
            Program.vNotifyProgramState(tenProgramState.enRequestShutdown, 0);
            this.Hide();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void tclsIniEdit_Resize(object sender, EventArgs e)
        {
            int iYOffset = 10;

            SettingsPanel.Width = this.ClientRectangle.Width - 25;
            SettingsPanel.Height = this.ClientRectangle.Height - 75;

            if (null != maclsLabelSection)
            {
            foreach (Label sectionLabel in maclsLabelSection)
            {
                if (null != sectionLabel)
                {
                    sectionLabel.Location = new System.Drawing.Point(10, iYOffset);
                    sectionLabel.Size = new System.Drawing.Size((int)(0.18 * this.ClientRectangle.Width), 15);
                }

                iYOffset += 25;
            }

            iYOffset = 10;

            foreach (Label settingLabel in maclsLabelSetting)
            {
                if (null != settingLabel)
                {
                    settingLabel.Location = new System.Drawing.Point((int)(0.20 * this.ClientRectangle.Width), iYOffset);
                    settingLabel.Size = new System.Drawing.Size((int)(0.18 * this.ClientRectangle.Width), 15);
                }

                iYOffset += 25;
            }

            iYOffset = 10;

            foreach (ComboBox comboBox in maclsSettingComboBox)
            {
                if (null != comboBox)
                {
                    comboBox.Location = new System.Drawing.Point((int)(0.40 * this.ClientRectangle.Width), iYOffset);
                    comboBox.Size = new System.Drawing.Size((int)(0.55 * this.ClientRectangle.Width), 15);
                }

                iYOffset += 25;
            }
            }

            this.SaveButton.Top = this.ClientRectangle.Height - 45;
            this.ExitButton.Top = this.ClientRectangle.Height - 45;
            this.SaveButton.Left = this.ClientRectangle.Width - 95;
            this.ExitButton.Left = this.ClientRectangle.Width - 170;
        }
    }
}