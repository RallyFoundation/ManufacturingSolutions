using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace DISManagementCenter
{
    public partial class FormDataMigration : MetroForm
    {
        public FormDataMigration(MetroForm Caller)
        {
            InitializeComponent();

            this.caller = Caller;
        }

        private MetroForm caller;

        private void metroLinkBack_Click(object sender, EventArgs e)
        {
            if (this.caller != null)
            {
                this.caller.Visible = true;
                this.Visible = false;
            }
        }

        private void metroButtonBrowse_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialogDataPackage.ShowDialog() == DialogResult.OK)
            {
                this.metroTextBoxDataPackage.Text = this.folderBrowserDialogDataPackage.SelectedPath;
            }
        }

        private void FormDataMigration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.caller != null)
            {
                this.caller.Visible = true;
            }
        }
    }
}
