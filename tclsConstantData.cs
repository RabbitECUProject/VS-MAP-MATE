/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Data Constants                                         */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsConstantData.cs                                    */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UDP
{
    public static class ConstantData
    {
        public class APPCONFIG
        {
            public const bool boLittleEndianPageFormat = true;
            public const string szAppName = "MAP-MATE";
        }

        public static class FILESETTINGS
        {
            public const UInt16 u16ASCFileTimeStampPostDecimalChars = 6;
            public const UInt16 u16ASCFileTimeStampPreDecimalChars = 6;
        }

        public static class GENERICDATA
        {
            public const UInt16 u16ENGINE_SPEED_RAW = 0;
            public const UInt16 u16THROTTLE_ANGLE = 1;
            public const UInt16 u16MAP_KPA = 2;
            public const UInt16 u16COOLANT_TEMPERATURE = 3;
            public const UInt16 u16AIR_TEMPERATURE = 4;
        }

        public static class BUFFERSIZES
        {
            public const UInt16 u16BMOU_BUFF_TXPAYLOAD_SIZE = 1000;
            public const UInt16 u16BMOU_BUFF_RXPAYLOAD_SIZE = 1000;
            public const UInt16 u16UDSOU_BUFF_TXPAYLOAD_SIZE = 600;
            public const UInt16 u16UDSOU_BUFF_RXPAYLOAD_SIZE = 600;
            public const UInt16 u16BMOU_CAN_MSG_SIZE = 16;
            public const UInt16 u16UDS_MEASURE_RATE_COUNT = 60;
        }

        public static class APPDATA
        {
            public const string rszAPP_WLAN_DEVICE = "Network adapter 'Intel(R) PRO/Wireless 3945ABG Network Connection (Microsoft's Packet Scheduler) ' on local host";
            public const string rszAPP_LAN_DEVICE = "Network adapter 'Intel(R) PRO/1000 PL Network Connection (Microsoft's Packet Scheduler) ' on local host";
            public const int ru32LineSaveCount = 1000;
            public const long ru32ComsTickMin = 5000;
            public const long ru32ComsTickMax = 100000;
        }

        public static class PROTOCOLDATA
        {
            public const UInt16 u16BMXDataLength = 1000;
            public const UInt16 u16ETHHeaderLength = 14;
        }

        public static class LANSETTINGS
        {
            public const string rszAPP_LOCAL_LAN_IP = "192.168.10.100";
            public const string rszAPP_REMOTE_LAN_IP = "192.168.10.0";
            public const ushort ru16APP_SOURCE_PORT_BMUX = 0x9c01;
            public const ushort ru16APP_DEST_PORT_BMUX = 0x9c11;
            public const ushort ru16APP_SOURCE_PORT_KWOU = 0x9c02;
            public const ushort ru16APP_DEST_PORT_KWOU = 0x9c12;
            public const ushort ru16APP_SOURCE_PORT_UDSOUDP = 0x9c04;
            public const ushort ru16APP_DEST_PORT_UDSOUDP = 0x9c14;
        }

        public static class WLANSETTINGS
        {
            public const string rszAPP_WIFI_MAC = "00-0A-E6-BC-EE-D8";
            public const string rszAPP_LOCAL_WLAN_IP = "10.0.0.100";
            public const string rszAPP_REMOTE_WLAN_IP = "10.0.0.0";
            public const byte ru8WLANLengthOffset = 16;
            public const byte ru8WLANSeqOffset = 18;
            public const byte ru8WLANDataOffset = 42;
            public const byte ru8WLANDreadTimeoutMs = 255;
        }

        public static class UDS
        {
            public const byte ru8SID_DSC = 0x10;
            public const byte ru8SID_ER = 0x11;

            public const byte ru8SID_RDTCREC = 0x12;
            public const byte ru8SID_CLDTCINF = 0x14;
            public const byte ru8SID_RDTCSTAT = 0x17;
            public const byte ru8SID_RSTATBYDTC = 0x18;
            public const byte ru8SID_RECUID = 0x1a;
            public const byte ru8SID_STPDIAG = 0x20;

            public const byte ru8SID_RDBI = 0x22;
            public const byte ru8SID_RMBA = 0x23;
            public const byte ru8SID_RSDBI = 0x24;
            public const byte ru8SID_SA = 0x27;
            public const byte ru8SID_CC = 0x28;
            public const byte ru8SID_RDBPI = 0x2a;
            public const byte ru8SID_DDDI = 0x2c;
            public const byte ru8SID_WDBI = 0x2e;
            public const byte ru8SID_IOC = 0x2f;
            public const byte ru8SID_RC = 0x31;
            public const byte ru8SID_RD = 0x34;
            public const byte ru8SID_RU = 0x35;
            public const byte ru8SID_TD = 0x36;
            public const byte ru8SID_RTE = 0x37;
            public const byte ru8SID_WMBA = 0x3d;
            public const byte ru8SID_TP = 0x3e;

            public const byte ru8RSP_GR	= 0x10;
            public const byte ru8RSP_SNS = 0x11;
            public const byte ru8RSP_SFNS = 0x12;
            public const byte ru8RSP_IMLOIF = 0x13;
            public const byte ru8RSP_RTL = 0x14;
            public const byte ru8RSP_BRR = 0x21;
            public const byte ru8RSP_CNC = 0x22;
            public const byte ru8RSP_RSE = 0x24;
            public const byte ru8RSP_NRFSC = 0x25;
            public const byte ru8RSP_FPEORA = 0x26;
            public const byte ru8RSP_ROOR = 0x31;
            public const byte ru8RSP_SAD = 0x33;
            public const byte ru8RSP_IK = 0x35;
            public const byte ru8RSP_ENOA = 0x36;
            public const byte ru8RSP_RTDNE = 0x37;
            public const byte ru8RSP_TDS = 0x71;
            public const byte ru8RSP_GPF = 0x72;
            public const byte ru8RSP_WBSC = 0x73;
            public const byte ru8RSP_RCRRP = 0x78;
            public const byte ru8RSP_SFNSIAS = 0x7E;
            public const byte ru8RSP_SNSIAS = 0x7F;
            public const byte ru8RSP_NSUPINSESS = 0x80;
            public const byte ru8RSP_SUPPRESS = 0xFD;
            public const byte ru8RSP_FLOW = 0xFE;
            public const byte ru8RSP_OK = 0xFF;

            public const byte ru8RCID_HighByte = 0x90;
            public const byte ru8RCID_RunDL = 0x00;
            public const byte ru8RCID_PartitionNVM = 0x01;
            public const byte ru8RCID_WorkNVMFreeze = 0x02;
            public const byte ru8RCID_WorkNVMClear = 0x03;

            public const byte ru8SID_CNTRDTC = 0x85;

            public const byte ru8SID_INSTRSTAT = 0xa2;

            public const byte ru8SID_INSTRBLID = 0xa5;

            public const byte ru8SID_RBDYNLID = 0xaa;

            public const byte ru8SID_INSTRBCID = 0xb2;


            public const byte ru8FRAME_PAD_BYTE = 0x00;
            public const byte ru8SFID_DS = 0x01;
            public const byte ru8SFID_PRGS = 0x02;
            public const byte ru8SFID_EXTDS = 0x03;
            public const byte ru8SFID_TSTPRSNT_RSPYES = 0x01;
            public const byte ru8SFID_TSTPRSNT_RSPNO = 0x02;
            public const byte ru8SFID_DDDI_DMBA = 0x02;

            public const byte ru8RMBA_HEADER_LENGTH = 0x02;
            public const byte ru8RMBA_MA_OFFSET = 0x02;
            public const byte ru8RMBA_MA_LENGTH = 0x04;
            public const byte ru8RMBA_MS_OFFSET = 0x06;
            public const byte ru8RMBA_MS_LENGTH = 0x02;
            public const byte ru8RMBA_ALFID = 0x24;

            public const byte ru8WMBA_HEADER_LENGTH = 0x02;
            public const byte ru8WMBA_MA_OFFSET = 0x02;
            public const byte ru8WMBA_MA_LENGTH = 0x04;
            public const byte ru8WMBA_MS_OFFSET = 0x06;
            public const byte ru8WMBA_MS_LENGTH = 0x02;
            public const byte ru8WMBA_ALFID = 0x24;

            public const byte ru8DDDI_HEADER_LENGTH = 0x05;
            public const byte ru8DDDI_MA_LENGTH = 0x04;
            public const byte ru8DDDI_MS_LENGTH = 0x01;
            public const byte ru8DDDI_ALFID = 0x14;

            public const byte ru8UDS_MEAS_LIST_COUNT = 60;
            public const byte ru8UDS_MEAS_DDDI_MAX_ELEMENTS = 60;
        }

        public static class GUI
        {
            public const byte ru8GUI_WINDOWS_MAX = 60;
            public const byte ru8GUI_CAL_GROUPS_MAX = 200;
            public const byte ru8GUI_AXIS_GROUPS_MAX = 100;
        }

        public static class ISO15765
        {
            public const byte ru8PIPED_RESP_COUNT = 50;
            public const uint ru32SEG_RESP_BYTE_COUNT = 1026u;
            public const int rs32SEG_TRANSFER_BLOCK_SIZE = 64;
        }

        public static class BMX
        {
            public const uint ru32BMX_CHANNEL_COUNT = 16u;
            public const uint ru32BMX_HEADER_EXTRA_BYTE_COUNT = 4u;
            public const uint ru32FRAME_DATA_OFFSET =
                    4 * ru32BMX_CHANNEL_COUNT + ru32BMX_HEADER_EXTRA_BYTE_COUNT;
            public const uint ru32MSG_CAN_LENGTH = 16u;
            //CHANNEL
            public const uint ru32MSG_HEADER_CAN_CH_MASK = 0xf0000000;
            public const uint ru32MSG_HEADER_CAN_CH_SHIFT = 28u;
            //ISCAN
            public const uint ru32MSG_HEADER_CAN_BIT_MASK = 0x00800000;
            //DLC
            public const uint ru32MSG_HEADER_CAN_DLC_MASK = 0x000000f0;
            public const uint ru32MSG_HEADER_CAN_DLC_SHIFT = 16u;
            //IDE
            public const uint ru32MSG_HEADER_CAN_IDE_MASK = 0x00200000;
            public const uint ru32MSG_CANID_STANDARD_MASK = 0x1ffc0000;
            public const uint ru32MSG_CANID_STANDARD_SHIFT = 18u;

            public const Single rfBMX_MS_PER_CPU_GLOBAL_TICK = 0.002f;

            public enum tenBMXChannels { UART0, UART1, UART2, UART3, UART4, UART5, MSCAN0, MSCAN1, BMXChannelCount };
        }

        public static class CRC16Data
        {
            public static readonly UInt16[] rau16CRC16 =
                {0x0000, 0x1189, 0x2312, 0x329B, 0x4624, 0x57AD, 0x6536, 0x74BF,
                0x8C48, 0x9DC1, 0xAF5A, 0xBED3, 0xCA6C, 0xDBE5, 0xE97E, 0xF8F7,
                0x0919, 0x1890, 0x2A0B, 0x3B82, 0x4F3D, 0x5EB4, 0x6C2F, 0x7DA6,
                0x8551, 0x94D8, 0xA643, 0xB7CA, 0xC375, 0xD2FC, 0xE067, 0xF1EE,
                0x1232, 0x03BB, 0x3120, 0x20A9, 0x5416, 0x459F, 0x7704, 0x668D,
                0x9E7A, 0x8FF3, 0xBD68, 0xACE1, 0xD85E, 0xC9D7, 0xFB4C, 0xEAC5,
                0x1B2B, 0x0AA2, 0x3839, 0x29B0, 0x5D0F, 0x4C86, 0x7E1D, 0x6F94,
                0x9763, 0x86EA, 0xB471, 0xA5F8, 0xD147, 0xC0CE, 0xF255, 0xE3DC,
                0x2464, 0x35ED, 0x0776, 0x16FF, 0x6240, 0x73C9, 0x4152, 0x50DB,
                0xA82C, 0xB9A5, 0x8B3E, 0x9AB7, 0xEE08, 0xFF81, 0xCD1A, 0xDC93,
                0x2D7D, 0x3CF4, 0x0E6F, 0x1FE6, 0x6B59, 0x7AD0, 0x484B, 0x59C2,
                0xA135, 0xB0BC, 0x8227, 0x93AE, 0xE711, 0xF698, 0xC403, 0xD58A,
                0x3656, 0x27DF, 0x1544, 0x04CD, 0x7072, 0x61FB, 0x5360, 0x42E9,
                0xBA1E, 0xAB97, 0x990C, 0x8885, 0xFC3A, 0xEDB3, 0xDF28, 0xCEA1,
                0x3F4F, 0x2EC6, 0x1C5D, 0x0DD4, 0x796B, 0x68E2, 0x5A79, 0x4BF0,
                0xB307, 0xA28E, 0x9015, 0x819C, 0xF523, 0xE4AA, 0xD631, 0xC7B8,
                0x48C8, 0x5941, 0x6BDA, 0x7A53, 0x0EEC, 0x1F65, 0x2DFE, 0x3C77,
                0xC480, 0xD509, 0xE792, 0xF61B, 0x82A4, 0x932D, 0xA1B6, 0xB03F,
                0x41D1, 0x5058, 0x62C3, 0x734A, 0x07F5, 0x167C, 0x24E7, 0x356E,
                0xCD99, 0xDC10, 0xEE8B, 0xFF02, 0x8BBD, 0x9A34, 0xA8AF, 0xB926,
                0x5AFA, 0x4B73, 0x79E8, 0x6861, 0x1CDE, 0x0D57, 0x3FCC, 0x2E45,
                0xD6B2, 0xC73B, 0xF5A0, 0xE429, 0x9096, 0x811F, 0xB384, 0xA20D,
                0x53E3, 0x426A, 0x70F1, 0x6178, 0x15C7, 0x044E, 0x36D5, 0x275C,
                0xDFAB, 0xCE22, 0xFCB9, 0xED30, 0x998F, 0x8806, 0xBA9D, 0xAB14,
                0x6CAC, 0x7D25, 0x4FBE, 0x5E37, 0x2A88, 0x3B01, 0x099A, 0x1813,
                0xE0E4, 0xF16D, 0xC3F6, 0xD27F, 0xA6C0, 0xB749, 0x85D2, 0x945B,
                0x65B5, 0x743C, 0x46A7, 0x572E, 0x2391, 0x3218, 0x0083, 0x110A,
                0xE9FD, 0xF874, 0xCAEF, 0xDB66, 0xAFD9, 0xBE50, 0x8CCB, 0x9D42,
                0x7E9E, 0x6F17, 0x5D8C, 0x4C05, 0x38BA, 0x2933, 0x1BA8, 0x0A21,
                0xF2D6, 0xE35F, 0xD1C4, 0xC04D, 0xB4F2, 0xA57B, 0x97E0, 0x8669,
                0x7787, 0x660E, 0x5495, 0x451C, 0x31A3, 0x202A, 0x12B1, 0x0338,
                0xFBCF, 0xEA46, 0xD8DD, 0xC954, 0xBDEB, 0xAC62, 0x9EF9, 0x8F70};
        }
    }
}
