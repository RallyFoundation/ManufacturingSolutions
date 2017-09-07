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

        private void doUpload(FileViewModel fileInfo)
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

            this.doUpload(this.fileViewModel);
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
