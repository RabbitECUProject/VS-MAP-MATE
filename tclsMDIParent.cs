/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      MDI Parent                                             */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsMDIParent.cs                                       */
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
using System.IO;
using System.Diagnostics;

namespace UDP
{
    public enum tenMDIParentNotify
    {
        enMDIDataPageRefreshed,
        enMDICommTimeout,
        enMDIStartInitialise,
        enMDIEndInitialise,
        enMDIRequestViewFocus,
        enMDIShowError,
        enMDIShowMessage,
        enMDIToolStripOnline,
        enMDIToolStripOffline,
        enMDILoaded,
        enMDIRequestShutdown
    }

    public enum tenMDIViewArrange
    {
        enMDIViewArrangeDefault,
        enMDIViewArrangeSaved
    }

    public partial class tclsMDIParent : Form
    {
        public static List<tclsMeasValueCharView> mlstMeasValueCharView;
        public static List<tclsICMeasSegmentView> mlstMeasSegmentView;
        public static List<tclsMeasCurveMapView> mlstMeasCurveMapView;
        public static List<tclsMeasValueLoggingView> mlstMeasValueLoggingView;
        public static List<tclsMeasValueGaugeView> mlstMeasValueGaugeView;
        public static List<tclsLogicBlockView> mlstLogicBlockView;
        public static List<tclsDSG> mlstDSGView;
        tclsNavTreeView mclsNavTreeView;
        tclsBlobSettings mclsBlobSettings;
        List<Form> mlstChildViews;
        List<tclsMeasValueLoggingView> mlstLoggingViews;
        tclsNotify mclsNotify;
        long mu64CommsCount;
        long mu64Delta;
        long mu64OldTicks;
        bool mboRequestShutdown;
        MicroLibrary.MicroTimer mclsCommsTimer = new MicroLibrary.MicroTimer();
        string mszGUILayout;

        public tclsMDIParent()
        {
            string szComsTickUS;
            string szGUILayoutXMLPath;
            long lComsTickUS;

            InitializeComponent();
            mboRequestShutdown = false;
            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHost Calibration.INI");

            try
            {
                szComsTickUS = mclsIniParser.GetSetting("Program", "ComsTickus");
            }
            catch
            {
                szComsTickUS = "Unknown";
            }

            try
            {
                szGUILayoutXMLPath = AppDomain.CurrentDomain.BaseDirectory + "Database\\GUI Layout Databases\\" + mclsIniParser.GetSetting("Databases", "SelectedLayoutXML");
            }
            catch
            {
                szGUILayoutXMLPath = "Unknown";
            }

            if (File.Exists(szGUILayoutXMLPath))
            {
                Program.mAPP_clsXMLConfig.ReadWindowConfigFile(szGUILayoutXMLPath);
            }

            mlstMeasValueCharView = new List<tclsMeasValueCharView>();
            mlstMeasSegmentView = new List<tclsICMeasSegmentView>();
            mlstMeasCurveMapView = new List<tclsMeasCurveMapView>();
            mlstMeasValueLoggingView = new List<tclsMeasValueLoggingView>();
            mlstMeasValueGaugeView = new List<tclsMeasValueGaugeView>();
            mlstLogicBlockView = new List<tclsLogicBlockView>();
            mlstDSGView = new List<tclsDSG>();
            mclsCommsTimer = new MicroLibrary.MicroTimer();
            mclsCommsTimer.MicroTimerElapsed += new MicroLibrary.MicroTimer.MicroTimerElapsedEventHandler(OnMicroTimerEvent);
            mlstChildViews = new List<Form>();
            mlstLoggingViews = new List<tclsMeasValueLoggingView>();

            lComsTickUS = Convert.ToUInt32(szComsTickUS);

            if ((ConstantData.APPDATA.ru32ComsTickMin <= lComsTickUS) &&
                (ConstantData.APPDATA.ru32ComsTickMax >= lComsTickUS))
            {
                mclsCommsTimer.Interval = lComsTickUS;
                mclsCommsTimer.Enabled= true;
            }

            mclsNavTreeView = new tclsNavTreeView();
            mclsNavTreeView.MdiParent = this;
            mclsNavTreeView.Show();

            mclsNotify = new tclsNotify();
            mclsNotify.MdiParent = this;
            mclsNotify.Left = 0;
            mclsNotify.Top = this.ClientRectangle.Height - 100;
            mclsNotify.Width = this.ClientRectangle.Width;
            mclsNotify.Show();
        }

        private void FormUDP_Load(object sender, EventArgs e)
        {
            int iFormIDX = 0;
            string szGUILayoutXMLPath;
            string[] aszGUILayout;
            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHOST Calibration.INI");
            int iMeasTablesViews = 0;
            int iMeasMapsViews = 0;
            int iMeasValueCharViews = 0;
            int iMeasSegmentViews = 0;
            int iDSGViews = 0;
            int iMapViewCountMax = 1;
            int iTableViewCountMax = 1;
            int iCharViewCountMax = 1;
            int iSegmentViewCountMax = 1;
            int iGaugeViewCountMax = 1;
            //todoint iDSGViewCountMax = 1;
            int iLogicBlockViews = 0;
            //todoint iLogicBlockViewCountMax = 1;
            int iMeasValueCharViewLastChildIndex = 1;
            int iMeasSegmentViewLastChildIndex = 1;
            int iMeasTableViewLastChildIndex = 1;
            int iLoggingViewLastChildIndex = 1;
            int iMeasMapViewLastChildIndex = 1;
            int iMeasGaugeViewLastChildIndex = 1;
            int iMeasCharConfigViewLastChildIndex = 1;
            int iLogicBlockViewLastChildIndex = 1;
            string szSegmentViewCountMax;
            string szMapViewCountMax;
            string szTableViewCountMax;
            string szCharViewCountMax;
            string szGaugeViewCountMax;


            try
            {
                szGUILayoutXMLPath = AppDomain.CurrentDomain.BaseDirectory + "Database\\GUI Layout Databases\\" + mclsIniParser.GetSetting("Databases", "SelectedLayoutXML");
            }
            catch
            {
                szGUILayoutXMLPath = AppDomain.CurrentDomain.BaseDirectory + "Database\\Unknown.XML";
            }

            try
            {
                szMapViewCountMax = mclsIniParser.GetSetting("Program", "MapViewCountMax");
                iMapViewCountMax = Convert.ToInt32(szMapViewCountMax);
            }
            catch
            {
                szMapViewCountMax = null;
            }

            try
            {
                szTableViewCountMax = mclsIniParser.GetSetting("Program", "TableViewCountMax");
                iTableViewCountMax = Convert.ToInt32(szTableViewCountMax);
            }
            catch
            {
                szTableViewCountMax = null;
            }

            try
            {
                szCharViewCountMax = mclsIniParser.GetSetting("Program", "CharViewCountMax");
                iCharViewCountMax = Convert.ToInt32(szCharViewCountMax);
            }
            catch
            {
                szCharViewCountMax = null;
            }

            try
            {
                szGaugeViewCountMax = mclsIniParser.GetSetting("Program", "GaugeViewCountMax");
                iGaugeViewCountMax = Convert.ToInt32(szGaugeViewCountMax);
            }
            catch
            {
                szGaugeViewCountMax = null;
            }

            try
            {
                szSegmentViewCountMax = mclsIniParser.GetSetting("Program", "SegmentViewCountMax");
                iSegmentViewCountMax = Convert.ToInt32(szSegmentViewCountMax);
            }
            catch
            {
                szSegmentViewCountMax = null;
            }

            aszGUILayout = szGUILayoutXMLPath.Split('\\');
                            aszGUILayout = aszGUILayout[aszGUILayout.Length - 1].Split('.');

            while (0 < Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX].Count)
            {
                switch ((tenWindowChildType)Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].iGUILinkIndex)
                {
                    case tenWindowChildType.enMeasValueView:
                        {
                            if (iMeasValueCharViews < 1)
                            {
                                tclsMeasValueCharView clsMeasValueCharView
                                                = new tclsMeasValueCharView(iFormIDX);
                                clsMeasValueCharView.MdiParent = this;
                                clsMeasValueCharView.Show();
                                mlstChildViews.Add(clsMeasValueCharView);
                                mlstMeasValueCharView.Add(clsMeasValueCharView);
                                iMeasValueCharViews++;
                                iMeasValueCharViewLastChildIndex = mlstChildViews.Count;
                            }
                            else
                            {
                                foreach(tclsMeasValueCharView clsMeasValueCharView in mlstMeasValueCharView)
                                {
                                    clsMeasValueCharView.AddViewToList(iFormIDX);
                                }
                            }

                            mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iMeasValueCharViewLastChildIndex);
                            iFormIDX++;
                            break;
                        }
                    case tenWindowChildType.enMeasTableView:
                        {
                            if (iMeasTablesViews < iTableViewCountMax)
                            {
                                tclsMeasCurveMapView clsMeasCurveMapView
                                    = new tclsMeasCurveMapView(iFormIDX, false);
                                clsMeasCurveMapView.MdiParent = this;
                                clsMeasCurveMapView.Show();
                                mlstChildViews.Add(clsMeasCurveMapView);
                                mlstMeasCurveMapView.Add(clsMeasCurveMapView);
                                iMeasTablesViews++;
                                iMeasTableViewLastChildIndex = mlstChildViews.Count;
                            }
                            else
                            {
                                foreach (tclsMeasCurveMapView clsMeasCurveMapView in mlstMeasCurveMapView)
                                {
                                    clsMeasCurveMapView.AddViewToList(iFormIDX, false);
                                }
                            }

                            mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iMeasTableViewLastChildIndex);
                            iFormIDX++;
                            break;
                        }
                    case tenWindowChildType.enLoggingView:
                        {
                            tclsMeasValueLoggingView clsMeasValueLoggingView
                                = new tclsMeasValueLoggingView(iFormIDX);
                            clsMeasValueLoggingView.MdiParent = this;
                            clsMeasValueLoggingView.Show();
                            mlstChildViews.Add(clsMeasValueLoggingView);
                            mlstLoggingViews.Add(clsMeasValueLoggingView);
                            mlstMeasValueLoggingView.Add(clsMeasValueLoggingView);

                            iLoggingViewLastChildIndex = mlstChildViews.Count;
                            mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iLoggingViewLastChildIndex);
                            iFormIDX++;

                            break;
                        }
                    case tenWindowChildType.enMeasMapView:
                        {
                            if (iMeasMapsViews < iMapViewCountMax)
                            {
                                tclsMeasCurveMapView clsMeasCurveMapView
                                = new tclsMeasCurveMapView(iFormIDX, true);
                                clsMeasCurveMapView.MdiParent = this;
                                clsMeasCurveMapView.Show();
                                mlstChildViews.Add(clsMeasCurveMapView);
                                mlstMeasCurveMapView.Add(clsMeasCurveMapView);
                                iMeasMapsViews++;
                                iMeasMapViewLastChildIndex = mlstChildViews.Count;
                            }
                            else
                            {
                                foreach(tclsMeasCurveMapView clsMeasCurveMapView in mlstMeasCurveMapView)
                                {
                                    clsMeasCurveMapView.AddViewToList(iFormIDX, true);
                                }                                                        
                            }

                            mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iMeasMapViewLastChildIndex);
                            iFormIDX++;
                            break;
                        }
                    case tenWindowChildType.enMeasGaugeView:
                        {
                            tclsMeasValueGaugeView clsMeasValueGaugeView
                                = new tclsMeasValueGaugeView(iFormIDX);
                            clsMeasValueGaugeView.MdiParent = this;
                            clsMeasValueGaugeView.Show();
                            mlstChildViews.Add(clsMeasValueGaugeView);
                            mlstMeasValueGaugeView.Add(clsMeasValueGaugeView);
                            iMeasGaugeViewLastChildIndex = mlstChildViews.Count;
                            mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iMeasGaugeViewLastChildIndex);

                            iFormIDX++;
                            break;
                        }
                    case tenWindowChildType.enMeasCharConfigView:
                        {
                            if (null == mclsBlobSettings)
                            {
                                mclsBlobSettings = new tclsBlobSettings(iFormIDX);
                                mclsBlobSettings.MdiParent = this;
                                mclsBlobSettings.Show();
                                mlstChildViews.Add(mclsBlobSettings);
                                iMeasCharConfigViewLastChildIndex = mlstChildViews.Count;
                                mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iMeasCharConfigViewLastChildIndex);
                            }
                            else
                            {
                                Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0,
                                    "Configuration view is already loaded!");
                            }

                            iFormIDX++;
                            break;
                        }
                    case tenWindowChildType.enMeasSegmentView:
                        {
                            if (iMeasSegmentViews < 1)
                            {
                                tclsICMeasSegmentView clsMeasSegmentView
                                                = new tclsICMeasSegmentView(iFormIDX);
                                clsMeasSegmentView.MdiParent = this;
                                clsMeasSegmentView.Show();
                                mlstChildViews.Add(clsMeasSegmentView);
                                mlstMeasSegmentView.Add(clsMeasSegmentView);
                                iMeasSegmentViews++;
                                iMeasSegmentViewLastChildIndex = mlstChildViews.Count;
                            }
                            else
                            {
                                foreach (tclsICMeasSegmentView clsMeasSegmentView in mlstMeasSegmentView)
                                {
                                    clsMeasSegmentView.AddViewToList(iFormIDX);
                                }
                            }

                            mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iMeasSegmentViewLastChildIndex);
                            iFormIDX++;
                            break;
                        }
                    case tenWindowChildType.enLogicBlockView:
                        {
                            tclsLogicBlockView clsLogicBlockView = new tclsLogicBlockView(iFormIDX);
                            clsLogicBlockView.MdiParent = this;
                            clsLogicBlockView.Show();
                            mlstChildViews.Add(clsLogicBlockView);
                            mlstLogicBlockView.Add(clsLogicBlockView);
                            iLogicBlockViews++;
                            iLogicBlockViewLastChildIndex = mlstChildViews.Count;
                            mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iLogicBlockViewLastChildIndex);
                            iFormIDX++;
                            break;
                        }
                    case tenWindowChildType.enDSGView:
                        {
                            if (iDSGViews < 1)
                            {
                                tclsDSG clsDSGView
                                                = new tclsDSG(iFormIDX);
                                clsDSGView.MdiParent = this;
                                clsDSGView.Show();
                                mlstChildViews.Add(clsDSGView);
                                mlstDSGView.Add(clsDSGView);
                                iMeasSegmentViews++;
                                iMeasSegmentViewLastChildIndex = mlstChildViews.Count;
                            }
                            else
                            {
                                foreach (tclsICMeasSegmentView clsMeasSegmentView in mlstMeasSegmentView)
                                {
                                    clsMeasSegmentView.AddViewToList(iFormIDX);
                                }
                            }

                            mclsNavTreeView.vAddViewNode(Program.mAPP_clsXMLConfig.mailstWindowLists[iFormIDX][0].szLabel, iFormIDX, iMeasSegmentViewLastChildIndex);
                            iFormIDX++;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            mszGUILayout = aszGUILayout[0];
            SetMDIText(null);

            this.WindowState = FormWindowState.Maximized;

            ArrangeWindowsFromINI();

            MDIStatusStrip.Items[0].Image = ConnectionImageList.Images[1];
            MDIStatusStrip.Items[0].Text = "OFFLINE";
            MDIStatusStrip.Items[1].Text = "Status: Application started...";

            bool boRetVal = Program.boSetCalibrationPath(AppDomain.CurrentDomain.BaseDirectory + "Database\\Calibration Databases\\default.XML", false);

            Program.vNotifyProgramEvent(tenProgramEvent.enWindowElementsLoaded, 0, null);
        }

        private void SetMDIText(string supplemental)
        {
            System.Reflection.Assembly clsAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo clsFVI = FileVersionInfo.GetVersionInfo(clsAssembly.Location);

            string szMDIText = "MDAC " + ConstantData.APPCONFIG.szAppName + ": version = " + clsFVI.FileVersion + "; Configuration = " + mszGUILayout;

            if (null != supplemental)
            {
                szMDIText += "; Filename = ";
                szMDIText += supplemental;
            }

            this.Text = szMDIText;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == tclsRegisterUSBDeviceNotification.WmDevicechange)
            {
                switch ((int)m.WParam)
                {
                    case tclsRegisterUSBDeviceNotification.DbtDeviceremovecomplete:
                        Program.vNotifyProgramEvent(tenProgramEvent.enUSBDisconnectDetected, 0, "");
                        break;
                    case tclsRegisterUSBDeviceNotification.DbtDevicearrival:
                        Program.vNotifyProgramEvent(tenProgramEvent.enUSBConnectDetected, 0, "");
                        break;
                }
            }
        }

        public void UpdateText1(byte[] arg)
        {
               Invoke((MethodInvoker)delegate
               {
                   //mFormGridView.UpdateText1(arg);
               });
        }

        public void vUpdateCANMsgs(List<tclsCANMsg> lstCANMsgs)
        {
            Invoke((MethodInvoker)delegate
            {
                //mFormGridView.vUpdateCANMsgs(lstCANMsgs);
            });
        }

        public void vDistributeMeasurements(List<UInt32> lstData, int iActiveMeasureIndex)
        {
            if (false == mboRequestShutdown)
            {
                foreach (tclsMeasCurveMapView clsMeasCurveMapView in mlstMeasCurveMapView)

                    Invoke((MethodInvoker)delegate
                    {
                        clsMeasCurveMapView.vUpdateMeasures(lstData, iActiveMeasureIndex);
                    });

                foreach (tclsICMeasSegmentView clsMeasSegmentView in mlstMeasSegmentView)

                    Invoke((MethodInvoker)delegate
                    {
                        clsMeasSegmentView.vUpdateMeasures(lstData, iActiveMeasureIndex);
                    });

                foreach (tclsMeasValueCharView clsMeasValueCharView in mlstMeasValueCharView)

                    Invoke((MethodInvoker)delegate
                    {
                        clsMeasValueCharView.vUpdateMeasures(lstData, iActiveMeasureIndex);
                    });

                foreach (tclsMeasValueLoggingView clsMeasValueLoggingView in mlstMeasValueLoggingView)

                    Invoke((MethodInvoker)delegate
                    {
                        clsMeasValueLoggingView.vUpdateMeasures(lstData, iActiveMeasureIndex);
                    });

                foreach (tclsMeasValueGaugeView clsMeasValueGaugeView in mlstMeasValueGaugeView)

                    Invoke((MethodInvoker)delegate
                    {
                        clsMeasValueGaugeView.vUpdateMeasures(lstData, iActiveMeasureIndex);
                    });

                foreach (tclsICMeasSegmentView clsMeasSegmentView in mlstMeasSegmentView)

                    Invoke((MethodInvoker)delegate
                    {
                        clsMeasSegmentView.vUpdateMeasures(lstData, iActiveMeasureIndex);
                    });

                foreach (tclsDSG clsDSG in mlstDSGView)

                    Invoke((MethodInvoker)delegate
                    {
                        clsDSG.vUpdateMeasures(lstData, iActiveMeasureIndex);
                    });
            }
        }


        private void DataPageTimer_Tick(object sender, EventArgs e)
        {
            if (false == mboRequestShutdown)
            {
                //bool Written;
                //Written = tclsDataPage.boClockDataPage();

                //if (false == Written)
                //{
                //    Written = tclsDataPage.boClockRegWritePage();
                //}
            }
            else
            {
                if (false == mclsCommsTimer.Enabled)
                {
                    if (5 < mu64CommsCount++)
                    {
                        foreach (Form clsView in this.MdiChildren)
                        {
                            clsView.Close();
                        }
                        vShutdown();
                    }
                }
            }
        }

        private void StripMenuItemUploadCal_Click(object sender, EventArgs e)
        {
            bool boResult = Program.mAPP_clsUDPComms.boRequestCalPageTransfer(tenChannelMode.enChannelModeUploading);

            if (true == boResult)
            {
                Program.vNotifyProgramState(tenProgramState.enProgInitialising, 0);
            }
        }

        private void StripMenuItemDownloadCal_Click(object sender, EventArgs e)
        {
            bool boResult = Program.mAPP_clsUDPComms.boRequestCalPageTransfer(tenChannelMode.enChannelModeDownloading);
        }

        public void vNotify(tenMDIParentNotify enMDIParentNotify, int iData, string szErrorMessage, string szProgressMessage)
        {
            switch (enMDIParentNotify)
            {
                case tenMDIParentNotify.enMDIDataPageRefreshed:
                    {
                        foreach (tclsMeasValueCharView clsMeasValueCharView in mlstMeasValueCharView)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                clsMeasValueCharView.vRefreshFromDataPage();
                            });
                        }

                        foreach (tclsMeasCurveMapView clsMeasCurveMapView in mlstMeasCurveMapView)
                        {
                            clsMeasCurveMapView.vRefreshFromDataPage();
                        }

                        foreach (tclsLogicBlockView clsLogicBlockView in mlstLogicBlockView)
                        {
                            clsLogicBlockView.vRefreshFromDataPage();
                        }

                        Invoke((MethodInvoker)delegate
                        {
                            if (null != mclsBlobSettings)
                            {
                            mclsBlobSettings.vRefreshFromDataPage();
                            }
                        });

                        break;
                    }
                case tenMDIParentNotify.enMDIStartInitialise:
                    {
                        foreach (tclsMeasValueCharView clsMeasValueCharView in mlstMeasValueCharView)
                        {
                            clsMeasValueCharView.vSetInitialise(true);
                        }
                        foreach (tclsMeasValueLoggingView clsMeasValueLoggingView in mlstMeasValueLoggingView)
                        {
                            clsMeasValueLoggingView.vSetInitialise(true);
                        }
                        foreach (tclsMeasValueGaugeView clsMeasValueGaugeView in mlstMeasValueGaugeView)
                        {
                            clsMeasValueGaugeView.vSetInitialise(true);
                        }
                        foreach (tclsMeasCurveMapView clsMeasCurveMapView in mlstMeasCurveMapView)
                        {
                            clsMeasCurveMapView.vSetInitialise(false);
                        }
                        foreach (tclsLogicBlockView clsLogicBlockView in mlstLogicBlockView)
                        {
                            clsLogicBlockView.vSetInitialise(false);
                        }
                        break;
                    }
                case tenMDIParentNotify.enMDIEndInitialise:
                    {
                        foreach (tclsMeasValueCharView clsMeasValueCharView in mlstMeasValueCharView)
                        {
                            clsMeasValueCharView.vSetInitialise(false);
                        }
                        foreach (tclsMeasValueLoggingView clsMeasValueLoggingView in mlstMeasValueLoggingView)
                        {
                            clsMeasValueLoggingView.vSetInitialise(false);
                        }
                        foreach (tclsMeasValueGaugeView clsMeasValueGaugeView in mlstMeasValueGaugeView)
                        {
                            clsMeasValueGaugeView.vSetInitialise(false);
                        }
                        foreach (tclsMeasCurveMapView clsMeasCurveMapView in mlstMeasCurveMapView)
                        {
                            clsMeasCurveMapView.vSetInitialise(false);
                        }
                        foreach (tclsLogicBlockView clsLogicBlockView in mlstLogicBlockView)
                        {
                            clsLogicBlockView.vSetInitialise(false);
                        }
                        break;
                    }
                case tenMDIParentNotify.enMDIRequestViewFocus:
                    {
                        int iFormIDX = iData & 0xff;
                        int iViewIDX = (iData & 0xff00) >> 8;

                        if ((-1 < iFormIDX) && (mlstChildViews.Count > iFormIDX))
                        {
                            mlstChildViews[iFormIDX].BringToFront();

                            foreach (tclsMeasValueCharView clsMeasValueCharView in mlstMeasValueCharView)
                            {
                                clsMeasValueCharView.RequestShowViewIndex(iViewIDX);
                            }

                            foreach (tclsMeasCurveMapView clsMeasCurveMapView in mlstMeasCurveMapView)
                            {
                                clsMeasCurveMapView.RequestShowViewIndex(iViewIDX);
                            }

                            foreach (tclsICMeasSegmentView clsMeasSegmentView in mlstMeasSegmentView)
                            {
                                clsMeasSegmentView.RequestShowViewIndex(iViewIDX);
                            }

                            foreach (tclsLogicBlockView clsLogicBlockView in  mlstLogicBlockView)
                            {
                                clsLogicBlockView.RequestShowViewIndex(iViewIDX);
                            }

                            foreach (tclsDSG clsDSG in mlstDSGView)
                            {
                                clsDSG.RequestShowViewIndex(iViewIDX);
                            }
                        }
                        break;
                    }
                case tenMDIParentNotify.enMDIRequestShutdown:
                    {
                        mboRequestShutdown = true;

                        foreach (tclsMeasValueLoggingView clsLoggingView in mlstLoggingViews)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                clsLoggingView.vRequestShutdown();
                            });
                        }

                        foreach (tclsMeasCurveMapView clsMeasCurveMapView in mlstMeasCurveMapView)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                clsMeasCurveMapView.vRequestShutdown();
                            });
                        }

                        foreach (tclsMeasValueCharView clsMeasValueCharView in mlstMeasValueCharView)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                clsMeasValueCharView.vRequestShutdown();
                            });
                        }

                        foreach (tclsMeasValueGaugeView clsMeasValueGaugeView in mlstMeasValueGaugeView)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                clsMeasValueGaugeView.vRequestShutdown();
                            });
                        }

                        foreach (tclsLogicBlockView clsLogicBlockView in mlstLogicBlockView)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                clsLogicBlockView.vRequestShutdown();
                            });
                        }

                        foreach (tclsICMeasSegmentView clsICMeasSegmentView in mlstMeasSegmentView)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                clsICMeasSegmentView.vRequestShutdown();
                            });
                        }

                        mclsNavTreeView.vRequestShutdown();
                        mclsNotify.vRequestShutdown();

                        if (null != mclsBlobSettings)
                        {
                            mclsBlobSettings.vRequestShutdown();
                        }

                        break;
                    }
                case tenMDIParentNotify.enMDIShowError:
                    {
                        mclsNotify.vAppendNotices("Error", szErrorMessage);
                        MessageBox.Show(szErrorMessage, "System Error!");
                        break;
                    }
                case tenMDIParentNotify.enMDIShowMessage:
                    {
                        mclsNotify.vAppendNotices("Message", szErrorMessage);
                        break;
                    }
                case tenMDIParentNotify.enMDIToolStripOnline:
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            MDIStatusStrip.Items[0].Image = ConnectionImageList.Images[0];
                            MDIStatusStrip.Items[0].Text = "ONLINE";
                            ProgressLabel.Text = szProgressMessage;
                            ProgressBar.Value = iData;
                        });
                        break;
                    }
                case tenMDIParentNotify.enMDIToolStripOffline:
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            MDIStatusStrip.Items[0].Image = ConnectionImageList.Images[1];
                            MDIStatusStrip.Items[0].Text = "OFFLINE";
                            ProgressLabel.Text = szProgressMessage;
                            ProgressBar.Value = iData;
                            ProgressBar.ForeColor = Color.DarkGray;
                            MDIStatusStrip.Refresh();
                        });
                        break;
                    }
                case tenMDIParentNotify.enMDILoaded:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveWindowsToINI();
            Program.vNotifyProgramState(tenProgramState.enRequestShutdown, 0);
        }

        private void vShutdown()
        {
            Program.vNotifyProgramState(tenProgramState.enShutdown, 0);
            this.Close();
        }

        private void OnMicroTimerEvent(object sender, MicroLibrary.MicroTimerEventArgs timerEventArgs)
        {
            long ticks = System.DateTime.Now.Ticks;
            mu64Delta = ticks - mu64OldTicks;
            mu64OldTicks = ticks;

            mu64CommsCount += mu64Delta;

            if (false == mboRequestShutdown)
            {
                mu64CommsCount = 0;
                Program.mAPP_clsUDPComms.ChannelUDP_vMain();
            }
            else
            {
                mclsCommsTimer.Stop();
                mclsCommsTimer.Abort();
            }
        }

        private void StripMenuItemFreezeCalToNVMTool_Click(object sender, EventArgs e)
        {
            bool boResult = Program.mAPP_clsUDPComms.boRequestCalPageTransfer(tenChannelMode.enChannelModeWorkingNVMFreeze);
        }

        private void clearNVMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool boResult = Program.mAPP_clsUDPComms.boRequestCalPageTransfer(tenChannelMode.enChannelModeNVMClear);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void openCalImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool boRetVal = Program.boSetCalibrationPath(null, true);

            if (true == boRetVal)
            {
                Program.vNotifyProgramState(tenProgramState.enProgInitialising, 0);
                boRetVal = Program.boLoadCalibration();
                SetMDIText(Program.mszCalibrationFilePath);
            }
        }

        private void programToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tclsIniEdit clsIniEdit = new tclsIniEdit();

            clsIniEdit.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tclsRoutineControl clsRoutineControl = new tclsRoutineControl();

            clsRoutineControl.Show();
        }

        private void tclsMDIParent_Resize(object sender, System.EventArgs e)
        {
            if (null != mclsNotify)
            {
                mclsNotify.Left = 0;
                mclsNotify.Top = this.ClientRectangle.Height - 150;
                mclsNotify.Width = this.ClientRectangle.Width;
                mclsNotify.Show();
            }
        }

        private void ArrangeWindowsFromINI()
        {
            int formIDX = 0;
            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Database\\GUI Layout Databases\\GUI Layout Rabbit 1_0 ECU.INI");
            
            foreach (Form clsForm in this.MdiChildren)
            {
                String szFormName = clsForm.Name;
                string szLeft; string szWidth; string szTop; string szHeight;
                int iLeft; int iTop; int iWidth; int iHeight;

                try
                {
                    szLeft = mclsIniParser.GetSetting("Window Layout", szFormName + formIDX.ToString() + ".Left");
                }
                catch
                {
                    szLeft = "-1";
                }

                try
                {
                    szWidth = mclsIniParser.GetSetting("Window Layout", szFormName + formIDX.ToString() + ".Width");
                }
                catch
                {
                    szWidth = "-1";
                }

                try
                {
                    szTop = mclsIniParser.GetSetting("Window Layout", szFormName + formIDX.ToString() + ".Top");
                }
                catch
                {
                    szTop = "-1";
                }

                try
                {
                    szHeight = mclsIniParser.GetSetting("Window Layout", szFormName + formIDX.ToString() + ".Height");
                }
                catch
                {
                    szHeight = "-1";
                }

                iLeft = Convert.ToInt16(szLeft);
                iTop = Convert.ToInt16(szTop);
                iWidth = Convert.ToInt16(szWidth);
                iHeight = Convert.ToInt16(szHeight);

                if ((0 <= iTop) && (0 < iHeight) && (0 <= iLeft) && (0 < iWidth))
                {
                    clsForm.Left = iLeft;
                    clsForm.Height = iHeight;
                    clsForm.Top = iTop;
                    clsForm.Width = iWidth;
                }

                formIDX++;
            }
        }

        private void SaveWindowsToINI()
        {
            int formIDX = 0;
            bool saveErr = false;
            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Database\\GUI Layout Databases\\GUI Layout Rabbit 1_0 ECU.INI");

            foreach (Form clsForm in this.MdiChildren)
            {
                String szFormName = clsForm.Name;
            
                try
                {
                    mclsIniParser.AddSetting("Window Layout", szFormName + formIDX.ToString() + ".Left", clsForm.Left.ToString());
                    mclsIniParser.AddSetting("Window Layout", szFormName + formIDX.ToString() + ".Width", clsForm.Width.ToString());
                    mclsIniParser.AddSetting("Window Layout", szFormName + formIDX.ToString() + ".Top", clsForm.Top.ToString());
                    mclsIniParser.AddSetting("Window Layout", szFormName + formIDX.ToString() + ".Height", clsForm.Height.ToString());
                }
                catch
                {
                    if (false == saveErr)
                    {
                        MessageBox.Show("Error saving view context");
                    }

                    saveErr = true;
                }

                formIDX++;
            }

            mclsIniParser.SaveSettings();
        }

        private void replayScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool boResult = Program.mAPP_clsUDPComms.boRequestScriptReplay(tenChannelMode.enChannelModeReplayScript);
            
            if (true == boResult)
            {

            }
        }

        private void MDIStatusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void saveCalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.boSaveCalibration();
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != mclsBlobSettings)
            {
                mclsBlobSettings.Show();
            }
        }

        private void StatusLabel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripView_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ArrangeWindowsFromMenu(tenMDIViewArrange.enMDIViewArrangeDefault);
        }

        private void ArrangeWindowsFromMenu(tenMDIViewArrange enViewArrange)
        {
            int iMapCurveCount = 0;
            int iMeasureValueCount = 0;


            foreach (Form clsForm in this.MdiChildren)
            {
                switch (enViewArrange)
                {
                    case tenMDIViewArrange.enMDIViewArrangeDefault:
                        {
                            if (clsForm is tclsNavTreeView)
                            {
                                clsForm.Left = 0;
                                clsForm.Width = 15 * this.Width / 100;
                                clsForm.Height = 75 * this.Height / 100;
                                clsForm.Top = 0;
                                clsForm.WindowState = FormWindowState.Normal;
                            }

                            if (clsForm is tclsNotify)
                            {
                                clsForm.Left = 0;
                                clsForm.Width = 55 * this.Width / 100;
                                clsForm.Height = this.Height / 3;
                                clsForm.Top = (75 * this.Height) / 100;
                                clsForm.WindowState = FormWindowState.Normal;
                            }

                            if (clsForm is tclsMeasValueLoggingView)
                            {
                                clsForm.Left = 55 * this.Width / 100;
                                clsForm.Width = 60 * this.Width / 100;
                                clsForm.Height = this.Height / 3;
                                clsForm.Top = 75 * this.Height / 100;
                                clsForm.WindowState = FormWindowState.Normal;
                            }

                            if (clsForm is tclsMeasValueGaugeView)
                            {
                                clsForm.Left = 75 * this.Width / 100;
                                clsForm.Width = 4 * this.Width / 10;
                                clsForm.Height = 75 * this.Height / 100;
                                clsForm.Top = 0;
                                clsForm.WindowState = FormWindowState.Normal;
                            }

                            if (clsForm is tclsMeasCurveMapView)
                            {
                                clsForm.Left = 15 * this.Width / 100 + 20 * iMapCurveCount;
                                clsForm.Width = 6 * this.Width / 10;
                                clsForm.Height = 75 * this.Height / 100;
                                clsForm.Top = 20 * iMapCurveCount;
                                clsForm.WindowState = FormWindowState.Normal;
                                iMapCurveCount++;
                            }

                            if (clsForm is tclsMeasValueCharView)
                            {
                                clsForm.Left = 15 * this.Width / 100 + 20 * iMeasureValueCount;
                                clsForm.Top = 0;
                                clsForm.Width = 6 * this.Width / 10;
                                clsForm.Height = 75 * this.Height / 100;
                                clsForm.WindowState = FormWindowState.Normal;
                                iMeasureValueCount++;
                            }

                            if (clsForm is tclsLogicBlockView)
                            {
                                clsForm.Left = 15 * this.Width / 100;
                                clsForm.Width = 6 * this.Width / 10;
                                clsForm.Height = 75 * this.Height / 100;
                                clsForm.Top = 0;
                                clsForm.WindowState = FormWindowState.Normal;
                                iMapCurveCount++;
                            }

                            if (clsForm is tclsBlobSettings)
                            {
                                clsForm.Left = 15 * this.Width / 100;
                                clsForm.Top = 0;
                                clsForm.Width = 6 * this.Width / 10;
                                clsForm.Height = 75 * this.Height / 100;
                                clsForm.WindowState = FormWindowState.Normal;
                            }

                            if (clsForm is tclsICMeasSegmentView)
                            {
                                clsForm.Left = 75 * this.Width / 100;
                                clsForm.Width = 4 * this.Width / 10;
                                clsForm.Height = 25 * this.Height / 100;
                                clsForm.Top = 0;
                                clsForm.WindowState = FormWindowState.Normal;
                            }

                            if (clsForm is tclsDSG)
                            {
                                clsForm.Left = 75 * this.Width / 100;
                                clsForm.Width = 4 * this.Width / 10;
                                clsForm.Height = 25 * this.Height / 100;
                                clsForm.Top = 0;
                                clsForm.WindowState = FormWindowState.Normal;
                            }

                            break;
                        }
                    default:
                        {
                            break;

                        }
                }
            }
        }

        private void clearMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mclsNotify.vClearNotices();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            tclsPDFView clsHelpPDF = new UDP.tclsPDFView(AppDomain.CurrentDomain.BaseDirectory + "Help Files\\MAP-MATE User Guide.pdf");

            try
            {
                if (true == clsHelpPDF.AcroLoadOK)
                {
                    clsHelpPDF.MdiParent = this;
                    clsHelpPDF.Show();
                    clsHelpPDF.Refresh();
                }
            }
            catch
            {
                MessageBox.Show("Starting PDF alternate viewer");
            }
        }
    }
}