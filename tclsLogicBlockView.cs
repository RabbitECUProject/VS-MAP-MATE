/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Logic Block                                            */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsLogicBlockView.cs                                  */
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
    public partial class tclsLogicBlockView : Form
    {
        ComboBox[] maclsCharacteristicCombo;
        ComboBox[] maclsCharacteristicCompareCombo;
        Label[] maclsCharacteristicUnitsLabel;
        TextBox[] maclsOperandTextBox;
        ComboBox[] maclsChainOrOutputCombo;
        Button[] maclsCharacteristicButton;

        tclsWindowElement[] maclsWindowElement;

        int[] maiVarWordIndices;
        int[] maiVarIndices;
        int[] maiVarCompuMethodIndices;
        int[] maiOperandIndices;
        int[] maiChainOutputIndices;
        int[] maiRelaysOutputIDX;

        String[] maszVarCompuMethodFormat;
        List<int> mlstFormIndices;
        bool mboInitialising;
        bool mboRequestShutdown;
        int miFormIDX;

        public tclsLogicBlockView(int iFormIDX)
        {
            miFormIDX = iFormIDX;
            InitializeComponent();
            SetWindowView(true);

            mlstFormIndices = new List<int>();
            mlstFormIndices.Add(miFormIDX);
        }

        public void SetWindowView(bool boSetup)
        {
            int iCharacteristicIDX = 0;
            int iElementIDX;
            int iVerticalElementCount = 0;

            maclsWindowElement = new tclsWindowElement[Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].Count];

            Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].CopyTo(maclsWindowElement, 0);

            /* Count number of elements assigned to this window */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "characteristic"))
                {
                    iCharacteristicIDX++;
                }
            }


            iCharacteristicIDX /= 3;

            maclsCharacteristicCombo = new ComboBox[iCharacteristicIDX];
            maclsCharacteristicCompareCombo = new ComboBox[iCharacteristicIDX];
            maclsCharacteristicUnitsLabel = new Label[iCharacteristicIDX];
            maclsOperandTextBox = new TextBox[iCharacteristicIDX];
            maclsChainOrOutputCombo = new ComboBox[iCharacteristicIDX];
            maclsCharacteristicButton = new Button[iCharacteristicIDX];


            maclsCharacteristicUnitsLabel = new Label[iCharacteristicIDX];
            maclsCharacteristicButton = new Button[iCharacteristicIDX];
            maszVarCompuMethodFormat = new String[iCharacteristicIDX];

            maiVarWordIndices = new int[iCharacteristicIDX];
            maiVarIndices = new int[iCharacteristicIDX];
            maiVarCompuMethodIndices = new int[iCharacteristicIDX];
            maiOperandIndices = new int[iCharacteristicIDX];
            maiChainOutputIndices = new int[iCharacteristicIDX];

            iCharacteristicIDX = 0;

            /* Loop through and create visual controls required */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if ((0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "characteristic")) &&
                   (0 == String.Compare(maclsWindowElement[iElementIDX + 1].szElementType, "characteristic")) &&
                   (0 == String.Compare(maclsWindowElement[iElementIDX + 2].szElementType, "characteristic")))
                {
                    int iCharIDX = tclsASAM.iGetCharIndex(maclsWindowElement[iElementIDX].szA2LName);

                    maiVarWordIndices[iCharacteristicIDX] = iCharIDX;

                    maclsCharacteristicCombo[iCharacteristicIDX] = new ComboBox();
                    maclsCharacteristicCombo[iCharacteristicIDX].AutoSize = false;
                    maclsCharacteristicCombo[iCharacteristicIDX].Left = 20;
                    maclsCharacteristicCombo[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsCharacteristicCombo[iCharacteristicIDX].Width = 130;
                    maclsCharacteristicCombo[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicCombo[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsCharacteristicCombo[iCharacteristicIDX]);
                    maclsCharacteristicCombo[iCharacteristicIDX].SelectedIndexChanged += new System.EventHandler(CharacteristicComboVars_SelectedIndexChanged);

                    maclsCharacteristicCompareCombo[iCharacteristicIDX] = new ComboBox();
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].AutoSize = false;
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].Left = 150;
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].Width = 30;
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].Items.Add('>');
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].Items.Add('<');
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].Items.Add("==");
                    maclsCharacteristicCompareCombo[iCharacteristicIDX].Items.Add("!=");
                    this.Controls.Add(maclsCharacteristicCompareCombo[iCharacteristicIDX]);

                    maclsOperandTextBox[iCharacteristicIDX] = new TextBox();
                    maclsOperandTextBox[iCharacteristicIDX].Text = "0";
                    maclsOperandTextBox[iCharacteristicIDX].Left = 180;
                    maclsOperandTextBox[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsOperandTextBox[iCharacteristicIDX].Width = 70;
                    maclsOperandTextBox[iCharacteristicIDX].BackColor = Color.FromArgb(255, 255, 255, 200);
                    maclsOperandTextBox[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsOperandTextBox[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsOperandTextBox[iCharacteristicIDX]);


                    maclsCharacteristicUnitsLabel[iCharacteristicIDX] = new Label();
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].AutoSize = false;
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].TextAlign = ContentAlignment.MiddleLeft;
                    if (-1 < maiVarCompuMethodIndices[iCharacteristicIDX])
                    {
                        maclsCharacteristicUnitsLabel[iCharacteristicIDX].Text =
                            tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iCharacteristicIDX]].szUnitsString;
                    }
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].Left = 240;
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].Width = 30;
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsCharacteristicUnitsLabel[iCharacteristicIDX]);

                    maclsChainOrOutputCombo[iCharacteristicIDX] = new ComboBox();
                    maclsChainOrOutputCombo[iCharacteristicIDX].AutoSize = false;
                    maclsChainOrOutputCombo[iCharacteristicIDX].Left = 150;
                    maclsChainOrOutputCombo[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsChainOrOutputCombo[iCharacteristicIDX].Width = 30;
                    maclsChainOrOutputCombo[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsChainOrOutputCombo[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsChainOrOutputCombo[iCharacteristicIDX]);

                    maclsCharacteristicButton[iCharacteristicIDX] = new Button();
                    maclsCharacteristicButton[iCharacteristicIDX].Text = "Enter";
                    maclsCharacteristicButton[iCharacteristicIDX].Left = 280;
                    maclsCharacteristicButton[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsCharacteristicButton[iCharacteristicIDX].Click +=
                        this.EnterButtonClicked;
                    maclsCharacteristicButton[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicButton[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsCharacteristicButton[iCharacteristicIDX]);


                    iCharIDX = tclsASAM.iGetCharIndex(maclsWindowElement[iElementIDX + 1].szA2LName);
                    maiOperandIndices[iCharacteristicIDX] = iCharIDX;

                    iCharIDX = tclsASAM.iGetCharIndex(maclsWindowElement[iElementIDX + 2].szA2LName);
                    maiChainOutputIndices[iCharacteristicIDX] = iCharIDX;

                    PopulateVarCombo(iCharacteristicIDX, 0, true);
                    PopulateLinkCombo(iCharacteristicIDX, 0, true);


                    if (-1 != maiVarCompuMethodIndices[iCharacteristicIDX])
                    {
                        vCreateCharCompuMethodFormat(iCharacteristicIDX);
                    }
                    else
                    {
                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0,
                            "No compu-method found for " + maclsWindowElement[iElementIDX].szLabel);
                    }


                    iCharacteristicIDX++;
                    iElementIDX += 2;
                }

                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "window"))
                {
                    this.Text = ConstantData.APPCONFIG.szAppName + ":" + maclsWindowElement[iElementIDX].szLabel;
                }
            }


            iVerticalElementCount = iCharacteristicIDX;

            if (this.Height < 80 + 25 * iVerticalElementCount)
            {
                this.Height = 80 + 25 * iVerticalElementCount;
            }

            this.OnResize(EventArgs.Empty);
        }

        public void RequestShowViewIndex(int iFormReqIDX)
        {
            foreach (int iFormIDX in mlstFormIndices)
            {
                if (iFormIDX == iFormReqIDX)
                {
                    Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);
                    miFormIDX = iFormIDX;
                    Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);
                    break;
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
                        szFormattedData = String.Format(maszVarCompuMethodFormat[iElementIDX], sScaledData);
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

        private void vCreateMeasCompuMethodFormat(int iMeasureIDX)
        {
            String szFormatString = "{0:";


            {
                szFormatString += "0";
            }

            if (0 < tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iMeasureIDX]].iPostDPCount)
            {
                szFormatString += "." +
                new String('0',
                tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iMeasureIDX]].iPostDPCount) + "}";
            }
            else
            {
                szFormatString += "}";
            }

            maszVarCompuMethodFormat[iMeasureIDX] = szFormatString;
        }

        private void vCreateCharCompuMethodFormat(int iCharIDX)
        {
            String szFormatString = "{0:";

            {
                szFormatString += "0";
            }

            if (0 < tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iCharIDX]].iPostDPCount)
            {
                szFormatString += "." +
                new String('0',
                tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iCharIDX]].iPostDPCount) + "}";
            }
            else
            {
                szFormatString += "}";
            }

            maszVarCompuMethodFormat[iCharIDX] = szFormatString;
        }

        public void vSetInitialise(bool boInitialise)
        {
            mboInitialising = boInitialise;
        }

        public void vRefreshFromDataPage()
        {
            int iCharIDX;
            UInt32 u32Data;
            UInt32 u32Address;

            for (iCharIDX = 0; iCharIDX < maclsCharacteristicCombo.Length; iCharIDX++)
            {
                if (-1 < maiVarWordIndices[iCharIDX])
                {
                    u32Address = tclsASAM.milstCharacteristicList[maiVarWordIndices[iCharIDX]].u32Address;

                    switch (tclsASAM.milstCharacteristicList[maiVarWordIndices[iCharIDX]].enRecLayout)
                    {
                        case tenRecLayout.enRL_VALU8:
                            {
                                byte u8CharData = 0;
                                tclsDataPage.u8GetWorkingData(u32Address, ref u8CharData);
                                u32Data = (UInt32)u8CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALU16:
                            {
                                UInt16 u16CharData = 0;
                                tclsDataPage.u16GetWorkingData(u32Address, ref u16CharData);
                                u32Data = (UInt32)u16CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALU32:
                            {
                                UInt32 u32CharData = 0;
                                tclsDataPage.u32GetWorkingData(u32Address, ref u32CharData);
                                u32Data = (UInt32)u32CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS8:
                            {
                                sbyte s8CharData = 0;
                                tclsDataPage.s8GetWorkingData(u32Address, ref s8CharData);
                                u32Data = (UInt32)s8CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS16:
                            {
                                Int16 s16CharData = 0;
                                tclsDataPage.s16GetWorkingData(u32Address, ref s16CharData);
                                u32Data = (UInt32)s16CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS32:
                            {
                                Int32 s32CharData = 0;
                                tclsDataPage.s32GetWorkingData(u32Address, ref s32CharData);
                                u32Data = (UInt32)s32CharData;
                                break;
                            }
                        default:
                            {
                                u32Data = 0;
                                break;
                            }
                    }

                    PopulateVarCombo(iCharIDX, u32Data, false);
                }
            }

            for (iCharIDX = 0; iCharIDX < maclsCharacteristicCombo.Length; iCharIDX++)
            {
                if (-1 < maiOperandIndices[iCharIDX])
                {
                    u32Address = tclsASAM.milstCharacteristicList[maiOperandIndices[iCharIDX]].u32Address;

                    switch (tclsASAM.milstCharacteristicList[maiOperandIndices[iCharIDX]].enRecLayout)
                    {
                        case tenRecLayout.enRL_VALU8:
                            {
                                byte u8CharData = 0;
                                tclsDataPage.u8GetWorkingData(u32Address, ref u8CharData);
                                u32Data = (UInt32)u8CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALU16:
                            {
                                UInt16 u16CharData = 0;
                                tclsDataPage.u16GetWorkingData(u32Address, ref u16CharData);
                                u32Data = (UInt32)u16CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALU32:
                            {
                                UInt32 u32CharData = 0;
                                tclsDataPage.u32GetWorkingData(u32Address, ref u32CharData);
                                u32Data = (UInt32)u32CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS8:
                            {
                                sbyte s8CharData = 0;
                                tclsDataPage.s8GetWorkingData(u32Address, ref s8CharData);
                                u32Data = (UInt32)s8CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS16:
                            {
                                Int16 s16CharData = 0;
                                tclsDataPage.s16GetWorkingData(u32Address, ref s16CharData);
                                u32Data = (UInt32)s16CharData;
                                break;
                            }
                        case tenRecLayout.enRL_VALS32:
                            {
                                Int32 s32CharData = 0;
                                tclsDataPage.s32GetWorkingData(u32Address, ref s32CharData);
                                u32Data = (UInt32)s32CharData;
                                break;
                            }
                        default:
                            {
                                u32Data = 0;
                                break;
                            }
                    }

                    if (-1 < maiVarCompuMethodIndices[iCharIDX])
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            maclsOperandTextBox[iCharIDX].Text = szGetScaledData(maiVarCompuMethodIndices[iCharIDX], iCharIDX, u32Data, tenMCVElementType.enMCVChar);
                        });
                    }
                }
            }

            for (iCharIDX = 0; iCharIDX < maclsCharacteristicCombo.Length; iCharIDX++)
            {
                if (-1 < maiChainOutputIndices[iCharIDX])
                {
                    u32Address = tclsASAM.milstCharacteristicList[maiChainOutputIndices[iCharIDX]].u32Address;

                    switch (tclsASAM.milstCharacteristicList[maiChainOutputIndices[iCharIDX]].enRecLayout)
                    {
                        case tenRecLayout.enRL_VALU16:
                            {
                                UInt16 u16CharData = 0;
                                tclsDataPage.u16GetWorkingData(u32Address, ref u16CharData);
                                u32Data = (UInt32)u16CharData;
                                break;
                            }
                        default:
                            {
                                u32Data = 0;
                                break;
                            }
                    }

                    PopulateLinkCombo(iCharIDX, u32Data, false);
                }
            }
        }

        private void PopulateLinkCombo(int iCharIDX, UInt32 u32Data, bool reload)
        {
            if (true == reload)
            {
                int iOutputIDX;
                tclsIniParser clsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHOST Calibration.INI");

                maclsChainOrOutputCombo[iCharIDX].Items.Add("No output");
                maclsChainOrOutputCombo[iCharIDX].Items.Add("AND NEXT");
                maclsChainOrOutputCombo[iCharIDX].Items.Add("OR NEXT");
                maclsChainOrOutputCombo[iCharIDX].Items.Add("NOT NEXT");
                maclsChainOrOutputCombo[iCharIDX].Items.Add("XOR NEXT");
                iOutputIDX = 5;

                int iRelaysListIDX = tclsASAM.iGetCharIndex("DUMMY RELAY");
                String szCompuMethod = tclsASAM.milstCharacteristicList[iRelaysListIDX].szCompuMethod;
                int iRelayCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szCompuMethod);
                int iVerbTableIDX = tclsASAM.milstCompuMethodList[iRelayCompuMethodIDX].iVerbTableIDX;
                int iVerbCount = tclsASAM.milstVarVerbList[iVerbTableIDX].lstVarVerb.Count;
                maiRelaysOutputIDX = new int[2 * iVerbCount + 5];
                maiRelaysOutputIDX[0] = 0;
                maiRelaysOutputIDX[1] = 1;
                maiRelaysOutputIDX[2] = 2;
                maiRelaysOutputIDX[3] = 3;
                maiRelaysOutputIDX[4] = 4;

                if (0 < iVerbCount)
                {
                    foreach (tstVerbRecord stVerbRecord in tclsASAM.milstVarVerbList[iVerbTableIDX].lstVarVerb)
                    {
                        maclsChainOrOutputCombo[iCharIDX].Items.Add(stVerbRecord.szVerb + " ON");
                        maiRelaysOutputIDX[iOutputIDX] = 2 * stVerbRecord.iValueLow + 5;
                        iOutputIDX++;
                        maclsChainOrOutputCombo[iCharIDX].Items.Add(stVerbRecord.szVerb + " OFF");
                        maiRelaysOutputIDX[iOutputIDX] = 2 * stVerbRecord.iValueLow + 6;
                        iOutputIDX++;
                    }
                }

                /*
                for (iOutputIDX = 0; iOutputIDX < 99; iOutputIDX++)
                {
                    szOutputString = "Output " + iOutputIDX.ToString();
                    szOutputStringAlias = clsIniParser.GetSetting("InputOutput", szOutputString);

                    if (null != szOutputStringAlias)
                    {
                        szOutputString = szOutputStringAlias + " On";
                    }
                    else
                    {
                        szOutputString += " On";
                    }

                    maclsChainOrOutputCombo[iCharIDX].Items.Add(szOutputString);

                    szOutputString = "Output " + iOutputIDX.ToString();
                    szOutputStringAlias = clsIniParser.GetSetting("InputOutput", szOutputString);

                    if (null != szOutputStringAlias)
                    {
                        szOutputString = szOutputStringAlias + " Off";
                    }
                    else
                    {
                        szOutputString += " Off";
                    }

                    maclsChainOrOutputCombo[iCharIDX].Items.Add(szOutputString);
                }
                */
            }
            else
            {
                UInt32 u32ComboIDX = 0;

                foreach (int iValue in maiRelaysOutputIDX)
                {
                    if (u32Data == (UInt32)iValue)
                    {
                        u32Data = u32ComboIDX;
                        break;
                    }

                    u32ComboIDX++;
                }

                if (maclsChainOrOutputCombo[iCharIDX].Items.Count > u32ComboIDX)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        maclsChainOrOutputCombo[iCharIDX].SelectedIndex = (int)u32ComboIDX;
                    });
                }
            }
        }


        private void EnterButtonClicked(object sender, EventArgs e)
        {
            int iControlIDX;
            int iCharacteristicIDX;
            byte[] au8Data;
            Single sInputData;
            Single sCellDataRaw;

            iControlIDX = Array.IndexOf(maclsCharacteristicButton, sender);
            iCharacteristicIDX = maiOperandIndices[iControlIDX];

            try
            {
                sInputData = Convert.ToSingle(maclsOperandTextBox[iControlIDX].Text);
                sInputData = tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].sLowerLim > sInputData ?
                    tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].sLowerLim : sInputData;
                sInputData = tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].sUpperLim < sInputData ?
                    tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].sUpperLim : sInputData;

                maclsOperandTextBox[iControlIDX].Text = String.Format(maszVarCompuMethodFormat[iControlIDX], sInputData);

                switch (tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iControlIDX]].enCM_Type)
                {
                    case tenCM_Type.enLINEAR:
                        {
                            sCellDataRaw = sInputData - tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iControlIDX]].sCoeff2;
                            sCellDataRaw /= tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iControlIDX]].sCoeff1;
                            break;
                        }
                    default:
                        {
                            sCellDataRaw = sInputData;
                            break;
                        }
                }

                switch (tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].iByteCount)
                {
                    case 1:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].boIsSigned)
                            {
                                au8Data = new byte[1];
                                au8Data[0] = Convert.ToByte(sCellDataRaw);
                                tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].u32Address,
                                    au8Data[0]);
                            }
                            else
                            {
                                au8Data = new byte[1];
                                au8Data[0] = Convert.ToByte(sCellDataRaw);
                                tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].u32Address,
                                    au8Data[0]);
                            }

                            break;
                        }
                    case 2:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].boIsSigned)
                            {
                                UInt16 u16InputData = Convert.ToUInt16(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(u16InputData);
                                tclsDataPage.u16SetWorkingData(tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].u32Address,
                                    BitConverter.ToUInt16(au8Data, 0));
                            }
                            else
                            {
                                Int16 s16InputData = Convert.ToInt16(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(s16InputData);
                                tclsDataPage.u16SetWorkingData(tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].u32Address,
                                    BitConverter.ToUInt16(au8Data, 0));
                            }
                            break;
                        }
                    case 4:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].boIsSigned)
                            {
                                UInt32 u32InputData = Convert.ToUInt32(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(u32InputData);
                                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].u32Address,
                                    BitConverter.ToUInt32(au8Data, 0));
                            }
                            else
                            {
                                Int32 s32InputData = Convert.ToInt32(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(s32InputData);
                                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].u32Address,
                                    BitConverter.ToUInt32(au8Data, 0));
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


                UInt32[] au32VarData = new UInt32[1];

                switch (au8Data.Length)
                {
                    case 4:
                        {
                            au32VarData[0] = 0;
                            au32VarData[0] = (UInt32)(au8Data[3] * 0x1000000);
                            au32VarData[0] += (UInt32)(au8Data[2] * 0x10000);
                            au32VarData[0] += (UInt32)(au8Data[1] * 0x100);
                            au32VarData[0] += (UInt32)au8Data[0];
                            break;
                        }
                    case 2:
                        {
                            au32VarData[0] = 0;
                            au32VarData[0] += (UInt32)(au8Data[1] * 0x100);
                            au32VarData[0] += (UInt32)au8Data[0];
                            break;
                        }
                    case 1:
                        {
                            au32VarData[0] = (UInt32)au8Data[0];
                            break;
                        }
                    default:
                        {
                            au32VarData[0] = 0;
                            break;
                        }
                }

                Program.vUpdateCalibration(tclsASAM.milstCharacteristicList[maiOperandIndices[iControlIDX]].szCharacteristicName, au32VarData);
            }
            catch
            {
                MessageBox.Show("Input data format or range error");
            }

            UInt32 u32Address = tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].u32Address;
            UInt32 u32Masks = 0;

            int iByteCount = tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].iByteCount - 1;
            int iSigned = tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].boIsSigned == true ? 1 : 0;
            int iCompareType = maclsCharacteristicCompareCombo[iControlIDX].SelectedIndex;

            u32Masks = (UInt32)(iByteCount << 20);
            u32Masks += (UInt32)(iSigned << 22);
            u32Masks += (UInt32)(iCompareType << 23);

            au8Data = BitConverter.GetBytes((0xfffff & u32Address) + u32Masks);
            tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[maiVarWordIndices[iControlIDX]].u32Address,
                BitConverter.ToUInt32(au8Data, 0));

            UInt32[] au32Data = new UInt32[1];

            au32Data[0] = u32Address + u32Masks;
            Program.vUpdateCalibration(tclsASAM.milstCharacteristicList[maiVarWordIndices[iControlIDX]].szCharacteristicName, au32Data);

            UInt16 u16TempIDX = (UInt16)maclsChainOrOutputCombo[iControlIDX].SelectedIndex;
            UInt16 u16ChainOrOutput = (UInt16)maiRelaysOutputIDX[u16TempIDX];
            au8Data = BitConverter.GetBytes(u16ChainOrOutput);
            tclsDataPage.u16SetWorkingData(tclsASAM.milstCharacteristicList[maiChainOutputIndices[iControlIDX]].u32Address,
                BitConverter.ToUInt16(au8Data, 0));

            au32Data[0] = u16ChainOrOutput;
            Program.vUpdateCalibration(tclsASAM.milstCharacteristicList[maiChainOutputIndices[iControlIDX]].szCharacteristicName, au32Data);
        }

        private void tclsMeasCharView_Load(object sender, EventArgs e)
        {
            this.Left = 150;
        }

        private void tclsLogicView_Load(object sender, EventArgs e)
        {

        }

        private void PopulateVarCombo(int varIDX, UInt32 varData, bool reload_vars)
        {
            UInt32 u32VarAddress;
            int compareType = -1;
            bool boIsSigned;
            int dataSize = -1;
            int selectedIDX = -1;
            int comboIDX = 0;

            varData &= 0x3ffffff;
            u32VarAddress = 0x20000000 + (varData & 0xfffff);
            compareType = (Int32)(varData >> 23);
            boIsSigned = (0x400000 & varData) != 0 ? true : false;
            dataSize = (Int32)(varData & 0x300000) >> 20;

            if (true == reload_vars)
            {
                maclsCharacteristicCombo[varIDX].Items.Clear();
            }

            comboIDX = 0;

            foreach (tstMeasurement stMeasurement in tclsASAM.milstAvailableMeasList)
            {
                if (true == reload_vars)
                {
                    maclsCharacteristicCombo[varIDX].Items.Add(stMeasurement.szMeasurementName);
                }

                if (u32VarAddress == stMeasurement.u32Address)
                {
                    selectedIDX = comboIDX;
                    maiVarCompuMethodIndices[varIDX] = tclsASAM.iGetCompuMethodIndexFromMeas(stMeasurement.szMeasurementName, 0);
                    break;
                }

                comboIDX++;
            }


            if (-1 != selectedIDX)
            {
                Invoke((MethodInvoker)delegate
                {
                    maclsCharacteristicCombo[varIDX].SelectedIndex = maclsCharacteristicCombo[varIDX].FindStringExact(tclsASAM.milstAvailableMeasList[selectedIDX].szMeasurementName);
                });
            }

            if ((-1 != compareType) && (true == this.IsHandleCreated))
            {
                switch (compareType)
                {
                    case 0:
                        Invoke((MethodInvoker)delegate
                        {
                            maclsCharacteristicCompareCombo[varIDX].SelectedIndex = maclsCharacteristicCompareCombo[varIDX].FindStringExact(">");
                        });
                        break;
                    case 1:
                        Invoke((MethodInvoker)delegate
                        {
                            maclsCharacteristicCompareCombo[varIDX].SelectedIndex = maclsCharacteristicCompareCombo[varIDX].FindStringExact("<");
                        });
                        break;
                    case 2:
                        Invoke((MethodInvoker)delegate
                        {
                            maclsCharacteristicCompareCombo[varIDX].SelectedIndex = maclsCharacteristicCompareCombo[varIDX].FindStringExact("==");
                        });
                        break;
                    case 3:
                        Invoke((MethodInvoker)delegate
                        {
                            maclsCharacteristicCompareCombo[varIDX].SelectedIndex = maclsCharacteristicCompareCombo[varIDX].FindStringExact("!=");
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        private void CharacteristicComboVars_SelectedIndexChanged(object sender, EventArgs e)
        {
            UInt32 u32VarAddress = 0;
            int iControlIDX = Array.IndexOf(maclsCharacteristicCombo, sender);
            String szSelectedVar = maclsCharacteristicCombo[iControlIDX].SelectedItem.ToString();

            maiVarIndices[iControlIDX] = tclsASAM.iGetAvailableMeasIndex(szSelectedVar);

            if (-1 != maiVarIndices[iControlIDX])
            {
                u32VarAddress = tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].u32Address;
                String szCompuMethod = tclsASAM.milstAvailableMeasList[maiVarIndices[iControlIDX]].szCompuMethod;
                maiVarCompuMethodIndices[iControlIDX] = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szCompuMethod);

                if (-1 != maiVarCompuMethodIndices[iControlIDX])
                {                
                    maclsCharacteristicUnitsLabel[iControlIDX].Text = tclsASAM.milstCompuMethodList[maiVarCompuMethodIndices[iControlIDX]].szUnitsString;
                }                              
            }
        }

        private void tclsLogicView_Resize(object sender, System.EventArgs e)
        {
            int iIDX = 0;

            if (maclsCharacteristicCombo != null)
            {
                iIDX = 0;
                foreach (ComboBox combo in maclsCharacteristicCombo)
                {
                    combo.Left = this.Width / 50;
                    combo.Width = 35 * this.Width / 100;
                    combo.Height = (this.Height - 35) / (maclsCharacteristicUnitsLabel.Length + 1);
                    combo.Font = new Font(combo.Font.FontFamily, this.Width / 75);
                    combo.Top = this.Height / 12 + iIDX * ((this.Height - 20) / (maclsCharacteristicUnitsLabel.Length + 1));
                    iIDX++;
                }

                iIDX = 0;
                foreach (ComboBox combo in maclsCharacteristicCompareCombo)
                {
                    combo.Left = this.Width / 50 + 35 * this.Width / 100;
                    combo.Width = 10 * this.Width / 100;
                    combo.Height = (this.Height - 35) / (maclsCharacteristicUnitsLabel.Length + 1);
                    combo.Font = new Font(combo.Font.FontFamily, this.Width / 75);
                    combo.Top = this.Height / 12 + iIDX * ((this.Height - 20) / (maclsCharacteristicUnitsLabel.Length + 1));
                    iIDX++;
                }

                iIDX = 0;
                foreach (TextBox textBox in maclsOperandTextBox)
                {
                    textBox.Left = this.Width / 50 + 40 * this.Width / 100 + 5 * this.Width / 100;
                    textBox.Width = 12 * this.Width / 100;
                    textBox.Font = new Font(textBox.Font.FontFamily, this.Width / 45);
                    textBox.Height = (this.Height - 35) / (maclsCharacteristicUnitsLabel.Length + 1) - 5;
                    textBox.Top = this.Height / 12 + iIDX * ((this.Height - 20) / (maclsCharacteristicUnitsLabel.Length + 1));
                    textBox.TextAlign = HorizontalAlignment.Center;
                    iIDX++;
                }


                iIDX = 0;
                foreach (Label label in maclsCharacteristicUnitsLabel)
                {
                    label.Left = this.Width / 50 + 40 * this.Width / 100 + 5 * this.Width / 100 + 12 * this.Width / 100;
                    label.Width = 13 * this.Width / 100;
                    label.Font = new Font(label.Font.FontFamily, this.Width / 75);
                    label.Height = (this.Height - 35) / (maclsCharacteristicUnitsLabel.Length + 1) - 10;
                    label.Top = this.Height / 12 + iIDX * ((this.Height - 20) / (maclsCharacteristicUnitsLabel.Length + 1));
                    label.TextAlign = ContentAlignment.TopRight;
                    iIDX++;
                }
                iIDX = 0;
                foreach (ComboBox combo in maclsChainOrOutputCombo)
                {
                    combo.Left = this.Width / 50 + 40 * this.Width / 100 + 5 * this.Width / 100 + 20 * this.Width / 100 + 5 * this.Width / 100;
                    combo.Width = 15 * this.Width / 100;
                    combo.Height = (this.Height - 35) / (maclsCharacteristicUnitsLabel.Length + 1);
                    combo.Font = new Font(combo.Font.FontFamily, this.Width / 95);
                    combo.Top = this.Height / 12 + iIDX * ((this.Height - 20) / (maclsCharacteristicUnitsLabel.Length + 1));
                    iIDX++;
                }

                iIDX = 0;
                foreach (Button button in maclsCharacteristicButton)
                {
                    button.Left = this.Width / 50 + 40 * this.Width / 100 + 5 * this.Width / 100 + 20 * this.Width / 100 + 5 * this.Width / 100 + 15 * this.Width / 100;
                    button.Width = 10 * this.Width / 100;
                    button.Font = new Font(button.Font.FontFamily, this.Width / 65);
                    button.Height = (this.Height - 35) / (maclsCharacteristicUnitsLabel.Length + 1) - 15;
                    button.Top = this.Height / 12 + iIDX * ((this.Height - 20) / (maclsCharacteristicUnitsLabel.Length + 1));
                    iIDX++;
                }
            }
        }

        private void toolStripMenuItemIOPDF_Click(object sender, EventArgs e)
        {

        }

        private void iOHelpPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tclsPDFView clsHelpPDF = new UDP.tclsPDFView(AppDomain.CurrentDomain.BaseDirectory + "Manuals\\Rabbit ECU Due IO.pdf");

            try
            {
                clsHelpPDF.Show();
            }
            catch
            {
                MessageBox.Show("Starting PDF alternate viewer");
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
    }
}
