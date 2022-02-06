/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Cruve and Maps View                                    */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsMeasureCurveMapView.cs                             */
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
using SurfaceChartWrapper;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;

namespace UDP
{
    public partial class tclsMeasCurveMapView : Form
    {
        Double[][,] maasTableShadowData;
        tclsWindowElement[] maclsWindowElement;
        int[] maiCharIndices;
        int[] maiCharCompuMethodIndices;
        String[] maszCharCompuMethodFormat;
        bool mboInitialising;
        bool mboIs3D;
        bool mboRequestShutdown;
        int miFormIDX;
        int miFormBaseIDX;
        int[] miXAxisPointCount;
        int[] miYAxisPointCount;
        int[] miXAxisRef;
        int[] miYAxisRef;
        UInt16 mu16SpreadDividend2D;
        UInt16 mu16SpreadRemainder2D;
        UInt16 mu16SpreadXDividend3D;
        UInt16 mu16SpreadXRemainder3D;
        UInt16 mu16SpreadYDividend3D;
        UInt16 mu16SpreadYRemainder3D;
        int miOldCellRowIDX;
        int miOldCellColIDX;
        int miLiveCellWaitCount;
        int miCharCount;
        int miDivider;
        SurfaceChartHost mHost;
        List<string>[] mlstXAxisLabels;
        List<string>[] mlstYAxisLabels;
        List<string>[] mlstZAxisLabels;
        List<int> mlstFormIndices;
        UInt32 mu32Spread3DTableAddress;
        UInt32 mu32Spread2DTableAddress;
        UInt32 mu32SpecialTableAddress;
        int miTimerCount;
        bool mboFocusActive;
        int miBadCRCCount;
        int miGoodCRCCount;
        String mszWindowLabel;
        int miOldCellChangedErrRowIDX;
        int miOldCellChangedErrColIDX;
        tclsTuningTargetDisplay mclsTuningDisplay;
        int miTuningRetVal;
        int miAutoTuneWait;

        enum tenTuningMode
        {
            enNone,
            enVE
        }

        tenTuningMode menTuningMode;

        public tclsMeasCurveMapView(int iFormIDX, bool boIs3D)
        {
            InitializeComponent();
            mboIs3D = boIs3D;
            mHost = new SurfaceChartHost();
            mHost.Dock = DockStyle.Fill;
            Host = mHost;
            tableLayoutPanel1.Controls.Add(mHost, 0, 1);

            if (true == mboIs3D)
            {
                miDivider = 5;

                int iCharIDX = tclsASAM.iGetCharIndex("_DIAG Spread Address Table 0");

                if (-1 != iCharIDX)
                {
                    mu32Spread3DTableAddress = tclsASAM.milstCharacteristicList[iCharIDX].u32Address;
                }

                iCharIDX = tclsASAM.iGetCharIndex("_DIAG Spread Address Table 3");

                if (-1 != iCharIDX)
                {
                    mu32SpecialTableAddress = tclsASAM.milstCharacteristicList[iCharIDX].u32Address;
                }
            }
            else
            {
                miDivider = 8;

                int iCharIDX = tclsASAM.iGetCharIndex("_DIAG Spread Address Table 2");

                if (-1 != iCharIDX)
                {
                    mu32Spread2DTableAddress = tclsASAM.milstCharacteristicList[iCharIDX].u32Address;
                }

                iCharIDX = tclsASAM.iGetCharIndex("_DIAG Spread Address Table 3");

                if (-1 != iCharIDX)
                {
                    mu32SpecialTableAddress = tclsASAM.milstCharacteristicList[iCharIDX].u32Address;
                }
            }

            miFormIDX = iFormIDX;
            miFormBaseIDX = iFormIDX;
            mlstFormIndices = new List<int>();
            mlstFormIndices.Add(miFormIDX);
            InitializeComponent();
            SetWindowView();
            SetSpreadAddresses();

            tclsASAM.stTryAddActiveMeas("_DIAG Spread Result Table 0", miFormIDX);
            tclsASAM.stTryAddActiveMeas("_DIAG Spread Result Table 1", miFormIDX);
            tclsASAM.stTryAddActiveMeas("_DIAG Spread Result Table 2", miFormIDX);
            tclsASAM.stTryAddActiveMeas("_DIAG Spread Result Table 3", miFormIDX);
            tclsASAM.stTryAddActiveMeas("USERCAL_ConfigCRC", miFormIDX);
            tclsASAM.vCalcDDDIAndSetRate(iFormIDX, 10);

            miOldCellChangedErrColIDX = -1;
            miOldCellChangedErrRowIDX = -1;

            menTuningMode = tenTuningMode.enNone;
            mclsTuningDisplay = null;
        }

        public void vSetInitialise(bool boInitialise)
        {
            mboInitialising = boInitialise;
        }

        public void AddViewToList(int iFormIDX, bool boIs3D)
        {
            if (mboIs3D == boIs3D)
            {
                mlstFormIndices.Add(iFormIDX);
            }
        }

        private void GetNextFormIDX()
        {
            int iListIDX = 0;

            foreach(int iFormIDX in mlstFormIndices)
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

        public void SetWindowView()
        { 
            int iCharacteristicIDX = 0;
            int iElementIDX;
            int iXAxisRef;
            int iXAxisPointCount;
            int iYAxisRef;
            int iYAxisPointCount;
            int iAxisPtsIDX;
            int iCompuMethodIDX;
            string szAxisPtsRef;
            string szAxisCompuMethod;
            bool boLoadAbort = false;
            int iBadCharIDX = -1;

            maclsWindowElement = new tclsWindowElement[Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].Count];
            Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].CopyTo(maclsWindowElement, 0);

            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "characteristic"))
                {
                    if (-1 == tclsASAM.iGetCharIndex(maclsWindowElement[iElementIDX].szA2LName))
                    {
                        boLoadAbort = true;
                        iBadCharIDX = iElementIDX;
                    }
                }
            }

            if (true == boLoadAbort)
            {
                Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, "Map or curve characteristic element " +
                    maclsWindowElement[iBadCharIDX].szA2LName + " not found in ASAM database, this map or curve will not be shown");
                return;
            }

            miLiveCellWaitCount = 0;
            miAutoTuneWait = -1;
            miTuningRetVal = -1;
            miOldCellColIDX = -1;
            miOldCellRowIDX = -1;
            miXAxisPointCount = new int[10];
            miYAxisPointCount = new int[10];
            miXAxisRef = new int[10];
            miYAxisRef = new int[10];
            mlstXAxisLabels = new List<string>[10];
            mlstYAxisLabels = new List<string>[10];
            mlstZAxisLabels = new List<string>[10];

            for (int iArrayIDX = 0; iArrayIDX < 10; iArrayIDX++)
            {
                mlstXAxisLabels[iArrayIDX] = new List<string>();
                mlstYAxisLabels[iArrayIDX] = new List<string>();
                mlstZAxisLabels[iArrayIDX] = new List<string>();
            }

            maclsWindowElement = new tclsWindowElement[Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].Count];
            maasTableShadowData = new Double[1][,];

            Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].CopyTo(maclsWindowElement, 0);
            maiCharIndices = new int[1];
            maiCharCompuMethodIndices = new int[1];
            maszCharCompuMethodFormat = new string[1];
            mboInitialising = false;

            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "characteristic"))
                {
                    int iCharIDX = tclsASAM.iGetCharIndex(maclsWindowElement[iElementIDX].szA2LName);

                    if ((tenParamType.enPT_CURVE == tclsASAM.milstCharacteristicList[iCharIDX].enParamType) ||
                       (tenParamType.enPT_MAP == tclsASAM.milstCharacteristicList[iCharIDX].enParamType))
                    {
                        Array.Resize(ref maiCharIndices, iCharacteristicIDX + 1);
                        Array.Resize(ref maiCharCompuMethodIndices, iCharacteristicIDX + 1);
                        Array.Resize(ref maszCharCompuMethodFormat, iCharacteristicIDX + 1);
                        maiCharIndices[iCharacteristicIDX] = tclsASAM.iGetCharIndex(maclsWindowElement[iElementIDX].szA2LName);
                        maiCharCompuMethodIndices[iCharacteristicIDX] = tclsASAM.iGetCompuMethodIndexFromChar(maclsWindowElement[iElementIDX].szA2LName);
                        iCharacteristicIDX++;
                    }
                }

                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "window"))
                {
                    mszWindowLabel = ConstantData.APPCONFIG.szAppName + ":" + maclsWindowElement[iElementIDX].szLabel;

                    SetWindowLabel("");
                }
            }

            maasTableShadowData = new Double[iCharacteristicIDX][,];

            for (int iCharIDX = 0; iCharIDX < iCharacteristicIDX; iCharIDX++)
            {
                iXAxisRef = tclsASAM.milstCharacteristicList[maiCharIndices[iCharIDX]].iXAxisRef;
                iXAxisPointCount = tclsASAM.milstAxisDescrList[iXAxisRef].iAxisPointCount;
                miXAxisPointCount[iCharIDX] = iXAxisPointCount;
                miXAxisRef[iCharIDX] = iXAxisRef;

                iYAxisRef = tclsASAM.milstCharacteristicList[maiCharIndices[iCharIDX]].iYAxisRef;

                if (-1 != iYAxisRef)
                {
                    iYAxisPointCount = tclsASAM.milstAxisDescrList[iYAxisRef].iAxisPointCount;
                    miYAxisRef[iCharIDX] = iYAxisRef;
                }
                else
                {
                    iYAxisPointCount = 1;
                    miYAxisRef[iCharIDX] = -1;
                }

                miYAxisPointCount[iCharIDX] = iYAxisPointCount;
                maasTableShadowData[iCharIDX] = new Double[iXAxisPointCount, iYAxisPointCount];
                miCharCount = iCharIDX + 1;
                vCreateCharCompuMethodFormat(iCharIDX);
            }

            vSetGridHeaderLabels();
            Host.SetDataMinMaxRange(tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim, tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim);
            Host.SetData(maasTableShadowData[0], true);
            Host.SetChartColorPreferences(Color.Orange, Color.OrangeRed);
            Host.SetChartDrawingPreferences(MeshDrawMode.SolidWireFrame);

            string szGridTitle = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szCharacteristicName;

            int iXAxis = tclsASAM.milstCharacteristicList[maiCharIndices[0]].iXAxisRef;
            int iYAxis = tclsASAM.milstCharacteristicList[maiCharIndices[0]].iYAxisRef;

            if (-1 < iXAxis)
            {
                szAxisPtsRef = tclsASAM.milstAxisDescrList[iXAxis].szAxisPointsRef;
                iAxisPtsIDX = tclsASAM.iGetAxisPtsIndex(szAxisPtsRef);
                szGridTitle += " X-Axis: " + tclsASAM.milstAxisDescrList[iAxisPtsIDX].szVar + " (";
                szAxisCompuMethod = tclsASAM.milstAxisPtsList[iAxisPtsIDX].szCompuMethod;
                iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szAxisCompuMethod);
                szGridTitle += tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;
                szGridTitle += ")";
            }

            if (-1 < iYAxis)
            {
                szAxisPtsRef = tclsASAM.milstAxisDescrList[iYAxis].szAxisPointsRef;
                iAxisPtsIDX = tclsASAM.iGetAxisPtsIndex(szAxisPtsRef);
                szGridTitle += " Y-Axis: " + tclsASAM.milstAxisDescrList[iAxisPtsIDX].szVar + " (";
                szAxisCompuMethod = tclsASAM.milstAxisPtsList[iAxisPtsIDX].szCompuMethod;
                iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szAxisCompuMethod);
                szGridTitle +=  tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;
                szGridTitle += ")";
            }

            szGridTitle += " Output Units: ";
            iCompuMethodIDX = maiCharCompuMethodIndices[0];
            szGridTitle += tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;

            Host.SetGridTitle(szGridTitle);
            Host.SetXAxesData(mlstXAxisLabels[0]);

            if (0 < miYAxisPointCount[0])
            {
                Host.SetYAxesData(mlstYAxisLabels[0]);
            }

            Host.SetZAxesData(mlstZAxisLabels[0]);

            for (int iRowIDX = 0; iRowIDX < miYAxisPointCount[0]; iRowIDX++)
            {
                for (int iColIDX = 0; iColIDX < miXAxisPointCount[0]; iColIDX++)
                {
                    if (0 == (iRowIDX % 2))
                    {
                        //UpdateGridColourPreference(iColIDX, iRowIDX, 0.4f + 0.1f * (float)(iColIDX % 2));
                    }
                    else
                    {
                        //UpdateGridColourPreference(iColIDX, iRowIDX, 0.5f - 0.1f * (float)(iColIDX % 2));
                    }
                }
            }

            setVerticalDivide(miDivider, 10 - miDivider);
            Host.CellDataChanged += (s, e) => CellDataChanged(e.Row, e.Column, e.NewValue, e.OldValue);

            if (null == mclsTuningDisplay)
            {
                if (mszWindowLabel.Contains("Volumetric Efficiency"))
                {
                    mclsTuningDisplay = new tclsTuningTargetDisplay();

                    menTuningMode = tenTuningMode.enVE;
                    mclsTuningDisplay.SetLabels("TRGT:", "FDBACK:");
                    mclsTuningDisplay.SetScaling(2, 2);
                    mclsTuningDisplay.SetTarget(0, 0xffff, 0, 0xffff);
                    mclsTuningDisplay.MdiParent = Program.mFormUDP;
                    mclsTuningDisplay.Show();
                }
                else
                {
                    menTuningMode = tenTuningMode.enNone;
                }
            }
            else
            {
                if (mszWindowLabel.Contains("Volumetric Efficiency"))
                {
                    mclsTuningDisplay.Show();
                    menTuningMode = tenTuningMode.enVE;
                }
                else
                {
                    mclsTuningDisplay.Hide();
                    menTuningMode = tenTuningMode.enNone;
                }
            }
        }

        private void SetWindowLabel(String szSupplemental)
        {
            this.Text = mszWindowLabel + szSupplemental;
        }

        public void CellDataChanged(int iRowIDX, int iColIDX, double newVal, double OldVal)
        {
            UInt32 u32TableAddress;
            tenRecLayout enRecLayout;
            Double sCellDataRaw;
            bool boErr = false;
            bool boErrSequenceEnable = false;

            /* Is this change in sequence? */
            if ((miOldCellChangedErrColIDX != -1) && (miOldCellChangedErrRowIDX != -1))
            {
                boErrSequenceEnable = (miOldCellChangedErrColIDX == (iColIDX + 1)) || (miOldCellChangedErrRowIDX == (iRowIDX + 1)) ? true : false;
            }
            else
            {
                boErrSequenceEnable = true;
            }

            if (newVal > tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim)
            {
                newVal = tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim;

                /* Suppress more sequential cell errors */
                if (true == boErrSequenceEnable)
                {
                    Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0,
                        "New value/s too high, now set to " + newVal);
                }

                boErr = true;
            }

            if (newVal < tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim)
            {
                newVal = tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim;

                /* Suppress more sequential cell errors */
                if (true == boErrSequenceEnable)
                {
                    Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0,
                        "New value/s too low, now set to " + newVal);
                }

                boErr = true;
            }

            maasTableShadowData[0][iColIDX, iRowIDX] = newVal;

            if (false == mboInitialising)
            {
                if (-1 < maiCharCompuMethodIndices[0])
                {
                    switch (tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].enCM_Type)
                    {
                        case tenCM_Type.enLINEAR:
                            {
                                sCellDataRaw = newVal - tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff2;
                                sCellDataRaw /= tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff1;
                                break;
                            }
                        default:
                            {
                                sCellDataRaw = newVal;
                                break;
                            }
                    }
                }
                else
                {
                    sCellDataRaw = newVal;
                }

                u32TableAddress = tclsASAM.milstCharacteristicList[maiCharIndices[0]].u32Address;
                enRecLayout = tclsASAM.milstCharacteristicList[maiCharIndices[0]].enRecLayout;

                switch (enRecLayout)
                {
                    case tenRecLayout.enRL_VALU8:
                        {
                            byte u8Data = Convert.ToByte(sCellDataRaw);
                            tclsDataPage.u8SetWorkingData(u32TableAddress + (UInt32)iRowIDX + (UInt32)iColIDX * (UInt32)miYAxisPointCount[0], u8Data);
                            break;
                        }
                    case tenRecLayout.enRL_VALU16:
                        {
                            UInt16 u16Data = Convert.ToUInt16(sCellDataRaw);

                            tclsDataPage.u16SetWorkingData(u32TableAddress + 2u * (UInt32)iRowIDX + 2u * (UInt32)iColIDX * (UInt32)miYAxisPointCount[0], u16Data);
                            break;
                        }
                    case tenRecLayout.enRL_VALU32:
                        {
                            UInt32 u32Data = Convert.ToUInt32(sCellDataRaw);
                            tclsDataPage.u32SetWorkingData(u32TableAddress + 4 * (UInt32)iRowIDX + 4u * (UInt32)iColIDX * (UInt32)miYAxisPointCount[0], u32Data);
                            break;
                        }
                    case tenRecLayout.enRL_VALS8:
                        {
                            SByte s8Data = Convert.ToSByte(sCellDataRaw);
                            tclsDataPage.s8SetWorkingData(u32TableAddress + (UInt32)iRowIDX + (UInt32)iColIDX * (UInt32)miYAxisPointCount[0], s8Data);
                            break;
                        }
                    case tenRecLayout.enRL_VALS16:
                        {
                            Int16 s16Data = Convert.ToInt16(sCellDataRaw);
                            tclsDataPage.s16SetWorkingData(u32TableAddress + 2 * (UInt32)iRowIDX + 2u * (UInt32)iColIDX * (UInt32)miYAxisPointCount[0], s16Data);
                            break;
                        }
                    case tenRecLayout.enRL_VALS32:
                        {
                            Int32 s32Data = Convert.ToInt32(sCellDataRaw);
                            tclsDataPage.s32SetWorkingData(u32TableAddress + 4 * (UInt32)iRowIDX + 4u * (UInt32)iColIDX * (UInt32)miYAxisPointCount[0], s32Data);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            if (true == boErr)
            {
                miOldCellChangedErrColIDX = iColIDX;
                miOldCellChangedErrRowIDX = iRowIDX;
            }
            else
            {
                miOldCellChangedErrColIDX = -1;
                miOldCellChangedErrRowIDX = -1;
            }
        }

        ISurfaceChart Host { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            Host.SetChartColorPreferences(RandomColor(), RandomColor());
        }

        Random rand = new Random();
        private Color RandomColor()
        {
            return Color.FromArgb(255, rand.Next(255), rand.Next(255), rand.Next(255));
        }

        private void vCreateCharCompuMethodFormat(int iCharIDX)
        {
            String szFormatString = "{0:";

            if (0 < tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharIDX]].iPreDPCount)
            {
                szFormatString +=
                   new String('0',
                   tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[iCharIDX]].iPreDPCount);
            }
            else
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

        private void vSetGridHeaderLabels()
        {
            for (int iCharIDX = 0; iCharIDX < miCharCount; iCharIDX++)
            {
                mlstXAxisLabels[iCharIDX].Clear();
                mlstYAxisLabels[iCharIDX].Clear();

                for (int iGridColIDX = 0; iGridColIDX < miXAxisPointCount[iCharIDX]; iGridColIDX++)
                {
                    string szXColHeader = szGetAxisPoint(iCharIDX, iGridColIDX, false);
                    mlstXAxisLabels[iCharIDX].Add(szXColHeader);
                }

                for (int iGridColIDX = 0; iGridColIDX < miYAxisPointCount[iCharIDX]; iGridColIDX++)
                {
                    string szYColHeader = szGetAxisPoint(iCharIDX, iGridColIDX, true);
                    mlstYAxisLabels[iCharIDX].Add(szYColHeader);
                }

                vCreateZAxisPoints(iCharIDX);
            }
        }

        private void vCreateZAxisPoints(int iCharIDX)
        {
            mlstZAxisLabels[iCharIDX].Clear();

            String szFormatString = "{0:";
            int iCompuMethodIDX = maiCharCompuMethodIndices[iCharIDX];

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

            Double range = tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim;
            Double step = range / 10;

            for (int iZStepIDX = 0; iZStepIDX < 11; iZStepIDX++)
            {
                string szAxisPoint = String.Format(szFormatString, tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim + iZStepIDX * step);
                szAxisPoint += " ";
                szAxisPoint += tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;
                mlstZAxisLabels[iCharIDX].Add(szAxisPoint);
            }
        }

        private string szGetAxisPoint(int iGridIDX, int iGridColIDX, bool boYAxis)
        {
            string szAxisPoint = "-";
            string szAxisPtsRef;
            string szAxisCompuMethod;
            int iAxisPtsIDX;
            int iCompuMethodIDX;
            UInt32 u32AxisPtsAddress;

            int iAxis = (false == boYAxis) ? tclsASAM.milstCharacteristicList[maiCharIndices[iGridIDX]].iXAxisRef :
                tclsASAM.milstCharacteristicList[maiCharIndices[iGridIDX]].iYAxisRef;

            if (-1 < iAxis)
            {
                szAxisPtsRef = tclsASAM.milstAxisDescrList[iAxis].szAxisPointsRef;
                iAxisPtsIDX = tclsASAM.iGetAxisPtsIndex(szAxisPtsRef);
                u32AxisPtsAddress = tclsASAM.milstAxisPtsList[iAxisPtsIDX].u32Address;
                szAxisCompuMethod = tclsASAM.milstAxisPtsList[iAxisPtsIDX].szCompuMethod;
                iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(szAxisCompuMethod);

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

                            u32AxisPtsAddress += (4 * (UInt32)iGridColIDX);
                            tclsDataPage.u16GetWorkingData(u32AxisPtsAddress, ref u16AxisPointData);

                            if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enLINEAR)
                            {
                                sScaledAxisPointData = u16AxisPointData * tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff1
                            + tclsASAM.milstCompuMethodList[iCompuMethodIDX].sCoeff2;
                            }

                            szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            szAxisPoint += " ";
                            szAxisPoint += tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;
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
                                szAxisPoint += " ";
                                szAxisPoint += tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;
                            }
                            else if (tclsASAM.milstCompuMethodList[iCompuMethodIDX].enCM_Type == tenCM_Type.enIDENTICAL)
                            {
                                sScaledAxisPointData = (Single)u32AxisPointData;

                                szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                                szAxisPoint += " ";
                                szAxisPoint += tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;
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
                            }

                            szAxisPoint = String.Format(szFormatString, sScaledAxisPointData);
                            szAxisPoint += " ";
                            szAxisPoint += tclsASAM.milstCompuMethodList[iCompuMethodIDX].szUnitsString;
                            break;
                        }
                }
            }

            return szAxisPoint;
        }

        public void vRequestCalibrationWrite()
        {
            int iIDX = 0;

            while (iIDX < maiCharIndices.Length)
            {
                if (-1 != maiCharIndices[iIDX])
                {
                    Program.vUpdateCalibrationFromDataPage(maiCharIndices[iIDX]);
                }
                iIDX++;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void vUpdateMeasures(List<UInt32> lstData, int iActiveMeasureIndex)
        {
            if (miFormBaseIDX == iActiveMeasureIndex)
            {
                if (5 == lstData.Count)
                {
                    mu16SpreadDividend2D = (UInt16)(lstData[2] & 0xffff);
                    mu16SpreadRemainder2D = (UInt16)((lstData[2] & 0xffff0000) >> 16);

                    mu16SpreadXDividend3D = (UInt16)(lstData[0] & 0xffff);
                    mu16SpreadXRemainder3D = (UInt16)((lstData[0] & 0xffff0000) >> 16);
                    mu16SpreadYDividend3D = (UInt16)(lstData[1] & 0xffff);
                    mu16SpreadYRemainder3D = (UInt16)((lstData[1] & 0xffff0000) >> 16);
                }

                CheckConfigCRC((UInt16)lstData[4], tclsDataPage.GetDataPageCRC16());

                if (null != mclsTuningDisplay)
                {
                    mclsTuningDisplay.SetValues((int)(lstData[3] >> 16),(int)(lstData[3] & 0xffff));
                    miTuningRetVal = mclsTuningDisplay.SetTarget(mu16SpreadXDividend3D, mu16SpreadXRemainder3D, mu16SpreadYDividend3D, mu16SpreadYRemainder3D);
                }
            }
        }

        private void CheckConfigCRC(UInt16 ECUCRC16, UInt16 LocalCRC16)
        {
            if (ECUCRC16 != LocalCRC16)
            {
                miBadCRCCount++;
                miGoodCRCCount = 0;

                tclsDataPage.vReportCRC16(false);

                if ((miBadCRCCount % 10) == 0)
                {
                    SetWindowLabel(" [CRC mismatch - upload now!]");
                }
            }
            else
            {
                miBadCRCCount = 0;
                miGoodCRCCount++;

                tclsDataPage.vReportCRC16(true);

                if ((miGoodCRCCount % 10) == 0)
                {
                    SetWindowLabel(" [CRC OK]");
                }
            }
        }

        public void vRefreshFromDataPage()
        {
            UInt32 u32Address;
            int iXAxisRef;
            int iYAxisRef;
            int iXAxisPoints;
            int iYAxisPoints;
            double sCellData;

            u32Address = tclsASAM.milstCharacteristicList[maiCharIndices[0]].u32Address;
            iXAxisRef = tclsASAM.milstCharacteristicList[maiCharIndices[0]].iXAxisRef;
            iXAxisPoints = tclsASAM.milstAxisDescrList[iXAxisRef].iAxisPointCount;
            iYAxisRef = tclsASAM.milstCharacteristicList[maiCharIndices[0]].iYAxisRef;
            if (-1 != iYAxisRef)
            {
                iYAxisPoints = tclsASAM.milstAxisDescrList[iYAxisRef].iAxisPointCount;
            }
            else
            {
                iYAxisPoints = 1;
            }

            switch (tclsASAM.milstCharacteristicList[maiCharIndices[0]].enRecLayout)
            {
                case tenRecLayout.enRL_VALU8:
                    {
                        break;
                    }
                case tenRecLayout.enRL_VALU16:
                    {
                        UInt16[] au16Data = new UInt16[iXAxisPoints * iYAxisPoints];
                        tclsDataPage.au16GetWorkingData(u32Address, ref au16Data);

                        for (int iRowIDX = 0; iRowIDX < iYAxisPoints; iRowIDX++)
                        {
                            for (int iColIDX = 0; iColIDX < iXAxisPoints; iColIDX++)
                            {
                                if (-1 != maiCharCompuMethodIndices[0])
                                {
                                    sCellData = tenCM_Type.enLINEAR == tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].enCM_Type ?
                                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff1 * (double)au16Data[iRowIDX * iYAxisPoints + iColIDX] +
                                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff2 : (double)au16Data[iRowIDX * iYAxisPoints + iColIDX];
                                }
                                else
                                {
                                    sCellData = (Single)au16Data[iRowIDX * iXAxisPoints + iColIDX];
                                }

                                if (1 < miYAxisPointCount[0])
                                {
                                    maasTableShadowData[0][iRowIDX, iColIDX] = sCellData;

                                    Invoke((MethodInvoker)delegate
                                    {
                                        UpdateGridColourPreference(iRowIDX, iColIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                                    });
                                }
                                else
                                {
                                    maasTableShadowData[0][iColIDX, iRowIDX] = sCellData;

                                    Invoke((MethodInvoker)delegate
                                    {
                                        UpdateGridColourPreference(iColIDX, iRowIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                                    });
                                }
                            }
                        }

                        break;
                    }
                case tenRecLayout.enRL_VALU32:
                    {
                        UInt32[] au32Data = new UInt32[iXAxisPoints * iYAxisPoints];
                        tclsDataPage.au32GetWorkingData(u32Address, ref au32Data);

                        for (int iRowIDX = 0; iRowIDX < iYAxisPoints; iRowIDX++)
                        {
                            for (int iColIDX = 0; iColIDX < iXAxisPoints; iColIDX++)
                            {
                                if (-1 != maiCharCompuMethodIndices[0])
                                {
                                    sCellData = tenCM_Type.enLINEAR == tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].enCM_Type ?
                                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff1 * (double)au32Data[iRowIDX * iYAxisPoints + iColIDX] +
                                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff2 : sCellData = (Single)au32Data[iRowIDX * iXAxisPoints + iColIDX];
                                }
                                else
                                {
                                    sCellData = (Single)au32Data[iRowIDX * iXAxisPoints + iColIDX];
                                }

                                if (1 < miYAxisPointCount[0])
                                {
                                    maasTableShadowData[0][iRowIDX, iColIDX] = sCellData;

                                    Invoke((MethodInvoker)delegate
                                    {
                                        UpdateGridColourPreference(iRowIDX, iColIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                                    });
                                }
                                else
                                {
                                    maasTableShadowData[0][iColIDX, iRowIDX] = sCellData;

                                    Invoke((MethodInvoker)delegate
                                    {
                                        UpdateGridColourPreference(iColIDX, iRowIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                                    });
                                }
                            }
                        }

                        break;
                    }
                case tenRecLayout.enRL_VALS8:
                    {
                        break;
                    }
                case tenRecLayout.enRL_VALS16:
                    {
                        Int16[] as16Data = new Int16[iXAxisPoints * iYAxisPoints];
                        tclsDataPage.as16GetWorkingData(u32Address, ref as16Data);

                        for (int iRowIDX = 0; iRowIDX < iYAxisPoints; iRowIDX++)
                        {
                            for (int iColIDX = 0; iColIDX < iXAxisPoints; iColIDX++)
                            {
                                if (-1 != maiCharCompuMethodIndices[0])
                                {
                                    sCellData = tenCM_Type.enLINEAR == tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].enCM_Type ?
                                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff1 * (double)as16Data[iRowIDX * iYAxisPoints + iColIDX] +
                                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff2 : (double)as16Data[iRowIDX * iYAxisPoints + iColIDX];
                                }
                                else
                                {
                                    sCellData = (Single)as16Data[iRowIDX * iXAxisPoints + iColIDX];
                                }

                                if (1 < miYAxisPointCount[0])
                                {
                                    maasTableShadowData[0][iRowIDX, iColIDX] = sCellData;

                                    Invoke((MethodInvoker)delegate
                                    {
                                        UpdateGridColourPreference(iRowIDX, iColIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                                    });
                                }
                                else
                                {
                                    maasTableShadowData[0][iColIDX, iRowIDX] = sCellData;

                                    Invoke((MethodInvoker)delegate
                                    {
                                        UpdateGridColourPreference(iColIDX, iRowIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                                    });
                                }
                            }
                        }

                        break;
                    }
                case tenRecLayout.enRL_VALS32:
                    {
                        Int32[] as32Data = new Int32[iXAxisPoints * iYAxisPoints];
                        tclsDataPage.as32GetWorkingData(u32Address, ref as32Data);

                        for (int iRowIDX = 0; iRowIDX < iYAxisPoints; iRowIDX++)
                        {
                            for (int iColIDX = 0; iColIDX < iXAxisPoints; iColIDX++)
                            {
                                if (-1 != maiCharCompuMethodIndices[0])
                                {
                                    sCellData = tenCM_Type.enLINEAR == tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].enCM_Type ?
                                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff1 * (double)as32Data[iRowIDX * iYAxisPoints + iColIDX] +
                                        tclsASAM.milstCompuMethodList[maiCharCompuMethodIndices[0]].sCoeff2 : (double)as32Data[iRowIDX * iYAxisPoints + iColIDX];
                                }
                                else
                                {
                                    sCellData = (Single)as32Data[iRowIDX * iXAxisPoints + iColIDX];
                                }

                                if (1 < miYAxisPointCount[0])
                                {
                                    maasTableShadowData[0][iRowIDX, iColIDX] = sCellData;

                                    Invoke((MethodInvoker)delegate
                                    {
                                        UpdateGridColourPreference(iRowIDX, iColIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                                    });
                                }
                                else
                                {
                                    maasTableShadowData[0][iColIDX, iRowIDX] = sCellData;

                                    Invoke((MethodInvoker)delegate
                                    {
                                        UpdateGridColourPreference(iColIDX, iRowIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                                    });
                                }
                            }
                        }                                    

                        break;
                    }

                default:
                    {
                        break;
                    }
            }


            vSetGridHeaderLabels();

            Invoke((MethodInvoker)delegate
            {
                Host.SetData(maasTableShadowData[0], false);
                Host.SetXAxesData(mlstXAxisLabels[0]);
                Host.SetYAxesData(mlstYAxisLabels[0]);
            });
        }

        private void UpdateCellColour()
        {
            Double sCellData;
            UInt16 u16SpreadXDividend3D = mu16SpreadXDividend3D;
            UInt16 u16SpreadYDividend3D = mu16SpreadYDividend3D;
            UInt16 u16SpreadDividend2D = mu16SpreadDividend2D;

            if (true == mboRequestShutdown) return;

            if (32768 <= mu16SpreadXRemainder3D)
            {
                u16SpreadXDividend3D++;
            }

            if (32768 <= mu16SpreadYRemainder3D)
            {
                u16SpreadYDividend3D++;
            }

            if (32768 <= mu16SpreadRemainder2D)
            {
                u16SpreadDividend2D++;
            }

            if ((false == mboIs3D) && (maasTableShadowData[0].GetLength(1) > u16SpreadDividend2D))
            {
                sCellData = maasTableShadowData[0][u16SpreadDividend2D,0];

                if (3 < miLiveCellWaitCount)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        Host.SetGridColorPreferences(Color.Yellow, Color.Black, u16SpreadDividend2D, 0);
                    });

                    if (((-1 != miOldCellColIDX) && (-1 != miOldCellRowIDX)) &&
                        (miOldCellColIDX != mu16SpreadDividend2D))
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            UpdateGridColourPreference(miOldCellColIDX, miOldCellRowIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                        });
                    }

                    miOldCellColIDX = u16SpreadDividend2D;
                    miOldCellRowIDX = 0;
                }
            }
            else if ((maasTableShadowData[0].GetLength(0) > u16SpreadXDividend3D) && (maasTableShadowData[0].GetLength(1) > u16SpreadYDividend3D))
            {
                sCellData = maasTableShadowData[0][u16SpreadXDividend3D, u16SpreadYDividend3D];

                if (3 < miLiveCellWaitCount)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        Host.SetGridColorPreferences(Color.Yellow, Color.Black, u16SpreadXDividend3D, u16SpreadYDividend3D);
                    });

                    if (((-1 != miOldCellColIDX) && (-1 != miOldCellRowIDX)) &&
                        ((miOldCellColIDX != u16SpreadXDividend3D) || (miOldCellRowIDX != u16SpreadYDividend3D)))
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            UpdateGridColourPreference(miOldCellColIDX, miOldCellRowIDX, ((Single)sCellData - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim) / ((Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim - (Single)tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim));
                        });
                    }

                    miOldCellColIDX = u16SpreadXDividend3D;
                    miOldCellRowIDX = u16SpreadYDividend3D;
                }
            }

            miLiveCellWaitCount++;
        }

        private void tclsMeasCurveMapView_Resize(object sender, System.EventArgs e)
        {

        }

        private void UpdateGridColourPreference(int col, int row, Single scaleValue)
        {
            if (0.15 > scaleValue)
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 206, 227, 253), Color.Black, col, row);
            }
            else if (0.25 > scaleValue)
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 157, 222, 247), Color.Black, col, row);
            }
            else if (0.35 > scaleValue)
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 156, 243, 248), Color.Black, col, row);
            }
            else if (0.45 > scaleValue)
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 155, 249, 211), Color.Black, col, row);
            }
            else if (0.55 > scaleValue)
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 154, 250, 166), Color.Black, col, row);
            }
            else if (0.65 > scaleValue)
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 219, 248, 153), Color.Black, col, row);
            }
            else if (0.75 > scaleValue)
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 249, 234, 151), Color.Black, col, row);
            }
            else if (0.85 > scaleValue)
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 255, 208, 166), Color.Black, col, row);
            }
            else 
            {
                Host.SetGridColorPreferences(Color.FromArgb(255, 252, 182, 214), Color.Black, col, row);
            }
        }

        private void SetSpreadAddresses()
        {
            UInt32 u32SpreadAddresses;

            if (true == mboIs3D)
            {
                u32SpreadAddresses = mu32Spread3DTableAddress;
            }
            else
            {
                u32SpreadAddresses = mu32Spread2DTableAddress;
            }

            int iAxisRef = tclsASAM.milstCharacteristicList[maiCharIndices[0]].iXAxisRef;

            if (-1 < iAxisRef)
            {
                string szAxisPtsRef = tclsASAM.milstAxisDescrList[iAxisRef].szAxisPointsRef;
                int iAxisPtsIDX = tclsASAM.iGetAxisPtsIndex(szAxisPtsRef);
                tstReg32Write stReg32Write = new tstReg32Write();
                stReg32Write.u32Address = u32SpreadAddresses;
                stReg32Write.u32Data = tclsASAM.milstAxisPtsList[iAxisPtsIDX].u32Address;
                tclsDataPage.AddReg32Write(stReg32Write);
            }

            iAxisRef = tclsASAM.milstCharacteristicList[maiCharIndices[0]].iYAxisRef;

            if (-1 < iAxisRef)
            {
                string szAxisPtsRef = tclsASAM.milstAxisDescrList[iAxisRef].szAxisPointsRef;
                int iAxisPtsIDX = tclsASAM.iGetAxisPtsIndex(szAxisPtsRef);
                tstReg32Write stReg32Write = new tstReg32Write();
                stReg32Write.u32Address = u32SpreadAddresses + 4;
                stReg32Write.u32Data = tclsASAM.milstAxisPtsList[iAxisPtsIDX].u32Address;
                tclsDataPage.AddReg32Write(stReg32Write);
            }

            if (mszWindowLabel.Contains("Volumetric Efficiency"))
            {
                int iMeasIDX = tclsASAM.iGetAvailableMeasIndex("VE AUTOTUNE DIAG");

                if (-1 != iMeasIDX)
                {
                    tstReg32Write stReg32Write = new tstReg32Write();
                    stReg32Write.u32Address = mu32SpecialTableAddress;
                    stReg32Write.u32Data = tclsASAM.milstAvailableMeasList[iMeasIDX].u32Address;
                    tclsDataPage.AddReg32Write(stReg32Write);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            miFormIDX = 3;

            SetWindowView();
            SetSpreadAddresses();

        }

        private void toolStripButtonPrevious_Click(object sender, EventArgs e)
        {
            GetPrevFormIDX();
            SetWindowView();
            SetSpreadAddresses();
            Host.ResetFocus();
            vRefreshFromDataPage();
        }

        private void toolStripButtonNext_Click(object sender, EventArgs e)
        {
            GetNextFormIDX();
            SetWindowView();
            SetSpreadAddresses();
            Host.ResetFocus();
            vRefreshFromDataPage();
        }

        public void RequestShowViewIndex(int iFormReqIDX)
        {
            foreach (int iFormIDX in mlstFormIndices)
            {
                if (iFormIDX == iFormReqIDX)
                {
                    Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);
                    miFormIDX = iFormIDX;
                    SetWindowView();
                    SetSpreadAddresses();
                    vRefreshFromDataPage();
                    Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);
                    break;
                }
            }
        }

        private void toolStripButtonUp_Click(object sender, EventArgs e)
        {
            miDivider = 1 < miDivider ? miDivider - 1 : 1;
            setVerticalDivide(miDivider, 10 - miDivider);
        }

        private void toolStripButtonDown_Click(object sender, EventArgs e)
        {
            miDivider = 9 > miDivider ? miDivider + 1 : 9;
            setVerticalDivide(miDivider, 10 - miDivider);
        }

        private void setVerticalDivide(int iTable, int iGrid)
        {
            Host.SetLayoutSize(iTable, iGrid);
        }

        private void toolStripButtonPlusMinus_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonMultiply_Click(object sender, EventArgs e)
        {

        }

        private void CurveMapToolstrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timerCellColour_Tick(object sender, EventArgs e)
        {
            UpdateCellColour();

            if (true == mboFocusActive)
            {
                if (3 < miTimerCount++)
                {
                    Host.CheckFocus();
                    miTimerCount = 0;
                }
            }
        }

        private void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }


        private void form_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void form_keyUp(object sender, KeyEventArgs e)
        {

        }

        private void toolStripButtonPlusMinus_Click_1(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.F9);
        }

        private void toolStripButtonMultiply_Click_1(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.F11);
        }

        private void toolStripButtonEquals_Click(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.F12);
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.ZOOM_IN);
        }

        private void toolStripButtonRotLeft_Click(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.ROT_LEFT);
        }

        private void toolStripButtonRotRight_Click(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.ROT_RIGHT);
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.ZOOM_OUT);
        }

        private void toolStripButtonSelectCol_Click(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.SEL_COL);
        }

        private void toolStripButtonSelectRow_Click(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.SEL_ROW);
        }

        private void toolStripButtonSelectTable_Click(object sender, EventArgs e)
        {
            Host.SetKeyDown(KeyDownCode.SEL_ALL);
        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            bool boHTMLShown = false;

            if (tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString.Contains("HTML"))
            {
                tclsHTMLHelp clsHTMLHelp;

                int iLen = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString.Length;
                int iPos = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString.IndexOf("HTML");
                String szHTMLResource = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString.Substring(iPos,iLen - iPos);

                while (szHTMLResource.Contains(" "))
                {
                    szHTMLResource = szHTMLResource.Replace(" ", "");
                }


                szHTMLResource = szHTMLResource.Replace("HTML=", "");
                iPos = szHTMLResource.IndexOf("HTML");
                szHTMLResource = szHTMLResource.Substring(0, iPos) + "HTML";

                String szXMLPath = AppDomain.CurrentDomain.BaseDirectory + "Database\\Help Databases\\" + szHTMLResource;

                if (File.Exists(szXMLPath))
                {
                    clsHTMLHelp = new tclsHTMLHelp(szXMLPath);
                    boHTMLShown = true;
                    clsHTMLHelp.ShowDialog();
                }
            }
            

            if (false == boHTMLShown)
            {
                int iPos = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString.IndexOf("HTML");
                String szShowString;

                if (-1 != iPos)
                {
                    szShowString = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString.Substring(1, iPos - 2);
                }
                else
                {
                    szShowString = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString;
                }

            tclsNotify clsNotify = new UDP.tclsNotify();

            clsNotify.Left = this.ClientRectangle.Width / 2;
            clsNotify.Top = this.ClientRectangle.Height / 2;
                clsNotify.vSetNotices("Map Help", szShowString);
            clsNotify.ShowDialog();
            }
        }

        private void FocusActivated(object sender, EventArgs e)
        {
            mboFocusActive = true;
        }

        private void tclsMeasCurveMapView_Deactivate(object sender, EventArgs e)
        {
            mboFocusActive = false;
        }

        private void timerAutoTune_Tick(object sender, EventArgs e)
        {
            miAutoTuneWait++;

            if ((-1 != miTuningRetVal) && (2 < miAutoTuneWait))
            {
                int iIndices = mclsTuningDisplay.GetIndices();
                int iXIDX = iIndices >> 16;
                int iYIDX = iIndices & 0xffff;
                double fOldVal;
                double fNewVal;

                if (true)
                {
                    fOldVal = maasTableShadowData[0][iXIDX, iYIDX];
                    fNewVal = fOldVal / (miTuningRetVal / 100f);

                    if (tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim >= fNewVal)
                    {
                        if (tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim >= fNewVal)
                        {
                            fNewVal = tclsASAM.milstCharacteristicList[maiCharIndices[0]].sLowerLim;
                        }
                    }
                    else
                    {
                        fNewVal = tclsASAM.milstCharacteristicList[maiCharIndices[0]].sUpperLim;
                    }

                    if (((int)(1000 * fOldVal)) != ((int)(1000 * fNewVal)))
                    {
                        AdjustAdjacentCells(iXIDX, iYIDX, 1.05f, 0.95f, fNewVal);

                        miAutoTuneWait = 0;
                    }
                }
            }
        }

        private void AdjustAdjacentCells(int iXIDX, int iYIDX, Single sRatioUp, Single sRatioDown, double fNewVal)
        {
            maasTableShadowData[0][iXIDX, iYIDX] = fNewVal;
            Host.SetDataByIndex(maasTableShadowData[0][iXIDX, iYIDX], iXIDX, iYIDX, false);

            /* Adjacent, X-1 */
            if (0 < iXIDX)
            {
                if (0 < iYIDX)
                {
                    if (fNewVal > maasTableShadowData[0][iXIDX - 1, iYIDX - 1])
                    {
                        /* New value is greater */
                        if (fNewVal > (sRatioUp * maasTableShadowData[0][iXIDX - 1, iYIDX - 1]))
                        {
                            maasTableShadowData[0][iXIDX - 1, iYIDX - 1] *= GetRatioTrim(iXIDX, iYIDX, -1, -1);
                            Host.SetDataByIndex(maasTableShadowData[0][iXIDX - 1, iYIDX - 1], iXIDX - 1, iYIDX - 1, false);
                        }
                    }
                    else
                    {
                        /* New value is less */
                        if (fNewVal < (sRatioDown * maasTableShadowData[0][iXIDX - 1, iYIDX - 1]))
                        {
                            maasTableShadowData[0][iXIDX - 1, iYIDX - 1] *= GetRatioTrim(iXIDX, iYIDX, -1, -1);
                            Host.SetDataByIndex(maasTableShadowData[0][iXIDX - 1, iYIDX - 1], iXIDX - 1, iYIDX - 1, false);
                        }
                    }
                }

                if (fNewVal > maasTableShadowData[0][iXIDX - 1, iYIDX])
                {
                    /* New value is greater */
                    if (fNewVal > (sRatioUp * maasTableShadowData[0][iXIDX - 1, iYIDX]))
                    {
                        maasTableShadowData[0][iXIDX - 1, iYIDX] *= GetRatioTrim(iXIDX, iYIDX, -1, 0);
                        Host.SetDataByIndex(maasTableShadowData[0][iXIDX - 1, iYIDX], iXIDX - 1, iYIDX, false);
                    }
                }
                else
                {
                    /* New value is less */
                    if (fNewVal < (sRatioDown * maasTableShadowData[0][iXIDX - 1, iYIDX]))
                    {
                        maasTableShadowData[0][iXIDX - 1, iYIDX] *= GetRatioTrim(iXIDX, iYIDX, -1, 0);
                        Host.SetDataByIndex(maasTableShadowData[0][iXIDX - 1, iYIDX], iXIDX - 1, iYIDX, false);
                    }
                }

                if ((maasTableShadowData[0].GetLength(1) - 1) > iYIDX)
                {
                    if (fNewVal > maasTableShadowData[0][iXIDX - 1, iYIDX + 1])
                    {
                        /* New value is greater */
                        if (fNewVal > (sRatioUp * maasTableShadowData[0][iXIDX - 1, iYIDX + 1]))
                        {
                            maasTableShadowData[0][iXIDX - 1, iYIDX + 1] *= GetRatioTrim(iXIDX, iYIDX, -1, 1);
                            Host.SetDataByIndex(maasTableShadowData[0][iXIDX - 1, iYIDX + 1], iXIDX - 1, iYIDX + 1, false);
                        }
                    }
                    else
                    {
                        /* New value is less */
                        if (fNewVal < (sRatioDown * maasTableShadowData[0][iXIDX - 1, iYIDX + 1]))
                        {
                            maasTableShadowData[0][iXIDX - 1, iYIDX + 1] *= GetRatioTrim(iXIDX , iYIDX, - 1, 1);
                            Host.SetDataByIndex(maasTableShadowData[0][iXIDX - 1, iYIDX + 1], iXIDX - 1, iYIDX + 1, false);
                        }
                    }
                }
            }


            /* Adjacent same X */
            if (0 < iYIDX)
            {
                if (fNewVal > maasTableShadowData[0][iXIDX, iYIDX - 1])
                {
                    /* New value is greater */
                    if (fNewVal > (sRatioUp * maasTableShadowData[0][iXIDX, iYIDX - 1]))
                    {
                        maasTableShadowData[0][iXIDX, iYIDX - 1] *= GetRatioTrim(iXIDX, iYIDX, 0, -1);
                        Host.SetDataByIndex(maasTableShadowData[0][iXIDX, iYIDX - 1], iXIDX, iYIDX - 1, false);
                    }
                }
                else
                {
                    /* New value is less */
                    if (fNewVal < (sRatioDown * maasTableShadowData[0][iXIDX, iYIDX - 1]))
                    {
                        maasTableShadowData[0][iXIDX, iYIDX - 1] *= GetRatioTrim(iXIDX, iYIDX, 0, -1);
                        Host.SetDataByIndex(maasTableShadowData[0][iXIDX, iYIDX - 1], iXIDX, iYIDX - 1, false);
                    }
                }
            }

            if ((maasTableShadowData[0].GetLength(1) - 1) > iYIDX)
            {
                if (fNewVal > maasTableShadowData[0][iXIDX, iYIDX + 1])
                {
                    /* New value is greater */
                    if (fNewVal > (sRatioUp * maasTableShadowData[0][iXIDX, iYIDX + 1]))
                    {
                        maasTableShadowData[0][iXIDX, iYIDX + 1] *= GetRatioTrim(iXIDX, iYIDX, 0, 1);
                        Host.SetDataByIndex(maasTableShadowData[0][iXIDX, iYIDX + 1], iXIDX, iYIDX + 1, false);
                    }
                }
                else
                {
                    /* New value is less */
                    if (fNewVal < (sRatioDown * maasTableShadowData[0][iXIDX, iYIDX + 1]))
                    {
                        maasTableShadowData[0][iXIDX, iYIDX + 1] *= GetRatioTrim(iXIDX, iYIDX, 0, 1);
                        Host.SetDataByIndex(maasTableShadowData[0][iXIDX, iYIDX + 1], iXIDX, iYIDX + 1, false);
                    }
                }
            }



            /* Adjacent, X+1 */
            if (maasTableShadowData[0].GetLength(1) > iXIDX)
            {
                if (0 < iYIDX)
                {
                    if (fNewVal > maasTableShadowData[0][iXIDX + 1, iYIDX - 1])
                    {
                        /* New value is greater */
                        if (fNewVal > (sRatioUp * maasTableShadowData[0][iXIDX + 1, iYIDX - 1]))
                        {
                            maasTableShadowData[0][iXIDX + 1, iYIDX - 1] *= GetRatioTrim(iXIDX, iYIDX, 1, -1);
                            Host.SetDataByIndex(maasTableShadowData[0][iXIDX + 1, iYIDX - 1], iXIDX + 1, iYIDX - 1, false);
                        }
                    }
                    else
                    {
                        /* New value is less */
                        if (fNewVal < (sRatioDown * maasTableShadowData[0][iXIDX + 1, iYIDX - 1]))
                        {
                            maasTableShadowData[0][iXIDX + 1, iYIDX - 1] *= GetRatioTrim(iXIDX, iYIDX, 1, -1);
                            Host.SetDataByIndex(maasTableShadowData[0][iXIDX - 1, iYIDX - 1], iXIDX + 1, iYIDX - 1, false);
                        }
                    }
                }

                if (fNewVal > maasTableShadowData[0][iXIDX + 1, iYIDX])
                {
                    /* New value is greater */
                    if (fNewVal > (sRatioUp * maasTableShadowData[0][iXIDX + 1, iYIDX]))
                    {
                        maasTableShadowData[0][iXIDX + 1, iYIDX] *= GetRatioTrim(iXIDX, iYIDX, 1, 0);
                        Host.SetDataByIndex(maasTableShadowData[0][iXIDX + 1, iYIDX], iXIDX + 1, iYIDX, false);
                    }
                }
                else
                {
                    /* New value is less */
                    if (fNewVal < (sRatioDown * maasTableShadowData[0][iXIDX + 1, iYIDX]))
                    {
                        maasTableShadowData[0][iXIDX + 1, iYIDX] *= GetRatioTrim(iXIDX, iYIDX, 1, 0);
                        Host.SetDataByIndex(maasTableShadowData[0][iXIDX + 1, iYIDX], iXIDX + 1, iYIDX, false);
                    }
                }

                if ((maasTableShadowData[0].GetLength(1) - 1) > iYIDX)
                {
                    if (fNewVal > maasTableShadowData[0][iXIDX + 1, iYIDX + 1])
                    {
                        /* New value is greater */
                        if (fNewVal > (sRatioUp * maasTableShadowData[0][iXIDX + 1, iYIDX + 1]))
                        {
                            maasTableShadowData[0][iXIDX + 1, iYIDX + 1] *= GetRatioTrim(iXIDX, iYIDX, 1, 1);
                            Host.SetDataByIndex(maasTableShadowData[0][iXIDX + 1, iYIDX + 1], iXIDX + 1, iYIDX + 1, false);
                        }
                    }
                    else
                    {
                        /* New value is less */
                        if (fNewVal < (sRatioDown * maasTableShadowData[0][iXIDX + 1, iYIDX + 1]))
                        {
                            maasTableShadowData[0][iXIDX + 1, iYIDX + 1] *= GetRatioTrim(iXIDX, iYIDX, 1, 1);
                            Host.SetDataByIndex(maasTableShadowData[0][iXIDX + 1, iYIDX + 1], iXIDX + 1, iYIDX + 1, false);
                        }
                    }
                }
            }



        }

        private Single GetRatioTrim(int iXIDX, int iYIDX, int iXOffset, int iYOffset)
        {
            Single fRatio = 1.00f;

            if (maasTableShadowData[0][iXIDX, iYIDX] > maasTableShadowData[0][iXIDX + iXOffset, iYIDX + iYOffset])
            {
                fRatio = 1.01f;
            }
            else
            {
                fRatio = 0.99f;
            }


            return fRatio;
        }

        public void vRequestShutdown()
        {
            mboRequestShutdown = true;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (true == mboRequestShutdown)
            {
                timerCellColour.Enabled = false;
            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("View minimised to the bottom", "Not possible to close view");
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void CurveMapToolstrip_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButtonAxes_Click(object sender, EventArgs e)
        {
            int iXAxisRef = tclsASAM.milstCharacteristicList[maiCharIndices[0]].iXAxisRef;
            int iYAxisRef = tclsASAM.milstCharacteristicList[maiCharIndices[0]].iYAxisRef;

            tclsAxesEdit mclsAxesEdit = new tclsAxesEdit(iXAxisRef, iYAxisRef, miXAxisRef[0], miYAxisRef[0], true);

            mclsAxesEdit.ShowDialog();
        }
    }
}



