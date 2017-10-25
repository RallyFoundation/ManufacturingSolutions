using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Runtime.InteropServices;

namespace SuperRun
{
    class Program
    {
        static string ExePath = ConfigurationManager.AppSettings.Get("ExePath");
        static bool RequireTransactionID = ConfigurationManager.AppSettings.Get("RequireTransactionID") == "true";
        static bool RequireAppRootPath = ConfigurationManager.AppSettings.Get("RequireAppRootPath") == "true";
        static bool ShouldUseShortPath = ConfigurationManager.AppSettings.Get("ShouldUseShortPath") == "true";
        static string ScriptPath = ConfigurationManager.AppSettings.Get("ScriptPath");
        static string ScriptArgs = ConfigurationManager.AppSettings.Get("ScriptArgs");
        static string ArgsTemp = ConfigurationManager.AppSettings.Get("ArgumentTemplate");
        static bool ShouldExitOnComplete = (ConfigurationManager.AppSettings.Get("ShouldExitOnComplete") == "true");
        static bool ShouldShellExecute = (ConfigurationManager.AppSettings.Get("ShouldShellExecute") == "true");
        static bool ShouldCreateNewWindow = (ConfigurationManager.AppSettings.Get("ShouldCreateNewWindow") == "true");
        static string LogFilePathTemplate = ConfigurationManager.AppSettings.Get("LogFilePathTemplate");

        static string LogFilePath = "";

        static void Main(string[] args)
        {
            string transactionID = Guid.NewGuid().ToString();

            string scriptFullPath = GetFullPath(ScriptPath);

            string appRootPath = GetFullPath("\\");
            appRootPath = appRootPath.Substring(0, (appRootPath.Length - 1));

            if (ShouldUseShortPath)
            {
                appRootPath = GetShortPath(appRootPath);
            }

            string logFilePath = appRootPath + LogFilePathTemplate;

            logFilePath = String.Format(logFilePath, transactionID);

            if (String.IsNullOrEmpty(scriptFullPath))
            {
                Console.WriteLine("Script file name should not be null!");
            }
            else if (!File.Exists(scriptFullPath))
            {
                Console.WriteLine("Script file \"{0}\" dose not exist!", ScriptPath);
            }
            else
            {
                string argsTemp = ArgsTemp; //"-ExecutionPolicy ByPass -NoExit -File \"{0}\"";

                string arguments = "";

                if (RequireTransactionID)
                {
                    //arguments += " ";
                    //arguments += transactionID;

                    arguments = String.Format(argsTemp, scriptFullPath, transactionID);
                }
                else
                {
                    //arguments += " TRANS_ID_NULL";

                    arguments = String.Format(argsTemp, scriptFullPath);
                }

                //if (RequireAppRootPath)
                //{
                //    arguments += " ";
                //    arguments += appRootPath;
                //}
                //else
                //{
                //    arguments += " APP_ROOT_NULL";
                //}

                if ((args != null) && (args.Length > 0))
                {
                    if (!String.IsNullOrEmpty(arguments))
                    {
                        arguments += " ";
                    }

                    for (int i = 0; i < args.Length; i++)
                    {
                        arguments += args[i];

                        if (i != (args.Length - 1))
                        {
                            arguments += " ";
                        }
                    }
                }

                if (!String.IsNullOrEmpty(ScriptArgs))
                {
                    arguments = String.Format("{0} {1}", arguments, ScriptArgs);
                }

                //StartProcess(ExePath, arguments, true, true);

                StartProcess(ExePath, arguments, ShouldCreateNewWindow, ShouldShellExecute, logFilePath);
            }

            if (!ShouldExitOnComplete)
            {
                Console.WriteLine("Press any key to exit...");
                Console.Read();
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "GetShortPathNameA")]
        static extern int GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, int cchBuffer);

        static string GetShortPath(string longPath)
        {
            string shortPath = longPath;

            StringBuilder sPath = new StringBuilder(longPath.Length);
            GetShortPathName(longPath, sPath, longPath.Length);
            shortPath = sPath.ToString();

            return shortPath;
        }

        static string GetFullPath(string relativePath)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;

            if (rootPath.EndsWith("\\"))
            {
                rootPath = rootPath.Substring(0, (rootPath.Length - 1));
            }

            if (!relativePath.StartsWith("\\"))
            {
                relativePath = "\\" + relativePath;
            }

            return rootPath + relativePath;
        }

        static string StartProcess(string AppPath, string AppParams, bool IsCreatingNewWindow, bool IsUsingShellExecute)
        {
            Process process = new Process();

            process.StartInfo.FileName = AppPath;
            process.StartInfo.Arguments = AppParams;
            process.StartInfo.UseShellExecute = IsUsingShellExecute;
            process.StartInfo.RedirectStandardError = !IsUsingShellExecute;
            process.StartInfo.RedirectStandardOutput = !IsUsingShellExecute;
            process.StartInfo.CreateNoWindow = !IsCreatingNewWindow;

            process.Start();

            process.WaitForExit();

            string result = "";

            if (!IsUsingShellExecute)
            {
                using (process.StandardOutput)
                {
                    result = process.StandardOutput.ReadToEnd();
                }
            }

            return result;
        }

        static void StartProcess(string AppPath, string AppParams, bool IsCreatingNewWindow, bool IsUsingShellExecute, string LogFileFullPath)
        {
            Process process = new Process();

            process.StartInfo.FileName = AppPath;
            process.StartInfo.Arguments = AppParams;
            process.StartInfo.UseShellExecute = IsUsingShellExecute;
            process.StartInfo.RedirectStandardError = !IsUsingShellExecute;
            process.StartInfo.RedirectStandardOutput = !IsUsingShellExecute;
            process.StartInfo.CreateNoWindow = !IsCreatingNewWindow;

            if (!IsUsingShellExecute)
            {
                LogFilePath = LogFileFullPath;

                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
            }

            process.Start();

            process.BeginOutputReadLine();

            process.WaitForExit();

            //string result = "";

            //if (!IsUsingShellExecute)
            //{
            //    using (process.StandardOutput)
            //    {
            //        result = process.StandardOutput.ReadToEnd();
            //    }
            //}

            //return result;
        }

        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine(e.Data);

                using (StreamWriter writer = File.AppendText(LogFilePath))
                {
                    writer.WriteLine(e.Data);
                }
            }
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine(e.Data);
                //File.AppendAllText(LogFilePath, e.Data, Encoding.UTF8);

                using (StreamWriter writer = File.AppendText(LogFilePath))
                {
                    writer.WriteLine(e.Data);
                }
            }    
        }
    }
}
