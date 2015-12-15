using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DISConfigurationCloud.StorageManagement;
using DISConfigurationCloud.Utility;

namespace DISConfigurationCloud.UserControls
{
    public partial class DBCreator : System.Web.UI.UserControl
    {
        private DatabaseManager databaseManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.setControlValues();

                this.databaseManager = new DatabaseManager();
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }

        protected void BottonCreateDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.TextBoxInstance.Text.StartsWith(".") || this.TextBoxInstance.Text.ToLower().StartsWith("localhost") || this.TextBoxInstance.Text.ToLower().StartsWith("(local)") || this.TextBoxInstance.Text.StartsWith("127."))
                {
                    string script = "window.alert('Server/Instance Name should not be starting with loopback address!')";
                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                    return;
                }

                this.createDatabse();
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }

        private void setControlValues() 
        { 
            if (Request.QueryString["ConfigID"] != null)
            {
                string sessionKey = String.Format("NEW-CONFIG-{0}", Request.QueryString["ConfigID"]);

                string[] configValues = Session[sessionKey] as string[];

                if ((configValues != null) && (configValues.Length == 4))
                {
                    this.TextBoxInstance.Text = configValues[0];
                    this.TextBoxLoginName.Text = configValues[1];
                    this.TextBoxPassword.Text = configValues[2];
                }
            }
        }

        private void createDatabse()
        {
            try
            {
                string serverName = this.TextBoxInstance.Text;
                string databaseName = this.TextBoxDatabaseName.Text;
                string userName = this.TextBoxLoginName.Text;
                string password = this.TextBoxPassword.Text;

                string connectionString = this.databaseManager.GetConnectionString(serverName, "master", userName, password);

                string script = String.Format("window.alert('Database \"{0}\" has been successfully created.')", databaseName);
                
                if (this.databaseManager.TestConnection(connectionString))
                {
                    string[] databaseNames = this.databaseManager.ListDatabases(connectionString);

                    if (databaseNames != null)
                    {
                        for (int i = 0; i < databaseNames.Length; i++)
                        {
                            if (databaseNames[i].ToLower() == databaseName.ToLower())
                            {
                                script = String.Format("window.alert('Another database with the same name of \"{0}\" already exists! Please choose a different database name.');", databaseName);
                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                                return;
                            }
                        }
                    }

                    string result = this.databaseManager.CreateDatabase(serverName, databaseName, userName, password);

                    TracingUtility.Trace(new object[] {serverName, databaseName, userName, password, ModuleConfiguration.SQLScriptFile_CreateDB, result }, TracingUtility.DefaultTraceSourceName);

                    if (String.IsNullOrEmpty(result) || (!result.Contains("Update complete.")))
                    {
                        script = String.Format("window.alert('Failed to create database \"{0}\" ! Please contact administrator.')", databaseName);
                    }

                    if ((Request.QueryString["CustomerID"] != null) && (Request.QueryString["ConfigID"] != null) && (Request.QueryString["ConfigIndex"] != null))
                    {
                        string sessionKey = String.Format("NEW-CONFIG-{0}", Request.QueryString["ConfigID"]);

                        string[] configValues = Session[sessionKey] as string[];

                        if ((configValues != null) && (configValues.Length == 4))
                        {
                            configValues[3] = databaseName;

                            Session[sessionKey] = configValues;
                        }

                        string redirectUrl = string.Format("~/CustomerWizard.aspx?CustomerID={0}&ConfigID={1}&ConfigIndex={2}", Request.QueryString["CustomerID"], Request.QueryString["ConfigID"], Request.QueryString["ConfigIndex"]);

                        Response.Redirect(redirectUrl);
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                    }
                }
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                string script = String.Format("window.alert('The following error(s) occurred connecting to the DB server: {0}');", ex.Message);

                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }
    }
}