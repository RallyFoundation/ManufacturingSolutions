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
        }

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
                string scriptFullPath = Utility.GetFullPath(scriptPath);

                string argsTemp = "-ExecutionPolicy ByPass -NoExit -File \"{0}\"";

                string arguments = String.Format(argsTemp, scriptFullPath);

                if (!String.IsNullOrEmpty(scriptArgs))
                {
                    arguments = String.Format("{0} {1}", arguments, scriptArgs);
                }

               Utility.StartProcess("PowerShell", arguments, true, true);
            }
        }
    }
}
