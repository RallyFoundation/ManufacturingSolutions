﻿using System;
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

namespace WindowsManufacturingStudio
{
    public partial class FormMain : MetroForm
    {
        public FormMain()
        {
            InitializeComponent();

            this.loadConfigs();
        }

        private string appRootDir, urlInstallImages, urlBootImages, urlImageGroups, urlImageLookups, urlClientPulse, urlConfigurations;

        private FormWebView formWebView;
        private FormCreateBootWinPE formCreateBootWinPE;

        private void metroTileSettings_Click(object sender, EventArgs e)
        {
            if (this.formWebView == null)
            {
                this.formWebView = new FormWebView(this, this.urlConfigurations);
            }
            else if (this.formWebView.Url != this.urlConfigurations)
            {
                this.formWebView.Navigate(this.urlConfigurations);
            }

            this.formWebView.Show();
            this.Visible = false;
        }

        private void metroTileFFUImages_Click(object sender, EventArgs e)
        {

        }

        private void metroTileImageLookups_Click(object sender, EventArgs e)
        {
            if (this.formWebView == null)
            {
                this.formWebView = new FormWebView(this, this.urlImageLookups);
            }
            else if (this.formWebView.Url != this.urlImageLookups)
            {
                this.formWebView.Navigate(this.urlImageLookups);
            }

            this.formWebView.Show();
            this.Visible = false;
        }

        private void metroTileClientPulse_Click(object sender, EventArgs e)
        {
            if (this.formWebView == null)
            {
                this.formWebView = new FormWebView(this, this.urlClientPulse);
            }
            else if (this.formWebView.Url != this.urlClientPulse)
            {
                this.formWebView.Navigate(this.urlClientPulse);
            }

            this.formWebView.Show();
            this.Visible = false;
        }

        private void metroTileCreateBootWindowsPE_Click(object sender, EventArgs e)
        {
            if (this.formCreateBootWinPE == null)
            {
                this.formCreateBootWinPE = new FormCreateBootWinPE(this, appRootDir);
            }

            this.formCreateBootWinPE.Show();
            this.Visible = false;
        }

        private void metroTileBootImages_Click(object sender, EventArgs e)
        {
            if (this.formWebView == null)
            {
                this.formWebView = new FormWebView(this, this.urlBootImages);
            }
            else if(this.formWebView.Url != this.urlBootImages)
            {
                this.formWebView.Navigate(this.urlBootImages);
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
                this.formWebView = new FormWebView(this, this.urlInstallImages);
            }
            else if (this.formWebView.Url != this.urlInstallImages)
            {
                this.formWebView.Navigate(this.urlInstallImages);
            }

            this.formWebView.Show();
            this.Visible = false;
        }

        private void loadConfigs()
        {
            this.appRootDir = Path.GetDirectoryName(Application.ExecutablePath);
            this.urlInstallImages = String.Format(ConfigurationManager.AppSettings.Get("UrlInstallImages"), appRootDir);
            this.urlBootImages = String.Format(ConfigurationManager.AppSettings.Get("UrlBootImages"), appRootDir);
            this.urlImageGroups = String.Format(ConfigurationManager.AppSettings.Get("UrlImageGroups"), appRootDir);
            this.urlImageLookups = String.Format(ConfigurationManager.AppSettings.Get("UrlImageLookups"), appRootDir);
            this.urlClientPulse = String.Format(ConfigurationManager.AppSettings.Get("UrlClientPulse"), appRootDir);
            this.urlConfigurations = String.Format(ConfigurationManager.AppSettings.Get("UrlConfigurations"), appRootDir);
        }
    }
}
