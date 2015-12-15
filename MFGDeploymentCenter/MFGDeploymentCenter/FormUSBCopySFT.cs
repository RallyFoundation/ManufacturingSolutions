using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using MetroFramework;
using MetroFramework.Forms;

namespace MfgSolutionsDeploymentCenter
{
    public partial class FormUSBCopySFT : MetroForm
    {
        public FormUSBCopySFT(MetroForm Sender, string[] RemovableDisks, string Source)
        {
            InitializeComponent();

            this.sender = Sender;
            this.source = Source;

            this.metroComboBoxRemovableDisks.DataSource = RemovableDisks;
            this.metroComboBoxRemovableDisks.SelectedIndex = 0;
        }

        private string source;
        private MetroForm sender;

        private void setControls(bool isCopying) 
        {
            this.metroProgressBarCopyProgress.Visible = isCopying;
            this.metroLabelCurrentItem.Visible = isCopying;
            this.metroLinkBack.Enabled = isCopying;
            this.metroButtonBeginCopy.Enabled = isCopying;
            this.metroComboBoxRemovableDisks.Enabled = isCopying;

            this.metroTextBoxMESCloudURL.Enabled = isCopying;
            this.metroTextBoxMESCloudUserName.Enabled = isCopying;
            this.metroTextBoxMESCloudPassword.Enabled = isCopying;

            if (isCopying)
            {
                string[] items = System.IO.Directory.GetFileSystemEntries(this.source, "*", SearchOption.AllDirectories);

                this.metroProgressBarCopyProgress.Maximum = items.Length;
                this.metroProgressBarCopyProgress.Minimum = 0;
                this.metroProgressBarCopyProgress.Value = 0;
            }
            else
            {
                this.metroProgressBarCopyProgress.Maximum = 0;
                this.metroProgressBarCopyProgress.Minimum = 0;
                this.metroProgressBarCopyProgress.Value = 0;
            }
        }

        private void setSFTConfigs() 
        {
            string sftExeName = ConfigurationManager.AppSettings.Get("SFTExeName");

            string sftExeConfigPath = this.metroComboBoxRemovableDisks.SelectedValue.ToString();

            if (!sftExeConfigPath.EndsWith("\\"))
            {
                sftExeConfigPath += "\\";
            }

            sftExeConfigPath += sftExeName;

            Configuration config = ConfigurationManager.OpenExeConfiguration(sftExeConfigPath);

            string mesCloudServicePoint = this.metroTextBoxMESCloudURL.Text;

            if (!mesCloudServicePoint.EndsWith("/"))
            {
                mesCloudServicePoint += "/";
            }

            config.AppSettings.Settings["IsSavingTestResultsToMESCloud"].Value = this.metroToggleMESSwitch.Checked ? "true" : "false";

            if (this.metroToggleMESSwitch.Checked)
            {
                config.AppSettings.Settings["MESCloudServicePoint"].Value = mesCloudServicePoint + "Services/MESService.svc";
                config.AppSettings.Settings["MESCloudUserName"].Value = this.metroTextBoxMESCloudUserName.Text;
                config.AppSettings.Settings["MESCloudPassword"].Value = this.metroTextBoxMESCloudPassword.Text;

                string externalAppParams = config.AppSettings.Settings["ExternalAppParams"].Value;

                string[] externalAppParamsArray = externalAppParams.Split(new string[] { " " }, StringSplitOptions.None);

                int position = -1;

                for (int i = 0; i < externalAppParamsArray.Length; i++)
                {
                    if (externalAppParamsArray[i] == "-Architecture")
                    {
                        position = i;
                        break;
                    }
                }

                if (position < (externalAppParamsArray.Length - 1))
                {
                    externalAppParamsArray[position + 1] = this.metroRadioButtonSysArcx86.Checked ? "x86" : "amd64";
                }

                externalAppParams = "";

                for (int i = 0; i < externalAppParamsArray.Length; i++)
                {
                    externalAppParams += externalAppParamsArray[i];

                    if (i != (externalAppParamsArray.Length - 1))
                    {
                        externalAppParams += " ";
                    }
                }

                config.AppSettings.Settings["ExternalAppParams"].Value = externalAppParams;
            }

            config.Save();
        }


        private void editOA3ToolConfig() 
        {
            string oa3ToolConfGenPath = ConfigurationManager.AppSettings.Get("OA3ToolConfGenPath");

            if (oa3ToolConfGenPath.StartsWith("\\"))
            {
                oa3ToolConfGenPath = oa3ToolConfGenPath.Substring(1);
            }

            string oa3ToolConfPath = this.metroRadioButtonSysArcx86.Checked ? ConfigurationManager.AppSettings.Get("OA3ToolConfPathx86") : ConfigurationManager.AppSettings.Get("OA3ToolConfPathAmd64");

            if (oa3ToolConfPath.StartsWith("\\"))
            {
                oa3ToolConfPath = oa3ToolConfPath.Substring(1);
            }

            string rootPath = this.metroComboBoxRemovableDisks.SelectedValue.ToString();

            if (!rootPath.EndsWith("\\"))
            {
                rootPath += "\\";
            }

            oa3ToolConfGenPath = rootPath + oa3ToolConfGenPath;
            oa3ToolConfPath = rootPath + oa3ToolConfPath;

            Utility.StartProcess(oa3ToolConfGenPath, oa3ToolConfPath, false, false);
        }

        private void CopyDirectory(string Source, string Destination)
        {
            String[] Files;

            if (Destination[Destination.Length - 1] != Path.DirectorySeparatorChar)
            {
                Destination += Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(Destination)) 
            {
                Directory.CreateDirectory(Destination);

                this.Invoke(new Action(() =>
                {
                    this.metroProgressBarCopyProgress.PerformStep();
                    this.metroLabelCurrentItem.Text = Source;
                    this.metroLabelCurrentItem.Refresh();
                }));
            }

            Files = Directory.GetFileSystemEntries(Source);

            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                {
                    CopyDirectory(Element, Destination + Path.GetFileName(Element));
                }
                // Files in directory
                else
                {
                    File.Copy(Element, Destination + Path.GetFileName(Element), true);

                    //this.Invoke(new Action(() =>
                    //{
                    //    this.metroProgressBarCopyProgress.PerformStep();
                    //    this.metroLabelCurrentItem.Text = Element;
                    //}));
                }

                this.Invoke(new Action(() =>
                {
                    this.metroProgressBarCopyProgress.PerformStep();
                    this.metroLabelCurrentItem.Text = Element;
                    this.metroLabelCurrentItem.Refresh();
                }));
            }
        }

        private void metroButtonBeginCopy_Click(object sender, EventArgs e)
        {
            try
            {
                //Utility.CopyDirectory(this.source, this.metroComboBoxRemovableDisks.SelectedValue.ToString());

                if (this.metroToggleMESSwitch.Checked)
                {
                    if (String.IsNullOrEmpty(this.metroTextBoxMESCloudURL.Text))
                    {
                        MetroMessageBox.Show(this, "MES Cloud URL is required!", "Infromation Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (String.IsNullOrEmpty(this.metroTextBoxMESCloudUserName.Text))
                    {
                        MetroMessageBox.Show(this, "MES Cloud User Name is required!", "Infromation Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (String.IsNullOrEmpty(this.metroTextBoxMESCloudPassword.Text))
                    {
                        MetroMessageBox.Show(this, "MES Cloud Password is required!", "Infromation Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                this.setControls(true);

                this.CopyDirectory(this.source, this.metroComboBoxRemovableDisks.SelectedValue.ToString());

                this.setSFTConfigs();

                if (MetroMessageBox.Show(this, String.Format("All files from {0} successfully depolyed to {1} ! Configure the OA3Tool configuration file now?", this.source, this.metroComboBoxRemovableDisks.SelectedValue.ToString()), "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.editOA3ToolConfig();
                }

                this.setControls(false);
                this.sender.Visible = true;
                this.Close();

            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, String.Format("Error(s) encountered: {0} ", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void metroLinkBack_Click(object sender, EventArgs e)
        {
            if (this.sender != null)
            {
                this.sender.Visible = true;
                this.Close();
            }
        }

        private void FormUSBCopy_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.sender != null)
            {
                this.sender.Visible = true;
            }
        }

        private void metroToggleShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.metroTextBoxMESCloudPassword.UseSystemPasswordChar = (!this.metroToggleShowPassword.Checked);
        }

        private void metroToggleMESSwitch_CheckedChanged(object sender, EventArgs e)
        {
            this.panelMES.Visible = this.metroToggleMESSwitch.Checked;
        }
    }
}
