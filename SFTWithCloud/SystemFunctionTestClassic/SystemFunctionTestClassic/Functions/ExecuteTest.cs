using DllLog;
//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Diagnostics;
using System.IO;
using win81FactoryTest.Setting;

namespace win81FactoryTest.Functions
{
    /// <summary>
    /// ExecuteTest: Static class that executes tests
    /// </summary>
    static class ExecuteTest
    {
        /// <summary>
        /// Executes the test [testName]
        /// </summary>
        public static bool Run(string testName)
        {
            string filePath = ConfigSettings.GetTestExes(testName);
            string args = ConfigSettings.GetTestArguments(testName);
            
            if (String.IsNullOrEmpty(filePath))
            {
                Log.LogError("RunTest: " + testName + "Empty file path");
                return false;
            }
            if (!File.Exists(filePath)) 
            {
                Log.LogError("RunTest: " + testName + " Failed - incorrect file path: " + filePath);
                return false;
            }

            Log.LogStart(testName + " - Inputs: " + args);
            Process proc = null;
            ProcessStartInfo startInfo = null;
            try
            {
                proc = new Process();

                startInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                proc.StartInfo = startInfo;
                
                proc.Start();
                proc.WaitForExit();
                int exitCode = proc.ExitCode;
                proc.Close();
                proc = null;
                startInfo = null;

                if (exitCode == 0)
                {
                    Log.LogPass(testName);
                    Log.LogFinish(testName);
                    return true;
                }
                else
                {
                    Log.LogFail(testName);
                    Log.LogFinish(testName);
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.LogError("RunTest: " + testName + " Failed - Exception Caught: " + e);
                Log.LogFinish(testName);
                return false;
            }
            finally
            {
                if (proc != null)
                {
                    proc.Dispose();
                }
            }

        }


    }
}
