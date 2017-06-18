using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace PowerShellDataProcessing
{
    public partial class FormGetInputPath : MetroForm
    {
        public FormGetInputPath()
        {
            InitializeComponent();
        }

        public FormGetInputPath(string Title, string Message)
        {
            InitializeComponent();

            if (!String.IsNullOrEmpty(Title))
            {
                this.Text = Title;
            }

            if (!String.IsNullOrEmpty(Message))
            {
                this.metroLabelMessage.Text = Message;
            }
        }

        public string InputPath { get { return this.metroTextBoxInputPath.Text; } }

        private void metroButtonOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.metroTextBoxInputPath.Text))
            {
                MessageBox.Show(this, "Please specify input path!", "Please specify input path!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void metroButtonBrowse_Click(object sender, EventArgs e)
        {
            if (this.openFileDialogInputPath.ShowDialog(this) == DialogResult.OK)
            {
                this.metroTextBoxInputPath.Text = this.openFileDialogInputPath.FileName;
            }
        }
    }
}
