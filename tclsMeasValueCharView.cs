/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Measure and Characteristics View                       */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsMeasValueCharView.cs                               */
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

namespace UDP
{
    public enum tenMCVElementType
    {
        enMCVMeasure,
        enMCVChar
    }

    public partial class tclsMeasValueCharView : Form
    {
        Label[] maclsMeasureLabel;
        Label[] maclsMeasureUnitsLabel;
        TextBox[] maclsMeasureTextBox;
        Label[] maclsCharacteristicLabel;
        Label[] maclsCharacteristicUnitsLabel;
        TextBox[] maclsCharacteristicTextBox;
        Button[] maclsCharacteristicButton;
        tclsWindowElement[] maclsWindowElement;
        tstActiveMeasureIndices[,] maastActiveMeasureIndices;
        int[] maiCharIndices;
        int[] maiCharCompuMethodIndices;
        ComboBox[] maclsCharacteristicComboBox;
        int[,] maaiMeasCompuMethodIndices;
        String[,] maaszMeasCompuMethodFormat;
        String[] maszCharCompuMethodFormat;
        List<int> mlstFormIndices;
        bool mboInitialising;
        bool mboSuspendUpdates;
        bool mboRequestShutdown;
        int miFormIDX;
        bool[] mboCharEnumMode;
        bool[] mboMeasEnumMode;

        public tclsMeasValueCharView(int iFormIDX)
        {
            miFormIDX = iFormIDX;
            InitializeComponent();
            mlstFormIndices = new List<int>();
            mlstFormIndices.Add(miFormIDX);
            maastActiveMeasureIndices = new tstActiveMeasureIndices[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT, ConstantData.UDS.ru8UDS_MEAS_DDDI_MAX_ELEMENTS];
            maaszMeasCompuMethodFormat = new String[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT, ConstantData.UDS.ru8UDS_MEAS_DDDI_MAX_ELEMENTS];
            maaiMeasCompuMethodIndices = new int[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT, ConstantData.UDS.ru8UDS_MEAS_DDDI_MAX_ELEMENTS];

            int ixIDX, iyIDX;

            for (ixIDX = 0; ixIDX < maastActiveMeasureIndices.GetLength(0); ixIDX++)
            {
                for (iyIDX = 0; iyIDX < maastActiveMeasureIndices.GetLength(1); iyIDX++)
                {
                    maastActiveMeasureIndices[ixIDX, iyIDX].iMeasureQueue = -1;
                    maastActiveMeasureIndices[ixIDX, iyIDX].iMeasureQueueIDX = -1;
                }
            }

            SetWindowView(true);
        }

        public void SetWindowView(bool boSetup)
        { 
            int iMeasureIDX = 0;
            int iCharacteristicIDX = 0;
            int iElementIDX;
            int iMeasuresPresent = 0;
            int iCharacteristicsPresent = 0;
            int iVerticalElementCount = 0;
            tstActiveMeasureIndices stActiveMeasureIndices;

            mboSuspendUpdates = true;
            RemoveControls();

            maclsWindowElement = new tclsWindowElement[Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].Count];

            Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].CopyTo(maclsWindowElement, 0);

            /* Count number of elements assigned to this window */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "measure"))
                {
                    if (-1 != tclsASAM.iGetAvailableMeasIndex(maclsWindowElement[iElementIDX].szA2LName))
                    {
                        iMeasureIDX++;
                        iMeasuresPresent = 1;
                    }
                }
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "characteristic"))
                {
                    iCharacteristicIDX++;
                    iCharacteristicsPresent = 1;
                }
            }

            maclsMeasureTextBox = new TextBox[iMeasureIDX];
            maclsMeasureLabel = new Label[iMeasureIDX];
            maclsMeasureUnitsLabel = new Label[iMeasureIDX];
            mboMeasEnumMode = new bool[iMeasureIDX];

            maiCharCompuMethodIndices = new int[iCharacteristicIDX];
            maiCharIndices = new int[iCharacteristicIDX];

            maclsCharacteristicTextBox = new TextBox[iCharacteristicIDX];
            maclsCharacteristicLabel = new Label[iCharacteristicIDX];
            maclsCharacteristicUnitsLabel = new Label[iCharacteristicIDX];
            maclsCharacteristicButton = new Button[iCharacteristicIDX];
            maszCharCompuMethodFormat = new String[iCharacteristicIDX];
            maclsCharacteristicComboBox = new ComboBox[iCharacteristicIDX];
            mboCharEnumMode = new bool[iCharacteristicIDX];

            iMeasureIDX = 0;
            iCharacteristicIDX = 0;

            /* Loop through and create visual controls required */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "measure"))
                {
                    if (-1 != tclsASAM.iGetAvailableMeasIndex(maclsWindowElement[iElementIDX].szA2LName))
                    {
                        maclsMeasureLabel[iMeasureIDX] = new Label();
                        maclsMeasureLabel[iMeasureIDX].AutoSize = false;
                        maclsMeasureLabel[iMeasureIDX].TextAlign = ContentAlignment.MiddleRight;
                        maclsMeasureLabel[iMeasureIDX].Text = maclsWindowElement[iElementIDX].szLabel;
                        maclsMeasureLabel[iMeasureIDX].Left = 20;
                        maclsMeasureLabel[iMeasureIDX].Top = 40 + 25 * iMeasureIDX;
                        maclsMeasureLabel[iMeasureIDX].Width = 120;
                        maclsMeasureLabel[iMeasureIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                        maclsMeasureLabel[iMeasureIDX].ForeColor = Color.Aquamarine;
                        this.Controls.Add(maclsMeasureLabel[iMeasureIDX]);

                        maclsMeasureTextBox[iMeasureIDX] = new TextBox();
                        maclsMeasureTextBox[iMeasureIDX].Text = "0";
                        maclsMeasureTextBox[iMeasureIDX].Left = 150;
                        maclsMeasureTextBox[iMeasureIDX].Top = 40 + 25 * iMeasureIDX;
                        maclsMeasureTextBox[iMeasureIDX].Width = 70;
                        maclsMeasureTextBox[iMeasureIDX].BackColor = Color.FromArgb(255, 255, 255, 200);
                        maclsMeasureTextBox[iMeasureIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                        maclsMeasureTextBox[iMeasureIDX].ForeColor = Color.Aquamarine;
                        this.Controls.Add(maclsMeasureTextBox[iMeasureIDX]);

                        if (true == boSetup)
                        {
                            stActiveMeasureIndices = tclsASAM.stTryAddActiveMeas(maclsWindowElement[iElementIDX].szA2LName, miFormIDX);
                            maastActiveMeasureIndices[miFormIDX, iMeasureIDX].iMeasureQueue = stActiveMeasureIndices.iMeasureQueue;
                            maastActiveMeasureIndices[miFormIDX, iMeasureIDX].iMeasureQueueIDX = stActiveMeasureIndices.iMeasureQueueIDX;
                            maaiMeasCompuMethodIndices[miFormIDX, iMeasureIDX] = tclsASAM.iGetCompuMethodIndexFromMeas(maclsWindowElement[iElementIDX].szA2LName, miFormIDX);
                        }

                        if (tenCM_Type.enTAB_VERB == tclsASAM.milstCompuMethodList[maaiMeasCompuMethodIndices[miFormIDX, iMeasureIDX]].enCM_Type)
                        {
                            mboMeasEnumMode[iMeasureIDX] = true;
                        }
                        else
                        {
                            mboMeasEnumMode[iMeasureIDX] = false;
                        }

                        maclsMeasureUnitsLabel[iMeasureIDX] = new Label();
                        maclsMeasureUnitsLabel[iMeasureIDX].AutoSize = false;
                        maclsMeasureUnitsLabel[iMeasureIDX].TextAlign = ContentAlignment.MiddleLeft;
                        maclsMeasureUnitsLabel[iMeasureIDX].Text =
                            tclsASAM.milstCompuMethodList[maaiMeasCompuMethodIndices[miFormIDX, iMeasureIDX]].szUnitsString;
                        maclsMeasureUnitsLabel[iMeasureIDX].Left = 230;
                        maclsMeasureUnitsLabel[iMeasureIDX].Top = 40 + 25 * iMeasureIDX;
                        maclsMeasureUnitsLabel[iMeasureIDX].Width = 30;
                        maclsMeasureUnitsLabel[iMeasureIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                        maclsMeasureUnitsLabel[iMeasureIDX].ForeColor = Color.Aquamarine;
                        this.Controls.Add(maclsMeasureUnitsLabel[iMeasureIDX]);

                        if (true == boSetup)
                        {
                            vCreateMeasCompuMethodFormat(iMeasureIDX);
                        }

                        iMeasureIDX++;
                    }
                    else
                    {
                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, "Layout measure element " +
                            maclsWindowElement[iElementIDX].szA2LName + " not found in ASAM database, this element will not be shown");
                    }
                }

                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "characteristic"))
                {
                    int iCharIDX = tclsASAM.iGetCharIndex(maclsWindowElement[iElementIDX].szA2LName);

                    maiCharIndices[iCharacteristicIDX] = iCharIDX;

                    maclsCharacteristicLabel[iCharacteristicIDX] = new Label();
                    maclsCharacteristicLabel[iCharacteristicIDX].AutoSize = false;
                    maclsCharacteristicLabel[iCharacteristicIDX].TextAlign = ContentAlignment.MiddleRight;
                    maclsCharacteristicLabel[iCharacteristicIDX].Text =
                        maclsWindowElement[iElementIDX].szLabel;
                    maclsCharacteristicLabel[iCharacteristicIDX].Left = 280 * iMeasuresPresent + 20;
                    maclsCharacteristicLabel[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsCharacteristicLabel[iCharacteristicIDX].Width = 130;
                    maclsCharacteristicLabel[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicLabel[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsCharacteristicLabel[iCharacteristicIDX]);

                    maclsCharacteristicTextBox[iCharacteristicIDX] = new TextBox();
                    maclsCharacteristicTextBox[iCharacteristicIDX].Text = "0";
                    maclsCharacteristicTextBox[iCharacteristicIDX].Left = 280 * iMeasuresPresent + 160;
                    maclsCharacteristicTextBox[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsCharacteristicTextBox[iCharacteristicIDX].Width = 70;
                    maclsCharacteristicTextBox[iCharacteristicIDX].BackColor = Color.FromArgb(255, 255, 255, 200);
                    maclsCharacteristicTextBox[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicTextBox[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsCharacteristicTextBox[iCharacteristicIDX]);


                    maclsCharacteristicComboBox[iCharacteristicIDX] = new ComboBox();
                    maclsCharacteristicComboBox[iCharacteristicIDX].Top = 30 + 25 * iCharacteristicIDX;
                    maclsCharacteristicComboBox[iCharacteristicIDX].Width = 70;
                    maclsCharacteristicComboBox[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicComboBox[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    maclsCharacteristicComboBox[iCharacteristicIDX].SelectedIndexChanged +=
                        this.ComboBoxChanged;
                    this.Controls.Add(maclsCharacteristicComboBox[iCharacteristicIDX]);

                    maiCharCompuMethodIndices[iCharacteristicIDX] = tclsASAM.iGetCompuMethodIndexFromChar(maclsWindowElement[iElementIDX].szA2LName);

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

                        mboCharEnumMode[iCharacteristicIDX] = true;
                    }
                    else
                    {
                        mboCharEnumMode[iCharacteristicIDX] = false;
                    }


                    maclsCharacteristicUnitsLabel[iCharacteristicIDX] = new Label();
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].AutoSize = false;
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].TextAlign = ContentAlignment.MiddleLeft;
                    if (-1 < maiCharCompuMethodIndices[iCharacteristicIDX])
                    {
                        maclsCharacteristicUnitsLabel[iCharacteristicIDX].Text =
                            tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharacteristicIDX]].szUnitsString;
                    }
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].Left = 280 * iMeasuresPresent + 240;
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].Width = 30;
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicUnitsLabel[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsCharacteristicUnitsLabel[iCharacteristicIDX]);

                    maclsCharacteristicButton[iCharacteristicIDX] = new Button();
                    maclsCharacteristicButton[iCharacteristicIDX].Text = "Enter";
                    maclsCharacteristicButton[iCharacteristicIDX].Left = 280 * iMeasuresPresent + 280;
                    maclsCharacteristicButton[iCharacteristicIDX].Top = 40 + 25 * iCharacteristicIDX;
                    maclsCharacteristicButton[iCharacteristicIDX].Click +=
                        this.EnterButtonClicked;
                    maclsCharacteristicButton[iCharacteristicIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                    maclsCharacteristicButton[iCharacteristicIDX].ForeColor = Color.Aquamarine;
                    this.Controls.Add(maclsCharacteristicButton[iCharacteristicIDX]);

                    if (-1 != maiCharCompuMethodIndices[iCharacteristicIDX])
                    {
                        vCreateCharCompuMethodFormat(iCharacteristicIDX);
                    }
                    else
                    {
                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0,
                            "No compu-method found for " + maclsWindowElement[iElementIDX].szLabel);
                    }

                    iCharacteristicIDX++;
                }

                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "window"))
                {
                    this.Text = ConstantData.APPCONFIG.szAppName + ":" + maclsWindowElement[iElementIDX].szLabel;
                }
            }

            if (this.Width < 380 * iCharacteristicsPresent + 300 * iMeasuresPresent)
            {
                this.Width = 380 * iCharacteristicsPresent + 300 * iMeasuresPresent;
            }

            iVerticalElementCount = iCharacteristicIDX > iMeasureIDX ?
            iCharacteristicIDX : iMeasureIDX;

            if (this.Height < 80 + 25 * iVerticalElementCount)
            {
                this.Height = 80 + 25 * iVerticalElementCount;
            }

            if ((true == boSetup) && (0 < iMeasureIDX))
            {
                tclsASAM.vCalcDDDIAndSetRate(miFormIDX, 500);
            }
            else
            {
                this.OnResize(EventArgs.Empty);
                vRefreshFromDataPage();
            }

            mboSuspendUpdates = false;
        }

        private void RemoveControls()
        {
            if (null != maclsMeasureTextBox)
            {
                foreach (TextBox textBox in maclsMeasureTextBox)
                {
                    this.Controls.Remove(textBox);
                }
            }

            if (null != maclsMeasureUnitsLabel)
            {
                foreach (Label label in maclsMeasureUnitsLabel)
                {
                    this.Controls.Remove(label);
                }
            }

            if (null != maclsMeasureLabel)
            {
                foreach (Label label in maclsMeasureLabel)
                {
                    this.Controls.Remove(label);
                }
            }

            if (null != maclsCharacteristicTextBox)
            {
                foreach (TextBox textBox in maclsCharacteristicTextBox)
                {
                    this.Controls.Remove(textBox);
                }
            }

            if (null != maclsCharacteristicUnitsLabel)
            {
                foreach (Label label in maclsCharacteristicUnitsLabel)
                {
                    this.Controls.Remove(label);
                }
            }

            if (null != maclsCharacteristicButton)
            {
                foreach(Button button in maclsCharacteristicButton)
                {
                    this.Controls.Remove(button);
                }
            }

            if (null != maclsCharacteristicLabel)
            {
                foreach (Label label in maclsCharacteristicLabel)
                {
                    this.Controls.Remove(label);
                }
            }

            if (null != maclsCharacteristicComboBox)
            {
                foreach (ComboBox combo in maclsCharacteristicComboBox)
                {
                    this.Controls.Remove(combo);
                }
            }
        }

        public void AddViewToList(int iFormIDX)
        {
            mlstFormIndices.Add(iFormIDX);
            miFormIDX = iFormIDX;
            SetWindowView(true);
        }

        public void vUpdateMeasures(List<UInt32> lstData, int iActiveMeasureIndex)
        {
            if ((false == mboSuspendUpdates) && (miFormIDX == iActiveMeasureIndex))
            {
                int iMeasIDX = 0;

                foreach (UInt32 u32MeasureData in lstData)
                /* Loop through received list of data */
                {
                    int iMeasCompuMethodIDX;
                    Single sUnscaledData;

                    iMeasCompuMethodIDX = maaiMeasCompuMethodIndices[miFormIDX, iMeasIDX];

                        if (-1 < iMeasCompuMethodIDX)
                        {
                            switch (tclsASAM.mailstActiveMeasLists[maastActiveMeasureIndices[miFormIDX, iMeasIDX].iMeasureQueue][maastActiveMeasureIndices[miFormIDX, iMeasIDX].iMeasureQueueIDX].enRecLayout)
                            {
                                case tenRecLayout.enRL_VALU8:
                                {
                                                sUnscaledData = (Single)(char)u32MeasureData;
                                                break;
                                }
                                case tenRecLayout.enRL_VALU16:
                                {
                                                sUnscaledData = (Single)(0xffff & u32MeasureData);
                                                break;
                                }
                                case tenRecLayout.enRL_VALU32:
                                {
                                                sUnscaledData = (Single)u32MeasureData;
                                                break;
                                }
                                case tenRecLayout.enRL_VALS8:
                                {
                                                sUnscaledData = (Single)(char)u32MeasureData;
                                                break;
                                }
                                case tenRecLayout.enRL_VALS16:
                                {
                                                sUnscaledData = (Single)(Int16)u32MeasureData;
                                                break;
                                }
                                case tenRecLayout.enRL_VALS32:
                                {
                                                sUnscaledData = (Single)(Int32)u32MeasureData;
                                                break;
                                }
                                default:
                                {
                                                sUnscaledData = u32MeasureData;
                                                break;
                                }
                        }


                        if (false == mboMeasEnumMode[iMeasIDX])
                        {
                        maclsMeasureTextBox[iMeasIDX].Text = szGetScaledData(iMeasCompuMethodIDX, iMeasIDX, sUnscaledData, tenMCVElementType.enMCVMeasure);
                    }
                        else
                        {
                            bool boVerbShown = false;

                            int iVerbData = (Int32)sUnscaledData;
                            int iVerbIDX = tclsASAM.milstCompuMethodList[maaiMeasCompuMethodIndices[miFormIDX, iMeasIDX]].iVerbTableIDX;

                            foreach (tstVerbRecord stVerbRecord in tclsASAM.milstVarVerbList[iVerbIDX].lstVarVerb)
                            {
                                if (iVerbData == stVerbRecord.iValueLow)
                                {
                                    maclsMeasureTextBox[iMeasIDX].Text = stVerbRecord.szVerb;
                                    boVerbShown = true;
                                }
                            }

                            if (false == boVerbShown)
                            {
                                maclsMeasureTextBox[iMeasIDX].Text = Convert.ToString(sUnscaledData);
                            }
                        }
                    }

                    iMeasIDX++;
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
                case tenMCVElementType.enMCVMeasure:
                    {
                        szFormattedData = String.Format(maaszMeasCompuMethodFormat[miFormIDX, iElementIDX], sScaledData);
                        break;
                    }
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

        private void vCreateMeasCompuMethodFormat(int iMeasureIDX)
        {
            String szFormatString = "{0:";

            //if (0 < tclsASAM.milstCompuMethodList[maiMeasCompuMethodIndices[iMeasureIDX]].iPreDPCount)
            //{
            //     szFormatString +=
            //        new String('0',
            //        tclsASAM.milstCompuMethodList[maiMeasCompuMethodIndices[iMeasureIDX]].iPreDPCount);
            //}
            //else
            {
                szFormatString += "0";
            }

            if (0 < tclsASAM.milstCompuMethodList[maaiMeasCompuMethodIndices[miFormIDX, iMeasureIDX]].iPostDPCount)
            {
                szFormatString += "." +
                new String('0',
                tclsASAM.milstCompuMethodList[maaiMeasCompuMethodIndices[miFormIDX, iMeasureIDX]].iPostDPCount) + "}";
            }
            else
            {
                szFormatString += "}";
            }

            maaszMeasCompuMethodFormat[miFormIDX, iMeasureIDX] = szFormatString;
        }

        private void vCreateCharCompuMethodFormat(int iCharIDX)
        {
            String szFormatString = "{0:";

            //if (0 < tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharIDX]].iPreDPCount)
            //{
            //     szFormatString +=
            //        new String('0',
            //        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharIDX]].iPreDPCount);
            //}
            //else
            {
                szFormatString += "0";
            }

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

        public void vSetInitialise(bool boInitialise)
        {
            mboInitialising = boInitialise;
        }

        public void vRefreshFromDataPage()
        {
            int iCharIDX;
            Single sCharData = 0;
            UInt32 u32Address;

            for (iCharIDX = 0; iCharIDX < maclsCharacteristicTextBox.Length; iCharIDX++)
            {
                if (-1 < maiCharIndices[iCharIDX])
                {
                    u32Address = tclsASAM.milstCharacteristicList[maiCharIndices[iCharIDX]].u32Address;

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
                    if (false == mboCharEnumMode[iCharIDX])
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
            int iControlIDX;
            int iCharacteristicIDX;
            byte[] au8Data;
            Single sInputData;
            Single sCellDataRaw;

            iControlIDX = Array.IndexOf(maclsCharacteristicButton, sender);
            iCharacteristicIDX = maiCharIndices[iControlIDX];

            try
            {
                sInputData = Convert.ToSingle(maclsCharacteristicTextBox[iControlIDX].Text);
                sInputData = tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].sLowerLim > sInputData ?
                    tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].sLowerLim : sInputData;
                sInputData = tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].sUpperLim < sInputData ?
                    tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].sUpperLim : sInputData;

                maclsCharacteristicTextBox[iControlIDX].Text = String.Format(maszCharCompuMethodFormat[iControlIDX], sInputData);


                switch (tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iControlIDX]].enCM_Type)
                {
                    case tenCM_Type.enLINEAR:
                        {
                            sCellDataRaw = sInputData - tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iControlIDX]].sCoeff2;
                            sCellDataRaw /= tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iControlIDX]].sCoeff1;
                            break;
                        }
                    default:
                        {
                            sCellDataRaw = sInputData;
                            break;
                        }
                }

                switch (tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].iByteCount)
                {
                    case 1:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].boIsSigned)
                            {
                                au8Data = new byte[1];
                                au8Data[0] = Convert.ToByte(sCellDataRaw);
                                tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].u32Address,
                                    au8Data[0]);
                            }
                            else
                            {
                                au8Data = new byte[1];
                                au8Data[0] = Convert.ToByte(sCellDataRaw);
                                tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].u32Address,
                                    au8Data[0]);
                            }

                            break;
                        }
                    case 2:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].boIsSigned)
                            {
                                UInt16 u16InputData = Convert.ToUInt16(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(u16InputData);
                                tclsDataPage.u16SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].u32Address,
                                    BitConverter.ToUInt16(au8Data, 0));
                            }
                            else
                            {
                                Int16 s16InputData = Convert.ToInt16(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(s16InputData);
                                tclsDataPage.u16SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].u32Address,
                                    BitConverter.ToUInt16(au8Data, 0));
                            }
                            break;
                        }
                    case 4:
                        {
                            if (false == tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].boIsSigned)
                            {
                                UInt32 u32InputData = Convert.ToUInt32(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(u32InputData);
                                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].u32Address,
                                    BitConverter.ToUInt32(au8Data, 0));
                            }
                            else
                            {
                                Int32 s32InputData = Convert.ToInt32(sCellDataRaw);
                                au8Data = BitConverter.GetBytes(s32InputData);
                                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].u32Address,
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

                //Program.mAPP_clsUDPComms.mclsUDS.vStartRPC(0x3d,
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


                Program.vUpdateCalibration(tclsASAM.milstCharacteristicList[maiCharIndices[iControlIDX]].szCharacteristicName, au32Data);
            }
            catch
            {
                MessageBox.Show("Input data format or range error");
            }
        }

        private void tclsMeasCharView_Load(object sender, EventArgs e)
        {
            this.Left = 150;
        }

        private void tclsMeasValueCharView_Load(object sender, EventArgs e)
        {

        }

        private void tclsMeasValueCharView_Resize(object sender, System.EventArgs e)
        {
            int iIDX = 0;

            if ((maclsCharacteristicLabel.Length == 0) &&
                (maclsMeasureLabel.Length != 0))
            {
                iIDX = 0;

                foreach(Label label in maclsMeasureLabel)
                {
                    label.Left = 2 * this.Width / 30;
                    label.Width = 3 * this.Width / 8;
                    label.Font = new Font(label.Font.FontFamily, this.Width / 45);
                    label.Height = (this.Height - 35) / (maclsMeasureLabel.Length + 1);
                    label.Top = this.Height / 9  + iIDX * ((this.Height - 20) / (maclsMeasureLabel.Length + 1));
                    label.TextAlign = ContentAlignment.TopRight;
                    iIDX++;
                }


                iIDX = 0;

                foreach(Label label in maclsMeasureUnitsLabel)
                {
                    label.Left = 5 * this.Width / 6;
                    label.Width = this.Width / 4;
                    label.Font = new Font(label.Font.FontFamily, this.Width / 45);
                    label.Height = (this.Height - 35) / (maclsMeasureLabel.Length + 1);
                    label.Top = this.Height / 9 + iIDX * ((this.Height - 20) / (maclsMeasureLabel.Length + 1));
                    label.TextAlign = ContentAlignment.TopLeft;
                    iIDX++;
                }

                iIDX = 0;

                foreach (TextBox textBox in maclsMeasureTextBox)
                {
                    textBox.Left = 52 * this.Width / 100;
                    textBox.Width = this.Width / 4;
                    textBox.Font = new Font(textBox.Font.FontFamily, this.Width / 45);
                    textBox.Height = (this.Height - 35) / (maclsMeasureLabel.Length + 1);
                    textBox.Top = this.Height / 9 + iIDX * ((this.Height - 20) / (maclsMeasureLabel.Length + 1));
                    textBox.TextAlign = HorizontalAlignment.Center;
                    iIDX++;
                }
            }
            else if ((maclsCharacteristicLabel.Length != 0) &&
                (maclsMeasureLabel.Length == 0))
            {
                iIDX = 0;

                while (iIDX < maclsCharacteristicLabel.Length)
                {
                    maclsCharacteristicLabel[iIDX].Left = 20;
                    maclsCharacteristicLabel[iIDX].Top = 40 + 25 * iIDX;
                    maclsCharacteristicLabel[iIDX].Width = 4 * this.Width / 11;

                    if (false == mboCharEnumMode[iIDX])
                    {
                        maclsCharacteristicTextBox[iIDX].Left = 30 + 4 * this.Width / 11;
                        maclsCharacteristicTextBox[iIDX].Top = 40 + 25 * iIDX;
                        maclsCharacteristicTextBox[iIDX].Width = 2 * this.Width / 11;
                        maclsCharacteristicComboBox[iIDX].Left = this.Width;
                    }
                    else
                    {
                        maclsCharacteristicTextBox[iIDX].Left = this.Width;
                        maclsCharacteristicComboBox[iIDX].Top = 40 + 25 * iIDX;
                        maclsCharacteristicComboBox[iIDX].Width = 2 * this.Width / 11;
                        maclsCharacteristicComboBox[iIDX].Left = 30 + 4 * this.Width / 11;
                    }

                    maclsCharacteristicUnitsLabel[iIDX].Left = 40 + 6 * this.Width / 11;
                    maclsCharacteristicUnitsLabel[iIDX].Top = 40 + 25 * iIDX;
                    maclsCharacteristicUnitsLabel[iIDX].Width = 2 * this.Width / 11;

                    maclsCharacteristicButton[iIDX].Left = 60 + 8 * this.Width / 11;
                    maclsCharacteristicButton[iIDX].Top = 40 + 25 * iIDX;
                    maclsCharacteristicButton[iIDX].Width = 2 * this.Width / 15;
                    iIDX++;
                }
            }
            else if ((maclsCharacteristicLabel.Length != 0) &&
                (maclsMeasureLabel.Length != 0))
            {
                iIDX = 0;

                foreach (Label label in maclsMeasureLabel)
                {
                    label.Left = this.Width / 30;
                    label.Width = this.Width / 4;
                    label.Font = new Font(label.Font.FontFamily, this.Width / 45);
                    label.Height = ((this.Height - 35) / (maclsMeasureLabel.Length + 1)) - 5;
                    label.Top = this.Height / 13 + iIDX * ((this.Height - 20) / (maclsMeasureLabel.Length + 1));
                    label.TextAlign = ContentAlignment.TopRight;
                    iIDX++;
                }

                iIDX = 0;

                foreach (Label label in maclsMeasureUnitsLabel)
                {
                    label.Left = 5 * this.Width / 12;
                    label.Width = this.Width / 2;
                    label.Font = new Font(label.Font.FontFamily, this.Width / 45);
                    label.Height = ((this.Height - 35) / (maclsMeasureLabel.Length + 1)) - 5;
                    label.Top = this.Height / 13 + iIDX * ((this.Height - 20) / (maclsMeasureLabel.Length + 1));
                    label.TextAlign = ContentAlignment.TopLeft;
                    iIDX++;
                }

                iIDX = 0;

                foreach (TextBox textBox in maclsMeasureTextBox)
                {
                    textBox.Left = 29 * this.Width / 100;
                    textBox.Width = this.Width / 10;
                    textBox.Font = new Font(textBox.Font.FontFamily, this.Width / 45);
                    textBox.Height = ((this.Height - 35) / (maclsMeasureLabel.Length + 1)) - 5;
                    textBox.Top = this.Height / 13 + iIDX * ((this.Height - 20) / (maclsMeasureLabel.Length + 1));
                    textBox.TextAlign = HorizontalAlignment.Center;
                    iIDX++;
                }


                iIDX = 0;

                while (iIDX < maclsCharacteristicLabel.Length)
                {
                    maclsCharacteristicLabel[iIDX].Left = this.Width / 2 + 20;
                    maclsCharacteristicLabel[iIDX].Top = 40 + 25 * iIDX;
                    maclsCharacteristicLabel[iIDX].Width = 4 * this.Width / 22;

                    if (false == mboCharEnumMode[iIDX])
                    {
                        maclsCharacteristicTextBox[iIDX].Left = 8 * this.Width / 11;
                        maclsCharacteristicTextBox[iIDX].Top = 40 + 25 * iIDX;
                        maclsCharacteristicTextBox[iIDX].Width = this.Width / 11;
                        maclsCharacteristicComboBox[iIDX].Left = this.Width;
                    }
                    else
                    {
                        maclsCharacteristicTextBox[iIDX].Left = this.Width;
                        maclsCharacteristicComboBox[iIDX].Top = 40 + 25 * iIDX;
                        maclsCharacteristicComboBox[iIDX].Width = this.Width / 11;
                        maclsCharacteristicComboBox[iIDX].Left = 8 * this.Width / 11;
                    }

                    maclsCharacteristicUnitsLabel[iIDX].Left = 40 + 6 * this.Width / 11;
                    maclsCharacteristicUnitsLabel[iIDX].Top = 40 + 25 * iIDX;
                    maclsCharacteristicUnitsLabel[iIDX].Width = 2 * this.Width / 11;

                    maclsCharacteristicButton[iIDX].Left = 40 + 17 * this.Width / 22;
                    maclsCharacteristicButton[iIDX].Top = 40 + 25 * iIDX;
                    maclsCharacteristicButton[iIDX].Width = 2 * this.Width / 15;
                    iIDX++;
                }
            }
        }

        private void toolStripButtonLeft_Click(object sender, EventArgs e)
        {
            GetPrevFormIDX();
            SetWindowView(false);
            vRefreshFromDataPage();
        }

        private void GetNextFormIDX()
        {
            int iListIDX = 0;

            foreach (int iFormIDX in mlstFormIndices)
            {
                if (miFormIDX == iFormIDX)
                {
                    if (iListIDX >= (mlstFormIndices.Count - 1))
                    {
                        miFormIDX = mlstFormIndices[0];
                    }
                    else
                    {
                        miFormIDX = mlstFormIndices[iListIDX + 1];
                    }
                    break;
                }

                iListIDX++;
            }
        }

        private void GetPrevFormIDX()
        {
            int iListIDX = 0;

            foreach (int iFormIDX in mlstFormIndices)
            {
                if (miFormIDX == iFormIDX)
                {
                    if (iListIDX > 0)
                    {
                        miFormIDX = mlstFormIndices[iListIDX - 1];
                    }
                    else
                    {
                        miFormIDX = mlstFormIndices[mlstFormIndices.Count - 1];
                    }
                    break;
                }

                iListIDX++;
            }
        }

        private void toolStripButtonRight_Click(object sender, EventArgs e)
        {
            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);
            GetNextFormIDX();
            SetWindowView(false);
            vRefreshFromDataPage();
            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);
        }

        public void RequestShowViewIndex(int iFormReqIDX)
        {
            foreach (int iFormIDX in mlstFormIndices)
            {
                if (iFormIDX == iFormReqIDX)
                {
                    Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);
                    miFormIDX = iFormIDX;
                    SetWindowView(false);
                    vRefreshFromDataPage();
                    Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);
                    break;
                }
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

        private void toolStripValueCharView_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            tclsHelpList clsHelpList = new tclsHelpList(maiCharIndices);
            clsHelpList.Show();
        }
    }
}