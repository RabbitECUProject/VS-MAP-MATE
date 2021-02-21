/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Notify View                                            */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsNotify.cs                                          */
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
    public partial class tclsNotify : Form
    {
        bool mboRequestShutdown;

        public tclsNotify()
        {
            InitializeComponent();
        }

        private void tclsNotify_Load(object sender, EventArgs e)
        {

        }

        public void vSetNotices(string szTitleString, string szNotifyString)
        {
            NotifyTextBox.Text += (szNotifyString + "\r\n");
            this.Text = szTitleString;
        }

        public void vAppendNotices(string szTitleString, string szNotifyString)
        {
            Invoke((MethodInvoker)delegate
            {
                if (5000 > NotifyTextBox.Text.Length)
                {
                    NotifyTextBox.Text += (szNotifyString + "\r\n");
                }
                else
                {
                    NotifyTextBox.Text = szNotifyString + "\r\n";
                }

                this.Text = szTitleString;

            });
        }

        public void vClearNotices()
        {
            Invoke((MethodInvoker)delegate
            {
                NotifyTextBox.Text = "";
            });
        }

        private void tclsNotify_Resize(object sender, System.EventArgs e)
        {
            NotifyTextBox.Left = 5;
            NotifyTextBox.Top = 5;
            NotifyTextBox.Width = this.Width - 27;
            NotifyTextBox.Height = this.Height - 50;
        }

        public void vRequestShutdown()
        {
            mboRequestShutdown = true;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((true == mboRequestShutdown) || (true == ((Form)sender).Modal))
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