using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;

namespace HttpFileClient
{
    class Program
    {
        static void Main(string[] args)
        {
            shouldExitOnComplete = (ConfigurationManager.AppSettings.Get("ShouldExitOnComplete") == "true");
            shouldOutputConnectionOptions = (ConfigurationManager.AppSettings.Get("ShouldOutputConnectionOptions") == "true");

            string fileUrl = (args!= null && args.Length > 0) ? args[0] : null;
            string filePath = (args != null && args.Length > 1) ? args[1] : null;
            string authScheme = (args != null && args.Length > 2) ? args[2] : null;
            string authValue = (args != null && args.Length > 3) ? args[3] : null;

            string authHeader = String.Format("{0} {1}", authScheme, authValue);

            if (String.IsNullOrEmpty(fileUrl))
            {
                Console.WriteLine("Please enter file URL:");
                fileUrl = Console.ReadLine();
            }

            if (String.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Please enter local file path:");
                filePath = Console.ReadLine();
            }

            if (shouldOutputConnectionOptions)
            {
                Console.WriteLine(fileUrl);

                Console.WriteLine(filePath);

                Console.WriteLine(authHeader);
            }

            DownloadFile(fileUrl, filePath, authHeader);

            if (!shouldExitOnComplete)
            {
                Console.Read();
            }    
        }

        static void DownloadFile(string Url, string FilePath, string AuthHeader)
        {
            WebClient webClient = new WebClient();

            if (!String.IsNullOrEmpty(AuthHeader))
            {
                webClient.Headers.Add(HttpRequestHeader.Authorization, AuthHeader);
            }

            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

            webClient.DownloadFileAsync(new Uri(Url), FilePath);        
        }

        private static void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Done!");
        }

        static int currentPercent = -1;
        static bool shouldExitOnComplete = true;
        static bool shouldOutputConnectionOptions = true;

        private static void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != currentPercent)
            {
                currentPercent = e.ProgressPercentage;

                string message = String.Format("Downloading...{0}% completed, {1} bytes received; Total bytes: {2}.----{3}", e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive, DateTime.Now);
                //Console.Clear();
                Console.WriteLine(message);
            }
        }
    }
}
