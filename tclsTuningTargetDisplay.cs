/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Tuning Target Display                                  */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsTuningTarget.cs                                    */
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
    public partial class tclsTuningTargetDisplay : Form
    {
        int miData;
        float mfXPercent;
        float mfYPercent;
        int miDelay;
        int miRate;
        int miStabilityCount;
        float mfRatio;
        int miRetRatio;
        int miXBandOld;
        int miYBandOld;

        public tclsTuningTargetDisplay()
        {
            InitializeComponent();

            this.TwinLEDDisplay.DisplayBuild();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void SetLabels(String szLeftLabel, String szRightLabel)
        {
            this.TwinLEDDisplay.SetLabelLeft(szLeftLabel);
            this.TwinLEDDisplay.SetLabelRight(szRightLabel);
        }

        public void SetValues(int iLeftVal, int iRightVal)
        {
            this.TwinLEDDisplay.SetValueLeft(iLeftVal);
            this.TwinLEDDisplay.SetValueRight(iRightVal);

            mfRatio = (float)iLeftVal / (float)iRightVal;
        }

        public void SetScaling(int iScaleLeft, int iScaleRight)
        {
            this.TwinLEDDisplay.SetDP(iScaleLeft, iScaleRight);
        }

        public int SetTarget(int iSpreadXDividend3D, int iSpreadXRemainder3D, int iSpreadYDividend3D, int iSpreadYRemainder3D)
        {
            bool boBandOK;
            int iXBand;
            int iYBand;

            if (false == this.Visible) return -1;

            mfXPercent = ((iSpreadXRemainder3D + 0x8000) % 0x10000) / 656;
            mfYPercent = ((iSpreadYRemainder3D + 0x8000) % 0x10000) / 656;

            int iXGlobalOffset = iSpreadXDividend3D * 0x10000 + iSpreadXRemainder3D;
            int iYGlobalOffset = iSpreadYDividend3D * 0x10000 + iSpreadYRemainder3D;

            iXBand = (iXGlobalOffset + 0x8000) / 0x10000;
            iYBand = (iYGlobalOffset + 0x8000) / 0x10000;

            boBandOK = (iXBand == miXBandOld) && (iYBand == miYBandOld);

            DrawTarget(mfXPercent, mfYPercent);

            if ((true == boBandOK) && (true == checkBoxEnable.Checked))
            {
                miRetRatio = AutoTuneResult();
            }
            else
            {
                miRetRatio = -1; 
            }

            miXBandOld = iXBand;
            miYBandOld = iYBand;

            return miRetRatio;
        }

        private int AutoTuneResult()
        {
            int iResult = -1;

            if (miDelay < miStabilityCount)
            {
                float div = mfRatio - 1;
                div /= (6 - miRate);
                div += 1;
                iResult = (int)(100f * div);
            }

            return iResult;
        }

        public int GetIndices()
        {
            return ((miXBandOld << 16) + miYBandOld);
        }

        public void DrawTarget(float xPercent, float yPercent)
        {
            if (xPercent < 10)
            {
                mfXPercent = 10;
            }
            else if (xPercent > 90)
            {
                mfXPercent = 90;
            }
            else
            {
                mfXPercent = xPercent;
            }

            if (yPercent < 10)
            {
                mfYPercent = 90;
            }
            else if (yPercent > 90)
            {
                mfYPercent = 10;
            }
            else
            {
                mfYPercent = 100 - yPercent;
            }

            pictureBox.Invalidate();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            /* Outline */
            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point(2, 2),
                new Point(2, pictureBox.Size.Height - 2));

            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point(2, pictureBox.Size.Height - 2),
                new Point(pictureBox.Size.Width - 2, pictureBox.Size.Height - 2));

            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point(pictureBox.Size.Width - 2, pictureBox.Size.Height - 2),
                new Point(pictureBox.Size.Width - 2, 2));

            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point(pictureBox.Size.Width - 2, 2),
                new Point(2, 2));

            /* Target box */
            if ((mfXPercent > 25) && (mfXPercent < 75) && (mfYPercent > 25) && (mfYPercent < 75))
            {
                if (-1 == miRetRatio)
                {
                    e.Graphics.FillRectangle(
                        new SolidBrush(Color.Blue),
                        0.25f * pictureBox.Size.Width,
                        0.25f * pictureBox.Size.Height,
                        0.50f * pictureBox.Size.Width,
                        0.50f * pictureBox.Size.Height);
                }
                else
                {
                    e.Graphics.FillRectangle(
                        new SolidBrush(Color.LightGreen),
                        0.25f * pictureBox.Size.Width,
                        0.25f * pictureBox.Size.Height,
                        0.50f * pictureBox.Size.Width,
                        0.50f * pictureBox.Size.Height);
                }

                miStabilityCount++;
            }
            else
            {
                e.Graphics.FillRectangle(
                    new SolidBrush(Color.IndianRed),
                    0.25f * pictureBox.Size.Width,
                    0.25f * pictureBox.Size.Height,
                    0.50f * pictureBox.Size.Width,
                    0.50f * pictureBox.Size.Height);

                miStabilityCount = 0;
            }

            /* Target box */
            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point((int)(0.25f * pictureBox.Size.Width), (int)(0.25f * pictureBox.Size.Height)),
                new Point((int)(0.25f * pictureBox.Size.Width), (int)(0.75f * pictureBox.Size.Height)));

            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point((int)(0.25f * pictureBox.Size.Width), (int)(0.75f * pictureBox.Size.Height)),
                new Point((int)(0.75f * pictureBox.Size.Width), (int)(0.75f * pictureBox.Size.Height)));

            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point((int)(0.75f * pictureBox.Size.Width), (int)(0.75f * pictureBox.Size.Height)),
                new Point((int)(0.75f * pictureBox.Size.Width), (int)(0.25f * pictureBox.Size.Height)));

            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point((int)(0.75f * pictureBox.Size.Width), (int)(0.25f * pictureBox.Size.Height)),
                new Point((int)(0.25f * pictureBox.Size.Width), (int)(0.25f * pictureBox.Size.Height)));

            /* Guides */
            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point((int)(mfXPercent / 100 * pictureBox.Size.Width), (int)(0.2f * pictureBox.Size.Height)),
                new Point((int)(mfXPercent / 100 * pictureBox.Size.Width), (int)(0.8f * pictureBox.Size.Height)));

            e.Graphics.DrawLine(
                new Pen(Color.OrangeRed, 1f),
                new Point((int)(0.2f * pictureBox.Size.Width), (int)(mfYPercent / 100 * pictureBox.Size.Height)),
                new Point((int)(0.8f * pictureBox.Size.Width), (int)(mfYPercent / 100 * pictureBox.Size.Height)));
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            miRate = trackBarSpeed.Value;
        }

        private void trackBarStability_Scroll(object sender, EventArgs e)
        {
            miDelay = trackBarStability.Value;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void tclsTuningTargetDisplay_Load(object sender, EventArgs e)
        {

        }
    }
}
