/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Gauges View                                            */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsMeasureValeGaugeView.cs                            */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using ElementHostGaugeTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace UDP
{
    public partial class tclsMeasValueGaugeView : Form
    {
        tclsWindowElement[] maclsWindowElement;
        tstActiveMeasureIndices[] mastActiveMeasureIndices;
        int[] maiMeasCompuMethodIndices;
        String[] maszMeasCompuMethodFormat;
        bool mboInitialising;
        int miFormIDX;
        ElementHost[] maHost;
        double[] mfGaugeMin;
        double[] mfGaugeMax;
        List<MDACCustomGaugeControl> mlstGauge;
        int miGaugeCount;
        bool mboGaugeSweeps;
        bool mboRequestShutdown;
        bool mboSuspendUpdates;
        System.Timers.Timer mclsGaugeTimer;
        int miGaugeSweepsCount;

        public tclsMeasValueGaugeView(int iFormIDX)
        {
            miGaugeSweepsCount = 0;
            miFormIDX = iFormIDX;
            InitializeComponent();
            mclsGaugeTimer = new System.Timers.Timer(50);
            mclsGaugeTimer.Enabled = false;
            mclsGaugeTimer.Elapsed += new ElapsedEventHandler(GaugeTimer);
            maclsWindowElement = new tclsWindowElement[Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX].Count];

            Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX].CopyTo(maclsWindowElement, 0);

            CreateGauges(iFormIDX, true);

            tclsASAM.vCalcDDDIAndSetRate(iFormIDX, 100);

            ArrangeElementHosts(false);
        }

        private void CreateGauges(int iFormIDX, bool boLoad)
        {
            tstActiveMeasureIndices stActiveMeasureIndices;
            int iElementIDX;
            int iMeasureIDX = 0;

            /* Count number of elements assigned to this window */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "measure"))
                {
                    iMeasureIDX++;
                }
            }

            if (boLoad)
            {
            maszMeasCompuMethodFormat = new String[iMeasureIDX];
            mastActiveMeasureIndices = new tstActiveMeasureIndices[iMeasureIDX];
            maiMeasCompuMethodIndices = new int[iMeasureIDX];
            mfGaugeMax = new double[iMeasureIDX];
            mfGaugeMin = new double[iMeasureIDX];
            }

            iMeasureIDX = 0;
            miGaugeCount = 0;

            if (null != maHost)
            {
                foreach (ElementHost clsHost in maHost)
                {
                    this.Controls.Remove(clsHost);
                }
            }

            maHost = new System.Windows.Forms.Integration.ElementHost[9];
            mlstGauge = new List<MDACCustomGaugeControl>();

            /* Loop through and create visual controls required */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "measure"))
                {
                    if (boLoad == true)
                    {
                    stActiveMeasureIndices = tclsASAM.stTryAddActiveMeas(maclsWindowElement[iElementIDX].szA2LName, iFormIDX);
                    mastActiveMeasureIndices[iMeasureIDX].iMeasureQueue = stActiveMeasureIndices.iMeasureQueue;
                    mastActiveMeasureIndices[iMeasureIDX].iMeasureQueueIDX = stActiveMeasureIndices.iMeasureQueueIDX;
                    maiMeasCompuMethodIndices[iMeasureIDX] = tclsASAM.iGetCompuMethodIndexFromMeas(maclsWindowElement[iElementIDX].szA2LName, iFormIDX);
                    vCreateMeasCompuMethodFormat(iMeasureIDX);
                    }

                    mfGaugeMin[iElementIDX - 1] = Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[1]);
                    mfGaugeMax[iElementIDX - 1] = Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[2]);

                    MDACCustomGaugeControl gauge = new MDACCustomGaugeControl(maclsWindowElement[iElementIDX].aszPresentationOptions[0],
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[1]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[2]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[3]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[4]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[5]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[6]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[7]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[8]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[9]),
                        Convert.ToDouble(maclsWindowElement[iElementIDX].aszPresentationOptions[10]));
                    maHost[miGaugeCount] = new ElementHost();
                    maHost[miGaugeCount].Child = gauge;
                    this.Controls.Add(maHost[miGaugeCount]);
                    mlstGauge.Add(gauge);
                    miGaugeCount++;
                    iMeasureIDX++;
                }                

                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "window"))
                {
                    this.Text = ConstantData.APPCONFIG.szAppName + ":" + maclsWindowElement[iElementIDX].szLabel;
                }
            }
        }

        private void ArrangeElementHosts(bool boLoad)
        {
            switch (miGaugeCount)
            {
                case 1:
                    {
                        maHost[0].Top = 25;
                        maHost[0].Width = this.Width - 10;
                        maHost[0].Height = this.Height - 50;
                        maHost[0].Left = 5;
                        break;
                    }
                case 2:
                    {
                        maHost[0].Top = 25;
                        maHost[0].Width = this.Width / 2;
                        maHost[0].Height = this.Height - 50;
                        maHost[0].Left = 0;

                        maHost[1].Top = 25;
                        maHost[1].Width = this.Width / 2;
                        maHost[1].Height = this.Height - 50;
                        maHost[1].Left = this.Width / 2;
                        break;
                    }
                case 3:
                    {
                        maHost[0].Top = 5;
                        maHost[0].Width = this.Width / 3 - 10;
                        maHost[0].Height = this.Height - 50;
                        maHost[0].Left = 5;

                        maHost[1].Top = 5;
                        maHost[1].Width = this.Width / 3 - 10;
                        maHost[1].Height = this.Height - 50;
                        maHost[1].Left = this.Width / 3 + 5;

                        maHost[2].Top = 5;
                        maHost[2].Width = this.Width / 3 - 10;
                        maHost[2].Height = this.Height - 50;
                        maHost[2].Left = 2 * this.Width / 3 + 5;
                        break;
                    }
                case 4:
                    {
                        if (boLoad)
                        {
                            this.Width = (int)(4 * System.Windows.SystemParameters.PrimaryScreenWidth) / 10;
                            this.Height = this.Width;
                        }

                        maHost[0].Top = 25;
                        maHost[0].Width = this.Width / 2 - 10;
                        maHost[0].Height = this.Height / 2 - 30;
                        maHost[0].Left = 5;

                        maHost[1].Top = 25;
                        maHost[1].Width = this.Width / 2 - 10;
                        maHost[1].Height = this.Height / 2 - 30;
                        maHost[1].Left = this.Width / 2 + 5;

                        maHost[2].Top = this.Height / 2;
                        maHost[2].Width = this.Width / 2 - 10;
                        maHost[2].Height = this.Height / 2 - 30;
                        maHost[2].Left = 5;

                        maHost[3].Top = this.Height / 2;
                        maHost[3].Width = this.Width / 2 - 10;
                        maHost[3].Height = this.Height / 2 - 30;
                        maHost[3].Left = this.Width / 2 + 5;
                        break;
                    }
                case 5:
                    {
                        maHost[0].Top = 25;
                        maHost[0].Width = this.Width / 3 - 10;
                        maHost[0].Height = this.Height / 2 - 30;
                        maHost[0].Left = 5;

                        maHost[1].Top = 25;
                        maHost[1].Width = this.Width / 3 - 10;
                        maHost[1].Height = this.Height / 2 - 30;
                        maHost[1].Left = this.Width / 3 + 5;

                        maHost[2].Top = 25;
                        maHost[2].Width = this.Width / 3 - 10;
                        maHost[2].Height = this.Height / 2 - 30;
                        maHost[2].Left = 2 * this.Width / 3 + 5;

                        maHost[3].Top = this.Height / 2;
                        maHost[3].Width = this.Width / 3 - 10;
                        maHost[3].Height = this.Height / 2 - 30;
                        maHost[3].Left = this.Width / 6 + 5;

                        maHost[4].Top = this.Height / 2;
                        maHost[4].Width = this.Width / 3 - 10;
                        maHost[4].Height = this.Height / 2 - 30;
                        maHost[4].Left = this.Width / 2 + 5;
                        break;
                    }
                case 6:
                    {
                        maHost[0].Top = 25;
                        maHost[0].Width = this.Width / 3 - 10;
                        maHost[0].Height = this.Height / 2 - 30;
                        maHost[0].Left = 5;

                        maHost[1].Top = 25;
                        maHost[1].Width = this.Width / 3 - 10;
                        maHost[1].Height = this.Height / 2 - 30;
                        maHost[1].Left = this.Width / 3 + 5;

                        maHost[2].Top = 25;
                        maHost[2].Width = this.Width / 3 - 10;
                        maHost[2].Height = this.Height / 2 - 30;
                        maHost[2].Left = 2 * this.Width / 3 + 5;

                        maHost[3].Top = this.Height / 2;
                        maHost[3].Width = this.Width / 3 - 10;
                        maHost[3].Height = this.Height / 2 - 30;
                        maHost[3].Left = 5;

                        maHost[4].Top = this.Height / 2;
                        maHost[4].Width = this.Width / 3 - 10;
                        maHost[4].Height = this.Height / 2 - 30;
                        maHost[4].Left = this.Width / 3 + 5;

                        maHost[5].Top = this.Height / 2;
                        maHost[5].Width = this.Width / 3 - 10;
                        maHost[5].Height = this.Height / 2 - 30;
                        maHost[5].Left = 2 * this.Width / 3 + 5;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        public void vUpdateMeasures(List<UInt32> lstData, int iActiveMeasureIndex)
        {
            int iMeasIDX = 0;

            if (miFormIDX != iActiveMeasureIndex) return;

            if (false == mboGaugeSweeps)
            {
                foreach (UInt32 u32MeasureData in lstData)
                {
                    int iGaugeIDX = 0;
                    int iMeasCompuMethodIDX;
                    Single sUnscaledData;

                    foreach (tstActiveMeasureIndices stActiveMeasureIndices in mastActiveMeasureIndices)
                    {
                        if (iActiveMeasureIndex == stActiveMeasureIndices.iMeasureQueue)
                        {
                            if (iMeasIDX == stActiveMeasureIndices.iMeasureQueueIDX)
                            {
                                iMeasCompuMethodIDX = maiMeasCompuMethodIndices[iGaugeIDX];

                                if (-1 < iMeasCompuMethodIDX)
                                {
                                    switch (tclsASAM.mailstActiveMeasLists[stActiveMeasureIndices.iMeasureQueue][stActiveMeasureIndices.iMeasureQueueIDX].enRecLayout)
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

                                    switch (tclsASAM.milstCompuMethodList[iMeasCompuMethodIDX].enCM_Type)
                                    {
                                        case tenCM_Type.enIDENTICAL:
                                            {
                                                mlstGauge[iGaugeIDX].SetDataValue(sUnscaledData);
                                                break;
                                            }
                                        case tenCM_Type.enLINEAR:
                                            {
                                                Single sScaledData = tclsASAM.milstCompuMethodList[iMeasCompuMethodIDX].sCoeff1 * sUnscaledData;
                                                sScaledData += tclsASAM.milstCompuMethodList[iMeasCompuMethodIDX].sCoeff2;
                                                mlstGauge[iGaugeIDX].SetDataValue(sScaledData);
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }


                                    iGaugeIDX++;
                                }
                            }
                        }
                        iGaugeIDX++;
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
                        szFormattedData = String.Format(maszMeasCompuMethodFormat[iElementIDX], sScaledData);
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

        void GaugeTimer(object sender, ElapsedEventArgs e)
        {
            if (true == mboGaugeSweeps)
            {
                for (int iGaugeIDX = 0; iGaugeIDX < miGaugeCount; iGaugeIDX++)
                {
                    if ((80 > miGaugeSweepsCount) && (40 <= miGaugeSweepsCount))
                    {
                        mlstGauge[iGaugeIDX].SetDataValue(mfGaugeMin[iGaugeIDX] + ((double)(miGaugeSweepsCount - 40) / 40f) * (double)(mfGaugeMax[iGaugeIDX] - mfGaugeMin[iGaugeIDX]));
                    }
                    else if (80 <= miGaugeSweepsCount)
                    {
                        mlstGauge[iGaugeIDX].SetDataValue(mfGaugeMin[iGaugeIDX] + ((120f - (double)miGaugeSweepsCount) / (double)(miGaugeSweepsCount - 40) * (double)(mfGaugeMax[iGaugeIDX] - mfGaugeMin[iGaugeIDX])));
                    }
                }
                miGaugeSweepsCount += 2;
            }
           

            if (120 < miGaugeSweepsCount)
            {
                mclsGaugeTimer.Enabled = false;
                mboGaugeSweeps = false;
            }
        }

        public void vSetInitialise(bool boInitialise)
        {
            mboInitialising = boInitialise;

            if (false == boInitialise)
            {
                mboGaugeSweeps = true;
                mclsGaugeTimer.Enabled = true;
            }
        }      

        private void tclsMeasCharView_Load(object sender, EventArgs e)
        {
            this.Left = 150;
        }

        private void tclsMeasValueGaugeView_Load(object sender, EventArgs e)
        {

        }

        private void GaugeView_Resize(object sender, System.EventArgs e)
        {
            ArrangeElementHosts(false);
            mboGaugeSweeps = true;
            miGaugeSweepsCount = 30;
            mclsGaugeTimer.Enabled = true;
        }

        private void tclsMeasValueGaugeView_Load_1(object sender, EventArgs e)
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

            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("View minimised to the bottom", "Not possible to close view");
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void addRemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tclsMeasureSelect clsMeasureSelect = new tclsMeasureSelect(miFormIDX, this);
            clsMeasureSelect.Show();
        }

        public bool AddMeasure(string measureName, string[] aszPresentationOptions)
        {
            mboSuspendUpdates = true;
            int iMeasureRemoveIDX = 0;
            int iMeasureIDX;

            if (false == MeasureIsActive(measureName))
            {
                if (maclsWindowElement.Length >= 7)
                {
                    MessageBox.Show("Only 6 gauges can be shown, please remove a gauge first");
                    return false;
                }
            }

            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);

            if (false == MeasureIsActive(measureName))
            {
                boAddMeasure(measureName);

                tclsWindowElement clsWindowElement = new tclsWindowElement("measure", -1, measureName, "Hello", aszPresentationOptions);
                    Array.Resize(ref maclsWindowElement, maclsWindowElement.Length + 1);

                maclsWindowElement[maclsWindowElement.Length - 1] = clsWindowElement;
            }
            else
            {
                boRemoveMeasure(measureName);

                foreach (tclsWindowElement clsWindowElement in maclsWindowElement)
                {
                    if (clsWindowElement.szA2LName != measureName)
                    {
                        iMeasureRemoveIDX++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (iMeasureIDX = iMeasureRemoveIDX; iMeasureIDX < maclsWindowElement.Length - 1; iMeasureIDX++)
                {
                    maclsWindowElement[iMeasureIDX].iGUILinkIndex = maclsWindowElement[iMeasureIDX + 1].iGUILinkIndex;
                    maclsWindowElement[iMeasureIDX].szA2LName = maclsWindowElement[iMeasureIDX + 1].szA2LName;
                    maclsWindowElement[iMeasureIDX].szElementType = maclsWindowElement[iMeasureIDX + 1].szElementType;
                    maclsWindowElement[iMeasureIDX].szLabel = maclsWindowElement[iMeasureIDX + 1].szLabel;
                    maclsWindowElement[iMeasureIDX].aszPresentationOptions = maclsWindowElement[iMeasureIDX + 1].aszPresentationOptions;
                }

                Array.Resize(ref maclsWindowElement, maclsWindowElement.Length - 1);
            }

            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestDDDIReset, 0, null);

            mboSuspendUpdates = false;

            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);

            CreateGauges(miFormIDX, false);
            ArrangeElementHosts(true);

            return true;
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
                tclsASAM.vCalcDDDIAndSetRate(miFormIDX, 100);

                Array.Resize(ref mfGaugeMin, mfGaugeMin.Length + 1);
                Array.Resize(ref mfGaugeMax, mfGaugeMax.Length + 1);
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
                Array.Resize(ref mfGaugeMin, mfGaugeMin.Length - 1);
                Array.Resize(ref mfGaugeMax, mfGaugeMax.Length - 1);
                tclsASAM.vSetDDDIResetPendingIDX(miFormIDX);
                tclsASAM.vCalcDDDIAndSetRate(miFormIDX, 100);
            }

            return ret_val;
        }

        private void gaugesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
