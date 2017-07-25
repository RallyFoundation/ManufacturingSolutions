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

        public FormGetInputPath(string Title, string Message, string PopupMessage)
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

            if (!String.IsNullOrEmpty(PopupMessage))
            {
                this.popupMessage = PopupMessage;
            }
        }

        private string popupMessage = "Please specify input path!";

        public string InputPath { get { return this.metroTextBoxInputPath.Text; } }

        public bool AbortOnCancel { get; set; }

        private void metroButtonOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.metroTextBoxInputPath.Text))
            {
                MessageBox.Show(this, popupMessage, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void metroButtonCancelExit_Click(object sender, EventArgs e)
        {
            if (this.AbortOnCancel)
            {
                this.DialogResult = DialogResult.Abort;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }
    }
}
