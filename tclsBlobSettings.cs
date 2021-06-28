/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Blob settings                                          */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsBlobSettings.cs                                    */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using GraphLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP
{
    public partial class tclsBlobSettings : Form
    {
        GraphLib.PlotterDisplayEx mclsDisplayTrigger;
        Label[] maclsCharacteristicLabel;
        Label[] maclsCharacteristicUnitsLabel;
        TextBox[] maclsCharacteristicTextBox;
        Button[] maclsCharacteristicButton;
        int[] maiCharIndices;
        int[] maiCharCompuMethodIndices;
        ComboBox[] maclsCharacteristicComboBox;
        String[] maszCharCompuMethodFormat;
        List<ToolStripMenuItem> mlstTriggerWizardLibrary;
        String[] maszTriggers;
        int miFormIDX;
        bool mboRequestShutdown;
        bool[] mboEnumMode;

        public tclsBlobSettings(int iFormIDX)
        {
            InitializeComponent();
            mclsDisplayTrigger = new GraphLib.PlotterDisplayEx(false);
            mclsDisplayTrigger.Location = new System.Drawing.Point(20, 5);
            this.Controls.Add(mclsDisplayTrigger);
            mlstTriggerWizardLibrary = new List<ToolStripMenuItem>();
            miFormIDX = iFormIDX;

            /* These are core settings of Base Configuration */
            string[] aszSettingStrings = {
                "Primary Edge Setup",
                "Secondary Edge Setup",
                "Primary VR Enable",
                "Secondary VR Enable",
                "First Edge Rising Primary",
                "First Edge Rising Secondary",
                "Trigger Type",
                "Sync Type",
                "Sync Phase Repeats"};

            maclsCharacteristicLabel = new Label[aszSettingStrings.Length];
            maclsCharacteristicUnitsLabel = new Label[aszSettingStrings.Length];
            maclsCharacteristicTextBox = new TextBox[aszSettingStrings.Length];
            maclsCharacteristicButton = new Button[aszSettingStrings.Length];
            maiCharIndices = new int[aszSettingStrings.Length];
            maiCharCompuMethodIndices = new int[aszSettingStrings.Length];
            maszCharCompuMethodFormat = new String[aszSettingStrings.Length];
            maclsCharacteristicComboBox = new ComboBox[aszSettingStrings.Length];
            mboEnumMode = new bool[aszSettingStrings.Length];

            int iCharacteristicIDX = 0;

            foreach (string szSetting in aszSettingStrings)
            {
                int iCharIDX = tclsASAM.iGetCharIndex(szSetting);

                maiCharIndices[iCharacteristicIDX] = iCharIDX;

                maclsCharacteristicLabel[iCharacteristicIDX] = new Label();
                maclsCharacteristicLabel[iCharacteristicIDX].AutoSize = false;
                maclsCharacteristicLabel[iCharacteristicIDX].TextAlign = ContentAlignment.MiddleRight;
                maclsCharacteristicLabel[iCharacteristicIDX].Text = szSetting;
                maclsCharacteristicLabel[iCharacteristicIDX].Left = 620;
                maclsCharacteristicLabel[iCharacteristicIDX].Top = 30 + 25 * iCharacteristicIDX;
                maclsCharacteristicLabel[iCharacteristicIDX].Width = 160;
                this.Controls.Add(maclsCharacteristicLabel[iCharacteristicIDX]);

                maclsCharacteristicTextBox[iCharacteristicIDX] = new TextBox();
                maclsCharacteristicTextBox[iCharacteristicIDX].Text = "0";
                maclsCharacteristicTextBox[iCharacteristicIDX].Left = 780;
                maclsCharacteristicTextBox[iCharacteristicIDX].Top = 30 + 25 * iCharacteristicIDX;
                maclsCharacteristicTextBox[iCharacteristicIDX].Width = 70;
                maclsCharacteristicTextBox[iCharacteristicIDX].BackColor = Color.FromArgb(255, 255, 255, 200);
                this.Controls.Add(maclsCharacteristicTextBox[iCharacteristicIDX]);

                maclsCharacteristicComboBox[iCharacteristicIDX] = new ComboBox();
                maclsCharacteristicComboBox[iCharacteristicIDX].Top = 30 + 25 * iCharacteristicIDX;
                maclsCharacteristicComboBox[iCharacteristicIDX].Width = 70;
                maclsCharacteristicComboBox[iCharacteristicIDX].BackColor = Color.FromArgb(255, 255, 255, 200);
                maclsCharacteristicComboBox[iCharacteristicIDX].SelectedIndexChanged +=
                    this.ComboBoxChanged;
                this.Controls.Add(maclsCharacteristicComboBox[iCharacteristicIDX]);

                maiCharCompuMethodIndices[iCharacteristicIDX] = tclsASAM.iGetCompuMethodIndexFromChar(szSetting);

                if (tenCM_Type.enTAB_VERB == tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].enCM_Type)
                {
                    int iVerbCount = tclsASAM.milstVarVerbList[tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].iVerbTableIDX].lstVarVerb.Count;

                    if (0 < iVerbCount)
                    {
                        foreach (tstVerbRecord stVerbRecord in tclsASAM.milstVarVerbList[tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].iVerbTableIDX].lstVarVerb)
                        {
                            maclsCharacteristicComboBox[iCharacteristicIDX].Items.Add(stVerbRecord.szVerb);
                        }
                    }

                    mboEnumMode[iCharacteristicIDX] = true;
                }

                maclsCharacteristicUnitsLabel[iCharacteristicIDX] = new Label();
                maclsCharacteristicUnitsLabel[iCharacteristicIDX].AutoSize = false;
                maclsCharacteristicUnitsLabel[iCharacteristicIDX].TextAlign = ContentAlignment.MiddleLeft;

                if (-1 < maiCharCompuMethodIndices[iCharacteristicIDX])
                {
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].Text =
                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].szUnitsString;
                }
                maclsCharacteristicUnitsLabel[iCharacteristicIDX].Left = 860;
                maclsCharacteristicUnitsLabel[iCharacteristicIDX].Top = 30 + 25 * iCharacteristicIDX;
                maclsCharacteristicUnitsLabel[iCharacteristicIDX].Width = 30;
                this.Controls.Add(maclsCharacteristicUnitsLabel[iCharacteristicIDX]);

                maclsCharacteristicButton[iCharacteristicIDX] = new Button();
                maclsCharacteristicButton[iCharacteristicIDX].Text = "Enter";
                maclsCharacteristicButton[iCharacteristicIDX].Left = 910 ;
                maclsCharacteristicButton[iCharacteristicIDX].Top = 30 + 25 * iCharacteristicIDX;
                maclsCharacteristicButton[iCharacteristicIDX].Click +=
                    this.EnterButtonClicked;
                this.Controls.Add(maclsCharacteristicButton[iCharacteristicIDX]);

                if (-1 != maiCharCompuMethodIndices[iCharacteristicIDX])
                {
                    vCreateCharCompuMethodFormat(iCharacteristicIDX);
                }
                else
                {
                    Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0,
                        "No compu-method found for " + szSetting);
                }

                iCharacteristicIDX++;
            }


            foreach(tclsWindowElement clsWindowElement in Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX])
            {
                if (0 == String.Compare(clsWindowElement.szElementType, "characteristic"))
                {
                    int iCharIDX = tclsASAM.iGetCharIndex(clsWindowElement.szA2LName);

                    if (0 <= iCharIDX)
                    {
                        if (tenParamType.enPT_VALUE == tclsASAM.milstCharacteristicList[iCharIDX].enParamType)
                        {
                            Array.Resize(ref maiCharIndices, iCharacteristicIDX + 1);
                            Array.Resize(ref maiCharCompuMethodIndices, iCharacteristicIDX + 1);
                            Array.Resize(ref maszCharCompuMethodFormat, iCharacteristicIDX + 1);
                            maiCharIndices[iCharacteristicIDX] = tclsASAM.iGetCharIndex(clsWindowElement.szA2LName);
                            maiCharCompuMethodIndices[iCharacteristicIDX] = tclsASAM.iGetCompuMethodIndexFromChar(clsWindowElement.szA2LName);

                            iCharIDX = tclsASAM.iGetCharIndex(clsWindowElement.szLabel);

                            maiCharIndices[iCharacteristicIDX] = iCharIDX;

                            Array.Resize(ref maclsCharacteristicButton, iCharacteristicIDX + 1);
                            Array.Resize(ref maclsCharacteristicLabel, iCharacteristicIDX + 1);
                            Array.Resize(ref maclsCharacteristicTextBox, iCharacteristicIDX + 1);
                            Array.Resize(ref maclsCharacteristicUnitsLabel, iCharacteristicIDX + 1);
                            Array.Resize(ref mboEnumMode, iCharacteristicIDX + 1);

                            maclsCharacteristicLabel[iCharacteristicIDX] = new Label();
                            maclsCharacteristicLabel[iCharacteristicIDX].AutoSize = false;
                            maclsCharacteristicLabel[iCharacteristicIDX].TextAlign = ContentAlignment.MiddleRight;
                            maclsCharacteristicLabel[iCharacteristicIDX].Text = clsWindowElement.szA2LName;
                            maclsCharacteristicLabel[iCharacteristicIDX].Left = 600;
                            maclsCharacteristicLabel[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                            maclsCharacteristicLabel[iCharacteristicIDX].Width = 160;
                            this.Controls.Add(maclsCharacteristicLabel[iCharacteristicIDX]);

                            maclsCharacteristicTextBox[iCharacteristicIDX] = new TextBox();
                            maclsCharacteristicTextBox[iCharacteristicIDX].Text = "0";
                            maclsCharacteristicTextBox[iCharacteristicIDX].Left = 760;
                            maclsCharacteristicTextBox[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                            maclsCharacteristicTextBox[iCharacteristicIDX].Width = 70;
                            maclsCharacteristicTextBox[iCharacteristicIDX].BackColor = Color.FromArgb(255, 255, 255, 200);
                            this.Controls.Add(maclsCharacteristicTextBox[iCharacteristicIDX]);

                            maiCharCompuMethodIndices[iCharacteristicIDX] = tclsASAM.iGetCompuMethodIndexFromChar(clsWindowElement.szA2LName);

                            maclsCharacteristicUnitsLabel[iCharacteristicIDX] = new Label();
                            maclsCharacteristicUnitsLabel[iCharacteristicIDX].AutoSize = false;
                            maclsCharacteristicUnitsLabel[iCharacteristicIDX].TextAlign = ContentAlignment.MiddleLeft;

                            if (-1 < maiCharCompuMethodIndices[iCharacteristicIDX])
                            {
                                maclsCharacteristicUnitsLabel[iCharacteristicIDX].Text =
                                    tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].szUnitsString;
                            }
                            maclsCharacteristicUnitsLabel[iCharacteristicIDX].Left = 840;
                            maclsCharacteristicUnitsLabel[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                            maclsCharacteristicUnitsLabel[iCharacteristicIDX].Width = 50;
                            this.Controls.Add(maclsCharacteristicUnitsLabel[iCharacteristicIDX]);

                            maclsCharacteristicButton[iCharacteristicIDX] = new Button();
                            maclsCharacteristicButton[iCharacteristicIDX].Text = "Enter";
                            maclsCharacteristicButton[iCharacteristicIDX].Left = 910;
                            maclsCharacteristicButton[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                            maclsCharacteristicButton[iCharacteristicIDX].Click +=
                                this.EnterButtonClicked;
                            this.Controls.Add(maclsCharacteristicButton[iCharacteristicIDX]);

                            if (-1 != maiCharCompuMethodIndices[iCharacteristicIDX])
                            {
                                vCreateCharCompuMethodFormat(iCharacteristicIDX);
                            }
                            else
                            {
                                Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0,
                                    "No compu-method found for " + clsWindowElement.szA2LName);
                            }

                            iCharacteristicIDX++;
                        }
                    }
                    else
                    {
                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0,
                            "No A2L characteristic record for " + clsWindowElement.szA2LName);
                    }
                }
            }


            UInt16[] au16PrimaryTriggerData = {0};
            UInt16[] au16SecondaryTriggerData = { 0 };
            UInt16[] au16SyncPointsData = { 0 };
            byte u8FirstEdgeRisingPrimary = 0;
            byte u8EdgePolarityPrimary = 0;
            byte u8FirstEdgeRisingSecondary = 0;
            byte u8EdgePolaritySecondary = 0;
            byte u8VREnablePrimary = 0;
            byte u8VREnableSecondary = 0;

            /* Draw the panel with these parameters */
            InitialiseTriggerPanel(au16PrimaryTriggerData, au16SecondaryTriggerData, au16SyncPointsData, u8FirstEdgeRisingPrimary, u8EdgePolarityPrimary, u8FirstEdgeRisingSecondary, u8EdgePolaritySecondary, u8VREnablePrimary, u8VREnableSecondary);
        }

        private void vCreateCharCompuMethodFormat(int iCharIDX)
        {
            String szFormatString = "{0:";
            szFormatString += "0";

            if (0 < tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharIDX]].iPostDPCount)
            {
                szFormatString += "." +
                new String('0',
                tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharIDX]].iPostDPCount) + "}";
            }
            else
            {
                szFormatString += "}";
            }

            maszCharCompuMethodFormat[iCharIDX] = szFormatString;
        }

        private void InitialiseTriggerPanel(UInt16[] au16TriggerData, UInt16[] au16SecondaryTriggerData, UInt16[] au16SyncPointsData, byte u8FirstEdgeRisingPrimary, byte u8EdgePolarityPrimary, byte u8FirstEdgeRisingSecondary, byte u8EdgePolaritySecondary, byte u8VREnablePrimary, byte u8VREnableSecondary)
        {
            mclsDisplayTrigger.DataSources.Clear();
            mclsDisplayTrigger.DataSources.Add(new DataSource());
            mclsDisplayTrigger.DataSources.Add(new DataSource());
            mclsDisplayTrigger.DataSources.Add(new DataSource());

            mclsDisplayTrigger.BackgroundColorTop = Color.Black;
            mclsDisplayTrigger.BackgroundColorBot = Color.DarkGray;
            mclsDisplayTrigger.SolidGridColor = Color.DarkGray;
            mclsDisplayTrigger.DashedGridColor = Color.GreenYellow;
            mclsDisplayTrigger.ForeColor = Color.GreenYellow;
            mclsDisplayTrigger.Smoothing = System.Drawing.Drawing2D.SmoothingMode.None;

            cPoint[] acPointPrimary = GetSimulatedWavePoints(au16TriggerData, u8FirstEdgeRisingPrimary, u8EdgePolarityPrimary, u8VREnablePrimary, 0f, true);
            mclsDisplayTrigger.DataSources[0].Samples= acPointPrimary;

            cPoint[] acPointSecondary = GetSimulatedWavePoints(au16SecondaryTriggerData, u8FirstEdgeRisingSecondary, u8EdgePolaritySecondary, u8VREnableSecondary, 1.4f, false);
            mclsDisplayTrigger.DataSources[1].Samples = acPointSecondary;

            cPoint[] acPointSync = GetSimulatedWavePoints(au16SyncPointsData, 1, 2, 0, 2.8f, true);
            mclsDisplayTrigger.DataSources[2].Samples = acPointSync;

            mclsDisplayTrigger.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;

            mclsDisplayTrigger.DataSources[0].GraphColor = Color.Yellow;
            mclsDisplayTrigger.DataSources[1].GraphColor = Color.LightBlue;
            mclsDisplayTrigger.DataSources[2].GraphColor = Color.LightCoral;

            mclsDisplayTrigger.SetDisplayRangeX(0, 675);
            mclsDisplayTrigger.SetGridDistanceX(1000);
            mclsDisplayTrigger.DataSources[0].SetDisplayRangeY(0, 4);
            mclsDisplayTrigger.DataSources[0].FixedOrdinates = true;
            mclsDisplayTrigger.DataSources[0].SetDataSourceDrawnAll(false);

            mclsDisplayTrigger.DataSources[1].SetDisplayRangeY(0, 4);
            mclsDisplayTrigger.DataSources[1].FixedOrdinates = true;
            mclsDisplayTrigger.DataSources[1].SetDataSourceDrawnAll(false);

            mclsDisplayTrigger.DataSources[2].SetDisplayRangeY(0, 4);
            mclsDisplayTrigger.DataSources[2].FixedOrdinates = true;
            mclsDisplayTrigger.DataSources[2].SetDataSourceDrawnAll(false);

            mclsDisplayTrigger.Refresh();
        }

        private cPoint[] GetSimulatedWavePoints(UInt16[] au16Data, byte firstEdgeRising, byte edgePolarity, byte VREnable, float offset, bool repeat)
        {
            int iTriggerEdgeCount = GetTriggerArrayLength(au16Data);
            int iPointCount = 0;
            int iStartOffset = 0 == au16Data[0] ? 0 : 1;
            UInt16[] au16DataRepeat;

            if (true == repeat)
            {
                int iDataIDX = 0;
                iTriggerEdgeCount *= 2;
                au16DataRepeat = new UInt16[iTriggerEdgeCount];

                for (iDataIDX = 0; iDataIDX < (iTriggerEdgeCount / 2); iDataIDX++)
                {
                    au16DataRepeat[iDataIDX] = (UInt16)(au16Data[iDataIDX] / 2);
                }

                for (iDataIDX = 0; iDataIDX < (iTriggerEdgeCount / 2); iDataIDX++)
                {
                    au16DataRepeat[iDataIDX + (iTriggerEdgeCount / 2)] = (UInt16)(0x8000 + au16Data[iDataIDX] / 2);
                }
            }
            else
            {
                au16DataRepeat = new UInt16[iTriggerEdgeCount];
                Array.Copy(au16Data, au16DataRepeat, iTriggerEdgeCount);
            }


            if ((0 == edgePolarity) || (1 == edgePolarity))
            {
                if (0 == VREnable)
                {
                    iPointCount = 4 * iTriggerEdgeCount + 1;
                    iPointCount += iStartOffset;
                }
                else
                {
                    iPointCount = 2 * iTriggerEdgeCount + 1;
                    iPointCount += iStartOffset;
                }
            }
            else
            {
                iPointCount = 2 * iTriggerEdgeCount + 1;
                iPointCount += iStartOffset;
            }

            cPoint[] acPoints = new cPoint[iPointCount];

            if (0 != au16Data[0])
            {
                acPoints[0].x = 10;

                if (1 == firstEdgeRising)
                {
                    acPoints[0].y = 0.1f + offset;
                }
                else
                {
                    acPoints[0].y = 0.9f + offset;
                }
            }


            if ((0 == edgePolarity) || (1 == edgePolarity))
            {
                if (0 == VREnable)
                {
                    for (int iPointIDX = iStartOffset; iPointIDX < acPoints.Length - iStartOffset - 1; iPointIDX += 4)
                    {
                        acPoints[iPointIDX + iStartOffset].x = (Single)au16DataRepeat[iPointIDX / 4] / 100f + 10;
                        acPoints[iPointIDX + 1 + iStartOffset].x = (Single)au16DataRepeat[iPointIDX / 4] / 100f + 10;

                        if ((iPointIDX / 4) < au16DataRepeat.Length - 1)
                        {
                            acPoints[iPointIDX + 2 + iStartOffset].x = (Single)au16DataRepeat[iPointIDX / 4] / 200f + 10 + (Single)au16DataRepeat[(iPointIDX / 4) + 1] / 200f;

                            if ((iPointIDX + 3 + iStartOffset) < acPoints.Length)
                            {
                                acPoints[iPointIDX + 3 + iStartOffset].x = (Single)au16DataRepeat[iPointIDX / 4] / 200f + 10 + (Single)au16DataRepeat[(iPointIDX / 4) + 1] / 200f;
                            }
                        }
                        else
                        {
                            acPoints[iPointIDX + 2 + iStartOffset].x = (Single)au16DataRepeat[iPointIDX / 4] / 200f + 10 + 65500 / 200f;

                            if ((iPointIDX + 3 + iStartOffset) < acPoints.Length)
                            {
                                acPoints[iPointIDX + 3 + iStartOffset].x = (Single)au16DataRepeat[iPointIDX / 4] / 200f + 10 + 65500 / 200f;
                            }
                        }

                        if (1 == firstEdgeRising)
                        {
                            acPoints[iPointIDX + iStartOffset].y = 0 == (iPointIDX % 2) ? 0.1f + offset : 0.9f + offset;

                            if ((iPointIDX + 3 + iStartOffset) < acPoints.Length)
                            {
                                acPoints[iPointIDX + iStartOffset + 3].y = 0 == (iPointIDX % 2) ? 0.1f + offset : 0.9f + offset;
                            }
                        }
                        else
                        {
                            acPoints[iPointIDX + iStartOffset].y = 0 == (iPointIDX % 2) ? 0.9f + offset : 0.1f + offset;

                            if ((iPointIDX + 3 + iStartOffset) < acPoints.Length)
                            {
                                acPoints[iPointIDX + iStartOffset + 3].y = 0 == (iPointIDX % 2) ? 0.9f + offset : 0.1f + offset;
                            }
                        }

                        if (1 == firstEdgeRising)
                        {
                            acPoints[iPointIDX + iStartOffset + 1].y = 0 != (iPointIDX % 2) ? 0.1f + offset : 0.9f + offset;
                            acPoints[iPointIDX + iStartOffset + 2].y = 0 != (iPointIDX % 2) ? 0.1f + offset : 0.9f + offset;
                        }
                        else
                        {
                            acPoints[iPointIDX + iStartOffset + 1].y = 0 != (iPointIDX % 2) ? 0.9f + offset : 0.1f + offset;
                            acPoints[iPointIDX + iStartOffset + 2].y = 0 != (iPointIDX % 2) ? 0.9f + offset : 0.1f + offset;
                        }
                    }

                    acPoints[acPoints.Length - 1].x = 665;

                    if (1 == firstEdgeRising)
                    {
                        acPoints[acPoints.Length - 1].y = 0.1f + offset;
                    }
                    else
                    {
                        acPoints[acPoints.Length - 1].y = 0.9f + offset;
                    }
                }
                else
                {
                    for (int iPointIDX = iStartOffset; iPointIDX < acPoints.Length - iStartOffset - 1; iPointIDX += 2)
                    {
                        acPoints[iPointIDX].x = (Single)au16DataRepeat[iPointIDX / 2] / 100f + 10;
                        acPoints[iPointIDX + 1].x = (Single)au16DataRepeat[iPointIDX / 2] / 100f + 10;

                        if (1 == edgePolarity)
                        {
                            acPoints[iPointIDX].y = 0.9f + offset;
                            acPoints[iPointIDX + 1].y = 0.1f + offset;
                        }
                        else
                        {
                            acPoints[iPointIDX].y = 0.1f + offset;
                            acPoints[iPointIDX + 1].y = 0.9f + offset;
                        }
                    }

                    acPoints[acPoints.Length - 1].x = 665;

                    if (1 == edgePolarity)
                    {
                        acPoints[acPoints.Length - 1].y = 0.9f + offset;
                    }
                    else
                    {
                        acPoints[acPoints.Length - 1].y = 0.1f + offset;
                    }
                }
            }
            else
            {
                if (0 == VREnable)
                {
                    for (int iPointIDX = iStartOffset; iPointIDX < acPoints.Length - iStartOffset - 1; iPointIDX += 2)
                    {
                        acPoints[iPointIDX].x = (Single)au16DataRepeat[(iPointIDX - iStartOffset) / 2] / 100f + 10;
                        acPoints[iPointIDX + 1].x = (Single)au16DataRepeat[(iPointIDX - iStartOffset) / 2] / 100f + 10;

                        if (1 == firstEdgeRising)
                        {
                            acPoints[iPointIDX].y = 0 == ((iPointIDX - iStartOffset) % 4) ? 0.1f + offset : 0.9f + offset;
                        }
                        else
                        {
                            acPoints[iPointIDX].y = 0 == ((iPointIDX - iStartOffset) % 4) ? 0.9f + offset : 0.1f + offset;
                        }

                        if (1 == firstEdgeRising)
                        {
                            acPoints[iPointIDX + 1].y = 0 != ((iPointIDX - iStartOffset) % 4) ? 0.1f + offset : 0.9f + offset;
                        }
                        else
                        {
                            acPoints[iPointIDX + 1].y = 0 != ((iPointIDX - iStartOffset) % 4) ? 0.9f + offset : 0.1f + offset;
                        }
                    }

                    if (1 == firstEdgeRising)
                    {
                        if (3 < acPoints.Length)
                        {
                            acPoints[acPoints.Length - 1].y = 0.1f + offset;
                        }
                        else
                        {
                            acPoints[acPoints.Length - 1].y = 0.9f + offset;
                        }
                    }
                    else
                    {
                        if (3 < acPoints.Length)
                        {
                            acPoints[acPoints.Length - 1].y = 0.9f + offset;
                        }
                        else
                        {
                            acPoints[acPoints.Length - 1].y = 0.1f + offset;
                        }
                    }
                }
                else
                {
                    if (0 != VREnable)
                    {
                        if (1 == firstEdgeRising)
                        {
                            acPoints[acPoints.Length - 1].y = 0.1f + offset;
                        }
                        else
                        {
                            acPoints[acPoints.Length - 1].y = 0.9f + offset;
                        }

                        if (1 == firstEdgeRising)
                        {
                            acPoints[acPoints.Length - 1].y = 0.9f + offset;
                        }
                        else
                        {
                            acPoints[acPoints.Length - 1].y = 0.1f + offset;
                        }
                    }
                }

                acPoints[acPoints.Length - 1].x = 665;
            }

            return acPoints;
        }

        private int GetTriggerArrayLength(UInt16[] au16TriggerData)
        {
            int iTriggerEdgeCount = 1;

            for (int triggerEdgeIDX = 1; triggerEdgeIDX < au16TriggerData.Length; triggerEdgeIDX++)
            {
                if (0 < au16TriggerData[triggerEdgeIDX])
                {
                    iTriggerEdgeCount++;
                }
                else
                {
                    break;
                }
            }

            return iTriggerEdgeCount;
        }


        public void vRefreshFromDataPage()
        {
            UInt16[] au16PrimaryTriggerData = null;
            UInt16[] au16SecondaryTriggerData = null;
            UInt16[] au16SyncPointsData = null;
            byte u8FirstEdgeRisingPrimary = 0;
            byte u8FirstEdgeRisingSecondary = 0;
            byte u8EdgePolarityPrimary = 0;
            byte u8EdgePolaritySecondary = 0;
            byte u8VREnablePrimary = 0;
            byte u8VREnableSecondary = 0;

            int iPrimaryTriggerIDX = tclsASAM.iGetBlobIndex("Primary Trigger Table");

            if (-1 != iPrimaryTriggerIDX)
            {
                int iPrimaryTriggerTablePointCount = tclsASAM.milstBlobList[iPrimaryTriggerIDX].iPointCount;

                if (tenRecLayout.enRL_VALU16 == tclsASAM.milstBlobList[iPrimaryTriggerIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstBlobList[iPrimaryTriggerIDX].u32Address;
                    au16PrimaryTriggerData = new UInt16[iPrimaryTriggerTablePointCount];
                    tclsDataPage.au16GetWorkingData(u32Address, ref au16PrimaryTriggerData);
                }
            }

            int iSecondaryTriggerIDX = tclsASAM.iGetBlobIndex("Secondary Trigger Table");

            if (-1 != iSecondaryTriggerIDX)
            {
                int iSecondaryTriggerTablePointCount = tclsASAM.milstBlobList[iSecondaryTriggerIDX].iPointCount;

                if (tenRecLayout.enRL_VALU16 == tclsASAM.milstBlobList[iSecondaryTriggerIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstBlobList[iSecondaryTriggerIDX].u32Address;
                    au16SecondaryTriggerData = new UInt16[iSecondaryTriggerTablePointCount];
                    tclsDataPage.au16GetWorkingData(u32Address, ref au16SecondaryTriggerData);
                }
            }

            int iSyncPointsIDX = tclsASAM.iGetBlobIndex("Sync Points Table");

            if (-1 != iSyncPointsIDX)
            {
                int iSyncPointsTablePointCount = tclsASAM.milstBlobList[iSyncPointsIDX].iPointCount;

                if (tenRecLayout.enRL_VALU16 == tclsASAM.milstBlobList[iSyncPointsIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstBlobList[iSyncPointsIDX].u32Address;
                    au16SyncPointsData = new UInt16[iSyncPointsTablePointCount];
                    tclsDataPage.au16GetWorkingData(u32Address, ref au16SyncPointsData);
                }
            }

            int iEdgePolarityPrimaryIDX = tclsASAM.iGetCharIndex("Primary Edge Setup");

            if (-1 != iEdgePolarityPrimaryIDX)
            {
                if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iEdgePolarityPrimaryIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstCharacteristicList[iEdgePolarityPrimaryIDX].u32Address;
                    tclsDataPage.u8GetWorkingData(u32Address, ref u8EdgePolarityPrimary);
                }
            }

            int iEdgePolaritySecondaryIDX = tclsASAM.iGetCharIndex("Secondary Edge Setup");

            if (-1 != iEdgePolaritySecondaryIDX)
            {
                if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iEdgePolaritySecondaryIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstCharacteristicList[iEdgePolaritySecondaryIDX].u32Address;
                    tclsDataPage.u8GetWorkingData(u32Address, ref u8EdgePolaritySecondary);
                }
            }

            int iVREnablePrimaryIDX = tclsASAM.iGetCharIndex("Primary VR Enable");

            if (-1 != iVREnablePrimaryIDX)
            {
                if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iVREnablePrimaryIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstCharacteristicList[iVREnablePrimaryIDX].u32Address;
                    tclsDataPage.u8GetWorkingData(u32Address, ref u8VREnablePrimary);
                }
            }

            int iVREnableSecondaryIDX = tclsASAM.iGetCharIndex("Secondary VR Enable");

            if (-1 != iVREnableSecondaryIDX)
            {
                if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iVREnableSecondaryIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstCharacteristicList[iVREnableSecondaryIDX].u32Address;
                    tclsDataPage.u8GetWorkingData(u32Address, ref u8VREnableSecondary);
                }
            }

            int iFirstEdgeRisingPrimaryIDX = tclsASAM.iGetCharIndex("First Edge Rising Primary");

            if (-1 != iFirstEdgeRisingPrimaryIDX)
            {
                if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iFirstEdgeRisingPrimaryIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstCharacteristicList[iFirstEdgeRisingPrimaryIDX].u32Address;
                    tclsDataPage.u8GetWorkingData(u32Address, ref u8FirstEdgeRisingPrimary);
                }
            }

            int iFirstEdgeRisingSecondaryIDX = tclsASAM.iGetCharIndex("First Edge Rising Secondary");

            if (-1 != iFirstEdgeRisingSecondaryIDX)
            {
                if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iFirstEdgeRisingSecondaryIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstCharacteristicList[iFirstEdgeRisingSecondaryIDX].u32Address;
                    tclsDataPage.u8GetWorkingData(u32Address, ref u8FirstEdgeRisingSecondary);
                }
            }

            InitialiseTriggerPanel(au16PrimaryTriggerData, au16SecondaryTriggerData, au16SyncPointsData, u8FirstEdgeRisingPrimary, u8EdgePolarityPrimary, u8FirstEdgeRisingSecondary, u8EdgePolaritySecondary, u8VREnablePrimary, u8VREnableSecondary);
            RefreshTextBoxesFromDataPage();
        }

        private void RefreshTextBoxesFromDataPage()
        { 
            int iCharIDX;
            Single sCharData = 0;

            for (iCharIDX = 0; iCharIDX < maclsCharacteristicTextBox.Length; iCharIDX++)
            {
                if (-1 < maiCharIndices[iCharIDX])
                {
                    UInt32 u32Address = tclsASAM.milstCharacteristicList[maiCharIndices[iCharIDX]].u32Address;

                    switch (tclsASAM.milstCharacteristicList[maiCharIndices[iCharIDX]].enRecLayout)
                    {
                        case tenRecLayout.enRL_VALU8:
                            {
                                byte u8CharData = 0;
                                tclsDataPage.u8GetWorkingData(u32Address, ref u8CharData);
                                sCharData = (Single)u8CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALU16:
                            {
                                UInt16 u16CharData = 0;
                                tclsDataPage.u16GetWorkingData(u32Address, ref u16CharData);
                                sCharData = (Single)u16CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALU32:
                            {
                                UInt32 u32CharData = 0;
                                tclsDataPage.u32GetWorkingData(u32Address, ref u32CharData);
                                sCharData = (Single)u32CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS8:
                            {
                                sbyte s8CharData = 0;
                                tclsDataPage.s8GetWorkingData(u32Address, ref s8CharData);
                                sCharData = (Single)s8CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS16:
                            {
                                Int16 s16CharData = 0;
                                tclsDataPage.s16GetWorkingData(u32Address, ref s16CharData);
                                sCharData = (Single)s16CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS32:
                            {
                                Int32 s32CharData = 0;
                                tclsDataPage.s32GetWorkingData(u32Address, ref s32CharData);
                                sCharData = (Single)s32CharData;
                                break;
                            }
                        default:
                            {
                                sCharData = 0;
                                break;
                            }
                    }
                }

                if (-1 < maiCharCompuMethodIndices[iCharIDX])
                {
                    if (false == mboEnumMode[iCharIDX])
                    {
                        maclsCharacteristicTextBox[iCharIDX].Text = szGetScaledData(maiCharCompuMethodIndices[iCharIDX], iCharIDX, sCharData, tenMCVElementType.enMCVChar);
                    }
                    else
                    {
                        int iVerbData = (Int32)sCharData;
                        int iVerbIDX = tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharIDX]].iVerbTableIDX;

                        foreach (tstVerbRecord stVerbRecord in tclsASAM.milstVarVerbList[iVerbIDX].lstVarVerb)
                        {
                            if (iVerbData == stVerbRecord.iValueLow)
                            {
                                iVerbData = stVerbRecord.iValueLow;
                                int iComboIDX = maclsCharacteristicComboBox[iCharIDX].Items.IndexOf(stVerbRecord.szVerb);

                                if (-1 != iComboIDX)
                                {
                                    maclsCharacteristicComboBox[iCharIDX].SelectedIndex = iComboIDX;
                                }
                            }
                        }
                    }
                }
            }
        }

        private String szGetScaledData(int iCompuMethodIDX, int iElementIDX, Single sUnscaledData, tenMCVElementType enMCVElementType)
        {
            Single sScaledData;
            String szFormattedData;

            switch (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type)
            {
                case tenCM_Type.enIDENTICAL:
                    {
                        sScaledData = sUnscaledData;
                        break;
                    }
                case tenCM_Type.enLINEAR:
                    {
                        sScaledData = tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1 * sUnscaledData;
                        sScaledData += tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2;
                        break;
                    }
                default:
                    {
                        sScaledData = sUnscaledData;
                        break;
                    }
            }

            switch (enMCVElementType)
            {
                case tenMCVElementType.enMCVChar:
                    {
                        szFormattedData = String.Format(maszCharCompuMethodFormat[iElementIDX], sScaledData);
                        break;
                    }
                default:
                    {
                        szFormattedData = "-";
                        break;
                    }
            }

            return szFormattedData;
        }

        private void tclsBlobSettings_Load(object sender, EventArgs e)
        {
            loadTriggerWizardMenu();
            loadToothWizardMenu();
        }

        private void ComboBoxChanged(object sender, EventArgs e)
        {
            int iCharacteristicIDX;
            int iVerbData = -1;

            iCharacteristicIDX = Array.IndexOf(maclsCharacteristicComboBox, sender);

            String szSelectedVerb = maclsCharacteristicComboBox[iCharacteristicIDX].SelectedItem.ToString();

            int iVerbIDX = tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].iVerbTableIDX;

            foreach(tstVerbRecord stVerbRecord in tclsASAM.milstVarVerbList[iVerbIDX].lstVarVerb)
            {
                if (szSelectedVerb == stVerbRecord.szVerb)
                {
                    iVerbData = stVerbRecord.iValueLow;
                }
            }

            if (-1 != iVerbData)
            {
                maclsCharacteristicTextBox[iCharacteristicIDX].Text = Convert.ToString(iVerbData);
            }
        }

        private void EnterButtonClicked(object sender, EventArgs e)
        {
            int iCharacteristicIDX;
            byte[] au8Data;
            Single sInputData;
            Single sCellDataRaw;

            iCharacteristicIDX = Array.IndexOf(maclsCharacteristicButton, sender);

            try
            {
                sInputData = Convert.ToSingle(maclsCharacteristicTextBox[iCharacteristicIDX].Text);
                sInputData = tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].sLowerLim > sInputData ?
                    tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].sLowerLim : sInputData;
                sInputData = tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].sUpperLim < sInputData ?
                    tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].sUpperLim : sInputData;

                maclsCharacteristicTextBox[iCharacteristicIDX].Text = String.Format(maszCharCompuMethodFormat[iCharacteristicIDX], sInputData);


                switch (tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].enCM_Type)
                {
                    case tenCM_Type.enLINEAR:
                        {
                            sCellDataRaw = sInputData - tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].sCoeff2;
                            sCellDataRaw /= tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].sCoeff1;
                            break;
                        }
                    default:
                        {
                            sCellDataRaw = sInputData;
                            break;
                        }
                }

                switch (tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].iByteCount)
                {
                    case 1:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].boIsSigned)
                            {
                                byte u8Data = Convert.ToByte(sCellDataRaw);
                                au8Data = new byte[1];
                                au8Data[0] = u8Data;
                                tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].u32Address,
                                    u8Data);
                            }
                            else
                            {
                                sbyte s8Data = Convert.ToSByte(sCellDataRaw);
                                au8Data = new byte[1];
                                au8Data[0] = Convert.ToByte(sCellDataRaw);
                                tclsDataPage.s8SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].u32Address,
                                    s8Data);
                            }
                            break;
                        }
                    case 2:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].boIsSigned)
                            {
                                UInt16 u16InputData = Convert.ToUInt16(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(u16InputData);
                                tclsDataPage.u16SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].u32Address,
                                    u16InputData);
                            }
                            else
                            {
                                Int16 s16InputData = Convert.ToInt16(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(s16InputData);
                                tclsDataPage.s16SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].u32Address,
                                    s16InputData);
                            }
                            break;
                        }
                    case 4:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].boIsSigned)
                            {
                                UInt32 u32InputData = Convert.ToUInt32(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(u32InputData);
                                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].u32Address,
                                    u32InputData);
                            }
                            else
                            {
                                Int32 s32InputData = Convert.ToInt32(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(s32InputData);
                                tclsDataPage.s32SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].u32Address,
                                    s32InputData);
                            }
                            break;
                        }
                    default:
                        {
                            byte u8InputData = Convert.ToByte(sCellDataRaw);
                            au8Data = BitConverter.GetBytes(u8InputData);
                        }
                        break;
                }

                //Program.mAPP_clsUDPWLAN.mclsUDS.vStartRPC(0x3d,
                //                0x00,
                //                tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].u32Address,
                //                au8Data.Length,
                //                au8Data);

                UInt32[] au32Data = new UInt32[1];

                switch (au8Data.Length)
                {
                    case 4:
                        {
                            au32Data[0] = 0;
                            au32Data[0] = (UInt32)(au8Data[3] * 0x1000000);
                            au32Data[0] += (UInt32)(au8Data[2] * 0x10000);
                            au32Data[0] += (UInt32)(au8Data[1] * 0x100);
                            au32Data[0] += (UInt32)au8Data[0];
                            break;
                        }
                    case 2:
                        {
                            au32Data[0] = 0;
                            au32Data[0] += (UInt32)(au8Data[1] * 0x100);
                            au32Data[0] += (UInt32)au8Data[0];
                            break;
                        }
                    case 1:
                        {
                            au32Data[0] = (UInt32)au8Data[0];
                            break;
                        }
                    default:
                        {
                            au32Data[0] = 0;
                            break;
                        }
                }

                Program.vUpdateCalibration(tclsASAM.milstCharacteristicList[maiCharIndices[iCharacteristicIDX]].szCharacteristicName, au32Data);
            }
            catch
            {
                MessageBox.Show("Input data format or range error");
                //maclsCharacteristicTextBox[iCharacteristicIDX].ForeColor = 
            }

            vRefreshFromDataPage();
        }

        private void tclsBlobSettings_Resize(object sender, System.EventArgs e)
        {
            RedrawForm();
        }

        private void RedrawForm()
        {
            if (null != mclsDisplayTrigger)
            {
                mclsDisplayTrigger.Width = this.Width - 390;
                mclsDisplayTrigger.Height = this.Height - 190;
                mclsDisplayTrigger.Left = 10;
                mclsDisplayTrigger.Top = 25;

                for (int iCharacteristicIDX = 0; iCharacteristicIDX < maclsCharacteristicLabel.Length; iCharacteristicIDX++)
                {
                    maclsCharacteristicLabel[iCharacteristicIDX].Left = this.Width - 390;

                    if (false == mboEnumMode[iCharacteristicIDX])
                    {
                        maclsCharacteristicTextBox[iCharacteristicIDX].Left = this.Width - 230;

                        if (9 > iCharacteristicIDX)
                        {
                            maclsCharacteristicComboBox[iCharacteristicIDX].Left = this.Width + 230;
                        }
                    }
                    else
                    {
                        maclsCharacteristicTextBox[iCharacteristicIDX].Left = this.Width + 230;

                        if (9 > iCharacteristicIDX)
                        {
                            maclsCharacteristicComboBox[iCharacteristicIDX].Left = this.Width - 230;
                        }
                    }

                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].Left = this.Width - 150;
                    maclsCharacteristicButton[iCharacteristicIDX].Left = this.Width - 100;
                }

                if (0 != mclsDisplayTrigger.DataSources.Count)
                {
                    mclsDisplayTrigger.DataSources[0].SetDataSourceDrawnAll(false);
                    mclsDisplayTrigger.DataSources[1].SetDataSourceDrawnAll(false);
                    mclsDisplayTrigger.DataSources[2].SetDataSourceDrawnAll(false);
                    mclsDisplayTrigger.Refresh();
                }

                groupBoxArrayInput.Width = this.Width - 40;
                groupBoxMissingToothInput.Width = this.Width - 40;

                groupBoxArrayInput.Top = this.Height - 110;
                groupBoxMissingToothInput.Top = this.Height - 160;
            }
        }

        private void buttonPrimary_Click(object sender, EventArgs e)
        {
            UInt16[] au16Elements = ProcessTextInput();

            SetInputArray(au16Elements, "Primary Trigger Table");
        }

        private void SetInputArray(UInt16[] au16Elements, String TableString)
        {
            if (au16Elements.Length <= 80)
            {
                Array.Resize(ref au16Elements, 80);
                    
                int iPrimaryTriggerIDX = tclsASAM.iGetBlobIndex(TableString);

                if (-1 != iPrimaryTriggerIDX)
                {
                    int iPrimaryTriggerTablePointCount = tclsASAM.milstBlobList[iPrimaryTriggerIDX].iPointCount;

                    if (tenRecLayout.enRL_VALU16 == tclsASAM.milstBlobList[iPrimaryTriggerIDX].enRecLayout)
                    {
                        UInt32 u32Address = tclsASAM.milstBlobList[iPrimaryTriggerIDX].u32Address;
                        tclsDataPage.au16SetWorkingData(u32Address, ref au16Elements);
                    }
                }

                vRefreshFromDataPage();
            }
        }

        private UInt16[] ProcessTextInput()
        {
            string szInput = textBoxPatternInput.Text;
            UInt16[] aiElements = null;
            UInt16[] aiElementsScaled = null;
            int iElementIDX = 0;
            string[] aszInput;

            szInput = szInput.Replace(" ", string.Empty);

            try
            {
                aszInput = szInput.Split(',');
                aiElements = new UInt16[aszInput.Length];

                foreach (string szElement in aszInput )
                {
                    aiElements[iElementIDX] = Convert.ToUInt16(szElement);
                    iElementIDX++;
                }
            }
            catch
            {

            }

            if (0x8000 > aiElements[aiElements.Length - 1])
            {
                iElementIDX = 0;
                aiElementsScaled = new UInt16[aiElements.Length];

                foreach (int element in aiElements)
                {
                    float val = (element * 65536) / 360;
                    aiElementsScaled[iElementIDX] =  (UInt16)val;
                    iElementIDX++;
                }

                aiElements = aiElementsScaled;
            }

            return aiElements;
        }

        private void buttonSecondary_Click(object sender, EventArgs e)
        {
            UInt16[] au16Elements = ProcessTextInput();

            SetInputArray(au16Elements, "Secondary Trigger Table");
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            UInt16[] au16Elements = ProcessTextInput();

            SetInputArray(au16Elements, "Sync Points Table");
        }

        private void loadFromLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string szSender = sender.ToString();

            foreach (string szTrigger in maszTriggers)
            {
                if (0 == String.Compare(szTrigger, szSender))
                {
                    tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Triggers\\MDAC ECUHOST Triggers.INI");
                    string szTriggerPrimary = null;
                    string szTriggerSecondary = null;
                    string szTriggerSyncPoints = null;
                    string szTriggerPrimaryEdge = null;
                    string szTriggerSecondaryEdge = null;
                    string szTriggerPrimaryVR = null;
                    string szTriggerSecondaryVR = null;
                    string szTriggerPrimaryFirstRising = null;
                    string szTriggerSecondaryFirstRising = null;
                    string szTriggerBaseTimingOffset = null;
                    string szTriggerCustomIndex = null;

                    try
                    {
                        szTriggerPrimary = mclsIniParser.GetSetting("TriggerData", szTrigger + "TRIGGERPRIMARY");
                    }
                    catch
                    {
                        szTriggerPrimary = null;
                    }

                    try
                    {
                        szTriggerSecondary = mclsIniParser.GetSetting("TriggerData", szTrigger + "TRIGGERSECONDARY");
                    }

                    catch
                    {
                        szTriggerSecondary = null;
                    }

                    try
                    {
                        szTriggerSyncPoints = mclsIniParser.GetSetting("TriggerData", szTrigger + "SYNCPOINTSTABLE");
                    }
                    catch
                    {
                        szTriggerSyncPoints = null;
                    }

                    try
                    {
                        szTriggerPrimaryEdge = mclsIniParser.GetSetting("TriggerData", szTrigger + "PRIMARYEDGESETUP");
                    }
                    catch
                    {
                        szTriggerPrimaryEdge = null;
                    }

                    try
                    {
                        szTriggerSecondaryEdge = mclsIniParser.GetSetting("TriggerData", szTrigger + "SECONDARYEDGESETUP");
                    }
                    catch
                    {
                        szTriggerSecondaryEdge = null;
                    }
                    try
                    {
                        szTriggerPrimaryVR = mclsIniParser.GetSetting("TriggerData", szTrigger + "PRIMARYVRENABLE");
                    }
                    catch
                    {
                        szTriggerPrimaryVR = null;
                    }

                    try
                    {
                        szTriggerSecondaryVR = mclsIniParser.GetSetting("TriggerData", szTrigger + "SECONDARYVRENABLE");
                    }
                    catch
                    {
                        szTriggerSecondaryVR = null;
                    }

                    try
                    {
                        szTriggerPrimaryFirstRising = mclsIniParser.GetSetting("TriggerData", szTrigger + "FIRSTEDGERISINGPRIMARY");
                    }
                    catch
                    {
                        szTriggerPrimaryFirstRising = null;
                    }

                    try
                    {
                        szTriggerSecondaryFirstRising = mclsIniParser.GetSetting("TriggerData", szTrigger + "FIRSTEDGERISINGSECONDARY");
                    }
                    catch
                    {
                        szTriggerSecondaryFirstRising = null;
                    }

                    try
                    {
                        szTriggerBaseTimingOffset = mclsIniParser.GetSetting("TriggerData", szTrigger + "BASETIMINGOFFSET");
                    }
                    catch
                    {
                        szTriggerBaseTimingOffset = null;
                    }

                    try
                    {
                        szTriggerCustomIndex = mclsIniParser.GetSetting("TriggerData", szTrigger + "CUSTOMINDEX");
                    }
                    catch
                    {
                        szTriggerCustomIndex = null;
                    }



                    if ((szTriggerPrimary != null) &&
                    (szTriggerSecondary != null) &&
                    (szTriggerSyncPoints != null) &&
                    (szTriggerPrimaryEdge != null) &&
                    (szTriggerSecondaryEdge != null) &&
                    (szTriggerPrimaryVR != null) &&
                    (szTriggerSecondaryVR != null) &&
                    (szTriggerPrimaryFirstRising != null) &&
                    (szTriggerSecondaryFirstRising != null) &&
                    (szTriggerCustomIndex != null) &&
                    (szTriggerBaseTimingOffset != null))
                    {
                        string[] aszPrimaryTriggerArray = null;
                        UInt16[] au16PrimaryTriggerData;

                        string[] aszSecondaryTriggerArray = null;
                        UInt16[] au16SecondaryTriggerData;

                        string[] aszSyncPointsData = null;
                        UInt16[] au16SyncPointsData;

                        byte u8FirstEdgeRisingPrimary = 0;
                        byte u8EdgePolarityPrimary = 0;
                        byte u8FirstEdgeRisingSecondary = 0;
                        byte u8EdgePolaritySecondary = 0;
                        byte u8VREnablePrimary = 0;
                        byte u8VREnableSecondary = 0;
                        byte u8CustomIndex = 0;
                        UInt16 u16BaseTimingOffset = 0;

                        int iArrayIDX;

                        szTriggerPrimary = szTriggerPrimary.Replace('{', ' ');
                        szTriggerPrimary = szTriggerPrimary.Replace('}', ' ');
                        szTriggerPrimary = szTriggerPrimary.Trim();

                        szTriggerSecondary = szTriggerSecondary.Replace('{', ' ');
                        szTriggerSecondary = szTriggerSecondary.Replace('}', ' ');
                        szTriggerSecondary = szTriggerSecondary.Trim();

                        szTriggerSyncPoints = szTriggerSyncPoints.Replace('{', ' ');
                        szTriggerSyncPoints = szTriggerSyncPoints.Replace('}', ' ');
                        szTriggerSyncPoints = szTriggerSyncPoints.Trim();

                        aszPrimaryTriggerArray = szTriggerPrimary.Split(',');
                        aszSecondaryTriggerArray = szTriggerSecondary.Split(',');
                        aszSyncPointsData = szTriggerSyncPoints.Split(',');

                        au16PrimaryTriggerData = new UInt16[80];
                        au16SecondaryTriggerData = new UInt16[40];
                        au16SyncPointsData = new UInt16[40];

                        /* Insert the primary trigger pattern into the data page */
                        iArrayIDX = 0;

                        foreach (string szTriggerDelay in aszPrimaryTriggerArray)
                        {
                            if (szTriggerDelay.Contains('x') | szTriggerDelay.Contains('X'))
                            {
                                string szNoX = szTriggerDelay.Replace("0x", "");
                                szNoX = szNoX.Replace("0X", "");

                                try
                                {
                                    au16PrimaryTriggerData[iArrayIDX] = (UInt16)int.Parse(szNoX, NumberStyles.HexNumber);
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                au16PrimaryTriggerData[iArrayIDX] = Convert.ToUInt16(szTriggerDelay);
                            }

                            if (iArrayIDX < 80)
                            {
                                iArrayIDX++;
                            }
                            else
                            {
                                /* Break too many values found in library string */
                                break;
                            }
                        }

                        int iPrimaryTriggerIDX = tclsASAM.iGetBlobIndex("Primary Trigger Table");

                        if (-1 != iPrimaryTriggerIDX)
                        {
                            int iPrimaryTriggerTablePointCount = tclsASAM.milstBlobList[iPrimaryTriggerIDX].iPointCount;

                            if (tenRecLayout.enRL_VALU16 == tclsASAM.milstBlobList[iPrimaryTriggerIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstBlobList[iPrimaryTriggerIDX].u32Address;
                                tclsDataPage.au16SetWorkingData(u32Address, ref au16PrimaryTriggerData);
                            }
                        }


                        /* Insert the secondary trigger pattern into the data page */
                        iArrayIDX = 0;

                        foreach (string szTriggerDelay in aszSecondaryTriggerArray)
                        {
                            if (szTriggerDelay.Contains('x') | szTriggerDelay.Contains('X'))
                            {
                                string szNoX = szTriggerDelay.Replace("0x", "");
                                szNoX = szNoX.Replace("0X", "");
                                au16SecondaryTriggerData[iArrayIDX] = (UInt16)int.Parse(szNoX, NumberStyles.HexNumber);
                            }
                            else
                            {
                                au16SecondaryTriggerData[iArrayIDX] = Convert.ToUInt16(szTriggerDelay);
                            }

                            if (iArrayIDX < 40)
                            {
                                iArrayIDX++;
                            }
                            else
                            {
                                /* Break too many values found in library string */
                                break;
                            }
                        }

                        int iSecondaryTriggerIDX = tclsASAM.iGetBlobIndex("Secondary Trigger Table");

                        if (-1 != iSecondaryTriggerIDX)
                        {
                            int iSecondaryTriggerTablePointCount = tclsASAM.milstBlobList[iSecondaryTriggerIDX].iPointCount;

                            if (tenRecLayout.enRL_VALU16 == tclsASAM.milstBlobList[iSecondaryTriggerIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstBlobList[iSecondaryTriggerIDX].u32Address;
                                tclsDataPage.au16SetWorkingData(u32Address, ref au16SecondaryTriggerData);
                            }
                        }

                        /* Insert the sync points pattern into the data page */
                        iArrayIDX = 0;

                        foreach (string szTriggerDelay in aszSyncPointsData)
                        {
                            if (szTriggerDelay.Contains('x') | szTriggerDelay.Contains('X'))
                            {
                                string szNoX = szTriggerDelay.Replace("0x", "");
                                szNoX = szNoX.Replace("0X", "");
                                au16SyncPointsData[iArrayIDX] = (UInt16)int.Parse(szNoX, NumberStyles.HexNumber);
                            }
                            else
                            {
                                au16SyncPointsData[iArrayIDX] = Convert.ToUInt16(szTriggerDelay);
                            }

                            if (iArrayIDX < 40)
                            {
                                iArrayIDX++;
                            }
                            else
                            {
                                /* Break too many values found in library string */
                                break;
                            }
                        }

                        int iSyncPointsIDX = tclsASAM.iGetBlobIndex("Sync Points Table");

                        if (-1 != iSyncPointsIDX)
                        {
                            int iSyncPointsTablePointCount = tclsASAM.milstBlobList[iSyncPointsIDX].iPointCount;

                            if (tenRecLayout.enRL_VALU16 == tclsASAM.milstBlobList[iSyncPointsIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstBlobList[iSyncPointsIDX].u32Address;
                                tclsDataPage.au16SetWorkingData(u32Address, ref au16SyncPointsData);
                            }
                        }

                        /* Insert the primary edge polarity into the data page */
                        szTriggerPrimaryEdge = szTriggerPrimaryEdge.Replace("{", "");
                        szTriggerPrimaryEdge = szTriggerPrimaryEdge.Replace("}", "");

                        if (szTriggerPrimaryEdge.Contains('x') | szTriggerPrimaryEdge.Contains('X'))
                        {
                            string szNoX = szTriggerPrimaryEdge.Replace("0x", "");
                            szNoX = szNoX.Replace("0X", "");
                            u8EdgePolarityPrimary = (byte)int.Parse(szNoX, NumberStyles.HexNumber);
                        }
                        else
                        {
                            u8EdgePolarityPrimary = Convert.ToByte(szTriggerPrimaryEdge);
                        }

                        int iTriggerEdgeIDX = tclsASAM.iGetCharIndex("Primary Edge Setup");

                        if (-1 != iTriggerEdgeIDX)
                        {
                            if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iTriggerEdgeIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstCharacteristicList[iTriggerEdgeIDX].u32Address;
                                tclsDataPage.u8SetWorkingData(u32Address, u8EdgePolarityPrimary);
                            }
                        }

                        /* Insert the secondary edge polarity into the data page */
                        szTriggerSecondaryEdge = szTriggerSecondaryEdge.Replace("{", "");
                        szTriggerSecondaryEdge = szTriggerSecondaryEdge.Replace("}", "");

                        if (szTriggerSecondaryEdge.Contains('x') | szTriggerSecondaryEdge.Contains('X'))
                        {
                            string szNoX = szTriggerSecondaryEdge.Replace("0x", "");
                            szNoX = szNoX.Replace("0X", "");
                            u8EdgePolaritySecondary = (byte)int.Parse(szNoX, NumberStyles.HexNumber);
                        }
                        else
                        {
                            u8EdgePolaritySecondary = Convert.ToByte(szTriggerSecondaryEdge);
                        }

                        int iSecondaryEdgeIDX = tclsASAM.iGetCharIndex("Secondary Edge Setup");

                        if (-1 != iSecondaryEdgeIDX)
                        {
                            if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iSecondaryEdgeIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstCharacteristicList[iSecondaryEdgeIDX].u32Address;
                                tclsDataPage.u8SetWorkingData(u32Address, u8EdgePolaritySecondary);
                            }
                        }

                        /* Insert the primary VR enable into the data page */
                        szTriggerPrimaryVR = szTriggerPrimaryVR.Replace("{", "");
                        szTriggerPrimaryVR = szTriggerPrimaryVR.Replace("}", "");

                        if (szTriggerPrimaryVR.Contains('x') | szTriggerPrimaryVR.Contains('X'))
                        {
                            string szNoX = szTriggerPrimaryVR.Replace("0x", "");
                            szNoX = szNoX.Replace("0X", "");
                            u8VREnablePrimary = (byte)int.Parse(szNoX, NumberStyles.HexNumber);
                        }
                        else
                        {
                            u8VREnablePrimary = Convert.ToByte(szTriggerPrimaryVR);
                        }

                        int iPrimaryVREnableIDX = tclsASAM.iGetCharIndex("Primary VR Enable");

                        if (-1 != iPrimaryVREnableIDX)
                        {
                            if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iPrimaryVREnableIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstCharacteristicList[iPrimaryVREnableIDX].u32Address;
                                tclsDataPage.u8SetWorkingData(u32Address, u8VREnablePrimary);
                            }
                        }

                        /* Insert the secondary VR enable into the data page */
                        szTriggerSecondaryVR = szTriggerSecondaryVR.Replace("{", "");
                        szTriggerSecondaryVR = szTriggerSecondaryVR.Replace("}", "");

                        if (szTriggerSecondaryVR.Contains('x') | szTriggerSecondaryVR.Contains('X'))
                        {
                            string szNoX = szTriggerSecondaryVR.Replace("0x", "");
                            szNoX = szNoX.Replace("0X", "");
                            u8VREnableSecondary = (byte)int.Parse(szNoX, NumberStyles.HexNumber);
                        }
                        else
                        {
                            u8VREnableSecondary = Convert.ToByte(szTriggerSecondaryVR);
                        }

                        int iSecondaryVREnableIDX = tclsASAM.iGetCharIndex("Secondary VR Enable");

                        if (-1 != iSecondaryVREnableIDX)
                        {
                            if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iSecondaryVREnableIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstCharacteristicList[iSecondaryVREnableIDX].u32Address;
                                tclsDataPage.u8SetWorkingData(u32Address, u8VREnableSecondary);
                            }
                        }


                        /* Insert the primary rising first into the data page */
                        szTriggerPrimaryFirstRising = szTriggerPrimaryFirstRising.Replace("{", "");
                        szTriggerPrimaryFirstRising = szTriggerPrimaryFirstRising.Replace("}", "");

                        if (szTriggerPrimaryFirstRising.Contains('x') | szTriggerPrimaryFirstRising.Contains('X'))
                        {
                            string szNoX = szTriggerPrimaryFirstRising.Replace("0x", "");
                            szNoX = szNoX.Replace("0X", "");
                            u8FirstEdgeRisingPrimary = (byte)int.Parse(szNoX, NumberStyles.HexNumber);
                        }
                        else
                        {
                            u8FirstEdgeRisingPrimary = Convert.ToByte(szTriggerPrimaryFirstRising);
                        }

                        int iPrimaryFirstRisingIDX = tclsASAM.iGetCharIndex("First Edge Rising Primary");

                        if (-1 != iPrimaryFirstRisingIDX)
                        {
                            if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iPrimaryFirstRisingIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstCharacteristicList[iPrimaryFirstRisingIDX].u32Address;
                                tclsDataPage.u8SetWorkingData(u32Address, u8FirstEdgeRisingPrimary);
                            }
                        }


                        /* Insert the secondary rising first into the data page */
                        szTriggerSecondaryFirstRising = szTriggerSecondaryFirstRising.Replace("{", "");
                        szTriggerSecondaryFirstRising = szTriggerSecondaryFirstRising.Replace("}", "");

                        if (szTriggerSecondaryFirstRising.Contains('x') | szTriggerSecondaryFirstRising.Contains('X'))
                        {
                            string szNoX = szTriggerSecondaryFirstRising.Replace("0x", "");
                            szNoX = szNoX.Replace("0X", "");
                            u8FirstEdgeRisingSecondary = (byte)int.Parse(szNoX, NumberStyles.HexNumber);
                        }
                        else
                        {
                            u8FirstEdgeRisingSecondary = Convert.ToByte(szTriggerSecondaryFirstRising);
                        }

                        int iSecondaryFirstRisingIDX = tclsASAM.iGetCharIndex("First Edge Rising Secondary");

                        if (-1 != iSecondaryFirstRisingIDX)
                        {
                            if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iSecondaryFirstRisingIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstCharacteristicList[iSecondaryFirstRisingIDX].u32Address;
                                tclsDataPage.u8SetWorkingData(u32Address, u8FirstEdgeRisingSecondary);
                            }
                        }

                        /* Insert the base timing offset into the data page */
                        szTriggerBaseTimingOffset = szTriggerBaseTimingOffset.Replace("{", "");
                        szTriggerBaseTimingOffset = szTriggerBaseTimingOffset.Replace("}", "");

                        if (szTriggerBaseTimingOffset.Contains('x') | szTriggerBaseTimingOffset.Contains('X'))
                        {
                            string szNoX = szTriggerBaseTimingOffset.Replace("0x", "");
                            szNoX = szNoX.Replace("0X", "");
                            u16BaseTimingOffset = (byte)int.Parse(szNoX, NumberStyles.HexNumber);
                        }
                        else
                        {
                            u16BaseTimingOffset = Convert.ToUInt16(szTriggerBaseTimingOffset);
                        }

                        int iBaseTimingOffsetIDX = tclsASAM.iGetCharIndex("Timing Main Offset");

                        if (-1 != iBaseTimingOffsetIDX)
                        {
                            if (tenRecLayout.enRL_VALU16 == tclsASAM.milstCharacteristicList[iBaseTimingOffsetIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstCharacteristicList[iBaseTimingOffsetIDX].u32Address;
                                tclsDataPage.u16SetWorkingData(u32Address, u16BaseTimingOffset);
                            }
                        }

                        /* Insert the custom index into the data page */
                        szTriggerCustomIndex = szTriggerCustomIndex.Replace("{", "");
                        szTriggerCustomIndex = szTriggerCustomIndex.Replace("}", "");

                        if (szTriggerCustomIndex.Contains('x') | szTriggerCustomIndex.Contains('X'))
                        {
                            string szNoX = szTriggerCustomIndex.Replace("0x", "");
                            szNoX = szNoX.Replace("0X", "");
                            u8CustomIndex = (byte)int.Parse(szNoX, NumberStyles.HexNumber);
                        }
                        else
                        {
                            u8CustomIndex = Convert.ToByte(szTriggerSecondaryFirstRising);
                        }

                        int iCustomIndexIDX = tclsASAM.iGetCharIndex("Trigger Type");

                        if (-1 != iCustomIndexIDX)
                        {
                            if (tenRecLayout.enRL_VALU8 == tclsASAM.milstCharacteristicList[iCustomIndexIDX].enRecLayout)
                            {
                                UInt32 u32Address = tclsASAM.milstCharacteristicList[iCustomIndexIDX].u32Address;
                                tclsDataPage.u8SetWorkingData(u32Address, u8CustomIndex);
                            }
                        }


                        /* Draw the panel with these parameters */
                        InitialiseTriggerPanel(au16PrimaryTriggerData, au16SecondaryTriggerData, au16SyncPointsData, u8FirstEdgeRisingPrimary, u8EdgePolarityPrimary, u8FirstEdgeRisingSecondary, u8EdgePolaritySecondary, u8VREnablePrimary, u8VREnableSecondary);

                        /* Update the text boxes */
                        RefreshTextBoxesFromDataPage();
                    }
                }
            }
        }

        private void triggerWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadToothWizardMenu()
        { 
            int itoothCount;

            for  (itoothCount = 0; itoothCount < 60; itoothCount++)
            {
                string szToothCount = "Teeth incl missing = " + Convert.ToString(itoothCount + 1);
                comboBoxTeethTotal.Items.Add(szToothCount);
            }

            comboBoxMissing1.Items.Add("None");
            comboBoxMissing2.Items.Add("None");
            comboBoxMissing3.Items.Add("None");
            comboBoxMissing4.Items.Add("None");

            for (itoothCount = 0; itoothCount < 60; itoothCount++)
            {
                string szToothCount = "Missing Tooth: " + Convert.ToString(itoothCount + 1);
                comboBoxMissing1.Items.Add(szToothCount);
                comboBoxMissing2.Items.Add(szToothCount);
                comboBoxMissing3.Items.Add(szToothCount);
                comboBoxMissing4.Items.Add(szToothCount);
            }

            comboBoxMissing1.SelectedIndex = 0;
            comboBoxMissing2.SelectedIndex = 0;
            comboBoxMissing3.SelectedIndex = 0;
            comboBoxMissing4.SelectedIndex = 0;
        }

        private void loadTriggerWizardMenu()
        {
            ToolStripMenuItem[] menuItems = null;
            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Triggers\\MDAC ECUHOST Triggers.INI");
            string szTriggerList = null;
            int iMenuItemIDX = 0;

            try
            {
                szTriggerList = mclsIniParser.GetSetting("TriggerList", "Triggers");
            }
            catch
            {
                szTriggerList = null;
            }


            if (null != szTriggerList)
            {
                szTriggerList = szTriggerList.Replace("{","");
                szTriggerList = szTriggerList.Replace("}", "");
                szTriggerList = szTriggerList.Replace(" ", "");

                maszTriggers = szTriggerList.Split(',');

                if (0 < maszTriggers.Length)
                {
                    menuItems = new ToolStripMenuItem[maszTriggers.Length];

                    foreach (string szTriggerString in maszTriggers)
                    {
                        menuItems[iMenuItemIDX] = new System.Windows.Forms.ToolStripMenuItem();
                        menuItems[iMenuItemIDX].Name = maszTriggers[iMenuItemIDX];
                        menuItems[iMenuItemIDX].Size = new System.Drawing.Size(170, 22);
                        menuItems[iMenuItemIDX].Text = maszTriggers[iMenuItemIDX];
                        menuItems[iMenuItemIDX].Click += new System.EventHandler(this.loadFromLibraryToolStripMenuItem_Click);
                        iMenuItemIDX++;
                    }
                }
            }

            if (null != menuItems)
            {
                this.toolStripDropDownButtonTrigger.DropDownItems.AddRange(menuItems);
            }
        }

        private void toolStripConfiguration_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void FocusActivated(object sender, EventArgs e)
        {
            RedrawForm();
        }

        private void configurationManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Manuals\\configuration.pdf");
            }
            catch
            {
                MessageBox.Show("The PDF file was not found or did not load");
            }
        }

        private void wizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iSyncPointsIDX = tclsASAM.iGetBlobIndex("Sync Points Table");
            UInt16[] au16SyncPointsData = null;
            tclsSequenceSetupWizard clsSequenceWizard = null;

            if (-1 != iSyncPointsIDX)
            {
                int iSyncPointsTablePointCount = tclsASAM.milstBlobList[iSyncPointsIDX].iPointCount;

                if (tenRecLayout.enRL_VALU16 == tclsASAM.milstBlobList[iSyncPointsIDX].enRecLayout)
                {
                    UInt32 u32Address = tclsASAM.milstBlobList[iSyncPointsIDX].u32Address;
                    au16SyncPointsData = new UInt16[iSyncPointsTablePointCount];
                    tclsDataPage.au16GetWorkingData(u32Address, ref au16SyncPointsData);
                }
            }

            int iSyncPointsCount = GetTriggerArrayLength(au16SyncPointsData);
            int iSyncRepeatsIDX = tclsASAM.iGetCharIndex("Sync Phase Repeats");
            int iCylCountIDX = tclsASAM.iGetCharIndex("Cyl Count");
            UInt32 u32SyncRepeats = 0;
            byte u8CylCount = 0;

            if (-1 != iSyncRepeatsIDX)
            {
                tclsDataPage.u32GetWorkingData(tclsASAM.milstCharacteristicList[iSyncRepeatsIDX].u32Address , ref u32SyncRepeats);
            }

            if (-1 != iCylCountIDX)
            {
                tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iCylCountIDX].u32Address, ref u8CylCount);
            }

            if ((-1 != iSyncPointsCount) && (-1 != iSyncRepeatsIDX) && (-1 != iCylCountIDX))
            {
                clsSequenceWizard = new tclsSequenceSetupWizard((int)u8CylCount, iSyncPointsCount, (int)u32SyncRepeats);
            }

            if (null != clsSequenceWizard)
            {
                clsSequenceWizard.ShowDialog();
            }
            else
            {
                MessageBox.Show("An error occurred opening the sequence wizard");
            }
        }

        private void wizardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tclsInputsSetupWizard clsInputs = null;
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
                byte u8CrankSensorType = 0;  tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iCrankTypeIDX].u32Address, ref u8CrankSensorType);
                byte u8CamSensorType = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iCamTypeIDX].u32Address, ref u8CamSensorType);
                byte u8CrankSensorStrength = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iTriggerPullStrengthIDX].u32Address, ref u8CrankSensorStrength);
                byte u8CamSensorStrength = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iCamPullStrengthIDX].u32Address, ref u8CamSensorStrength);
                byte u8CTSRes = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iCTSSourceIDX].u32Address, ref u8CTSRes);
                byte u8ATSRes = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iATSSourceIDX].u32Address, ref u8ATSRes);
                byte u8TPSRes = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iTPSSourceIDX].u32Address, ref u8TPSRes);
                byte u8MAPRes = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iMAPSourceIDX].u32Address, ref u8MAPRes);
                byte u8MAFRes = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iMAFSourceIDX].u32Address, ref u8MAFRes);
                byte u8TPSCAN = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iTPSCANIDX].u32Address, ref u8TPSCAN);
                byte u8MAPCAN = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iMAPCANIDX].u32Address, ref u8MAPCAN);
                byte u8ATSCAN = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iATSCANIDX].u32Address, ref u8ATSCAN);
                byte u8CTSCAN = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iCTSCANIDX].u32Address, ref u8CTSCAN);
                byte u8PPSCAN = 0; tclsDataPage.u8GetWorkingData(tclsASAM.milstCharacteristicList[iPPSCANIDX].u32Address, ref u8PPSCAN);


                clsInputs = new tclsInputsSetupWizard(u8CrankSensorType, u8CamSensorType, u8CrankSensorStrength, u8CamSensorStrength, u8CTSRes, u8ATSRes, u8TPSRes, u8MAPRes, u8MAFRes, u8TPSCAN, u8MAPCAN, u8ATSCAN, u8CTSCAN, u8PPSCAN);
                clsInputs.ShowDialog();
            }
        }

        public void vRequestShutdown()
        {
            mboRequestShutdown = true;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (true == mboRequestShutdown)
            {

            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("View minimised to the bottom", "Not possible to close view");
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UInt16[] au16PrimaryTriggerData;
            byte[] aiMissingIndex = new byte[4];
            int iToothCount = ~0;
            int iSearchIDX;

            aiMissingIndex[0] = 0xff;
            aiMissingIndex[1] = 0xff;
            aiMissingIndex[2] = 0xff;
            aiMissingIndex[3] = 0xff;

            if (0 < comboBoxMissing1.SelectedIndex)
            {
                for (iSearchIDX = 0; iSearchIDX < 4; iSearchIDX++)
                {
                    if (0xff == aiMissingIndex[iSearchIDX])
                    {
                        aiMissingIndex[iSearchIDX] = (byte)comboBoxMissing1.SelectedIndex;
                        break;
                    }
                }
            }

            if (0 < comboBoxMissing2.SelectedIndex)
            {
                for (iSearchIDX = 0; iSearchIDX < 4; iSearchIDX++)
                {
                    if (0xff == aiMissingIndex[iSearchIDX])
                    {
                        aiMissingIndex[iSearchIDX] = (byte)comboBoxMissing2.SelectedIndex;
                        break;
                    }
                }
            }

            if (0 < comboBoxMissing3.SelectedIndex)
            {
                for (iSearchIDX = 0; iSearchIDX < 4; iSearchIDX++)
                {
                    if (0xff == aiMissingIndex[iSearchIDX])
                    {
                        aiMissingIndex[iSearchIDX] = (byte)comboBoxMissing3.SelectedIndex;
                        break;
                    }
                }
            }

            if (0 < comboBoxMissing4.SelectedIndex)
            {
                for (iSearchIDX = 0; iSearchIDX < 4; iSearchIDX++)
                {
                    if (0xff == aiMissingIndex[iSearchIDX])
                    {
                        aiMissingIndex[iSearchIDX] = (byte)comboBoxMissing4.SelectedIndex;
                        break;
                    }
                }
            }

            if (-1 != comboBoxTeethTotal.SelectedIndex)
            {
                iToothCount = comboBoxTeethTotal.SelectedIndex + 1;
                Single fOffset;
                List<int> lstOffsets = new List<int>();
                bool boMissing = false;

                for (iSearchIDX = 0; iSearchIDX < iToothCount; iSearchIDX++)
                {
                    fOffset = (Single)iSearchIDX / (Single)iToothCount;
                    fOffset *= 65536f;

                    boMissing = false;

                    if ((iSearchIDX == aiMissingIndex[0]) ||
                        (iSearchIDX == aiMissingIndex[1]) ||
                        (iSearchIDX == aiMissingIndex[2]) ||
                        (iSearchIDX == aiMissingIndex[3]))
                    {
                        boMissing = true;
                    }

                    if (false == boMissing)
                    {
                       lstOffsets.Add((int)fOffset);
                    }
                }

                au16PrimaryTriggerData = new UInt16[80];

                iSearchIDX = 0;
                UInt32 u32Address = tclsASAM.milstBlobList[0].u32Address;

                foreach (int iOffset in lstOffsets)
                {
                    au16PrimaryTriggerData[iSearchIDX] = (ushort)iOffset;
                    iSearchIDX++;
                }

                tclsDataPage.au16SetWorkingData(u32Address, ref au16PrimaryTriggerData);
            }

            vRefreshFromDataPage();
        }
    }
}
