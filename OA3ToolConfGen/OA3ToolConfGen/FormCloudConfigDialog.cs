using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DISAdapter;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Contract;

namespace OA3ToolConfGen
{
    public partial class FormCloudConfigDialog : Form
    {
        public FormCloudConfigDialog()
        {
            InitializeComponent();

            this.textBoxDatabaseName.Text = ModuleConfiguration.Configuration_Database_Name;
        }

        private Dictionary<string, object> Settings;

        private Customer[] currentCustomers = null;

        public FormCloudConfigDialog(Dictionary<string, object> Settings)
        {
            InitializeComponent();

            this.Settings = Settings;

            this.textBoxDISConfiguraitonCloudUrl.Text = this.Settings[ModuleConfiguration.AppStateKey_CloudServicePoint].ToString();
            this.textBoxUserName.Text = this.Settings[ModuleConfiguration.AppStateKey_CloudUserName].ToString();
            this.textBoxPassword.Text = this.Settings[ModuleConfiguration.AppStateKey_CloudPassword].ToString();

            this.currentCustomers = (this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationSets] != null) ? this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationSets] as Customer[] : null;

            this.comboBoxBusiness.DataSource = this.currentCustomers;

            if (this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID] != null)
            {
                this.comboBoxBusiness.SelectedItem = ModuleConfiguration.GetConfigurationSetByConfigurationID(this.currentCustomers, this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID].ToString());
            }

            if (this.Settings[ModuleConfiguration.AppStateKey_CloudClientDBName] != null)
            {
                ModuleConfiguration.Configuration_Database_Name = this.Settings[ModuleConfiguration.AppStateKey_CloudClientDBName].ToString();
            }

            this.textBoxDatabaseName.Text = ModuleConfiguration.Configuration_Database_Name;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if ((this.currentCustomers != null) && (this.comboBoxBusiness.SelectedIndex >= 0))
            {
                //this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID] = this.currentCustomers.FirstOrDefault((o) => (o.ID == this.comboBoxBusiness.SelectedValue.ToString())).Configurations.FirstOrDefault((c) => (c.ConfigurationType == ConfigurationType.FactoryFloor)).ID;
                //this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] = this.currentCustomers.FirstOrDefault((o) => (o.ID == this.comboBoxBusiness.SelectedValue.ToString())).Configurations.FirstOrDefault((c) => (c.ConfigurationType == ConfigurationType.FactoryFloor)).DbConnectionString;
                
                Configuration ffConfig = ModuleConfiguration.GetFactoryFloorConfigurationByBusinessID(this.currentCustomers, this.comboBoxBusiness.SelectedValue.ToString());
                this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID] = ffConfig.ID;
                this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] = ffConfig.DbConnectionString;

                OA3ToolConfiguration oa3Config = this.Settings[ModuleConfiguration.AppStateKey_OA3ToolConfiguration] as OA3ToolConfiguration;

                if (oa3Config == null)
                {
                    oa3Config = new OA3ToolConfiguration();
                }

                if (oa3Config.ServerBased == null)
                {
                    oa3Config.ServerBased = new OA3ServerBased();
                }

                if (oa3Config.ServerBased.Parameters == null)
                {
                    oa3Config.ServerBased.Parameters = new OA3ServerBasedParameters();
                }

                oa3Config.ServerBased.Parameters.CloudConfigurationID = ffConfig.ID;

                this.Settings[ModuleConfiguration.AppStateKey_OA3ToolConfiguration] = oa3Config;
            }

            this.Settings[ModuleConfiguration.AppStateKey_CloudServicePoint] = this.textBoxDISConfiguraitonCloudUrl.Text;
            this.Settings[ModuleConfiguration.AppStateKey_CloudUserName] = this.textBoxUserName.Text;
            this.Settings[ModuleConfiguration.AppStateKey_CloudPassword] = this.textBoxPassword.Text;
            this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationSets] = this.currentCustomers;
            this.Settings[ModuleConfiguration.AppStateKey_CloudClientDBName] = this.textBoxDatabaseName.Text;

            DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxDISConfiguraitonCloudUrl.Text))
            {
                MessageBox.Show("Host address for MDOS Client database server is required!", "Information Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (String.IsNullOrEmpty(this.textBoxDatabaseName.Text))
            {
                MessageBox.Show("Database name for MDOS Client is required!", "Information Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (String.IsNullOrEmpty(this.textBoxUserName.Text))
            {
                MessageBox.Show("User Name for MDOS Client database server is required!", "Information Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (String.IsNullOrEmpty(this.textBoxPassword.Text))
            {
                MessageBox.Show("Password for MDOS Client database server is required!", "Information Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ModuleConfiguration.Configuration_Database_Name = this.textBoxDatabaseName.Text;

                this.currentCustomers = ModuleConfiguration.GetFactoryFloorConfigurationSets(this.textBoxDISConfiguraitonCloudUrl.Text, this.textBoxUserName.Text, this.textBoxPassword.Text);

                this.comboBoxBusiness.DataSource = this.currentCustomers;

                if ((this.currentCustomers != null) && (this.currentCustomers.Length > 0))
                {
                    this.comboBoxBusiness.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = String.Format("Error(s) occurred connecting to DIS Configuration Cloud: {0}", ex.ToString());
                MessageBox.Show(errorMessage, "Cloud Connection Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBoxShowPasswordContent_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPassword.UseSystemPasswordChar = !(sender as CheckBox).Checked;
        }
    }
}
