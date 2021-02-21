/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Logging View                                           */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsMeasValueLoggingView.cs                            */
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
using GraphLib;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace UDP
{
    public partial class tclsMeasValueLoggingView : Form
    {
        tclsWindowElement[] maclsWindowElement;
        tstActiveMeasureIndices[] mastActiveMeasureIndices;
        int[] maiMeasCompuMethodIndices;
        String[] maszMeasCompuMethodFormat;
        bool mboInitialising;
        bool mboLogging;
        bool mboPCTimeStamps;
        bool mboRequestShutdown;
        int miTimeStampIDX;
        int miFormIDX;
        int miLoggingIDX;
        UInt32[,] maau32LogData;
        UInt32 mu32GlobalLogPCMsStart;
        UInt32 mu32GlobalLogPCMs;
        UInt32 mu32GlobalLogMeasureMsStart;
        UInt32 mu32GlobalLogMeasureMs;
        OxyPlot.WindowsForms.PlotView mclsPlotLogging;
        OxyPlot.Axes.LinearAxis mstXAxis;
        OxyPlot.Axes.LinearAxis[] mastYAxes;
        OxyColor[] maenLogColours;
        OxyPlot.PlotModel mclsPlotModel;
        int miZoomForwardIDX;
        int miZoomBackIDX;
        int miZoomForward;
        int miZoomBack;
        LineSeries[] seriesdata;
        DataTable mDataTable;
        int miMeasureCount;
        Color[] maclsLoggingColor;
        UInt32 mu32TableRefreshCount;
        UInt32 mu32TableRefreshCountOld;
        int[] maiZoom = { 5, 10, 25, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 25000, 50000, 100000, 200000, 500000, 1000000, 2000000, 4000000 };
        bool mboSuspendUpdates;
        bool mboOldSuspendUpdates;
        Single[,] maafMinMaxLogData;
        bool[] mboDrawBold;
        Single mfSplitterPosition;

        public tclsMeasValueLoggingView(int iFormIDX)
        {
            int iMeasureIDX = 0;
            int iElementIDX;
            tstActiveMeasureIndices stActiveMeasureIndices;

            InitializeComponent();

            comboBoxLoggingVars.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxLoggingVars.DrawItem += new DrawItemEventHandler(comboBoxLoggingVars_DrawItem);

            miFormIDX = iFormIDX;
            vResetLogging();
            mfSplitterPosition = 70;
            mboDrawBold = new bool[tclsASAM.milstAvailableMeasList.Count];

            maclsWindowElement = new tclsWindowElement[Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX].Count];

            Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX].CopyTo(maclsWindowElement, 0);

            /* Count number of elements assigned to this window */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "measure"))
                {
                    iMeasureIDX++;
                }
            }

            miMeasureCount = iMeasureIDX;
            maszMeasCompuMethodFormat = new String[iMeasureIDX];
            mastActiveMeasureIndices = new tstActiveMeasureIndices[iMeasureIDX];
            maiMeasCompuMethodIndices = new int[iMeasureIDX];
            maau32LogData = new UInt32[65536, iMeasureIDX];
            maafMinMaxLogData = new Single[iMeasureIDX, 2];
            iMeasureIDX = 0;

            /* Loop through and create visual controls required */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "measure"))
                {
                    stActiveMeasureIndices = tclsASAM.stTryAddActiveMeas(maclsWindowElement[iElementIDX].szA2LName, iFormIDX);
                    mastActiveMeasureIndices[iMeasureIDX].iMeasureQueue = stActiveMeasureIndices.iMeasureQueue;
                    mastActiveMeasureIndices[iMeasureIDX].iMeasureQueueIDX = stActiveMeasureIndices.iMeasureQueueIDX;
                    maiMeasCompuMethodIndices[iMeasureIDX] = tclsASAM.iGetCompuMethodIndexFromMeas(maclsWindowElement[iElementIDX].szA2LName, iFormIDX);

                    if (true == maclsWindowElement[iElementIDX].szA2LName.Contains("TimeTick"))
                    {
                        if (1 == iElementIDX)
                        {
                            miTimeStampIDX = iElementIDX - 1;
                            mboPCTimeStamps = false;
                        }
                        else
                        {
                            MessageBox.Show("A TimeTick measure was found, but it is not the first in the measure list and will not be used for graphic X-Index", "Important");
                        }
                    }

                    vCreateMeasCompuMethodFormat(iMeasureIDX);
                    iMeasureIDX++;
                }

                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "window"))
                {
                    this.Text = ConstantData.APPCONFIG.szAppName + ":" + maclsWindowElement[iElementIDX].szLabel;
                }
            }

            vLoadColours();
            vLoadPlotView();

            tclsASAM.vCalcDDDIAndSetRate(iFormIDX, 1);

            LoadComboBoxLoggingVars();
            vSetupDataGrid();
        }

        private void vLoadColours()
        {
            maenLogColours = new OxyColor[10];
            maenLogColours[0] = OxyColors.Aqua;
            maenLogColours[1] = OxyColors.Yellow;
            maenLogColours[2] = OxyColors.Orange;
            maenLogColours[3] = OxyColors.Green;
            maenLogColours[4] = OxyColors.Beige;
            maenLogColours[5] = OxyColors.Pink;
            maenLogColours[6] = OxyColors.Purple;
            maenLogColours[7] = OxyColors.DarkGray;
            maenLogColours[8] = OxyColors.SlateBlue;
            maenLogColours[9] = OxyColors.White;
        }


        private void vResetLogging()
        {
            miLoggingIDX = 0;
            mboLogging = true;
            mboPCTimeStamps = true;
            miTimeStampIDX = -1;
            mu32GlobalLogMeasureMsStart = 0xffffffff;
            mu32GlobalLogPCMs = 0;
        }

        private bool boAddMeasure(string szMeasure)
        {
            bool ret_val = true;
            tstActiveMeasureIndices stActiveMeasureIndices;

            stActiveMeasureIndices = tclsASAM.stTryAddActiveMeas(szMeasure, miFormIDX);

            if (-1 != stActiveMeasureIndices.iMeasureQueue)
            {
                Array.Resize(ref mastActiveMeasureIndices, mastActiveMeasureIndices.Length + 1);
                mastActiveMeasureIndices[mastActiveMeasureIndices.Length - 1].iMeasureQueue = stActiveMeasureIndices.iMeasureQueue;
                mastActiveMeasureIndices[mastActiveMeasureIndices.Length - 1].iMeasureQueueIDX = stActiveMeasureIndices.iMeasureQueueIDX;

                Array.Resize(ref maiMeasCompuMethodIndices, maiMeasCompuMethodIndices.Length + 1);
                maiMeasCompuMethodIndices[maiMeasCompuMethodIndices.Length - 1] = tclsASAM.iGetCompuMethodIndexFromMeas(szMeasure, miFormIDX);

                Array.Resize(ref maszMeasCompuMethodFormat, maszMeasCompuMethodFormat.Length + 1);
                vCreateMeasCompuMethodFormat(stActiveMeasureIndices.iMeasureQueueIDX);
                tclsASAM.vSetDDDIResetPendingIDX(miFormIDX);
                tclsASAM.vCalcDDDIAndSetRate(miFormIDX, 1);
            }

            return ret_val;
        }

        private bool boRemoveMeasure(string szMeasure)
        {
            bool ret_val = true;
            tstActiveMeasureIndices stRemovedMeasureIndices;
            int iQueueIDX;

            stRemovedMeasureIndices = tclsASAM.stTryRemoveActiveMeas(szMeasure, miFormIDX);

            if (-1 != stRemovedMeasureIndices.iMeasureQueue)
            {
                for (iQueueIDX = stRemovedMeasureIndices.iMeasureQueueIDX; iQueueIDX < (mastActiveMeasureIndices.Length - 1); iQueueIDX++)
                {
                    //mastActiveMeasureIndices[iQueueIDX] = mastActiveMeasureIndices[iQueueIDX + 1];
                    maiMeasCompuMethodIndices[iQueueIDX] = maiMeasCompuMethodIndices[iQueueIDX + 1];
                    maszMeasCompuMethodFormat[iQueueIDX] = maszMeasCompuMethodFormat[iQueueIDX + 1];
                }

                Array.Resize(ref mastActiveMeasureIndices, mastActiveMeasureIndices.Length - 1);
                Array.Resize(ref maiMeasCompuMethodIndices, maiMeasCompuMethodIndices.Length - 1);
                Array.Resize(ref maszMeasCompuMethodFormat, maszMeasCompuMethodFormat.Length - 1);
                tclsASAM.vSetDDDIResetPendingIDX(miFormIDX);
                tclsASAM.vCalcDDDIAndSetRate(miFormIDX, 1);
            }

            return ret_val;
        }

        private void vLoadPlotViewNewMeasure()
        {
            int iNewIndex;

            Array.Resize(ref seriesdata, mastActiveMeasureIndices.Length);
            Array.Resize(ref mastYAxes, mastActiveMeasureIndices.Length);
            ResizeArray(ref maau32LogData, 65536, mastActiveMeasureIndices.Length);
            ResizeArray(ref maafMinMaxLogData, mastActiveMeasureIndices.Length, 2);

            iNewIndex = mastActiveMeasureIndices.Length - 1;
            seriesdata[iNewIndex] = new LineSeries();
            seriesdata[iNewIndex].Points.Add(new DataPoint(0, 0));
            seriesdata[iNewIndex].Color = maenLogColours[mastActiveMeasureIndices.Length % 10];

            Single fMax;
            Single fMin;
            string szVarName;
            string szUnits;

            fMax = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iNewIndex].iMeasureQueueIDX].sUpperLim;
            fMin = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iNewIndex].iMeasureQueueIDX].sLowerLim;
            szVarName = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iNewIndex].iMeasureQueueIDX].szMeasurementName;
            szUnits = tclsASAM.milstCompuMethodList[maiMeasCompuMethodIndices[iNewIndex]].szUnitsString;
        }


        private void vLoadPlotView()
        {
            tableLayoutPanelLogging.Controls.Remove(mclsPlotLogging);

            if (maau32LogData.GetLength(1) != mastActiveMeasureIndices.Length)
            {
                ResizeArray(ref maau32LogData, 65536, mastActiveMeasureIndices.Length);
            }

            if (maafMinMaxLogData.GetLength(0) != mastActiveMeasureIndices.Length)
            {
                ResizeArray(ref maafMinMaxLogData, mastActiveMeasureIndices.Length, 2);
            }

            seriesdata = new LineSeries[mastActiveMeasureIndices.Length];
            mastYAxes = new LinearAxis[mastActiveMeasureIndices.Length];

            mclsPlotLogging = new OxyPlot.WindowsForms.PlotView();
            mclsPlotLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            mclsPlotLogging.Location = new System.Drawing.Point(0, 0);
            mclsPlotLogging.Margin = new System.Windows.Forms.Padding(0);
            mclsPlotLogging.Name = "plot1";
            mclsPlotLogging.PanCursor = System.Windows.Forms.Cursors.Hand;
            mclsPlotLogging.Size = new System.Drawing.Size(632, 446);
            mclsPlotLogging.TabIndex = 0;
            mclsPlotLogging.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            mclsPlotLogging.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            mclsPlotLogging.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;

            miZoomForwardIDX = 6;
            miZoomBackIDX = 6;
            miZoomForward = maiZoom[miZoomForwardIDX];
            miZoomBack = maiZoom[miZoomBackIDX];

            for (int iSeriesIDX = 0; iSeriesIDX < mastActiveMeasureIndices.Length; iSeriesIDX++)
            {
                seriesdata[iSeriesIDX] = new LineSeries();
                seriesdata[iSeriesIDX].Points.Add(new DataPoint(0, 0));
                seriesdata[iSeriesIDX].Color = maenLogColours[iSeriesIDX];

                Single fMax;
                Single fMin;
                string szVarName;
                string szUnits;

                fMax = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iSeriesIDX].iMeasureQueueIDX].sUpperLim;
                fMin = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iSeriesIDX].iMeasureQueueIDX].sLowerLim;
                szVarName = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iSeriesIDX].iMeasureQueueIDX].szMeasurementName;
                szUnits = tclsASAM.milstCompuMethodList[maiMeasCompuMethodIndices[iSeriesIDX]].szUnitsString;

                mastYAxes[iSeriesIDX] = pstCreateNewYAxis(iSeriesIDX, fMax, 0, szVarName, szUnits, maenLogColours[iSeriesIDX]);
                seriesdata[iSeriesIDX].YAxisKey = "YAxis" + iSeriesIDX.ToString();

                maafMinMaxLogData[iSeriesIDX, 0] = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iSeriesIDX].iMeasureQueueIDX].sUpperLim;
            }

            mclsPlotModel = new PlotModel
            {
                PlotType = PlotType.Cartesian,
                Background = OxyColors.Black,
                TextColor = OxyColors.Aqua
            };

            mstXAxis = new OxyPlot.Axes.LinearAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                AbsoluteMaximum = 3600000,
                AbsoluteMinimum = 0,
                MinorStep = 5,
                Unit = "s",
                TicklineColor = OxyColors.Aqua,
                TitleColor = OxyColors.Aqua,
                AxislineColor = OxyColors.Aqua,
                ExtraGridlineColor = OxyColors.Aqua,
                MajorGridlineColor = OxyColors.Aqua,
                TitleFontWeight = FontWeights.Bold
            };

            mstXAxis.Zoom(0f, 30f);
            mclsPlotModel.Axes.Add(mstXAxis);

            for (int iSeriesIDX = 0; iSeriesIDX < mastActiveMeasureIndices.Length; iSeriesIDX++)
            {
                if (iSeriesIDX != miTimeStampIDX)
                {
                    mclsPlotModel.Series.Add(seriesdata[iSeriesIDX]);
                    mclsPlotModel.Axes.Add(mastYAxes[iSeriesIDX]);
                }
            }

            mclsPlotLogging.Model = mclsPlotModel;
            miLoggingIDX = 1;

            tableLayoutPanelLogging.Controls.Add(mclsPlotLogging, 0, 1);
            tableLayoutPanelLogging.SetColumnSpan(mclsPlotLogging, 9);
            tableLayoutPanelLogging.SetRowSpan(mclsPlotLogging, 1);

            LoadComboBoxLoggingVars();
        }

        private void vSetupDataGrid()
        {
            mDataTable = new DataTable();

            mDataTable.Columns.Add("Measure Name", typeof(String));
            mDataTable.Columns.Add("Value Min", typeof(Single));
            mDataTable.Columns.Add("Value Max", typeof(Single));
            mDataTable.Columns.Add("Value Highest", typeof(Single));
            mDataTable.Columns.Add("Value Lowest", typeof(Single));
            mDataTable.Columns.Add("Value Current", typeof(Single));
            mDataTable.Columns.Add("Units", typeof(String));

            for (int iSeriesIDX = 0; iSeriesIDX < mastActiveMeasureIndices.Length; iSeriesIDX++)
            {
                Single fMax;
                Single fMin;
                string szVarName;
                string szUnits;

                fMax = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iSeriesIDX].iMeasureQueueIDX].sUpperLim;
                fMin = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iSeriesIDX].iMeasureQueueIDX].sLowerLim;
                szVarName = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iSeriesIDX].iMeasureQueueIDX].szMeasurementName;
                szUnits = tclsASAM.milstCompuMethodList[maiMeasCompuMethodIndices[iSeriesIDX]].szUnitsString;
                mDataTable.Rows.Add(szVarName, fMin, fMax, 0, 0, 0, szUnits);
            }

            dataGridViewLogging.DataSource = mDataTable;
        }

        private void dataGridViewLogging_DataBindingComplete(object sender,
             DataGridViewBindingCompleteEventArgs e)
        {
            int iRowIDX = 0;

            foreach (DataGridViewRow row in dataGridViewLogging.Rows)
            {
                Color backColor;
                if (0 == (iRowIDX % 2))
                {
                    backColor = Program.mProgramTextBackcolor;
                }
                else
                {
                    backColor = Program.mProgramTextToggleBackcolor;
                }

                row.Cells[0].Style.ForeColor = Program.mProgramTextForecolor;
                row.Cells[0].Style.BackColor = backColor;
                row.Cells[1].Style.ForeColor = Program.mProgramTextForecolor;
                row.Cells[1].Style.BackColor = backColor;
                row.Cells[2].Style.ForeColor = Program.mProgramTextForecolor;
                row.Cells[2].Style.BackColor = backColor;
                row.Cells[3].Style.ForeColor = Program.mProgramTextForecolor;
                row.Cells[3].Style.BackColor = backColor;
                row.Cells[4].Style.ForeColor = Program.mProgramTextForecolor;
                row.Cells[4].Style.BackColor = backColor;
                row.Cells[5].Style.ForeColor = Program.mProgramTextForecolor;
                row.Cells[5].Style.BackColor = backColor;
                row.Cells[6].Style.ForeColor = Program.mProgramTextForecolor;
                row.Cells[6].Style.BackColor = backColor;

                iRowIDX++;
            }
        }


        private void LoadComboBoxLoggingVars()
        {
            int iMeasureIDX = 0;
            int iOldMeasureSelectedIndex = comboBoxLoggingVars.SelectedIndex;

            comboBoxLoggingVars.Items.Clear();

            foreach (tstMeasurement stMeasurement in tclsASAM.milstAvailableMeasList)
            {
                if (true == MeasureIsActive(stMeasurement.szMeasurementName))
                {
                    mboDrawBold[iMeasureIDX] = true;
                    comboBoxLoggingVars.Items.Add(stMeasurement.szMeasurementName);
                }
                else
                {
                    mboDrawBold[iMeasureIDX] = false;
                    comboBoxLoggingVars.Items.Add(stMeasurement.szMeasurementName);
                }

                iMeasureIDX++;
            }

            comboBoxLoggingVars.Font = Program.GetTextFont();

            comboBoxLoggingVars.SelectedIndex = iOldMeasureSelectedIndex;
        }

        private void comboBoxLoggingVars_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font font = null;
            Brush brush;
            string text = comboBoxLoggingVars.Items[e.Index].ToString();
            int iListIDX = tclsASAM.iGetAvailableMeasIndex(text);

            if (true == mboDrawBold[iListIDX])
            {
                brush = Brushes.Green;
                font = Program.GetTextFont();
                font = new Font(font, FontStyle.Italic);
            }
            else
            {
                brush = Brushes.Orange;
                font = Program.GetTextFont();
                font = new Font(font, FontStyle.Regular);
            }

            e.Graphics.DrawString(text, font, brush, e.Bounds);
        }

        private bool MeasureIsActive(String szMeasureName)
        {
            bool IsActive = false;

            foreach (tstMeasurement stMeasurement in tclsASAM.mailstActiveMeasLists[miFormIDX])
            {
                if (szMeasureName == stMeasurement.szMeasurementName)
                {
                    IsActive = true;
                    break;
                }
            }

            return IsActive;
        }

        private OxyPlot.Axes.LinearAxis pstCreateNewYAxis(int iIDX, Single fMax, Single fMin, String szVarName, String szUnits, OxyColor enColour)
        {
            OxyPlot.Axes.LinearAxis stLinearYAxis = new OxyPlot.Axes.LinearAxis()
            {
                Maximum = fMax,
                Minimum = fMin,
                Position = AxisPosition.Left,
                Title = szVarName,
                TitleFontWeight = FontWeights.Bold,

                Key = "YAxis" + iIDX.ToString(),
                AbsoluteMaximum = fMax,
                AbsoluteMinimum = fMin,
                //MinorStep = ((fMax - fMin) / 10f),
                PositionTier = iIDX + 1,
                StartPosition = 0,
                TicklineColor = enColour,
                TitleColor = enColour,
                Unit = szUnits
            };

            return stLinearYAxis;
        }

        public void vUpdateMeasures(List<UInt32> lstData, int iActiveMeasureIndex)
        {
            Single sUnscaledData;
            Single sScaledData;
            UInt32 u32MeasureData;
            int iTimeStampIDXOffset = false == mboPCTimeStamps ? 1 : 0;
            Single fMeasureTimeStamp = 0;

            if (miFormIDX != iActiveMeasureIndex) return;

            if (true == mboSuspendUpdates)
            {
                return;
            }
            else if ((false == mboSuspendUpdates) && (true == mboOldSuspendUpdates))
            {
                Array.Clear(maau32LogData, 0, maau32LogData.Length);
                mboOldSuspendUpdates = false;
                mboSuspendUpdates = false;
                mu32GlobalLogPCMsStart = 0;
                miLoggingIDX = 1;
                mu32GlobalLogMeasureMsStart = 0xffffffff;
                miTimeStampIDX = -1;
            }

            if (0 == mu32GlobalLogPCMsStart)
            {
                mu32GlobalLogPCMsStart = (UInt32)(DateTime.Now.Ticks / 10000);
            }

            mu32GlobalLogPCMs = (UInt32)(DateTime.Now.Ticks / 10000) - mu32GlobalLogPCMsStart;

            if ((miFormIDX == iActiveMeasureIndex) && (true == mboLogging) && ((maau32LogData.Length / mastActiveMeasureIndices.Length) > miLoggingIDX))
            {
                if (lstData.Count == mastActiveMeasureIndices.Length)
                {
                    UInt32[] au32Data = lstData.ToArray();
                    Buffer.BlockCopy(au32Data, 0, maau32LogData, 4 * mastActiveMeasureIndices.Length * miLoggingIDX, 4 * mastActiveMeasureIndices.Length);
                }

                /* If using PC timestamps */
                if (-1 == miTimeStampIDX)
                {
                    fMeasureTimeStamp = (Single)mu32GlobalLogPCMs;
                }


                for (int iLogElementIDX = 0; iLogElementIDX < mastActiveMeasureIndices.Length; iLogElementIDX++)
                {
                    if (miTimeStampIDX == iLogElementIDX)
                    {
                        iTimeStampIDXOffset = 1;

                        if (0xffffffff == mu32GlobalLogMeasureMsStart)
                        {
                            mu32GlobalLogMeasureMsStart = maau32LogData[miLoggingIDX, iLogElementIDX];
                        }
                        mu32GlobalLogMeasureMs = maau32LogData[miLoggingIDX, iLogElementIDX] - mu32GlobalLogMeasureMsStart;
                        fMeasureTimeStamp = (Single)mu32GlobalLogMeasureMs;
                    }
                    else
                    {
                        u32MeasureData = maau32LogData[miLoggingIDX, iLogElementIDX];

                        switch (tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iLogElementIDX].iMeasureQueueIDX].enRecLayout)
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

                        sScaledData = fGetScaledData(maiMeasCompuMethodIndices[iLogElementIDX], iLogElementIDX, sUnscaledData);

                        seriesdata[iLogElementIDX].Points.Add(new DataPoint(fMeasureTimeStamp, sScaledData));

                        if (mu32TableRefreshCount != mu32TableRefreshCountOld)
                        {
                            vShowGridData(iLogElementIDX, sScaledData); 
                        }

                    }
                }

                if (0 < (fMeasureTimeStamp - (Single)miZoomBack / 1000f))
                {
                    mstXAxis.Zoom(fMeasureTimeStamp - (Double)(miZoomBack), (Double)fMeasureTimeStamp + (Double)(miZoomForward));
                    mstXAxis.IsZoomEnabled = true;
                }
                else
                {
                    mstXAxis.Zoom(0f, fMeasureTimeStamp + (Single)miZoomForward / 1000f);
                }

                mclsPlotLogging.InvalidatePlot(true);      
                miLoggingIDX++;

                progressBarLogBuffer.Value = 1 + miLoggingIDX / 660;
                mu32TableRefreshCountOld = mu32TableRefreshCount;
            }
        }

        private void vShowGridData(int iLogElementIDX, Single sScaledData)
        {
            mDataTable.Rows[iLogElementIDX][5] = sScaledData;

            if (sScaledData < maafMinMaxLogData[iLogElementIDX, 0])
            {
                maafMinMaxLogData[iLogElementIDX, 0] = sScaledData;
                mDataTable.Rows[iLogElementIDX][4] = sScaledData;
            }

            if (sScaledData > maafMinMaxLogData[iLogElementIDX, 1])
            {
                maafMinMaxLogData[iLogElementIDX, 1] = sScaledData;
                mDataTable.Rows[iLogElementIDX][3] = sScaledData;
            }
        }

        private String szGetScaledData(int iCompuMethodIDX, int iElementIDX, Single sUnscaledData)
        {
            Single sScaledData = fGetScaledData(iCompuMethodIDX, iElementIDX, sUnscaledData);
            String szFormattedData = String.Format(maszMeasCompuMethodFormat[iElementIDX], sScaledData);
            return szFormattedData;
        }

        private Single fGetScaledData(int iCompuMethodIDX, int iElementIDX, Single sUnscaledData)
        {
            Single sScaledData;

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

            return sScaledData;
        }

        private void vCreateMeasCompuMethodFormat(int iMeasureIDX)
        {
            String szFormatString = "{0:";

            {
                szFormatString += "0";
            }

            if (0 < tclsASAM.milstCompuMethodList[maiMeasCompuMethodIndices[iMeasureIDX]].iPostDPCount)
            {
                szFormatString += "." +
                new String('0',
                tclsASAM.milstCompuMethodList[maiMeasCompuMethodIndices[iMeasureIDX]].iPostDPCount) + "}";
            }
            else
            {
                szFormatString += "}";
            }

            maszMeasCompuMethodFormat[iMeasureIDX] = szFormatString;
        }        

        public void vSetInitialise(bool boInitialise)
        {
            mboInitialising = boInitialise;
        }


        private void tclsMeasValueLoggingView_Load(object sender, EventArgs e)
        {
            //float fYGraphMin, fYGraphMax;
            //mclsDisplay.DataSources.Clear();
            //int iTimeStampIDXOffset = false == mboPCTimeStamps ? 1 : 0;

            //GetColorsFromINI();

            //for (int iDataSourceIDX = iTimeStampIDXOffset; iDataSourceIDX < mastActiveMeasureIndices.Length; iDataSourceIDX++)
            //{
            //    mclsDisplay.DataSources.Add(new DataSource());
            //    mclsDisplay.DataSources[iDataSourceIDX - iTimeStampIDXOffset].Length = miDataSourceLength;
            //    mclsDisplay.DataSources[iDataSourceIDX - iTimeStampIDXOffset].AutoScaleY = false;
            //    fYGraphMin = tclsASAM.mailstActiveMeasLists[miFormIDX][iDataSourceIDX].sLowerLim;
            //    fYGraphMax = tclsASAM.mailstActiveMeasLists[miFormIDX][iDataSourceIDX].sUpperLim;
            //    mclsDisplay.DataSources[iDataSourceIDX - iTimeStampIDXOffset].SetDisplayRangeY(fYGraphMin, fYGraphMax);
            //    mclsDisplay.SetDisplayRangeX(0, mfDisplayRangeX);
            //    mclsDisplay.DataSources[iDataSourceIDX - iTimeStampIDXOffset].SetGridDistanceY((fYGraphMax - fYGraphMin) / 5);
            //    mclsDisplay.DataSources[iDataSourceIDX - iTimeStampIDXOffset].FixedOrdinates = false;

            //    for (int iX = 0; iX < miDataSourceLength; iX++)
            //    {

            //        mclsDisplay.DataSources[iDataSourceIDX - iTimeStampIDXOffset].Samples[iX].x = (Single)iX;
            //        mclsDisplay.DataSources[iDataSourceIDX - iTimeStampIDXOffset].Samples[iX].y = (Single)iX + 10 * iDataSourceIDX;
            //    }

            //    mclsDisplay.DataSources[iDataSourceIDX - iTimeStampIDXOffset].GraphColor = maclsLoggingColor[iDataSourceIDX - iTimeStampIDXOffset];

            //    this.Left = 150;
            //}

            //mclsDisplay.BackgroundColorTop = mclsBgColorTop;
            //mclsDisplay.BackgroundColorBot = mclsBgColorBottom;
            //mclsDisplay.SolidGridColor = mclsGridColorSolid;
            //mclsDisplay.DashedGridColor = mclsGridColorDashed;
            //mclsDisplay.ForeColor = mclsGridColorFore;
            //mclsDisplay.Smoothing = System.Drawing.Drawing2D.SmoothingMode.None;
            //mclsDisplay.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
            //mclsDisplay.Width = 300;
            //mclsDisplay.Refresh();
        }

        private void buttonLogSave_Click(object sender, EventArgs e)
        {
            mboLogging = false;
            List<string> lstLogStrings = new List<string>();
            string szLogHeaderLine = "";

            for (int iLogElementIDX = 0; iLogElementIDX < mastActiveMeasureIndices.Length; iLogElementIDX++)
            {
                string szLogHeaderVar = tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iLogElementIDX].iMeasureQueueIDX].szMeasurementName;
                szLogHeaderVar += " (";
                szLogHeaderVar += tclsASAM.milstCompuMethodList[maiMeasCompuMethodIndices[iLogElementIDX]].szUnitsString;
                szLogHeaderVar += "),";
                szLogHeaderLine += szLogHeaderVar;
            }

            lstLogStrings.Add(szLogHeaderLine);

            for (int iLogIDX = 0; iLogIDX < miLoggingIDX; iLogIDX++)
            {
                string szLogLine = "";
                Single sUnscaledData;
                UInt32 u32MeasureData;

                for (int iLogElementIDX = 0; iLogElementIDX < mastActiveMeasureIndices.Length; iLogElementIDX++)
                {
                    u32MeasureData = maau32LogData[iLogIDX, iLogElementIDX];

                    switch (tclsASAM.mailstActiveMeasLists[miFormIDX][mastActiveMeasureIndices[iLogElementIDX].iMeasureQueueIDX].enRecLayout)
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

                    string szLogValue = szGetScaledData(maiMeasCompuMethodIndices[iLogElementIDX], iLogElementIDX, sUnscaledData);
                    szLogLine += szLogValue;
                    szLogLine += ",";
                }

                lstLogStrings.Add(szLogLine);
            }

            String szLogFileName = AppDomain.CurrentDomain.BaseDirectory + "DataLogs\\Datalog_" + DateTime.UtcNow.Day + "_"
                + DateTime.UtcNow.Month + "_"
                + DateTime.UtcNow.Year + "_"
                + DateTime.UtcNow.Hour + "_"
                + DateTime.UtcNow.Minute + ".csv";


            System.IO.File.WriteAllLines(szLogFileName, lstLogStrings.ToArray());

            mboLogging = true;
            miLoggingIDX = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mu32TableRefreshCount++;
        }

        private void tclsMeasValueLoggingView_Resize(object sender, System.EventArgs e)
        {
            //if (null != mclsDisplay)
            //{
            //    mclsDisplay.Width = this.Width - 27;
            //    mclsDisplay.Height = this.Height - 115;
            //    mclsDisplay.Left = 10;
            //    mclsDisplay.Top = 40;
            //    comboBoxLoggingVars.Top = this.Height - 65;
            //    buttonLogAdd.Top = this.Height - 65;
            //    mclsDisplay.DataSources[0].SetDataSourceDrawnAll(false);
            //    mclsDisplay.Refresh();
            //}
        }

        private void GetColorsFromINI()
        {
            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Database\\GUI Layout Databases\\GUI Layout Rabbit 1_0 ECU.INI");

            string szBgColorTop; string szBgColorBottom; string szGridColorSolid; string szGridColorDashed; string szGridColorFore; 
            string[] aszLoggingColor = new string[20];
            string[] aszSplitRGB;
            maclsLoggingColor = new Color[20];
            int iColorIDX;

            try
            {
                szBgColorTop = mclsIniParser.GetSetting("Logging Options", "BgColorTop");
                szBgColorTop = null == szBgColorTop ? "255,255,255" : szBgColorTop;
            }
            catch
            {
                szBgColorTop = "255,255,255";
            }

            try
            {
                szBgColorBottom = mclsIniParser.GetSetting("Logging Options", "BgColorBottom");
                szBgColorBottom = null == szBgColorBottom ? "255,255,255" : szBgColorBottom;
            }
            catch
            {
                szBgColorBottom = "255,255,255";
            }

            try
            {
                szGridColorSolid = mclsIniParser.GetSetting("Logging Options", "GridColorSolid");
                szGridColorSolid = null == szGridColorSolid ? "255,255,255" : szGridColorSolid;
            }
            catch
            {
                szGridColorSolid = "255,255,255";
            }

            try
            {
                szGridColorDashed = mclsIniParser.GetSetting("Logging Options", "GridColorDashed");
                szGridColorDashed = null == szGridColorDashed ? "255,255,255" : szGridColorDashed;
            }
            catch
            {
                szGridColorDashed = "255,255,255";
            }

            try
            {
                szGridColorFore = mclsIniParser.GetSetting("Logging Options", "GridColorFore");
                szGridColorFore = null == szGridColorFore ? "255,255,255" : szGridColorFore;
            }
            catch
            {
                szGridColorFore = "255,255,255";
            }


            for (iColorIDX = 1; iColorIDX < 20; iColorIDX++)
            {
                try
                {
                    aszLoggingColor[iColorIDX - 1] = mclsIniParser.GetSetting("Logging Options", "LoggingColor" + iColorIDX.ToString());
                    aszLoggingColor[iColorIDX - 1] = null == aszLoggingColor[iColorIDX - 1] ? "255,255,255" : aszLoggingColor[iColorIDX - 1];
                }
                catch
                {
                    aszLoggingColor[iColorIDX - 1] = "255,255,255";
                }
            }

            aszSplitRGB = szBgColorTop.Split(',');
            //mclsBgColorTop = System.Drawing.Color.FromArgb(Convert.ToInt16(aszSplitRGB[0]), Convert.ToInt16(aszSplitRGB[1]), Convert.ToInt16(aszSplitRGB[2]));

            aszSplitRGB = szBgColorBottom.Split(',');
            //mclsBgColorBottom = System.Drawing.Color.FromArgb(Convert.ToInt16(aszSplitRGB[0]), Convert.ToInt16(aszSplitRGB[1]), Convert.ToInt16(aszSplitRGB[2]));

            aszSplitRGB = szGridColorSolid.Split(',');
            //mclsGridColorSolid = System.Drawing.Color.FromArgb(Convert.ToInt16(aszSplitRGB[0]), Convert.ToInt16(aszSplitRGB[1]), Convert.ToInt16(aszSplitRGB[2]));

            aszSplitRGB = szGridColorDashed.Split(',');
            //mclsGridColorDashed = System.Drawing.Color.FromArgb(Convert.ToInt16(aszSplitRGB[0]), Convert.ToInt16(aszSplitRGB[1]), Convert.ToInt16(aszSplitRGB[2]));

            aszSplitRGB = szGridColorFore.Split(',');
            //mclsGridColorFore = System.Drawing.Color.FromArgb(Convert.ToInt16(aszSplitRGB[0]), Convert.ToInt16(aszSplitRGB[1]), Convert.ToInt16(aszSplitRGB[2]));

            for (iColorIDX = 1; iColorIDX < 20; iColorIDX++)
            {
                aszSplitRGB = aszLoggingColor[iColorIDX - 1].Split(',');
                maclsLoggingColor[iColorIDX - 1] = System.Drawing.Color.FromArgb(Convert.ToInt16(aszSplitRGB[0]), Convert.ToInt16(aszSplitRGB[1]), Convert.ToInt16(aszSplitRGB[2]));
            }
        }

        private void SaveColorsToINI()
        {
            int iColorIDX;
            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Database\\GUI Layout Databases\\GUI Layout Rabbit 1_0 ECU.INI");
            //UInt32 u32ARGB;
            //string szSetting;
            //byte u8ColorByte;

            for (iColorIDX = 1; iColorIDX < 20; iColorIDX++)
            {
                //szSetting = "";
                //u32ARGB = maclsLoggingColor[iColorIDX - 1].ToArgb();
                //u8ColorByte = (byte)(u32ARGB >> 24);
                //szSetting += u8ColorByte.ToString();
            }

            foreach (Form clsForm in this.MdiChildren)
            {
                String szFormTitle = clsForm.Text;

                mclsIniParser.AddSetting("Window Layout", szFormTitle + ".Left", clsForm.Left.ToString());
                mclsIniParser.AddSetting("Window Layout", szFormTitle + ".Width", clsForm.Width.ToString());
                mclsIniParser.AddSetting("Window Layout", szFormTitle + ".Top", clsForm.Top.ToString());
                mclsIniParser.AddSetting("Window Layout", szFormTitle + ".Height", clsForm.Height.ToString());

            }

            mclsIniParser.SaveSettings();
        }

        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            if (0 < miZoomForwardIDX)
            {
                miZoomForwardIDX--;
            }

            miZoomForward = maiZoom[miZoomForwardIDX] / 2;
            miZoomBack = maiZoom[miZoomForwardIDX];
            miZoomBackIDX = miZoomForwardIDX;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((maiZoom.Length - 1) > miZoomForwardIDX)
            {
                miZoomForwardIDX++;
            }

            miZoomForward = maiZoom[miZoomForwardIDX] / 2;
            miZoomBack = maiZoom[miZoomForwardIDX];
            miZoomBackIDX = miZoomForwardIDX;
        }

        private void buttonLogAdd_Click(object sender, EventArgs e)
        {
            mboSuspendUpdates = true;

            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);

            if (null != comboBoxLoggingVars.SelectedItem)
            {
                vResetLogging();

                if (false == MeasureIsActive(comboBoxLoggingVars.SelectedItem.ToString()))
                {
                    boAddMeasure(comboBoxLoggingVars.SelectedItem.ToString());
                }
                else
                {
                    boRemoveMeasure(comboBoxLoggingVars.SelectedItem.ToString());
                }

                Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestDDDIReset, 0, null);
                vLoadPlotView();
                vSetupDataGrid();
            }
            else
            {
                Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, "No measure was selected!");
            }

            mboOldSuspendUpdates = true;
            mboSuspendUpdates = false;

            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);
        }

        private void ResizeArray<T>(ref T[,] original, int newColNum, int newRowNum)
        {
            var newArray = new T[newColNum, newRowNum];

            int columnsToCopy = newColNum < original.GetLength(0) ? newColNum : original.GetLength(0);
            int rowsToCopy = newRowNum < original.GetLength(1) ? newRowNum : original.GetLength(1);

            int colIDX;
            int rowIDX;

            for (colIDX = 0; colIDX < columnsToCopy; colIDX++)
            {
                for (rowIDX = 0; rowIDX < rowsToCopy; rowIDX++)
                {
                    newArray[colIDX, rowIDX] = original[colIDX, rowIDX];
                }
            }

            original = newArray;
        }

        private void buttonSplitterUp_Click(object sender, EventArgs e)
        {
            tableLayoutPanelLogging.RowStyles.Clear();

            if (20 < mfSplitterPosition)
            {
                mfSplitterPosition -= 2;
            }

            tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, mfSplitterPosition));
            tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 - mfSplitterPosition));
        }

        private void buttonSplitterDown_Click(object sender, EventArgs e)
        {
            tableLayoutPanelLogging.RowStyles.Clear();

            if (80 > mfSplitterPosition)
            {
                mfSplitterPosition += 2;
            }

            tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, mfSplitterPosition));
            tableLayoutPanelLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 - mfSplitterPosition));
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            mboSuspendUpdates = true;

            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);
            vLoadPlotView();
            vSetupDataGrid();
            mboOldSuspendUpdates = true;
            mboSuspendUpdates = false;
            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);
        }

        private void comboBoxLoggingVars_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void vRequestShutdown()
        {
            mboRequestShutdown = true;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (true == mboRequestShutdown)
            {
                SaveColorsToINI();
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