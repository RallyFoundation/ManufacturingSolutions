using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using FluentFTP;
using FluentFTP.Proxy;

namespace UnitTestFtp
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverAddress = "minint-et2evvt";//"minint-kuf9jnn";
            string userName = "WDS";
            string password = "W@lcome!";

            // create an FTP client
            FtpClient client = new FtpClient(serverAddress);
            //FtpClientHttp11Proxy client = new FtpClientHttp11Proxy(new ProxyInfo() { });

            client.Host = serverAddress;
            client.Port = 21;
            client.RetryAttempts = 5;
            client.DataConnectionReadTimeout = 30;
            client.DataConnectionConnectTimeout = 30;
            client.ConnectTimeout = 30;
            //client.BulkListing = false;
            //client.SocketKeepAlive = true;

            // if you don't specify login credentials, we use the "anonymous" user account
            client.Credentials = new NetworkCredential(userName, password);

            client.DataConnectionType = FtpDataConnectionType.AutoActive;

            // begin connecting to the server
            client.Connect();

            // get a list of files and directories in the "/htdocs" folder
            foreach (FtpListItem item in client.GetListing("/"))
            {

                // if this is a file
                //if (item.Type == FtpFileSystemObjectType.File)
                //{

                //    // get the file size
                //    long size = client.GetFileSize(item.FullName);    

                //}

                // get modified date/time of the file or folder
                // DateTime time = client.GetModifiedTime(item.FullName);

                // calculate a hash for the file on the server side (default algorithm)
                //FtpHash hash = client.GetHash(item.FullName);


                Console.WriteLine(String.Format("{0}:{1}", item.Type, item.FullName));

            }

            Console.Read();
        }
    }
}
