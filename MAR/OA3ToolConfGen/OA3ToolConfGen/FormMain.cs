using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DISAdapter;
using Contract;

namespace OA3ToolConfGen
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            if (!ModuleConfiguration.ShowConfigurationsFromServer)
            {
                this.tabControlMain.Controls.Remove(this.tabPageCloud);
            }
        }

        public FormMain(string ConfigFilePath)
        {
            this.currentFilePath = ConfigFilePath;
            InitializeComponent();

            if (!ModuleConfiguration.ShowConfigurationsFromServer)
            {
                this.tabControlMain.Controls.Remove(this.tabPageCloud);
            }    
        }

        private string currentFilePath;

        protected OA3ToolConfiguration OA3ToolConfiguration = new OA3ToolConfiguration();

        public Dictionary<string, object> Settings = new Dictionary<string, object>() 
        {
            {ModuleConfiguration.AppStateKey_DBConnectionString, ""},
            {ModuleConfiguration.AppStateKey_CloudConfigurationID, ""},
            {ModuleConfiguration.AppStateKey_CloudServicePoint, ""},
            //{ModuleConfiguration.AppStateKey_CloudUserName, "MDOS"},
            //{ModuleConfiguration.AppStateKey_CloudPassword, "D!S@OMSG.msft"},
            {ModuleConfiguration.AppStateKey_CloudUserName, ModuleConfiguration.Configuration_Database_Username},
            {ModuleConfiguration.AppStateKey_CloudPassword, ModuleConfiguration.Configuration_Database_Password},
            {ModuleConfiguration.AppStateKey_OA3ToolConfiguration, null},
            {ModuleConfiguration.AppStateKey_CloudConfigurationSets, null},
            //{ModuleConfiguration.AppStateKey_CloudClientDBName, "MDOSKeyStore_CloudOA"},
            {ModuleConfiguration.AppStateKey_CloudClientDBName, ModuleConfiguration.Configuration_Database_Name},
            {ModuleConfiguration.AppStateKey_KeyTypeID, ModuleConfiguration.KeyTypeID}
        };

        private void saveAppState()
        {
            string filePath = Application.StartupPath;

            if (!filePath.EndsWith("\\"))
            {
                filePath += "\\";
            }

            filePath += "appstate.dat";

            this.setOA3ToolConfiguration();

            this.Settings[ModuleConfiguration.AppStateKey_OA3ToolConfiguration] = this.OA3ToolConfiguration;
            this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationSets] = null;
            this.Settings[ModuleConfiguration.AppStateKey_KeyTypeID] = ModuleConfiguration.KeyTypeID;

            byte[] bytes = Utility.BinarySerialize(this.Settings);

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private void loadAppState()
        {
            string filePath = Application.StartupPath;

            if (!filePath.EndsWith("\\"))
            {
                filePath += "\\";
            }

            filePath += "appstate.dat";

            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);

                this.Settings = Utility.BinaryDeserialize(bytes) as Dictionary<string, object>;

                if (this.Settings != null)
                {
                    this.OA3ToolConfiguration = this.Settings[ModuleConfiguration.AppStateKey_OA3ToolConfiguration] as OA3ToolConfiguration;

                    if (this.Settings.ContainsKey(ModuleConfiguration.AppStateKey_KeyTypeID))
                    {
                        ModuleConfiguration.KeyTypeID = (int)(this.Settings[ModuleConfiguration.AppStateKey_KeyTypeID]);
                    }  
                }
            }
        }

        private void setOA3ToolConfiguration() 
        {
            this.OA3ToolConfiguration = new OA3ToolConfiguration()
            {
                ServerBased = new OA3ServerBased()
                {
                    KeyProviderServerLocation = new OA3ServerBasedKeyProviderServerLocation()
                    {
                        IPAddress = this.textBoxKPSAddress.Text,
                        EndPoint = this.textBoxKeyProviderServicePortNumber.Text,
                        ProtocolSequence = ModuleConfiguration.OA3ToolConfigurationValue_ProtocolSequence,
                        Options = this.checkBoxEnable4KHardwareHash.Checked ? new OA3ServerBasedOptions()
                        {
                            HardwareHashVersion = ModuleConfiguration.OA3ToolConfigurationValue_Options_HardwareHashVersion,
                            HardwareHashPadding = ModuleConfiguration.OA3ToolConfigurationValue_Options_HardwareHashPadding
                        } : null
                    },
                    Parameters = new OA3ServerBasedParameters()
                    {
                        //CloudConfigurationID = this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID] != null ? this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID].ToString() : "", 
                        CloudConfigurationID = !String.IsNullOrEmpty((string)this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID]) ? this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID].ToString() : this.textBoxCloudConfigurationID.Text,
                    }
                },

                OutputData = new OA3OutputData()
                {
                    AssembledBinaryFile = this.textBoxBinFileOutputPath.Text,
                    ReportedXMLFile = this.textBoxXMLResultFileOutputPath.Text
                }
            };

            List<OA3ServerBasedParametersParameter> parameters = new List<OA3ServerBasedParametersParameter>();

            if (this.ucParameterLPN.IsSelected)
            {
                parameters.Add(new OA3ServerBasedParametersParameter() { name = this.ucParameterLPN.ParameterName, value = this.ucParameterLPN.ParameterValue });
            }

            if (this.ucParameterOPN.IsSelected)
            {
                parameters.Add(new OA3ServerBasedParametersParameter() { name = this.ucParameterOPN.ParameterName, value = this.ucParameterOPN.ParameterValue });
            }

            if (this.ucParameterOPON.IsSelected)
            {
                parameters.Add(new OA3ServerBasedParametersParameter() { name = this.ucParameterOPON.ParameterName, value = this.ucParameterOPON.ParameterValue });
            }

            if (this.ucParameterSN.IsSelected)
            {
                parameters.Add(new OA3ServerBasedParametersParameter() { name = this.ucParameterSN.ParameterName, value = this.ucParameterSN.ParameterValue });
            }

            this.OA3ToolConfiguration.ServerBased.Parameters.Parameter = parameters.ToArray();

            if (this.checkBoxOHRRequired.Checked)
            {
                Dictionary<string, string> ohrValues = this.ucohrOHRData.GetOHR();

                this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo = new OA3ServerBasedParametersOEMOptionalInfoField[ohrValues.Count];

                for (int i = 0; i < ohrValues.Keys.Count; i++)
                {
                    this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo[i] = new OA3ServerBasedParametersOEMOptionalInfoField()
                    {
                        Name = ohrValues.Keys.ToArray()[i],
                        Value = ohrValues.Values.ToArray()[i]
                    };
                }
            }

            if (this.checkBoxPromoCodeRequired.Checked)
            {
                string promoCodes = this.ucpgmeligPromoCode.GetPromoCodes();

                OA3ServerBasedParametersOEMOptionalInfoField oemOptionalInfoZPGMELIG = new OA3ServerBasedParametersOEMOptionalInfoField()
                {
                    Name = ModuleConfiguration.OEMOptionalInfoKey_ZPGM_ELIG_VAL,
                    Value = promoCodes
                };

                if (this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo == null || this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo.Length <= 0)
                {
                    this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo = new OA3ServerBasedParametersOEMOptionalInfoField[]{oemOptionalInfoZPGMELIG};
                }
                else
                {
                    List<OA3ServerBasedParametersOEMOptionalInfoField> oemOptionalInfoes = new List<OA3ServerBasedParametersOEMOptionalInfoField>(this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo);
                    oemOptionalInfoes.Add(oemOptionalInfoZPGMELIG);
                    this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo = oemOptionalInfoes.ToArray();
                }
            }
        }

        private string getOA3ToolConfigurationXml() 
        {
            string returnValue = "";

            this.setOA3ToolConfiguration();

            //Type[] types = new Type[] { typeof(OA3ServerBased), typeof(OA3ServerBasedKeyProviderServerLocation), typeof(OA3ServerBasedParameters), typeof(OA3ServerBasedParametersOEMOptionalInfoField), typeof(OA3ServerBasedParametersParameter), typeof(OA3OutputData) };

            returnValue = Utility.XmlSerialize(this.OA3ToolConfiguration, null, "UTF-8");

            return returnValue;
        }

        private string getCloudConfigSetsXml() 
        {
            string returnValue = "";

            Customer[] customers = this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationSets] as Customer[];

            if (customers != null)
            {
                returnValue = Utility.XmlSerialize(customers, null, "UTF-8");
            }
            
            return returnValue;
        }

        private void openConfig(string configFilePath) 
        {
            string oa3ConfigXml = "";

            using (FileStream stream = new FileStream(this.currentFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    oa3ConfigXml = reader.ReadToEnd();
                }
            }

            try
            {
                //Type[] types = new Type[] { typeof(OA3ServerBased), typeof(OA3ServerBasedKeyProviderServerLocation), typeof(OA3ServerBasedParameters), typeof(OA3ServerBasedParametersOEMOptionalInfoField), typeof(OA3ServerBasedParametersParameter), typeof(OA3OutputData) };

                DISAdapter.OA3ToolConfiguration oa3ToolConf = Utility.XmlDeserialize(oa3ConfigXml, typeof(OA3ToolConfiguration), null, "UTF-8") as OA3ToolConfiguration;

                if (oa3ToolConf != null)
                {
                    this.OA3ToolConfiguration = oa3ToolConf;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = String.Format("Error(s) occurred: {0}", ex.ToString());
                MessageBox.Show(errorMessage, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            try
            {
                string url = this.Settings[ModuleConfiguration.AppStateKey_CloudServicePoint].ToString();
                string userName = this.Settings[ModuleConfiguration.AppStateKey_CloudUserName].ToString();
                string password = this.Settings[ModuleConfiguration.AppStateKey_CloudPassword].ToString();
                string dbConnectionString = ModuleConfiguration.GetDBConnectionString(url, userName, password, this.OA3ToolConfiguration.ServerBased.Parameters.CloudConfigurationID);

                this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] = dbConnectionString;
            }
            catch (Exception ex)
            {
                this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] = null;
            }

            this.setControls();

            this.toolStripStatusLabel1.Text = this.currentFilePath;
        }

        private void setControls() 
        {
            string dbConnectionString = (this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] != null) ? this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString].ToString() : "";

            this.ucParameterLPN.DBConnectionString = dbConnectionString;
            this.ucParameterOPN.DBConnectionString = dbConnectionString;
            this.ucParameterOPON.DBConnectionString = dbConnectionString;
            this.ucParameterSN.DBConnectionString = dbConnectionString;

            this.ucParameterLPN.Enable(false);
            this.ucParameterOPN.Enable(false);
            this.ucParameterOPON.Enable(false);
            this.ucParameterSN.Enable(false);

            if (this.OA3ToolConfiguration.ServerBased != null)
            {
                if (this.OA3ToolConfiguration.ServerBased.KeyProviderServerLocation != null)
                {
                    this.textBoxKPSAddress.Text = this.OA3ToolConfiguration.ServerBased.KeyProviderServerLocation.IPAddress;
                    this.textBoxKeyProviderServicePortNumber.Text = this.OA3ToolConfiguration.ServerBased.KeyProviderServerLocation.EndPoint;

                    this.checkBoxEnable4KHardwareHash.Checked = (this.OA3ToolConfiguration.ServerBased.KeyProviderServerLocation.Options != null);
                }

                if (this.OA3ToolConfiguration.ServerBased.Parameters != null)
                {
                    //if (this.OA3ToolConfiguration.ServerBased.Parameters.CloudConfigurationID != null)
                    //{
                    //    this.textBoxCloudConfigurationID.Text = this.OA3ToolConfiguration.ServerBased.Parameters.CloudConfigurationID;
                    //}
                    //else
                    //{
                    //    this.textBoxCloudConfigurationID.Text = (this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID] != null) ? this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID].ToString() : "";
                    //}

                    string configurationID = this.OA3ToolConfiguration.ServerBased.Parameters.CloudConfigurationID;

                    this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID] = configurationID;

                    this.ucParameterLPN.ConfigurationID = configurationID;
                    this.ucParameterOPN.ConfigurationID = configurationID;
                    this.ucParameterOPON.ConfigurationID = configurationID;
                    this.ucParameterSN.ConfigurationID = configurationID;

                    if (this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo != null)
                    {
                        if (this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo.Length >= 5 && this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo.FirstOrDefault(o => o.Name == ModuleConfiguration.OHRKey_ZPC_MODEL_SKU) != null)
                        {
                            this.checkBoxOHRRequired.Checked = true;

                            Dictionary<string, string> ohrData = new Dictionary<string, string>();

                            foreach (var item in this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo)
                            {
                                if (item.Name != ModuleConfiguration.OEMOptionalInfoKey_ZPGM_ELIG_VAL)
                                {
                                    ohrData.Add(item.Name, item.Value);
                                }   
                            }

                            this.ucohrOHRData.SetOHR(ohrData);
                        }

                        OA3ServerBasedParametersOEMOptionalInfoField oemOptionalInfoZPGMELIG = this.OA3ToolConfiguration.ServerBased.Parameters.OEMOptionalInfo.FirstOrDefault(o => o.Name == ModuleConfiguration.OEMOptionalInfoKey_ZPGM_ELIG_VAL);

                        if (oemOptionalInfoZPGMELIG != null)
                        {
                            this.checkBoxPromoCodeRequired.Checked = true;

                            this.ucpgmeligPromoCode.SetPromoCodes(oemOptionalInfoZPGMELIG.Value);
                        }
                    }

                    if ((this.OA3ToolConfiguration.ServerBased.Parameters.Parameter != null) && (this.OA3ToolConfiguration.ServerBased.Parameters.Parameter.Length > 0))
                    {
                        foreach (var parameter in this.OA3ToolConfiguration.ServerBased.Parameters.Parameter)
                        {
                            if (this.ucParameterLPN.ParameterName == parameter.name)
                            {
                                this.ucParameterLPN.Enable(true);
                                this.ucParameterLPN.Populate();
                                this.ucParameterLPN.SetParameterValue(parameter.value);
                            }
                            else if (this.ucParameterOPN.ParameterName == parameter.name) 
                            {
                                this.ucParameterOPN.Enable(true);
                                this.ucParameterOPN.Populate();
                                this.ucParameterOPN.SetParameterValue(parameter.value);
                            }
                            else if (this.ucParameterOPON.ParameterName == parameter.name)
                            {
                                this.ucParameterOPON.Enable(true);
                                this.ucParameterOPON.Populate();
                                this.ucParameterOPON.SetParameterValue(parameter.value);
                            }
                            else if (this.ucParameterSN.ParameterName == parameter.name)
                            {
                                this.ucParameterSN.Enable(true);
                                this.ucParameterSN.Populate();
                                this.ucParameterSN.SetParameterValue(parameter.value);
                            }
                        }
                    }
                }
            }

            this.textBoxCloudConfigurationID.Text = (this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID] != null) ? this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID].ToString() : "";

            if (this.OA3ToolConfiguration.OutputData != null)
            {
                this.textBoxBinFileOutputPath.Text = this.OA3ToolConfiguration.OutputData.AssembledBinaryFile;
                this.textBoxXMLResultFileOutputPath.Text = this.OA3ToolConfiguration.OutputData.ReportedXMLFile;
            }

            switch (ModuleConfiguration.KeyTypeID)
            {
                case 1:
                    this.radioButtonStandard.Checked = true;
                    break;
                case 2:
                    this.radioButtonMBR.Checked = true;
                    break;
                case 4:
                    this.radioButtonMAT.Checked = true;
                    break;
                default:
                    this.radioButtonStandard.Checked = true;
                    break;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.loadAppState();

            this.setControls();

            if (!String.IsNullOrEmpty(this.currentFilePath))
            {
                this.openConfig(this.currentFilePath);

                if ((this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] == null) || (this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] == ""))
                {
                    this.buttonMore_Click(sender, e);
                }
            }
        }

        private void checkBoxOHRRequired_CheckedChanged(object sender, EventArgs e)
        {
            this.ucohrOHRData.Enabled = this.checkBoxOHRRequired.Checked;
        }

        private void buttonMore_Click(object sender, EventArgs e)
        {
            FormCloudConfigDialog formCloudConfig = new FormCloudConfigDialog(this.Settings);

            if (formCloudConfig.ShowDialog(this) == DialogResult.OK)
            {
                //this.textBoxCloudConfigurationID.Text = this.Settings[ModuleConfiguration.AppStateKey_CloudConfigurationID].ToString(); //this.OA3ToolConfiguration.Parameters[0].CloudConfigurationID;

                //this.ucParameterLPN.DBConnectionString = this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString].ToString();
                //this.ucParameterOPN.DBConnectionString = this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString].ToString();
                //this.ucParameterOPON.DBConnectionString = this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString].ToString();
                //this.ucParameterSN.DBConnectionString = this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString].ToString();

                if (this.Settings != null)
                {
                    this.OA3ToolConfiguration = this.Settings[ModuleConfiguration.AppStateKey_OA3ToolConfiguration] as OA3ToolConfiguration;
                }

                this.setControls();
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string hostName = this.textBoxKPSAddress.Text;
            string portNumber = this.textBoxKeyProviderServicePortNumber.Text;

            if (String.IsNullOrEmpty(hostName))
            {
                MessageBox.Show("Key Provider Service Address is required!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (String.IsNullOrEmpty(portNumber))
            {
                MessageBox.Show("Key Provider Service Port Number is required!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            int portNumberInt = -1;

            if (!int.TryParse(portNumber, out portNumberInt))
            {
                MessageBox.Show("Key Provider Service Port Number is NOT a valid integer value!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            string message = "";

            try
            {
                System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient();

                tcpClient.Connect(hostName, portNumberInt);

                message = String.Format("Connection test success! The port number {0} on host {1} is active and listening.", portNumber, hostName);

                if (MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                {
                    tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                message = String.Format("Connection test to the port number {0} on host {1} failed. Error detail: {2}", portNumber, hostName, ex.Message);

                MessageBox.Show(message, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(this.currentFilePath)) && (this.saveFileDialogSave.ShowDialog(this) == DialogResult.OK))
            {
                this.currentFilePath = this.saveFileDialogSave.FileName;
            }

            if (!string.IsNullOrEmpty(this.currentFilePath))
            {
                try
                {
                    string xml = this.getOA3ToolConfigurationXml();

                    using (FileStream stream = new FileStream(this.currentFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                        {
                            writer.Write(xml);
                        }
                    }

                    string message = String.Format("OA3Tool configuration settings successfully saved to \"{0}\"", this.currentFilePath);

                    if (MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.toolStripStatusLabel1.Text = this.currentFilePath;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = String.Format("Error(s) occurred: {0}", ex.ToString());
                    MessageBox.Show(errorMessage, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialogOpen.ShowDialog(this) == DialogResult.OK)
            {
                this.currentFilePath = this.openFileDialogOpen.FileName;

                this.openConfig(this.currentFilePath);

                if ((this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] == null) || (this.Settings[ModuleConfiguration.AppStateKey_DBConnectionString] == ""))
                {
                    this.buttonMore_Click(sender, e);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.currentFilePath))
            {
                string message = String.Format("Save the OA3Tool configuration settings to \"{0}\" ?", this.currentFilePath);

                if (MessageBox.Show(message, "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.saveToolStripMenuItem_Click(sender, e);
                }
            }

            this.Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.saveAppState();
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControlMain.SelectedTab == this.tabPagePreview)
            {
                string oa3ToolConfigXml = this.getOA3ToolConfigurationXml();

                this.webBrowserPreview.DocumentText = oa3ToolConfigXml;
            }
            else if (this.tabControlMain.SelectedTab == this.tabPageCloud)
            {
                this.webBrowserCloud.DocumentText = this.getCloudConfigSetsXml();
            }
        }

        private void toolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialogSave.ShowDialog(this) == DialogResult.OK)
            {
                this.currentFilePath = this.saveFileDialogSave.FileName;

                try
                {
                    string xml = this.getOA3ToolConfigurationXml();

                    using (FileStream stream = new FileStream(this.currentFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                        {
                            writer.Write(xml);
                        }
                    }

                    string message = String.Format("OA3Tool configuration settings successfully saved to \"{0}\"", this.currentFilePath);

                    if (MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.toolStripStatusLabel1.Text = this.currentFilePath;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = String.Format("Error(s) occurred: {0}", ex.ToString());
                    MessageBox.Show(errorMessage, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog(this);
        }

        private void checkBoxPromoCodeRequired_CheckedChanged(object sender, EventArgs e)
        {
            this.ucpgmeligPromoCode.Enabled = this.checkBoxPromoCodeRequired.Checked;
        }

        private void radioButtonStandard_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                ModuleConfiguration.KeyTypeID = 1;
            }
        }

        private void radioButtonMBR_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                ModuleConfiguration.KeyTypeID = 2;
            }
        }

        private void radioButtonMAT_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                ModuleConfiguration.KeyTypeID = 4;
            }
        }
    }
}
