using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
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

        private void doDownload(FileViewModel fileInfo)
        {
            WebClient client = new WebClient();

            if (!String.IsNullOrEmpty(fileInfo.UserName) && !String.IsNullOrEmpty(fileInfo.Password) && (fileInfo.AuthenticationScheme == "Basic"))
            {
                string basicAuthHeaderValue = this.getBasicAuthorizationHeaderValue(fileInfo.UserName, fileInfo.Password);

                client.Headers.Add(HttpRequestHeader.Authorization, basicAuthHeaderValue);
            }

            client.DownloadFileAsync(new Uri(fileInfo.Url), fileInfo.Path);

            client.DownloadProgressChanged += Client_DownloadProgressChanged;

            client.DownloadFileCompleted += Client_DownloadFileCompleted1; ;
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

            this.doDownload(this.fileViewModel);
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
