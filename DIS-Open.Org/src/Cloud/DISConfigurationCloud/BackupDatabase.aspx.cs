using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DISConfigurationCloud.Security;
using DISConfigurationCloud.StorageManagement;
using DISConfigurationCloud.Utility;

namespace DISConfigurationCloud
{
    public partial class DatabaseBackupWizard : AuthPageBase
    {
        private IDatabaseManager databaseManager = null;
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void ButtonBackupDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                string databaseName = "";
                string customerName = this.CustomerDatabaseSelector.SelectedCustomerName;
                string dbConnectionString = this.CustomerDatabaseSelector.SelectedDBConnectionString;

                string script = "window.alert('Please select a database to backup!');";

                if (String.IsNullOrEmpty(dbConnectionString))
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                    return;
                }

                if (this.backupDatabase(customerName, dbConnectionString, out databaseName))
                {
                    //script = String.Format("window.alert('Database \"{0}\" has been successfully backed up to backup device \"{1}\".')", databaseName, customerName);
                    //this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);

                    string redirectUrl = String.Format("~/BackupRecords.aspx?CustomerID={0}&ConfigID={1}", this.CustomerDatabaseSelector.SelectedCustomerID, this.CustomerDatabaseSelector.SelectedConfigurationID);

                    Response.Redirect(redirectUrl);
                }
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }

        private bool backupDatabase(string customerName, string dbConnectionString, out string dbName) 
        {
            if (this.databaseManager == null)
            {
                this.databaseManager = new DatabaseManager();
            }

            string connectionString = dbConnectionString;

            string[] connectionInfo = connectionString.Split(new string[] { ";" }, StringSplitOptions.None);

            string serverName = connectionInfo[0].Split(new string[]{"="},  StringSplitOptions.None)[1];
            string databaseName = connectionInfo[1].Split(new string[]{"="},  StringSplitOptions.None)[1];
            string userName = connectionInfo[2].Split(new string[]{"="},  StringSplitOptions.None)[1];
            string password = connectionInfo[3].Split(new string[]{"="},  StringSplitOptions.None)[1];

            connectionString = this.databaseManager.GetConnectionString(serverName, "master", userName, password);

            string[] backupDevices = this.databaseManager.ListBackupDevices(connectionString);

            string backupDeviceName = customerName;

            bool deviceExists = false;

            if (backupDevices != null)
            {
                foreach (string deviceName in backupDevices)
                {
                    if (deviceName.ToLower() == backupDeviceName.ToLower())
                    {
                        deviceExists = true;
                        break;
                    }
                }
            }

            if (!deviceExists)
            {
                this.databaseManager.CreateBackupDevice(backupDeviceName, connectionString);
            }

            dbName = databaseName;

            int result = this.databaseManager.BackupDatabase(backupDeviceName, databaseName, connectionString);

            return true;
        }
    }
}