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
    public partial class FormFileUploadProgress : MetroForm
    {
        public FormFileUploadProgress()
        {
            InitializeComponent();
        }

        public FormFileUploadProgress(FileViewModel FileInfo)
        {
            InitializeComponent();

            this.fileViewModel = FileInfo;
        }

        private FileViewModel fileViewModel;
        private bool isBusy = false;
        private string fileServiceType = "ftp";

        private void doUpload(FileViewModel fileInfo)
        {
            Uri uri = new Uri(fileInfo.Url);

            fileInfo.Host = uri.Host;
            fileInfo.Port = uri.Port;
            fileInfo.Protocol = uri.Scheme;

            this.fileServiceType = fileInfo.Protocol;

            switch (this.fileServiceType.ToLower())
            {
                case "http": {
                        this.doHttpUpload(fileInfo);
                        break;
                    }
                case "ftp": {
                        this.doFtpUpload(fileInfo);
                        break;
                    }
                default:{
                        this.doFtpUpload(fileInfo);
                        break;
                    }
            }
        }

        private async void doFtpUpload(FileViewModel fileInfo)
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
                this.metroLabelUploadStatus.Text = String.Format("Uploading local file \"{0}\" to \"{1}\"...please wait...", this.fileViewModel.Path, this.fileViewModel.Url);
                this.metroProgressBarUploadProgress.ProgressBarStyle = ProgressBarStyle.Marquee;
                this.metroProgressBarUploadProgress.HideProgressText = true;

                string remoteFileName = fileInfo.Path;
                remoteFileName = remoteFileName.Substring(remoteFileName.LastIndexOf("\\") + 1);
                remoteFileName = remoteFileName.Insert(remoteFileName.IndexOf("."), String.Format("-{0}", Guid.NewGuid().ToString()));
                remoteFileName = (fileInfo.Url.Substring(fileInfo.Url.LastIndexOf("/")) + "/" + remoteFileName);

                //bool result = client.Upload(stream, remoteFileName, FtpExists.Overwrite);

                //if (result)
                //{
                //    this.isBusy = false;
                //    this.metroButtonOK.Enabled = true;
                //}
                //else
                //{
                //    //Console.WriteLine("Error!");
                //}

               await client.UploadAsync(stream, remoteFileName, FtpExists.Overwrite);

               this.isBusy = false;
               this.metroLabelUploadStatus.Text = "Done.";
               this.metroProgressBarUploadProgress.ProgressBarStyle = ProgressBarStyle.Continuous;
               this.metroProgressBarUploadProgress.Maximum = 100;
               this.metroProgressBarUploadProgress.Value = 100;
               this.metroProgressBarUploadProgress.HideProgressText = false;
               this.metroButtonOK.Enabled = true;
            }
        }

        private void doHttpUpload(FileViewModel fileInfo)
        {
            WebClient client = new WebClient();

            if (!String.IsNullOrEmpty(fileInfo.UserName) && !String.IsNullOrEmpty(fileInfo.Password) && (fileInfo.AuthenticationScheme == "Basic"))
            {
                string basicAuthHeaderValue = this.getBasicAuthorizationHeaderValue(fileInfo.UserName, fileInfo.Password);

                client.Headers.Add(HttpRequestHeader.Authorization, basicAuthHeaderValue);
            }

            client.UploadFileAsync(new Uri(fileInfo.Url), fileInfo.Path);
            client.UploadProgressChanged += Client_UploadProgressChanged;
            client.UploadFileCompleted += Client_UploadFileCompleted;
        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
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

        private void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            //Task.Run(() =>
            //{

            //});

            this.metroLabelUploadStatus.Text = String.Format("Uploading \"{0}\"...{1} bytes sent...", this.fileViewModel.Path, e.BytesSent);
            this.metroProgressBarUploadProgress.Maximum = (int)e.TotalBytesToSend;
            this.metroProgressBarUploadProgress.Value = (int)e.BytesSent;
        }

        private void FormFileUploadProgress_Load(object sender, EventArgs e)
        {
            this.isBusy = true;
            this.metroButtonOK.Enabled = false;
            this.metroLabelUploadStatus.Text = String.Format("Preparing to upload local file \"{0}\" to \"{1}\"...", this.fileViewModel.Path, this.fileViewModel.Url);

            try
            {
                this.doUpload(this.fileViewModel);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    this.isBusy = false;
                    this.metroButtonOK.Enabled = true;
                    this.metroLabelUploadStatus.Text = "";
                }
            }
        }

        private void metroButtonOK_Click(object sender, EventArgs e)
        {
            this.metroLabelUploadStatus.Text = "Done!";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormFileUploadProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isBusy)
            {
                if (MessageBox.Show("File uploading is in progress, are you sure to exit and cancel the upload?", "Are You Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
