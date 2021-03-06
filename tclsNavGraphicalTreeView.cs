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
    public partial class tclsNavGraphicalTreeView : Form
    {
        List<KeyValuePair<string, int>> mlstKVP;
        bool mboRequestShutdown;

        public tclsNavGraphicalTreeView()
        {
            InitializeComponent();
            mlstKVP = new List<KeyValuePair<string, int>>();
        }

        public void vAddViewNode(string szNodeText, int iFormIDX, int iMDIChildIDX, tenWindowChildType enWindowChildType)
        {
            TreeNode viewNode = new TreeNode(szNodeText, (int)enWindowChildType + 9, (int)enWindowChildType + 9);
            navTreeViewGraphicalNodes.Nodes[0].Nodes[(int)enWindowChildType].Nodes.Add(viewNode);
            KeyValuePair<string, int> kvp = new System.Collections.Generic.KeyValuePair<string, int>(szNodeText, (iMDIChildIDX << 8) + iFormIDX);
            mlstKVP.Add(kvp);
        }

        private void tclsNavGraphicalTreeView_Load(object sender, EventArgs e)
        {
            TreeNode viewRootNode = new TreeNode("ECU", 18, 18);
            navTreeViewGraphicalNodes.Nodes.Add(viewRootNode);
            navTreeViewGraphicalNodes.Nodes[0].Expand();

            TreeNode viewTypesNode = new TreeNode("Data & Settings", 0, 9);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);
            navTreeViewGraphicalNodes.Nodes[0].Nodes[0].Expand();

            viewTypesNode = new TreeNode("Tables", 1, 1);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);
            navTreeViewGraphicalNodes.Nodes[0].Nodes[1].Expand();

            viewTypesNode = new TreeNode("Maps", 2, 2);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);
            navTreeViewGraphicalNodes.Nodes[0].Nodes[2].Expand();

            viewTypesNode = new TreeNode("Logging", 3, 3);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);
            navTreeViewGraphicalNodes.Nodes[0].Nodes[3].Expand();

            viewTypesNode = new TreeNode("Gauges", 4, 4);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);
            navTreeViewGraphicalNodes.Nodes[0].Nodes[4].Expand();

            viewTypesNode = new TreeNode("Config", 5, 5);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);

            viewTypesNode = new TreeNode("Segment Displays", 6, 6);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);

            viewTypesNode = new TreeNode("Logic Blocks", 7, 7);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);

            viewTypesNode = new TreeNode("DSG", 8, 8);
            navTreeViewGraphicalNodes.Nodes[0].Nodes.Add(viewTypesNode);
        }

        private void navTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (null != e.Node.Parent)
            {
                if (null != e.Node.Parent.Parent)
                {
                    foreach (KeyValuePair<string, int> kvp in mlstKVP)
                    {
                        if (e.Node.Text == kvp.Key)
                        {
                            Program.vNotifyProgramState(UDP.tenProgramState.enRequestViewFocus, kvp.Value);
                        }
                    }
                }
            }
        }

        private void tclsNavTreeView_Resize(object sender, System.EventArgs e)
        {
            if (this.Width > System.Windows.SystemParameters.PrimaryScreenWidth / 4)
            {
                this.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth / 4;
            }
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

        private void tclsNavTree_LocationChanged(object sender, EventArgs e)
        {
            Location = new Point(0, 0);
        }
    }
}
