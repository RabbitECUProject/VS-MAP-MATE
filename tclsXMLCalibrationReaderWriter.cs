/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Calibration Reader Writer                              */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsCalibrationReaderWriter.cs                         */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace UDP
{
    class tclsXMLCalibrationReaderWriter
    {
        XmlDocument mClsXDoc = new XmlDocument();
        List<tclsCalibrationElement>[] malstCalibrationLists;
        public IList<tclsCalibrationElement>[] mailstCalibrationLists;
        List<tclsAxisElement>[] malstAxisLists;
        public IList<tclsAxisElement>[] mailstAxisLists;
        List<tclsBlobElement>[] malstBlobLists;
        public IList<tclsBlobElement>[] mailstBlobLists;
        private List<List<UInt32[]>> mlstlstau32CalibrationData;
        List<List<UInt32[]>> mlstlstau32AxisData;
        List<List<UInt32[]>> mlstlstau32BlobData;
        string mszReturnString;
        string mszCalibrationFileCurrent;

        public tclsXMLCalibrationReaderWriter()
        {
            malstCalibrationLists = new List<tclsCalibrationElement>[ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX];
            mailstCalibrationLists = new IList<tclsCalibrationElement>[ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX];
            malstAxisLists = new List<tclsAxisElement>[ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX];
            mailstAxisLists = new IList<tclsAxisElement>[ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX];
            malstBlobLists = new List<tclsBlobElement>[ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX];
            mailstBlobLists = new IList<tclsBlobElement>[ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX];
            mlstlstau32CalibrationData = new List<List<UInt32[]>>();
            mlstlstau32AxisData = new List<List<UInt32[]>>();
            mlstlstau32BlobData = new List<List<UInt32[]>>();

            for (int iListIDX = 0; iListIDX < ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX; iListIDX++ )
            {
                List<UInt32[]> lstau32CalibrationData = new List<UInt32[]>();
                List<UInt32[]> lstau32AxisData = new List<UInt32[]>();
                List<UInt32[]> lstau32BlobData = new List<UInt32[]>();
                mlstlstau32CalibrationData.Add(lstau32CalibrationData);
                mlstlstau32AxisData.Add(lstau32AxisData);
                mlstlstau32BlobData.Add(lstau32BlobData);
                malstCalibrationLists[iListIDX] = new List<tclsCalibrationElement>();
                mailstCalibrationLists[iListIDX] = malstCalibrationLists[iListIDX].AsReadOnly();
                malstAxisLists[iListIDX] = new List<tclsAxisElement>();
                mailstAxisLists[iListIDX] = malstAxisLists[iListIDX].AsReadOnly();
                malstBlobLists[iListIDX] = new List<tclsBlobElement>();
                mailstBlobLists[iListIDX] = malstBlobLists[iListIDX].AsReadOnly();
            }

            mszReturnString = "Errors: none";
        }

        public bool boReadCalibrationFile(String szXMLCalibrationFileName, bool boReLoadData, bool boForceReload)
        {
            int iSheetIndex = 0;
            tclsCalibrationElement clsCalibrationElement;
            tclsAxisElement clsAxisPtsElement;
            tclsBlobElement clsBlobElement;
            bool boSheetFound = true;
            bool boRetVal = false;
            UInt32 u32DataIDX;
            bool boFileReadyToReadWrite = true;
            String szCalTemplateLatestXML;

            szCalTemplateLatestXML = GetCalTemplateLatest();

            try
            {
                if (false == File.Exists(szXMLCalibrationFileName))
                {
                    try
                    {
                        File.Copy(szCalTemplateLatestXML, szXMLCalibrationFileName);
                        File.SetAttributes(szXMLCalibrationFileName, File.GetAttributes(szXMLCalibrationFileName) & ~FileAttributes.ReadOnly);
                    }
                    catch
                    {
                        Program.vNotifyProgramEvent(tenProgramEvent.enProgramError, 0, "Template file missing");
                        boFileReadyToReadWrite = false;
                    }
                }


                if (((0 != String.Compare(szXMLCalibrationFileName, mszCalibrationFileCurrent)) ||
                    (true == boForceReload)) && (true == boFileReadyToReadWrite))
                {
                    /* Clear all the descriptor lists and data ready to load unknown file */
                    for (int iListIDX = 0; iListIDX < ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX; iListIDX++)
                    {
                        mlstlstau32CalibrationData[iListIDX].Clear();
                        mlstlstau32AxisData[iListIDX].Clear();
                        mlstlstau32BlobData[iListIDX].Clear();
                        malstCalibrationLists[iListIDX].Clear();
                        malstAxisLists[iListIDX].Clear();
                        malstBlobLists[iListIDX].Clear();
                    }

                    mClsXDoc.Load(szXMLCalibrationFileName);
                    XmlNamespaceManager clsXMLNS = new XmlNamespaceManager(mClsXDoc.NameTable);
                    XmlElement clsXRoot = mClsXDoc.DocumentElement;
                    clsXMLNS.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

                    while (true == boSheetFound)
                    {
                        String szXPath = "//ss:Worksheet[" + Convert.ToString(iSheetIndex + 1) + "]//ss:Row";
                        XmlNodeList lstXMLElementNodes = clsXRoot.SelectNodes(szXPath, clsXMLNS);

                        if (0 < lstXMLElementNodes.Count)
                        {
                            boRetVal = true;

                            foreach (XmlNode clsXMLNode in lstXMLElementNodes)
                            {
                                XmlNodeList lstXMLElementConfigNodes = clsXMLNode.ChildNodes;
                                List<String> lstNodeString = new List<String>();

                                foreach (XmlNode clsXMLNodeConfig in lstXMLElementConfigNodes)
                                {
                                    lstNodeString.Add(clsXMLNodeConfig.InnerText);
                                }

                                if (0 != String.Compare(lstNodeString[0], "data", true))
                                {
                                    if (0 == String.Compare(lstNodeString[1], "0", true))
                                    {
                                        clsCalibrationElement = new tclsCalibrationElement(lstNodeString);
                                        UInt32[] au32Data = new UInt32[clsCalibrationElement.u32DataPointCount];

                                        if (((lstNodeString.Count - 3) >= clsCalibrationElement.u32DataPointCount) &&
                                            (true == boReLoadData))
                                        {
                                            u32DataIDX = 0;

                                            while (clsCalibrationElement.u32DataPointCount > u32DataIDX)
                                            {
                                                if (0 == lstNodeString[3 + (int)u32DataIDX].IndexOf("-"))
                                                {
                                                    Int32 sint32 = Convert.ToInt32(lstNodeString[3 + (int)u32DataIDX]);
                                                    au32Data[u32DataIDX] = (UInt32)sint32;
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        au32Data[u32DataIDX] = Convert.ToUInt32(lstNodeString[3 + (int)u32DataIDX]);
                                                    }
                                                    catch
                                                    {
                                                        au32Data[u32DataIDX] = 0;
                                                    }
                                                }
                                                u32DataIDX++;
                                            }
                                        }

                                        malstCalibrationLists[iSheetIndex].Add(clsCalibrationElement);
                                        mlstlstau32CalibrationData[iSheetIndex].Add(au32Data);
                                    }
                                    if (0 == String.Compare(lstNodeString[1], "1", true))
                                    {
                                        clsAxisPtsElement = new tclsAxisElement(lstNodeString);
                                        UInt32[] au32Data = new UInt32[clsAxisPtsElement.u32DataPointCount];
                                        malstAxisLists[iSheetIndex].Add(clsAxisPtsElement);
                                        mlstlstau32AxisData[iSheetIndex].Add(au32Data);
                                    }
                                    if (0 == String.Compare(lstNodeString[1], "2", true))
                                    {
                                        clsBlobElement = new tclsBlobElement(lstNodeString);
                                        UInt32[] au32Data = new UInt32[clsBlobElement.u32PointCount];
                                        malstBlobLists[iSheetIndex].Add(clsBlobElement);
                                        mlstlstau32BlobData[iSheetIndex].Add(au32Data);
                                    }
                                }
                            }
                            iSheetIndex++;
                        }
                        else
                        {
                            boSheetFound = false;
                            //boRetVal = false;
                        }
                    }

                    for (int iListIDX = 0; iListIDX < ConstantData.GUI.ru8GUI_CAL_GROUPS_MAX; iListIDX++ )
                    {
                        mailstCalibrationLists[iListIDX] = malstCalibrationLists[iListIDX].AsReadOnly();
                    }
                 
                    for (int iListIDX = 0; iListIDX < ConstantData.GUI.ru8GUI_AXIS_GROUPS_MAX; iListIDX++)
                    {
                        mailstAxisLists[iListIDX] = malstAxisLists[iListIDX].AsReadOnly();
                    }

                    for (int iListIDX = 0; iListIDX < ConstantData.GUI.ru8GUI_AXIS_GROUPS_MAX; iListIDX++)
                    {
                        mailstBlobLists[iListIDX] = malstBlobLists[iListIDX].AsReadOnly();
                    }

                    mszCalibrationFileCurrent = szXMLCalibrationFileName;
                }
                else
                {
                    boRetVal = true;
                }
            }
            catch
            {

            }
            
            return boRetVal;
        }


        bool boPrepareWriteCalibration(String szXMLCalibrationFileName)
        {
            int iSheetIndex = 0;
            bool boRetVal = true;
            bool boSheetFound = true;

            mClsXDoc.Load(szXMLCalibrationFileName);
            XmlNamespaceManager clsXMLNS = new XmlNamespaceManager(mClsXDoc.NameTable);
            XmlElement clsXRoot = mClsXDoc.DocumentElement;
            clsXMLNS.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

            while (true == boSheetFound)
            {
                String szXPath = "//ss:Worksheet[" + Convert.ToString(iSheetIndex + 1) + "]//ss:Row";
                XmlNodeList lstXMLElementNodes = clsXRoot.SelectNodes(szXPath, clsXMLNS);

                if (0 < lstXMLElementNodes.Count)
                {
                    boRetVal = true;

                    foreach (XmlNode clsXMLNode in lstXMLElementNodes)
                    {
                        XmlNodeList lstXMLElementConfigNodes = clsXMLNode.ChildNodes;
                        List<String> lstNodeString = new List<String>();

                        foreach (XmlNode clsXMLNodeConfig in lstXMLElementConfigNodes)
                        {
                            lstNodeString.Add(clsXMLNodeConfig.InnerText);
                        }

                        if (0 != String.Compare(lstNodeString[0], "name", true))
                        {
                            if (0 == String.Compare(lstNodeString[1], "0", true))
                            {
                                int iSheetIDX = 0;
                                int iListIDX = 0;

                                foreach (List<tclsCalibrationElement> lstCalibrationElements in malstCalibrationLists)
                                { 
                                    foreach (tclsCalibrationElement clsCalibrationElement in lstCalibrationElements)
                                    {
                                        if (0 == String.Compare(clsCalibrationElement.szCharacteristicName, lstNodeString[0]))
                                        {
                                            for (int iIDX = 0; iIDX < mlstlstau32CalibrationData[iSheetIDX][iListIDX].Length; iIDX++)
                                            { 
                                                String szOldInnerXML = clsXMLNode.ChildNodes[iIDX + 3].InnerXml;
                                                String szOldInnerText = clsXMLNode.ChildNodes[iIDX + 3].InnerText;

                                                if ((UInt32.MaxValue / 2) > mlstlstau32CalibrationData[iSheetIDX][iListIDX][iIDX])
                                                {
                                                    clsXMLNode.ChildNodes[iIDX + 3].InnerXml = clsXMLNode.ChildNodes[iIDX + 3].InnerXml.Replace(szOldInnerText, mlstlstau32CalibrationData[iSheetIDX][iListIDX][iIDX].ToString());
                                                }
                                                else
                                                {
                                                    UInt32 u32Temp = UInt32.MaxValue - mlstlstau32CalibrationData[iSheetIDX][iListIDX][iIDX] + 1;
                                                    Int32 s32Temp = -1 * (Int32)u32Temp;
                                                    clsXMLNode.ChildNodes[iIDX + 3].InnerXml = clsXMLNode.ChildNodes[iIDX + 3].InnerXml.Replace(szOldInnerText, s32Temp.ToString());
                                                }
                                            }                                            
                                        }

                                        iListIDX++;
                                    }

                                    iSheetIDX++;
                                }
                            }
                            if (0 == String.Compare(lstNodeString[1], "1", true))
                            {
                                int iSheetIDX = 0;
                                int iListIDX = 0;

                                foreach (List<tclsCalibrationElement> lstCalibrationElements in malstCalibrationLists)
                                {
                                    foreach (tclsCalibrationElement clsCalibrationElement in lstCalibrationElements)
                                    {
                                        if (0 == String.Compare(clsCalibrationElement.szCharacteristicName, lstNodeString[0]))
                                        {
                                            for (int iIDX = 0; iIDX < mlstlstau32CalibrationData[iSheetIDX][iListIDX].Length; iIDX++)
                                            {
                                                String szOldInnerXML = clsXMLNode.ChildNodes[iIDX + 3].InnerXml;
                                                String szOldInnerText = clsXMLNode.ChildNodes[iIDX + 3].InnerText;

                                                if ((UInt32.MaxValue / 2) > mlstlstau32CalibrationData[iSheetIDX][iListIDX][iIDX])
                                                {
                                                    clsXMLNode.ChildNodes[iIDX + 3].InnerXml = clsXMLNode.ChildNodes[iIDX + 3].InnerXml.Replace(szOldInnerText, mlstlstau32CalibrationData[iSheetIDX][iListIDX][iIDX].ToString());
                                                }
                                                else
                                                {
                                                    UInt32 u32Temp = UInt32.MaxValue - mlstlstau32CalibrationData[iSheetIDX][iListIDX][iIDX] + 1;
                                                    Int32 s32Temp = -1 * (Int32)u32Temp;
                                                    clsXMLNode.ChildNodes[iIDX + 3].InnerXml = clsXMLNode.ChildNodes[iIDX + 3].InnerXml.Replace(szOldInnerText, s32Temp.ToString());
                                                }
                                            }
                                        }

                                        iListIDX++;
                                    }

                                    iSheetIDX++;
                                }

                                iSheetIDX = 0;
                                iListIDX = 0;

                                foreach (List<tclsAxisElement> lstAxisElements in malstAxisLists)
                                {
                                    foreach (tclsAxisElement clsAxisElement in lstAxisElements)
                                    {
                                        if (0 == String.Compare(clsAxisElement.szAxisName, lstNodeString[0]))
                                        {
                                            for (int iIDX = 0; iIDX < mlstlstau32AxisData[iSheetIDX][iListIDX].Length; iIDX++)
                                            {
                                                String szOldInnerXML = clsXMLNode.ChildNodes[iIDX + 3].InnerXml;
                                                String szOldInnerText = clsXMLNode.ChildNodes[iIDX + 3].InnerText;

                                                if ((UInt32.MaxValue / 2) > mlstlstau32AxisData[iSheetIDX][iListIDX][iIDX])
                                                {
                                                    clsXMLNode.ChildNodes[iIDX + 3].InnerXml = clsXMLNode.ChildNodes[iIDX + 3].InnerXml.Replace(szOldInnerText, mlstlstau32AxisData[iSheetIDX][iListIDX][iIDX].ToString());
                                                }
                                                else
                                                {
                                                    UInt32 u32Temp = UInt32.MaxValue - mlstlstau32AxisData[iSheetIDX][iListIDX][iIDX] + 1;
                                                    Int32 s32Temp = -1 * (Int32)u32Temp;
                                                    clsXMLNode.ChildNodes[iIDX + 3].InnerXml = clsXMLNode.ChildNodes[iIDX + 3].InnerXml.Replace(szOldInnerText, s32Temp.ToString());
                                                }
                                            }
                                        }

                                        iListIDX++;
                                    }

                                    iSheetIDX++;
                                }


                            }
                            if (0 == String.Compare(lstNodeString[1], "2", true))
                            {
                                int iSheetIDX = 0;
                                int iListIDX = 0;

                                foreach (List<tclsBlobElement> lstBlobElements in malstBlobLists)
                                {
                                    foreach (tclsBlobElement clsBlobElement in lstBlobElements)
                                    {
                                        if (0 == String.Compare(clsBlobElement.szBlobName, lstNodeString[0]))
                                        {
                                            for (int iIDX = 0; iIDX < mlstlstau32BlobData[iSheetIDX][iListIDX].Length; iIDX++)
                                            {
                                                String szOldInnerXML = clsXMLNode.ChildNodes[iIDX + 3].InnerXml;
                                                String szOldInnerText = clsXMLNode.ChildNodes[iIDX + 3].InnerText;

                                                if ((UInt32.MaxValue / 2) > mlstlstau32BlobData[iSheetIDX][iListIDX][iIDX])
                                                {
                                                    clsXMLNode.ChildNodes[iIDX + 3].InnerXml = clsXMLNode.ChildNodes[iIDX + 3].InnerXml.Replace(szOldInnerText, mlstlstau32BlobData[iSheetIDX][iListIDX][iIDX].ToString());
                                                }
                                                else
                                                {
                                                    UInt32 u32Temp = UInt32.MaxValue - mlstlstau32CalibrationData[iSheetIDX][iListIDX][iIDX] + 1;
                                                    Int32 s32Temp = -1 * (Int32)u32Temp;
                                                    clsXMLNode.ChildNodes[iIDX + 3].InnerXml = clsXMLNode.ChildNodes[iIDX + 3].InnerXml.Replace(szOldInnerText, s32Temp.ToString());
                                                }
                                            }
                                        }

                                        iListIDX++;
                                    }

                                    iSheetIDX++;
                                }


                            }
                        }
                    }
                    iSheetIndex++;
                }
                else
                {
                    boSheetFound = false;
                    boRetVal = false;
                }
            }

            return boRetVal;
        }

        public bool boWriteCalibrationFile(String szXMLCalibrationFileName)
        {
            bool boRetVal = true;
            int iCharIDX = 0;
            int iBlobIDX = 0;
            int iAxisIDX = 0;

            boRetVal = boReadCalibrationFile(szXMLCalibrationFileName, false, false);

            if (true == boRetVal)
            {
                while (tclsASAM.milstCharacteristicList.Count > iCharIDX)
                {
                    vUpdateCalibrationFromDataPage(iCharIDX);
                    iCharIDX++;
                }

                while (tclsASAM.milstBlobList.Count > iBlobIDX)
                {
                    vUpdateBlobFromDataPage(iBlobIDX);
                    iBlobIDX++;
                }

                while (tclsASAM.milstAxisPtsList.Count > iAxisIDX)
                {
                    vUpdateAxisFromDataPage(iAxisIDX);
                    iAxisIDX++;
                }

                boPrepareWriteCalibration(szXMLCalibrationFileName);

                if (null != mClsXDoc)
                {
                    mClsXDoc.Save(szXMLCalibrationFileName);
                }
            }

            return boRetVal;
        }

        void vUpdateCalibrationFromDataPage(int iCharIDX)
        {
            UInt32 u32Address = tclsASAM.milstCharacteristicList[iCharIDX].u32Address;
            String szCharName = tclsASAM.milstCharacteristicList[iCharIDX].szCharacteristicName;
            int iPointsCount;
            int iCharPos;

            iCharPos = szCharName.IndexOf("_");


            if (0 != iCharPos)
            {
                if ((-1 == tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef) && (-1 != tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef))
                {
                    iPointsCount = tclsASAM.milstAxisDescrList[tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef].iAxisPointCount;
                }
                else if ((-1 == tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef) && (-1 != tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef))
                {
                    iPointsCount = tclsASAM.milstAxisDescrList[tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef].iAxisPointCount;
                }
                else if ((-1 != tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef) && (-1 != tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef))
                {
                    iPointsCount = tclsASAM.milstAxisDescrList[tclsASAM.milstCharacteristicList[iCharIDX].iYAxisRef].iAxisPointCount *
                                   tclsASAM.milstAxisDescrList[tclsASAM.milstCharacteristicList[iCharIDX].iXAxisRef].iAxisPointCount;
                }
                else
                {
                    iPointsCount = 1;
                }

                UInt32[] au32Data = new UInt32[iPointsCount];

                switch (tclsASAM.milstCharacteristicList[iCharIDX].enRecLayout)
                {
                    case tenRecLayout.enRL_VALU8:
                        {
                            byte[] au8Data = new byte[iPointsCount];
                            tclsDataPage.au8GetWorkingData(u32Address, ref au8Data);
                            au32Data = Array.ConvertAll(au8Data, Convert.ToUInt32);
                            break;
                        }
                    case tenRecLayout.enRL_VALU16:
                        {
                            UInt16[] au16Data = new ushort[iPointsCount];
                            tclsDataPage.au16GetWorkingData(u32Address, ref au16Data);
                            au32Data = Array.ConvertAll(au16Data, Convert.ToUInt32);
                            break;
                        }
                    case tenRecLayout.enRL_VALU32:
                        {
                            tclsDataPage.au32GetWorkingData(u32Address, ref au32Data);
                            break;
                        }
                    case tenRecLayout.enRL_VALS8:
                        {
                            byte[] au8Data = new byte[iPointsCount];
                            tclsDataPage.au8GetWorkingData(u32Address, ref au8Data);
                            au32Data = Array.ConvertAll(au8Data, Convert.ToUInt32);
                            break;
                        }
                    case tenRecLayout.enRL_VALS16:
                        {
                            Int16[] as16Data = new short[iPointsCount];
                            tclsDataPage.as16GetWorkingData(u32Address, ref as16Data);

                            for (int iIDX = 0; iIDX < iPointsCount; iIDX++)
                            {
                                au32Data[iIDX] = (UInt32)as16Data[iIDX];
                            }
                            break;
                        }
                    case tenRecLayout.enRL_VALS32:
                        {
                            Int32[] as32Data = new Int32[iPointsCount];
                            tclsDataPage.as32GetWorkingData(u32Address, ref as32Data);

                            for (int iIDX = 0; iIDX < iPointsCount; iIDX++)
                            {
                                au32Data[iIDX] = (UInt32)as32Data[iIDX];
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                vSetCharData(tclsASAM.milstCharacteristicList[iCharIDX].szCharacteristicName, au32Data);
            }
        }

        void vUpdateAxisFromDataPage(int iAxisIDX)
        {
            UInt32 u32Address = tclsASAM.milstAxisPtsList[iAxisIDX].u32Address;
            String szAxisName = tclsASAM.milstAxisPtsList[iAxisIDX].szAxisPtsName;
            int iPointsCount = tclsASAM.milstAxisPtsList[iAxisIDX].iAxisPointCount;

            UInt32[] au32Data = new UInt32[iPointsCount];

            switch (tclsASAM.milstAxisPtsList[iAxisIDX].enRecLayout)
            {
                case tenRecLayout.enRL_VALU8:
                    {
                        byte[] au8Data = new byte[iPointsCount];
                        tclsDataPage.au8GetWorkingData(u32Address, ref au8Data);
                        au32Data = Array.ConvertAll(au8Data, Convert.ToUInt32);
                        break;
                    }
                case tenRecLayout.enRL_VALU16:
                    {
                        UInt16[] au16Data = new ushort[iPointsCount];
                        tclsDataPage.au16GetWorkingData(u32Address, ref au16Data);
                        au32Data = Array.ConvertAll(au16Data, Convert.ToUInt32);
                        break;
                    }
                case tenRecLayout.enRL_VALU32:
                    {
                        tclsDataPage.au32GetWorkingData(u32Address, ref au32Data);
                        break;
                    }
                case tenRecLayout.enRL_VALS8:
                    {
                        byte[] au8Data = new byte[iPointsCount];
                        tclsDataPage.au8GetWorkingData(u32Address, ref au8Data);
                        au32Data = Array.ConvertAll(au8Data, Convert.ToUInt32);
                        break;
                    }
                case tenRecLayout.enRL_VALS16:
                    {
                        Int16[] as16Data = new short[iPointsCount];
                        tclsDataPage.as16GetWorkingData(u32Address, ref as16Data);

                        for (int iIDX = 0; iIDX < iPointsCount; iIDX++)
                        {
                            au32Data[iIDX] = (UInt32)as16Data[iIDX];
                        }
                        break;
                    }
                case tenRecLayout.enRL_VALS32:
                    {
                        Int32[] as32Data = new Int32[iPointsCount];
                        tclsDataPage.as32GetWorkingData(u32Address, ref as32Data);

                        for (int iIDX = 0; iIDX < iPointsCount; iIDX++)
                        {
                            au32Data[iIDX] = (UInt32)as32Data[iIDX];
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            vSetAxisData(tclsASAM.milstAxisPtsList[iAxisIDX].szAxisPtsName, au32Data);
        }

        void vUpdateBlobFromDataPage(int iBlobIDX)
        {
            UInt32 u32Address = tclsASAM.milstBlobList[iBlobIDX].u32Address;
            int iPointsCount;

            iPointsCount = tclsASAM.milstBlobList[iBlobIDX].iPointCount;

            UInt32[] au32Data = new UInt32[iPointsCount];

            switch (tclsASAM.milstBlobList[iBlobIDX].enRecLayout)
            {
                case tenRecLayout.enRL_VALU8:
                    {
                        byte[] au8Data = new byte[iPointsCount];
                        tclsDataPage.au8GetWorkingData(u32Address, ref au8Data);
                        au32Data = Array.ConvertAll(au8Data, Convert.ToUInt32);
                        break;
                    }
                case tenRecLayout.enRL_VALU16:
                    {
                        UInt16[] au16Data = new ushort[iPointsCount];
                        tclsDataPage.au16GetWorkingData(u32Address, ref au16Data);
                        au32Data = Array.ConvertAll(au16Data, Convert.ToUInt32);
                        break;
                    }
                case tenRecLayout.enRL_VALU32:
                    {

                        tclsDataPage.au32GetWorkingData(u32Address, ref au32Data);
                        break;
                    }
                case tenRecLayout.enRL_VALS8:
                    {
                        byte[] au8Data = new byte[iPointsCount];
                        tclsDataPage.au8GetWorkingData(u32Address, ref au8Data);
                        au32Data = Array.ConvertAll(au8Data, Convert.ToUInt32);
                        break;
                    }
                case tenRecLayout.enRL_VALS16:
                    {
                        Int16[] as16Data = new short[iPointsCount];
                        tclsDataPage.as16GetWorkingData(u32Address, ref as16Data);

                        for (int iIDX = 0; iIDX < iPointsCount; iIDX++)
                        {
                            au32Data[iIDX] = (UInt32)as16Data[iIDX];                   
                        }
                        break;
                    }
                case tenRecLayout.enRL_VALS32:
                    {
                        Int32[] as32Data = new Int32[iPointsCount];
                        tclsDataPage.as32GetWorkingData(u32Address, ref as32Data);

                        for (int iIDX = 0; iIDX < iPointsCount; iIDX++)
                        {
                            au32Data[iIDX] = (UInt32)as32Data[iIDX];
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            vSetBlobData(tclsASAM.milstBlobList[iBlobIDX].szBlobName, au32Data);
        }


        public UInt32[] u32GetCharData(String szCharName)
        {
            UInt32[] au32Data = null;
            int iTableIDX = 0;
            int iListIDX = 0;

            foreach (List<tclsCalibrationElement> lstCalElements in malstCalibrationLists)
            {
                if (0 < lstCalElements.Count)
                {
                    foreach (tclsCalibrationElement calElement in lstCalElements)
                    {
                        if (0 == String.Compare(calElement.szCharacteristicName, szCharName))
                        {
                            au32Data = new UInt32[calElement.u32DataPointCount];
                            Array.Copy(mlstlstau32CalibrationData[iTableIDX][iListIDX], au32Data, calElement.u32DataPointCount);
                        }

                        iListIDX++;
                    }
                }

                iTableIDX++;
            }

            return au32Data;
        }


        public void vSetAxisData(String szAxisName, UInt32[] au32Data)
        {
            int iListArrayIDX = 0;
            int iListIDX = 0;
            int iListArrayFoundIDX = -1;
            int iListFoundIDX = -1;

            if (null != malstAxisLists)
            {
                while (malstAxisLists.Length > iListArrayIDX)
                {
                    List<tclsAxisElement> lstAxisElements = malstAxisLists[iListArrayIDX];
                    iListIDX = 0;

                    while (lstAxisElements.Count > iListIDX)
                    {
                        if (0 == String.Compare(lstAxisElements[iListIDX].szAxisName, szAxisName))
                        {
                            iListArrayFoundIDX = iListArrayIDX;
                            iListFoundIDX = iListIDX;
                        }

                        iListIDX++;
                    }

                    iListArrayIDX++;
                }
            }

            List<string> aszAxisStrings = new List<string>();
            int iDataIDX;

            if ((-1 != iListArrayFoundIDX) && (-1 != iListFoundIDX))
            {
                for (iDataIDX = 0; iDataIDX < malstAxisLists[iListArrayFoundIDX][iListFoundIDX].u32DataPointCount; iDataIDX++)
                {
                    mlstlstau32AxisData[iListArrayFoundIDX][iListFoundIDX][iDataIDX] = au32Data[iDataIDX];
                }
            }
        }

        public void vSetCharData(String szCharName, UInt32[] au32Data)
        {
            int iListArrayIDX = 0;
            int iListIDX = 0;
            int iListArrayFoundIDX = -1;
            int iListFoundIDX = -1;

            if (null != malstCalibrationLists)
            {
                while (malstCalibrationLists.Length > iListArrayIDX)
                {
                    List<tclsCalibrationElement> lstCalElements = malstCalibrationLists[iListArrayIDX];
                    iListIDX = 0;

                    while (lstCalElements.Count > iListIDX)
                    {
                            if (0 == String.Compare(lstCalElements[iListIDX].szCharacteristicName, szCharName))
                            {
                                iListArrayFoundIDX = iListArrayIDX;
                                iListFoundIDX = iListIDX;
                            }

                        iListIDX++;
                    }

                    iListArrayIDX++;
                }
            }

            List<string> aszCharStrings = new List<string>();
            int iDataIDX;

            if ((-1 != iListArrayFoundIDX) && (-1 != iListFoundIDX))
            {
                for (iDataIDX = 0; iDataIDX < malstCalibrationLists[iListArrayFoundIDX][iListFoundIDX].u32DataPointCount; iDataIDX++)
                {
                    mlstlstau32CalibrationData[iListArrayFoundIDX][iListFoundIDX][iDataIDX] = au32Data[iDataIDX];
                }
            }
        }

        public void vSetBlobData(String szBlobName, UInt32[] au32Data)
        {
            int iListArrayIDX = 0;
            int iListIDX = 0;
            int iListArrayFoundIDX = -1;
            int iListFoundIDX = -1;

            if (null != malstBlobLists)
            {
                while (malstBlobLists.Length > iListArrayIDX)
                {
                    List<tclsBlobElement> lstBlobElements = malstBlobLists[iListArrayIDX];
                    iListIDX = 0;

                    while (lstBlobElements.Count > iListIDX)
                    {
                        if (0 == String.Compare(lstBlobElements[iListIDX].szBlobName, szBlobName))
                        {
                            iListArrayFoundIDX = iListArrayIDX;
                            iListFoundIDX = iListIDX;
                            break;
                        }

                        iListIDX++;
                    }

                    iListArrayIDX++;
                }
            }

            List<string> aszCharStrings = new List<string>();
            int iDataIDX;

            if ((-1 != iListArrayFoundIDX) && (-1 != iListFoundIDX))
            {
                for (iDataIDX = 0; iDataIDX < malstBlobLists[iListArrayFoundIDX][iListFoundIDX].u32PointCount; iDataIDX++)
                {
                    mlstlstau32BlobData[iListArrayFoundIDX][iListFoundIDX][iDataIDX] = au32Data[iDataIDX];
                }
            }
        }




        public UInt32[] u32GetAxisData(String szAxisName)
        {
            UInt32[] au32Data = null;

            foreach (List<tclsAxisElement> lstAxisElements in malstAxisLists)
            {
                if (0 < lstAxisElements.Count)
                {
                    foreach (tclsAxisElement axisElement in lstAxisElements)
                    {
                        if (0 == String.Compare(axisElement.szAxisName, szAxisName))
                        {
                            au32Data = new UInt32[axisElement.u32DataPointCount];

                            if (null != axisElement.au32Data)
                            {
                                Array.Copy(axisElement.au32Data, au32Data, axisElement.u32DataPointCount);
                            }
                        }
                    }
                }
            }

            return au32Data;
        }

        public UInt32[] u32GetBlobData(String szBlobName)
        {
            UInt32[] au32Data = null;

            foreach (List<tclsBlobElement> lstBlobElements in malstBlobLists)
            {
                if (0 < lstBlobElements.Count)
                {
                    foreach (tclsBlobElement blobElement in lstBlobElements)
                    {
                        if (0 == String.Compare(blobElement.szBlobName, szBlobName))
                        {
                            au32Data = new UInt32[blobElement.u32PointCount];

                            if (null != blobElement.au32Data)
                            {
                                Array.Copy(blobElement.au32Data, au32Data, blobElement.u32PointCount);
                            }
                        }
                    }
                }
            }

            return au32Data;
        }

        String GetCalTemplateLatest()
        {
            string[] aszXMLFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Database\\Calibration Databases", "*.XML");
            int iVersionLatest = 0;
            String szFilePath = null;

            foreach (String XMLFile in aszXMLFiles)
            {
                if (XMLFile.Contains("CAL_TEMPLATE_V1_"))
                {
                    string szFile = XMLFile.Replace("Database\\Calibration Databases", "^");
                    int iCharPos = szFile.IndexOf('^') + 2;
                    szFile = szFile.Substring(iCharPos);

                    String verString = szFile.Substring(16);
                    int iVer = 0;

                    try
                    {
                        verString = verString.Replace(".XML", "");
                        verString = verString.Replace(".xml", "");
                        iVer = Convert.ToInt16(verString);
                    }
                    catch
                    {

                    }

                    if (0 != iVer)
                    {
                        if (iVer > iVersionLatest)
                        {
                            iVersionLatest = iVer;
                            szFilePath = XMLFile;
                        }
                    }
                }
            }

            return szFilePath;
        }
    }

    public class tclsCalibrationElement
    {
        public string szCharacteristicName;
        public UInt32 u32DataPointCount;
        private UInt32 u32DataIDX = 0;

        public tclsCalibrationElement(List<String> lstInitString)
        {
            szCharacteristicName = lstInitString[0];
            u32DataPointCount = Convert.ToUInt32(lstInitString[2]);
        }
    }

    public class tclsAxisElement
    {
        public string szAxisName;
        public UInt32 u32DataPointCount;
        public UInt32[] au32Data;
        private UInt32 u32DataIDX = 0;

        public tclsAxisElement(List<String> lstInitString)
        {
            szAxisName = lstInitString[0];
            u32DataPointCount = Convert.ToUInt32(lstInitString[2]);

            if ((lstInitString.Count - 3) >= u32DataPointCount)
            {
                au32Data = new UInt32[u32DataPointCount];

                while (u32DataPointCount > u32DataIDX)
                {
                    if (0 == lstInitString[3 + (int)u32DataIDX].IndexOf("-"))
                    {
                        Int32 sint32 = Convert.ToInt32(lstInitString[3 + (int)u32DataIDX]);
                        au32Data[u32DataIDX] = (UInt32)sint32;
                    }
                    else
                    {
                        au32Data[u32DataIDX] = Convert.ToUInt32(lstInitString[3 + (int)u32DataIDX]);
                    }
                    u32DataIDX++;
                }
            }
        }
    }

    public class tclsBlobElement
    {
        public string szBlobName;
        public UInt32 u32PointCount;
        public UInt32[] au32Data;
        private UInt32 u32DataIDX = 0;

        public tclsBlobElement(List<String> lstInitString)
        {
            szBlobName = lstInitString[0];
            u32PointCount = Convert.ToUInt32(lstInitString[2]);

            if ((lstInitString.Count - 3) >= u32PointCount)
            {
                au32Data = new UInt32[u32PointCount];

                while (u32PointCount > u32DataIDX)
                {
                    if (0 == lstInitString[3 + (int)u32DataIDX].IndexOf("-"))
                    {
                        Int32 sint32 = Convert.ToInt32(lstInitString[3 + (int)u32DataIDX]);
                        au32Data[u32DataIDX] = (UInt32)sint32;
                    }
                    else
                    {
                        au32Data[u32DataIDX] = Convert.ToUInt32(lstInitString[3 + (int)u32DataIDX]);
                    }
                    u32DataIDX++;
                }
            }
        }
    }
}
