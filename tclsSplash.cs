/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Splash Screen                                          */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsSplash.cs                                          */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System.Diagnostics;
using System.Windows.Forms;

namespace UDP
{
    public partial class tclsSplash : Form
    {
        int m_AppVersion;
        public tclsSplash()
        {
            InitializeComponent();
            labelSplash.Refresh();

            System.Reflection.Assembly clsAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo clsFVI = FileVersionInfo.GetVersionInfo(clsAssembly.Location);
            m_AppVersion = clsFVI.FilePrivatePart;

            labelSplash.Text = "Version " + m_AppVersion.ToString() + ", Loading Application..";
        }

        public void ClockSplash()
        {
            Invoke((MethodInvoker)delegate
            {
                if (labelSplash.Text.Length > 38)
                {
                    labelSplash.Text = "Version " + m_AppVersion.ToString() + ", Loading Application..";
                    labelSplash.Refresh();
                }
                else
                {
                    labelSplash.Text += ".";
                    labelSplash.Refresh();
                }
            });
        }
    }
}
