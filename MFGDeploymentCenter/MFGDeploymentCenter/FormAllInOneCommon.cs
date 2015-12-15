using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Diagnostics;
using System.Configuration;

namespace MfgSolutionsDeploymentCenter
{
    public partial class FormAllInOneCommon : MetroForm
    {
        public FormAllInOneCommon(MetroForm Sender, int Mode)
        {
            InitializeComponent();
            this.sender = Sender;

            if (Mode == 0)
            {
                this.Text = "Installing DIS 2.0";
            }
            else
            {
                this.Text = "Installing Auto Scanning Suite";
            }

            this.mode = Mode;
        }

        private MetroForm sender;
        private int mode = 0;

        private void setControls(bool isVisible) 
        {
            this.metroTextBoxPreparationDetail.Visible = isVisible;
            this.metroTileFinish.Visible = isVisible;
            this.metroTileStart.Enabled = !isVisible;
            this.metroTileFinish.Enabled = !isVisible;
        }

        private void executePSScript(string scriptFileName, Dictionary<string, object> scriptParams) 
        {
            string scriptPath = Application.StartupPath;

            //scriptPath = scriptPath.Substring(0, scriptPath.LastIndexOf("\\"));

            scriptPath += "\\";

            scriptPath += scriptFileName;

            Utility.ExecutePSScriptFileAsync(scriptPath, scriptParams, (o) =>
            {
                foreach (var item in o)
                {
                    this.metroTextBoxPreparationDetail.AppendText(item.ToString());
                }

                return null;
            });
        }

        private void FormPreparation_FormClosing(object sender, FormClosingEventArgs e)
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
            this.setControls(true);

            if (this.mode == 0)
            {
                this.executePSScript(ConfigurationManager.AppSettings.Get("DISPreparationScriptPath"), null);

                this.executePSScript(ConfigurationManager.AppSettings.Get("DISDBPreparationScriptPath"), null);

                this.executePSScript(ConfigurationManager.AppSettings.Get("SQLServiceRestartScriptPath"), null);

                this.executePSScript(ConfigurationManager.AppSettings.Get("DISCertImportScriptPath"), null);

                this.executePSScript(ConfigurationManager.AppSettings.Get("DISInstallationScriptPath"), new Dictionary<string, object>() { { "unattend", Application.StartupPath + "\\Tools\\unattend-cloud.xml"}});

                this.executePSScript(ConfigurationManager.AppSettings.Get("DISInstallationScriptPath"), new Dictionary<string, object>() { { "unattend", Application.StartupPath + "\\Tools\\unattend-tpi.xml" } });

                this.executePSScript(ConfigurationManager.AppSettings.Get("DISInstallationScriptPath"), new Dictionary<string, object>() { { "unattend", Application.StartupPath + "\\Tools\\unattend-ff.xml" } });

                this.executePSScript(ConfigurationManager.AppSettings.Get("DISInstallationScriptPath"), new Dictionary<string, object>() { { "unattend", Application.StartupPath + "\\Tools\\unattend-oem.xml" } });
            }
            else
            {
                this.executePSScript(ConfigurationManager.AppSettings.Get("MESPreparationScriptPath"), null);

                this.executePSScript(ConfigurationManager.AppSettings.Get("MESDBPreparationScriptPath"), null);

                this.executePSScript(ConfigurationManager.AppSettings.Get("SQLServiceRestartScriptPath"), null);

                this.executePSScript(ConfigurationManager.AppSettings.Get("MESInstallationScriptPath"), new Dictionary<string, object>() { { "unattend", Application.StartupPath + "\\Tools\\unattend-mes.xml" } });
            }
        }

        private void metroTileFinish_Click(object sender, EventArgs e)
        {
            //if (MetroFramework.MetroMessageBox.Show(this, "Are you sure to log off?", "Log Off", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    Utility.StartProcess("PowerShell", "-Command (Get-WMIObject -class Win32_OperatingSystem -Computername localhost).Win32Shutdown(0)", false);
            //}
            //else
            //{
            //    this.setControls(false);
            //}

            this.setControls(false);
        }
    }
}
