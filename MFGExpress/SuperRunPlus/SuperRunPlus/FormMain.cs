using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using MetroFramework.Forms;

namespace SuperRunPlus
{
    public partial class FormMain : MetroForm
    {
        public FormMain()
        {
            InitializeComponent();

            CommandItems commandItems = this.getCommandItems();
            
            if (commandItems != null && commandItems.Count > 0)
            {
                this.metroComboBoxOptions.DataSource = commandItems;
                this.metroComboBoxOptions.DisplayMember = "Name";
                this.metroComboBoxOptions.SelectedIndex = 0;
            }

            string shouldWait = ConfigurationManager.AppSettings.Get("ShouldWait");

            this.shouldWaitChild = (shouldWait == "true" || shouldWait == "1");

            string isNewWindow = ConfigurationManager.AppSettings.Get("ShouldCreateNewWindow");

            this.shouldCreateNewWindow = (isNewWindow == "true" || isNewWindow == "1");

            string shellEx = ConfigurationManager.AppSettings.Get("ShouldShellExecute");

            this.shouldShellExecute = (shellEx == "true" || shellEx == "1");

            this.argumentTemplate = ConfigurationManager.AppSettings.Get("ArgumentTemplate");

            this.logPath = ConfigurationManager.AppSettings.Get("LogPath");

            string redirectOutput = ConfigurationManager.AppSettings.Get("ShouldRedirectOutput");

            this.shouldRedirectOutput = (redirectOutput == "true" || redirectOutput == "1");
        }

        private bool shouldWaitChild = false;
        private bool shouldCreateNewWindow = true;
        private bool shouldShellExecute = true;
        private bool shouldRedirectOutput = true;
        private string argumentTemplate = "";
        private string logPath = "";

        private CommandItems getCommandItems()
        {
            //return new List<CommandItem>(new CommandItem[] { new CommandItem() { Name = "Online", Command = "Script\validate-online.ps1" }, new CommandItem() { Name = "Offline",  Command = "Script\validate-offline.ps1" } }).ToArray();

            string commandItemXml = "";
            CommandItems commandItems = null;

            using (FileStream stream = new FileStream("Command-Config.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    commandItemXml = reader.ReadToEnd();
                }
            }

            if (!String.IsNullOrEmpty(commandItemXml))
            {
                commandItems = Utility.XmlDeserialize(commandItemXml, typeof(CommandItems), new Type[] { typeof(CommandItem) }, "utf-8") as CommandItems;
            }

            return commandItems;
        }

        private void metroButtonOK_Click(object sender, EventArgs e)
        {
            string scriptPath = (this.metroComboBoxOptions.SelectedItem as CommandItem).Command;
            string scriptArgs = (this.metroComboBoxOptions.SelectedItem as CommandItem).Arguments;

            if (String.IsNullOrEmpty(scriptPath))
            {
                MessageBox.Show(this, "Script file name should not be null!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!File.Exists(scriptPath))
            {
                MessageBox.Show(this, String.Format("Script file \"{0}\" dose not exist!", scriptPath), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string transactionID = Guid.NewGuid().ToString();

                string logFullPath = Utility.GetFullPath(logPath);

                if (!logFullPath.EndsWith("\\"))
                {
                    logFullPath += "\\";
                }

                logFullPath += transactionID + ".log";

                string scriptFullPath = Utility.GetFullPath(scriptPath);

                string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\"";

                if (!String.IsNullOrEmpty(this.argumentTemplate))
                {
                    argsTemp = this.argumentTemplate;
                }

                string arguments = String.Format(argsTemp, scriptFullPath);

                if (!String.IsNullOrEmpty(scriptArgs))
                {
                    if (scriptArgs.Contains("-TransactionID {0}"))
                    {
                        scriptArgs = String.Format(scriptArgs, transactionID);
                    }

                    arguments = String.Format("{0} {1}", arguments, scriptArgs);
                }

               string output = Utility.StartProcess("PowerShell", arguments, this.shouldCreateNewWindow, this.shouldShellExecute, this.shouldWaitChild);

                if (!String.IsNullOrEmpty(output) && this.shouldRedirectOutput)
                {
                    using (FileStream stream = new FileStream(logFullPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(output);
                        }
                    }
                }
            }
        }
    }
}
