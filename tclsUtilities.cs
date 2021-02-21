/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Utilities                                              */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsUtilities.cs                                       */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UDP
{
    public static class tclsUtilities
    {
        public static UInt16 u16EndianConvert(UInt16 u16Data)
        {
            return (UInt16)((u16Data & 0xFFu) << 8 | (u16Data & 0xFF00u) >> 8);
        }
        public static UInt32 u32EndianConvert(UInt32 u32Data)
        {
            return (UInt32)((u32Data & 0xFFu) << 8 | (u32Data & 0xFF00u) >> 8 
                          | (u32Data & 0xFF0000u) << 8 | (u32Data & 0xFF000000u) >> 8);
        }

        public static T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
        }
    }
}
