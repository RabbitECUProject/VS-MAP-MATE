/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Communications Channel                                 */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsCommsChannel.cs                                    */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;

namespace UDP
{
    public enum tenChannelMode
    {
        enChannelModeUploading,
        enChannelModeDownloading,
        enChannelModeWorkingNVMFreeze,
        enChannelModeNVMClear,
        enChannelModeReplayScript,
        enChannelModeNone
    }

    public class tclsCommsChannel
    {
        public tclsUDS mclsUDS;
        protected UInt16 mu16PacketTXID;
        protected UInt16 mu16PacketRXID;
        protected UInt16 mu16ResponseTimeoutCounter;
        protected byte[] mau8ChannelTXPayload;
        protected static byte[] mau8InPacketBuffer;
        protected bool mboChannelActive;
        protected string mszConnectionException;
        protected tstTransferPageCB mstTransferPageCB;
        protected tstPollingCB mstPollingCB;

        public tclsCommsChannel()
        {
            mboChannelActive = false;
            mu16PacketTXID = 0;
            mu16PacketRXID = 0;
            mu16ResponseTimeoutCounter = 0;
            mszConnectionException = "";
        }
    }
}


