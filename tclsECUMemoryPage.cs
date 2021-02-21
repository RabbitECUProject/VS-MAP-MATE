/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      ECU Memory                                             */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsECUMemoryPage.cs                                   */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UDP
{
    class tclsECUMemoryPage
    {
        byte[] mau8WorkingData;
        UInt32 mu32WorkingDataBase;

        public tclsECUMemoryPage()
        {
            mau8WorkingData = new byte[65536];
            mu32WorkingDataBase = 0xffffffff;
        }
    }
}
