using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace SuperRun
{
    class Program
    {
        static string ScriptPath = ConfigurationManager.AppSettings.Get("ScriptPath");
        static string ScriptArgs = ConfigurationManager.AppSettings.Get("ScriptArgs");
        static void Main(string[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                ScriptPath = args[0];
            }

            if (String.IsNullOrEmpty(ScriptPath))
            {
                Console.WriteLine("Script file name should not be null!");
            }
            else if (!File.Exists(ScriptPath))
            {
                Console.WriteLine("Script file \"{0}\" dose not exist!", ScriptPath);
            }
            else
            {
                string scriptFullPath = GetFullPath(ScriptPath);

                string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\"";

                string arguments = String.Format(argsTemp, scriptFullPath);

                if (!String.IsNullOrEmpty(ScriptArgs))
                {
                    arguments = String.Format("{0} {1}", arguments, ScriptArgs);
                }

                StartProcess("PowerShell", arguments, true, true);
            }

            Console.WriteLine("Press any key to exit...");

            Console.Read();
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
    }
}
