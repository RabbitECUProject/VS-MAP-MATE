/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      UDS                                                    */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsUDS.cs                                             */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UDP
{
    public enum tenProgramEvent
    {
        enRPCFAIL,
        enRPCOK,
        enRPCDDDIOK,
        enRPCUploadComplete,
        enRPCUploadIncrement,
        enRPCDownloadComplete,
        enRPCDownloadIncrement,
        enLoadCalibrationComplete,
        enProgramMessage,
        enProgramOnline,
        enProgramOffline,
        enWindowElementsLoaded,
        enProgramError,
        enCommDisconnectOrError,
        enCommRequestSuspend,
        enCommRequestUnSuspend,
        enCommRequestDDDIReset,
        enUSBConnectDetected,
        enUSBDisconnectDetected,
        enSlowTaskProgress,
    }

    public struct tstRPCResponse
    {
        public tenProgramEvent enRPCResponse;
        public byte u8UDSResponse;
        public UInt32 u32RPCData1;
        public UInt32 u32RPCData2;
    }

    public class tclsUDSFrame
    {
        public UInt32 u32TimeStamp;
        public byte u8ByteCount;
        public byte[] au8Data;

        public tclsUDSFrame()
        {
            au8Data = new byte[8];
        }

        public void vFrameClearTrailing()
        {
            int uiIndex;

            for (uiIndex = (int)(8u - u8ByteCount); uiIndex < 8; uiIndex++)
            {
                au8Data[uiIndex] = ConstantData.UDS.ru8FRAME_PAD_BYTE;
            }
        }
    }

    public class tclsUDS
    {
        tclsISO15765 mclsISO15765;
        Queue<tclsUDSFrame> mclsOutputQueue;
        Queue<tclsUDSFrame> mclsInputQueue;
        tclsASCParser mclsASCParser;
        private byte[] mau8CommandBuffer;
        byte[] au8RMBA;
        byte[] au8WMBA;
        byte[] au8DDDI;

        public enum tenUDSResponse
        {
            enUDSRespOK   
        }

        public tclsUDS()
        {
            mau8CommandBuffer = new byte[8];
            mclsOutputQueue = new Queue<tclsUDSFrame> ();
            mclsInputQueue = new Queue<tclsUDSFrame> ();
            mclsISO15765 = new tclsISO15765();
            au8RMBA = new byte[ConstantData.UDS.ru8RMBA_HEADER_LENGTH +
                                ConstantData.UDS.ru8RMBA_MA_LENGTH +
                                ConstantData.UDS.ru8RMBA_MS_LENGTH];
            au8WMBA = new byte[ConstantData.UDS.ru8WMBA_HEADER_LENGTH +
                                ConstantData.UDS.ru8WMBA_MA_LENGTH +
                                ConstantData.UDS.ru8WMBA_MS_LENGTH];
            au8DDDI = new byte[ConstantData.UDS.ru8DDDI_HEADER_LENGTH];
        }

        public void vStartReplay(String szReplayASCPath)
        {
            mclsASCParser = new tclsASCParser(szReplayASCPath);
        }

        public void vReplay()
        {
            List<tclsUDSFrame> lstUDSReplayFrames = new List<tclsUDSFrame>();
            bool boHoldList = false;

            lstUDSReplayFrames = mclsASCParser.GetReplayMessages(false, ref boHoldList);

            /* Hold List when a segmented message is in prep but not yet time valid */
            if (false == boHoldList)
            {
                foreach (tclsUDSFrame clsUDSReplayFrame in lstUDSReplayFrames)
                {
                    mclsOutputQueue.Enqueue(clsUDSReplayFrame);
                }
            }
        }

        public int iGetReplayProgress()
        {
            int iReplayProgress = -1;

            if (null != mclsASCParser)
            {
                iReplayProgress = mclsASCParser.GetReplayProgress();
            }

            return iReplayProgress;
        }

        public void vClearQueue()
        {
            mclsOutputQueue.Clear();
        }

        public int vGetQueueCount()
        {
            return mclsOutputQueue.Count;
        }

        public void vStartRPC(byte u8SID, byte u8SSID, UInt32 u32Arg1, Int32 i32Arg2, byte[] au8Data)
        {
            byte[] au8Temp;
            tclsUDSFrame clsUDSFrame = new tclsUDSFrame();
            
            List<tclsUDSFrame> lUDSFrameList;

            switch (u8SID)
            {
                case ConstantData.UDS.ru8SID_DSC:
                {
                    clsUDSFrame.au8Data[0] = 2;
                    clsUDSFrame.au8Data[1] = ConstantData.UDS.ru8SID_DSC;
                    clsUDSFrame.au8Data[2] = u8SSID;
                    clsUDSFrame.u8ByteCount = 3;
                    mclsOutputQueue.Enqueue(clsUDSFrame);
                    break;
                }

                case ConstantData.UDS.ru8SID_TP:
                {
                    clsUDSFrame.au8Data[0] = 2;
                    clsUDSFrame.au8Data[1] = ConstantData.UDS.ru8SID_TP;
                    clsUDSFrame.au8Data[2] = 0;
                    clsUDSFrame.u8ByteCount = 3;
                    mclsOutputQueue.Enqueue(clsUDSFrame);
                    break;
                }

                case ConstantData.UDS.ru8SID_RDBI:
                {
                    clsUDSFrame.au8Data[0] = 3;
                    clsUDSFrame.au8Data[1] = ConstantData.UDS.ru8SID_RDBI;
                    au8Temp = BitConverter.GetBytes((UInt16)u32Arg1);
                    Array.Copy(au8Temp, 0, clsUDSFrame.au8Data, 2, 2);
                    clsUDSFrame.u8ByteCount = 4;
                    mclsOutputQueue.Enqueue(clsUDSFrame);
                    break;
                }

                case ConstantData.UDS.ru8SID_RMBA:
                {
                    au8RMBA[0] = ConstantData.UDS.ru8SID_RMBA;
                    au8RMBA[1] = ConstantData.UDS.ru8RMBA_ALFID;

                    switch (ConstantData.UDS.ru8RMBA_MA_LENGTH)
                    {
                        /* TODO as needed 
                        case 2:
                        {
                            au8Temp = BitConverter.GetBytes((UInt16)u32Arg1);
                            Array.Copy(au8Temp, 0, au8RMBA, ConstantData.UDS.ru8RMBA_MA_OFFSET, ConstantData.UDS.ru8RMBA_MA_LENGTH);
                            break;
                        }
                        */
                        case 4:
                        {
                            au8Temp = BitConverter.GetBytes(u32Arg1);
                            Array.Copy(au8Temp, 0, au8RMBA, ConstantData.UDS.ru8RMBA_MA_OFFSET, ConstantData.UDS.ru8RMBA_MA_LENGTH);
                            break;
                        }
                        /* TODO as needed 
                        default:
                        {
                            au8RMBA[ConstantData.UDS.ru8RMBA_MA_OFFSET] = (byte)(u32Arg1 & 0xff);
                            break;
                        }
                        */
                    }


                    switch (ConstantData.UDS.ru8RMBA_MS_LENGTH)
                    {
                        case 2:
                            {
                                au8Temp = BitConverter.GetBytes((UInt16)i32Arg2);
                                Array.Copy(au8Temp, 0, au8RMBA, ConstantData.UDS.ru8RMBA_MS_OFFSET, ConstantData.UDS.ru8RMBA_MS_LENGTH);
                                break;
                            }
                        /* TODO as needed 
                        default:
                            {
                                au8RMBA[ConstantData.UDS.ru8RMBA_MS_OFFSET] = (byte)(i32Arg2 & 0xff);
                                break;
                            }
                        */
                    }

                    lUDSFrameList = mclsISO15765.lstSegmentData(au8RMBA);

                    foreach (tclsUDSFrame clsSegUDSFrame in lUDSFrameList)
                    {
                        mclsOutputQueue.Enqueue(clsSegUDSFrame);
                    }

                    break;
                }

                case ConstantData.UDS.ru8SID_WMBA:
                {
                    au8WMBA[0] = ConstantData.UDS.ru8SID_WMBA;
                    au8WMBA[1] = ConstantData.UDS.ru8WMBA_ALFID;

                    switch (ConstantData.UDS.ru8WMBA_MA_LENGTH)
                    {
                        /* TODO as needed 
                        case 2:
                            {
                                au8Temp = BitConverter.GetBytes((UInt16)u32Arg1);
                                Array.Copy(au8Temp, 0, au8WMBA, ConstantData.UDS.ru8WMBA_MA_OFFSET, ConstantData.UDS.ru8WMBA_MA_LENGTH);
                                break;
                            }
                        */
                        case 4:
                            {
                                au8Temp = BitConverter.GetBytes(u32Arg1);
                                Array.Copy(au8Temp, 0, au8WMBA, ConstantData.UDS.ru8WMBA_MA_OFFSET, ConstantData.UDS.ru8WMBA_MA_LENGTH);
                                break;
                            }
                        /* TODO as needed 
                        default:
                            {
                                au8WMBA[ConstantData.UDS.ru8WMBA_MA_OFFSET] = (byte)(u32Arg1 & 0xff);
                                break;
                            }
                        */
                    }

                    switch (ConstantData.UDS.ru8WMBA_MS_LENGTH)
                    {
                        case 2:
                            {
                                au8Temp = BitConverter.GetBytes((UInt16)i32Arg2);
                                Array.Copy(au8Temp, 0, au8WMBA, ConstantData.UDS.ru8WMBA_MS_OFFSET, ConstantData.UDS.ru8WMBA_MS_LENGTH);
                                break;
                            }
                        /* TODO as needed 
                        default:
                            {
                                au8WMBA[ConstantData.UDS.ru8WMBA_MS_OFFSET] = (byte)(i32Arg2 & 0xff);
                                break;
                            }
                        */
                    }

                    byte[] au8WMBAWithData = new byte[au8WMBA.Length + au8Data.Length];

                    Array.Copy(au8WMBA, au8WMBAWithData, au8WMBA.Length);
                    Array.Copy(au8Data, 0, au8WMBAWithData, au8WMBA.Length, au8Data.Length);

                    lUDSFrameList = mclsISO15765.lstSegmentData(au8WMBAWithData);

                    foreach (tclsUDSFrame clsSegUDSFrame in lUDSFrameList)
                    {
                        mclsOutputQueue.Enqueue(clsSegUDSFrame);
                    }

                    break;
                }

                case ConstantData.UDS.ru8SID_DDDI:
                {
                    byte[] au8DDDIWithData = new byte[au8DDDI.Length + au8Data.Length];

                    au8DDDI[0] = ConstantData.UDS.ru8SID_DDDI;
                    au8DDDI[1] = ConstantData.UDS.ru8SFID_DDDI_DMBA;
                    au8DDDI[2] = (byte)(u32Arg1 & 0xff);
                    au8DDDI[3] = (byte)((u32Arg1 >> 8) & 0xff);
                    au8DDDI[4] = ConstantData.UDS.ru8DDDI_ALFID;
                    
                    Array.Copy(au8DDDI, au8DDDIWithData, au8DDDI.Length);
                    Array.Copy(au8Data, 0, au8DDDIWithData, au8DDDI.Length, au8Data.Length);

                    lUDSFrameList = mclsISO15765.lstSegmentData(au8DDDIWithData);

                    foreach (tclsUDSFrame clsSegUDSFrame in lUDSFrameList)
                    {
                        mclsOutputQueue.Enqueue(clsSegUDSFrame);
                    }

                    break;
                }

                case ConstantData.UDS.ru8SID_RC:
                {
                    clsUDSFrame.au8Data[0] = 4;
                    clsUDSFrame.au8Data[1] = ConstantData.UDS.ru8SID_RC;
                    clsUDSFrame.au8Data[2] = (byte)u32Arg1;
                    clsUDSFrame.au8Data[3] = ConstantData.UDS.ru8RCID_HighByte;
                    clsUDSFrame.au8Data[4] = u8SSID;
                    clsUDSFrame.u8ByteCount = 5;
                    mclsOutputQueue.Enqueue(clsUDSFrame);
                    break;
                }
            }
        }

        public void enProcessRPCResponse(byte[] au8Data, int iRXBufferStart, int iRXBufferEnd, ref tstRPCResponse stRPCResponse)
        {
            List<Byte[]> lstResponse;
            List<UInt32> lstData;
            UInt32 u32Data;
            int iMeasureDataIDX;
            int iResponseIDX;
            bool boAbortLoop = false;

            lstResponse = mclsISO15765.lstUnsegmentData(au8Data, iRXBufferStart, iRXBufferEnd);
            lstData = new List<UInt32>();

            if (0 < lstResponse.Count)
            {
                //try
                //{
                    for (iResponseIDX = 0; (iResponseIDX < lstResponse.Count) && (false == boAbortLoop); iResponseIDX++)
                    {
                        byte[] au8Response = lstResponse[iResponseIDX];

                        switch (au8Response[2])
                        {
                            case 0x40 + ConstantData.UDS.ru8SID_RDBI:
                                {
                                    UInt16 iReceivedMeasSID = BitConverter.ToUInt16(au8Response, 3);
                                    int iReceivedMeasureIDX = (int)(iReceivedMeasSID & 0xff);

                                    iMeasureDataIDX = 5;
                                    lstData.Clear();

                                    foreach (tstMeasurement stMeasurement in tclsASAM.mailstActiveMeasLists[iReceivedMeasureIDX])
                                    {
                                        switch (stMeasurement.iByteCount)
                                        {
                                            case 1:
                                                {
                                                    u32Data = (byte)au8Response[iMeasureDataIDX];
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    u32Data = (UInt32)BitConverter.ToUInt16(au8Response, iMeasureDataIDX);
                                                    break;
                                                }
                                            case 4:
                                                {
                                                    u32Data = BitConverter.ToUInt32(au8Response, iMeasureDataIDX);
                                                    break;
                                                }
                                            default:
                                                {
                                                    u32Data = 0;
                                                    break;
                                                }

                                        }

                                        lstData.Add(u32Data);
                                        iMeasureDataIDX += stMeasurement.iByteCount;
                                    }

                                    Program.vMeasuresUpdate(lstData, iReceivedMeasureIDX);
                                    stRPCResponse.enRPCResponse = tenProgramEvent.enRPCOK;
                                    break;
                                }

                            case 0x40 + ConstantData.UDS.ru8SID_RMBA:
                                {
                                    UInt32 u32TargetAddress = (UInt32)(au8Response[3] + 0x100 * au8Response[4] + 0x10000 * au8Response[5] + 0x1000000 * au8Response[6]);
                                    int iResponseDataByteCount = (int)(0x100 * au8Response[0] + au8Response[1] - 6);
                                    byte[] au8PageData = new byte[iResponseDataByteCount];//matthew check
                                    bool boTransferComplete;
                                    bool boUpdateOK;

                                    Array.Copy(au8Response, 7, au8PageData, 0, au8PageData.Length);
                                    boTransferComplete = Program.mAPP_clsUDPComms.boTransferCallBack(ref u32TargetAddress);

                                    if (true == boTransferComplete)
                                    {
                                        tclsDataPage.vSetWorkingData(u32TargetAddress, au8PageData, false, true);
                                        stRPCResponse.enRPCResponse = tenProgramEvent.enRPCUploadComplete;
                                        boAbortLoop = true;
                                    }
                                    else
                                    {
                                        boUpdateOK = tclsDataPage.vSetWorkingData(u32TargetAddress, au8PageData, false, false);

                                        if (true == boUpdateOK)
                                        {
                                            stRPCResponse.enRPCResponse = tenProgramEvent.enRPCUploadIncrement;
                                            Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "Uploading address: " + u32TargetAddress.ToString());
                                        }
                                    }
                                    break;
                                }

                        case 0x40 + ConstantData.UDS.ru8SID_WMBA:
                            {
                                UInt32 u32TargetAddress = (UInt32)(au8Response[3] + 0x100 * au8Response[4] + 0x10000 * au8Response[5] + 0x1000000 * au8Response[6]);
                                bool boTransferComplete;                               

                                boTransferComplete = Program.mAPP_clsUDPComms.boTransferCallBack(ref u32TargetAddress);

                                if (true == boTransferComplete)
                                {
                                    stRPCResponse.enRPCResponse = tenProgramEvent.enRPCDownloadComplete;
                                    boAbortLoop = true;
                                }
                                else
                                {
                                    stRPCResponse.enRPCResponse = tenProgramEvent.enRPCDownloadIncrement;
                                    Program.vNotifyProgramEvent(tenProgramEvent.enProgramMessage, 0, "Downloading address: " + u32TargetAddress.ToString());
                                }
                                break;
                            }

                        case 0x40 + ConstantData.UDS.ru8SID_RC:
                                {
                                    UInt32 u32TargetAddress = 0;

                                    if (ConstantData.UDS.ru8RCID_HighByte == au8Response[4])
                                    {
                                        switch (au8Response[5])
                                        {
                                            case ConstantData.UDS.ru8RCID_WorkNVMFreeze:
                                                {
                                                    Program.mAPP_clsUDPComms.boTransferCallBack(ref u32TargetAddress);
                                                    break;
                                                }
                                            case ConstantData.UDS.ru8RCID_WorkNVMClear:
                                                {
                                                    Program.mAPP_clsUDPComms.boTransferCallBack(ref u32TargetAddress);
                                                    break;
                                                }
                                            default:
                                                {
                                                    break;
                                                }
                                        }
                                    }
                                    break;
                                }

                            case 0x40 + ConstantData.UDS.ru8SID_DDDI:
                                {
                                    stRPCResponse.enRPCResponse = tenProgramEvent.enRPCDDDIOK;
                                    stRPCResponse.u32RPCData1 = (UInt32)(0x100 * au8Response[5]);
                                    stRPCResponse.u32RPCData1 += (UInt32)au8Response[4];
                                    break;
                                }

                            default:
                                {
                                    break;

                                }
                        }
                    }
                //}
                //catch
                //{
                //    Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, "RPC Response Exception");
                //}
            }

        }

        public tclsUDSFrame clsGetNextFrame()
        {
            if (mclsOutputQueue.Count != 0)
            {
                return mclsOutputQueue.Dequeue();
            }
            else
            {
                return null;
            }
        }
    }
}
