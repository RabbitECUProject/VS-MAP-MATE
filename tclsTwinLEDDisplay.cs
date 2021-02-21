/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Twin LED Display                                       */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsTwinLEDDisplay.cs                                  */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP
{
    public partial class tclsTwinLEDDisplay : UserControl
    {
        LBSoft.IndustrialCtrls.Leds.LB7SegmentDisplay[,] mLEDDisplays;
        Label[] mLEDLabels;

        public tclsTwinLEDDisplay()
        {
            InitializeComponent();
        }

        private void tclsTwinLEDDisplay_Load(object sender, EventArgs e)
        {

        }

        public void DisplayBuild()
        {
            mLEDDisplays = new LBSoft.IndustrialCtrls.Leds.LB7SegmentDisplay[2, 4];

            int iSegmentIDX;

            for (iSegmentIDX = 0; iSegmentIDX < 4; iSegmentIDX++)
            {
                mLEDDisplays[0, iSegmentIDX] = new LBSoft.IndustrialCtrls.Leds.LB7SegmentDisplay();
                mLEDDisplays[1, iSegmentIDX] = new LBSoft.IndustrialCtrls.Leds.LB7SegmentDisplay();

                mLEDDisplays[0, iSegmentIDX].Text = "0";
                mLEDDisplays[0, iSegmentIDX].Left = 80 + 20 * iSegmentIDX;
                mLEDDisplays[0, iSegmentIDX].Top = 0;
                mLEDDisplays[0, iSegmentIDX].Width = 20;
                mLEDDisplays[0, iSegmentIDX].Height = 40;
                mLEDDisplays[0, iSegmentIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                mLEDDisplays[0, iSegmentIDX].ForeColor = Color.OrangeRed;

                mLEDDisplays[1, iSegmentIDX].Text = "0";
                mLEDDisplays[1, iSegmentIDX].Left = 240 + 20 * iSegmentIDX;
                mLEDDisplays[1, iSegmentIDX].Top = 0;
                mLEDDisplays[1, iSegmentIDX].Width = 20;
                mLEDDisplays[1, iSegmentIDX].Height = 40;
                mLEDDisplays[1, iSegmentIDX].BackColor = Color.FromArgb(255, 32, 32, 32);
                mLEDDisplays[1, iSegmentIDX].ForeColor = Color.OrangeRed;

                this.Controls.Add(mLEDDisplays[0, iSegmentIDX]);
                this.Controls.Add(mLEDDisplays[1, iSegmentIDX]);
            }


            mLEDLabels = new Label[2];
            mLEDLabels[0] = new Label();
            mLEDLabels[0].Width = 80;
            mLEDLabels[0].Left = 0;
            mLEDLabels[0].BackColor = Color.FromArgb(255, 32, 32, 32);
            mLEDLabels[0].ForeColor = Color.OrangeRed;
            mLEDLabels[0].Font = new Font(mLEDLabels[0].Font.FontFamily, 10);
            mLEDLabels[0].TextAlign = ContentAlignment.TopRight;

            mLEDLabels[1] = new Label();
            mLEDLabels[1].Width = 80;
            mLEDLabels[1].Left = 160;
            mLEDLabels[1].BackColor = Color.FromArgb(255, 32, 32, 32);
            mLEDLabels[1].ForeColor = Color.OrangeRed;
            mLEDLabels[1].Font = new Font(mLEDLabels[0].Font.FontFamily, 10);
            mLEDLabels[1].TextAlign = ContentAlignment.TopRight;

            this.Controls.Add(mLEDLabels[0]);
            this.Controls.Add(mLEDLabels[1]);

        }

        public void SetLabelLeft(String szLabel)
        {
            mLEDLabels[0].Text = szLabel;
        }

        public void SetLabelRight(String szLabel)
        {
            mLEDLabels[1].Text = szLabel;
        }

        public void SetValueLeft(int iValue)
        {
            if (null != mLEDDisplays)
            {
                mLEDDisplays[0, 0].val = (int)(iValue / 1000);
                mLEDDisplays[0, 1].val = (int)((iValue % 1000) / 100);
                mLEDDisplays[0, 2].val = (int)((iValue % 100) / 10);
                mLEDDisplays[0, 3].val = (int)(iValue % 10);
                mLEDDisplays[0, 0].Refresh();
                mLEDDisplays[0, 1].Refresh();
                mLEDDisplays[0, 2].Refresh();
                mLEDDisplays[0, 3].Refresh();
            }
        }

        public void SetValueRight(int iValue)
        {
            if (null != mLEDDisplays)
            {
                mLEDDisplays[1, 0].val = (int)(iValue / 1000);
                mLEDDisplays[1, 1].val = (int)((iValue % 1000) / 100);
                mLEDDisplays[1, 2].val = (int)((iValue % 100) / 10);
                mLEDDisplays[1, 3].val = (int)(iValue % 10);
                mLEDDisplays[1, 0].Refresh();
                mLEDDisplays[1, 1].Refresh();
                mLEDDisplays[1, 2].Refresh();
                mLEDDisplays[1, 3].Refresh();
            }
        }

        public void SetDP(int miScaleLeft, int miScaleRight)
        {
            int iIDX;

            for (iIDX = 0; iIDX < 4; iIDX++)
            {
                if (miScaleLeft == iIDX)
                {
                    mLEDDisplays[0, 3 - iIDX].ShowDP = true;
                }
                else
                {
                    mLEDDisplays[0, 3 - iIDX].ShowDP = false;
                }
            }

            for (iIDX = 0; iIDX < 4; iIDX++)
            {
                if (miScaleRight == iIDX)
                {
                    mLEDDisplays[1, 3 - iIDX].ShowDP = true;
                }
                else
                {
                    mLEDDisplays[1, 3 - iIDX].ShowDP = false;
                }
            }
        }
    }
}
