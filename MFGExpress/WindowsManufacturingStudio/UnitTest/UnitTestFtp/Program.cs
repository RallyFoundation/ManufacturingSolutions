using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using FluentFTP;
using FluentFTP.Proxy;

namespace UnitTestFtp
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverAddress = "win-ed8tg5vh0c3";//"minint-et2evvt";//"minint-kuf9jnn";
            string userName = "root";//"WDS";
            string password = "12345"; //"W@lcome!";

            string localFilePath = @"D:\WDS-Images\Install\install-rs3.wim";
            string remoteFilePath = String.Format("/Install/install-rs3-{0}.wim", Guid.NewGuid().ToString());

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

            client.DataConnectionType = FtpDataConnectionType.AutoPassive; //FtpDataConnectionType.AutoActive;

            // begin connecting to the server
            client.Connect();

            // get a list of files and directories in the "/htdocs" folder
            foreach (FtpListItem item in client.GetListing("/"))
            {

                // if this is a file
                if (item.Type == FtpFileSystemObjectType.File)
                {

                    // get the file size
                    long size = client.GetFileSize(item.FullName);

                }

                // get modified date/time of the file or folder
                DateTime time = client.GetModifiedTime(item.FullName);

                // calculate a hash for the file on the server side (default algorithm)
                //FtpHash hash = client.GetHash(item.FullName);


                Console.WriteLine(String.Format("{0}:{1}:{2}", item.Type, item.FullName, time));

            }

            using (FileStream stream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Console.WriteLine("Uploading...");
                bool result = client.Upload(stream, remoteFilePath, FtpExists.Overwrite);

                if (result)
                {
                    Console.WriteLine("Done.");
                }
                else
                {
                    Console.WriteLine("Error!");
                }
            }

            Console.Read();
        }
    }
}
