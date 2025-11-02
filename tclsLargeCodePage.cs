using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP
{
    static class tclsLargeCodePage
    {
        static byte[] mau8CodePage;
        static string mszFWFileName;

        static tclsLargeCodePage()
        {
            mau8CodePage = new byte[65536 * 3];

            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHost Calibration.INI");

            try
            {
                mszFWFileName = mclsIniParser.GetSetting("FirmwareUpdate", "FWU");
                mszFWFileName = AppDomain.CurrentDomain.BaseDirectory + "Firmwares\\" + mszFWFileName;
            }
            catch
            {
                mszFWFileName = "Firmware Update not found";
            }

            LoadFile();
        }

        static void LoadFile()
        {
            TextReader FWFile = null;
            String strLine = null;
            UInt32 u32Address = 0;
            UInt16 u16LineSize;
            byte[] FWdata = new byte[16];
            UInt16 u16DataIDX;
            string szSize;
            string szData;
            string szAddress;
            string szHexData;
            UInt32 u32RolloverAddress = 0;
            Int32 i32OldAddress = -1;


            if (File.Exists(mszFWFileName))
            {
                try
                {
                    FWFile = new StreamReader(mszFWFileName);

                    strLine = FWFile.ReadLine();

                    while (strLine != null)
                    {
                        if (strLine != "")
                        {
                            szSize = strLine.Substring(1, 2);
                            szAddress = strLine.Substring(3, 4);
                            szData = strLine.Substring(9);

                            u32Address = UInt32.Parse(szAddress, System.Globalization.NumberStyles.HexNumber);
                            u16LineSize = UInt16.Parse(szSize, System.Globalization.NumberStyles.HexNumber);

                            u16DataIDX = 0;

                            if (16 == u16LineSize)
                            {
                                if (16 != FWdata.Length)
                                {
                                    FWdata = new byte[16];
                                }

                                while (u16LineSize-- > 0)
                                {
                                    szHexData = szData.Substring(u16DataIDX * 2, 2);
                                    FWdata[u16DataIDX] = byte.Parse(szHexData, System.Globalization.NumberStyles.HexNumber);
                                    u16DataIDX++;
                                }
                            }
                            else
                            {
                                FWdata = new byte[u16LineSize];

                                while (u16LineSize-- > 0)
                                {
                                    szHexData = szData.Substring(u16DataIDX * 2, 2);
                                    FWdata[u16DataIDX] = byte.Parse(szHexData, System.Globalization.NumberStyles.HexNumber);
                                    u16DataIDX++;
                                }
                            }

                            if (i32OldAddress > u32Address)
                            {
                                u32RolloverAddress += 0x10000;
                            }

                            i32OldAddress = (Int32)u32Address;

                            vSetWorkingCode(u32RolloverAddress + u32Address, FWdata);
                        }

                        strLine = FWFile.ReadLine();
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("An error occurred opening the Firmware update resource!");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("The Firmware update resource file does not exist!");
            }
        }

        static public void SetCodePageCRC16()
        {
            UInt16 u16CRC = GetCodePageCRC16();

            mau8CodePage[0x30000 - 1] = (byte)(u16CRC >> 8);
            mau8CodePage[0x30000 - 2] = (byte)(u16CRC & 0xff);
        }

        static public UInt16 GetCodePageCRC16()
        {
            UInt16 CRC16 = 0xffff;
            UInt32 iConfigSize = 0x30000;
            UInt16 iByteOffset;

            /* Subtract bytes used for CRC and correct by 4 */
            iConfigSize -= 2;

            iByteOffset = 0;

            while (iConfigSize-- > 0)
            {
                UInt16 CRC16ShiftL = (UInt16)((CRC16 & 0x00ff) << 8);
                UInt16 CRC16ShiftR = (UInt16)((CRC16 & 0xff00) >> 8);
                UInt16 CRC16Index = (UInt16)(CRC16ShiftR ^ mau8CodePage[iByteOffset]);

                CRC16 = (UInt16)(CRC16ShiftL ^ ConstantData.CRC16Data.rau16CRC16[CRC16Index]);
                iByteOffset++;
            }

            return CRC16;
        }

        static public void au8GetWorkingCode(UInt32 u32Address, ref byte[] au8Data)
        {
            Array.Copy(mau8CodePage, u32Address, au8Data, 0, au8Data.Length);
        }

        static public void vSetWorkingCode(UInt32 u32Address, byte[] au8Data)
        {
            Array.Copy(au8Data, 0, mau8CodePage, u32Address, au8Data.Length);
        }
    }
}

