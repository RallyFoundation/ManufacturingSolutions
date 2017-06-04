using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DISConfigurationCloud.Security;
using DISConfigurationCloud.StorageManagement;
//using DISConfigurationCloud.Utility;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.Facade;

namespace DISConfigurationCloud
{
    public partial class RestoreDatabase : AuthPageBase
    {
        IDatabaseManager databaseManager;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonRestoreDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                int backupSequence = this.DBBackupDetail.GetSelectedBakcupSequence();

                string script = "window.alert('Please select a database, and its relative backup from the back records before restore!');";

                if (backupSequence <= 0)
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                    return;
                }

                string deviceName, databaseName;

                if (this.restoreDatabase(backupSequence, out deviceName, out databaseName))
                {
                    script = String.Format("window.alert('Database \"{0}\" has been successfully restored from backup device \"{1}\" with backup file at \"{2}\".')", databaseName, deviceName, backupSequence);

                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        protected void CustomerDatabaseSelector_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.bindBackupRecordDetail();
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        private void bindBackupRecordDetail() 
        {
            string customerName = this.CustomerDatabaseSelector.SelectedCustomerName;
            string connectionString = this.CustomerDatabaseSelector.SelectedDBConnectionString;

            string serverName, databaseName, userName, password;

            DatabaseManager.ParseConnectionString(connectionString, out serverName, out databaseName, out userName, out password);

            connectionString = DatabaseManager.BuildConnectionString(serverName, "master", userName, password);

            this.DBBackupDetail.BindData(customerName, databaseName, connectionString, false);
        }

        private bool restoreDatabase(int bakcupSequence, out string backupDeviceName, out string dbName) 
        {
            string customerName = this.CustomerDatabaseSelector.SelectedCustomerName;
            string connectionString = this.CustomerDatabaseSelector.SelectedDBConnectionString;

            string serverName, databaseName, userName, password;

            DatabaseManager.ParseConnectionString(connectionString, out serverName, out databaseName, out userName, out password);

            connectionString = DatabaseManager.BuildConnectionString(serverName, "master", userName, password);

            if (this.databaseManager == null)
            {
                this.databaseManager = new DatabaseManager();
            }

            this.databaseManager.RestoreDatabase(customerName, bakcupSequence.ToString(), databaseName, connectionString);

            backupDeviceName = customerName;
            dbName = databaseName;

            return true;
        }
    }
}