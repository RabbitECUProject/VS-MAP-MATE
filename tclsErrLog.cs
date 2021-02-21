/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Error Log                                              */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsErrLog.cs                                          */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace UDP
{
    static class tclsErrlog
    {
        private static string szBaseLogDirectory;

        public static void OpenLog(string szBaseDirectory)
        {
            szBaseLogDirectory = szBaseDirectory;
            string szLogPath = szBaseDirectory + "\\start_log.txt";
            System.Reflection.Assembly clsAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo clsFVI = FileVersionInfo.GetVersionInfo(clsAssembly.Location);


            try
            {
                using (StreamWriter w = File.CreateText(szLogPath))
                {
                    w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                    w.WriteLine(ConstantData.APPCONFIG.szAppName + ": version = " + clsFVI.FileVersion);
                    w.WriteLine("-------------------------------------------");
                }
            }
            catch
            {
                MessageBox.Show("Error creating log file: " + szLogPath);
            }
        }


        public static void LogAppend(string logMessage)
        {
            string szLogPath = szBaseLogDirectory + "\\start_log.txt";

            using (StreamWriter w = File.AppendText(szLogPath))
            {
                try
                {
                    w.Write("\r\nLog Entry : " + logMessage);
                }
                catch
                {
                    MessageBox.Show("Error appending to log file: " + szLogPath);
                }

            }
        }
    }
}
