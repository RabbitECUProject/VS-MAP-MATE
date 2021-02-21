/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      UDS Comms                                              */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsUDSComms.cs                                        */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.AirPcap;
using SharpPcap.WinPcap;
using System.Windows.Forms;


namespace UDP
{
    public class tclsUDSComms : tclsCommsChannel
    {
        Queue<int>[] maqiMeasureSubIDXPendingArray;
        int miDDDIIDX;
        int miResponseCount;
        int miTransferBlockSize;
        public tstRPCResponse mstRPCResponse;
        String mszReplayPath;
        bool mboReplayActive;
        tclsCommsInterface mclsCommsInterface;
        int miCommsWaits;

        public bool Connected {
            get { return mboChannelActive; }
        }

        public tclsUDSComms(string szAdapterDeviceName, IntPtr MDIFormHandle)
        {
            string szTransferBlockSize;
            mclsUDS = new tclsUDS();
            mclsCommsInterface = new tclsCommsInterface(MDIFormHandle);
            int iTransferBlockSizeLegal;

            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHost Calibration.INI");
            tclsErrlog.LogAppend("INI file opened...");

            try
            {
                szTransferBlockSize = mclsIniParser.GetSetting("Program", "TransferBlockSize");
                miTransferBlockSize = ConstantData.ISO15765.rs32SEG_TRANSFER_BLOCK_SIZE;

                for (iTransferBlockSizeLegal = 1; iTransferBlockSizeLegal <= 512; iTransferBlockSizeLegal *= 2)
                {
                    if (iTransferBlockSizeLegal == (int)Convert.ToInt16(szTransferBlockSize))
                    {
                        miTransferBlockSize = iTransferBlockSizeLegal;
                        break;
                    }
                }

                tclsErrlog.LogAppend("Transfer block size: " + miTransferBlockSize.ToString());
            }
            catch
            {
                miTransferBlockSize = ConstantData.ISO15765.rs32SEG_TRANSFER_BLOCK_SIZE;
            }


            maqiMeasureSubIDXPendingArray = new Queue<int>[ConstantData.BUFFERSIZES.u16UDS_MEASURE_RATE_COUNT];

            mau8InPacketBuffer = new byte[ConstantData.BUFFERSIZES.u16UDSOU_BUFF_RXPAYLOAD_SIZE];
            mau8ChannelTXPayload = new byte[ConstantData.BUFFERSIZES.u16UDSOU_BUFF_TXPAYLOAD_SIZE];

            mstTransferPageCB.enChannelMode = tenChannelMode.enChannelModeNone;
            mstTransferPageCB.u32StartAddress = 0;
            mstTransferPageCB.u32EndAddress = 0;
            mstTransferPageCB.iBlockSize = miTransferBlockSize;
            mstTransferPageCB.u32BytesToTransfer = 0;
            miDDDIIDX = 0;
            miResponseCount = 0;
            mboReplayActive = false;
            mstRPCResponse = new tstRPCResponse();
            miCommsWaits = 0;

            mboChannelActive = mclsCommsInterface.CommsTryConnect(this, 
                AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHost Calibration.INI",
                ConstantData.BUFFERSIZES.u16UDSOU_BUFF_RXPAYLOAD_SIZE);

            tclsErrlog.LogAppend("Channel active: " + mboChannelActive.ToString());
        }

        public void ChannelUDP_vMain()
        {
            int iTXFrameIDX = 0;
            tclsUDSFrame clsUDSTXFrame;
            UInt16 iMeasList;
            byte[] au8Payload;
            bool boNewPacket = false;
            int iMaxTXData = 0;

            if ((true == mboChannelActive) && 
                (false == Program.mboCommsSuspend) && 
                (false == Program.mboCommsSuspendError) &&
                (true == Program.mboCommsOnline) &&
                (0 == miCommsWaits))
            {
                if (0 == (mu16ResponseTimeoutCounter++ % 2))
                {
                    if ((mu16PacketRXID == mu16PacketTXID) ||
                        (tenChannelMode.enChannelModeReplayScript == mstTransferPageCB.enChannelMode))
                    /* only re-populate outgoing packet if ID response was 
                       received or timeout for dropped response */
                    {
                        if (20 > miResponseCount)
                        {
                            miResponseCount += 4;

                            if (20 <= miResponseCount)
                            {
                                Program.vNotifyProgramEvent(tenProgramEvent.enProgramOnline, 0, "");
                            }
                        }

                        Array.Clear(mau8ChannelTXPayload, 0, base.mau8ChannelTXPayload.Length);

                        /* Have all DDDI commands been sent? */
                        if ((miDDDIIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT) &&
                            (100 > mstPollingCB.u32DDDIRetries))
                        {
                            mclsUDS.vClearQueue();

                            if (0 <= mstPollingCB.u32DDDIWaitResponseCount)
                            {
                                mstPollingCB.u32DDDIRetries++;
                                tclsASAM.vClocksReset();

                                UInt16 uiDDDIIDX = 0;
                                au8Payload = null;

                                while (((0 == uiDDDIIDX) || (null == au8Payload)) && (miDDDIIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT))
                                {
                                    uiDDDIIDX = tclsASAM.uiGetDDDISID(miDDDIIDX);

                                    if (0 != uiDDDIIDX)
                                    {
                                        au8Payload = tclsASAM.au8GetDDDI(miDDDIIDX);
                                    }
                                    else
                                    { 
                                        mstPollingCB.u32DDDIWaitResponseCount = 0;
                                    }

                                    if (null == au8Payload)
                                    {
                                        miDDDIIDX++;
                                    }
                                }                                    

                                if ((null != au8Payload) && (0 < au8Payload.Length))
                                {
                                    base.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_DDDI,
                                        0,
                                        (uint)uiDDDIIDX,
                                        0,
                                        au8Payload);
                                }
                                else
                                {
                                    /* Skip the DDDI with the bad buffer */
                                    miDDDIIDX++;
                                    mstPollingCB.u32DDDIWaitResponseCount = 0;
                                }

                                mstPollingCB.u32DDDIWaitResponseCount = 50;

                                iMaxTXData = mau8ChannelTXPayload.Length;
                            }
                            else
                            {
                                mstPollingCB.u32DDDIWaitResponseCount--;
                            }
                        }
                        else if (miDDDIIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT)
                        {
                            Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, "Install Dynamic Data Identifier Service Failed");
                            miDDDIIDX = ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT;
                        }
                        else
                        {
                            if (tenChannelMode.enChannelModeNone == mstTransferPageCB.enChannelMode)
                            {
                                bool dataUpdate = false;

                                mstPollingCB.u32DDDIWaitResponseCount = 10;
                                mstPollingCB.u32DDDIRetries = 0;

                                if (mclsCommsInterface.GetUSBPendingCount() == 0)
                                {
                                    dataUpdate = tclsDataPage.boClockRegWritePage();
                                }

                                if ((false == dataUpdate) && (mclsCommsInterface.GetUSBPendingCount() == 0))
                                {
                                    /* If normal duplex dataflow */
                                    tclsASAM.vClockMeasurements(100);

                                    /* Send measurement requests */
                                    iMeasList = tclsASAM.iGetNextMeasListID();

                                    while (0 != iMeasList)
                                    {
                                        base.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_RDBI,
                                            0,
                                            (uint)iMeasList,
                                            0,
                                            null);

                                        iMeasList = tclsASAM.iGetNextMeasListID();
                                    }

                                    iMaxTXData = 56;
                                }
                                else
                                {
                                    iMaxTXData = 56;
                                    miCommsWaits = 2;
                                }
                            }
                            else if (tenChannelMode.enChannelModeReplayScript == mstTransferPageCB.enChannelMode)
                            {
                                UInt32 u32Temp = 0;
                                int iReplayProgress;

                                if (true == mboReplayActive)
                                {
                                    iReplayProgress = base.mclsUDS.iGetReplayProgress();
                                    if (100 > iReplayProgress)
                                    {
                                        base.mclsUDS.vReplay();
                                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "Script replay progress = " + iReplayProgress.ToString() + "%");
                                    }
                                    else
                                    {
                                        /* Kill the replay mode it finished */
                                        boTransferCallBack(ref u32Temp);
                                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "Script replay complete");
                                    }
                                }

                                iMaxTXData = mau8ChannelTXPayload.Length;
                            }
                            else if (tenChannelMode.enChannelModeUploading == mstTransferPageCB.enChannelMode)
                            {
                                mstTransferPageCB.iBlockSize = miTransferBlockSize;

                                if (0 == mstTransferPageCB.u32WaitResponseCount)
                                {
                                    base.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_RMBA,
                                    0,
                                    mstTransferPageCB.u32StartAddress,
                                    (Int32)mstTransferPageCB.iBlockSize,
                                    null);

                                    mstTransferPageCB.u32WaitResponseCount = 2;
                                }
                                else
                                {
                                    mstTransferPageCB.u32WaitResponseCount--;
                                }

                                iMaxTXData = mau8ChannelTXPayload.Length;
                            }
                            else if (tenChannelMode.enChannelModeDownloading == mstTransferPageCB.enChannelMode)
                            {
                                if (0 == mstTransferPageCB.u32WaitResponseCount)
                                {
                                    mstTransferPageCB.iBlockSize = miTransferBlockSize / 8;
                                    byte[] au8Data = new byte[mstTransferPageCB.iBlockSize];

                                    tclsDataPage.au8GetWorkingData(mstTransferPageCB.u32StartAddress, ref au8Data);

                                    base.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_WMBA,
                                        0,
                                        mstTransferPageCB.u32StartAddress,
                                        (Int32)mstTransferPageCB.iBlockSize,
                                        au8Data);

                                    mstTransferPageCB.u32WaitResponseCount = 1;
                                }
                                else
                                {
                                    mstTransferPageCB.u32WaitResponseCount--;
                                }

                                iMaxTXData = mau8ChannelTXPayload.Length;
                            }
                            else if (tenChannelMode.enChannelModeWorkingNVMFreeze == mstTransferPageCB.enChannelMode)
                            {
                                base.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_RC,
                                    ConstantData.UDS.ru8RCID_WorkNVMFreeze,
                                    1,
                                    0,
                                    null);

                                iMaxTXData = mau8ChannelTXPayload.Length;
                            }
                            else if (tenChannelMode.enChannelModeNVMClear == mstTransferPageCB.enChannelMode)
                            {
                                base.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_RC,
                                    ConstantData.UDS.ru8RCID_WorkNVMClear,
                                    1,
                                    0,
                                    null);

                                iMaxTXData = mau8ChannelTXPayload.Length;
                            }
                        }

                        clsUDSTXFrame = mclsUDS.clsGetNextFrame();

                        if ((null == clsUDSTXFrame) &&
                            (tenChannelMode.enChannelModeReplayScript != mstTransferPageCB.enChannelMode) &&
                            (tenChannelMode.enChannelModeUploading != mstTransferPageCB.enChannelMode) &&
                            (miDDDIIDX >= ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT))
                        {
                            base.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_TP,
                                0,
                                0,
                                0,
                                null);

                            clsUDSTXFrame = mclsUDS.clsGetNextFrame();
                        }

                        while (null != clsUDSTXFrame)
                        {
                            Array.Copy(clsUDSTXFrame.au8Data, 0,
                                mau8ChannelTXPayload, 8 * iTXFrameIDX, 8);

                            iTXFrameIDX++;

                            if ((iMaxTXData / 8) > iTXFrameIDX)
                            {
                                clsUDSTXFrame = mclsUDS.clsGetNextFrame();
                            }
                            else
                            {
                                clsUDSTXFrame = null;
                            }

                            boNewPacket = true;
                        }

                        if (true == boNewPacket)
                        {
                            vSendPacket();
                        }
                    }

                    /* retry after no response */
                    else if (tenChannelMode.enChannelModeReplayScript != mstTransferPageCB.enChannelMode)
                    {
                        if (0 < miResponseCount)
                        {
                            miResponseCount--;

                            if (0 == miResponseCount)
                            {
                                Program.vNotifyProgramEvent(tenProgramEvent.enProgramOffline, 0, "");
                            }
                        }

                        //vSendPacket();
                    }
                }
            }
            else
            {
                miResponseCount = 0;

                if ((true == Program.mboCommsSuspend) ||
                    (true == Program.mboCommsSuspendError) ||
                    (false == Program.mboCommsOnline))
                {
                    miCommsWaits = 10;
                }
            }

            miCommsWaits = 0 < miCommsWaits ? miCommsWaits - 1 : 0;
        }

        public void CommsDisconnect()
        {
            mclsCommsInterface.CommsTryDisconnect();
        }

        public void vResetDDDI()
        {
            /* Reset the DDDI index */
            miDDDIIDX = 0;

            /* Reset the DDDI retires */
            mstPollingCB.u32DDDIRetries = 0;
        }

        private bool vSetReplayScriptPath()
        {
            mboReplayActive = false;
            OpenFileDialog clsFileDialog = new OpenFileDialog();
            clsFileDialog.DefaultExt = "ASC:asc";
            clsFileDialog.InitialDirectory = "c:\\MinGW\\COMSProject\\";
            DialogResult Result = clsFileDialog.ShowDialog();

            if (DialogResult.OK == Result)
            {
                mszReplayPath = clsFileDialog.FileName;
                base.mclsUDS.vStartReplay(mszReplayPath);

                mboReplayActive = true;
            }

            return mboReplayActive;
        }

        private void vSendPacket()
        {
            try
            {
                mclsCommsInterface.SendPacket(mau8ChannelTXPayload);
                //Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "Poor response (" + iYield.ToString() + "%), increase Program Settings->ComsTickus");
            }
            catch
            {
                Program.vNotifyProgramEvent(tenProgramEvent.enCommDisconnectOrError, 0, "Comm interface disconnect or error");
                mboChannelActive = false;
            }
        }


        public void vDispose()
        {

        }

        public bool boTransferCallBack(ref UInt32 u32TargetAddress)
        {
            bool boTransferComplete = false;

            switch (mstTransferPageCB.enChannelMode)
            {
                case tenChannelMode.enChannelModeUploading:
                    {
                        if (u32TargetAddress == mstTransferPageCB.u32StartAddress)
                        {
                            mstTransferPageCB.u32StartAddress += (UInt32)mstTransferPageCB.iBlockSize;
                            mstTransferPageCB.u32WaitResponseCount = 0;
                            mstTransferPageCB.fProgress = 80 * (mstTransferPageCB.u32StartAddress - tclsASAM.u32GetCharMinAddress()) /
                                                                (tclsASAM.u32GetCharMaxAddress() - tclsASAM.u32GetCharMinAddress());
                        }

                        if (mstTransferPageCB.u32EndAddress <= mstTransferPageCB.u32StartAddress)
                        {
                            mstTransferPageCB.enChannelMode = tenChannelMode.enChannelModeNone;
                            boTransferComplete = true;
                            mstTransferPageCB.fProgress = 100;
                        }
                        break;
                    }
                case tenChannelMode.enChannelModeDownloading:
                    {
                        if (u32TargetAddress == mstTransferPageCB.u32StartAddress)
                        {
                            mstTransferPageCB.u32StartAddress += (UInt32)mstTransferPageCB.iBlockSize;
                            mstTransferPageCB.u32WaitResponseCount = 0;
                            mstTransferPageCB.fProgress = 80 * (mstTransferPageCB.u32StartAddress - tclsASAM.u32GetCharMinAddress()) /
                                                                (tclsASAM.u32GetCharMaxAddress() - tclsASAM.u32GetCharMinAddress());
                        }

                        if (mstTransferPageCB.u32EndAddress <= mstTransferPageCB.u32StartAddress)
                        {
                            mstTransferPageCB.enChannelMode = tenChannelMode.enChannelModeNone;
                            boTransferComplete = true;
                            mstTransferPageCB.fProgress = 100;
                        }

                        break;
                    }
                case tenChannelMode.enChannelModeWorkingNVMFreeze:
                    {
                        mstTransferPageCB.enChannelMode = tenChannelMode.enChannelModeNone;
                        boTransferComplete = true;
                        break;
                    }
                case tenChannelMode.enChannelModeNVMClear:
                    {
                        mstTransferPageCB.enChannelMode = tenChannelMode.enChannelModeNone;
                        boTransferComplete = true;
                        break;
                    }
                default:
                    {
                        mstTransferPageCB.enChannelMode = tenChannelMode.enChannelModeNone;
                        break;
                    }
            }
            return boTransferComplete;
        }

#if BUILD_CAN_KVASER
        public void vRXCallBackUDSOverCAN(object sender, MDACUDSDotNet64Interface.CaptureEventArgs e)
        {
            int iRXByteIDX = 10;

            if (mau8InPacketBuffer.Length < e.au8ReceivedData.Length)
            {
                Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, "RX Can buffer overflow");
            }

            Array.Copy(e.au8ReceivedData, 0, mau8InPacketBuffer, 0, e.au8ReceivedData.GetLength(0));
            
            while ((0 < mau8InPacketBuffer[iRXByteIDX])
                && ((mau8InPacketBuffer.Length - 8) > iRXByteIDX))
            {
                iRXByteIDX += 8;
            }

            iRXByteIDX = 0 < iRXByteIDX ? iRXByteIDX + 8 : 0;

            mclsUDS.enProcessRPCResponse(mau8InPacketBuffer, 10, iRXByteIDX, ref mstRPCResponse);
            Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, "");

            if (tenProgramEvent.enRPCDDDIOK == mstRPCResponse.enRPCResponse)
            {
                if (miDDDIIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT)
                {
                    UInt16 uiDDDIIDX = tclsASAM.uiGetDDDISID(miDDDIIDX);

                    if ((uiDDDIIDX == (UInt16)mstRPCResponse.u32RPCData1) ||
                        (uiDDDIIDX == 0))
                    {
                        miDDDIIDX++;
                    }
                }
            }

            mu16ResponseTimeoutCounter = 0;
            mu16PacketRXID = mu16PacketTXID;
        }
#endif

        public void vRXCallBack(byte[] au8Data)
        {
            int iRXByteIDX = 10;

            Array.Clear(mau8InPacketBuffer, 0, mau8InPacketBuffer.GetLength(0));
            Array.Copy(au8Data, 0, mau8InPacketBuffer, 0, au8Data.GetLength(0));

            while ((0 < mau8InPacketBuffer[iRXByteIDX])
                && ((mau8InPacketBuffer.Length - 8) > iRXByteIDX))
            {
                iRXByteIDX += 8;
            }

            iRXByteIDX = 0 < iRXByteIDX ? iRXByteIDX + 8 : 0;

            mclsUDS.enProcessRPCResponse(mau8InPacketBuffer, 10, iRXByteIDX, ref mstRPCResponse);
            //Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, 0, "");

            if (tenProgramEvent.enRPCDDDIOK == mstRPCResponse.enRPCResponse)
            {
                //Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, 0, "");

                if (miDDDIIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT)
                {
                    UInt16 uiDDDIIDX = tclsASAM.uiGetDDDISID(miDDDIIDX);

                    if (uiDDDIIDX == (UInt16)mstRPCResponse.u32RPCData1)
                    {
                        if (0 != uiDDDIIDX)
                        {
                            tclsASAM.vSetDDDIResetPending(miDDDIIDX, false);
                            Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "Install Dynamic Data Identifier PID: " + uiDDDIIDX.ToString() + " Successful");
                            mstPollingCB.u32DDDIRetries = 0;
                        }

                        tclsASAM.vSetDDDIResetPending(miDDDIIDX, false);
                        miDDDIIDX++;
                    }
                }

                mstPollingCB.u32DDDIWaitResponseCount = 0;
            }
            else if (tenProgramEvent.enRPCUploadIncrement == mstRPCResponse.enRPCResponse)
            {
                if (99 > mstTransferPageCB.fProgress)
                {
                    Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, (int)mstTransferPageCB.fProgress, "Upload Progress ");
                }
                else
                {
                    Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, (int)mstTransferPageCB.fProgress, "Upload Complete ");
                }
            }
            else if (tenProgramEvent.enRPCDownloadIncrement == mstRPCResponse.enRPCResponse)
            {
                Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, (int)mstTransferPageCB.fProgress, "Download Progress ");
            }
            else
            {
                Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, 0, "");
            }

            mu16ResponseTimeoutCounter = 0;
            mu16PacketRXID = mu16PacketTXID;
        }


#if BUILD_NATIVE_USB
        public void vRXCallBackUDSOverUSB(object sender, MDACUSBInterface.CaptureEventArgs e)
        {
            int iRXByteIDX = 10;


            if (mau8InPacketBuffer.Length < e.au8ReceivedData.Length)
            {
                Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, "RX USB buffer overflow");
            }

            Array.Copy(e.au8ReceivedData, 0, mau8InPacketBuffer, 0, e.au8ReceivedData.GetLength(0));
            
            while ((0 < mau8InPacketBuffer[iRXByteIDX])
                && ((mau8InPacketBuffer.Length - 8) > iRXByteIDX))
            {
                iRXByteIDX += 8;
            }

            iRXByteIDX = 0 < iRXByteIDX ? iRXByteIDX + 8 : 0;

            mclsUDS.enProcessRPCResponse(mau8InPacketBuffer, 10, iRXByteIDX, ref mstRPCResponse);

            if (tenProgramEvent.enRPCDDDIOK == mstRPCResponse.enRPCResponse)
            {
                Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, 0, "");

                if (miDDDIIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT)
                {
                    UInt16 uiDDDIIDX = tclsASAM.uiGetDDDISID(miDDDIIDX);

                    //if ((uiDDDIIDX == (UInt16)mstRPCResponse.u32RPCData1) ||
                    //    (uiDDDIIDX == 0))
                    if (uiDDDIIDX == (UInt16)mstRPCResponse.u32RPCData1) 
                    {
                        miDDDIIDX++;

                        if (0 != uiDDDIIDX)
                        {
                            Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "Install Dynamic Data Identifier PID: " + uiDDDIIDX.ToString() + " Successful");
                            mstPollingCB.u32DDDIRetries = 0;
                        }                        
                    }
                }

                mstPollingCB.u32DDDIWaitResponseCount = 0;
            }
            else if (tenProgramEvent.enRPCUploadIncrement == mstRPCResponse.enRPCResponse)
            {
                if (99 > mstTransferPageCB.fProgress)
                {
                    Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, (int)mstTransferPageCB.fProgress, "Upload Progress ");
                }
                else
                {
                    Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, (int)mstTransferPageCB.fProgress, "Upload Complete ");
                }
            }
            else if (tenProgramEvent.enRPCDownloadIncrement == mstRPCResponse.enRPCResponse)
            {
                Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, (int)mstTransferPageCB.fProgress, "Download Progress ");
            }
            else
            {
                Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, 0, "");
            }

               mu16ResponseTimeoutCounter = 0;
            mu16PacketRXID = mu16PacketTXID;
        }
#endif

#if BUILD_WIFI || BUILD_ETHERNET
        public void vRXCallBackUDSOverUDP(object sender, CaptureEventArgs e)
        {

            mau8InPacketBuffer = e.Packet.Data;
            UInt16 u16PacketLength;
            int iRXByteIDX;
            byte[] au8PacketLength = new byte[2];
            byte[] au8PacketID = new byte[2];
            byte[] au8WIFIMAC = new byte[6];
            string szWIFIMAC = "00-00-00-00-00-00";

            try
            {
                Array.Copy(mau8InPacketBuffer, 6, au8WIFIMAC, 0, 6);
            }
            catch
            {

            }

            szWIFIMAC = BitConverter.ToString(au8WIFIMAC);

            if (0 == ConstantData.WLANSETTINGS.rszAPP_WIFI_MAC.CompareTo(szWIFIMAC))
            {
                Array.Copy(mau8InPacketBuffer, ConstantData.WLANSETTINGS.ru8WLANLengthOffset, au8PacketLength, 0, 2);
                Array.Copy(mau8InPacketBuffer, ConstantData.WLANSETTINGS.ru8WLANSeqOffset, au8PacketID, 0, 2);
                Array.Reverse(au8PacketID);
                Array.Reverse(au8PacketLength);
                u16PacketLength = BitConverter.ToUInt16(au8PacketLength, 0);

                base.mu16PacketRXID = BitConverter.ToUInt16(au8PacketID, 0);
                //if (mu16PacketRXID == mu16PacketTXID)
                {
                    //copy is a waste here - pass in start and length instead
                    //Array.Copy(mau8InPacketBuffer, ConstantData.WLANSETTINGS.ru8WLANDataOffset, mau8InBusMuxBuffer, 0, mau8InPacketBuffer.Length - ConstantData.WLANSETTINGS.ru8WLANDataOffset);
                    //mclsBMXMuxDemux.vReceiveBMX(mau8InBusMuxBuffer);
                    mu16ResponseTimeoutCounter = 0;
                    mu16PacketRXID = mu16PacketTXID;

                    iRXByteIDX = ConstantData.WLANSETTINGS.ru8WLANDataOffset;

                    while ((0 < mau8InPacketBuffer[iRXByteIDX])
                        && ((mau8InPacketBuffer.Length - 8) > iRXByteIDX))
                    {
                        iRXByteIDX += 8;
                    }

                    iRXByteIDX = 0 < iRXByteIDX ? iRXByteIDX + 8 : 0;

                    mclsUDS.enProcessRPCResponse(mau8InPacketBuffer, ConstantData.WLANSETTINGS.ru8WLANDataOffset, iRXByteIDX, ref mstRPCResponse);
                    Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, "");

                    if (tenProgramEvent.enRPCDDDIOK == mstRPCResponse.enRPCResponse)
                    {
                        if (miDDDIIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT)
                        {
                            UInt16 uiDDDIIDX = tclsASAM.uiGetDDDISID(miDDDIIDX);

                            if ((uiDDDIIDX == (UInt16)mstRPCResponse.u32RPCData1) ||
                                (uiDDDIIDX == 0))
                            {
                                miDDDIIDX++;
                            }
                        }
                    }
                }
                //else
                //{
                //    mu16PacketRXID = 0;
                //}
            }
            else
            {

            }
        }
#endif

        public byte[] ChannelUDP_au8GetTxPayload()
        {
            return base.mau8ChannelTXPayload;
        }

        public bool boRequestCalPageTransfer(tenChannelMode enChannelMode)
        {
            bool boResult = false;

            if (tenChannelMode.enChannelModeNone == mstTransferPageCB.enChannelMode)
            {
                mstTransferPageCB.enChannelMode = enChannelMode;
                mstTransferPageCB.u32StartAddress = tclsASAM.u32GetCharMinAddress();
                mstTransferPageCB.u32EndAddress = tclsASAM.u32GetCharMaxAddress();
                mstTransferPageCB.u32BytesToTransfer = mstTransferPageCB.u32EndAddress - mstTransferPageCB.u32StartAddress + 1;
                mstTransferPageCB.fProgress = 0;
                boResult = true;
            }

            return boResult;
        }

        public bool boRequestScriptReplay(tenChannelMode enChannelMode)
        {
            bool boResult = false;

            if (tenChannelMode.enChannelModeNone == mstTransferPageCB.enChannelMode)
            {
                boResult = vSetReplayScriptPath();

                if (true == boResult)
                {
                    mstTransferPageCB.enChannelMode = enChannelMode;
                    Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "Replay Script Started");
                }
            }

            return boResult;
        }

        public bool boRequestUserRoutineControl(byte u8RID, bool boStart)
        {
            bool boResult = false;
            UInt32 u32ArgStartStop = (true == boStart) ? (UInt32)1 : (UInt32)0;

            base.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_RC,
                u8RID,
                u32ArgStartStop,
                0,
                null);

            return boResult;
        }
    }

    public struct tstTransferPageCB
    {
        public tenChannelMode enChannelMode;
        public UInt32 u32StartAddress;
        public UInt32 u32EndAddress;
        public UInt32 u32BytesToTransfer;
        public UInt32 u32WaitResponseCount;
        public int iBlockSize;
        public Single fProgress;
    }

    public struct tstPollingCB
    {
        public UInt32 u32DDDIWaitResponseCount;
        public UInt32 u32DDDIRetries;
    }
}
