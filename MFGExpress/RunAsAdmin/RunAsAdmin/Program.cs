using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace RunAsAdmin
{
    class Program
    {
        static string ExePath = ConfigurationManager.AppSettings.Get("ExePath");
        static bool RequireTransactionID = ConfigurationManager.AppSettings.Get("RequireTransactionID") == "true";

        static void Main(string[] args)
        {
            string transactionID = Guid.NewGuid().ToString();

            string exeFullPath = GetFullPath(ExePath);

            string arguments = "";

            if (RequireTransactionID)
            {
                arguments = transactionID;
            }

            if (args != null && args.Length > 0)
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

            StartProcess(ExePath, arguments, true, true);

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
