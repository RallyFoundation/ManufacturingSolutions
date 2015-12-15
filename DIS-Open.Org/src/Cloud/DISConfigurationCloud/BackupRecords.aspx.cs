using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DISConfigurationCloud.Security;
using DISConfigurationCloud.MetaManagement;
using DISConfigurationCloud.StorageManagement;
using DISConfigurationCloud.Utility;

namespace DISConfigurationCloud
{
    public partial class BackupRecords : AuthPageBase
    {
        IMetaManager metaManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string customerID = Request.QueryString["CustomerID"];
                string configurationID = Request.QueryString["ConfigID"];

                if ((!String.IsNullOrEmpty(customerID)) && (!String.IsNullOrEmpty(configurationID)))
                {
                    if (this.metaManager == null)
                    {
                        this.metaManager = new MetaManager();
                    }

                    Customer customer = this.metaManager.GetCustomer(customerID);

                    if (customer != null)
                    {
                        Configuration configuration = customer.Configurations.First((o) => (o.ID.ToLower() == configurationID.ToLower()));

                        if (configuration != null)
                        {
                            string serverName, databaseName, userName, password;

                            DatabaseManager.ParseConnectionString(configuration.DbConnectionString, out serverName, out databaseName, out userName, out password);

                            string connectionString = DatabaseManager.BuildConnectionString(serverName, "master", userName, password);

                            this.DBBackupDetail.BindData(customer.Name, databaseName, connectionString, this.Page.IsPostBack);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }
    }
}