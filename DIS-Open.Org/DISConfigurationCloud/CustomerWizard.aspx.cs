using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DISConfigurationCloud.MetaManagement;
//using DISConfigurationCloud.Security;
//using DISConfigurationCloud.Utility;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.Facade;

namespace DISConfigurationCloud
{
    public partial class CustomerWizard : AuthPageBase
    {
        private IMetaManager metaManager;

        private Customer customer;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.metaManager = new MetaManager();

                if (!this.Page.IsPostBack)
                {
                    if (String.IsNullOrEmpty(Request.QueryString["CustomerID"]))
                    {
                        this.InitControls();
                    }
                    else if ((!String.IsNullOrEmpty(Request.QueryString["CustomerID"])) && (!String.IsNullOrEmpty(Request.QueryString["ConfigID"])) && (!String.IsNullOrEmpty(Request.QueryString["ConfigIndex"])))
                    {
                        this.restoreControlStateFromSession(Request.QueryString["CustomerID"], Request.QueryString["ConfigID"], Request.QueryString["ConfigIndex"]);
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

        protected void WizardCustomer_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            try
            {
                string customerID = this.TextBoxBusinessID.Text; //this.HFCustomerID.Value;

                string sessionKey = String.Format("NEW-CUSTOMER-{0}", customerID);

                switch (e.CurrentStepIndex)
                {
                    case 0:
                        {
                            string customerName = this.TextBoxCustomerName.Text;

                            //if (this.isCustomerNameInExistence(customerName))
                            //{
                            //    e.Cancel = true;

                            //    string script = String.Format("window.alert('Another customer with the same name of \"{0}\" already exists! Please choose a different customer name.');", customerName);

                            //    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                            //}

                            int existnecyCheckingResult = this.isCustomerInExistence(customerID, customerName);

                            if (existnecyCheckingResult > 0)
                            {
                                e.Cancel = true;

                                string script = String.Format("window.alert('Another business with the same name of \"{0}\" already exists! Please choose a different customer name.');", customerName);

                                if (existnecyCheckingResult == 1)
                                {
                                    script = String.Format("window.alert('Another business with the same ID of \"{0}\" already exists! Please choose a different business ID.');", customerID);
                                }

                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                            }
                            else
                            {
                                this.customer = new Customer()
                                {
                                    ID = customerID,
                                    Name = customerName
                                };

                                Session[sessionKey] = this.customer;
                            }

                            break;
                        }
                    case 1:
                        {
                            string dbName = "";
                            string serverName = "";
                            Dictionary<string, string> suspectConfs = null;

                            if (!this.DBConnectionEditorULS.ValidateDISDatabase(out dbName))
                            {
                                e.Cancel = true;

                                string script = String.Format("window.alert('The selected database \"{0}\" is NOT a valid DIS database! Please check the database.')", dbName);
                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                            }

                            if (this.isDatabaseInUse(this.DBConnectionEditorULS.DBConnectionString, out dbName, out serverName, out suspectConfs))
                            {
                                e.Cancel = true;

                                string script = String.Format("window.alert('The selected database \"{0}\" on server \"{1}\" has been bound to another configuration! Please double check!')", dbName, serverName);
                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                            }
                            else if((suspectConfs != null) && (suspectConfs.Count > 0))
                            {
                                e.Cancel = true;

                                string suspectConfListTemplate = "Business Name: \"{0}\", Server Name: \"{1}\"; ";
                                
                                string message = string.Format("The database \"{0}\" is also found in the bindings of the following configurations, please double check to make sure that they are not the same database: ", dbName);

                                foreach (string bizName in suspectConfs.Keys)
                                {
                                    message += string.Format(suspectConfListTemplate, bizName, suspectConfs[bizName]);
                                }

                                string script = String.Format("window.alert('{0}')", message);

                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                            }

                            this.customer = Session[sessionKey] as Customer;

                            if (this.customer.Configurations == null)
                            {
                                this.customer.Configurations = new Configuration[]
                                {
                                    new Configuration()
                                    {
                                         ID =  this.DBConnectionEditorULS.ConfigurationID, //Guid.NewGuid().ToString(),
                                         ConfigurationType = ((ConfigurationType)(int.Parse(this.DropDownListConfigurationTypeULS.SelectedValue))),
                                         DbConnectionString = this.DBConnectionEditorULS.DBConnectionString
                                    }
                                };
                            }
                            else if(this.customer.Configurations.Length > 0) //Fix for bug#58 - Rally, Nov 27, 2014
                            {
                                if (this.customer.Configurations[0] == null)
                                {
                                    this.customer.Configurations[0] = new Configuration();
                                }

                                this.customer.Configurations[0].ID = this.DBConnectionEditorULS.ConfigurationID;
                                this.customer.Configurations[0].ConfigurationType = ((ConfigurationType)(int.Parse(this.DropDownListConfigurationTypeULS.SelectedValue)));
                                this.customer.Configurations[0].DbConnectionString = this.DBConnectionEditorULS.DBConnectionString;
                            }
                           
                            Session[sessionKey] = this.customer;

                            break;
                        }
                    case 2:
                        {
                            string dbName = "";
                            string serverName = "";
                            Dictionary<string, string> suspectConfs = null;

                            if (!this.DBConnectionEditorDLS.ValidateDISDatabase(out dbName))
                            {
                                e.Cancel = true;

                                string script = String.Format("window.alert('The selected database \"{0}\" is NOT a valid DIS database! Please check the database.')", dbName);
                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                            }

                            if (this.isDatabaseInUse(this.DBConnectionEditorDLS.DBConnectionString, out dbName, out serverName, out suspectConfs))
                            {
                                e.Cancel = true;

                                string script = String.Format("window.alert('The selected database \"{0}\" on server \"{1}\" has been bound to another configuration! Please double check!')", dbName, serverName);
                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                            }
                            else if ((suspectConfs != null) && (suspectConfs.Count > 0))
                            {
                                e.Cancel = true;

                                string suspectConfListTemplate = "Business Name: \"{0}\", Server Name: \"{1}\"; ";

                                string message = string.Format("The database \"{0}\" is also found in the bindings of the following configurations, please double check to make sure that they are not the same database: ", dbName);

                                foreach (string bizName in suspectConfs.Keys)
                                {
                                    message += string.Format(suspectConfListTemplate, bizName, suspectConfs[bizName]);
                                }

                                string script = String.Format("window.alert('{0}')", message);

                                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                            }

                            this.customer = Session[sessionKey] as Customer;

                            List<Configuration> configurations = new List<Configuration>(this.customer.Configurations);

                            if (configurations.Count < 2)
                            {
                                configurations.Add(new Configuration
                                {
                                    ID = this.DBConnectionEditorDLS.ConfigurationID, //Guid.NewGuid().ToString(),
                                    ConfigurationType = ((ConfigurationType)(int.Parse(this.DropDownListConfigurationTypeDLS.SelectedValue))),
                                    DbConnectionString = this.DBConnectionEditorDLS.DBConnectionString
                                });
                            }
                            else
                            {
                                if (configurations[1] == null)
                                {
                                    configurations[1] = new Configuration();
                                }

                                configurations[1].ID = this.DBConnectionEditorDLS.ConfigurationID;
                                configurations[1].ConfigurationType = ((ConfigurationType)(int.Parse(this.DropDownListConfigurationTypeDLS.SelectedValue)));
                                configurations[1].DbConnectionString = this.DBConnectionEditorDLS.DBConnectionString;
                            }

                            this.customer.Configurations = configurations.ToArray();

                            Session[sessionKey] = this.customer;

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;

                //string script = String.Format("window.alert('The following error(s) occurred: {0}');", ex.Message);

                //this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }


        protected void WizardCustomer_ActiveStepChanged(object sender, EventArgs e)
        {
            try
            {
                string customerID = this.TextBoxBusinessID.Text; //this.HFCustomerID.Value;

                switch (this.WizardCustomer.ActiveStepIndex)
                {
                    //case 1: 
                    //    {
                    //        this.DBConnectionEditorULS.ConfigurationID = customerID;

                    //        break;
                    //    }
                    //case 2: 
                    //    {
                    //        this.DBConnectionEditorDLS.ConfigurationID = customerID;

                    //        break;
                    //    }
                    case 3:
                        {
                            string sessionKey = String.Format("NEW-CUSTOMER-{0}", customerID);

                            this.customer = Session[sessionKey] as Customer;

                            this.bindSummaryControls(this.customer);

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;

                //string script = String.Format("window.alert('The following error(s) occurred: {0}');", ex.Message);

                //this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        protected void WizardCustomer_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            try
            {
                //this.setCustomer(true);

                string customerID = this.TextBoxBusinessID.Text; //this.HFCustomerID.Value;

                string sessionKey = String.Format("NEW-CUSTOMER-{0}", customerID);

                this.customer = Session[sessionKey] as Customer;

                Customer cust = new Customer() 
                {
                     ID = this.customer.ID,
                     Name = this.customer.Name,
                     Configurations = (new List<Configuration>(this.customer.Configurations)).Where((c)=>(c != null)).ToArray()
                };

                if ((cust.Configurations!= null) && (cust.Configurations.Length > 0))
                {
                    customerID = this.metaManager.AddCustomerConfiguration(cust);

                    if (!string.IsNullOrEmpty(customerID))
                    {
                        string redirectUrl = String.Format("~/CustomerDetail.aspx?CustomerID={0}", customerID);

                        Response.Redirect(redirectUrl);
                    }
                }
                else
                {
                    string script = "window.alert('You have not created any configuration for this configuation set, please create 1 at least.');";

                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                }
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                //throw;

                string exMsg = ex.Message;

                exMsg = exMsg.Replace(System.Environment.NewLine, "");
                exMsg = exMsg.Replace("\r\n", "");

                string script = String.Format("window.alert('The following error(s) occurred: {0}');", exMsg);

                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        private bool isCustomerNameInExistence(string customerName) 
        {
            bool returnValue = false;

            Customer[] customers = this.metaManager.ListCustomers(false);

            if ((customers != null) && (customers.Length > 0))
            {
                foreach (var customer in customers)
                {
                    if (customer.Name.ToLower() == customerName.ToLower())
                    {
                        returnValue = true;

                        break;
                    }
                }
            }

            return returnValue;
        }

        private int isCustomerInExistence(string customerID, string customerName)
        {
            Customer[] customers = this.metaManager.ListCustomers(false);

            if ((customers != null) && (customers.Length > 0))
            {
                foreach (var customer in customers)
                {
                    if (customer.ID.ToLower() == customerID.ToLower())
                    {
                        return 1;
                    }

                    if (customer.Name.ToLower() == customerName.ToLower())
                    {
                        return 2;
                    }
                }
            }

            return 0;
        }

        //private bool isDatabaseInUse(Customer customer, out string databaseName, out string serverName) 
        //{
        //    Customer[] customers = this.metaManager.ListCustomers(true);

        //    databaseName = "";
        //    serverName = "";
        //    string userName = "", password = "";

        //    if ((customers != null) && (customers.Length > 0))
        //    {
        //        foreach (var cust in customers)
        //        {
        //            foreach (var custConf in cust.Configurations)
        //            {
        //                foreach (var conf in customer.Configurations)
        //                {
        //                    if (conf.DbConnectionString.ToLower() == custConf.DbConnectionString.ToLower())
        //                    {
        //                        DISConfigurationCloud.StorageManagement.DatabaseManager.ParseConnectionString(conf.DbConnectionString, out serverName, out databaseName, out userName, out password);

        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        private bool isDatabaseInUse(string dbConnectionString, out string databaseName, out string serverName, out Dictionary<string, string> suspectConfigurations)
        {
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

        private void setCustomer(bool generateGUID) 
        {
            this.customer = new Customer()
            {
                ID = generateGUID ? Guid.NewGuid().ToString() : null,
                Name = this.TextBoxCustomerName.Text,

                Configurations = new Configuration[]
                 {
                     new Configuration ()
                     {
                          ID = generateGUID ? Guid.NewGuid().ToString() : null,
                          ConfigurationType = ((ConfigurationType)(int.Parse(this.DropDownListConfigurationTypeULS.SelectedValue))),
                          DbConnectionString = this.DBConnectionEditorULS.DBConnectionString
                     },
                      new Configuration ()
                     {
                          ID = generateGUID ? Guid.NewGuid().ToString() : null,
                          ConfigurationType = ((ConfigurationType)(int.Parse(this.DropDownListConfigurationTypeDLS.SelectedValue))),
                          DbConnectionString = this.DBConnectionEditorDLS.DBConnectionString
                     }
                 }
            };
        }

        private void bindSummaryControls(Customer customer) 
        {
            this.LabelCustomerName.Text = customer.Name;

            if (customer.Configurations[0] != null)
            {
                this.LabelConfigurationTypeULS.Text = customer.Configurations[0].ConfigurationType.ToString();
                this.LabelDBConnectionStringULS.Text = customer.Configurations[0].DbConnectionString;
            }
            else
            {
                this.LabelConfigurationTypeULS.Text = "";
                this.LabelDBConnectionStringULS.Text = "";
            }

            if (customer.Configurations[1] != null)
            {
                this.LabelConfigurationTypeDLS.Text = customer.Configurations[1].ConfigurationType.ToString();
                this.LabelDBConnectionStringDLS.Text = customer.Configurations[1].DbConnectionString;
            }
            else
            {
                this.LabelConfigurationTypeDLS.Text = "";
                this.LabelDBConnectionStringDLS.Text = "";
            }
        }

        private void InitControls() 
        {
            //this.HFCustomerID.Value = Guid.NewGuid().ToString();

            this.TextBoxBusinessID.Text = Guid.NewGuid().ToString();

            this.DBConnectionEditorDLS.CustomerID = this.TextBoxBusinessID.Text; //this.HFCustomerID.Value;
            this.DBConnectionEditorDLS.ConfigurationID = Guid.NewGuid().ToString();
            this.DBConnectionEditorDLS.ConfigurationIndex = "1";

            this.DBConnectionEditorULS.CustomerID = this.TextBoxBusinessID.Text; //this.HFCustomerID.Value;
            this.DBConnectionEditorULS.ConfigurationID = Guid.NewGuid().ToString();
            this.DBConnectionEditorULS.ConfigurationIndex = "0";
        }

        private void restoreControlStateFromSession(string customerID, string configurationID, string configurationIndex) 
        {
            string sessionKey = String.Format("NEW-CUSTOMER-{0}", customerID);

            Customer customer = Session[sessionKey] as Customer;

            if (customer != null)
            {
                //this.HFCustomerID.Value = customer.ID;
                this.TextBoxBusinessID.Text = customer.ID;
                this.TextBoxCustomerName.Text = customer.Name;

                if ((customer.Configurations != null) && (customer.Configurations.Length >= 1))
                {
                    if ((customer.Configurations.Length == 1) && (customer.Configurations[0] != null))
                    {
                        this.DBConnectionEditorULS.ConfigurationID = customer.Configurations[0].ID;
                        this.DBConnectionEditorULS.DBConnectionString = customer.Configurations[0].DbConnectionString;
                        this.DropDownListConfigurationTypeULS.SelectedValue = ((int)(customer.Configurations[0].ConfigurationType)).ToString();
                    }
                    else if (customer.Configurations.Length == 2)
                    {
                        if (customer.Configurations[0] != null)
                        {
                            this.DBConnectionEditorULS.ConfigurationID = customer.Configurations[0].ID;
                            this.DBConnectionEditorULS.DBConnectionString = customer.Configurations[0].DbConnectionString;
                            this.DropDownListConfigurationTypeULS.SelectedValue = ((int)(customer.Configurations[0].ConfigurationType)).ToString();
                        }

                        if (customer.Configurations[1] != null)
                        {
                            this.DBConnectionEditorDLS.ConfigurationID = customer.Configurations[1].ID;
                            this.DBConnectionEditorDLS.DBConnectionString = customer.Configurations[1].DbConnectionString;
                            this.DropDownListConfigurationTypeDLS.SelectedValue = ((int)(customer.Configurations[1].ConfigurationType)).ToString();
                        }
                    }
                }
            }

            sessionKey = String.Format("NEW-CONFIG-{0}", configurationID);

            string[] configValues = Session[sessionKey] as string[];

            if (configValues != null)
            {
                string connectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", configValues[0], configValues[3], configValues[1], configValues[2]);

                int configIndex = -1;

                if (int.TryParse(configurationIndex, out configIndex))
                {
                    switch (configIndex)
                    {
                        case 0:
                            {
                                this.DBConnectionEditorULS.ConfigurationID = configurationID;
                                this.DBConnectionEditorULS.DBConnectionString = connectionString;
                                break;
                            }
                        case 1: 
                            {
                                this.DBConnectionEditorDLS.ConfigurationID = configurationID;
                                this.DBConnectionEditorDLS.DBConnectionString = connectionString;
                                break;
                            }
                    }

                    this.WizardCustomer.ActiveStepIndex = (configIndex + 1);
                }
            }

            this.DBConnectionEditorULS.CustomerID = customerID;
            this.DBConnectionEditorULS.ConfigurationIndex = "0";
            this.DBConnectionEditorDLS.CustomerID = customerID;
            this.DBConnectionEditorDLS.ConfigurationIndex = "1";

            if (String.IsNullOrEmpty(this.DBConnectionEditorDLS.ConfigurationID))
            {
                this.DBConnectionEditorDLS.ConfigurationID = Guid.NewGuid().ToString();
            }

            if (String.IsNullOrEmpty(this.DBConnectionEditorULS.ConfigurationID))
            {
                this.DBConnectionEditorULS.ConfigurationID = Guid.NewGuid().ToString();
            }
        }

        private void setSkippedCustomerConfiguration(int index) 
        {
            string customerID = this.TextBoxBusinessID.Text;

            string sessionKey = String.Format("NEW-CUSTOMER-{0}", customerID);

            this.customer = Session[sessionKey] as Customer;

            if (this.customer.Configurations == null)
            {
                this.customer.Configurations = new Configuration[] { null };
            }
            else if (this.customer.Configurations.Length >= (index + 1))
            {
                this.customer.Configurations[index] = null;
            }
            else if (this.customer.Configurations.Length < (index + 1))
            {
                List<Configuration> configurations = new List<Configuration>(this.customer.Configurations);

                configurations.Add(null);

                this.customer.Configurations = configurations.ToArray();
            }

            Session[sessionKey] = this.customer;
        }

        protected void linkButtonSkipStepULS_Click(object sender, EventArgs e)
        {
            this.setSkippedCustomerConfiguration(0);
            this.WizardCustomer.MoveTo(this.WizardStepDLS);
        }

        protected void linkButtonSkipStepDLS_Click(object sender, EventArgs e)
        {
            this.setSkippedCustomerConfiguration(1);
            this.WizardCustomer.MoveTo(this.WizardStepSummary);
        }
    }
}