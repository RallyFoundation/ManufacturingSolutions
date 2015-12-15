using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Configuration;

namespace MfgSolutionsDeploymentCenter
{
    public partial class FormCloudInstallation : MetroForm
    {
        public FormCloudInstallation(MetroForm Sender)
        {
            InitializeComponent();

            this.ucCloudInstallation = new UCCloudInstallation();

            this.metroTabPageArguments.Controls.Add(this.ucCloudInstallation);
            this.metroTabControlCloudInstallation.SelectedTab = this.metroTabPageArguments;

            this.sender = Sender;
        }

        private UCCloudInstallation ucCloudInstallation;
        private MetroForm sender;

        private void FormCloudInstallation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.sender != null)
            {
                e.Cancel = true;
                this.Visible = false;
                this.sender.Visible = true;
            }
        }
        private void metroLinkBack_Click(object sender, EventArgs e)
        {
            if (this.sender != null)
            {
                this.Visible = false;
                this.sender.Visible = true;
            }
        }

        private void metroTileStart_Click(object sender, EventArgs e)
        {
            string unattendFilePath = this.ucCloudInstallation.GetUnattendFile();
            this.metroTextBoxResults.Text = unattendFilePath;

            this.metroTabControlCloudInstallation.SelectedTab = this.metroTabPageResults;

            string scriptPath = Application.StartupPath;

            scriptPath = scriptPath.Substring(0, scriptPath.LastIndexOf("\\"));

            scriptPath += "\\";

            scriptPath += ConfigurationManager.AppSettings.Get("InstallationScriptPath");

            //scriptPath = Application.StartupPath + @"\test.ps1";

            Utility.ExecutePSScriptFileAsync(scriptPath, null, (o) =>
            {
                foreach (var item in o)
                {
                    this.metroTextBoxResults.AppendText(item.ToString());
                }

                return null;
            });
        }
    }
}
