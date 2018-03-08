using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using FluentFTP;

namespace FtpDownloader
{
    class Program
    {
        const string ParamName_Host = "HOST";
        const string ParamName_Port = "PORT";
        const string ParamName_UserName = "USERNAME";
        const string ParamName_Password = "PASSWORD";
        const string ParamName_RemoteFile = "REMOTE";
        const string ParamName_LocalFile = "LOCAL";
        const string Pair_Separator = "=";
        static void Main(string[] args)
        {
            parseArguments(args);

            string host = ftpParameters[ParamName_Host];
            int port = int.Parse(ftpParameters[ParamName_Port]);
            string userName = ftpParameters[ParamName_UserName];
            string password = ftpParameters[ParamName_Password];
            string remoteFile = ftpParameters[ParamName_RemoteFile];
            string localFile = ftpParameters[ParamName_LocalFile];

            doFtpDownload(host, port, userName, password, remoteFile, localFile);

            //Console.Read();
        }

        static Dictionary<string, string> ftpParameters;

        static void parseArguments(string[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                string[] pair;
                string key, value;
                ftpParameters = new Dictionary<string, string>();

                foreach (string arg in args)
                {
                    pair = arg.Split(new string[] { Pair_Separator},  StringSplitOptions.RemoveEmptyEntries);

                    if ((pair != null) && (pair.Length > 1))
                    {
                        key = pair[0].Substring(1).ToUpper();
                        value = pair[1];

                        if (!ftpParameters.ContainsKey(key))
                        {
                            ftpParameters.Add(key, value);
                        }
                        else
                        {
                            ftpParameters[key] = value;
                        }               
                    }
                }
            }
        }

        static async void doFtpDownload(string host, int port, string userName, string password, string remoteFile, string localFile)
        {
            FtpClient client = new FtpClient(host);

            client.Host = host; //fileInfo.Url;
            client.Port = port; //21;
            client.RetryAttempts = 5;
            client.DataConnectionReadTimeout = 30;
            client.DataConnectionConnectTimeout = 30;
            client.ConnectTimeout = 30;
            //client.BulkListing = false;
            //client.SocketKeepAlive = true;

            client.Credentials = new NetworkCredential(userName, password);
            client.DataConnectionType = FtpDataConnectionType.AutoPassive; //FtpDataConnectionType.AutoActive;

            client.Connect();

            bool result = false;

            Console.WriteLine(String.Format("Downloading file from \"{0}\" to \"{1}\"...please wait...", host, localFile));

            using (FileStream stream = new FileStream(localFile, FileMode.Create, FileAccess.Write, FileShare.Write))
            {  
                result = await client.DownloadAsync(stream, remoteFile);

                Console.WriteLine(result);

                while (!result)
                {

                }
            }

            Console.WriteLine("Done!");
        }
    }
}
