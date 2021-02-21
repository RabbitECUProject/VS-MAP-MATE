/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Axes Edit                                              */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsAxesEdit.cs                                        */
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
    public partial class tclsAxesEdit : Form
    {
        int miXAxisRef;
        int miYAxisRef;
        int miXAxisCount;
        int miYAxisCount;
        int miXAxisIDX;
        int miYAxisIDX;
        UInt16 mu16GenericXIDX;
        UInt16 mu16GenericYIDX;
        int miGenericXCharIDX;
        int miGenericYCharIDX;
        int miGenericCompuMethodXIDX;
        int miGenericCompuMethodYIDX;
        int miGenericXMeasureIDX;
        int miGenericYMeasureIDX;
        TextBox[] maclsXAxesTextBox;
        TextBox[] maclsYAxesTextBox;
        ComboBox mclsAxesGenericXIDX;
        ComboBox mclsAxesGenericYIDX;
        Label mclsXAxisLabel;
        Label mclsYAxisLabel;
        bool mboGenericAxes;
        List<String> mlstXAxisLabels;
        List<String> mlstYAxisLabels;


        public tclsAxesEdit(int iXAxisRef, int iYAxisRef, int iXAxisCompuMethodIDX, int iYAxisCompuMethodIDX, bool boShowControl)
        {
            if (true == boShowControl)
            {
                InitializeComponent();
            }

            miGenericCompuMethodXIDX = -1;
            miGenericCompuMethodYIDX = -1;

            if (-1 != iXAxisRef)
            {
                mlstXAxisLabels = new List<String>();
            }

            if (-1 != iYAxisRef)
            {
                mlstYAxisLabels = new List<String>();
            }

            if (0 == tclsASAM.milstAxisDescrList[iXAxisRef].szAxisPointsRef.IndexOf("PWM"))
            {
                mboGenericAxes = true;
            }
            else
            {
                mboGenericAxes = false;
            }

            AddControls(iXAxisRef, iYAxisRef, iXAxisCompuMethodIDX, iYAxisCompuMethodIDX);
        }

        public List<String> GetXAxisLabels()
        {
            return mlstXAxisLabels;
        }

        public List<String> GetYAxisLabels()
        {
            return mlstYAxisLabels;
        }

        private void AddControls(int iXAxisRef, int iYAxisRef, int iXAxisCompuMethodIDX, int iYAxisCompuMethodIDX)
        {
            miXAxisRef = iXAxisRef;
            miYAxisRef = iYAxisRef;
            miXAxisIDX = iXAxisCompuMethodIDX;
            miYAxisIDX = iYAxisCompuMethodIDX;

            if (-1 != miXAxisRef)
            {
                miXAxisCount = tclsASAM.milstAxisDescrList[iXAxisRef].iAxisPointCount;
                maclsXAxesTextBox = new TextBox[miXAxisCount];
                mclsXAxisLabel = new Label();

                String szCompuMethod = tclsASAM.milstAxisDescrList[miXAxisIDX].szCompuMethod;
                int iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szCompuMethod);

                for (int iXIDX = 0; iXIDX < miXAxisCount; iXIDX++)
                {
                    maclsXAxesTextBox[iXIDX] = new TextBox();
                    maclsXAxesTextBox[iXIDX].Left = 20 + 40 * iXIDX;
                    maclsXAxesTextBox[iXIDX].Top = 40;
                    maclsXAxesTextBox[iXIDX].Width = 40;
                    this.Controls.Add(maclsXAxesTextBox[iXIDX]);

                    maclsXAxesTextBox[iXIDX].Text = szGetAxisPointData(iXIDX, false);

                    if (true == mboGenericAxes)
                    {
                        mlstXAxisLabels.Add(szGetAxisPointData(iXIDX, false) + " " + tclsASAM.milstCompuMethodList[miGenericCompuMethodXIDX].szUnitsString);
                    }
                    else
                    {
                        mlstXAxisLabels.Add(szGetAxisPointData(iXIDX, false) + " " + tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString);
                    }
                }

                mclsXAxisLabel.Left = 20;
                mclsXAxisLabel.Width = 400;
                mclsXAxisLabel.Top = 10;
                mclsXAxisLabel.Font = new Font(this.Font.FontFamily, 10);
                this.Controls.Add(mclsXAxisLabel);

                if (true == mboGenericAxes)
                {
                    mclsXAxisLabel.Text = tclsASAM.milstAvailableMeasList[miGenericXMeasureIDX].szMeasurementName + " [" + miXAxisCount.ToString() + " points], Units: " + tclsASAM.milstCompuMethodList[miGenericCompuMethodXIDX].szUnitsString;
                    mclsAxesGenericXIDX = new ComboBox();
                    LoadGenericAxesCombo(false);
                    mclsAxesGenericXIDX.SelectedIndex = mu16GenericXIDX;
                    mclsAxesGenericXIDX.Left = 30 + 40 * miXAxisCount;
                    mclsAxesGenericXIDX.Top = 40;
                    this.Controls.Add(mclsAxesGenericXIDX);
                }
                else
                {
                    mclsXAxisLabel.Text = tclsASAM.milstAxisDescrList[iXAxisRef].szVar + " [" + miXAxisCount.ToString() + " points], Units: " + tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;
                }
            }

            if (-1 != miYAxisRef)
            {
                miYAxisCount = tclsASAM.milstAxisDescrList[iYAxisRef].iAxisPointCount;
                maclsYAxesTextBox = new TextBox[miYAxisCount];
                mclsYAxisLabel = new Label();

                String szCompuMethod = tclsASAM.milstAxisDescrList[miYAxisIDX].szCompuMethod;
                int iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szCompuMethod);

                if (-1 != iCompuMethodIDX)
                {
                    for (int iYIDX = 0; iYIDX < miYAxisCount; iYIDX++)
                    {
                        maclsYAxesTextBox[iYIDX] = new TextBox();
                        maclsYAxesTextBox[iYIDX].Left = 20 + 40 * iYIDX;
                        maclsYAxesTextBox[iYIDX].Top = 110;
                        maclsYAxesTextBox[iYIDX].Width = 40;
                        this.Controls.Add(maclsYAxesTextBox[iYIDX]);

                        maclsYAxesTextBox[iYIDX].Text = szGetAxisPointData(iYIDX, true);
                    }

                    mclsYAxisLabel.Left = 20;
                    mclsYAxisLabel.Width = 400;
                    mclsYAxisLabel.Top = 80;
                    mclsYAxisLabel.Font = new Font(this.Font.FontFamily, 10);
                    this.Controls.Add(mclsYAxisLabel);

                    if (true == mboGenericAxes)
                    {
                        mclsYAxisLabel.Text = tclsASAM.milstAvailableMeasList[miGenericYMeasureIDX].szMeasurementName + " [" + miYAxisCount.ToString() + " points], Units: " + tclsASAM.milstCompuMethodList[miGenericCompuMethodYIDX].szUnitsString;
                        mclsAxesGenericYIDX = new ComboBox();
                        LoadGenericAxesCombo(true);
                        mclsAxesGenericYIDX.SelectedIndex = mu16GenericYIDX;
                        mclsAxesGenericYIDX.Left = 30 + 40 * miXAxisCount;
                        mclsAxesGenericYIDX.Top = 110;
                        this.Controls.Add(mclsAxesGenericYIDX);
                    }
                    else
                    {
                        mclsYAxisLabel.Text = tclsASAM.milstAxisDescrList[iYAxisRef].szVar + " [" + miYAxisCount.ToString() + " points], Units: " + tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString; 
                    }
                }
            }

            if ((-1 != miXAxisRef) && (-1 != miYAxisRef))
            {
                this.Height = 250;

                if (true == mboGenericAxes)
                {
                    this.Width = 40 * miXAxisCount + 180;
                }
                else
                {
                this.Width = 40 * miXAxisCount + 60;
                }


                buttonEnter.Left = this.Width - 140;
                buttonEnter.Top = this.Height - 90;
            }
            else
            {
                this.Height = 180;

                if (true == mboGenericAxes)
                {
                    this.Width = 40 * miXAxisCount + 180;
                }
                else
                {
                this.Width = 40 * miXAxisCount + 60;
                }

                buttonEnter.Left = this.Width - 140;
                buttonEnter.Top = this.Height - 90;
            }
        }

        private void LoadGenericAxesCombo(bool isYAxis)
        {
            ComboBox clsCombo = false == isYAxis ? mclsAxesGenericXIDX : mclsAxesGenericYIDX;

            if (null != clsCombo)
            {
                clsCombo.Items.Add("Engine Speed Raw");
                clsCombo.Items.Add("Throttle Angle");
                clsCombo.Items.Add("MAP kPa");
                clsCombo.Items.Add("Coolant Temperature");
                clsCombo.Items.Add("Air Temperature");
            }
        }

        private string szGetAxisPointData(int iGridColIDX, bool boYAxis)
        {
            string szAxisPoint = "-";
            string szAxisPtsRef;
            string szAxisCompuMethod;
            int iAxisPtsIDX;
            int iCompuMethodIDX;
            UInt32 u32AxisPtsAddress;

            int iAxis = (false == boYAxis) ? miXAxisRef : miYAxisRef;

            if (-1 < iAxis)
            {
                szAxisPtsRef = tclsASAM.milstAxisDescrList[iAxis].szAxisPointsRef;
                iAxisPtsIDX = tclsASAM.iGetAxisPtsIndex(szAxisPtsRef);
                u32AxisPtsAddress = tclsASAM.milstAxisPtsList[iAxisPtsIDX].u32Address;
                szAxisCompuMethod = tclsASAM.milstAxisPtsList[iAxisPtsIDX].szCompuMethod;

                if (0 == szAxisCompuMethod.IndexOf("CM_GENERIC_SOURCE_IDX"))
                {
                    string szGenericIDX = szAxisCompuMethod.Substring(21);

                    try
                    {
                        int iGenericIDX = Convert.ToInt16(szGenericIDX);

                        switch (iGenericIDX)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            {
                                if (-1 == miGenericCompuMethodXIDX)
                                {
                                    string szAxesSource = "PWM 2D " + szGenericIDX + " Axis Source X";
                                    miGenericXCharIDX = tclsASAM.iGetCharIndex(szAxesSource);

                                    UInt32 u32AxesSourceIDXAddress = tclsASAM.milstCharacteristicList[miGenericXCharIDX].u32Address;

                                    tclsDataPage.u16GetWorkingData(u32AxesSourceIDXAddress, ref mu16GenericXIDX);

                                    string szGenericMeasure = "Generic Data " + (mu16GenericXIDX + 1).ToString() + "|LINK|";
                                    miGenericXMeasureIDX = tclsASAM.iGetAvailableMeasIndex(szGenericMeasure);

                                    string szCompuMethod = tclsASAM.milstAvailableMeasList[miGenericXMeasureIDX].szCompuMethod;
                                    miGenericCompuMethodXIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szCompuMethod);
                                }

                                break;
                            }
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                            {
                                if (-1 == miGenericCompuMethodXIDX)
                                {
                                    string szAxesSource = "PWM 3D " + (iGenericIDX - 8).ToString() + " Axis Source X";
                                    miGenericXCharIDX = tclsASAM.iGetCharIndex(szAxesSource);

                                    UInt32 u32AxesSourceIDXAddress = tclsASAM.milstCharacteristicList[miGenericXCharIDX].u32Address;

                                    tclsDataPage.u16GetWorkingData(u32AxesSourceIDXAddress, ref mu16GenericXIDX);

                                    string szGenericMeasure = "Generic Data " + (mu16GenericXIDX + 1).ToString() + "|LINK|";
                                    miGenericXMeasureIDX = tclsASAM.iGetAvailableMeasIndex(szGenericMeasure);

                                    string szCompuMethod = tclsASAM.milstAvailableMeasList[miGenericXMeasureIDX].szCompuMethod;
                                    miGenericCompuMethodXIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szCompuMethod);
                                }

                                break;
                            }
                            case 13:
                            case 14:
                            case 15:
                            case 16:
                            {
                                if (-1 == miGenericCompuMethodYIDX)
                                {
                                    string szAxesSource = "PWM 3D " + (iGenericIDX - 12).ToString() + " Axis Source Y";
                                    miGenericYCharIDX = tclsASAM.iGetCharIndex(szAxesSource);

                                    UInt32 u32AxesSourceIDXAddress = tclsASAM.milstCharacteristicList[miGenericYCharIDX].u32Address;

                                    tclsDataPage.u16GetWorkingData(u32AxesSourceIDXAddress, ref mu16GenericYIDX);

                                    string szGenericMeasure = "Generic Data " + (mu16GenericYIDX + 1).ToString() + "|LINK|";
                                    miGenericYMeasureIDX = tclsASAM.iGetAvailableMeasIndex(szGenericMeasure);

                                    string szCompuMethod = tclsASAM.milstAvailableMeasList[miGenericYMeasureIDX].szCompuMethod;
                                    miGenericCompuMethodYIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szCompuMethod);
                                }

                                break;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error calculating generic spread");
                    }
                }

                if (true == boYAxis)
                {
                    if (true == mboGenericAxes)
                    {
                        iCompuMethodIDX = miGenericCompuMethodYIDX;
                    }
                    else
                    {
                iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szAxisCompuMethod);
                    }
                }
                else
                {
                    if (true == mboGenericAxes)
                    {
                        iCompuMethodIDX = miGenericCompuMethodXIDX;
                    }
                    else
                    {
                        iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szAxisCompuMethod);
                    }
                }


                String szFormatString = "{0:";

                if (0 < tclsASAM.milstCompuMethodList[iCompuMethodIDX].iPreDPCount)
                {
                    szFormatString +=
                       new String('0',
                       tclsASAM.milstCompuMethodList[iCompuMethodIDX].iPreDPCount);
                }
                else
                {
                    szFormatString += "0";
                }

                if (0 < tclsASAM.milstCompuMethodList[iCompuMethodIDX].iPostDPCount)
                {
                    szFormatString += "." +
                    new String('0',
                    tclsASAM.milstCompuMethodList[iCompuMethodIDX].iPostDPCount) + "}";
                }
                else
                {
                    szFormatString += "}";
                }

                switch (tclsASAM.milstAxisPtsList[iAxisPtsIDX].enRecLayout)
                {
                    case tenRecLayout.enRL_VALU8:
                        {
                            break;
                        }
                    case tenRecLayout.enRL_VALU16:
                        {
                            UInt16 u16AxisPointData = 0;
                            Single sScaledAxisPointData = 0;

                            u32AxisPtsAddress += (2 * (UInt32)iGridColIDX);
                            tclsDataPage.u16GetWorkingData(u32AxisPtsAddress, ref u16AxisPointData);

                            if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                            {
                                sScaledAxisPointData = u16AxisPointData * tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1
                            + tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2;

                                szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            }
                            else if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enIDENTICAL)
                            {
                                sScaledAxisPointData = (Single)u16AxisPointData;

                                szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            }

                            szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            break;
                        }
                    case tenRecLayout.enRL_VALU32:
                        {
                            UInt32 u32AxisPointData = 0;
                            Single sScaledAxisPointData;

                            u32AxisPtsAddress += (4 * (UInt32)iGridColIDX);
                            tclsDataPage.u32GetWorkingData(u32AxisPtsAddress, ref u32AxisPointData);

                            if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                            {
                                sScaledAxisPointData = u32AxisPointData * tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1
                                + tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2;

                                szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            }
                            else if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enIDENTICAL)
                            {
                                sScaledAxisPointData = (Single)u32AxisPointData;

                                szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            }

                            break;
                        }
                    case tenRecLayout.enRL_VALS8:
                        {
                            break;
                        }
                    case tenRecLayout.enRL_VALS16:
                        {
                            break;
                        }
                    case tenRecLayout.enRL_VALS32:
                        {
                            Int32 s32AxisPointData = 0;
                            Single sScaledAxisPointData = 0;

                            u32AxisPtsAddress += (4 * (UInt32)iGridColIDX);
                            tclsDataPage.s32GetWorkingData(u32AxisPtsAddress, ref s32AxisPointData);

                            if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                            {
                                sScaledAxisPointData = s32AxisPointData * tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1
                            + tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2;

                                szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            }
                            else if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enIDENTICAL)
                            {
                                sScaledAxisPointData = (Single)s32AxisPointData;

                            szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            }
 
                            break;
                        }
                }
            }

            return szAxisPoint;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            string szAxisPtsRef;
            string szAxisCompuMethod;
            int iAxisPtsIDX;
            int iCompuMethodIDX;
            UInt32 u32AxisPtsAddress;
            Single sScaledAxisPointData;

            if (-1 < miXAxisIDX)
            {
                szAxisPtsRef = tclsASAM.milstAxisDescrList[miXAxisIDX].szAxisPointsRef;
                iAxisPtsIDX = tclsASAM.iGetAxisPtsIndex(szAxisPtsRef);
                u32AxisPtsAddress = tclsASAM.milstAxisPtsList[iAxisPtsIDX].u32Address;
                szAxisCompuMethod = tclsASAM.milstAxisPtsList[iAxisPtsIDX].szCompuMethod;
                iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szAxisCompuMethod);

                for (int iGridColIDX = 0; iGridColIDX < miXAxisCount; iGridColIDX++)
                {
                    try
                    {
                        sScaledAxisPointData = Convert.ToSingle(maclsXAxesTextBox[iGridColIDX].Text);
                    }
                    catch
                    {
                        sScaledAxisPointData = 0;
                    }

                    switch (tclsASAM.milstAxisPtsList[iAxisPtsIDX].enRecLayout)
                    {
                        case tenRecLayout.enRL_VALU8:
                            {
                                break;
                            }
                        case tenRecLayout.enRL_VALU16:
                            {
                                UInt16 u16AxisPointData = 0;

                                if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                                {
                                    sScaledAxisPointData = u16AxisPointData * tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1
                                    + tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2;
                                }

                                tclsDataPage.u16SetWorkingData(u32AxisPtsAddress, u16AxisPointData);
                                u32AxisPtsAddress += 2;

                                break;
                            }
                        case tenRecLayout.enRL_VALU32:
                            {
                                UInt32 u32AxisPointData = 0;

 
                                if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                                {
                                    u32AxisPointData = (UInt32)((sScaledAxisPointData - tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2) /
                                        tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1);
                                }
                                else if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enIDENTICAL)
                                {
                                    u32AxisPointData = (UInt32)sScaledAxisPointData;
                                }

                                tclsDataPage.u32SetWorkingData(u32AxisPtsAddress, u32AxisPointData);
                                u32AxisPtsAddress += 4;

                                break;
                            }
                        case tenRecLayout.enRL_VALS8:
                            {
                                break;
                            }
                        case tenRecLayout.enRL_VALS16:
                            {
                                break;
                            }
                        case tenRecLayout.enRL_VALS32:
                            {
                                Int32 s32AxisPointData = 0;

                                if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                                {
                                    s32AxisPointData = (Int32)((sScaledAxisPointData - tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2) /
                                        tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1);
                                }

                                tclsDataPage.s32SetWorkingData(u32AxisPtsAddress, s32AxisPointData);

                                u32AxisPtsAddress += 4;
                                break;
                            }
                    }
                }
            }

            if (-1 < miYAxisIDX)
            {
                szAxisPtsRef = tclsASAM.milstAxisDescrList[miYAxisIDX].szAxisPointsRef;
                iAxisPtsIDX = tclsASAM.iGetAxisPtsIndex(szAxisPtsRef);
                u32AxisPtsAddress = tclsASAM.milstAxisPtsList[iAxisPtsIDX].u32Address;
                szAxisCompuMethod = tclsASAM.milstAxisPtsList[iAxisPtsIDX].szCompuMethod;
                iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szAxisCompuMethod);

                for (int iGridColIDX = 0; iGridColIDX < miYAxisCount; iGridColIDX++)
                {
                    try
                    {
                        sScaledAxisPointData = Convert.ToSingle(maclsYAxesTextBox[iGridColIDX].Text);
                    }
                    catch
                    {
                        sScaledAxisPointData = 0;
                    }

                    switch (tclsASAM.milstAxisPtsList[iAxisPtsIDX].enRecLayout)
                    {
                        case tenRecLayout.enRL_VALU8:
                            {
                                break;
                            }
                        case tenRecLayout.enRL_VALU16:
                            {
                                UInt16 u16AxisPointData = 0;

                                if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                                {
                                    sScaledAxisPointData = u16AxisPointData * tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1
                                    + tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2;
                                }

                                tclsDataPage.u16SetWorkingData(u32AxisPtsAddress, u16AxisPointData);
                                u32AxisPtsAddress += 2;

                                break;
                            }
                        case tenRecLayout.enRL_VALU32:
                            {
                                UInt32 u32AxisPointData = 0;


                                if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                                {
                                    u32AxisPointData = (UInt32)((sScaledAxisPointData - tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2) /
                                        tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1);
                                }
                                else if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enIDENTICAL)
                                {
                                    u32AxisPointData = (UInt32)sScaledAxisPointData;
                                }

                                tclsDataPage.u32SetWorkingData(u32AxisPtsAddress, u32AxisPointData);
                                u32AxisPtsAddress += 4;

                                break;
                            }
                        case tenRecLayout.enRL_VALS8:
                            {
                                break;
                            }
                        case tenRecLayout.enRL_VALS16:
                            {
                                break;
                            }
                        case tenRecLayout.enRL_VALS32:
                            {
                                Int32 s32AxisPointData = 0;

                                if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                                {
                                    s32AxisPointData = (Int32)((sScaledAxisPointData - tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2) /
                                        tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1);
                                }

                                tclsDataPage.s32SetWorkingData(u32AxisPtsAddress, s32AxisPointData);

                                u32AxisPtsAddress += 4;
                                break;
                            }
                    }
                }
            }

            this.Close();
        }
    }
}
