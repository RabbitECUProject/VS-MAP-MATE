/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Program                                                */
/* DESCRIPTION:                                                               */
/* FILE NAME:          Program.cs                                             */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UDP
{
    public enum tenProgramState
    {
        enProgInitialising,
        enProgWindowsLoaded,
        enRequestViewFocus,
        enRequestShutdown,
        enShutdown,
        enProgNormal
    }

    public enum tenWindowChildType
    {
        enMeasValueView,
        enMeasTableView,
        enMeasMapView,
        enLoggingView,
        enMeasGaugeView,
        enMeasCharConfigView,
        enMeasSegmentView,
        enLogicBlockView,
        enDSGView
    }

    public enum tenCommsType
    {
        enCANAL,
        enCANRP1210,
        enCANLIBKVASER,
        enEthernet,
        enWIFI,
        enUSBCDC,
        enSerial,
        enUnknown
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static tclsXMLConfigReader mAPP_clsXMLConfig = new tclsXMLConfigReader();
        public static tclsXMLCalibrationReaderWriter mAPP_clsXMLCalibration = new tclsXMLCalibrationReaderWriter();
        public static tclsMDIParent mFormUDP;
        public static tclsUDSComms mAPP_clsUDPComms;
        public static tclsIniParser mAPP_mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHOST Calibration.INI");
        public static string mszCalibrationFilePath;
        public static bool mboCommsSuspend;
        public static bool mboCommsSuspendError;
        public static BackgroundWorker mSplashWorker;
        public static BackgroundWorker mFileOpenWorker;
        public static tclsSplash mSplashForm;
        public static bool mSplashLoading;
        public static bool mFileOpenOK;
        public static bool mboCommsOnline;
        public static System.Drawing.Color mProgramTextForecolor;
        public static System.Drawing.Color mProgramTextBackcolor;
        public static System.Drawing.Color mProgramTextToggleBackcolor;
        public static string mszAdapterDeviceName;

        static Program()
        {
            tclsErrlog.OpenLog(AppDomain.CurrentDomain.BaseDirectory);

            tclsDataPage.vSetWorkingBaseAddress(tclsASAM.u32GetCharMinAddress());
            tclsErrlog.LogAppend("ASAM parsed...");

            mProgramTextForecolor = System.Drawing.Color.Aquamarine;
            mProgramTextBackcolor = System.Drawing.Color.Black;
            mProgramTextToggleBackcolor = System.Drawing.Color.DarkSlateGray;
            mSplashLoading = true;
            mSplashWorker = new BackgroundWorker();
            mSplashWorker.DoWork += new DoWorkEventHandler(Splash);
            tclsErrlog.LogAppend("Splash started...");
            mFileOpenWorker = new BackgroundWorker();
            mFileOpenWorker.DoWork += new DoWorkEventHandler(FileOpen);
            mSplashWorker.RunWorkerAsync();

            try
            {
                mszAdapterDeviceName = mAPP_mclsIniParser.GetSetting("NetworkConnection", "SelectedDevice");
                tclsErrlog.LogAppend("Adapter Device Name: " + mszAdapterDeviceName);
            }
            catch
            {
                mszAdapterDeviceName = "Unknown";
            }

            mszCalibrationFilePath = null;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mFormUDP = new tclsMDIParent();
            tclsErrlog.LogAppend("MDI created...");

            mAPP_clsUDPComms = new tclsUDSComms(mszAdapterDeviceName, mFormUDP.Handle);
            mboCommsOnline = mAPP_clsUDPComms.Connected;
            tclsErrlog.LogAppend("CommsOnline: " + mboCommsOnline.ToString());

            if (false == mboCommsOnline)
            {
                mAPP_clsUDPComms.vDispose();
            }

            mboCommsSuspend = true;

            UInt16 u16ASAMCRC = tclsASAM.u16GetCRC16();

            //if (0 != u16ASAMCRC)
            if (39430 != u16ASAMCRC)
            {
                Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "ASAM file does not contain a correct CRC");
            }
            else
            {
                tclsErrlog.LogAppend("ASAM CRC correct...");
            }
        }


        [STAThread]
        static void Main()
        {

            mSplashForm = new tclsSplash();
            mSplashForm.StartPosition = FormStartPosition.CenterScreen;
            mSplashForm.Refresh();


            Application.Run(mFormUDP);
            Application.Exit();
        }

        public static Font GetTextFont()
        {
            String szTreeFont;
            String szTreeFontSize;
            String szTreeFontBold;
            FontStyle fontStyle = FontStyle.Regular;
            Font font;

            try
            {
                szTreeFont = mAPP_mclsIniParser.GetSetting("Program", "TextFont");
            }
            catch
            {
                szTreeFont = "System";
            }

            try
            {
                szTreeFontSize = mAPP_mclsIniParser.GetSetting("Program", "TextFontSize");
            }
            catch
            {
                szTreeFontSize = "10";
            }

            try
            {
                szTreeFontBold = mAPP_mclsIniParser.GetSetting("Program", "TextFontBold");

                if (0 == Convert.ToInt16(szTreeFontBold))
                {
                    fontStyle = FontStyle.Regular;
                }
                else
                {
                    fontStyle = FontStyle.Bold;
                }
            }
            catch
            {
                szTreeFontBold = "0";
            }

            font = new Font(szTreeFont, (float)Convert.ToDouble(szTreeFontSize), fontStyle);

            return font;
        }

        public static void Splash(object sender, DoWorkEventArgs e)
        {
            int iSplashCount = 0;

            while ((100 > iSplashCount) && (mSplashLoading))
            {
                if (null != mSplashForm)
                {
                    if (iSplashCount == 0)
                    {
                        mSplashForm.Show();
                        mSplashForm.Refresh();
                    }
                    else
                    {
                        mSplashForm.ClockSplash();
                    }
                    iSplashCount++;
                }

                System.Threading.Thread.Sleep(200);
            }

            mSplashForm.Dispose();
        }

        public static void vMeasuresUpdate(List<UInt32> lstData, int iActiveMeasureIDX)
        {
            mFormUDP.vDistributeMeasurements(lstData, iActiveMeasureIDX);
        }

        public static bool boLoadCalibration()
        {
            if (false == mFileOpenWorker.IsBusy)
            {
                vNotifyProgramEvent(tenProgramEvent.enSlowTaskProgress, 0, "Loading calibration file...");
                mFileOpenWorker.RunWorkerAsync();
            }

            return mFileOpenOK;
        }

        public static void FileOpen(object sender, DoWorkEventArgs e)
        {
            mFileOpenOK = false;
            bool boFileOpen = false;

            if (null != mszCalibrationFilePath)
            {
                vNotifyProgramEvent(tenProgramEvent.enSlowTaskProgress, 10, "Reading calibration file...");
                System.Threading.Thread.Sleep(200);
                boFileOpen = mAPP_clsXMLCalibration.boReadCalibrationFile(mszCalibrationFilePath, true, true);

                if (true == boFileOpen)
                {
                System.Threading.Thread.Sleep(200);
                vNotifyProgramEvent(tenProgramEvent.enSlowTaskProgress, 20, "Converting file data...");
                tclsDataPage.vSetChangeLock(true);
                vBufferCalibrationToDataPage();
                tclsDataPage.vSetChangeLock(false);
                vNotifyProgramEvent(tenProgramEvent.enSlowTaskProgress, 100, "Load calibration file complete");
                vNotifyProgramEvent(tenProgramEvent.enLoadCalibrationComplete, 100, "Load calibration file complete");
                Program.vNotifyProgramState(tenProgramState.enProgNormal, 100);
                mFileOpenOK = true;
            }
                else
                {
                    vNotifyProgramEvent(tenProgramEvent.enProgramError, 100, "Error opening calibration file");
                    vNotifyProgramEvent(tenProgramEvent.enSlowTaskProgress, 100, "Calibration file error");
                }
            }

        }

        public static void vUpdateCalibration(String szCharName, UInt32[] au32Data)
        {
            Program.mAPP_clsXMLCalibration.vSetCharData(szCharName, au32Data);
        }


        public static void vUpdateCalibrationFromDataPage(int iCharIDX)
        {
            UInt32 u32Address = tclsASAM.milstCharacteristicList[iCharIDX].u32Address;
            int iPointsCount;

            if ((-1 == tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef) && (-1 != tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef))
            {
                iPointsCount = tclsASAM.milstAxisDescrList[tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef].iAxisPointCount;
            }
            else if ((-1 == tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef) && (-1 != tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef))
            {
                iPointsCount = tclsASAM.milstAxisDescrList[tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef].iAxisPointCount;
            }
            else if ((-1 != tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef) && (-1 != tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef))
            {
                iPointsCount = tclsASAM.milstAxisDescrList[tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef].iAxisPointCount *
                               tclsASAM.milstAxisDescrList[tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef].iAxisPointCount;
            }
            else
            {
                iPointsCount = 1;
            }

            UInt32[] au32Data = new UInt32[iPointsCount];

            switch (tclsASAM.milstCharacteristicList[iCharIDX].enRecLayout)
            {
                case tenRecLayout.enRL_VALU16:
                    {
                        UInt16[] au16Data = new ushort[iPointsCount];
                        tclsDataPage.au16GetWorkingData(u32Address, ref au16Data);
                        au32Data = Array.ConvertAll(au16Data, Convert.ToUInt32);
                        break;
                    }
                case tenRecLayout.enRL_VALU32:
                    {

                        tclsDataPage.au32GetWorkingData(u32Address, ref au32Data);
                        break;
                    }
                case tenRecLayout.enRL_VALS16:
                    {
                        Int16[] as16Data = new short[iPointsCount];
                        tclsDataPage.as16GetWorkingData(u32Address, ref as16Data);

                        for (int iIDX = 0; iIDX < iPointsCount; iIDX++)
                        {
                            if (0 > as16Data[iIDX])
                            {
                                au32Data[iIDX] = (UInt32)as16Data[iIDX];
                                au32Data[iIDX] += UInt32.MaxValue / 2;
                                au32Data[iIDX] += 2;
                            }
                            else
                            {
                                au32Data[iIDX] = (UInt32)as16Data[iIDX];
                            }
                        }
                        break;
                    }
                case tenRecLayout.enRL_VALS32:
                    {
                        Int32[] as32Data = new Int32[iPointsCount];
                        tclsDataPage.as32GetWorkingData(u32Address, ref as32Data);

                        for (int iIDX = 0; iIDX < iPointsCount; iIDX++)
                        {
                            if (0 > as32Data[iIDX])
                            {
                                au32Data[iIDX] = (UInt32)as32Data[iIDX];
                                au32Data[iIDX] += UInt32.MaxValue / 2;
                                au32Data[iIDX] += 2;
                            }
                            else
                            {
                                au32Data[iIDX] = (UInt32)as32Data[iIDX];
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            Program.mAPP_clsXMLCalibration.vSetCharData(tclsASAM.milstCharacteristicList[iCharIDX].szCharacteristicName, au32Data);
        }

        private static void vBufferCalibrationToDataPage()
        {
            int iProgressCount = 0;
            Single fProgress;
            String szUpdateString = "Converting characteristics...";

            foreach (tstCharacteristic stCharacteristic in tclsASAM.milstCharacteristicList)
            {
                UInt32[] au32Data;

                iProgressCount++;
                fProgress = 20f + 30f * ((Single)iProgressCount / ((Single)tclsASAM.milstCharacteristicList.Count + (Single)tclsASAM.milstAxisPtsList.Count + (Single)tclsASAM.milstBlobList.Count));

                vNotifyProgramEvent(tenProgramEvent.enSlowTaskProgress, (int)fProgress, szUpdateString);
                
                if (32 < szUpdateString.Length)
                {
                    szUpdateString = "Converting characteristics...";
                }
                else
                {
                    szUpdateString += ".";
                    System.Threading.Thread.Sleep(20);
                }

                au32Data = Program.mAPP_clsXMLCalibration.u32GetCharData(stCharacteristic.szCharacteristicName);

                if ((null != au32Data) && (0 != au32Data.Length))
                {
                    switch (stCharacteristic.enParamType)
                    {
                        case tenParamType.enPT_VALUE:
                            {
                                switch (stCharacteristic.enRecLayout)
                                {
                                    case tenRecLayout.enRL_VALU8:
                                        {
                                            tclsDataPage.u8SetWorkingData(stCharacteristic.u32Address, (byte)au32Data[0]);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALS8:
                                        {
                                            tclsDataPage.s8SetWorkingData(stCharacteristic.u32Address, (sbyte)au32Data[0]);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALU16:
                                        {
                                            tclsDataPage.u16SetWorkingData(stCharacteristic.u32Address, (UInt16)au32Data[0]);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALS16:
                                        {
                                            tclsDataPage.s16SetWorkingData(stCharacteristic.u32Address, (Int16)au32Data[0]);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALU32:
                                        {
                                            tclsDataPage.u32SetWorkingData(stCharacteristic.u32Address, (UInt32)au32Data[0]);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALS32:
                                        {
                                            tclsDataPage.s32SetWorkingData(stCharacteristic.u32Address, (Int32)au32Data[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                                break;
                            }
                        case tenParamType.enPT_CURVE:
                            {
                                int iCols = tclsASAM.milstAxisPtsList[stCharacteristic.iXAxisRef].iAxisPointCount;

                                switch (stCharacteristic.enRecLayout)
                                {
                                    case tenRecLayout.enRL_VALS16:
                                        {
                                            byte[] au8Data = new byte[2 * iCols];

                                            for (int iColIDX = 0; iColIDX < iCols; iColIDX++)
                                            {
                                                au8Data[2 * iColIDX + 1] = (byte)(au32Data[iColIDX] / 0x100);
                                                au8Data[2 * iColIDX] = (byte)(au32Data[iColIDX]);
                                            }

                                            tclsDataPage.vSetWorkingData(stCharacteristic.u32Address, au8Data, true, false);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALU16:
                                        {
                                            byte[] au8Data = new byte[2 * iCols];

                                            for (int iColIDX = 0; iColIDX < iCols; iColIDX++)
                                            {
                                                au8Data[2 * iColIDX + 1] = (byte)(au32Data[iColIDX] / 0x100);
                                                au8Data[2 * iColIDX] = (byte)(au32Data[iColIDX]);
                                            }

                                            tclsDataPage.vSetWorkingData(stCharacteristic.u32Address, au8Data, true, false);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALS32:
                                        {
                                            byte[] au8Data = new byte[4 * iCols];

                                            for (int iColIDX = 0; iColIDX < iCols; iColIDX++)
                                            {
                                                au8Data[4 * iColIDX + 3] = (byte)(au32Data[iColIDX] / 0x1000000);
                                                au8Data[4 * iColIDX + 2] = (byte)(au32Data[iColIDX] / 0x10000);
                                                au8Data[4 * iColIDX + 1] = (byte)(au32Data[iColIDX] / 0x100);
                                                au8Data[4 * iColIDX] = (byte)(au32Data[iColIDX]);
                                            }

                                            tclsDataPage.vSetWorkingData(stCharacteristic.u32Address, au8Data, true, false);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALU32:
                                        {
                                            byte[] au8Data = new byte[4 * iCols];

                                            for (int iColIDX = 0; iColIDX < iCols; iColIDX++)
                                            {
                                                au8Data[4 * iColIDX + 3] = (byte)(au32Data[iColIDX] / 0x1000000);
                                                au8Data[4 * iColIDX + 2] = (byte)(au32Data[iColIDX] / 0x10000);
                                                au8Data[4 * iColIDX + 1] = (byte)(au32Data[iColIDX] / 0x100);
                                                au8Data[4 * iColIDX] = (byte)(au32Data[iColIDX]);
                                            }

                                            tclsDataPage.vSetWorkingData(stCharacteristic.u32Address, au8Data, true, false);
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                                break;
                            }
                        case tenParamType.enPT_MAP:
                            {
                                int iCols = tclsASAM.milstAxisPtsList[stCharacteristic.iXAxisRef].iAxisPointCount;
                                int iRows = tclsASAM.milstAxisPtsList[stCharacteristic.iYAxisRef].iAxisPointCount;

                                switch (stCharacteristic.enRecLayout)
                                {
                                    case tenRecLayout.enRL_VALS16:
                                        {
                                            byte[] au8Data = new byte[2 * iCols * iRows];

                                            for (int iRowIDX = 0; iRowIDX < iRows; iRowIDX++)
                                            {
                                                for (int iColIDX = 0; iColIDX < iCols; iColIDX++)
                                                {
                                                    au8Data[iCols * 2 * iRowIDX + 2 * iColIDX + 1] = (byte)(au32Data[iCols * iRowIDX + iColIDX] / 0x100);
                                                    au8Data[iCols * 2 * iRowIDX + 2 * iColIDX] = (byte)(au32Data[iCols * iRowIDX + iColIDX]);
                                                }
                                            }

                                            tclsDataPage.vSetWorkingData(stCharacteristic.u32Address, au8Data, true, false);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALU16:
                                        {
                                            byte[] au8Data = new byte[2 * iCols * iRows];

                                            for (int iRowIDX = 0; iRowIDX < iRows; iRowIDX++)
                                            {
                                                for (int iColIDX = 0; iColIDX < iCols; iColIDX++)
                                                {
                                                    au8Data[iCols * 2 * iRowIDX + 2 * iColIDX + 1] = (byte)(au32Data[iCols * iRowIDX + iColIDX] / 0x100);
                                                    au8Data[iCols * 2 * iRowIDX + 2 * iColIDX] = (byte)(au32Data[iCols * iRowIDX + iColIDX]);
                                                }
                                            }

                                            tclsDataPage.vSetWorkingData(stCharacteristic.u32Address, au8Data, true, false);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALS32:
                                        {
                                            byte[] au8Data = new byte[4 * iCols * iRows];

                                            for (int iRowIDX = 0; iRowIDX < iRows; iRowIDX++)
                                            {
                                                for (int iColIDX = 0; iColIDX < iCols; iColIDX++)
                                                {
                                                    au8Data[iCols * 4 * iRowIDX + 4 * iColIDX + 1] = (byte)(au32Data[iCols * iRowIDX + iColIDX] / 0x100);
                                                    au8Data[iCols * 4 * iRowIDX + 4 * iColIDX] = (byte)(au32Data[iCols * iRowIDX + iColIDX]);
                                                }
                                            }

                                            tclsDataPage.vSetWorkingData(stCharacteristic.u32Address, au8Data, true, false);
                                            break;
                                        }
                                    case tenRecLayout.enRL_VALU32:
                                        {
                                            byte[] au8Data = new byte[4 * iCols * iRows];

                                            for (int iRowIDX = 0; iRowIDX < iRows; iRowIDX++)
                                            {
                                                for (int iColIDX = 0; iColIDX < iCols; iColIDX++)
                                                {
                                                    au8Data[iCols * 4 * iRowIDX + 4 * iColIDX + 3] = (byte)(au32Data[iCols * iRowIDX + iColIDX] / 0x1000000);
                                                    au8Data[iCols * 4 * iRowIDX + 4 * iColIDX + 2] = (byte)(au32Data[iCols * iRowIDX + iColIDX] / 0x10000);
                                                    au8Data[iCols * 4 * iRowIDX + 4 * iColIDX + 1] = (byte)(au32Data[iCols * iRowIDX + iColIDX] / 0x100);
                                                    au8Data[iCols * 4 * iRowIDX + 4 * iColIDX] = (byte)(au32Data[iCols * iRowIDX + iColIDX]);
                                                }
                                            }

                                            tclsDataPage.vSetWorkingData(stCharacteristic.u32Address, au8Data, true, false);
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                }
                else
                {
                    if (0 != stCharacteristic.szCharacteristicName.IndexOf('_'))
                    {
                        String szErrMsg = "No calibration data was found for " + stCharacteristic.szCharacteristicName + "!";
                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, szErrMsg);
                    }
                }
            }

            foreach (tstAxisPts stAxisPts in tclsASAM.milstAxisPtsList)
            {
                UInt32[] au32Data;

                iProgressCount++;
                fProgress = 20f + 30f * ((Single)iProgressCount / ((Single)tclsASAM.milstCharacteristicList.Count + (Single)tclsASAM.milstAxisPtsList.Count + (Single)tclsASAM.milstBlobList.Count));

                vNotifyProgramEvent(tenProgramEvent.enSlowTaskProgress, (int)fProgress, szUpdateString);

                if (21 < szUpdateString.Length)
                {
                    szUpdateString = "Converting axes...";
                }
                else
                {
                    szUpdateString += ".";
                    System.Threading.Thread.Sleep(20);
                }

                au32Data = Program.mAPP_clsXMLCalibration.u32GetAxisData(stAxisPts.szAxisPtsName);

                if (null != au32Data)
                {
                    switch (stAxisPts.enRecLayout)
                    {
                        case tenRecLayout.enRL_VALU16:
                            {
                                byte[] au8Data = new byte[2 * stAxisPts.iAxisPointCount];

                                for (int iAxisPtIDX = 0; iAxisPtIDX < stAxisPts.iAxisPointCount; iAxisPtIDX++)
                                {
                                    au8Data[iAxisPtIDX * 2 + 1] = (byte)(au32Data[iAxisPtIDX] / 0x100);
                                    au8Data[iAxisPtIDX * 2] = (byte)(au32Data[iAxisPtIDX]);
                                }

                                tclsDataPage.vSetWorkingData(stAxisPts.u32Address, au8Data, true, false);
                                break;
                            }
                        case tenRecLayout.enRL_VALS32:
                            {
                                byte[] au8Data = new byte[4 * stAxisPts.iAxisPointCount];

                                for (int iAxisPtIDX = 0; iAxisPtIDX < stAxisPts.iAxisPointCount; iAxisPtIDX++)
                                {
                                    au8Data[iAxisPtIDX * 4 + 3] = (byte)(au32Data[iAxisPtIDX] / 0x1000000);
                                    au8Data[iAxisPtIDX * 4 + 2] = (byte)(au32Data[iAxisPtIDX] / 0x10000);
                                    au8Data[iAxisPtIDX * 4 + 1] = (byte)(au32Data[iAxisPtIDX] / 0x100);
                                    au8Data[iAxisPtIDX * 4] = (byte)(au32Data[iAxisPtIDX]);
                                }

                                tclsDataPage.vSetWorkingData(stAxisPts.u32Address, au8Data, true, false);
                                break;
                            }
                        case tenRecLayout.enRL_VALU32:
                            {
                                byte[] au8Data = new byte[4 * stAxisPts.iAxisPointCount];

                                for (int iAxisPtIDX = 0; iAxisPtIDX < stAxisPts.iAxisPointCount; iAxisPtIDX++)
                                {
                                    au8Data[iAxisPtIDX * 4 + 3] = (byte)(au32Data[iAxisPtIDX] / 0x1000000);
                                    au8Data[iAxisPtIDX * 4 + 2] = (byte)(au32Data[iAxisPtIDX] / 0x10000);
                                    au8Data[iAxisPtIDX * 4 + 1] = (byte)(au32Data[iAxisPtIDX] / 0x100);
                                    au8Data[iAxisPtIDX * 4] = (byte)(au32Data[iAxisPtIDX]);
                                }

                                tclsDataPage.vSetWorkingData(stAxisPts.u32Address, au8Data, true, false);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    String szErrMsg = "No axis data was found for " + stAxisPts.szAxisPtsName + "!";
                    Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, szErrMsg);

                }
            }

            foreach (tstBlob stBlob in tclsASAM.milstBlobList)
            {
                UInt32[] au32Data;

                au32Data = Program.mAPP_clsXMLCalibration.u32GetBlobData(stBlob.szBlobName);

                iProgressCount++;
                fProgress = 20f + 30f * ((Single)iProgressCount / ((Single)tclsASAM.milstCharacteristicList.Count + (Single)tclsASAM.milstAxisPtsList.Count + (Single)tclsASAM.milstBlobList.Count));

                vNotifyProgramEvent(tenProgramEvent.enSlowTaskProgress, (int)fProgress, szUpdateString);

                if (22 < szUpdateString.Length)
                {
                    szUpdateString = "Converting blobs...";
                }
                else
                {
                    szUpdateString += ".";
                    System.Threading.Thread.Sleep(20);
                }

                if (null != au32Data)
                {
                    switch (stBlob.enRecLayout)
                    {
                        case tenRecLayout.enRL_VALU16:
                            {
                                byte[] au8Data = new byte[2 * stBlob.iPointCount];

                                for (int iAxisPtIDX = 0; iAxisPtIDX < stBlob.iPointCount; iAxisPtIDX++)
                                {
                                    au8Data[iAxisPtIDX * 2 + 1] = (byte)(au32Data[iAxisPtIDX] / 0x100);
                                    au8Data[iAxisPtIDX * 2] = (byte)(au32Data[iAxisPtIDX]);
                                }

                                tclsDataPage.vSetWorkingData(stBlob.u32Address, au8Data, true, false);
                                break;
                            }
                        case tenRecLayout.enRL_VALS32:
                            {
                                byte[] au8Data = new byte[4 * stBlob.iPointCount];

                                for (int iAxisPtIDX = 0; iAxisPtIDX < stBlob.iPointCount; iAxisPtIDX++)
                                {
                                    au8Data[iAxisPtIDX * 4 + 3] = (byte)(au32Data[iAxisPtIDX] / 0x1000000);
                                    au8Data[iAxisPtIDX * 4 + 2] = (byte)(au32Data[iAxisPtIDX] / 0x10000);
                                    au8Data[iAxisPtIDX * 4 + 1] = (byte)(au32Data[iAxisPtIDX] / 0x100);
                                    au8Data[iAxisPtIDX * 4] = (byte)(au32Data[iAxisPtIDX]);
                                }

                                tclsDataPage.vSetWorkingData(stBlob.u32Address, au8Data, true, false);
                                break;
                            }
                        case tenRecLayout.enRL_VALU32:
                            {
                                byte[] au8Data = new byte[4 * stBlob.iPointCount];

                                for (int iAxisPtIDX = 0; iAxisPtIDX < stBlob.iPointCount; iAxisPtIDX++)
                                {
                                    au8Data[iAxisPtIDX * 4 + 3] = (byte)(au32Data[iAxisPtIDX] / 0x1000000);
                                    au8Data[iAxisPtIDX * 4 + 2] = (byte)(au32Data[iAxisPtIDX] / 0x10000);
                                    au8Data[iAxisPtIDX * 4 + 1] = (byte)(au32Data[iAxisPtIDX] / 0x100);
                                    au8Data[iAxisPtIDX * 4] = (byte)(au32Data[iAxisPtIDX]);
                                }

                                tclsDataPage.vSetWorkingData(stBlob.u32Address, au8Data, true, false);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    String szErrMsg = "No generic data was found for " + stBlob.szBlobName + "!";
                    Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, szErrMsg);
                }
            }
        }

        public static bool boSetCalibrationPath(string szCalibrationFilePath, bool boPromptUser)
        {
            bool boRetVal = false;

            if (true == boPromptUser)
            {
                OpenFileDialog clsFileDialog = new OpenFileDialog();
                clsFileDialog.Title = "Open Calibration File";
                clsFileDialog.DefaultExt = "XML:xml";
                clsFileDialog.Filter = "XML files (*.xml)|*.xml";
                clsFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Database\\Calibration Databases";
                DialogResult Result = clsFileDialog.ShowDialog();

                if (DialogResult.OK == Result)
                {
                    mszCalibrationFilePath = clsFileDialog.FileName;
                    boRetVal = true;
                }
            }
            else
            {
                mszCalibrationFilePath = szCalibrationFilePath;
                boRetVal = true;
            }

            return boRetVal;
        }

        public static bool boSaveCalibration()
        {
            bool boRetVal = false;

            SaveFileDialog clsFileDialog = new SaveFileDialog();
            clsFileDialog.Title = "Save Calibration File";
            clsFileDialog.DefaultExt = "XML:xml";
            clsFileDialog.AddExtension = true;
            clsFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Database\\Calibration Databases";
            clsFileDialog.Filter = "XML files (*.xml)|*.xml";
            DialogResult Result = clsFileDialog.ShowDialog();

            if (DialogResult.OK == Result)
            {
                mszCalibrationFilePath = clsFileDialog.FileName;
                Program.mAPP_clsXMLCalibration.boWriteCalibrationFile(mszCalibrationFilePath);
            }

            return boRetVal;
        }

        public static void vNotifyProgramEvent(tenProgramEvent enRPCResponse, int iData, string szErrOrMessage)
        {
            if (null != mFormUDP)
            {
                switch (enRPCResponse)
                {
                    case tenProgramEvent.enRPCUploadComplete:
                        {
                            tclsDataPage.vSetChangeLock(true);
                            mboCommsSuspend = true;
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIDataPageRefreshed, 0, "", "");
                            tclsDataPage.vSetChangeLock(false);
                            mboCommsSuspend = false;
                            vNotifyProgramState(tenProgramState.enProgNormal, 0);
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOnline, 100, szErrOrMessage, "Upload complete");
                            break;
                        }
                    case tenProgramEvent.enRPCDownloadComplete:
                        {
                            vNotifyProgramState(tenProgramState.enProgNormal, 0);
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOnline, 100, szErrOrMessage, "Download complete");
                            break;
                        }
                    case tenProgramEvent.enWindowElementsLoaded:
                        {
                            vNotifyProgramState(tenProgramState.enProgNormal, 0);
                            mFormUDP.vNotify(tenMDIParentNotify.enMDILoaded, 0, "", "");
                            mboCommsSuspend = false;
                            mSplashLoading = false;
                            break;
                        }
                    case tenProgramEvent.enLoadCalibrationComplete:
                        {
                            tclsDataPage.vSetChangeLock(true);
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIDataPageRefreshed, 0, "", "");
                            tclsDataPage.vSetChangeLock(false);
                            vNotifyProgramState(tenProgramState.enProgNormal, 0);
                            break;
                        }
                    case tenProgramEvent.enProgramError:
                        {
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIShowError, 0, szErrOrMessage, "");
                            break;
                        }
                    case tenProgramEvent.enProgramMessage:
                        {
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIShowMessage, 0, szErrOrMessage, "");
                            break;
                        }
                    case tenProgramEvent.enProgramOnline:
                        {
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOnline, 0, szErrOrMessage, "");
                            mboCommsOnline = true;
                            break;
                        }
                    case tenProgramEvent.enProgramOffline:
                        {
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOffline, 0, szErrOrMessage, "");
                            mboCommsOnline = false;
                            break;
                        }
                    case tenProgramEvent.enRPCUploadIncrement:
                        {
                            if (75 > iData)
                            {
                                mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOnline, iData, szErrOrMessage, "Uploading " + iData.ToString() + "%");
                            }
                            else
                            {
                                mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOnline, iData, szErrOrMessage, "Uploaded data processing 80 %");
                            }
                            break;
                        }
                    case tenProgramEvent.enRPCDownloadIncrement:
                        {
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOnline, iData, szErrOrMessage, "Downloading " + iData.ToString() + "%");
                            break;
                        }
                    case tenProgramEvent.enSlowTaskProgress:
                        {
                            if (true == mboCommsOnline)
                            {
                                mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOnline, iData, szErrOrMessage, szErrOrMessage);
                            }
                            else
                            {
                                mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOffline, iData, szErrOrMessage, szErrOrMessage);
                            }

                            break;
                        }
                    case tenProgramEvent.enCommDisconnectOrError:
                        {
                            mboCommsSuspendError = true;
                            mFormUDP.vNotify(tenMDIParentNotify.enMDIToolStripOffline, 100, szErrOrMessage, "Comm disconnect or error, now offline please save and restart");
                            mboCommsOnline = false;
                            break;
                        }
                    case tenProgramEvent.enCommRequestSuspend:
                        {
                            mboCommsSuspend = true;
                            break;
                        }
                    case tenProgramEvent.enCommRequestUnSuspend:
                        {
                            mboCommsSuspend = false;
                            break;
                        }
                    case tenProgramEvent.enCommRequestDDDIReset:
                        {
                            mAPP_clsUDPComms.vResetDDDI();
                            break;
                        }
                    case tenProgramEvent.enUSBConnectDetected:
                        {
                            if (false == mboCommsOnline)
                            {
                                mAPP_clsUDPComms = new tclsUDSComms(mszAdapterDeviceName, mFormUDP.Handle);
                                mboCommsOnline = mAPP_clsUDPComms.Connected;

                                if (false == mboCommsOnline)
                                {
                                    mAPP_clsUDPComms.vDispose();
                                }
                                else
                                {
                                    mAPP_clsUDPComms.vResetDDDI();

                                    int iDDDIIDX;

                                    for (iDDDIIDX = 0; iDDDIIDX < ConstantData.BUFFERSIZES.u16UDS_MEASURE_RATE_COUNT; iDDDIIDX++)
                                    {
                                        tclsASAM.vSetDDDIResetPending(iDDDIIDX, true);
                                    }                                   

                                    mboCommsSuspendError = false;
                                }
                            }

                            break;
                        }
                    case tenProgramEvent.enUSBDisconnectDetected:
                        {
                            if (true == mboCommsOnline)
                            {
                                mboCommsOnline = false;
                                mAPP_clsUDPComms.CommsDisconnect();
                                mAPP_clsUDPComms.vDispose();
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

        public static void vNotifyProgramState(tenProgramState enProgramState, int iData)
        {
            switch (enProgramState)
            {
                case tenProgramState.enProgInitialising:
                    {
                        mFormUDP.vNotify(tenMDIParentNotify.enMDIStartInitialise, 0, "", "");
                        break;
                    }
                case tenProgramState.enProgNormal:
                    {
                        mFormUDP.vNotify(tenMDIParentNotify.enMDIEndInitialise, 0, "", "");
                        break;
                    }
                case tenProgramState.enRequestViewFocus:
                    {
                        mFormUDP.vNotify(tenMDIParentNotify.enMDIRequestViewFocus, iData, "", "");
                        break;
                    }
                case tenProgramState.enRequestShutdown:
                    {
                        mboCommsSuspend = true;
                        mAPP_clsUDPComms.CommsDisconnect();
                        mFormUDP.vNotify(tenMDIParentNotify.enMDIRequestShutdown, 0, "", "");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}