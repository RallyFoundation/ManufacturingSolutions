using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }

        private void webBrowserReport_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            
        }
    }
}
