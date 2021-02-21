/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Bus Multiplex                                          */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsBMXMsg.cs                                          */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UDP
{
    public class tclsBMXMsg
    {
        public DateTime mclsDateTime;
        public ConstantData.BMX.tenBMXChannels menBMXChannel;

        public tclsBMXMsg()
        {
            mclsDateTime = new DateTime();
        }

        public string szGetHeader()
        {
            return ("Base Class Header");
        }
    }

    class tclsUARTMsg : tclsBMXMsg
    {
        new public string szGetHeader()
        {
            return ("UART Header");
        }
    }

    public class tclsCANMsg : tclsBMXMsg
    {
        public UInt32 mu32CS;
        public UInt32 mu32ID;
        public UInt32 mu32DH;
        public UInt32 mu32DL;
        public UInt32 mu32TicksDelta;

        new public string szGetHeader()
        {
            int i32ID;

            if (ConstantData.BMX.ru32MSG_HEADER_CAN_IDE_MASK == (mu32CS & ConstantData.BMX.ru32MSG_HEADER_CAN_IDE_MASK))
            {
                i32ID = (int)(mu32ID >> 3);
                return (i32ID.ToString("X8") + "29-bit");
            }
            else
            {
                i32ID = (int)((mu32ID & ConstantData.BMX.ru32MSG_CANID_STANDARD_MASK)
                    >> (int)ConstantData.BMX.ru32MSG_CANID_STANDARD_SHIFT);
                return ("ID = 0x" + i32ID.ToString("X3") + " 11-bit");
            }
        }
    }
}
