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
    public partial class FormPreparation : MetroForm
    {
        public FormPreparation(MetroForm Sender)
        {
            InitializeComponent();
            this.sender = Sender;
        }

        private MetroForm sender;

        private void setControls(bool isVisible) 
        {
            this.metroTextBoxPreparationDetail.Visible = isVisible;
            this.metroTileFinish.Visible = isVisible;
            this.metroTileStart.Enabled = !isVisible;
            this.metroTileFinish.Enabled = !isVisible;
            this.metroTileConfigSecPol.Visible = isVisible;
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

            string scriptPath = Application.StartupPath;

            scriptPath = scriptPath.Substring(0, scriptPath.LastIndexOf("\\"));

            scriptPath += "\\";

            scriptPath += ConfigurationManager.AppSettings.Get("PreparationScriptPath");

            //scriptPath = Application.StartupPath + @"\test.ps1";

            Utility.ExecutePSScriptFileAsync(scriptPath, null, (o) => 
            {
                foreach (var item in o)
                {
                    this.metroTextBoxPreparationDetail.AppendText(item.ToString());
                }

                return null; 
            });
        }

        private void metroTileFinish_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Are you sure to log off?", "Log Off", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Utility.StartProcess("PowerShell", "-Command (Get-WMIObject -class Win32_OperatingSystem -Computername localhost).Win32Shutdown(0)", false, false);
            }
            else
            {
                this.setControls(false);
            }
        }

        private void metroTileConfigSecPol_Click(object sender, EventArgs e)
        {
            Utility.StartProcess("PowerShell", "-Command \"secpol.msc\"", false, false);

            this.metroTileFinish.Enabled = true;
        }
    }
}
