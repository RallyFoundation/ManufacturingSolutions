using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using MetroFramework.Controls;
using DIS.Management.Deployment.Model;

namespace MfgSolutionsDeploymentCenter
{
    public partial class UCCloudInstallation : MetroUserControl
    {
        public UCCloudInstallation()
        {
            InitializeComponent();
        }

        private void metroButtonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog()
            {
                RootFolder = Environment.SpecialFolder.ProgramFiles
            };

            if (folderDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.metroTextBoxDISHome.Text = folderDialog.SelectedPath;
            }
        }

        public string GetUnattendFile() 
        {
            string unattendFileName = String.Format("unattend-cloud-{0}.xml", Guid.NewGuid().ToString());

            string unattendFileFullName = Application.StartupPath + "\\" + unattendFileName;

            Installation installationInfo = Utility.GetCloudInstallationInfo(this.metroTextBoxDISHome.Text, this.metroTextBoxDBInstance.Text, this.metroTextBoxDBLoginName.Text, this.metroTextBoxDBLoginPassword.Text);

            return Utility.GenerateUnattendFile(installationInfo, unattendFileFullName);
        }
    }
}
