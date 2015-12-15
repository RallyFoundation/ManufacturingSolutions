using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace MfgSolutionsDeploymentCenter
{
    public partial class FormInstallation : MetroForm
    {
        public FormInstallation(MetroForm Sender)
        {
            InitializeComponent();
            this.sender = Sender;
        }

        private MetroForm sender;
        private FormCloudInstallation formCloudInstallation;

        private void FormInstallation_FormClosing(object sender, FormClosingEventArgs e)
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

        private void metroTileDISConfigurationCloud_Click(object sender, EventArgs e)
        {
            if (this.formCloudInstallation == null)
            {
                this.formCloudInstallation = new FormCloudInstallation(this);
                this.formCloudInstallation.Show();
            }
            else
            {
                this.formCloudInstallation.Visible = true;
            }

            this.Visible = false;
        }
    }
}
