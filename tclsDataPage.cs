/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Data Page                                              */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsDataPage.cs                                        */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UDP
{
    public static class ExtensionMethods
    {
        public static byte[] ByteSubArray(byte[] data, int index, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

    public struct tstReg32Write
    {
        public UInt32 u32Address;
        public UInt32 u32Data;
        public int iSequence;
    }

    public struct tstReg16Write
    {
        public UInt32 u32Address;
        public UInt16 u16Data;
        public int iSequence;
    }

    public struct tstReg8Write
    {
        public UInt32 u32Address;
        public byte u8Data;
        public int iSequence;
    }

    static class tclsDataPage
    {
        static byte[] mau8WorkingPage;
        static UInt32 mu32WorkingBaseAddress;
        static int miStartDirtyRange;
        static int miEndDirtyRange;
        static bool mboLockForChanges;
        static UInt32 mu32OldUpdateAddress;
        static int miTransferBlockSize;
        static List<tstReg32Write> mlstReg32Write;
        static List<tstReg16Write> mlstReg16Write;
        static List<tstReg8Write> mlstReg8Write;
        static List<tstReg32Write> mlstReg32RepeatWrite;
        static List<tstReg16Write> mlstReg16RepeatWrite;
        static List<tstReg8Write> mlstReg8RepeatWrite;
        static int miSequence;
        static int miSequencePending;
        static int miRepeat16Index;
        static bool boCRC16ReportOK;
        static bool boCRC16Pending;
        static bool mboSort16Pending;
        static bool mboSort32Pending;

        static tclsDataPage()
        {
            mau8WorkingPage = new byte[32768];
            mu32OldUpdateAddress = 0xffffffff;
            mu32WorkingBaseAddress = 0;
            miSequence = -1;
            miSequencePending = -1;
            miStartDirtyRange = -1;
            miEndDirtyRange = -1;
            mboLockForChanges = false;
            string szTransferBlockSize;
            int iTransferBlockSizeLegal;

            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHost Calibration.INI");

            try
            {
                szTransferBlockSize = mclsIniParser.GetSetting("Program", "TransferBlockSize");
                miTransferBlockSize = -1;

                for (iTransferBlockSizeLegal = 1; iTransferBlockSizeLegal <= 512; iTransferBlockSizeLegal *= 2)
                {
                    if (iTransferBlockSizeLegal == (int)Convert.ToInt16(szTransferBlockSize))
                    {
                        miTransferBlockSize = iTransferBlockSizeLegal;
                        break;
                    }
                }
            }
            catch
            {
                miTransferBlockSize = -1;
            }

            mlstReg32Write = new List<tstReg32Write>();
            mlstReg16Write = new List<tstReg16Write>();
            mlstReg8Write = new List<tstReg8Write>();
            mlstReg32RepeatWrite = new List<tstReg32Write>();
            mlstReg16RepeatWrite = new List<tstReg16Write>();
            mlstReg8RepeatWrite = new List<tstReg8Write>();
        }

        static public void IncSequence()
        {
            miSequence++;
        }

        static public void vReportCRC16(bool CRC16OK)
        {
            if ((true == boCRC16Pending) && (true == CRC16OK))
            {
                mlstReg8RepeatWrite.Clear();
                mlstReg16RepeatWrite.Clear();
                mlstReg32RepeatWrite.Clear();

                boCRC16Pending = false;
                miRepeat16Index = 0;
            }

            boCRC16ReportOK = CRC16OK;
        }

        static public UInt16 GetDataPageCRC16()
        {
            UInt16 CRC16 = 0xffff;
            UInt16 iConfigSize = (UInt16)(tclsASAM.u32GetCharMaxAddress() - (int)tclsASAM.u32GetCharMinAddress());
            UInt16 iByteOffset = 0;

            /* Subtract bytes used for CRC and correct by 1 */
            iConfigSize -= 1;

            while (iConfigSize-- > 0)
            {
                UInt16 CRC16ShiftL = (UInt16)((CRC16 & 0x00ff) << 8);
                UInt16 CRC16ShiftR = (UInt16)((CRC16 & 0xff00) >> 8);
                UInt16 CRC16Index = (UInt16)(CRC16ShiftR ^ mau8WorkingPage[iByteOffset]);

                CRC16 = (UInt16)(CRC16ShiftL ^ ConstantData.CRC16Data.rau16CRC16[CRC16Index]);
                iByteOffset++;
            }

            return CRC16;
        }

        static public void AddReg32Write(tstReg32Write stReg32Write)
        {
            mlstReg32Write.Add(stReg32Write);
        }

        static public void AddReg16Write(tstReg16Write stReg16Write)
        {
            mlstReg16Write.Add(stReg16Write);

            mboSort16Pending = true;
        }

        static public void AddReg8Write(tstReg8Write stReg8Write)
        {
            mlstReg8Write.Add(stReg8Write);
        }

        static public void vSetChangeLock(bool boLockForChanges)
        {
            if (true == boLockForChanges)
            {
                mboLockForChanges = boLockForChanges;
            }
            else
            {
                mboLockForChanges = boLockForChanges;
                miStartDirtyRange = -1;
                miEndDirtyRange = -1;
            }
        }

        static public void vSetWorkingBaseAddress(UInt32 u32WorkingBaseAddress)
        {
            mu32WorkingBaseAddress = u32WorkingBaseAddress;
        }

        static public void au8GetWorkingData(UInt32 u32Address, ref byte[] au8Data)
        {
            Array.Copy(mau8WorkingPage, u32Address - mu32WorkingBaseAddress, au8Data, 0, au8Data.Length);
        }

        static public bool vSetWorkingData(UInt32 u32Address, byte[] au8Data, bool boFromFile, bool boLastBlock)
        {
            bool boUpdateOK = false;

            if (true == boFromFile)
            {
                Array.Copy(au8Data, 0, mau8WorkingPage, u32Address - mu32WorkingBaseAddress, au8Data.Length);
                boUpdateOK = true;
                mu32OldUpdateAddress = u32Address;
            }
            else
            {
                if (0xffffffff == mu32OldUpdateAddress)
                {
                    if (mu32WorkingBaseAddress == u32Address)
                    {
                        Array.Copy(au8Data, 0, mau8WorkingPage, u32Address - mu32WorkingBaseAddress, au8Data.Length);
                        boUpdateOK = true;
                        mu32OldUpdateAddress = u32Address;
                    }
                }
                else
                {
                    if ((mu32OldUpdateAddress + miTransferBlockSize) == u32Address)
                    {
                        Array.Copy(au8Data, 0, mau8WorkingPage, u32Address - mu32WorkingBaseAddress, au8Data.Length);
                        boUpdateOK = true;
                        mu32OldUpdateAddress = u32Address;
                    }

                    mu32OldUpdateAddress = true == boLastBlock ? 0xffffffff : mu32OldUpdateAddress;
                }
            }

            return boUpdateOK;
        }

        static public void au16GetWorkingData(UInt32 u32Address, ref UInt16[] au16Data)
        {
            for (int i16IDX = 0; i16IDX < au16Data.Length; i16IDX++)
            {
                au16Data[i16IDX] = BitConverter.ToUInt16(mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress + 2 * i16IDX));
            }
        }

        static public void au32GetWorkingData(UInt32 u32Address, ref UInt32[] au32Data)
        {
            for (int iUInt32IDX = 0; iUInt32IDX < au32Data.Length; iUInt32IDX++)
            {
                au32Data[iUInt32IDX] = BitConverter.ToUInt32(mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress + 4 * iUInt32IDX));
            }
        }

        static public void as8GetWorkingData(UInt32 u32Address, ref SByte[] as8Data)
        {
//MATTHEW TBC
        }

        static public void as16GetWorkingData(UInt32 u32Address, ref Int16[] as16Data)
        {
            for (int i16IDX = 0; i16IDX < as16Data.Length; i16IDX++)
            {
                as16Data[i16IDX] = BitConverter.ToInt16(mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress + 2 * i16IDX));
            }
        }

        static public void as32GetWorkingData(UInt32 u32Address, ref Int32[] as32Data)
        {
            for (int iUInt32IDX = 0; iUInt32IDX < as32Data.Length; iUInt32IDX++)
            {
                as32Data[iUInt32IDX] = BitConverter.ToInt32(mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress + 4 * iUInt32IDX));
            }
        }


        static public void u8GetWorkingData(UInt32 u32Address, ref byte u8Data)
        {
            u8Data = mau8WorkingPage[u32Address - mu32WorkingBaseAddress];
        }

        static public void u16GetWorkingData(UInt32 u32Address, ref UInt16 u16Data)
        {
            u16Data = BitConverter.ToUInt16(mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress));
        }

        static public void u32GetWorkingData(UInt32 u32Address, ref UInt32 u32Data)
        {
            u32Data = BitConverter.ToUInt32(mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress));
        }

        static public void s8GetWorkingData(UInt32 u32Address, ref sbyte s8Data)
        {
            s8Data = (sbyte)mau8WorkingPage[u32Address - mu32WorkingBaseAddress];
        }

        static public void s16GetWorkingData(UInt32 u32Address, ref Int16 s16Data)
        {
            s16Data = BitConverter.ToInt16(mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress));
        }

        static public void s32GetWorkingData(UInt32 u32Address, ref Int32 s32Data)
        {
            s32Data = BitConverter.ToInt32(mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress));
        }

        static public void u8SetWorkingData(UInt32 u32Address, byte u8Data)
        {
            if (mau8WorkingPage[u32Address - mu32WorkingBaseAddress] != u8Data)
            {
                mau8WorkingPage[u32Address - mu32WorkingBaseAddress] = u8Data;

                if (false == mboLockForChanges)
                {
                    tstReg8Write stReg8Write = new tstReg8Write();
                    stReg8Write.iSequence = miSequence;
                    stReg8Write.u32Address = u32Address;
                    stReg8Write.u8Data = u8Data;
                    mlstReg8Write.Add(stReg8Write);
                }
            }
        }

        static public void au16SetWorkingData(UInt32 u32Address, ref UInt16[] au16Data)
        {
            foreach( UInt16 u16Data in au16Data)
            {
                u16SetWorkingData(u32Address, u16Data);

                if (false == mboLockForChanges)
                {
                    tstReg16Write stReg16Write = new tstReg16Write();
                    stReg16Write.iSequence = miSequence;
                    stReg16Write.u32Address = u32Address;
                    stReg16Write.u16Data = u16Data;
                    mlstReg16Write.Add(stReg16Write);
                    mboSort16Pending = true;
                }

                u32Address += 2;
            }
        }

        static public void u16SetWorkingData(UInt32 u32Address, UInt16 u16Data)
        {
            byte[] au8Data = BitConverter.GetBytes(u16Data);

            if (!Array.Equals(ExtensionMethods.ByteSubArray(mau8WorkingPage, (int)(u32Address - mu32WorkingBaseAddress), 2), au8Data))
            {
                Array.Copy(au8Data, 0, mau8WorkingPage, 
                    (int)(u32Address - mu32WorkingBaseAddress), au8Data.Length);

                if (false == mboLockForChanges)
                {
                    tstReg16Write stReg16Write = new tstReg16Write();
                    stReg16Write.iSequence = miSequence;
                    stReg16Write.u32Address = u32Address;
                    stReg16Write.u16Data = u16Data;
                    mlstReg16Write.Add(stReg16Write);
                    mboSort16Pending = true;
                }
            }
        }

        static public void u32SetWorkingData(UInt32 u32Address, UInt32 u32Data)
        {
            byte[] au8Data = BitConverter.GetBytes(u32Data);

            if (!Array.Equals(ExtensionMethods.ByteSubArray(mau8WorkingPage, (int)(u32Address - mu32WorkingBaseAddress), 4), au8Data))
            {
                Array.Copy(au8Data, 0, mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress), au8Data.Length);

                if (false == mboLockForChanges)
                {
                    tstReg32Write stReg32Write = new tstReg32Write();
                    stReg32Write.u32Address = u32Address;
                    stReg32Write.u32Data = u32Data;
                    mlstReg32Write.Add(stReg32Write);
                }
            }
        }

        static public void s8SetWorkingData(UInt32 u32Address, sbyte s8Data)
        {
            if (mau8WorkingPage[u32Address - mu32WorkingBaseAddress] != (byte)s8Data)
            {
                mau8WorkingPage[u32Address - mu32WorkingBaseAddress] = (byte)s8Data;

                if (false == mboLockForChanges)
                {
                    tstReg8Write stReg8Write = new tstReg8Write();
                    stReg8Write.iSequence = miSequence;
                    stReg8Write.u32Address = u32Address;
                    stReg8Write.u8Data = (byte)s8Data;
                    mlstReg8Write.Add(stReg8Write);
                }
            }
        }

        static public void s16SetWorkingData(UInt32 u32Address, Int16 s16Data)
        {
            byte[] au8Data = BitConverter.GetBytes(s16Data);

            if (!Array.Equals(ExtensionMethods.ByteSubArray(mau8WorkingPage, (int)(u32Address - mu32WorkingBaseAddress), 2), au8Data))
            {
                Array.Copy(au8Data, 0, mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress), au8Data.Length);

                if (false == mboLockForChanges)
                {
                    tstReg16Write stReg16Write = new tstReg16Write();
                    stReg16Write.iSequence = miSequence;
                    stReg16Write.u32Address = u32Address;
                    stReg16Write.u16Data = (UInt16)s16Data;
                    mlstReg16Write.Add(stReg16Write);
                    mboSort16Pending = true;
                }
            }
        }

        static public void s32SetWorkingData(UInt32 u32Address, Int32 s32Data)
        {
            byte[] au8Data = BitConverter.GetBytes(s32Data);

            if (!Array.Equals(ExtensionMethods.ByteSubArray(mau8WorkingPage, (int)(u32Address - mu32WorkingBaseAddress), 4), au8Data))
            {
                Array.Copy(au8Data, 0, mau8WorkingPage,
                    (int)(u32Address - mu32WorkingBaseAddress), au8Data.Length);

                if (false == mboLockForChanges)
                {
                    tstReg32Write stReg32Write = new tstReg32Write();
                    stReg32Write.iSequence = miSequence;
                    stReg32Write.u32Address = u32Address;
                    stReg32Write.u32Data = (UInt32)s32Data;
                    mlstReg32Write.Add(stReg32Write);
                }
            }
        }

        static void vSetDirtyRange(UInt32 u32PageOffset, int iDataSize)
        {
            int iPageOffset = (int)u32PageOffset;

            if (false == mboLockForChanges)
            {

                if (-1 == miStartDirtyRange)
                {
                    miStartDirtyRange = iPageOffset;
                }
                else
                {
                    miStartDirtyRange = iPageOffset < miStartDirtyRange ? iPageOffset : miStartDirtyRange;
                }

                if (-1 == miEndDirtyRange)
                {
                    miEndDirtyRange = iPageOffset + iDataSize;
                }
                else
                {
                    miEndDirtyRange = (iPageOffset + iDataSize) > miEndDirtyRange ? (iPageOffset + iDataSize) : miEndDirtyRange;
                }
            }
        }

        public static bool boClockRegWritePage()
        {
            bool boWritten = false;

            if ((0 != mlstReg32Write.Count) || ((0 != mlstReg32RepeatWrite.Count) && (-1 != miRepeat16Index)))
            {
                UInt32 u32Address = 0;
                byte[] au8Data = GetData32Contiguous(ref u32Address);

                Program.mAPP_clsUDPComms.mclsUDS.vClearQueue();
                Program.mAPP_clsUDPComms.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_WMBA,
                    0x00,
                    u32Address,
                    au8Data.Length,
                    au8Data);

                boWritten = true;
            }
            else if (0 != mlstReg16Write.Count)
            {
                UInt32 u32Address = 0;

                if (0 == Program.mAPP_clsUDPComms.mclsUDS.vGetQueueCount())
                {
                    byte[] au8Data = GetData16Contiguous(ref u32Address, false);

                    Program.mAPP_clsUDPComms.mclsUDS.vClearQueue();
                    Program.mAPP_clsUDPComms.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_WMBA,
                        0x00,
                        u32Address,
                        au8Data.Length,
                        au8Data);

                    boWritten = true;
                }
            }
            else if ((0 != mlstReg16RepeatWrite.Count) && (-1 != miRepeat16Index))
            {
                UInt32 u32Address = 0;

                if (0 == Program.mAPP_clsUDPComms.mclsUDS.vGetQueueCount())
                {
                    byte[] au8Data = GetData16Contiguous(ref u32Address, true);

                    Program.mAPP_clsUDPComms.mclsUDS.vClearQueue();
                    Program.mAPP_clsUDPComms.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_WMBA,
                        0x00,
                        u32Address,
                        au8Data.Length,
                        au8Data);

                    boWritten = true;
                }
            }
            else if ((0 != mlstReg8Write.Count) || ((0 != mlstReg8RepeatWrite.Count) && (-1 != miRepeat16Index)))
            {
                byte[] au8Data = new byte[2];

                au8Data[0] = (byte)(mlstReg8Write[mlstReg8Write.Count - 1].u8Data);

                Program.mAPP_clsUDPComms.mclsUDS.vClearQueue();
                Program.mAPP_clsUDPComms.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_WMBA,
                    0x00,
                    mlstReg8Write[mlstReg8Write.Count - 1].u32Address,
                    1,
                    au8Data);

                mlstReg8Write.Remove(mlstReg8Write[mlstReg8Write.Count - 1]);

                boWritten = true;
            }

            if (true == boWritten)
            {
                boCRC16Pending = true;
            }
            return boWritten;
        }

        public static byte[] GetData16Contiguous(ref UInt32 u32OutAddress, bool repeat)
        {
            byte[] au8Data = new byte[2];
            UInt32 u32Address = 0;

            if (mboSort16Pending == true)
            {
                Sort16Pending();
                mboSort16Pending = false;
            }

            if (repeat == false)
            {
                u32Address = mlstReg16Write[0].u32Address;

                au8Data[0] = (byte)(mlstReg16Write[0].u16Data & 0xff);
                au8Data[1] = (byte)((mlstReg16Write[0].u16Data & 0xff00) >> 8);

                //mlstReg16RepeatWrite.Add(mlstReg16Write[0]);
                mlstReg16Write.RemoveAt(0);

                u32OutAddress = u32Address;

                while ((0 < mlstReg16Write.Count) && (32 > au8Data.Length))
                {
                    if (u32Address == mlstReg16Write[0].u32Address)
                    {
                        /* Remove duplicates */
                        mlstReg16Write.RemoveAt(0);
                    }
                    else if ((u32Address + 2) == mlstReg16Write[0].u32Address)
                    {
                        Array.Resize(ref au8Data, au8Data.Length + 2);
                        au8Data[au8Data.Length - 2] = (byte)(mlstReg16Write[0].u16Data & 0xff);
                        au8Data[au8Data.Length - 1] = (byte)((mlstReg16Write[0].u16Data & 0xff00) >> 8);

                        //mlstReg16RepeatWrite.Add(mlstReg16Write[0]);
                        mlstReg16Write.RemoveAt(0);
                        u32Address += 2;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                if (-1 != miRepeat16Index)
                {
                    u32Address = mlstReg16RepeatWrite[miRepeat16Index].u32Address;
                    u32OutAddress = u32Address;

                    au8Data[0] = (byte)(mlstReg16RepeatWrite[miRepeat16Index].u16Data & 0xff);
                    au8Data[1] = (byte)((mlstReg16RepeatWrite[miRepeat16Index].u16Data & 0xff00) >> 8);

                    miRepeat16Index = (mlstReg16RepeatWrite.Count - 1) > miRepeat16Index ?
                                        miRepeat16Index + 1 : -1;

                    while ((0 < mlstReg16RepeatWrite.Count) && (-1 != miRepeat16Index))
                    {
                        if (u32Address + 2 == mlstReg16RepeatWrite[miRepeat16Index].u32Address)
                        {
                            Array.Resize(ref au8Data, au8Data.Length + 2);
                            au8Data[au8Data.Length - 2] = (byte)(mlstReg16RepeatWrite[miRepeat16Index].u16Data & 0xff);
                            au8Data[au8Data.Length - 1] = (byte)((mlstReg16RepeatWrite[miRepeat16Index].u16Data & 0xff00) >> 8);

                            miRepeat16Index = (mlstReg16RepeatWrite.Count - 1) > miRepeat16Index ?
                                                miRepeat16Index + 1 : -1;

                            u32Address += 2;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return au8Data;
        }

        public static byte[] GetData32Contiguous(ref UInt32 u32OutAddress)
        {
            byte[] au8Data = null;

            if (mboSort32Pending == true)
            {
                Sort32Pending();
                mboSort32Pending = false;
            }

            if (miSequencePending == -1)
            {
                miSequencePending = miSequence;
                int iListIDX = 0;
                au8Data = new byte[4];
                UInt32 u32Address = mlstReg32Write[0].u32Address;

                au8Data[0] = (byte)(mlstReg32Write[0].u32Data & 0xff);
                au8Data[1] = (byte)((mlstReg32Write[0].u32Data & 0xff00) >> 8);
                au8Data[2] = (byte)((mlstReg32Write[0].u32Data & 0xff0000) >> 16);
                au8Data[3] = (byte)((mlstReg32Write[0].u32Data & 0xff000000) >> 24);

                mlstReg32Write.RemoveAt(0);
                u32OutAddress = u32Address;

                while (mlstReg32Write.Count > iListIDX)
                {
                    if ((u32Address + 4) == mlstReg32Write[iListIDX].u32Address)
                    {
                        Array.Resize(ref au8Data, au8Data.Length + 4);
                        au8Data[au8Data.Length - 4] = (byte)(mlstReg32Write[iListIDX].u32Data & 0xff);
                        au8Data[au8Data.Length - 3] = (byte)((mlstReg32Write[iListIDX].u32Data & 0xff00) >> 8);
                        au8Data[au8Data.Length - 2] = (byte)((mlstReg32Write[iListIDX].u32Data & 0xff0000) >> 16);
                        au8Data[au8Data.Length - 1] = (byte)((mlstReg32Write[iListIDX].u32Data & 0xff000000) >> 24);
                        mlstReg32Write.RemoveAt(iListIDX);
                        u32Address += 4;
                    }
                    else
                    {
                        iListIDX++;
                    }
                }
            }

            return au8Data;
        }

        public static bool boClockDataPage()
        {
            bool Written = false;

            if (false == mboLockForChanges)
            {
                if (miEndDirtyRange > miStartDirtyRange)
                {
                    byte[] au8Data = new byte[miEndDirtyRange - miStartDirtyRange];

                    Array.Copy(mau8WorkingPage, miStartDirtyRange, au8Data, 0, au8Data.Length);

                    Program.mAPP_clsUDPComms.mclsUDS.vStartRPC(ConstantData.UDS.ru8SID_WMBA,
                        0x00,
                        mu32WorkingBaseAddress + (UInt32)miStartDirtyRange,
                        au8Data.Length,
                        au8Data);

                    miEndDirtyRange = -1;
                    miStartDirtyRange = -1;

                    Written = true;
                }
            }

            return Written;
        }

        private static void Sort16Pending()
        {
            List<tstReg16Write> mlstReg16WriteTemp;

            mlstReg16WriteTemp = new List<tstReg16Write>();

            while (0 < mlstReg16Write.Count)
            {
                int iListIDX;
                int iLowestListIDX;
                UInt32 u32LowestAddress;

                iLowestListIDX = mlstReg16Write.Count;
                u32LowestAddress = 0xffffffff;

                for (iListIDX = 0; iListIDX < mlstReg16Write.Count; iListIDX++)
                {
                    if (mlstReg16Write[iListIDX].u32Address < u32LowestAddress)
                    {
                        iLowestListIDX = iListIDX;
                        u32LowestAddress = mlstReg16Write[iListIDX].u32Address;
                    }
                }

                mlstReg16WriteTemp.Add(mlstReg16Write[iLowestListIDX]);
                mlstReg16Write.RemoveAt(iLowestListIDX);
            }

            mlstReg16Write = mlstReg16WriteTemp;
        }

        private static void Sort32Pending()
        {
            List<tstReg32Write> mlstReg32WriteTemp;

            mlstReg32WriteTemp = new List<tstReg32Write>();

            while (0 < mlstReg32Write.Count)
            {
                int iListIDX;
                int iLowestListIDX;
                UInt32 u32LowestAddress;

                iLowestListIDX = mlstReg32Write.Count;
                u32LowestAddress = 0xffffffff;

                for (iListIDX = 0; iListIDX < mlstReg32Write.Count; iListIDX++)
                {
                    if (mlstReg32Write[iListIDX].u32Address < u32LowestAddress)
                    {
                        iLowestListIDX = iListIDX;
                        u32LowestAddress = mlstReg32Write[iListIDX].u32Address;
                    }
                }

                mlstReg32WriteTemp.Add(mlstReg32Write[iLowestListIDX]);
                mlstReg32Write.RemoveAt(iLowestListIDX);
            }

            mlstReg32Write = mlstReg32WriteTemp;
        }
    }
}
