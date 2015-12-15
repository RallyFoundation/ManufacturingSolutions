using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using DISConfigurationCloud.MetaManagement;
using DISConfigurationCloud.Utility;

namespace DISConfigurationCloud.UserControls
{
    public partial class CustomerDatabaseSelector : System.Web.UI.UserControl
    {
        private IMetaManager metaManager = null;
        private List<Customer> customers = null;

        [Editor]
        public event ValueChangedEventHandler ValueChanged;

        public string SelectedCustomerID 
        {
            get { return this.DropDownListCustomers.SelectedValue; }
        }

        public string SelectedConfigurationID 
        {
            get { return this.RadioButtonListCustomerDatabases.SelectedValue; }
        }
        public string SelectedCustomerName 
        {
            get { return this.DropDownListCustomers.SelectedItem.Text; }
        }

        public string SelectedDBConnectionString 
        {
            get { return this.RadioButtonListCustomerDatabases.SelectedItem.Text; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    this.getData();
                }
                else
                {
                    this.customers = this.Page.Cache["CurrentCustomer"] as List<Customer>;
                }

                if (!this.Page.IsPostBack)
                {
                    this.bindData();
                    this.DropDownListCustomers.SelectedIndex = 0;
                    this.DropDownListCustomers_SelectedIndexChanged(this.DropDownListCustomers, e);
                }
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }

        protected void DropDownListCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((this.customers != null) && (this.customers.Count > 0))
                {
                    this.RadioButtonListCustomerDatabases.DataSource = this.customers.First((o) => (o.ID.ToLower() == (sender as DropDownList).SelectedValue.ToLower())).Configurations;
                    this.RadioButtonListCustomerDatabases.DataBind();
                }
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }

        protected void RadioButtonListCustomerDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ValueChanged != null)
                {
                    this.ValueChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }

        private void getData()
        {
            if (this.metaManager == null)
            {
                this.metaManager = new MetaManager();
            }

            this.customers = new List<Customer>(this.metaManager.ListCustomers());

            this.Page.Cache["CurrentCustomer"] = this.customers;
        }

        private void bindData()
        {
            this.DropDownListCustomers.DataSource = this.customers;

            this.DropDownListCustomers.DataBind();
        }
    }

    public delegate void ValueChangedEventHandler(object sender, EventArgs e);
}