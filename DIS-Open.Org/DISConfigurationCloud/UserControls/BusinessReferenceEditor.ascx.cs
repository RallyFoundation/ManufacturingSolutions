using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using DISConfigurationCloud.MetaManagement;

namespace DISConfigurationCloud.UserControls
{
    public partial class BusinessReferenceEditor : System.Web.UI.UserControl
    {
        public string[] BusinessReferences { get; set; }

        [Editor]
        public bool IsReadOnly { get; set; }

        public void BindData() 
        {
            this.bindData();
        }

        private List<Customer> customers = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack)
            {
                this.getCustomerDataFromCache();
                this.BusinessReferences = this.customers[0].ReferenceID;
            }
            else
            {
                this.bindData();
            }

            this.setControls();
        }

        protected void gridViewBizRefs_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.gridViewBizRefs.EditIndex = e.NewEditIndex;
            this.bindData();
        }

        protected void gridViewBizRefs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.gridViewBizRefs.EditIndex = -1;
            this.bindData();
        }

        protected void gridViewBizRefs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<string> refs = new List<string>(this.BusinessReferences);

            refs.RemoveAt(e.RowIndex);

            this.BusinessReferences = refs.ToArray();

            this.getCustomerDataFromCache();

            this.customers[0].ReferenceID = this.BusinessReferences;

            this.bindData();

            this.addCustomerDataToCache();
        }

        protected void gridViewBizRefs_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            List<string> refs = new List<string>(this.BusinessReferences);

            string newValue = (this.gridViewBizRefs.Rows[e.RowIndex].FindControl("txtRefID") as TextBox).Text;

            int index = refs.IndexOf(newValue);

            if ((index > 0) && (index != e.RowIndex))
            {
                e.Cancel = true;

                this.gridViewBizRefs.Rows[index].BackColor = System.Drawing.Color.Red;

                return;
            }

            refs[e.RowIndex] = newValue;

            this.BusinessReferences = refs.ToArray();

            this.getCustomerDataFromCache();

            this.customers[0].ReferenceID = this.BusinessReferences;

            this.addCustomerDataToCache();

            this.gridViewBizRefs.EditIndex = -1;

            this.bindData();
        }

        protected void btnAddBizRef_Click(object sender, EventArgs e)
        {
            List<string> refs = this.BusinessReferences != null ? new List<string>(this.BusinessReferences) : new List<string>();

            string newBizRefID = this.txtNewBizRef.Text;

            if ((refs != null) && (refs.Contains(newBizRefID)))
            {
                this.gridViewBizRefs.Rows[refs.IndexOf(newBizRefID)].BackColor = System.Drawing.Color.Red;
                return;
            }

            refs.Add(newBizRefID);

            this.BusinessReferences = refs.ToArray();

            this.getCustomerDataFromCache();

            this.customers[0].ReferenceID = this.BusinessReferences;

            this.bindData();

            this.addCustomerDataToCache();
        }

        private void bindData() 
        {
            this.gridViewBizRefs.DataSource = this.BusinessReferences;
            this.gridViewBizRefs.DataBind();
        }

        private void setControls() 
        {
            if (this.IsReadOnly)
            {
                this.txtNewBizRef.Visible = false;
                this.btnAddBizRef.Visible = false;
                this.RFVTextBoxBizRefID.Visible = false;
                this.gridViewBizRefs.Columns[1].Visible = false;
            }
        }

        private void addCustomerDataToCache()
        {
            this.Page.Cache["CurrentCustomer"] = this.customers;
        }
        private void getCustomerDataFromCache()
        {
            this.customers = Page.Cache["CurrentCustomer"] as List<Customer>;
        }
    }
}