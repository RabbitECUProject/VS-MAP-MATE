/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Measure Select                                         */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsMeasureSelect.cs                                   */
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
    public partial class tclsMeasureSelect : Form
    {
        bool[] mboDrawBold;
        int miFormIDX;
        tclsMeasValueGaugeView mParentForm;

        public tclsMeasureSelect(int iFormIDX, Form parentForm)
        {
            miFormIDX = iFormIDX;
            mParentForm = (tclsMeasValueGaugeView)parentForm;
            InitializeComponent();

            comboBoxGaugeVars.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxGaugeVars.DrawItem += new DrawItemEventHandler(comboBoxGaugeVars_DrawItem);

            mboDrawBold = new bool[tclsASAM.milstAvailableMeasList.Count];
            LoadComboBoxLoggingVars();
        }

        private void LoadComboBoxLoggingVars()
        {
            int iMeasureIDX = 0;
            int iOldMeasureSelectedIndex = comboBoxGaugeVars.SelectedIndex;

            comboBoxGaugeVars.Items.Clear();

            foreach (tstMeasurement stMeasurement in tclsASAM.milstAvailableMeasList)
            {
                if (true == MeasureIsActive(stMeasurement.szMeasurementName))
                {
                    mboDrawBold[iMeasureIDX] = true;
                    comboBoxGaugeVars.Items.Add(stMeasurement.szMeasurementName);
                }
                else
                {
                    mboDrawBold[iMeasureIDX] = false;
                    comboBoxGaugeVars.Items.Add(stMeasurement.szMeasurementName);
                }

                iMeasureIDX++;
            }

            comboBoxGaugeVars.Font = Program.GetTextFont();
            comboBoxGaugeVars.SelectedIndex = iOldMeasureSelectedIndex;
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

        private void buttonGaugeAdd_Click(object sender, EventArgs e)
        {
            bool boGaugeAddRemovedOK;

            if (null != comboBoxGaugeVars.SelectedItem)
            {
                string[] aszPresentationOptions = new string[11];
                string szMeasure = comboBoxGaugeVars.SelectedItem.ToString();
                int iMeasureIDX;
                int iCompuMethodIDX;

                iMeasureIDX = tclsASAM.iGetAvailableMeasIndex(szMeasure);
                iCompuMethodIDX = tclsASAM.iGetCompuMethodIndexFromCompuMethod(tclsASAM.milstAvailableMeasList[iMeasureIDX].szCompuMethod);

                if (-1 != iMeasureIDX)
                {
                    aszPresentationOptions[0] = tclsASAM.milstAvailableMeasList[iMeasureIDX].szMeasurementName;
                    aszPresentationOptions[1] = Convert.ToString((int)tclsASAM.milstAvailableMeasList[iMeasureIDX].sLowerLim);
                    aszPresentationOptions[2] = Convert.ToString((int)tclsASAM.milstAvailableMeasList[iMeasureIDX].sUpperLim);
                    aszPresentationOptions[3] = "230";
                    aszPresentationOptions[4] = "-50";
                    aszPresentationOptions[5] = "90";
                    aszPresentationOptions[6] = "45";
                    aszPresentationOptions[7] = "45";
                    aszPresentationOptions[8] = "0";
                    aszPresentationOptions[9] = "0";
                    aszPresentationOptions[10] = "-49";
                }

                boGaugeAddRemovedOK = mParentForm.AddMeasure(szMeasure, aszPresentationOptions);

                if (boGaugeAddRemovedOK)
                {
                    this.Close();
                }
            }
        }

        private void comboBoxGaugeVars_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font font = null;
            Brush brush;
            string text = comboBoxGaugeVars.Items[e.Index].ToString();
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
    }


}
