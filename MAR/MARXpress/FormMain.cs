using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Xml;
using MetroFramework.Forms;

namespace MARXpress
{
    public partial class FormMain : MetroForm
    {
        public FormMain()
        {
            InitializeComponent();

            this.loadAppConfigs();

            this.loadUiLayoutConfigs();
        }

        private string OA3StartScriptPath;
        //private string OA3ValidateScriptPath;
        private string OA3ToolConfGenPath;
        private string OA3ToolConfPath;
        private string LookupMapperPath;
        private string LookupConfPath;
        //private string OA3ToolPath;
        //private string InjectionToolPath;
        //private string InjectionToolEraseCommand;
        //private string EraseScriptPath;


        private void loadAppConfigs()
        {
            this.OA3StartScriptPath = ConfigurationManager.AppSettings.Get("OA3StartScriptPath");
            //this.OA3ValidateScriptPath = ConfigurationManager.AppSettings.Get("OA3ValidateScriptPath");
            this.OA3ToolConfGenPath = ConfigurationManager.AppSettings.Get("OA3ToolConfGenPath");
            //this.OA3ToolConfPath = ConfigurationManager.AppSettings.Get("OA3ToolConfPathX86");
            this.OA3ToolConfPath = ConfigurationManager.AppSettings.Get("OA3ToolConfPath");
            this.LookupMapperPath = ConfigurationManager.AppSettings.Get("LookupMapperPath");
            this.LookupConfPath = ConfigurationManager.AppSettings.Get("LookupConfPath");
            //this.OA3ToolPath = ConfigurationManager.AppSettings.Get("OA3ToolPathX86");
            //this.InjectionToolPath = ConfigurationManager.AppSettings.Get("InjectionToolPath");
            //this.InjectionToolEraseCommand = ConfigurationManager.AppSettings.Get("InjectionToolEraseCommand");
            //this.EraseScriptPath = ConfigurationManager.AppSettings.Get("EraseScriptPath");
        }

        //private void loadAppConfigByAchitecture(string architecture)
        //{
        //    switch (architecture.ToLower())
        //    {
        //        case "x86":
        //            this.OA3ToolConfPath = ConfigurationManager.AppSettings.Get("OA3ToolConfPathX86");
        //            this.OA3ToolPath = ConfigurationManager.AppSettings.Get("OA3ToolPathX86");
        //            break;
        //        case "amd64":
        //            this.OA3ToolConfPath = ConfigurationManager.AppSettings.Get("OA3ToolConfPathAmd64");
        //            this.OA3ToolPath = ConfigurationManager.AppSettings.Get("OA3ToolPathAmd64");
        //            break;
        //        default:
        //            this.OA3ToolConfPath = ConfigurationManager.AppSettings.Get("OA3ToolConfPathX86");
        //            this.OA3ToolPath = ConfigurationManager.AppSettings.Get("OA3ToolPathX86");
        //            break;
        //    }
        //}

        private void loadUiLayoutConfigs()
        {
            string configPath = "Config\\ui-layout.xml";

            if (File.Exists(configPath))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(configPath);
                Window window = Utility.XmlDeserialize(xml.InnerXml, typeof(Window), new Type[] { typeof(Tile[]), typeof(Tile) }, "utf-8") as Window;

                if (window != null)
                {
                    this.Height = window.Height;
                    this.Width = window.Width;
                    this.Text = window.Text;

                    if (window.Tiles != null)
                    {
                        Control control = null;

                        Control[] children = null;

                        foreach (var tile in window.Tiles)
                        {
                            children = (this.Controls.Find(tile.Name, true) as Control[]);

                            if ((children != null) && (children.Length > 0))
                            {
                                control = children[0];

                                control.Height = tile.Height;
                                control.Width = tile.Width;
                                control.Visible = tile.Visible;
                                control.Location = new Point(tile.X, tile.Y);
                                control.Text = tile.Text;
                            }
                        }
                    }
                }
            }
        }

        private string getFullPath(string relativePath)
        {
            string rootPath = Application.StartupPath;

            if (rootPath.EndsWith("\\"))
            {
                rootPath = rootPath.Substring(0, (rootPath.Length - 1));
            }

            if (!relativePath.StartsWith("\\"))
            {
                relativePath = "\\" + relativePath;
            }

            return rootPath + relativePath;
        }

        //private void metroRadioButtonSysArchX86_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!this.metroRadioButtonSysArchX86.Checked)
        //    {
        //        this.loadAppConfigByAchitecture("amd64");
        //    }
        //    else
        //    {
        //        this.loadAppConfigByAchitecture("x86");
        //    }
        //}

        private void metroTileConfigure_Click(object sender, EventArgs e)
        {
            string oa3ToolConfGenFullPath = this.getFullPath(this.OA3ToolConfGenPath);

            if (!String.IsNullOrEmpty(this.OA3ToolConfPath))
            {
                string oa3ToolConfFullPath = this.getFullPath(this.OA3ToolConfPath);
                Utility.StartProcess(oa3ToolConfGenFullPath, oa3ToolConfFullPath, true, true);
            }
            else
            {
                Utility.StartProcess(oa3ToolConfGenFullPath, null, true, true);
            }          
        }

        private void metroTileStart_Click(object sender, EventArgs e)
        {
            string startScriptFullPath = this.getFullPath(this.OA3StartScriptPath);

            string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\" -Architecture \"{1}\"";

            //string args = String.Format(argsTemp, startScriptFullPath, this.metroRadioButtonSysArchX86.Checked ? "x86" : "amd64");

            string args = String.Format(argsTemp, startScriptFullPath, "x86");

            Utility.StartProcess("PowerShell", args, true, true);
        }

        private void metroTileConfigureLookup_Click(object sender, EventArgs e)
        {
            string lookupMapperFullPath = this.getFullPath(this.LookupMapperPath);
            string lookupConfFullPath = this.getFullPath(this.LookupConfPath);

            Utility.StartProcess(lookupMapperFullPath, lookupConfFullPath, true, true);
        }

        //private void metroTileValidate_Click(object sender, EventArgs e)
        //{
        //    //string oa3ToolFullPath = this.getFullPath(this.OA3ToolPath);

        //    //string argsTemp = "-ExecutionPolicy ByPass -NoExit -Command \"{0} /Validate\"";

        //    //string args = String.Format(argsTemp, oa3ToolFullPath);

        //    string validateScriptFullPath = this.getFullPath(this.OA3ValidateScriptPath);

        //    string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\" -Architecture \"{1}\"";

        //    string args = String.Format(argsTemp, validateScriptFullPath, this.metroRadioButtonSysArchX86.Checked ? "x86" : "amd64");

        //    Utility.StartProcess("PowerShell", args, true, true);
        //}

        //private void metroTileErase_Click(object sender, EventArgs e)
        //{
        //    //string injectionToolFullPath = this.getFullPath(this.InjectionToolPath);

        //    //string argsTemp = "-ExecutionPolicy ByPass -NoExit -Command \"{0} {1}\"";

        //    //string args = String.Format(argsTemp, injectionToolFullPath, this.InjectionToolEraseCommand);

        //    string eraseScriptFullPath = this.getFullPath(this.EraseScriptPath);

        //    string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\" -Architecture \"{1}\"";

        //    string args = String.Format(argsTemp, eraseScriptFullPath, this.metroRadioButtonSysArchX86.Checked ? "x86" : "amd64");

        //    Utility.StartProcess("PowerShell", args, true, true);
        //}
    }
}
