using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Management.Automation;
using FluentFTP;

namespace PowerShellFTPClient
{
    [Cmdlet(VerbsCommon.Get, "FTPFile")]
    public class GetFTPFileCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The host of the FTP server.")]
        public string Host { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The port of the FTP server.")]
        public int Port { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The user name of the FTP server.")]
        public string UserName { get; set; }

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "The password of the FTP server.")]
        public string Password { get; set; }

        [Parameter(Position = 4, Mandatory = true, HelpMessage = "The name of the file on the FTP server.")]
        public string RemoteName { get; set; }

        [Parameter(Position = 5, Mandatory = true, HelpMessage = "The name of the file to be saved in the local place.")]
        public string LocalName { get; set; }
        protected override void ProcessRecord()
        {
            FtpClient client = new FtpClient(this.Host);

            client.Host = this.Host; 
            client.Port = this.Port; 
            client.RetryAttempts = 6;
            client.DataConnectionReadTimeout = 30;
            client.DataConnectionConnectTimeout = 30;
            client.ConnectTimeout = 30;
            //client.BulkListing = false;
            //client.SocketKeepAlive = true;

            if (!String.IsNullOrEmpty(this.UserName))
            {
                client.Credentials = new NetworkCredential(this.UserName, this.Password);
            }          

            client.DataConnectionType = FtpDataConnectionType.AutoPassive; //FtpDataConnectionType.AutoActive;

            client.Connect();

            int activityID = new Random().Next();

            using (FileStream stream = new FileStream(this.LocalName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                this.WriteObject(String.Format("Downloading file from \"{0}\" to \"{1}\"...please wait...", this.Host, this.LocalName));

                this.WriteProgress(new ProgressRecord(activityID, "Downloading...", String.Format("Downloading file from \"{0}\" to \"{1}\"...please wait...", this.Host, this.LocalName)) { RecordType = ProgressRecordType.Processing, PercentComplete = -1 });

                client.Download(stream, this.RemoteName);

                this.WriteObject("Done.");

                this.WriteProgress(new ProgressRecord(activityID, "Completed", "Done.") { RecordType = ProgressRecordType.Completed, PercentComplete = 100 });
            }
        }
    }
}
