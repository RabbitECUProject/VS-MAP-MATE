using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP
{
    public partial class tclsHelpList : Form
    {
        List<Button> mlstCharButtons;
        int[] maiCharIndices;
        public tclsHelpList(int[] charIDXList)
        {
            int charIDX;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tclsHelpList));

            maiCharIndices = charIDXList;
            InitializeComponent();
            this.tableLayoutPanelCharHelp.RowCount = charIDXList.Count();
            mlstCharButtons = new List<Button>();
            tableLayoutPanelCharHelp.RowStyles.Clear();


            for (charIDX = 0; charIDX < charIDXList.Count(); charIDX++)
            {
                Label charLabel = new Label();
                charLabel.BackColor = Color.FromArgb(255, 32, 32, 32);
                charLabel.ForeColor = Color.Aquamarine;
                charLabel.Text = tclsASAM.milstCharacteristicList[charIDXList[charIDX]].szCharacteristicName;
                charLabel.AutoSize = true;
                charLabel.TextAlign = ContentAlignment.BottomRight;
                tableLayoutPanelCharHelp.Controls.Add(charLabel, 0, charIDX);

                Label charLabelDetail = new Label();
                charLabelDetail.BackColor = Color.FromArgb(255, 32, 32, 32);
                charLabelDetail.ForeColor = Color.Aquamarine;

                charLabelDetail.Text = "Min: " + tclsASAM.milstCharacteristicList[charIDXList[charIDX]].sLowerLim.ToString() +
                                         "     Max: " + tclsASAM.milstCharacteristicList[charIDXList[charIDX]].sUpperLim.ToString();
                charLabelDetail.AutoSize = true;
                charLabelDetail.TextAlign = ContentAlignment.BottomRight;
                tableLayoutPanelCharHelp.Controls.Add(charLabelDetail, 1, charIDX);

                Button charHelpButton = new Button();
                Bitmap helpImage = Properties.Resources.HELP_36;
                charHelpButton.BackgroundImage = helpImage;
                charHelpButton.UseVisualStyleBackColor = true;
                charHelpButton.Width = 24;
                charHelpButton.Height = 24;
                charHelpButton.ImageAlign = ContentAlignment.MiddleCenter;
                charHelpButton.BackgroundImageLayout = ImageLayout.Stretch;
                mlstCharButtons.Add(charHelpButton);
                charHelpButton.Click += new EventHandler(CharHelpButtonClicked);
                tableLayoutPanelCharHelp.Controls.Add(charHelpButton, 2, charIDX);
                tableLayoutPanelCharHelp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }


            this.Height = 60 + (30 * charIDX);
            this.Width = 550;

            //this.Show();
        }

        private void CharHelpButtonClicked(object sender, EventArgs e)
        {
            int iControlIDX;
            int iCharacteristicIDX;
            bool boHTMLShown = false;

            iControlIDX = mlstCharButtons.IndexOf((Button)sender);
            iCharacteristicIDX = maiCharIndices[iControlIDX];


            if (tclsASAM.milstCharacteristicList[iCharacteristicIDX].szInfoString.Contains("HTML"))
            {
                tclsHTMLHelp clsHTMLHelp;

                int iLen = tclsASAM.milstCharacteristicList[iCharacteristicIDX].szInfoString.Length;
                int iPos = tclsASAM.milstCharacteristicList[iCharacteristicIDX].szInfoString.IndexOf("HTML");
                String szHTMLResource = tclsASAM.milstCharacteristicList[iCharacteristicIDX].szInfoString.Substring(iPos, iLen - iPos);

                while (szHTMLResource.Contains(" "))
                {
                    szHTMLResource = szHTMLResource.Replace(" ", "");
                }


                szHTMLResource = szHTMLResource.Replace("HTML=", "");
                iPos = szHTMLResource.IndexOf("HTML");
                szHTMLResource = szHTMLResource.Substring(0, iPos) + "HTML";

                String szXMLPath = AppDomain.CurrentDomain.BaseDirectory + "Database\\Help Databases\\" + szHTMLResource;

                if (File.Exists(szXMLPath))
                {
                    clsHTMLHelp = new tclsHTMLHelp(szXMLPath);
                    boHTMLShown = true;
                    clsHTMLHelp.ShowDialog();
                }
            }


            if (false == boHTMLShown)
            {
                int iPos = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString.IndexOf("HTML");
                String szShowString;

                if (-1 != iPos)
                {
                    szShowString = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString.Substring(1, iPos - 2);
                }
                else
                {
                    szShowString = tclsASAM.milstCharacteristicList[maiCharIndices[0]].szInfoString;
                }

                tclsNotify clsNotify = new UDP.tclsNotify();

                clsNotify.Left = this.ClientRectangle.Width / 2;
                clsNotify.Top = this.ClientRectangle.Height / 2;
                clsNotify.vSetNotices("Map Help", szShowString);
                clsNotify.ShowDialog();
            }
        }

        private void tclsHelpList_Load(object sender, EventArgs e)
        {

        }
    }
}
