/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Navigation Tree                                        */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsNavTreeView.cs                                     */
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
    public partial class tclsNavTreeView : Form
    {
        int[] maMDIChildIDX;
        bool mboRequestShutdown;

        public tclsNavTreeView()
        {
            InitializeComponent();
            navTreeView.Font = Program.GetTextFont();
        }

        private void tclsNavTreeView_Load(object sender, EventArgs e)
        {
            TreeNode viewRootNode = new TreeNode("Views");
            navTreeView.Nodes.Add(viewRootNode);
        }

        public void vAddViewNode(string szNodeText, int iFormIDX, int iMDIChildIDX)
        {
            TreeNode viewNode = new TreeNode(szNodeText);
            navTreeView.Nodes[0].Nodes.Add(viewNode);
            navTreeView.Nodes[0].Expand();

            Array.Resize(ref maMDIChildIDX, iFormIDX + 1);

            maMDIChildIDX[iFormIDX] = iMDIChildIDX - 1;
        }

        private void navTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Program.vNotifyProgramState(UDP.tenProgramState.enRequestViewFocus, maMDIChildIDX[e.Node.Index] + (e.Node.Index << 8));
        }

        private void tclsNavTreeView_Resize(object sender, System.EventArgs e)
        {
            navTreeView.Left = 5;
            navTreeView.Top = 5;
            navTreeView.Width = this.Width - 27;
            navTreeView.Height = this.Height - 50;
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
                if (e.CloseReason == CloseReason.UserClosing)
                {
                e.Cancel = true;
                    MessageBox.Show("View can't be minimised or closed", "Not possible to close navigation view");
                }
                else
                {
                    e.Cancel = true;
                    MessageBox.Show("Please select Application->Exit to shut down ");
                }
            }
        }
    }
}