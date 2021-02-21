/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Routine Control                                        */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsRoutineControl.cs                                  */
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
    public partial class tclsRoutineControl : Form
    {
        List<byte> mlstU8RIDList;
        ComboBox mclsRoutinesCombo;
        Button mclsRunButton;
        int miRoutineCount;

        public tclsRoutineControl()
        {
            mlstU8RIDList = new List<byte>();
            miRoutineCount = 1;
            mclsRoutinesCombo = new ComboBox();
            mclsRoutinesCombo.Left = 20;
            mclsRoutinesCombo.Top = 20;
            mclsRunButton = new Button();
            mclsRunButton.Text = "Run Routine";
            mclsRunButton.Click +=
                this.RoutineButtonClicked;
            mclsRunButton.Left = 20 + mclsRoutinesCombo.Width;
            mclsRunButton.Top = 20;
            this.Controls.Add(mclsRoutinesCombo);
            this.Controls.Add(mclsRunButton);
            this.Text = "User Routine Control";
            this.Width = 40 + mclsRoutinesCombo.Width
                            + mclsRunButton.Width;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            InitializeComponent();
        }

        private void tclsRoutineControl_Load(object sender, EventArgs e)
        {
            int iRoutIDX = 1;

            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Routines.INI");

            while (1000 > iRoutIDX)
            {
                string szRoutineIDXString = "Routine" + iRoutIDX.ToString();
                string szRoutineString = "0:0";

                try
                {
                    szRoutineString = mclsIniParser.GetSetting("UserRoutineList", szRoutineIDXString);
                }
                catch
                {
                    szRoutineString = null;
                }

                if ((1000 > iRoutIDX) && (null != szRoutineString))
                {
                    string[] aszRoutineString = szRoutineString.Split(':');

                    mclsRoutinesCombo.Items.Add(aszRoutineString[0]);
                    mlstU8RIDList.Add((byte)(128 + Convert.ToUInt16(aszRoutineString[1])));
                    iRoutIDX++;
                }
                else
                {
                    if (1 == iRoutIDX)
                    {
                        MessageBox.Show("No user defined routines were found in Routines.ini", "Important");
                    }
                    break;
                }
            }
        }

        private void RoutineButtonClicked(object sender, EventArgs e)
        {
            int iRoutineIDX = mclsRoutinesCombo.SelectedIndex;

            if ((-1 < iRoutineIDX) && (mlstU8RIDList.Count > iRoutineIDX))
            {
                bool boResult = Program.mAPP_clsUDPComms.boRequestUserRoutineControl(mlstU8RIDList[iRoutineIDX], true);
            }
        }
    }
}