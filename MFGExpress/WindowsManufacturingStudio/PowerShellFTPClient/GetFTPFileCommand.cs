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

        [Parameter(Position = 6, Mandatory = false, HelpMessage = "The length of time in milliseconds the data channel should wait for the server to send data.")]
        public int DataReadTimeout { get; set; }

        [Parameter(Position = 7, Mandatory = false, HelpMessage = "The length of time in milliseconds for a data connection to be established before giving up.")]
        public int DataConnectTimeout { get; set; }

        [Parameter(Position = 8, Mandatory = false, HelpMessage = "The length of time in milliseconds to wait for a data connection to be established before giving up.")]
        public int ConnectTimeout { get; set; }

        [Parameter(Position = 9, Mandatory = false, HelpMessage = "The retry attemps allowed when a verification failure occures during download.")]
        public int RetryAttempts { get; set; }
        protected override void ProcessRecord()
        {
            try
            {
                FtpClient client = new FtpClient(this.Host);

                client.Host = this.Host;
                client.Port = this.Port;
                client.RetryAttempts = this.RetryAttempts > 0 ? this.RetryAttempts : 6;
                client.DataConnectionReadTimeout = this.DataReadTimeout > 0 ? this.DataReadTimeout : 30;
                client.DataConnectionConnectTimeout = this.DataConnectTimeout > 0 ? this.DataConnectTimeout : 30;
                client.ConnectTimeout = this.ConnectTimeout > 0 ? this.ConnectTimeout : 30;
                client.BulkListing = false;
                client.SocketKeepAlive = true;

                if (!String.IsNullOrEmpty(this.UserName))
                {
                    client.Credentials = new NetworkCredential(this.UserName, this.Password);
                }

                client.DataConnectionType = FtpDataConnectionType.AutoPassive; //FtpDataConnectionType.AutoActive;

                client.Connect();

                int activityID = new Random().Next();

                this.WriteObject(String.Format("Downloading file from \"{0}\" to \"{1}\"...please wait...", this.Host, this.LocalName));

                this.WriteProgress(new ProgressRecord(activityID, "Downloading...", String.Format("Downloading file from \"{0}\" to \"{1}\"...please wait...", this.Host, this.LocalName)) { RecordType = ProgressRecordType.Processing, PercentComplete = -1 });

                bool result = false;

                //using (FileStream stream = new FileStream(this.LocalName, FileMode.Create, FileAccess.Write, FileShare.Write))
                //{
                //    result = client.Download(stream, this.RemoteName);
                //}

                result = client.DownloadFile(this.LocalName, this.RemoteName, true, FtpVerify.Retry);

                if (result){
                    this.WriteObject("Done.");
                }else{
                    this.WriteObject("Error!");
                }

                this.WriteProgress(new ProgressRecord(activityID, "Completed", "Done.") { RecordType = ProgressRecordType.Completed, PercentComplete = 100 });
            }
            catch (Exception ex)
            {
                this.WriteWarning(ex.ToString());
                this.WriteWarning(ex.InnerException.ToString());
            }
        }
    }
}
