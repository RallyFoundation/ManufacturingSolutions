using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace PowerShellDataProcessing
{
    public partial class FormReport : MetroForm
    {
        public FormReport()
        {
            InitializeComponent();
        }

        public string Uri { get; set; }

        public void Navigate(string uri)
        {
            this.Uri = uri;
            //this.webBrowserReport.Url = new Uri(uri);
            this.webBrowserReport.Navigate(uri);
            this.webBrowserReport.Refresh();
        }

        private void webBrowserReport_FileDownload(object sender, EventArgs e)
        {
            //MessageBox.Show(e.ToString());
            
        }

        private void webBrowserReport_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //MessageBox.Show(e.Url.AbsoluteUri);

            if (e.Url.AbsoluteUri.EndsWith("#close"))
            {
                e.Cancel = true;
                this.Close();
            }

            if (e.Url.AbsoluteUri.StartsWith("file:///") && e.Url.AbsoluteUri.EndsWith("#save"))
            {
                e.Cancel = true;

                string sourcePath = e.Url.AbsoluteUri.Substring(8);
                sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf("#"));

                if (this.saveFileDialogSaveFile.ShowDialog() == DialogResult.OK)
                {
                    string destPath = this.saveFileDialogSaveFile.FileName;

                    //MessageBox.Show(sourcePath);

                    //MessageBox.Show(destPath);

                    File.Copy(sourcePath, destPath);

                    MessageBox.Show(String.Format("File \"{0}\" successfully saved to: \"{1}\".", sourcePath, destPath), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
