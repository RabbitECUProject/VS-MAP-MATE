/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      PDF Viewer                                             */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsPDFView.cs                                         */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP
{
    public partial class tclsPDFView : Form
    {
        dynamic maxAcroPDF1;
        public bool AcroLoadOK;

        public tclsPDFView(String szPDFPath)
        {
            bool PDFOK = false;

            InitializeComponent();

            try
            {
                var DLL = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "References\\AxInterop.AcroPDFLib.dll");

                maxAcroPDF1 = DLL.CreateInstance("AxAcroPDFLib.AxAcroPDF");

                //axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsPDFView));
                ((System.ComponentModel.ISupportInitialize)(maxAcroPDF1)).BeginInit();
                this.SuspendLayout();
                // 
                // axAcroPDF1
                // 
                maxAcroPDF1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                maxAcroPDF1.Enabled = true;
                maxAcroPDF1.Location = new System.Drawing.Point(12, 12);
                maxAcroPDF1.Name = "axAcroPDF1";
                maxAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
                maxAcroPDF1.Size = new System.Drawing.Size(609, 373);
                maxAcroPDF1.TabIndex = 0;
                //this.axAcroPDF1.OnError += new System.EventHandler(this.axAcroPDF1_OnError);
                // 
                // 
                // tclsPDFView
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(633, 436);
                this.Controls.Add(maxAcroPDF1);
                this.MinimizeBox = false;
                this.Name = "tclsPDFView";
                this.Text = "Help PDF View";
                ((System.ComponentModel.ISupportInitialize)(maxAcroPDF1)).EndInit();
                this.ResumeLayout(false);
                PDFOK = true;
                AcroLoadOK = true;
            }
            catch
            {
                PDFOK = false;
            }

            if (false == PDFOK)
            {
                MessageBox.Show("Integrated PDF view could not be started, you should open help file manually from " + AppDomain.CurrentDomain.BaseDirectory + "Manuals");
            }
        }

        private void axAcroPDF1_OnError(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenPDF();
        }

        private void OpenPDF()
        { 
            OpenFileDialog clsFileDialog = new OpenFileDialog();
            clsFileDialog.Title = "Open Help File";
            clsFileDialog.DefaultExt = "PDF:pdf";
            clsFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            clsFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Help Files";
            DialogResult Result = clsFileDialog.ShowDialog();

            if (DialogResult.OK == Result)
            {
                maxAcroPDF1.src = clsFileDialog.FileName;
            }
        }

        private void tclsPDFView_Load(object sender, EventArgs e)
        {
            OpenPDF();
        }
    }
}
