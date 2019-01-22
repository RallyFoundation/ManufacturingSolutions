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

namespace VamtXpress
{
    public partial class FormMain : MetroForm
    {
        public FormMain()
        {
            InitializeComponent();

            this.loadAppConfigs();

            this.loadUiLayoutConfigs();
        }

        private string VamtStartScriptPath;
        private string VamtPrepareScriptPath;
        private string VamtCleanupScriptPath;
        private string VamtConfGenPath;
        private string VamtConfPath;


        private void loadAppConfigs()
        {
            this.VamtStartScriptPath = ConfigurationManager.AppSettings.Get("VamtStartScriptPath");
            this.VamtPrepareScriptPath = ConfigurationManager.AppSettings.Get("VamtPrepareScriptPath");
            this.VamtCleanupScriptPath = ConfigurationManager.AppSettings.Get("VamtCleanupScriptPath");
            this.VamtConfGenPath = ConfigurationManager.AppSettings.Get("VamtConfGenPath");
            this.VamtConfPath = ConfigurationManager.AppSettings.Get("VamtConfPath");
        }

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

        private void metroTileConfigure_Click(object sender, EventArgs e)
        {
            string vamtConfGenFullPath = this.getFullPath(this.VamtConfGenPath);

            if (!String.IsNullOrEmpty(this.VamtConfPath))
            {
                string oa3ToolConfFullPath = this.getFullPath(this.VamtConfPath);
                Utility.StartProcess(vamtConfGenFullPath, oa3ToolConfFullPath, true, true);
            }
            else
            {
                Utility.StartProcess(vamtConfGenFullPath, null, true, true);
            }          
        }

        private void metroTileStart_Click(object sender, EventArgs e)
        {
            string scriptFullPath = this.getFullPath(this.VamtStartScriptPath);

            string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\"";

            string args = String.Format(argsTemp, scriptFullPath);

            Utility.StartProcess("PowerShell", args, true, true);
        }

        private void metroTilePrepare_Click(object sender, EventArgs e)
        {
            string scriptFullPath = this.getFullPath(this.VamtPrepareScriptPath);

            string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\"";

            string args = String.Format(argsTemp, scriptFullPath);

            Utility.StartProcess("PowerShell", args, true, true);
        }

        private void metroTileCleanup_Click(object sender, EventArgs e)
        {
            string scriptFullPath = this.getFullPath(this.VamtCleanupScriptPath);

            string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\"";

            string args = String.Format(argsTemp, scriptFullPath);

            Utility.StartProcess("PowerShell", args, true, true);
        }
    }
}
