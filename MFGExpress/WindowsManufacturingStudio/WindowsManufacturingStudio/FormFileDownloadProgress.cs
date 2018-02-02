using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.Net;
using System.IO;
using FluentFTP;
using MetroFramework.Forms;
using WindowsManufacturingStudio.ViewModels;

namespace WindowsManufacturingStudio
{
    public partial class FormFileDownloadProgress : MetroForm
    {
        public FormFileDownloadProgress()
        {
            InitializeComponent();
        }

        public FormFileDownloadProgress(FileViewModel FileInfo)
        {
            InitializeComponent();

            this.fileViewModel = FileInfo;
        }

        private FileViewModel fileViewModel;
        private bool isBusy = false;
        private string fileServiceType = "ftp";

        private void doDownload(FileViewModel fileInfo)
        {
            Uri uri = new Uri(fileInfo.Url);

            fileInfo.Host = uri.Host;
            fileInfo.Port = uri.Port;
            fileInfo.Protocol = uri.Scheme;

            this.fileServiceType = fileInfo.Protocol;

            switch (this.fileServiceType.ToLower())
            {
                case "http":{
                        this.doHttpDownload(fileInfo);
                        break;
                    }
                case "ftp":{
                        this.doFtpDownload(fileInfo);
                        break;
                    }
                default:{
                        this.doFtpDownload(fileInfo);
                        break;
                    }
            }
        }

        private async void doFtpDownload(FileViewModel fileInfo)
        {
            FtpClient client = new FtpClient(fileInfo.Host);

            client.Host = fileInfo.Host; //fileInfo.Url;
            client.Port = fileInfo.Port; //21;
            client.RetryAttempts = 5;
            client.DataConnectionReadTimeout = 30;
            client.DataConnectionConnectTimeout = 30;
            client.ConnectTimeout = 30;
            //client.BulkListing = false;
            //client.SocketKeepAlive = true;

            client.Credentials = new NetworkCredential(fileInfo.UserName, fileInfo.Password);
            client.DataConnectionType = FtpDataConnectionType.AutoPassive; //FtpDataConnectionType.AutoActive;

            // begin connecting to the server
            client.Connect();

            using (FileStream stream = new FileStream(fileInfo.Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                this.metroLabelDownloadStatus.Text = String.Format("downloading file from \"{0}\" to \"{1}\"...please wait...", this.fileViewModel.Url, this.fileViewModel.Path);
                this.metroProgressBarDownloadProgress.ProgressBarStyle = ProgressBarStyle.Marquee;
                this.metroProgressBarDownloadProgress.HideProgressText = true;

                string remoteFileName = fileInfo.Url.Substring(fileInfo.Url.LastIndexOf("/"));

                //bool result = client.Download(stream, fileInfo.Url);

                //if (result)
                //{
                //    this.isBusy = false;
                //    this.metroButtonOK.Enabled = true;
                //}
                //else
                //{
                //    //Console.WriteLine("Error!");
                //}

                await client.DownloadAsync(stream, remoteFileName);

                this.isBusy = false;
                this.metroLabelDownloadStatus.Text = "Done.";
                this.metroProgressBarDownloadProgress.ProgressBarStyle = ProgressBarStyle.Continuous;
                this.metroProgressBarDownloadProgress.Maximum = 100;
                this.metroProgressBarDownloadProgress.Value = 100;
                this.metroProgressBarDownloadProgress.HideProgressText = false;
                this.metroButtonOK.Enabled = true;
            }
        }

        private void doHttpDownload(FileViewModel fileInfo)
        {
            WebClient client = new WebClient();

            if (!String.IsNullOrEmpty(fileInfo.UserName) && !String.IsNullOrEmpty(fileInfo.Password) && (fileInfo.AuthenticationScheme == "Basic"))
            {
                string basicAuthHeaderValue = this.getBasicAuthorizationHeaderValue(fileInfo.UserName, fileInfo.Password);

                client.Headers.Add(HttpRequestHeader.Authorization, basicAuthHeaderValue);
            }

            client.DownloadFileAsync(new Uri(fileInfo.Url), fileInfo.Path);
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted1;
        }

        private void Client_DownloadFileCompleted1(object sender, AsyncCompletedEventArgs e)
        {
            this.isBusy = false;
            this.metroButtonOK.Enabled = true;
        }

        private string getBasicAuthorizationHeaderValue(string userName, string password)
        {
            string authString = String.Format("{0}:{1}", userName, password);
            byte[] authBytes = System.Text.Encoding.UTF8.GetBytes(authString);
            string authBase64 = Convert.ToBase64String(authBytes);
            string basicAuthHeaderValue = String.Format("Basic {0}", authBase64);

            return basicAuthHeaderValue;
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Task.Run(() =>
            //{

            //});

            this.metroLabelDownloadStatus.Text = String.Format("Downloading \"{0}\"...{1} bytes received...", this.fileViewModel.Path, e.BytesReceived);
            this.metroProgressBarDownloadProgress.Maximum = (int)e.TotalBytesToReceive;
            this.metroProgressBarDownloadProgress.Value = (int)e.BytesReceived;
        }

        private void FormFileUploadProgress_Load(object sender, EventArgs e)
        {
            this.isBusy = true;
            this.metroButtonOK.Enabled = false;
            this.metroLabelDownloadStatus.Text = String.Format("Preparing to download from \"{0}\" ", this.fileViewModel.Url);

            try
            {
                this.doDownload(this.fileViewModel);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    this.isBusy = false;
                    this.metroButtonOK.Enabled = true;
                    this.metroLabelDownloadStatus.Text = "";
                }
            }     
        }

        private void metroButtonOK_Click(object sender, EventArgs e)
        {
            this.metroLabelDownloadStatus.Text = "Done!";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormFileUploadProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isBusy)
            {
                if (MessageBox.Show("File downloading is in progress, are you sure to exit and cancel the download?", "Are You Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
