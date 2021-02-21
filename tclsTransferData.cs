/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Transfer Data                                          */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsTransferData.cs                                    */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UDP
{
    class tclsTransferData
    {
        byte[] mau8Data;
        byte mu8SID;
        byte mu8Sequence;
        UInt32 mu32Address;
        UInt32 mu32TotalByteCount;
        UInt32 mu32BlockByteCount;
        tclsUDSFrame mclsUDSFrame;

        public tclsTransferData()
        {
            mau8Data = new byte[65536];
            mclsUDSFrame = new tclsUDSFrame();
        }

        public uint u16GetTotalByteCount()
        {
            return mu32TotalByteCount;
        }

        public void setData(byte[] au8SourceData, UInt32 u32Address)
        {
            if (au8SourceData.Length <= mau8Data.Length)
            {
                Array.Copy(au8SourceData, mau8Data, au8SourceData.Length);
                mu32Address = u32Address;
                mu32TotalByteCount = (ushort)(au8SourceData.Length);
            }
        }

        public tclsUDSFrame clsSetParams(byte u8SID, byte u8SSID)
        {
            switch (u8SID)
            {
                case ConstantData.UDS.ru8SID_RD:
                    mu8Sequence = 0x10;
                    mclsUDSFrame.au8Data[ 0 ] = mu8Sequence;
                    mclsUDSFrame.au8Data[ 1 ] = 0x0c;
                    mclsUDSFrame.au8Data[ 2 ] = u8SID;
                    mclsUDSFrame.au8Data[ 3 ] = 0x00;
                    mclsUDSFrame.au8Data[ 4 ] = u8SSID;
                    mclsUDSFrame.au8Data[ 5 ] = (byte)(mu32Address / 0x1000000u);
                    mu32Address -= 0x1000000u * mclsUDSFrame.au8Data[ 5 ];
                    mclsUDSFrame.au8Data[ 6 ] = (byte)(mu32Address / 0x10000u);
                    mu32Address -= 0x10000u * mclsUDSFrame.au8Data[6];
                    mclsUDSFrame.au8Data[ 7 ] = (byte)(mu32Address / 0x100u);
                    mu32Address -= 0x100u * mclsUDSFrame.au8Data[7];
                    mu8SID = u8SID;
                    mu8Sequence = 0x21;
                    break;
            }

            return mclsUDSFrame;

        }

        public tclsUDSFrame clsGetNextFrame()
        {
            byte u8Index;
            bool boFrameAvailable = false;

            switch(mu8SID)
            {
                case ConstantData.UDS.ru8SID_RD:
                    mclsUDSFrame.au8Data[0] = mu8Sequence++;
                    mclsUDSFrame.au8Data[1] = (byte)(mu32Address);
                    mclsUDSFrame.au8Data[2] = (byte)(mu32TotalByteCount / 0x1000000u);
                    mu32TotalByteCount -= 0x1000000u * mclsUDSFrame.au8Data[2];
                    mclsUDSFrame.au8Data[3] = (byte)(mu32TotalByteCount / 0x10000u);
                    mu32TotalByteCount -= 0x10000u * mclsUDSFrame.au8Data[3];
                    mclsUDSFrame.au8Data[4] = (byte)(mu32TotalByteCount / 0x100u);
                    mu32TotalByteCount -= 0x100u * mclsUDSFrame.au8Data[4];
                    mclsUDSFrame.au8Data[5] = (byte)(mu32TotalByteCount);
                    mclsUDSFrame.au8Data[6] = 0x00;
                    mclsUDSFrame.au8Data[7] = 0x00;
                    mu8SID = 0;
                    boFrameAvailable = true;
                    break;

                default:
                    mclsUDSFrame.au8Data[0] = mu8Sequence++;
                    u8Index = 1;
                    mu8Sequence |= 0x1f;
                    while((mu32BlockByteCount > 0) && (u8Index < 8))
                    {
                      mclsUDSFrame.au8Data[ u8Index ] = 
                          mau8Data[ mau8Data.Length - mu32BlockByteCount ];
                      mu32BlockByteCount--;
                      u8Index++;
                      boFrameAvailable = true;
                    }
                    break;
            }

            if (true == boFrameAvailable)
            {
                return mclsUDSFrame;
            }
            else
            {
                return null;
            }
        }
    }
}
