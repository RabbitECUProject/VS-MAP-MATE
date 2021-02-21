/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Communications Interface                               */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsCommsInterface.cs                                  */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using SharpPcap.WinPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDACUDSDotNet64Interface;

namespace UDP
{
    public class CaptureEventArgs
    {
        public byte[] au8ReceivedData;
    }


    public class tclsCommsInterface
    {
        bool mboChannelActive;
        MDACUSB2CAN64Interface.tclsMDACUSB2CAN64Interface mclsUSB2CAN64Interface;
        MDACUDSDotNet64Interface.tclsMDACUDSDotNet64Interface mclsUDSDotNet64Interface;
        WinPcapDevice mclsWLANDevice = null;
        MDACUSBInterface.MDACUSBInterface mclsUSBInterface;
        tenCommsType menCommsType;
        tclsUDSComms mclsUDSComms;
        bool mUSBCDCSubscribed = false;


        public int GetUSBPendingCount()
        {
            int iPendingCount = 0;

            if (tenCommsType.enUSBCDC == menCommsType)
            {
                iPendingCount = mclsUSBInterface.GetPendingCount();
            }

            return iPendingCount;
        }

        public tclsCommsInterface(IntPtr MDIFormHandle)
        {
            menCommsType = tenCommsType.enUnknown;
            tclsRegisterUSBDeviceNotification.RegisterUsbDeviceNotification(MDIFormHandle);
        }

        public bool CommsTryConnect(tclsUDSComms clsUDSComms, string szIniFilePath, int iReceiveMaxLength)
        {
            tclsIniParser mclsIniParser;
            string szDLL;
            string szBaud;
            int iBaud = 0;
            string szDeviceSerial;
            string szConnectionType;
            string szCommPort;
            string szDiagRXID;
            string szDiagTXID;
            UInt32 u32DiagRXID = 0;
            UInt32 u32DiagTXID = 0;

            mclsUDSComms = clsUDSComms;

            mclsIniParser = new tclsIniParser(szIniFilePath);

            try
            {
                szConnectionType = mclsIniParser.GetSetting("Program", "ConnectionType");

                if (0 == String.Compare(szConnectionType, "CANAL"))
                {
                    menCommsType = tenCommsType.enCANAL;
                }
                if (0 == String.Compare(szConnectionType, "CANRP1210"))
                {
                    menCommsType = tenCommsType.enCANRP1210;
                }
                if (0 == String.Compare(szConnectionType, "USBCDC"))
                {
                    menCommsType = tenCommsType.enUSBCDC;
                }
                if (0 == String.Compare(szConnectionType, "Serial"))
                {
                    menCommsType = tenCommsType.enSerial;
                }
                if (0 == String.Compare(szConnectionType, "CANLIBKVASER"))
                {
                    menCommsType = tenCommsType.enCANLIBKVASER;
                }
            }
            catch
            {
                szConnectionType = null;
            }

            try
            {
                szBaud = mclsIniParser.GetSetting("Devices", "ComsBaud");
                iBaud = Convert.ToInt16(szBaud);
            }
            catch
            {
                szBaud = null;
            }

            try
            {
                szDeviceSerial = mclsIniParser.GetSetting("Devices", "ComsDeviceSerial");
            }
            catch
            {
                szDeviceSerial = null;
            }

            try
            {
                szDLL = mclsIniParser.GetSetting("Devices", "RP1210Lib");
            }
            catch
            {
                szDLL = "Unknown";
            }

            try
            {
                szDiagRXID = mclsIniParser.GetSetting("Devices", "ComsDiagIDRX");

                u32DiagRXID = Convert.ToUInt32(szDiagRXID);
            }
            catch
            {
                szDiagRXID = null;
            }

            try
            {
                szDiagTXID = mclsIniParser.GetSetting("Devices", "ComsDiagIDTX");

                u32DiagTXID = Convert.ToUInt32(szDiagTXID);
            }
            catch
            {
                szDiagTXID = null;
            }

            try
            {
                szCommPort = mclsIniParser.GetSetting("Devices", "ComsPort");
            }
            catch
            {
                szCommPort = null;
            }

            switch (menCommsType)
            {
                case tenCommsType.enCANAL:
                {
                    if ((null != szDeviceSerial) && (null != szBaud) && (null != szDiagRXID))
                    {
                        try
                        {
                            mclsUSB2CAN64Interface = new MDACUSB2CAN64Interface.tclsMDACUSB2CAN64Interface(szDeviceSerial, szBaud);
                            mclsUSB2CAN64Interface.DataReceived +=
                                            new MDACUSB2CAN64Interface.tclsMDACUSB2CAN64Interface.DataReceivedEventHandler(vRXCallBackUDSOverCAN2USB);
                            mclsUSB2CAN64Interface.SetHeaderSize(10);
                            mboChannelActive = mclsUSB2CAN64Interface.Connect(iBaud, (int)u32DiagRXID, (int)u32DiagTXID);
                        }
                        catch
                        {
                            mboChannelActive = false;
                        }
                    }

                    break;
                }
                case tenCommsType.enCANLIBKVASER:
                {
                    if ((null != szDeviceSerial) && (null != szBaud) && (null != szDiagRXID))
                    {
                        try
                        {
                            mclsUDSDotNet64Interface = new MDACUDSDotNet64Interface.tclsMDACUDSDotNet64Interface();
                            mclsUDSDotNet64Interface.DataReceived +=
                                            new MDACUDSDotNet64Interface.tclsMDACUDSDotNet64Interface.DataReceivedEventHandler(vRXCallBackUDSOverCANLIBKVASER);
                            mclsUDSDotNet64Interface.SetHeaderSize(10);
                            mboChannelActive = mclsUDSDotNet64Interface.Connect(iBaud, szDLL, u32DiagRXID);
                        }
                        catch
                        {
                            mboChannelActive = false;
                        }
                    }

                    break;
                }
                case tenCommsType.enUSBCDC:
                {
                    if ((null != szBaud) && (null != szCommPort))
                    {
                        try
                        {
                            mclsUSBInterface = new MDACUSBInterface.MDACUSBInterface();
                            mclsUSBInterface.DataReceived +=
                                            new MDACUSBInterface.MDACUSBInterface.DataReceivedEventHandler(vRXCallBackUDSOverUSB);
                            mclsUSBInterface.SetHeaderSize(10);
                            mboChannelActive = mclsUSBInterface.Connect(MDACUSBInterface.MDACUSBInterface.tenClassType.enUSBVirtualComm,
                                0, 0, 0, 0, szCommPort, 1000 * iBaud);

                            if (true == mboChannelActive)
                            {
                                mUSBCDCSubscribed = true;
                            }
                        }
                        catch
                        {

                        }
                    }

                    break;
                }
            }

            return mboChannelActive;
        }

        public void CommsTryDisconnect()
        {
            switch (menCommsType)
            {
                case tenCommsType.enUSBCDC:
                {
                    try
                    {
                        if (true == mUSBCDCSubscribed)
                        {
                            mclsUSBInterface.DataReceived -= vRXCallBackUDSOverUSB;
                            mclsUSBInterface.Disconnect();
                            mUSBCDCSubscribed = false;
                        }
                    }
                    catch
                    {

                    }
                    break;
                }

                default:
                {
                    break;
                }
            }
        }


        public void vRXCallBackUDSOverUSB(object sender, MDACUSBInterface.CaptureEventArgs e)
        {
            mclsUDSComms.vRXCallBack(e.au8ReceivedData);
        }

        public void vRXCallBackUDSOverCAN2USB(object sender, MDACUSB2CAN64Interface.CaptureEventArgs e)
        {
            int iByteInputIDX = 17;
            int iByteOutputIDX = 10;
            int iFrames = (e.au8ReceivedData.Length - 10) / 15;
            byte[] au8Data = new byte[10 + iFrames * 8];

            while (iFrames-- > 0)
            {
                Array.Copy(e.au8ReceivedData, iByteInputIDX, au8Data, iByteOutputIDX, 8);
                iByteOutputIDX += 8;
                iByteInputIDX += 15;
            }

            mclsUDSComms.vRXCallBack(au8Data);
        }

        public void vRXCallBackUDSOverCANLIBKVASER(object sender, MDACUDSDotNet64Interface.CaptureEventArgs e)
        {
            mclsUDSComms.vRXCallBack(e.au8ReceivedData);
        }

        public void vRXCallBackUDSOverUDP(object sender, CaptureEventArgs e)
        {
            //mau8InPacketBuffer = e.Packet.Data;
            //UInt16 u16PacketLength;
            //int iRXByteIDX;
            //byte[] au8PacketLength = new byte[2];
            //byte[] au8PacketID = new byte[2];
            //byte[] au8WIFIMAC = new byte[6];
            //string szWIFIMAC = "00-00-00-00-00-00";

            //try
            //{
            //    Array.Copy(mau8InPacketBuffer, 6, au8WIFIMAC, 0, 6);
            //}
            //catch
            //{

            //}

            //szWIFIMAC = BitConverter.ToString(au8WIFIMAC);

            //if (0 == ConstantData.WLANSETTINGS.rszAPP_WIFI_MAC.CompareTo(szWIFIMAC))
            //{
            //    Array.Copy(mau8InPacketBuffer, ConstantData.WLANSETTINGS.ru8WLANLengthOffset, au8PacketLength, 0, 2);
            //    Array.Copy(mau8InPacketBuffer, ConstantData.WLANSETTINGS.ru8WLANSeqOffset, au8PacketID, 0, 2);
            //    Array.Reverse(au8PacketID);
            //    Array.Reverse(au8PacketLength);
            //    u16PacketLength = BitConverter.ToUInt16(au8PacketLength, 0);

            //    base.mu16PacketRXID = BitConverter.ToUInt16(au8PacketID, 0);
            //    //if (mu16PacketRXID == mu16PacketTXID)
            //    {
            //        //copy is a waste here - pass in start and length instead
            //        //Array.Copy(mau8InPacketBuffer, ConstantData.WLANSETTINGS.ru8WLANDataOffset, mau8InBusMuxBuffer, 0, mau8InPacketBuffer.Length - ConstantData.WLANSETTINGS.ru8WLANDataOffset);
            //        //mclsBMXMuxDemux.vReceiveBMX(mau8InBusMuxBuffer);
            //        mu16ResponseTimeoutCounter = 0;
            //        mu16PacketRXID = mu16PacketTXID;

            //        iRXByteIDX = ConstantData.WLANSETTINGS.ru8WLANDataOffset;

            //        while ((0 < mau8InPacketBuffer[iRXByteIDX])
            //            && ((mau8InPacketBuffer.Length - 8) > iRXByteIDX))
            //        {
            //            iRXByteIDX += 8;
            //        }

            //        iRXByteIDX = 0 < iRXByteIDX ? iRXByteIDX + 8 : 0;

            //        mclsUDS.enProcessRPCResponse(mau8InPacketBuffer, ConstantData.WLANSETTINGS.ru8WLANDataOffset, iRXByteIDX, ref mstRPCResponse);
            //        Program.vNotifyProgramEvent(mstRPCResponse.enRPCResponse, "");

            //        if (tenProgramEvent.enRPCDDDIOK == mstRPCResponse.enRPCResponse)
            //        {
            //            if (miDDDIIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT)
            //            {
            //                UInt16 uiDDDIIDX = tclsASAM.uiGetDDDISID(miDDDIIDX);

            //                if ((uiDDDIIDX == (UInt16)mstRPCResponse.u32RPCData1) ||
            //                    (uiDDDIIDX == 0))
            //                {
            //                    miDDDIIDX++;
            //                }
            //            }
            //        }
            //    }
            //    //else
            //    //{
            //    //    mu16PacketRXID = 0;
            //    //}
            //}
            //else
            //{

            //}
        }

        public void SendPacket(byte[] au8SendPacket)
        {
            byte[] au8Data = new byte[ConstantData.BUFFERSIZES.u16UDSOU_BUFF_TXPAYLOAD_SIZE + 10];

            Array.Copy(au8SendPacket, 0, au8Data, 10, ConstantData.BUFFERSIZES.u16UDSOU_BUFF_TXPAYLOAD_SIZE);

            switch (menCommsType)
            {
                case tenCommsType.enCANAL:
                    {
                        if (null != mclsUSB2CAN64Interface)
                        {
                            mclsUSB2CAN64Interface.SendPipelinedUDSArray(au8Data, false);
                        }
                        break;
                    }
                case tenCommsType.enCANLIBKVASER:
                    {
                        if (null != mclsUDSDotNet64Interface)
                        {
                            mclsUDSDotNet64Interface.SendPipelinedUDSArray(au8Data, false);
                        }
                        break;
                    }
                case tenCommsType.enUSBCDC:
                    {
                        try
                        {
                            mclsUSBInterface.SendPipelinedUDSArray(au8Data, false);
                        }
                        catch
                        {
                            Program.vNotifyProgramEvent(tenProgramEvent.enCommDisconnectOrError, 0, "Comm interface disconnect or error");
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void vDispose()
        {
            if (null != mclsWLANDevice)
            {
                mclsWLANDevice.StopCapture();
                mclsWLANDevice.Close();
            }
        }
    }
}
