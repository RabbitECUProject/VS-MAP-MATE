/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Segment Display View                                   */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsICMeasSegmentView.cs                               */
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
using LBSoft.IndustrialCtrls;

namespace UDP
{
    public partial class tclsICMeasSegmentView : Form
    {
        tclsWindowElement[] maclsWindowElement;
        tstActiveMeasureIndices[,] maastActiveMeasureIndices;
        int[,] maaiMeasCompuMethodIndices;
        LBSoft.IndustrialCtrls.Leds.LB7SegmentDisplay[,] maaclsDigitalMeter;
        Label[] maclsMeasureLabel;
        List<int> mlstFormIndices;
        List<int> mlstSegCount;
        List<int> mlstPreDPCount;
        bool mboInitialising;
        bool mboSuspendUpdates;
        bool mboRequestShutdown;
        int miFormIDX;

        public tclsICMeasSegmentView(int iFormIDX)
        {
            miFormIDX = iFormIDX;
            InitializeComponent();
            mlstFormIndices = new List<int>();
            mlstFormIndices.Add(miFormIDX);
            maastActiveMeasureIndices = new tstActiveMeasureIndices[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT, ConstantData.UDS.ru8UDS_MEAS_DDDI_MAX_ELEMENTS];
            maaiMeasCompuMethodIndices = new int[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT, ConstantData.UDS.ru8UDS_MEAS_DDDI_MAX_ELEMENTS];
            mlstSegCount = new List<int>();
            mlstPreDPCount = new List<int>();

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
            int iElementIDX;
            int iVerticalElementCount = 0;
            int iMaxSegments = 0;
            tstActiveMeasureIndices stActiveMeasureIndices;

            mboSuspendUpdates = true;
            RemoveControls();

            maclsWindowElement = new tclsWindowElement[Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].Count];

            Program.mAPP_clsXMLConfig.mailstWindowLists[miFormIDX].CopyTo(maclsWindowElement, 0);

            mlstSegCount.Clear();
            mlstPreDPCount.Clear();


            /* Count number of elements assigned to this window */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "measure"))
                {
                    if (-1 != tclsASAM.iGetAvailableMeasIndex(maclsWindowElement[iElementIDX].szA2LName))
                    {
                        if (true == boSetup)
                        {
                            stActiveMeasureIndices = tclsASAM.stTryAddActiveMeas(maclsWindowElement[iElementIDX].szA2LName, miFormIDX);
                            maastActiveMeasureIndices[miFormIDX, iMeasureIDX].iMeasureQueue = stActiveMeasureIndices.iMeasureQueue;
                            maastActiveMeasureIndices[miFormIDX, iMeasureIDX].iMeasureQueueIDX = stActiveMeasureIndices.iMeasureQueueIDX;
                            maaiMeasCompuMethodIndices[miFormIDX, iMeasureIDX] = tclsASAM.iGetCompuMethodIndexFromMeas(maclsWindowElement[iElementIDX].szA2LName, miFormIDX);
                        }

                        int iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromMeas(maclsWindowElement[iElementIDX].szA2LName, miFormIDX);
                        int iSegments = tclsASAM.milstCompuMethodList[iCompuMethodIDX].iPostDPCount + tclsASAM.milstCompuMethodList[iCompuMethodIDX].iPreDPCount;
                        iMaxSegments = iMaxSegments < iSegments ? iSegments : iMaxSegments;
                        mlstSegCount.Add(iSegments);
                        mlstPreDPCount.Add(tclsASAM.milstCompuMethodList[iCompuMethodIDX].iPreDPCount);
                        iMeasureIDX++;
                    }
                    else
                    {
                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, "Layout measure element " +
                            maclsWindowElement[iElementIDX].szA2LName + " not found in ASAM database, this element will not be shown");
                    }

                }
            }

            maaclsDigitalMeter = new LBSoft.IndustrialCtrls.Leds.LB7SegmentDisplay[iMeasureIDX, iMaxSegments];
            maclsMeasureLabel = new Label[iMeasureIDX];

            iMeasureIDX = 0;

            /* Loop through and create visual controls required */
            for (iElementIDX = 0; iElementIDX < maclsWindowElement.Length; iElementIDX++)
            {
                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "measure"))
                {
                    int iSegmentIDX;

                    if (-1 != tclsASAM.iGetAvailableMeasIndex(maclsWindowElement[iElementIDX].szA2LName))
                    {
                        for (iSegmentIDX = 0; iSegmentIDX < mlstSegCount[iMeasureIDX]; iSegmentIDX++)
                        {
                            maaclsDigitalMeter[iMeasureIDX, iSegmentIDX] = new LBSoft.IndustrialCtrls.Leds.LB7SegmentDisplay();

                            if ((iSegmentIDX == (mlstPreDPCount[iMeasureIDX] - 1)) &&
                                (mlstPreDPCount[iMeasureIDX] != mlstSegCount[iMeasureIDX]))
                            {
                                maaclsDigitalMeter[iMeasureIDX, iSegmentIDX].ShowDP = true;
                            }

                            maaclsDigitalMeter[iMeasureIDX, iSegmentIDX].Text = iSegmentIDX.ToString();
                            maaclsDigitalMeter[iMeasureIDX, iSegmentIDX].Left = 100 + 25 * iSegmentIDX;
                            maaclsDigitalMeter[iMeasureIDX, iSegmentIDX].Top = 40 + 25 * iMeasureIDX;
                            maaclsDigitalMeter[iMeasureIDX, iSegmentIDX].Width = 70;
                            maaclsDigitalMeter[iMeasureIDX, iSegmentIDX].BackColor = Color.FromArgb(255, 255, 255, 200);
                            maaclsDigitalMeter[iMeasureIDX, iSegmentIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                            maaclsDigitalMeter[iMeasureIDX, iSegmentIDX].ForeColor = Color.OrangeRed;
                            this.Controls.Add(maaclsDigitalMeter[iMeasureIDX, iSegmentIDX]);
                        }

                        maclsMeasureLabel[iMeasureIDX] = new Label();
                        maclsMeasureLabel[iMeasureIDX].AutoSize = false;
                        maclsMeasureLabel[iMeasureIDX].TextAlign = ContentAlignment.MiddleRight;
                        maclsMeasureLabel[iMeasureIDX].Text = maclsWindowElement[iElementIDX].szLabel;
                        maclsMeasureLabel[iMeasureIDX].Left = 20;
                        maclsMeasureLabel[iMeasureIDX].Top = 40 + 25 * iMeasureIDX;
                        maclsMeasureLabel[iMeasureIDX].Width = 120;
                        maclsMeasureLabel[iMeasureIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                        maclsMeasureLabel[iMeasureIDX].ForeColor = Color.OrangeRed;
                        this.Controls.Add(maclsMeasureLabel[iMeasureIDX]);

                        iMeasureIDX++;
                    }
                }

                if (0 == String.Compare(maclsWindowElement[iElementIDX].szElementType, "window"))
                {
                    this.Text = ConstantData.APPCONFIG.szAppName + ":" + maclsWindowElement[iElementIDX].szLabel;
                }
            }


            iVerticalElementCount = iMeasureIDX;

            if (this.Height < 80 + 25 * iVerticalElementCount)
            {
                this.Height = 80 + 25 * iVerticalElementCount;
            }

            if ((true == boSetup) && (0 < iMeasureIDX))
            {
                tclsASAM.vCalcDDDIAndSetRate(miFormIDX, 500);
            }
            //else
            //{
                this.OnResize(EventArgs.Empty);
            //}

            mboSuspendUpdates = false;
        }

        private void RemoveControls()
        {
            if (null != maclsMeasureLabel)
            {
                foreach (Label label in maclsMeasureLabel)
                {
                    this.Controls.Remove(label);
                }
            }

            if (null != maaclsDigitalMeter)
            {
                foreach (LBSoft.IndustrialCtrls.Leds.LB7SegmentDisplay meter in maaclsDigitalMeter)
                {
                    this.Controls.Remove(meter);
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

                        int iSegmentIDX;
                        Single sScaledData;

                        switch (tclsASAM.milstCompuMethodList[iMeasCompuMethodIDX].enCM_Type)
                        {
                            case tenCM_Type.enIDENTICAL:
                                {
                                    sScaledData = sUnscaledData;
                                    break;
                                }
                            case tenCM_Type.enLINEAR:
                                {
                                    sScaledData = tclsASAM.milstCompuMethodList[iMeasCompuMethodIDX].sCoeff1 * sUnscaledData;
                                    sScaledData += tclsASAM.milstCompuMethodList[iMeasCompuMethodIDX].sCoeff2;
                                    break;
                                }
                            default:
                                {
                                    sScaledData = sUnscaledData;
                                    break;
                                }
                        }

                        int iLoopIDX;

                        for (iLoopIDX = 0; iLoopIDX < (mlstSegCount[iMeasIDX] - mlstPreDPCount[iMeasIDX]); iLoopIDX++)
                        {
                            sScaledData *= 10;
                        }

                        for (iSegmentIDX = mlstSegCount[iMeasIDX] - 1; 0 <= iSegmentIDX; iSegmentIDX--)
                        {
                            if ((1 <= sScaledData) ||
                                (iSegmentIDX == mlstSegCount[iMeasIDX] - 1))
                            {
                                maaclsDigitalMeter[iMeasIDX, iSegmentIDX].val = (int)sScaledData % 10;
                                maaclsDigitalMeter[iMeasIDX, iSegmentIDX].Refresh();
                                sScaledData /= 10;
                            }
                            else
                            {
                                maaclsDigitalMeter[iMeasIDX, iSegmentIDX].val = -1;
                                maaclsDigitalMeter[iMeasIDX, iSegmentIDX].Refresh();
                            }


                        }                        
                    }

                    iMeasIDX++; 
                }
            }
        }




        public void vSetInitialise(bool boInitialise)
        {
            mboInitialising = boInitialise;
        }

        private void tclsICMeasSegmentView_Load(object sender, EventArgs e)
        {
            this.Left = 150;
        }

        private void tclsICMeasSegmentView_Resize(object sender, System.EventArgs e)
        {
            int iIDX = 0;

            if (null != maclsMeasureLabel)
            {
            if (maclsMeasureLabel.Length != 0)
            {
                iIDX = 0;

                foreach (Label label in maclsMeasureLabel)
                {
                    if (null != label)
                    {
                        label.Left = 2 * this.Width / 40;
                        label.Width = 5 * this.Width / 16;
                        label.Font = new Font(label.Font.FontFamily, this.Width / 45);
                        label.Height = (this.Height - 35) / (maclsMeasureLabel.Length + 1);
                        label.Top = this.Height / 9 + iIDX * ((this.Height - 20) / (maclsMeasureLabel.Length + 1));
                        label.TextAlign = ContentAlignment.TopRight;
                    }

                    iIDX++;
                }


                iIDX = 0;
                int iRowIDX = 0;
                int iColIDX = 0;

                for (iRowIDX = 0; iRowIDX < maaclsDigitalMeter.GetLength(0); iRowIDX++)
                {
                    for (iColIDX = 0; iColIDX < maaclsDigitalMeter.GetLength(1); iColIDX++)
                    {
                        if (null != maaclsDigitalMeter[iRowIDX, iColIDX])
                        {
                            maaclsDigitalMeter[iRowIDX, iColIDX].Left = 3 * this.Width / 8 + iColIDX * this.Width / (2 * mlstSegCount[iRowIDX]);
                            maaclsDigitalMeter[iRowIDX, iColIDX].Width = 9 * this.Width / (16 * mlstSegCount[iRowIDX]);
                            maaclsDigitalMeter[iRowIDX, iColIDX].Height = (this.Height - 35) / (maclsMeasureLabel.Length + 1);
                            maaclsDigitalMeter[iRowIDX, iColIDX].Top = this.Height / 9 + iRowIDX * ((this.Height - 20) / (maclsMeasureLabel.Length + 1));
                            }
                        }
                    }
                }
            }
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


        public void RequestShowViewIndex(int iFormReqIDX)
        {
            foreach (int iFormIDX in mlstFormIndices)
            {
                if (iFormIDX == iFormReqIDX)
                {
                    Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);
                    miFormIDX = iFormIDX;
                    SetWindowView(false);
                    Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);
                    break;
                }
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButtonNext_Click(object sender, EventArgs e)
        {
            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestSuspend, 0, null);
            GetNextFormIDX();
            SetWindowView(false);
            Program.vNotifyProgramEvent(tenProgramEvent.enCommRequestUnSuspend, 0, null);
        }

        private void toolStripButtonPrev_Click(object sender, EventArgs e)
        {
            GetPrevFormIDX();
            SetWindowView(false);
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
