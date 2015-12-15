using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using System.Configuration;

namespace MfgSolutionsDeploymentCenter
{
    public partial class FormMain : MetroForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private FormPreparation formPreparation;
        private FormInstallation formInstallation;
        private FormAllInOneCommon formAllInOneCommonDIS;
        private FormAllInOneCommon formAllInOneCommonMES;

        //private FormUSBCopy formUSBCopy;
        private FormUSBCopySFT formUSBCopySFT;
        private FormUSBCopySNTool formUSBCopySNTool;

        private void metroTileInstall_Click(object sender, EventArgs e)
        {
            //if (this.formInstallation == null)
            //{
            //    this.formInstallation = new FormInstallation(this);
            //    this.formInstallation.Show();
            //}
            //else
            //{
            //    this.formInstallation.Visible = true;
            //}

            //if (this.formAllInOneCommonDIS == null)
            //{
            //    this.formAllInOneCommonDIS = new FormAllInOneCommon(this, 0);
            //    this.formAllInOneCommonDIS.Show();
            //}
            //else
            //{
            //    this.formAllInOneCommonDIS.Visible = true;
            //}

            List<object> resultData = new List<object>();

            FormConfirmPassword formConfirmPassword = new FormConfirmPassword(resultData);

            if (formConfirmPassword.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string scriptPath = Application.StartupPath;

                string windowsUserName = resultData[0].ToString();
                string windowsPassword = resultData[1].ToString();

                string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\" -WindowsUserName \"{1}\" -WindowsPassword \"{2}\"";

                string args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISDeploymentScriptPath"), String.Format(@".\{0}", windowsUserName), windowsPassword);
                Utility.StartProcess("PowerShell", args, true, true);
            }

            //string scriptPath = Application.StartupPath; //Application.StartupPath + @"\test.ps1";

            //scriptPath = scriptPath.Substring(0, scriptPath.LastIndexOf("\\"));

            //scriptPath += "\\";

            //scriptPath += ConfigurationManager.AppSettings.Get("DISPreparationScriptPath");

            //string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\""; //"-ExecutionPolicy ByPass -NoExit -File {0}";

            //string args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISDeploymentScriptPath"));
            //Utility.StartProcess("PowerShell", args, true, true);

            //string args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISPreparationScriptPath"));
            //Utility.StartProcess("PowerShell", args, true, true);

            ////Utility.StartProcess(scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISDBPreparationScriptPath"), null, true, true);

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISDBPreparationScriptPath"));
            //Utility.StartProcess("PowerShell", args, true, true);

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("SQLServiceRestartScriptPath"));
            //Utility.StartProcess("PowerShell", args, true, true);

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISCertImportScriptPath"));
            //Utility.StartProcess("PowerShell", args, true, true);

            //argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\" -unattend \"{1}\"";

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISInstallationScriptPath"), scriptPath + "\\Tools\\unattend-cloud.xml");
            //Utility.StartProcess("PowerShell", args, true, true);

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISInstallationScriptPath"), scriptPath + "\\Tools\\unattend-tpi.xml");
            //Utility.StartProcess("PowerShell", args, true, true);

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISInstallationScriptPath"), scriptPath + "\\Tools\\unattend-ff.xml");
            //Utility.StartProcess("PowerShell", args, true, true);

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("DISInstallationScriptPath"), scriptPath + "\\Tools\\unattend-oem.xml");
            //Utility.StartProcess("PowerShell", args, true, true);
        }

        //private void metroTilePrepare_Click(object sender, EventArgs e)
        //{
        //    if (this.formPreparation == null)
        //    {
        //        this.formPreparation = new FormPreparation(this);
        //        this.formPreparation.Show();
        //    }
        //    else
        //    {
        //        this.formPreparation.Visible = true;
        //    }

        //    this.Visible = false;
        //}

        private void metroTileAutoScanning_Click(object sender, EventArgs e)
        {
            //if (this.formAllInOneCommonMES == null)
            //{
            //    this.formAllInOneCommonMES = new FormAllInOneCommon(this, 1);
            //    this.formAllInOneCommonMES.Show();
            //}
            //else
            //{
            //    this.formAllInOneCommonMES.Visible = true;
            //}

            //this.Visible = false;

            string scriptPath = Application.StartupPath; 

            string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\""; //"-ExecutionPolicy ByPass -NoExit -File {0}";

            string args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("MESDeploymentScriptPath"));
            Utility.StartProcess("PowerShell", args, true, true);

            //string args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("MESPreparationScriptPath"));
            //Utility.StartProcess("PowerShell", args, true, true);

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("MESDBPreparationScriptPath"));
            //Utility.StartProcess("PowerShell", args, true, true);

            ////Utility.StartProcess(scriptPath + "\\" + ConfigurationManager.AppSettings.Get("MESDBPreparationScriptPath"), null, true, true);

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("SQLServiceRestartScriptPath"));
            //Utility.StartProcess("PowerShell", args, true, true);

            //argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\" -unattend \"{1}\"";

            //args = String.Format(argsTemp, scriptPath + "\\" + ConfigurationManager.AppSettings.Get("MESInstallationScriptPath"), scriptPath + "\\Tools\\unattend-mes.xml");
            //Utility.StartProcess("PowerShell", args, true, true);
        }

        private void metroTileAutoOA30SFT_Click(object sender, EventArgs e)
        {
            string[] removableDrives = Utility.GetRemovableDrives();

            if ((removableDrives == null) || (removableDrives.Length <= 0))
            {
                MetroMessageBox.Show(this, "Please insert a USB disk!", "USB Disk NOT Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            //else if (removableDrives.Length == 1)
            //{
            //    try
            //    {
            //        Utility.CopyDirectory(Application.StartupPath + "\\" + ConfigurationManager.AppSettings.Get("SFTPath"), removableDrives[0]);

            //        if (MetroMessageBox.Show(this, String.Format("SFT depolyed to {0} successfully!", removableDrives[0]), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            //        {
            //            return;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MetroMessageBox.Show(this, String.Format("Error(s) encountered: {0} ", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    } 
            //}
            else if(removableDrives.Length >= 1)
            {
                this.formUSBCopySFT = new FormUSBCopySFT(this, removableDrives, Application.StartupPath + "\\" + ConfigurationManager.AppSettings.Get("SFTPath"));
                this.formUSBCopySFT.Show();
                this.Visible = false;
            }
        }

        private void metroTileSNInjectionTool_Click(object sender, EventArgs e)
        {
            string snInjectionFlag = ConfigurationManager.AppSettings.Get("SNInjectionEnabled");

            if (!String.IsNullOrEmpty(snInjectionFlag) && (snInjectionFlag.ToLower() == "true"))
            {
                string[] removableDrives = Utility.GetRemovableDrives();

                if ((removableDrives == null) || (removableDrives.Length <= 0))
                {
                    MetroMessageBox.Show(this, "Please insert a USB disk!", "USB Disk NOT Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (removableDrives.Length >= 1)
                {
                    string sourcePath = Application.StartupPath + "\\" + ConfigurationManager.AppSettings.Get("SNInjectionToolPath");

                    if (!Directory.Exists(sourcePath))
                    {
                        MetroMessageBox.Show(this, "Please contact your firmware vendor!", "Vendor Approval Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string[] entries = Directory.GetDirectories(sourcePath);

                    if ((entries == null) || (entries.Length <= 0))
                    {
                        MetroMessageBox.Show(this, "Please contact your firmware vendor!", "Vendor Approval Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    this.formUSBCopySNTool = new FormUSBCopySNTool(this, removableDrives, sourcePath);
                    this.formUSBCopySNTool.Show();
                    this.Visible = false;
                }
            }
        }
    }
}
