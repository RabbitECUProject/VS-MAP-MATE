/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      ISO15765                                               */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsISO15765.cs                                        */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UDP
{
    class tclsISO15765
    {
        List<tclsUDSFrame> mTransmitFrameList;
        List<byte[]> mlau8ReceiveBufferList;
        byte[][] maau8ReceiveBuffer;

        public tclsISO15765()
        {
            int iBufferIDX;
            mTransmitFrameList = new List<tclsUDSFrame>();
            mlau8ReceiveBufferList = new List<byte[]>();
            maau8ReceiveBuffer = new byte[ConstantData.ISO15765.ru8PIPED_RESP_COUNT][];

            for (iBufferIDX = 0; iBufferIDX < ConstantData.ISO15765.ru8PIPED_RESP_COUNT; iBufferIDX++)
            {
                maau8ReceiveBuffer[iBufferIDX] = new byte[ConstantData.ISO15765.ru32SEG_RESP_BYTE_COUNT];
                //mlau8ReceiveBufferList.Add(maau8ReceiveBuffer[iBufferIDX]);
            }
        }

        public List<tclsUDSFrame> lstSegmentData(byte[] au8InputData)
        {
            int iSourceIDX;
            int iDestIDX;
            int iFrameIDX;
            int iTXBytesRequired;
            int iTXPayloadBytes;
            byte[] au8TempData = new byte[7];
            byte u8Seq;

            if (7 < au8InputData.Length)
            {
                iTXBytesRequired = 8 + (8 * (((au8InputData.Length - 7) / 7) + 1));
            }
            else
            {
                iTXBytesRequired = 8;
            }

            byte[] au8OutputData = new byte[iTXBytesRequired];

            mTransmitFrameList.Clear();

            if (8 > au8InputData.Length)
            {
                tclsUDSFrame clsOutputFrame = new tclsUDSFrame();
                Array.Copy(au8TempData, 0, clsOutputFrame.au8Data, 1, au8InputData.Length);
                clsOutputFrame.au8Data[0] = (byte)au8InputData.Length;
                mTransmitFrameList.Add(clsOutputFrame);
            }
            else
            {
                iFrameIDX = au8InputData.Length / 7;
                iDestIDX = 9 + 8 * (iFrameIDX - 1);
                iSourceIDX = 6 + 7 * (iFrameIDX - 1);

                /* Copy data */
                for (; iFrameIDX >= 1; iFrameIDX--)
                {
                    Array.Copy(au8InputData, iSourceIDX, 
                            au8TempData, 0, Math.Min(au8InputData.Length - iSourceIDX, 7));
                    Array.Copy(au8TempData, 0, au8OutputData, iDestIDX, 7);
                    iDestIDX -= 8;
                    iSourceIDX -= 7;
                }
                Array.Copy(au8InputData, 0, au8OutputData, iDestIDX + 1, 6);

                /* Write sequence numbers */
                iFrameIDX = au8InputData.Length / 7;
                u8Seq = (byte)((iFrameIDX - 1) & 0x0f);
                iDestIDX = 8 + 8 * (iFrameIDX - 1);
                for (; iFrameIDX >= 1; iFrameIDX--)
                {
                    au8OutputData[iDestIDX] = (byte)(0x20 + u8Seq--);
                    u8Seq &= 0x0f;
                    iDestIDX -= 8;
                }

                iTXPayloadBytes = 7 * (((iTXBytesRequired - 8) / 8)) + 6;

                au8OutputData[iDestIDX + 1] = (byte)(iTXPayloadBytes & 0xff);
                au8OutputData[iDestIDX] = (byte)(0x10 + ((iTXPayloadBytes & 0xf00) >> 8));

                for (iFrameIDX = 0; iFrameIDX < (iTXBytesRequired / 8); iFrameIDX++)
                {
                    tclsUDSFrame clsOutputFrame = new tclsUDSFrame();
                    Array.Copy(au8OutputData, 8 * iFrameIDX, clsOutputFrame.au8Data, 0, 8);
                    mTransmitFrameList.Add(clsOutputFrame);
                }
            }

            return mTransmitFrameList;
        }

        public List<byte[]> lstUnsegmentData(byte[] au8Data, int iRXBuffer, int iRXBufferEnd)
        {
            int iBufferIDX;
            int iBufferByteIDX = 0;
            int iSegRXCount = 0;
            int iSegRXReqCount = 0;
            byte u8Seq = 0;

            for (iBufferIDX = 0; iBufferIDX < ConstantData.ISO15765.ru8PIPED_RESP_COUNT; iBufferIDX++)
            {
                Array.Clear(maau8ReceiveBuffer[iBufferIDX], 0, ConstantData.ISO15765.ru8PIPED_RESP_COUNT);//matthew clear on demand
            }

            mlau8ReceiveBufferList.Clear();

            iBufferIDX = 0;

            while (((iRXBufferEnd - 8) >= iRXBuffer) && (ConstantData.ISO15765.ru8PIPED_RESP_COUNT > iBufferIDX))
            {
                /* Is the response a normal diag single frame response? */
                if ((0x00 != au8Data[iRXBuffer]) && (0x00 == (0xf0 & au8Data[iRXBuffer])) || (0x30 == (0xf0 & au8Data[iRXBuffer])))
                {
                    iBufferByteIDX = 1;
                    Array.Copy(au8Data, iRXBuffer, maau8ReceiveBuffer[iBufferIDX], iBufferByteIDX, 8);
                    mlau8ReceiveBufferList.Add(maau8ReceiveBuffer[iBufferIDX]);
                    iBufferByteIDX += 8;
                    iBufferIDX++;
                    iRXBuffer += 8;
                }
                
                /* Is the response an error response? */
                else if (0x7f == (0xf0 & au8Data[iRXBuffer]))
                {
                    iBufferByteIDX = 1;
                    Array.Copy(au8Data, iRXBuffer, maau8ReceiveBuffer[iBufferIDX], iBufferByteIDX, 8);
                    maau8ReceiveBuffer[iBufferIDX][0] &= 0x0f;
                    iSegRXReqCount = 0x100 * maau8ReceiveBuffer[iBufferIDX][0] + maau8ReceiveBuffer[iBufferIDX][1];
                    iBufferByteIDX += 8;
                    iRXBuffer += 8;
                    iSegRXCount = 6;
                    u8Seq = 0x0f;
                }

                else if (0x10 == (0xf0 & au8Data[iRXBuffer]))
                {
                    iBufferByteIDX = 0;
                    Array.Copy(au8Data, iRXBuffer, maau8ReceiveBuffer[iBufferIDX], iBufferByteIDX, 8);
                    maau8ReceiveBuffer[iBufferIDX][0] &= 0x0f;
                    iSegRXReqCount = 0x100 * maau8ReceiveBuffer[iBufferIDX][0] + maau8ReceiveBuffer[iBufferIDX][1];
                    iBufferByteIDX += 8;
                    iRXBuffer += 8;
                    iSegRXCount = 6;
                    u8Seq = 0x0f;
                }

                else if (0x20 == (0xf0 & au8Data[iRXBuffer]))
                {
                    if (((u8Seq + 1) % 0x10) == (0x0f & au8Data[iRXBuffer]))
                    {
                        u8Seq = au8Data[iRXBuffer];
                        u8Seq &= 0x0f;
                        Array.Copy(au8Data, iRXBuffer + 1, maau8ReceiveBuffer[iBufferIDX], iBufferByteIDX, 7);
                        iSegRXCount += 7;
                        if (iSegRXReqCount <= iSegRXCount)
                        {
                            mlau8ReceiveBufferList.Add(maau8ReceiveBuffer[iBufferIDX]);
                            iBufferIDX++;
                        }
                        iRXBuffer += 8;
                        iBufferByteIDX += 7;
                    }
                    else
                    {
                        iSegRXReqCount = 0;
                        iRXBuffer += 8;
                    }
                }
                else
                {
                    break;
                }
            }

            return mlau8ReceiveBufferList;
        }
    }
}
