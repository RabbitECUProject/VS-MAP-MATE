/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Config Reader                                          */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsConfigReader.cs                                    */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace UDP
{
    class tclsXMLConfigReader
    {
        XmlDocument clsXDoc = new XmlDocument();
        List<tclsWindowElement>[] malstWindowLists;
        public IList<tclsWindowElement>[] mailstWindowLists;

        public tclsXMLConfigReader()
        {
            malstWindowLists = new List<tclsWindowElement>[ConstantData.GUI.ru8GUI_WINDOWS_MAX];
            mailstWindowLists = new IList<tclsWindowElement>[ConstantData.GUI.ru8GUI_WINDOWS_MAX];

            for (int iListIDX = 0; iListIDX < ConstantData.GUI.ru8GUI_WINDOWS_MAX; iListIDX++ )
            {
                malstWindowLists[iListIDX] = new List<tclsWindowElement>();
                mailstWindowLists[iListIDX] = malstWindowLists[iListIDX].AsReadOnly();
            }       
        }

        public bool ReadWindowConfigFile(String szXMLConfigFileName)
        {
            int iSheetIndex = 0;
            tclsWindowElement clsWinElement;
            bool boSheetFound = true;

            try
            {
                clsXDoc.Load(szXMLConfigFileName);
                XmlNamespaceManager clsXMLNS = new XmlNamespaceManager(clsXDoc.NameTable);
                XmlElement clsXRoot = clsXDoc.DocumentElement;
                clsXMLNS.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

                while (true == boSheetFound)
                {
                    String szXPath = "//ss:Worksheet[" + Convert.ToString(iSheetIndex + 1) + "]//ss:Row";
                    XmlNodeList lstXMLElementNodes = clsXRoot.SelectNodes(szXPath, clsXMLNS);

                    if (1 < lstXMLElementNodes.Count)
                    {
                        foreach (XmlNode clsXMLNode in lstXMLElementNodes)
                        {
                            XmlNodeList lstXMLElementConfigNodes = clsXMLNode.ChildNodes;
                            List<String> lstNodeString = new List<String>();

                            foreach (XmlNode clsXMLNodeConfig in lstXMLElementConfigNodes)
                            {
                                lstNodeString.Add(clsXMLNodeConfig.InnerText);
                            }

                            int iPresentationOptionCount = lstNodeString.Count - 4;
                            String[] aszPresentationOptions = null;

                            if (0 < iPresentationOptionCount)
                            {
                                aszPresentationOptions = new String[iPresentationOptionCount];

                                for (int iStringIDX = 4; iStringIDX < (4 + iPresentationOptionCount); iStringIDX++)
                                {
                                    aszPresentationOptions[iStringIDX - 4] = lstNodeString[iStringIDX];
                                }
                            }

                            clsWinElement = new tclsWindowElement(lstNodeString[0],
                                Convert.ToInt16(lstNodeString[1]),
                                lstNodeString[2], lstNodeString[3], aszPresentationOptions);

                            malstWindowLists[iSheetIndex].Add(clsWinElement);
                        }
                        iSheetIndex++;
                    }
                    else
                    {
                        boSheetFound = false;
                    }
                }

                for (int iListIDX = 0; iListIDX < ConstantData.GUI.ru8GUI_WINDOWS_MAX; iListIDX++ )
                {
                    mailstWindowLists[iListIDX] = malstWindowLists[iListIDX].AsReadOnly();
                }  
            }
            catch
            {

            }
            
            return true;
        }
    }

    public class tclsWindowElement
    {
        public String szElementType;
        public int iGUILinkIndex;
        public String szLabel;
        public String szA2LName;
        public String[] aszPresentationOptions;

        public tclsWindowElement(String szElementType, int iGUILinkIndex, String szA2LName, String szLabel, String[] aszPresentationOptions)
        {
            this.szElementType = szElementType;
            this.iGUILinkIndex = iGUILinkIndex;
            this.szLabel = szLabel;
            this.szA2LName = szA2LName;
            this.aszPresentationOptions = aszPresentationOptions;
        }
    }
}
