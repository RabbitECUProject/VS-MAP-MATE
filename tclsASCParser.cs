/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      ASC Parser                                             */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsASCParser.cs                                       */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace UDP
{
    public class tclsASCParser
    {
        List<tclsUDSFrame> mlstCANMessages;
        List<tclsUDSFrame> mlstUDSFramesReplay;
        int miReplayListIndex;
        int miReplayProgress;
        UInt64 mu64StartTimeuS;
        UInt32 mu32StartTimeFileuS;
        UInt32 mu32EndTimeFileuS;
        bool mboHoldList;

        public tclsASCParser(string szASCPath)
        {
            mlstCANMessages = new List<tclsUDSFrame>();
            mlstUDSFramesReplay = new List<tclsUDSFrame>();
            TextReader ASCFile = null;
            String strLine = null;
            miReplayListIndex = 0;
            int iDLC = 0;
            mboHoldList = false;

            mu64StartTimeuS = (UInt64)(DateTime.Now.Ticks / 10L);
            mu32EndTimeFileuS = 0;
            mu32StartTimeFileuS = 0xffffffff;

            if (File.Exists(szASCPath))
            {
                try
                {
                    ASCFile = new StreamReader(szASCPath);

                    strLine = ASCFile.ReadLine();

                    while (strLine != null)
                    {
                        if (strLine != "")
                        {
                            strLine = strLine.Trim();

                            while (true == strLine.Contains("  "))
                            {
                                strLine = strLine.Replace("  ", " ");
                            }

                            string[] aszLineArray = strLine.Split(' ');

                            if (6 < aszLineArray.Length)
                            {
                                if ((0 == String.Compare("Tx", aszLineArray[3], true))
                                    && (0 == String.Compare("d", aszLineArray[4], true)))
                                {
                                    try
                                    {
                                        iDLC = Convert.ToInt16(aszLineArray[5]);
                                        iDLC = 9 > iDLC ? iDLC : (int)8;
                                    }
                                    catch
                                    {
                                        /* force fail at check enough string array tokens */
                                        iDLC = 1000;
                                    }

                                    if ((6 + iDLC) <= aszLineArray.Length)
                                    {
                                        tclsUDSFrame clsUDSFrame =
                                            new tclsUDSFrame();

                                        try
                                        {
                                            clsUDSFrame.u8ByteCount = (byte)iDLC;

                                            for (int iDataIDX = 0; iDataIDX < iDLC; iDataIDX++)
                                            {
                                                clsUDSFrame.au8Data[iDataIDX] =
                                                    byte.Parse(aszLineArray[6 + iDataIDX], System.Globalization.NumberStyles.HexNumber);
                                            }

                                            Single fTimeStamp = Convert.ToSingle(aszLineArray[0]);
                                            fTimeStamp /= 4;

                                            if (4295 > fTimeStamp)
                                            {
                                                clsUDSFrame.u32TimeStamp =
                                                    (UInt32)(1000000 * fTimeStamp);

                                                if (clsUDSFrame.u32TimeStamp
                                                    > mu32EndTimeFileuS)
                                                {
                                                    mu32EndTimeFileuS = clsUDSFrame.u32TimeStamp;
                                                }

                                                if (clsUDSFrame.u32TimeStamp
                                                    < mu32StartTimeFileuS)
                                                {
                                                    mu32StartTimeFileuS = clsUDSFrame.u32TimeStamp;
                                                }
                                            }

                                            mlstCANMessages.Add(clsUDSFrame);
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                            }
                        }

                        strLine = ASCFile.ReadLine();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (ASCFile != null)
                        ASCFile.Close();
                }
            }
            else
                throw new FileNotFoundException("Unable to locate " + szASCPath);
        }

        public List<tclsUDSFrame> GetReplayMessages(bool boRepeat, ref bool boHold)
        {
            bool boSegmented = false;
            bool boContinue = true;
            boHold = false;

            if ((false == boRepeat) && (0 < mlstCANMessages.Count))
            {
                if (false == mboHoldList)
                {
                    /* Start over we are not continuing from the last call */
                    mlstUDSFramesReplay.Clear();
                }
                else
                {
                    /* Must continue in segmented mode */
                    boSegmented = true;
                }

                mboHoldList = false;

                UInt64 u64TimeNow = (UInt64)(DateTime.Now.Ticks / 10L);

                while ((false == mboHoldList) && (true == boContinue))
                {
                    if (mlstCANMessages.Count > miReplayListIndex)
                    {
                        if (0x00 == (0xf0 & mlstCANMessages[miReplayListIndex].au8Data[0]))
                        {
                            /* Just send the one UDT */
                            boSegmented = false;
                        }
                        if (0x10 == (0xf0 & mlstCANMessages[miReplayListIndex].au8Data[0]))
                        {
                            if (false == boSegmented)
                            {
                                /* Start of first SDT */
                                boSegmented = true;
                            }
                            else
                            {
                                /* Start of subsequent SDT */
                                boContinue = false;
                            }
                        }

                        if (true == boContinue)
                        {
                            if (u64TimeNow >=
                            ((UInt64)mlstCANMessages[miReplayListIndex].u32TimeStamp + (UInt64)mu64StartTimeuS))
                            {
                                mlstUDSFramesReplay.Add(mlstCANMessages[miReplayListIndex]);
                                miReplayListIndex = miReplayListIndex + 1;

                                if (mlstCANMessages.Count == miReplayListIndex)
                                /* Are we done? If so stop the loop */
                                {
                                    boContinue = false;
                                }

                                /* Just send one UDT */
                                if (false == boSegmented)
                                {
                                    boContinue = false;
                                }
                            }
                            else
                            {
                                if (true == boSegmented)
                                {
                                    mboHoldList = true;
                                }
                                boContinue = false;
                            }
                        }
                    }
                }
            }

            if ((true == mboHoldList) && (0 < mlstCANMessages.Count))
            {
                boHold = mboHoldList;
            }

            return mlstUDSFramesReplay;
        }

        public int GetReplayProgress()
        {
            UInt64 u64TimeNow = (UInt64)(DateTime.Now.Ticks / 10L);
            Single progress = 0;

            if (u64TimeNow >= mu64StartTimeuS)
            {
                progress = (Single)(u64TimeNow - mu64StartTimeuS) /
                    (Single)((UInt64)mu32EndTimeFileuS - mu32StartTimeFileuS);
                miReplayProgress = progress > 0 ? (int)(100 * progress) : 0;
            }
            else
            {
                miReplayProgress = 0;
            }

            if (mlstCANMessages.Count > miReplayListIndex)
            {
                miReplayProgress = miReplayProgress > 99 ? 99 : miReplayProgress;
            }

            return miReplayProgress;
        }
    }
}
