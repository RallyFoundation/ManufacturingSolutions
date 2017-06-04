using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DISConfigurationCloud.UserControls;
//using DISConfigurationCloud.Security;
using DISConfigurationCloud.MetaManagement;
//using DISConfigurationCloud.Utility;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.Facade;

namespace DISConfigurationCloud
{
    public partial class CustomerDetail : AuthPageBase
    {
        private List<Customer> customers;
        private MetaManager metaManager;
        private string customerID;

        private bool isSwitchingViewMode;

        private bool isDBConnectionStringReApplied;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["CustomerID"] != null)
                {
                    this.customerID = Request.QueryString["CustomerID"];
                }

                if (!this.Page.IsPostBack)
                {
                    this.getData(this.customerID);

                    this.addCustomerDataToCache();
                }
                else
                {
                    this.getCustomerDataFromCache();
                }

                if (!this.Page.IsPostBack)
                {
                    this.bindData();
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] {ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        protected void DetailsViewCustomer_DataBound(object sender, EventArgs e)
        {
            try
            {
                this.bindCustomerConfigurationsToGridView();

                if (!this.Page.IsPostBack || this.isSwitchingViewMode || this.isDBConnectionStringReApplied)
                {
                    this.bindBusinessReferences();
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        protected void GridViewCustomerConfigsEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                RadioButtonList radioButtonlist = e.Row.FindControl("RadioButtonListConfigTypes") as RadioButtonList;

                if (radioButtonlist != null)
                {
                    radioButtonlist.SelectedValue = ((int)((e.Row.DataItem as DISConfigurationCloud.MetaManagement.Configuration).ConfigurationType)).ToString();
                }

                DBConnectionEditor dbConnectionEditor = e.Row.FindControl("DBConnectionEditor") as DBConnectionEditor;

                if (dbConnectionEditor != null)
                {
                    dbConnectionEditor.DBConnectionString = (e.Row.DataItem as DISConfigurationCloud.MetaManagement.Configuration).DbConnectionString;
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            } 
        }

        protected void DetailsViewCustomer_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            try
            {
                this.isSwitchingViewMode = true;

                this.DetailsViewCustomer.ChangeMode(e.NewMode);

                if (e.CancelingEdit)
                {
                    this.getData(this.customerID);
                    this.addCustomerDataToCache();
                }

                this.bindData();
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            } 
        }

        protected void DetailsViewCustomer_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                //e.Cancel = true;

                if (this.metaManager == null)
                {
                    this.metaManager = new MetaManager();
                }

                if ((e.NewValues.Count > 0) && (e.NewValues.Contains("ReferenceID")))
                {
                    if (e.NewValues["ReferenceID"] != null)
                    {
                        //this.customers[0].ReferenceID = e.NewValues["ReferenceID"].ToString();
                        this.customers[0].ReferenceID = e.NewValues["ReferenceID"] as string[];
                    }
                    else
                    {
                        this.customers[0].ReferenceID = null;
                    }
                }

                //if (!String.IsNullOrEmpty(this.customers[0].ReferenceID))
                //{
                //    Customer[] custsInDB = this.metaManager.ListCustomers(false);

                //    if (custsInDB.Count((c)=>((c.ReferenceID == this.customers[0].ReferenceID) && (c.ID != this.customers[0].ID))) > 0)
                //    {
                //        e.Cancel = true;

                //        string script = String.Format("window.alert('The Business Reference ID of \"{0}\" has been bound to another configuration set, please double check!');", this.customers[0].ReferenceID);

                //        this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                //    }
                //}

                if (this.customers[0].ReferenceID != null)
                {
                    Customer[] custsInDB = this.metaManager.ListCustomers(false);

                    if (custsInDB != null)
                    {
                        if (custsInDB.Count((c)=>(c.ID != this.customers[0].ID)) > 0)
                        {
                            custsInDB = custsInDB.Where((c) => (c.ID != this.customers[0].ID)).ToArray();

                            foreach (string refID in this.customers[0].ReferenceID)
                            {
                                if (custsInDB.Count((c) => ((c.ReferenceID != null) && (c.ReferenceID.Contains(refID)))) > 0)
                                {
                                    e.Cancel = true;

                                    string script = String.Format("window.alert('The Business Reference ID of \"{0}\" has been bound to another configuration set, please double check!');", refID);

                                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);

                                    return;
                                }
                            }
                        }
                    }
                }

                int result = this.metaManager.UpdateCustomerConfiguration(this.customers[0]);

                this.DetailsViewCustomer.ChangeMode(DetailsViewMode.ReadOnly);

                this.isSwitchingViewMode = true;

                this.getData(this.customerID);
                this.addCustomerDataToCache();
                this.bindData();
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                //throw;

                //string script = String.Format("window.alert('The following error(s) occurred: {0}');", ex.Message);

                string exMsg = ex.Message;

                exMsg = exMsg.Replace(System.Environment.NewLine, "");
                exMsg = exMsg.Replace("\r\n", "");

                string script = String.Format("window.alert('The following error(s) occurred: {0}');", exMsg);

                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        protected void RadioButtonListConfigTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    Control parent = (sender as Control).Parent;

                    if (parent != null)
                    {
                        parent = parent.Parent;

                        if ((parent != null) && (parent is GridViewRow))
                        {
                            string configID = (parent as GridViewRow).Cells[0].Text;
                            this.customers[0].Configurations.First((o) => (o.ID.ToLower() == configID.ToLower())).ConfigurationType = (ConfigurationType)(int.Parse((sender as RadioButtonList).SelectedValue));
                            this.addCustomerDataToCache();
                            this.bindData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        protected void DBConnectionEditor_Apply(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    Control parent = (sender as Control).Parent;

                    if (parent != null)
                    {
                        parent = parent.Parent;

                        if ((parent != null) && (parent is GridViewRow))
                        {
                            string dbConnectionString = (sender as DBConnectionEditor).DBConnectionString;

                            string dbName = "";
                            string serverName = "";
                            Dictionary<string, string> suspectConfs = null;
                            string suspectConfListTemplate = "Business Name: \"{0}\", Server Name: \"{1}\"; ";
                            string message = "", script = "";

                            if (this.isDatabaseInUse(dbConnectionString, out dbName, out serverName, out suspectConfs))
                            {
                                script = String.Format("window.alert('The selected database \"{0}\" on server \"{1}\" has been bound to another configuration! Please double check!')", dbName, serverName);
                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);

                                return;
                            }
                            else if ((suspectConfs != null) && (suspectConfs.Count > 0))
                            {
                                message = string.Format("The database \"{0}\" is also found in the bindings of the following configurations, please double check to make sure that they are not the same database: ", dbName);

                                foreach (string bizName in suspectConfs.Keys)
                                {
                                    message += string.Format(suspectConfListTemplate, bizName, suspectConfs[bizName]);
                                }

                                script = String.Format("window.alert('{0}')", message);

                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);

                                return;
                            }

                            this.isDBConnectionStringReApplied = true;

                            string configID = (parent as GridViewRow).Cells[0].Text;
                            this.getCustomerDataFromCache();
                            this.customers[0].Configurations.First((o) => (o.ID.ToLower() == configID.ToLower())).DbConnectionString = dbConnectionString;
                            this.addCustomerDataToCache();
                            this.bindData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);
                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        private void getData(string customerID)
        {
            if (this.metaManager == null)
            {
                this.metaManager = new MetaManager();
            }

            Customer customer = this.metaManager.GetCustomer(customerID);

            this.customers = new List<Customer>(new Customer[] { customer });
        }

        private void addCustomerDataToCache()
        {
            this.Page.Cache["CurrentCustomer"] = this.customers;
        }

        private void getCustomerDataFromCache()
        {
            this.customers = Page.Cache["CurrentCustomer"] as List<Customer>;
        }

        private void bindData()
        {
            this.DetailsViewCustomer.DataSource = this.customers;
            this.DetailsViewCustomer.DataBind();
        }

        private void bindCustomerConfigurationsToGridView()
        {
            if (this.customers != null)
            {
                Control control = this.DetailsViewCustomer.Rows[3].FindControl("GridViewCustomerConfigs");

                if (control != null)
                {
                    GridView gridView = control as GridView;

                    gridView.DataSource = this.customers[0].Configurations;
                    gridView.DataBind();
                }
            }
        }

        private void bindBusinessReferences() 
        {
            if (this.customers != null)
            {
                Control control = this.DetailsViewCustomer.Rows[2].FindControl("bizRefEditor");

                if (control != null)
                {
                    BusinessReferenceEditor bizRefEditor = control as BusinessReferenceEditor;

                    bizRefEditor.BusinessReferences = this.customers[0].ReferenceID;
                    bizRefEditor.BindData();
                }
            }
        }

        private bool isDatabaseInUse(string dbConnectionString, out string databaseName, out string serverName, out Dictionary<string, string> suspectConfigurations)
        {
            if (this.metaManager == null)
            {
                this.metaManager = new MetaManager();
            }

            Customer[] customers = this.metaManager.ListCustomers(true);

            databaseName = "";
            serverName = "";
            suspectConfigurations = null;
            string userName = "", password = "", existingDBName = "", existingServerName = "";

            if ((customers != null) && (customers.Length > 0))
            {
                foreach (var cust in customers)
                {
                    foreach (var custConf in cust.Configurations)
                    {
                        if (custConf.DbConnectionString.ToLower() == dbConnectionString.ToLower())
                        {
                            DISConfigurationCloud.StorageManagement.DatabaseManager.ParseConnectionString(dbConnectionString, out serverName, out databaseName, out userName, out password);

                            return true;
                        }
                        else
                        {
                            DISConfigurationCloud.StorageManagement.DatabaseManager.ParseConnectionString(dbConnectionString, out serverName, out databaseName, out userName, out password);

                            DISConfigurationCloud.StorageManagement.DatabaseManager.ParseConnectionString(custConf.DbConnectionString, out existingServerName, out existingDBName, out userName, out password);

                            if ((databaseName.ToLower() == existingDBName.ToLower()) && (serverName.ToLower() == existingServerName.ToLower()))
                            {
                                return true;
                            }
                            else if (databaseName.ToLower() == existingDBName.ToLower())
                            {
                                if (suspectConfigurations == null)
                                {
                                    suspectConfigurations = new Dictionary<string, string>();
                                }

                                if (!suspectConfigurations.ContainsKey(cust.Name))
                                {
                                    suspectConfigurations.Add(cust.Name, existingServerName);
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}