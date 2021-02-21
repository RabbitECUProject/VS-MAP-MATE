/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      HTML Help                                              */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsHTMLHelp.cs                                        */
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
    public partial class tclsHTMLHelp : Form
    {
        public tclsHTMLHelp(String szHTMLResource)
        {
            InitializeComponent();

            this.webBrowser1.Navigate(@szHTMLResource);
            this.webBrowser1.Show();
            this.webBrowser1.Refresh();
        }
    }
}
