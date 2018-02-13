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
using Utility;

namespace LookupManager
{
    public partial class FormMain : Form
    {
        private string currentFilePath = "lookup.xml";

        public FormMain()
        {
            InitializeComponent();
        }

        public FormMain(string ConfigFilePath)
        {
            this.currentFilePath = ConfigFilePath;
            InitializeComponent();
        }

        private void saveMapping(string key, string value)
        {
            Mapping mapping = new Mapping()
            {
                Key = key,
                Value = value
            };

            string xml = XmlUtility.XmlSerialize(mapping, null, "utf-8");

            using (FileStream stream = new FileStream(this.currentFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(xml);
                }
            }
        }

        private void loadMapping()
        {
            Mapping mapping = null;

            string xml = "<Mapping><Key/><Value/></Mapping>";

            using (FileStream stream = new FileStream(this.currentFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    xml = reader.ReadToEnd();
                }
            }

            mapping = XmlUtility.XmlDeserialize(xml, typeof(Mapping), null, "utf-8") as Mapping;

            this.textBoxKey.Text = mapping.Key;
            this.textBoxValue.Text = mapping.Value;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxKey.Text))
            {
                MessageBox.Show("Key is required!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (String.IsNullOrEmpty(this.textBoxValue.Text))
            {
                MessageBox.Show("Value is required!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            this.saveMapping(this.textBoxKey.Text, this.textBoxValue.Text);

            string message = String.Format("Key/Value mapping configuration settings successfully saved to \"{0}\"", this.currentFilePath);

            if (MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                this.statusStripLabel1.Text = this.currentFilePath;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.saveMapping(this.textBoxKey.Text, this.textBoxValue.Text);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.loadMapping();
        }
    }
}
