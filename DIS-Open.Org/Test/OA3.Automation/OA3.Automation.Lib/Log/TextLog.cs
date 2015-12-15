//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OA3.Automation.Lib.Log
{
    /// <summary>
    /// Record log message into a txt file
    /// </summary>
    public class TextLog
    {
        //The path where thd log file created
        public static string logFilePath = "LOG_" + Helper.GetDateTimeString() + ".txt";

        public static void LogMessage(string message)
        {
            using (FileStream fs = File.Open(logFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                StreamWriter sw = new StreamWriter(fs);
                //Log message
                sw.WriteLine(DateTime.Now.ToString() + " : " + message);
                sw.Close();
            }
        }
    }
}
