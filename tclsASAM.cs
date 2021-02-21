/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      ASAM                                                   */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsASAM.cs                                            */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace UDP
{
    public enum tenAxisType
    {
        enAT_FIX_AXIS,
        enAT_STD_AXIS,
        enAT_COM_AXIS,
        enAT_Invalid
    }

    public enum tenParamType
    {
        enPT_VALUE = 0,
        enPT_ASCII = 1,
        enPT_VAL_BLK = 2,
        enPT_CURVE = 3,
        enPT_MAP = 4,
        enPT_CUBOID = 5,
        enPT_CUBE_4 = 6,
        enPT_CUBE_5 = 7,
        enPT_BLOB = 8,
        enPT_Invalid = 9
    };

    public enum tenRecLayout
    {
        enRL_VALU8 = 0,
        enRL_VALS8 = 1,
        enRL_VALU16 = 2,
        enRL_VALS16 = 3,
        enRL_VALU32 = 4,
        enRL_VALS32 = 5,
        enRL_Invalid = 6
    };

    public enum tenCM_Type
    {
        enIDENTICAL = 0,
        enLINEAR = 1,
        enRAT_FUNC = 2,
        enTAB_INTP = 3,
        enTAB_NOINTP = 4,
        enTAB_VERB = 5,
        enFORM = 6
    };

    public enum tenCM_CoeffType
    {
        enCOEFFS = 0,
        enCOEFFS_LINEAR = 1
    };

    public enum tenParseMode
    {
        enPM_MEASUREMENT_PARSING,
        enPM_CHARACTERISTIC_PARSING,
        enPM_COMPU_METHOD_PARSING,
        enPM_AXIS_DESCRIPTION_PARSING,
        enPM_AXIS_POINTS_PARSING,
        enPM_COMPU_TAB_RANGE_PARSING,
        enPM_MEASUREMENT_FOUND,
        enPM_CHARACTERISTIC_FOUND,
        enPM_COMPU_METHOD_FOUND,
        enPM_AXIS_DESCRIPTION_FOUND,
        enPM_AXIS_POINTS_FOUND,
        enPM_COMPU_TAB_RANGE_FOUND,
        enPM_BLOB_PARSING,
        enPM_BLOB_FOUND,
        enPM_Invalid
    };

    public enum tenAxis
    {
        enAX_X,
        enAX_Y
    };

    public static class tclsASAM
    {
        static List<tstCharacteristic> mlstCharacteristicList;
        static List<tstMeasurement> mlstAvailableMeasList;
        static List<tstMeasurement>[] malstActiveMeasLists;
        static List<tstVarBits> mlstCompuMethodList;
        static List<tstAxisDescr> mlstAxisDescrList;
        static List<tstVarVerb> mlstVarVerbList;
        static List<tstAxisPts> mlstAxisPtsList;
        static List<tstBlob> mlstBlobList;
        public static IList<tstCharacteristic> milstCharacteristicList;
        public static IList<tstMeasurement> milstAvailableMeasList;
        public static IList<tstMeasurement>[] mailstActiveMeasLists;
        public static IList<tstVarBits> milstCompuMethodList;
        public static IList<tstVarVerb> milstVarVerbList;
        public static IList<tstAxisDescr> milstAxisDescrList;
        public static IList<tstAxisPts> milstAxisPtsList;
        public static IList<tstBlob> milstBlobList;
        static Queue<UInt16> mqMeasListQueue;
        static Array[] mau8DDDI;
        static UInt16[] maiMeasListSID;
        static int[] maiMeasurementListTimers;
        static int[] maiMeasurementListTimerLimits;
        static UInt32 mu32CharMinAddress;
        static UInt32 mu32CharMaxAddress;
        static bool[] maboDDDIResetPending;
        static UInt16 mu16CRC;
        static byte[] mau8CRCData;
        static UInt16 mu16CRCDataArrayIDX;
        static tclsASAM()
        {
            int iListIDX;
            string szA2LPath;

            mu16CRC = 0xffff;
            mau8CRCData = new byte[256];
            mu16CRCDataArrayIDX = 0;

            tclsIniParser mclsIniParser = new tclsIniParser(AppDomain.CurrentDomain.BaseDirectory + "Config\\MDAC ECUHost Calibration.INI");

            try
            {
                szA2LPath = AppDomain.CurrentDomain.BaseDirectory + "Database\\A2L Databases\\" + mclsIniParser.GetSetting("Databases", "SelectedA2L");
            }
            catch
            {
                szA2LPath = "Unknown";
            }

            mlstCharacteristicList = new List<tstCharacteristic>();
            mlstAvailableMeasList = new List<tstMeasurement>();
            milstAvailableMeasList = mlstAvailableMeasList.AsReadOnly();
            malstActiveMeasLists = new List<tstMeasurement>[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT];
            mailstActiveMeasLists = new IList<tstMeasurement>[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT];
            maboDDDIResetPending = new bool[ConstantData.BUFFERSIZES.u16UDS_MEASURE_RATE_COUNT];
            mu32CharMaxAddress = 0;
            mu32CharMinAddress = 0xffffffff;

            for (iListIDX = 0; iListIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT; iListIDX++ )
            {
                malstActiveMeasLists[iListIDX] = new List<tstMeasurement>();
                mailstActiveMeasLists[iListIDX] = malstActiveMeasLists[iListIDX].AsReadOnly();
                maboDDDIResetPending[iListIDX] = true;
            }            
            
            mlstCompuMethodList = new List<tstVarBits>();
            mlstAxisDescrList = new List<tstAxisDescr>();
            mlstAxisPtsList = new List<tstAxisPts>();
            mlstVarVerbList = new List<tstVarVerb>();
            mlstBlobList = new List<tstBlob>();
            mqMeasListQueue = new Queue<UInt16>();
            maiMeasurementListTimers = new int[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT];
            maiMeasurementListTimerLimits = new int[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT];
            maiMeasListSID = new UInt16[ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT];
            mau8DDDI = new Array[ConstantData.BUFFERSIZES.u16UDS_MEASURE_RATE_COUNT];

            vOpenAndParse(szA2LPath);
        }

        static void vOpenAndParse(String szFileName)
        {
            int[] aiParseLineIndex = { 0, 0, 0 };
            int iParseNestLevel = 0;
            bool boParseError = false;
            int[] aiParamTypeByteCounts = { 1, 1, 2, 2, 4, 4 };
            bool[] boParamTypeIsSigned = { false, true, false, true, false, true };
            String[,] aaszInputStrings = new String[5,100];
            tenParseMode[] aenParseMode = { tenParseMode.enPM_Invalid, tenParseMode.enPM_Invalid, tenParseMode.enPM_Invalid };
            String[] aszInputSubStrings = new String[100];
            String[] aszParamTypeStrings = {"VALUE", "ASCII", "VAL_BLK", "CURVE", "MAP", "CUBOID", "CUBE_4", "CUBE_5", "BLOB"};
            String[] aszRecLayoutString = { "RL_VALU8", "RL_VALS8", "RL_VALU16", "RL_VALS16", "RL_VALU32", "RL_VALS32" };
            String[] aszCMTypeStrings = { "IDENTICAL", "LINEAR", "RAT_FUNC", "TAB_INTP", "TAB_NOINTP", "TAB_VERB", "FORM" };
            String[] aszCMCoeffStrings = { "COEFFS", "COEFFS_LINEAR" };
            String[] aszAxisTypeStrings = { "FIX_AXIS", "STD_AXIS", "COM_AXIS" };

            // Gets a NumberFormatInfo associated with the en-US culture.
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;


            using (StreamReader clsSR = new StreamReader(szFileName))
            {
                while ((false == boParseError) && (clsSR.Peek() >= 0))
                {
                    String szInputLine = clsSR.ReadLine();

                    if (0 < szInputLine.Length)
                    {
                        while (0 < szInputLine.IndexOf("  "))
                        {
                            szInputLine = szInputLine.Replace("  ", " ");
                        }

                        if ((tenParseMode.enPM_Invalid != aenParseMode[iParseNestLevel]) && (0 == aiParseLineIndex[iParseNestLevel]))
                        {
                            szInputLine = szInputLine.Replace("\"", " ");                            
                            szInputLine = szInputLine.Trim();
                        }
                        else
                        {
                            //szInputLine = szInputLine.ToLower();
                            szInputLine = szInputLine.Trim();
                        }

                        if (0 == szInputLine.IndexOf("/begin measurement", StringComparison.InvariantCultureIgnoreCase))
                        {
                            vStartParseNewRecord(tenParseMode.enPM_MEASUREMENT_PARSING, ref aenParseMode, ref iParseNestLevel);
                            vClearStringArray(ref aaszInputStrings, iParseNestLevel);
                            aaszInputStrings[iParseNestLevel, 0] = szInputLine.Substring(18);
                            aaszInputStrings[iParseNestLevel, 0] = aaszInputStrings[iParseNestLevel, 0].Trim();
                            aiParseLineIndex[iParseNestLevel] = 1;
                        }
                        else if (0 == szInputLine.IndexOf("/begin characteristic", StringComparison.InvariantCultureIgnoreCase))
                        {
                            vStartParseNewRecord(tenParseMode.enPM_CHARACTERISTIC_PARSING, ref aenParseMode, ref iParseNestLevel);
                            vClearStringArray(ref aaszInputStrings, iParseNestLevel);
                            aaszInputStrings[iParseNestLevel, 0] = szInputLine.Substring(21);
                            aaszInputStrings[iParseNestLevel, 0] = aaszInputStrings[iParseNestLevel, 0].Trim();
                            aiParseLineIndex[iParseNestLevel] = 1;
                        }
                        else if (0 == szInputLine.IndexOf("/begin compu_method", StringComparison.InvariantCultureIgnoreCase))
                        {
                            vStartParseNewRecord(tenParseMode.enPM_COMPU_METHOD_PARSING, ref aenParseMode, ref iParseNestLevel);
                            vClearStringArray(ref aaszInputStrings, iParseNestLevel);
                            aaszInputStrings[iParseNestLevel, 0] = szInputLine.Substring(19);
                            aaszInputStrings[iParseNestLevel, 0] = aaszInputStrings[iParseNestLevel, 0].Trim();
                            aiParseLineIndex[iParseNestLevel] = 1;
                        }
                        else if (0 == szInputLine.IndexOf("/begin axis_descr", StringComparison.InvariantCultureIgnoreCase))
                        {
                            vStartParseNewRecord(tenParseMode.enPM_AXIS_DESCRIPTION_PARSING, ref aenParseMode, ref iParseNestLevel);
                            vClearStringArray(ref aaszInputStrings, iParseNestLevel);
                            aiParseLineIndex[iParseNestLevel] = 1;
                        }
                        else if (0 == szInputLine.IndexOf("/begin axis_pts", StringComparison.InvariantCultureIgnoreCase))
                        {
                            vStartParseNewRecord(tenParseMode.enPM_AXIS_POINTS_PARSING, ref aenParseMode, ref iParseNestLevel);
                            vClearStringArray(ref aaszInputStrings, iParseNestLevel);
                            aaszInputStrings[iParseNestLevel, 0] = szInputLine.Substring(15);
                            aaszInputStrings[iParseNestLevel, 0] = aaszInputStrings[iParseNestLevel, 0].Trim();
                            aiParseLineIndex[iParseNestLevel] = 1;
                        }
                        else if (0 == szInputLine.IndexOf("/begin blob", StringComparison.InvariantCultureIgnoreCase))
                        {
                            vStartParseNewRecord(tenParseMode.enPM_BLOB_PARSING, ref aenParseMode, ref iParseNestLevel);
                            vClearStringArray(ref aaszInputStrings, iParseNestLevel);
                            aaszInputStrings[iParseNestLevel, 0] = szInputLine.Substring(12);
                            aaszInputStrings[iParseNestLevel, 0] = aaszInputStrings[iParseNestLevel, 0].Trim();
                            aiParseLineIndex[iParseNestLevel] = 1;
                        }
                        else if (0 == szInputLine.IndexOf("/begin compu_tab_range", StringComparison.InvariantCultureIgnoreCase))
                        {
                            vStartParseNewRecord(tenParseMode.enPM_COMPU_TAB_RANGE_PARSING, ref aenParseMode, ref iParseNestLevel);
                            vClearStringArray(ref aaszInputStrings, iParseNestLevel);
                            aaszInputStrings[iParseNestLevel, 0] = szInputLine.Substring(23);
                            aaszInputStrings[iParseNestLevel, 0] = aaszInputStrings[iParseNestLevel, 0].Trim();
                            aiParseLineIndex[iParseNestLevel] = 1;
                        }
                        else if (0 == szInputLine.IndexOf("/end measurement", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (tenParseMode.enPM_MEASUREMENT_PARSING == aenParseMode[iParseNestLevel])
                            {
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_MEASUREMENT_FOUND;
                            }
                        }
                        else if (0 == szInputLine.IndexOf("/end characteristic", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (tenParseMode.enPM_CHARACTERISTIC_PARSING == aenParseMode[iParseNestLevel])
                            {
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_CHARACTERISTIC_FOUND;
                            }
                        }
                        else if (0 == szInputLine.IndexOf("/end compu_method", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (tenParseMode.enPM_COMPU_METHOD_PARSING == aenParseMode[iParseNestLevel])
                            {
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_COMPU_METHOD_FOUND;
                            }
                        }
                        else if (0 == szInputLine.IndexOf("/end axis_descr", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (tenParseMode.enPM_AXIS_DESCRIPTION_PARSING == aenParseMode[iParseNestLevel])
                            {
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_AXIS_DESCRIPTION_FOUND;
                            }
                        }
                        else if (0 == szInputLine.IndexOf("/end axis_pts", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (tenParseMode.enPM_AXIS_POINTS_PARSING == aenParseMode[iParseNestLevel])
                            {
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_AXIS_POINTS_FOUND;
                            }
                        }
                        else if (0 == szInputLine.IndexOf("/end compu_tab_range", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (tenParseMode.enPM_COMPU_TAB_RANGE_PARSING == aenParseMode[iParseNestLevel])
                            {
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_COMPU_TAB_RANGE_FOUND;
                            }
                        }
                        else if (0 == szInputLine.IndexOf("/end blob", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (tenParseMode.enPM_BLOB_PARSING == aenParseMode[iParseNestLevel])
                            {
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_BLOB_FOUND;
                            }
                        }
                        else
                        {
                            switch (aenParseMode[iParseNestLevel])
                            {
                                case tenParseMode.enPM_CHARACTERISTIC_PARSING:
                                case tenParseMode.enPM_MEASUREMENT_PARSING:
                                case tenParseMode.enPM_BLOB_PARSING:
                                {
                                    switch (aiParseLineIndex[iParseNestLevel])
                                    {
                                        case 2:
                                        {
                                            foreach (String szValid in aszParamTypeStrings)
                                            {
                                                if (0 == String.Compare(szValid, szInputLine, StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                                    aiParseLineIndex[iParseNestLevel]++;
                                                    break;
                                                }
                                            }
                                            if (3 != aiParseLineIndex[iParseNestLevel]) aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                                            break;
                                        }
                                        case 4:
                                        {
                                            foreach (String szValid in aszRecLayoutString)
                                            {
                                                if (0 == String.Compare(szValid, szInputLine, StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                                    aiParseLineIndex[iParseNestLevel]++;
                                                    break;
                                                }
                                            }
                                            if (5 != aiParseLineIndex[iParseNestLevel]) aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                                            break;
                                        }
                                        case 3:
                                        {
                                            szInputLine = szInputLine.Replace("x", "0");
                                            aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                            aiParseLineIndex[iParseNestLevel]++;
                                            break;
                                        }
                                        case 1:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                        {
                                            aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                            aiParseLineIndex[iParseNestLevel]++;
                                            break;
                                        }
                                        default:
                                        {
                                            break;
                                        }
                                    }
                                    break;
                                }

                                case tenParseMode.enPM_COMPU_METHOD_PARSING:
                                {
                                    switch (aiParseLineIndex[iParseNestLevel])
                                    {
                                        case 2:
                                        {
                                            aszInputSubStrings = szInputLine.Split(' ');
                                            foreach (String szValid in aszCMTypeStrings)
                                            {
                                                if (0 == String.Compare(szValid, aszInputSubStrings[0], StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = aszInputSubStrings[0];
                                                    aiParseLineIndex[iParseNestLevel]++;
                                                    break;
                                                }
                                            }
                                            if (3 != aiParseLineIndex[iParseNestLevel])
                                            {
                                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                                                break;
                                            }
                                            aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]++] = aszInputSubStrings[1];
                                            aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]++] = aszInputSubStrings[2];
                                            break;
                                        }
                                        case 5:
                                        {
                                            if (0 == szInputLine.IndexOf("/end compu_method"))
                                            {
                                                for (int iStringIDX = aiParseLineIndex[iParseNestLevel]; iStringIDX < 10; iStringIDX++)
                                                {
                                                    aaszInputStrings[iParseNestLevel, iStringIDX] = "0";
                                                }

                                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_COMPU_METHOD_FOUND;
                                                break;
                                            }

                                            aszInputSubStrings = szInputLine.Split(' ');

                                            foreach (String szValid in aszCMCoeffStrings)
                                            {
                                                if (0 == String.Compare(szValid, aszInputSubStrings[0], StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = aszInputSubStrings[0];
                                                    aiParseLineIndex[iParseNestLevel]++;
                                                    break;
                                                }
                                            }

                                            if (6 != aiParseLineIndex[iParseNestLevel])
                                            {
                                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                                                break;
                                            }

                                            for (int iSubStringIDX = 1; iSubStringIDX < aszInputSubStrings.Length; iSubStringIDX++)
                                            {
                                                aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]++] = aszInputSubStrings[iSubStringIDX];
                                            }

                                            for (int iStringIDX = aiParseLineIndex[iParseNestLevel]; iStringIDX < 10; iStringIDX++)
                                            {
                                                aaszInputStrings[iParseNestLevel, iStringIDX] = "0";
                                            }

                                            aenParseMode[iParseNestLevel] = tenParseMode.enPM_COMPU_METHOD_FOUND;
                                            break;
                                        }

                                        default:
                                        {
                                            aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                            aiParseLineIndex[iParseNestLevel]++;
                                            break;
                                        }
                                    }
                                    break;
                                }
                                case tenParseMode.enPM_AXIS_DESCRIPTION_PARSING:
                                {
                                    aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                    aiParseLineIndex[iParseNestLevel]++;
                                    break;
                                }
                                case tenParseMode.enPM_AXIS_POINTS_PARSING:
                                {

                                    switch (aiParseLineIndex[iParseNestLevel])
                                    {
                                        case 2:
                                            {
                                                szInputLine = szInputLine.Replace("x", "0");
                                                aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                                aiParseLineIndex[iParseNestLevel]++;
                                                break;
                                            }
                                        default:
                                            {
                                                aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                                aiParseLineIndex[iParseNestLevel]++;
                                                break;
                                            }
                                    }
                                    break;
                                }
                                case tenParseMode.enPM_COMPU_TAB_RANGE_PARSING:
                                {
                                    switch (aiParseLineIndex[iParseNestLevel])
                                    {
                                        default:
                                            {
                                                aaszInputStrings[iParseNestLevel, aiParseLineIndex[iParseNestLevel]] = szInputLine;
                                                aiParseLineIndex[iParseNestLevel]++;
                                                break;
                                            }
                                    }
                                    break;
                                }


                                default:
                                {
                                    break;
                                }
                            }
                        }

                        if (tenParseMode.enPM_CHARACTERISTIC_FOUND == aenParseMode[iParseNestLevel])
                        {
                            tstCharacteristic stCharacteristic = new tstCharacteristic();
                            int iCharByteCount;

                            //try
                            {
                                stCharacteristic.szCharacteristicName = aaszInputStrings[iParseNestLevel, 0];
                                stCharacteristic.szInfoString = aaszInputStrings[iParseNestLevel, 1];
                                stCharacteristic.enParamType = (tenParamType)Array.IndexOf(aszParamTypeStrings, aaszInputStrings[iParseNestLevel, 2]);
                                stCharacteristic.u32Address = System.UInt32.Parse(aaszInputStrings[iParseNestLevel, 3], NumberStyles.AllowHexSpecifier);
                                stCharacteristic.enRecLayout = (tenRecLayout)Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 4]);
                                stCharacteristic.iByteCount = aiParamTypeByteCounts[Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 4])];
                                stCharacteristic.boIsSigned = boParamTypeIsSigned[Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 4])];
                                stCharacteristic.szCompuMethod = aaszInputStrings[iParseNestLevel, 5];
                                stCharacteristic.szGroup = aaszInputStrings[iParseNestLevel, 6];
                                stCharacteristic.sLowerLim = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 7], nfi);
                                stCharacteristic.sUpperLim = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 8], nfi);
                                stCharacteristic.iXAxisRef = iGetAxisIDX(stCharacteristic.szCharacteristicName, tenAxis.enAX_X);
                                stCharacteristic.iYAxisRef = iGetAxisIDX(stCharacteristic.szCharacteristicName, tenAxis.enAX_Y);
                                mlstCharacteristicList.Add(stCharacteristic);

                                switch (stCharacteristic.enParamType)
                                {                                    
                                    case tenParamType.enPT_VALUE:
                                        {
                                            iCharByteCount = stCharacteristic.iByteCount;
                                            break;
                                        }
                                    case tenParamType.enPT_CURVE:
                                        {
                                            if (-1 < stCharacteristic.iXAxisRef)
                                            {
                                                iCharByteCount = stCharacteristic.iByteCount * tclsASAM.mlstAxisDescrList[stCharacteristic.iXAxisRef].iAxisPointCount;
                                            }
                                            else
                                            {
                                                iCharByteCount = stCharacteristic.iByteCount;
                                            }
                                            break;
                                        }
                                    default:
                                        {
                                            iCharByteCount = stCharacteristic.iByteCount;
                                            break;
                                        }
                                }

                                if (false == stCharacteristic.szCharacteristicName.Contains("DIAG Spread Address Table"))
                                {
                                    mu32CharMinAddress = stCharacteristic.u32Address < mu32CharMinAddress ? stCharacteristic.u32Address : mu32CharMinAddress;
                                    mu32CharMaxAddress = (stCharacteristic.u32Address + (UInt32)iCharByteCount - 1u) > mu32CharMaxAddress ?
                                        stCharacteristic.u32Address + (UInt32)iCharByteCount - 1 : mu32CharMaxAddress;
                                }

 
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                            //}
                            //catch
                            //{
                            //    boParseError = true;
                            }
                            vEndParseNewRecord(ref iParseNestLevel);
                        }

                        if (tenParseMode.enPM_MEASUREMENT_FOUND == aenParseMode[iParseNestLevel])
                        {
                            tstMeasurement stMeasurement = new tstMeasurement();

                            try
                            {
                                stMeasurement.szMeasurementName = aaszInputStrings[iParseNestLevel, 0];
                                stMeasurement.szInfoString = aaszInputStrings[iParseNestLevel, 1];
                                stMeasurement.enParamType = (tenParamType)Array.IndexOf(aszParamTypeStrings, aaszInputStrings[iParseNestLevel, 2]);
                                stMeasurement.enRecLayout = (tenRecLayout)Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 4]);
                                stMeasurement.iByteCount = aiParamTypeByteCounts[Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 4])];
                                stMeasurement.boIsSigned = boParamTypeIsSigned[Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 4])];
                                stMeasurement.u32Address = System.UInt32.Parse(aaszInputStrings[iParseNestLevel, 3], NumberStyles.AllowHexSpecifier);
                                stMeasurement.szCompuMethod = aaszInputStrings[iParseNestLevel, 5];
                                stMeasurement.szGroup = aaszInputStrings[iParseNestLevel, 6];
                                stMeasurement.sLowerLim = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 7], nfi);
                                stMeasurement.sUpperLim = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 8], nfi);
                                mlstAvailableMeasList.Add(stMeasurement);
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                            }
                            catch
                            {
                                boParseError = true;
                            }
                            vEndParseNewRecord(ref iParseNestLevel);
                        }

                        if (tenParseMode.enPM_BLOB_FOUND == aenParseMode[iParseNestLevel])
                        {
                            tstBlob stBlob = new tstBlob();

                            try
                            {
                                stBlob.szBlobName = aaszInputStrings[iParseNestLevel, 0];
                                stBlob.szInfoString = aaszInputStrings[iParseNestLevel, 1];
                                stBlob.enParamType = (tenParamType)Array.IndexOf(aszParamTypeStrings, aaszInputStrings[iParseNestLevel, 2]);
                                stBlob.enRecLayout = (tenRecLayout)Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 4]);
                                stBlob.iPointCount = (int)Convert.ToSingle(aaszInputStrings[iParseNestLevel, 8], nfi);
                                stBlob.boIsSigned = boParamTypeIsSigned[Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 4])];
                                stBlob.u32Address = System.UInt32.Parse(aaszInputStrings[iParseNestLevel, 3], NumberStyles.AllowHexSpecifier);
                                stBlob.szGroup = aaszInputStrings[iParseNestLevel, 5];
                                stBlob.sLowerLim = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 6], nfi);
                                stBlob.sUpperLim = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 7], nfi);
                                mlstBlobList.Add(stBlob);
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                            }
                            catch
                            {
                                boParseError = true;
                            }
                            vEndParseNewRecord(ref iParseNestLevel);
                        }

                        if (tenParseMode.enPM_COMPU_METHOD_FOUND == aenParseMode[iParseNestLevel])
                        {
                            tstVarBits stCompuMethod = new tstVarBits();

                            try
                            {
                                stCompuMethod.szCompuMethodName = aaszInputStrings[iParseNestLevel, 0];
                                aaszInputStrings[iParseNestLevel, 1] = aaszInputStrings[iParseNestLevel, 1].Replace('"', ' ');
                                aaszInputStrings[iParseNestLevel, 1] = aaszInputStrings[iParseNestLevel, 1].Trim();
                                stCompuMethod.szInfoString = aaszInputStrings[iParseNestLevel, 1];
                                stCompuMethod.enCM_Type = (tenCM_Type)Array.IndexOf(aszCMTypeStrings, aaszInputStrings[iParseNestLevel, 2]);
                                aaszInputStrings[iParseNestLevel, 3] = aaszInputStrings[iParseNestLevel, 3].Replace('%', '0');
                                aaszInputStrings[iParseNestLevel, 3] = aaszInputStrings[iParseNestLevel, 3].Replace('"', ' ');
                                aszInputSubStrings = aaszInputStrings[iParseNestLevel, 3].Split('.');

                                stCompuMethod.iPreDPCount = Convert.ToInt16(aszInputSubStrings[0], nfi) - Convert.ToInt16(aszInputSubStrings[1], nfi);
                                stCompuMethod.iPostDPCount = Convert.ToInt16(aszInputSubStrings[1], nfi);

                                aaszInputStrings[iParseNestLevel, 4] = aaszInputStrings[iParseNestLevel, 4].Replace('"', ' ');
                                aaszInputStrings[iParseNestLevel, 4] = aaszInputStrings[iParseNestLevel, 4].Trim();
                                stCompuMethod.szUnitsString = aaszInputStrings[iParseNestLevel, 4];

                                stCompuMethod.enCM_CoeffType = (tenCM_CoeffType)Array.IndexOf(aszCMCoeffStrings, aaszInputStrings[iParseNestLevel, 5]);
                                stCompuMethod.sCoeff1 = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 6], nfi);
                                stCompuMethod.sCoeff2 = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 7], nfi);
                                stCompuMethod.sCoeff3 = Convert.ToSingle(aaszInputStrings[iParseNestLevel, 8], nfi);
                                mlstCompuMethodList.Add(stCompuMethod);
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                            }
                            catch
                            {
                                boParseError = true;
                            }

                            vEndParseNewRecord(ref iParseNestLevel);
                        }

                        if (tenParseMode.enPM_COMPU_TAB_RANGE_FOUND == aenParseMode[iParseNestLevel])
                        {
                            int iVerbCount;
                            tstVarVerb stVarVerb = new tstVarVerb();
                            stVarVerb.lstVarVerb = new List<tstVerbRecord>();
                            int iTempCount;

                            try
                            {
                                stVarVerb.szCompuRangeName = aaszInputStrings[iParseNestLevel, 0];
                                stVarVerb.szInfoString = aaszInputStrings[iParseNestLevel, 1];
                                iVerbCount = Convert.ToInt16(aaszInputStrings[iParseNestLevel, 2], nfi);

                                iTempCount = 3;

                                while (0 < iVerbCount)
                                {
                                    tstVerbRecord stVarVerbRecord = new tstVerbRecord();
                                    String szVarVerbString = aaszInputStrings[iParseNestLevel, iTempCount];

                                    while (szVarVerbString.Contains("  "))
                                    {
                                        szVarVerbString = szVarVerbString.Replace("  ", " ");
                                    }
                                    String[] aszVarVerbStrings;

                                    aszVarVerbStrings = szVarVerbString.Split(' ');
                                    stVarVerbRecord.szVerb = aszVarVerbStrings[1];
                                    stVarVerbRecord.iValueLow = Convert.ToInt32(aszVarVerbStrings[0], nfi);
                                    stVarVerb.lstVarVerb.Add(stVarVerbRecord);

                                    iTempCount++;
                                    iVerbCount--;
                                }

                                mlstVarVerbList.Add(stVarVerb);

                                /* Now add a dummy compu method that links to this verbal range */
                                tstVarBits stCompuMethod = new tstVarBits();

                                stCompuMethod.szCompuMethodName = stVarVerb.szCompuRangeName.Replace("TAB_VERB_", "");
                                stCompuMethod.szInfoString = stVarVerb.szInfoString;
                                stCompuMethod.enCM_Type = tenCM_Type.enTAB_VERB;
                                stCompuMethod.szUnitsString = "ENUM";
                                stCompuMethod.iVerbTableIDX = mlstVarVerbList.Count - 1;

                                mlstCompuMethodList.Add(stCompuMethod);
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                            }
                            catch
                            {
                                boParseError = true;
                            }

                            vEndParseNewRecord(ref iParseNestLevel);
                        }

                        if (tenParseMode.enPM_AXIS_DESCRIPTION_FOUND == aenParseMode[iParseNestLevel])
                        {
                            tstAxisDescr stAxisDescr = new tstAxisDescr();
                            String szTemp = "";

                            try
                            {
                                stAxisDescr.enAxisType = (tenAxisType)Array.IndexOf(aszAxisTypeStrings, aaszInputStrings[iParseNestLevel, 1]);
                                stAxisDescr.szVar = aaszInputStrings[iParseNestLevel, 2];
                                stAxisDescr.szCompuMethod = aaszInputStrings[iParseNestLevel, 3];
                                stAxisDescr.iAxisPointCount = Convert.ToInt32(aaszInputStrings[iParseNestLevel, 4], nfi);
                                stAxisDescr.iLowerLim = Convert.ToInt32(aaszInputStrings[iParseNestLevel, 5], nfi);
                                stAxisDescr.iUpperLim = Convert.ToInt32(aaszInputStrings[iParseNestLevel, 6], nfi);
                                if (0 == aaszInputStrings[iParseNestLevel, 7].IndexOf("axis_pts_ref", StringComparison.OrdinalIgnoreCase))
                                {
                                    szTemp = aaszInputStrings[iParseNestLevel, 7].Substring(12).Trim();
                                }
                                stAxisDescr.szAxisPointsRef = szTemp;
                                mlstAxisDescrList.Add(stAxisDescr);
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                            }
                            catch
                            {
                                boParseError = true;
                            }
                            vEndParseNewRecord(ref iParseNestLevel);
                        }

                        if (tenParseMode.enPM_AXIS_POINTS_FOUND == aenParseMode[iParseNestLevel])
                        {
                            tstAxisPts stAxisPts = new tstAxisPts();

                            try
                            {

                                stAxisPts.szAxisPtsName = aaszInputStrings[iParseNestLevel, 0];
                                stAxisPts.szInfoString = aaszInputStrings[iParseNestLevel, 1];
                                stAxisPts.u32Address = System.UInt32.Parse(aaszInputStrings[iParseNestLevel, 2], NumberStyles.AllowHexSpecifier);
                                stAxisPts.enRecLayout = (tenRecLayout)Array.IndexOf(aszRecLayoutString, aaszInputStrings[iParseNestLevel, 3]);
                                stAxisPts.szVar = aaszInputStrings[iParseNestLevel, 4];
                                stAxisPts.szCompuMethod = aaszInputStrings[iParseNestLevel, 5];
                                stAxisPts.iAxisPointCount = Convert.ToInt32(aaszInputStrings[iParseNestLevel, 6]);
                                stAxisPts.iLowerLim = Convert.ToInt32(aaszInputStrings[iParseNestLevel, 7]);
                                stAxisPts.iUpperLim = Convert.ToInt32(aaszInputStrings[iParseNestLevel, 8]);
                                mlstAxisPtsList.Add(stAxisPts);
                                aenParseMode[iParseNestLevel] = tenParseMode.enPM_Invalid;
                            }
                            catch
                            {
                                boParseError = true;
                            }
                            vEndParseNewRecord(ref iParseNestLevel);
                        }
                    }
                }
                milstCharacteristicList = mlstCharacteristicList.AsReadOnly();
                milstCompuMethodList = mlstCompuMethodList.AsReadOnly();
                milstAxisDescrList = mlstAxisDescrList.AsReadOnly();
                milstAxisPtsList = mlstAxisPtsList.AsReadOnly();
                milstBlobList = mlstBlobList.AsReadOnly();
                milstVarVerbList = mlstVarVerbList.AsReadOnly();
            }

            if (true == boParseError)
            {
                System.Windows.Forms.MessageBox.Show("A Fatal ASAM Parse Error Occurred, Rectify ASAM A2L File or Number Format Problem", "Fatal Error");
            }

            u16GetASAMCRC();
        }

        static void vStartParseNewRecord(tenParseMode enParseMode, ref tenParseMode[] aenParseMode, ref int iNestLevel)
        {
            if (tenParseMode.enPM_Invalid != aenParseMode[iNestLevel])
            {
                aenParseMode[++iNestLevel] = enParseMode;
            }
            else
            {
                aenParseMode[iNestLevel] = enParseMode;
            }
        }

        static void vClearStringArray(ref String[,] aaszStrings, int iParseNestLevel)
        {
            int iStringIDX;

            for (iStringIDX = 0; iStringIDX < 10; iStringIDX++)
            {
                aaszStrings[iParseNestLevel, iStringIDX] = "0";
            }
        }

        static void vEndParseNewRecord(ref int iNestLevel)
        {
            iNestLevel = (0 < iNestLevel) ? iNestLevel - 1 : iNestLevel;
        }

        public static void vSetDDDIResetPendingIDX(int iQueueIDX)
        {
            if ((maboDDDIResetPending.Length > iQueueIDX) && (null != mau8DDDI[iQueueIDX]))
            {
                maboDDDIResetPending[iQueueIDX] = true;
            }
        }

        public static void vCalcDDDIAndSetRate(int iQueueIDX, int iRateMs)
        {
            byte[] au8Data = new byte[4];
            byte[] au8DDDIData = new byte[256];
            int iByteIDX = 0;
            int iByteCount;
            byte au8RateByte;

            foreach (tstMeasurement stMeasurement in malstActiveMeasLists[iQueueIDX])
            {
                au8Data = BitConverter.GetBytes(stMeasurement.u32Address);
                for (iByteCount = 0; iByteCount < (ConstantData.UDS.ru8DDDI_ALFID & 0x0f); iByteCount++)
                {
                    au8DDDIData[iByteIDX++] = au8Data[iByteCount];
                }

                au8Data = BitConverter.GetBytes(stMeasurement.iByteCount);
                for (iByteCount = 0; iByteCount < (ConstantData.UDS.ru8DDDI_ALFID >> 0x04); iByteCount++)
                {
                    au8DDDIData[iByteIDX++] = au8Data[iByteCount];
                }
            }

            Array.Resize(ref au8DDDIData, iByteIDX);
            mau8DDDI[iQueueIDX] = au8DDDIData;

            switch (iRateMs)
            {
                case 1: au8RateByte = 0; break;
                case 2: au8RateByte = 1; break;
                case 5: au8RateByte = 2; break;
                case 10: au8RateByte = 3; break;
                case 20: au8RateByte = 4; break;
                case 50: au8RateByte = 5; break;
                case 100: au8RateByte = 6; break;
                case 200: au8RateByte = 7; break;
                case 500: au8RateByte = 8; break;
                case 1000: au8RateByte = 9; break;
                case 2000: au8RateByte = 10; break;
                default: au8RateByte = 15; break;
            }

            maiMeasListSID[iQueueIDX] = (UInt16)(0x9000 + 0x100 * au8RateByte + iQueueIDX);
            maiMeasurementListTimerLimits[iQueueIDX] = iRateMs;
        }

        public static void vSetDDDIResetPending(int iDDDIIDX, bool boPending)
        {
            if (mau8DDDI[iDDDIIDX] != null)
            {
                maboDDDIResetPending[iDDDIIDX] = boPending;
            }
        }


        public static byte[] au8GetDDDI(int iDDDIIDX)
        {
            if ((mau8DDDI[iDDDIIDX].Length > 0) && (true == maboDDDIResetPending[iDDDIIDX]))
            {
                return (byte[])mau8DDDI[iDDDIIDX];
            }
            else
            {
                return null;
            }
        }

        public static UInt16 uiGetDDDISID(int iDDDIIDX)
        {
            return maiMeasListSID[iDDDIIDX];
        }

        public static void vClocksReset()
        {
            int iMeasurementListIDX = 0;

            for (iMeasurementListIDX = 0; iMeasurementListIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT; iMeasurementListIDX++)
            {
                maiMeasurementListTimers[iMeasurementListIDX] = 0;
            }
        }

        public static void vClockMeasurements(int iCallPeriodMs)
        {
            int iMeasurementListIDX = 0;

            for (iMeasurementListIDX = 0; iMeasurementListIDX < ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT; iMeasurementListIDX++)
            {
                maiMeasurementListTimers[iMeasurementListIDX] += iCallPeriodMs;

                if (maiMeasurementListTimerLimits[iMeasurementListIDX] <= maiMeasurementListTimers[iMeasurementListIDX])
                {
                    if (16 > mqMeasListQueue.Count)
                    { 
                        maiMeasurementListTimers[iMeasurementListIDX] = 0;

                        if ((0 != maiMeasListSID[iMeasurementListIDX]) && (mau8DDDI[iMeasurementListIDX].Length > 0))
                        {
                            mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);

                            if (0x9003 == maiMeasListSID[iMeasurementListIDX])
                            {
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                                //mqMeasListQueue.Enqueue(maiMeasListSID[iMeasurementListIDX]);
                            }
                        }
                    }
                }
            }
        }

        public static UInt16 iGetNextMeasListID()
        {
            UInt16 iNextMeasListSID = 0;

            if (0 < mqMeasListQueue.Count)
            {
                iNextMeasListSID = mqMeasListQueue.Dequeue();
            }

            return iNextMeasListSID;
        }

        public static tstActiveMeasureIndices stTryAddActiveMeas(String szMeasName, int iMeasureQueueIDX)
        {
            tstActiveMeasureIndices stActiveMeasureIndices;
            int iAvailableIDX;

            stActiveMeasureIndices.iMeasureQueue = -1;
            stActiveMeasureIndices.iMeasureQueueIDX = -1;

            if (ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT > iMeasureQueueIDX)
            {
                stActiveMeasureIndices.iMeasureQueue = iMeasureQueueIDX;
                iAvailableIDX = iGetAvailableMeasIndex(szMeasName);

                if (-1 < iAvailableIDX)
                {
                    tstMeasurement stMeas = mlstAvailableMeasList[iAvailableIDX];
                    malstActiveMeasLists[iMeasureQueueIDX].Add(stMeas);
                    stActiveMeasureIndices.iMeasureQueueIDX = malstActiveMeasLists[iMeasureQueueIDX].Count - 1;
                }
            }

            return stActiveMeasureIndices;
        }

        public static tstActiveMeasureIndices stTryRemoveActiveMeas(String szMeasName, int iMeasureQueueIDX)
        {
            tstActiveMeasureIndices stRemovedMeasureIndices;
            int iActiveMeasureIDX = 0;

            stRemovedMeasureIndices.iMeasureQueue = -1;
            stRemovedMeasureIndices.iMeasureQueueIDX = -1;

            if (ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT > iMeasureQueueIDX)
            {
                foreach (tstMeasurement stMeasurement in malstActiveMeasLists[iMeasureQueueIDX])
                {
                    if (szMeasName == stMeasurement.szMeasurementName)
                    {
                        stRemovedMeasureIndices.iMeasureQueue = iMeasureQueueIDX;
                        stRemovedMeasureIndices.iMeasureQueueIDX = iActiveMeasureIDX;
                        malstActiveMeasLists[iMeasureQueueIDX].RemoveAt(iActiveMeasureIDX);
                        break;
                    }

                    iActiveMeasureIDX++;
                }
            }

            return stRemovedMeasureIndices;
        }

        public static tstActiveMeasureIndices stGetActiveMeas(String szMeasName, int iMeasureQueueIDX)
        {
            tstActiveMeasureIndices stActiveMeasureIndices;
            int iAvailableIDX;

            stActiveMeasureIndices.iMeasureQueue = -1;
            stActiveMeasureIndices.iMeasureQueueIDX = -1;

            if (ConstantData.UDS.ru8UDS_MEAS_LIST_COUNT > iMeasureQueueIDX)
            {
                stActiveMeasureIndices.iMeasureQueue = iMeasureQueueIDX;
                iAvailableIDX = iGetAvailableMeasIndex(szMeasName);

                if (-1 < iAvailableIDX)
                {
                    tstMeasurement stMeas = mlstAvailableMeasList[iAvailableIDX];
                    stActiveMeasureIndices.iMeasureQueueIDX = malstActiveMeasLists[iMeasureQueueIDX].Count - 1;
                }
            }

            return stActiveMeasureIndices;
        }


        public static int iGetBlobIndex(String szBlobName)
        {
            int iBlobIDX = -1;
            int iBlobListIDX = 0;

            foreach (tstBlob stBlob in mlstBlobList)
            {
                if (0 == String.Compare(szBlobName, stBlob.szBlobName))
                {
                    iBlobIDX = iBlobListIDX;
                    break;
                }
                iBlobListIDX++;
            }

            return iBlobIDX;
        }

        public static int iGetCharIndex(String szCharName)
        {
            int iCharIDX = -1;
            int iCharListIDX = 0;

            foreach (tstCharacteristic stChar in mlstCharacteristicList)
            {
                if (0 == String.Compare(szCharName, stChar.szCharacteristicName))
                {
                    iCharIDX = iCharListIDX;
                    break;
                }
                iCharListIDX++;
            }

            return iCharIDX;
        }

        public static int iGetAvailableMeasIndex(String szMeasName)
        {
            int iMeasIDX = -1;
            int iMeasListIDX = 0;

            foreach (tstMeasurement stMeas in mlstAvailableMeasList)
            {
                if (0 == String.Compare(szMeasName, stMeas.szMeasurementName))
                {
                    iMeasIDX = iMeasListIDX;
                    break;
                }
                iMeasListIDX++;
            }

 
            if (-1 == iMeasIDX)
            {
                /* Try find linked measure */
                string szLinkedMeasure = null;

                foreach (tstMeasurement stMeas in mlstAvailableMeasList)
                {
                    if (0 == stMeas.szMeasurementName.IndexOf(szMeasName))
                    {
                        string[] aszLinkedMeasure = stMeas.szMeasurementName.Split('|');
                        szLinkedMeasure = aszLinkedMeasure[2];
                        break;
                    }
                }

                if (null != szLinkedMeasure)
                {
                    iMeasListIDX = 0;

                    foreach (tstMeasurement stMeas in mlstAvailableMeasList)
                    {
                        if (0 == String.Compare(szLinkedMeasure, stMeas.szMeasurementName))
                        {
                            iMeasIDX = iMeasListIDX;
                            break;
                        }

                        iMeasListIDX++;
                    }
                }
            }

            return iMeasIDX;
        }

        public static int iGetActiveMeasIndex(String szMeasName, int iMeasureQueueIDX)
        {
            int iMeasIDX = -1;
            int iMeasListIDX = 0;

            foreach (tstMeasurement stMeas in mailstActiveMeasLists[iMeasureQueueIDX])
            {
                if (0 == String.Compare(szMeasName, stMeas.szMeasurementName))
                {
                    iMeasIDX = iMeasListIDX;
                    break;
                }
                iMeasListIDX++;
            }

            return iMeasIDX;
        }

        public static int iGetCompuMethodIndexFromMeas(String szCharMeasName, int iMeasureQueueIDX)
        {
            int iActiveMeasListIDX;
            String szCompuMethod = "";
            int iCompuMethodIDX = -1;
            int iCompuMethodListIDX = 0;

            iActiveMeasListIDX = iGetActiveMeasIndex(szCharMeasName, iMeasureQueueIDX);

            if (-1 < iActiveMeasListIDX)
            {
                szCompuMethod = malstActiveMeasLists[iMeasureQueueIDX]
                    [iActiveMeasListIDX].szCompuMethod;
            }

            if (0 != szCompuMethod.Length)
            {
                foreach (tstVarBits stCompuMethod in mlstCompuMethodList)
                {
                    if (0 == String.Compare(szCompuMethod, stCompuMethod.szCompuMethodName))
                    {
                        iCompuMethodIDX = iCompuMethodListIDX;
                        break;
                    }
                    iCompuMethodListIDX++;
                }
            }

            return iCompuMethodIDX;
        }

        public static int iGetCompuMethodIndexFromChar(String szCharMeasName)
        {
            String szCompuMethod = "";
            int iCompuMethodIDX = -1;
            int iCompuMethodListIDX = 0;
            int iCharIDX = iGetCharIndex(szCharMeasName);

            if (-1 < iCharIDX)
            {
                szCompuMethod = mlstCharacteristicList[iCharIDX].szCompuMethod;
            }

            if (0 != szCompuMethod.Length)
            {
                foreach (tstVarBits stCompuMethod in mlstCompuMethodList)
                {
                    if (0 == String.Compare(szCompuMethod, stCompuMethod.szCompuMethodName))
                    {
                        iCompuMethodIDX = iCompuMethodListIDX;
                        break;
                    }
                    iCompuMethodListIDX++;
                }
            }

            return iCompuMethodIDX;
        }

        public static int iGetCompuMethodIndexFromCompuMethod(String szCompuMethod)
        {
            //tstActiveMeasureIndices stActiveMeasureIndices;
            int iCompuMethodIDX = -1;
            int iCompuMethodListIDX = 0;

            foreach (tstVarBits stCompuMethod in mlstCompuMethodList)
            {
                if (0 == String.Compare(szCompuMethod, stCompuMethod.szCompuMethodName))
                {
                    iCompuMethodIDX = iCompuMethodListIDX;
                    break;
                }
                iCompuMethodListIDX++;
            }

            return iCompuMethodIDX;
        }

        public static int iGetAxisPtsIndex(String szAxisPtsRef)
        {
            int iAxisPtsIDX = -1;
            int iAxisPtsListIDX = 0;

            foreach (tstAxisPts stAxisPts in mlstAxisPtsList)
            {
                if (0 == String.Compare(szAxisPtsRef, stAxisPts.szAxisPtsName))
                {
                    iAxisPtsIDX = iAxisPtsListIDX;
                    break;
                }
                iAxisPtsListIDX++;
            }

            return iAxisPtsIDX;
        }

        public static int iGetAxisIDX(String szCharName, tenAxis enAxis)
        {
            int iAxisIDX = -1;
            int iAxisListIDX = 0;

            if (tenAxis.enAX_X == enAxis)
            {
                szCharName += "_xaxis";
            }

            if (tenAxis.enAX_Y == enAxis)
            {
                szCharName += "_yaxis";
            }

            foreach(tstAxisDescr stAxisDescr in mlstAxisDescrList)
            {
                if (0 == String.Compare(szCharName, stAxisDescr.szAxisPointsRef, StringComparison.OrdinalIgnoreCase))
                {
                   iAxisIDX = iAxisListIDX;
                   break;
                }
                iAxisListIDX++;
            }

            return iAxisIDX;
        }

        public static UInt32 u32GetCharMinAddress()
        {
            return mu32CharMinAddress;
        }

        public static UInt32 u32GetCharMaxAddress()
        {
            return mu32CharMaxAddress;
        }

        public static UInt16 u16GetCRC16()
        {
            return mu16CRC;
        }

        static UInt16 u16GetASAMCRC()
        {
            mu16CRCDataArrayIDX = 0;
            mu16CRC = 0xffff;

            foreach (tstCharacteristic stCharacteristic in mlstCharacteristicList)
            {
                string szData = null;

                byte[] au8Data = BitConverter.GetBytes(stCharacteristic.u32Address);
                GetDataCRC16FromByteArrayOrString(ref au8Data, ref szData);

                au8Data = BitConverter.GetBytes(stCharacteristic.iYAxisRef);
                GetDataCRC16FromByteArrayOrString(ref au8Data, ref szData);

                au8Data = BitConverter.GetBytes(stCharacteristic.iXAxisRef);
                GetDataCRC16FromByteArrayOrString(ref au8Data, ref szData);

                au8Data = BitConverter.GetBytes(stCharacteristic.iByteCount);
                GetDataCRC16FromByteArrayOrString(ref au8Data, ref szData);

                au8Data = null;
                szData = stCharacteristic.szCharacteristicName +
                    stCharacteristic.szCompuMethod +
                    stCharacteristic.szGroup +
                    stCharacteristic.szInfoString;

                GetDataCRC16FromByteArrayOrString(ref au8Data, ref szData);
            }

            foreach (tstMeasurement stMeasurement in mlstAvailableMeasList)
            {

            }

            foreach (tstVarBits stVarBits in mlstCompuMethodList)
            {

            }

            foreach (tstAxisDescr stAxisDescr in mlstAxisDescrList)
            {

            }

            foreach (tstAxisPts stAxisPts in mlstAxisPtsList)
            {

            }

            foreach (tstBlob stBlob in mlstBlobList)
            {

            }

            return mu16CRC;
        }

        static public UInt16 GetDataCRC16FromByteArrayOrString(ref byte[] au8Data, ref string szDataString)
        {
            int iDataSize = 0;

            if (null != au8Data)
            {
                iDataSize = au8Data.Length;

                if (256 - mu16CRCDataArrayIDX >= iDataSize)
                {
                    Array.Copy(au8Data, 0, mau8CRCData, mu16CRCDataArrayIDX, iDataSize);

                    while (iDataSize-- > 0)
                    {
                        UInt16 CRC16ShiftL = (UInt16)((mu16CRC & 0x00ff) << 8);
                        UInt16 CRC16ShiftR = (UInt16)((mu16CRC & 0xff00) >> 8);
                        UInt16 CRC16Index = (UInt16)(CRC16ShiftR ^ mau8CRCData[mu16CRCDataArrayIDX]);

                        mu16CRC = (UInt16)(CRC16ShiftL ^ ConstantData.CRC16Data.rau16CRC16[CRC16Index]);
                        mu16CRCDataArrayIDX++;
                    }
                }
                else
                {
                    int iWrap = iDataSize;
                    iDataSize = 256 - mu16CRCDataArrayIDX;

                    /* First the end of the full buffer */
                    if (0 != iDataSize)
                    {
                        Array.Copy(au8Data, 0, mau8CRCData, mu16CRCDataArrayIDX, iDataSize);

                        while (iDataSize-- > 0)
                        {
                            UInt16 CRC16ShiftL = (UInt16)((mu16CRC & 0x00ff) << 8);
                            UInt16 CRC16ShiftR = (UInt16)((mu16CRC & 0xff00) >> 8);
                            UInt16 CRC16Index = (UInt16)(CRC16ShiftR ^ mau8CRCData[mu16CRCDataArrayIDX]);

                            mu16CRC = (UInt16)(CRC16ShiftL ^ ConstantData.CRC16Data.rau16CRC16[CRC16Index]);
                            mu16CRCDataArrayIDX++;
                        }
                    }


                    /* Next wrap around to start of array */
                    iDataSize = iWrap;
                    mu16CRCDataArrayIDX = 0;

                    Array.Copy(au8Data, 0, mau8CRCData, mu16CRCDataArrayIDX, iDataSize);

                    while (iDataSize-- > 0)
                    {
                        UInt16 CRC16ShiftL = (UInt16)((mu16CRC & 0x00ff) << 8);
                        UInt16 CRC16ShiftR = (UInt16)((mu16CRC & 0xff00) >> 8);
                        UInt16 CRC16Index = (UInt16)(CRC16ShiftR ^ mau8CRCData[mu16CRCDataArrayIDX]);

                        mu16CRC = (UInt16)(CRC16ShiftL ^ ConstantData.CRC16Data.rau16CRC16[CRC16Index]);
                        mu16CRCDataArrayIDX++;
                    }
                }
            }
            else if (null != szDataString)
            {
                char[] au8StringData = szDataString.ToCharArray();
                byte[] au8Chars = new byte[szDataString.Length];
                int iCharCount = 0;

                foreach(char ucData in au8StringData)
                {
                    au8Chars[iCharCount++] = Convert.ToByte(ucData);
                }   

                iDataSize = au8Chars.Length;

                if (256 - mu16CRCDataArrayIDX >= iDataSize)
                {
                    Array.Copy(au8Chars, 0, mau8CRCData, mu16CRCDataArrayIDX, iDataSize);

                    while (iDataSize-- > 0)
                    {
                        UInt16 CRC16ShiftL = (UInt16)((mu16CRC & 0x00ff) << 8);
                        UInt16 CRC16ShiftR = (UInt16)((mu16CRC & 0xff00) >> 8);
                        UInt16 CRC16Index = (UInt16)(CRC16ShiftR ^ mau8CRCData[mu16CRCDataArrayIDX]);

                        mu16CRC = (UInt16)(CRC16ShiftL ^ ConstantData.CRC16Data.rau16CRC16[CRC16Index]);
                        mu16CRCDataArrayIDX++;
                    }
                }
                else
                {
                    int iWrap = iDataSize;
                    iDataSize = 256 - mu16CRCDataArrayIDX;
                    iWrap -= iDataSize;

                    /* First the end of the full buffer */
                    if (0 != iDataSize)
                    {
                        Array.Copy(au8Chars, 0, mau8CRCData, mu16CRCDataArrayIDX, iDataSize);

                        while (iDataSize-- > 0)
                        {
                            UInt16 CRC16ShiftL = (UInt16)((mu16CRC & 0x00ff) << 8);
                            UInt16 CRC16ShiftR = (UInt16)((mu16CRC & 0xff00) >> 8);
                            UInt16 CRC16Index = (UInt16)(CRC16ShiftR ^ mau8CRCData[mu16CRCDataArrayIDX]);

                            mu16CRC = (UInt16)(CRC16ShiftL ^ ConstantData.CRC16Data.rau16CRC16[CRC16Index]);
                            mu16CRCDataArrayIDX++;
                        }
                    }


                    /* Next wrap around to start of array */
                    iDataSize = iWrap;
                    mu16CRCDataArrayIDX = 0;
                    iWrap -= iDataSize;

                    Array.Copy(au8Chars, 0, mau8CRCData, mu16CRCDataArrayIDX, iDataSize);

                    while (iDataSize-- > 0)
                    {
                        UInt16 CRC16ShiftL = (UInt16)((mu16CRC & 0x00ff) << 8);
                        UInt16 CRC16ShiftR = (UInt16)((mu16CRC & 0xff00) >> 8);
                        UInt16 CRC16Index = (UInt16)(CRC16ShiftR ^ mau8CRCData[mu16CRCDataArrayIDX]);

                        mu16CRC = (UInt16)(CRC16ShiftL ^ ConstantData.CRC16Data.rau16CRC16[CRC16Index]);
                        mu16CRCDataArrayIDX++;
                    }
                }

            }

            return mu16CRC;
        }
    }

    public struct tstVarBits
    {
        public String szCompuMethodName;
        public String szInfoString;
        public tenCM_Type enCM_Type;
        public tenCM_CoeffType enCM_CoeffType;
        public Single sCoeff1;
        public Single sCoeff2;
        public Single sCoeff3;
        public Single sCoeff4;
        public Single sCoeff5;
        public Single sCoeff6;
        public int iPreDPCount;
        public int iPostDPCount;
        public String szUnitsString;
        public int iVerbTableIDX;
    }

    public struct tstVerbRecord
    {
        public String szVerb;
        public String szInfoString;
        public int iValueLow;
        public int iValueHigh;
    }

    public struct tstVarVerb
    {
        public String szCompuRangeName;
        public String szInfoString;
        public List<tstVerbRecord> lstVarVerb;
    }

    public struct tstAxisDescr
    {
        public String szInfoString;
        public String szCompuMethod;
        public String szVar;
        public int iAxisPointCount;
        public int iUpperLim;
        public int iLowerLim;
        public String szAxisPointsRef;
        public tenAxisType enAxisType;
    }

    public struct tstCharacteristic
    {
        public String szCharacteristicName;
        public String szInfoString;
        public tenParamType enParamType;
        public tenRecLayout enRecLayout;
        public UInt32 u32Address;
        public String szCompuMethod;
        public String szGroup;
        public int iXAxisRef;
        public int iYAxisRef;
        public Single sUpperLim;
        public Single sLowerLim;
        public int iByteCount;
        public bool boIsSigned;
    }

    public struct tstMeasurement
    {
        public String szMeasurementName;
        public String szInfoString;
        public tenParamType enParamType;
        public tenRecLayout enRecLayout;
        public UInt32 u32Address;
        public String szCompuMethod;
        public String szGroup;
        public Single sUpperLim;
        public Single sLowerLim;
        public int iByteCount;
        public bool boIsSigned;
    }

    public struct tstBlob
    {
        public String szBlobName;
        public String szInfoString;
        public tenParamType enParamType;
        public tenRecLayout enRecLayout;
        public UInt32 u32Address;
        public String szGroup;
        public Single sUpperLim;
        public Single sLowerLim;
        public int iPointCount;
        public bool boIsSigned;
    }

    public struct tstAxisPts
    {
        public string szAxisPtsName;
        public String szInfoString;
        public String szCompuMethod;
        public tenRecLayout enRecLayout;
        public String szVar;
        public int iAxisPointCount;
        public int iUpperLim;
        public int iLowerLim;
        public UInt32 u32Address;
    }


    public struct tstActiveMeasureIndices
    {
        public int iMeasureQueue;
        public int iMeasureQueueIDX;
    }
}
