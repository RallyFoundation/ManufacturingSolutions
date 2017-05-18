using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace OA3ToolConfGen
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))] 
    public partial class UCParameter : UserControl
    {
        public UCParameter()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        public ParameterType ParameterType { get; set; }

        public string DBConnectionString { get; set; }

        public string ConfigurationID { get; set; }

        public bool IsSelected { get { return this.checkBoxSelection.Checked; } }

        public string ParameterName { get { return this.ParameterType.ToString(); } }
        public string ParameterValue { get { return this.comboBoxParameterValue.Text; } }

        const string SQLCommandText = "SELECT DISTINCT {0} FROM ProductKeyInfo WHERE ProductKeyID IN (SELECT ProductKeyID FROM KeyInfoEx WHERE CloudOA_BusinessId = '{1}' AND KeyType = {2})";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.setLabelText();
        }

        public bool Populate() 
        {
            if (!String.IsNullOrEmpty(this.DBConnectionString))
            {
                this.comboBoxParameterValue.DataSource = this.getValuesFromDB(this.DBConnectionString, this.ParameterType.ToString());
                this.comboBoxParameterValue.Refresh();

                return (this.comboBoxParameterValue.Items.Count > 0);
            }

            return false;
        }

        public void Enable(bool IsEnabled) 
        {
            this.checkBoxSelection.Checked = IsEnabled;
            this.buttonGet.Enabled = IsEnabled && !String.IsNullOrEmpty(this.DBConnectionString);
        }

        public void SetParameterValue(string Value) 
        {
            if ((this.comboBoxParameterValue.Items != null) && (this.comboBoxParameterValue.Items.Contains(Value)))
            {
                this.comboBoxParameterValue.SelectedItem = Value;
            }
            else
            {
                this.comboBoxParameterValue.Text = Value;
            }
        }

        private void checkBoxSelection_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as CheckBox).Checked;

            this.comboBoxParameterValue.Enabled = isChecked;
            this.buttonGet.Enabled = isChecked && !String.IsNullOrEmpty(this.DBConnectionString);

            if (!isChecked)
            {
                this.comboBoxParameterValue.DataSource = null;
                this.comboBoxParameterValue.Items.Clear();
                this.comboBoxParameterValue.Text = "";
            }
        }

        private void setLabelText() 
        {
            switch (this.ParameterType)
            {
                case ParameterType.LicensablePartNumber:
                    this.labelParameterName.Text = "Licensable Part Number: ";
                    break;
                case ParameterType.OEMPartNumber:
                    this.labelParameterName.Text = "OEM Part Number: ";
                    break;
                case ParameterType.OEMPONumber:
                    this.labelParameterName.Text = "OEM PO Number: ";
                    break;
                case ParameterType.SerialNumber:
                    this.labelParameterName.Text = "Serial Number: ";
                    break;
                default:
                    break;
            }
        }

        private List<String> getValuesFromDB(string connectionString, string columName) 
        {
            List<string> returnValue = new List<string>();

            try
            {
                string commandText = String.Format(SQLCommandText, columName, ConfigurationID, ModuleConfiguration.KeyTypeID);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = conn.CreateCommand();
                    sqlCommand.CommandText = commandText;
                    sqlCommand.CommandType = CommandType.Text;

                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    using (SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            returnValue.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }

            return returnValue;
        }

        private void buttonGet_Click(object sender, EventArgs e)
        {
            this.Populate();
        }
    }
}


