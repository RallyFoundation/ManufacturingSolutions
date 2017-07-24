using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using MetroFramework.Forms;
using Gecko;

namespace WDSManager
{
    public partial class FormMain : MetroForm
    {
        public FormMain()
        {
            InitializeComponent();

            this.loadConfigs();
        }

        private string appRootDir, urlInstallImage, urlBootImage, urlImageGroups;

        private FormWebView formWebView;

        private void metroTileBootImages_Click(object sender, EventArgs e)
        {
            if (this.formWebView == null)
            {
                this.formWebView = new FormWebView(this, this.urlBootImage);
            }
            else if(this.formWebView.Url != this.urlBootImage)
            {
                this.formWebView.Navigate(this.urlBootImage);
            }

            this.formWebView.Show();
            this.Visible = false;      
        }

        private void metroTileImageGroups_Click(object sender, EventArgs e)
        {
            if (this.formWebView == null)
            {
                this.formWebView = new FormWebView(this, this.urlImageGroups);
            }
            else if (this.formWebView.Url != this.urlImageGroups)
            {
                this.formWebView.Navigate(this.urlImageGroups);
            }

            this.formWebView.Show();
            this.Visible = false;
        }

        private void metroTileInstallImages_Click(object sender, EventArgs e)
        {
            if (this.formWebView == null)
            {
                this.formWebView = new FormWebView(this, this.urlInstallImage);
            }
            else if (this.formWebView.Url != this.urlInstallImage)
            {
                this.formWebView.Navigate(this.urlInstallImage);
            }

            this.formWebView.Show();
            this.Visible = false;
        }

        private void loadConfigs()
        {
            this.appRootDir = Path.GetDirectoryName(Application.ExecutablePath);
            this.urlInstallImage = String.Format(ConfigurationManager.AppSettings.Get("UrlInstallImages"), appRootDir);
            this.urlBootImage = String.Format(ConfigurationManager.AppSettings.Get("UrlBootImages"), appRootDir);
            this.urlImageGroups = String.Format(ConfigurationManager.AppSettings.Get("UrlImageGroups"), appRootDir);
        }
    }
}
