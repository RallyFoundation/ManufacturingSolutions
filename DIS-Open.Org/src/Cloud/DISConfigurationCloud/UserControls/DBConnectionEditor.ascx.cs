using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using DISConfigurationCloud.StorageManagement;
using DISConfigurationCloud.Utility;

namespace DISConfigurationCloud.UserControls
{
    public partial class DBConnectionEditor : System.Web.UI.UserControl
    {
        private IDatabaseManager databaseManager;

        [Editor]
        public event DBConnectionEditorOnApplyEventHandler Apply;

        [Editor]
        public bool IsShowingApplyButton 
        {
            get { return this.ButtonApply.Visible; }
            set { this.ButtonApply.Visible = value; }
        }
        
        [Editor]
        public bool IsAllowingCreatingNewDB 
        {
            get { return this.LinkButtonCreateDB.Visible; }
            set { this.LinkButtonCreateDB.Visible = value; }
        }

        public string CustomerID 
        {
            get { return this.HFCustomerID.Value; }
            set { this.HFCustomerID.Value = value; }
        }
        public string ConfigurationID 
        {
            get { return this.HFConfigurationID.Value; }
            set { this.HFConfigurationID.Value = value; }
        }

        public string ConfigurationIndex 
        {
            get { return this.HFConfigurationIndex.Value; }
            set { this.HFConfigurationIndex.Value = value; }
        }

        [Bindable(true)]
        public string DBConnectionString 
        {
            get 
            { 
                return this.databaseManager.GetConnectionString(this.TextBoxInstance.Text, this.DropDownListDBNames.SelectedValue, this.TextBoxLoginName.Text, this.TextBoxPassword.Text); 
            }
            set 
            {
                try
                {
                    string[] fields = value.Split(new string[] { ";" }, StringSplitOptions.None);

                    if ((fields != null) && (fields.Length == 4))
                    {
                        string[] pair = null;

                        string dbName = "";

                        for (int i = 0; i < fields.Length; i++)
                        {
                            pair = fields[i].Split(new string[] { "=" }, StringSplitOptions.None);

                            switch (i)
                            {
                                case 0:
                                    {
                                        this.TextBoxInstance.Text = pair[1];
                                        break;
                                    }
                                case 1:
                                    {
                                        dbName = pair[1];
                                        break;
                                    }
                                case 2:
                                    {
                                        this.TextBoxLoginName.Text = pair[1];
                                        break;
                                    }
                                case 3:
                                    {
                                        this.TextBoxPassword.Text = pair[1];
                                        break;
                                    }
                            }
                        }

                        this.bindDBNamesDropdwonList();

                        if ((!String.IsNullOrEmpty(dbName)) && (this.DropDownListDBNames.DataSource != null))
                        {
                            this.DropDownListDBNames.SelectedValue = dbName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                    //throw;
                }
            }
        }

        public bool ValidateDISDatabase(out string databaseName) 
        {
            databaseName = this.DropDownListDBNames.SelectedValue;

            return this.validateDatabaseSchema();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.databaseManager = new DatabaseManager();
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }

        protected void BottonTestDBConnection_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.TextBoxInstance.Text.StartsWith(".") || this.TextBoxInstance.Text.ToLower().StartsWith("localhost") || this.TextBoxInstance.Text.ToLower().StartsWith("(local)") || this.TextBoxInstance.Text.StartsWith("127."))
                {
                    string script = "window.alert('Server/Instance Name should not be starting with loopback address!')";
                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                    return;
                }

                this.bindDBNamesDropdwonList();
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                string script = String.Format("window.alert('The following error(s) occurred connecting to the DB server: {0}');", ex.Message);

                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        protected void LinkButtonCreateDB_Click(object sender, EventArgs e)
        {
            try
            {
                //this.bindDBNamesDropdwonList();

                //string redirectUrl = String.Format("~/NewDatabase.aspx?DBInst={0}&DBLogin={1}&DBPwd={2}", this.TextBoxInstance.Text, this.TextBoxLoginName.Text, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.TextBoxPassword.Text)));

                if (this.TextBoxInstance.Text.StartsWith(".") || this.TextBoxInstance.Text.ToLower().StartsWith("localhost") || this.TextBoxInstance.Text.ToLower().StartsWith("(local)") || this.TextBoxInstance.Text.StartsWith("127."))
                {
                    string script = "window.alert('Server/Instance Name should not be starting with loopback address!')";
                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                    return;
                }

                string redirectUrl = "~/NewDatabase.aspx";

                if (!String.IsNullOrEmpty(this.ConfigurationID))
                {
                    string sessionKey = String.Format("NEW-CONFIG-{0}", this.ConfigurationID);

                    Session[sessionKey] = new String[] { this.TextBoxInstance.Text, this.TextBoxLoginName.Text, this.TextBoxPassword.Text, "" };

                    redirectUrl = String.Format("~/NewDatabase.aspx?ConfigID={0}", this.ConfigurationID);

                    if (!String.IsNullOrEmpty(this.CustomerID))
                    {
                        redirectUrl += "&CustomerID=" + this.CustomerID;
                    }

                    if (!String.IsNullOrEmpty(this.ConfigurationIndex))
                    {
                        redirectUrl += "&ConfigIndex=" + this.ConfigurationIndex;
                    }
                }

                Response.Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                throw;
            }
        }

        protected void ButtonApply_Click(object sender, EventArgs e)
        {
            if (this.TextBoxInstance.Text.StartsWith(".") || this.TextBoxInstance.Text.ToLower().StartsWith("localhost") || this.TextBoxInstance.Text.ToLower().StartsWith("(local)") || this.TextBoxInstance.Text.StartsWith("127."))
            {
                string script = "window.alert('Server/Instance Name should not be starting with loopback address!')";
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                return;
            }

            if (!this.validateDatabaseSchema())
            {
                string script = String.Format("window.alert('The selected database \"{0}\" is NOT a valid DIS database! Please check the database.')", this.DropDownListDBNames.SelectedValue);
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                return;
            }

            if (this.Apply != null)
            {
                this.Apply(this, e);
            }
        }

        private void bindDBNamesDropdwonList() 
        {
            string serverName = this.TextBoxInstance.Text;
            string databaseName = "master";
            string userName = this.TextBoxLoginName.Text;
            string password = this.TextBoxPassword.Text;

            if (this.databaseManager == null)
            {
                this.databaseManager = new DatabaseManager();
            }

            string connectionString = this.databaseManager.GetConnectionString(serverName, databaseName, userName, password);

            string[] databaseNames = this.databaseManager.ListDatabases(connectionString);

            //List<string> databaseNames = new List<string>();

            //IDictionary<string, IDictionary<string, string[]>> databases = this.databaseManager.ListDatabaseTables(connectionString);
            
            //foreach (string dbName in databases.Keys)
            //{
            //    foreach (string tbName in databases[dbName].Keys)
            //    {
            //        if (tbName.ToLower() == "productkeyinfo")
            //        {
            //            if ((databases[dbName][tbName].FirstOrDefault((o) => (o.ToLower() == "productkey")) != null) && (databases[dbName][tbName].FirstOrDefault((o) => (o.ToLower() == "productkeyid")) != null) && (databases[dbName][tbName].FirstOrDefault((o) => (o.ToLower() == "hardwareid")) != null))
            //            {
            //                databaseNames.Add(dbName);
            //            }

            //            break;
            //        }
            //    }
            //}

            this.DropDownListDBNames.DataSource = databaseNames;
            this.DropDownListDBNames.DataBind();
        }

        private bool validateDatabaseSchema() 
        {
            string serverName = this.TextBoxInstance.Text;
            string databaseName = "master";
            string userName = this.TextBoxLoginName.Text;
            string password = this.TextBoxPassword.Text;
            string dbToValidate = this.DropDownListDBNames.SelectedValue;

            if (this.databaseManager == null)
            {
                this.databaseManager = new DatabaseManager();
            }

            string connectionString = this.databaseManager.GetConnectionString(serverName, databaseName, userName, password);

            IDictionary<string, IDictionary<string, string[]>> databases = this.databaseManager.ListDatabaseTables(connectionString, dbToValidate);

            foreach (string dbName in databases.Keys)
            {
                foreach (string tbName in databases[dbName].Keys)
                {
                    if (tbName.ToLower() == "productkeyinfo")
                    {
                        if ((databases[dbName][tbName].FirstOrDefault((o) => (o.ToLower() == "productkey")) != null) && (databases[dbName][tbName].FirstOrDefault((o) => (o.ToLower() == "productkeyid")) != null) && (databases[dbName][tbName].FirstOrDefault((o) => (o.ToLower() == "hardwareid")) != null))
                        {
                            return true;
                        }

                        break;
                    }
                }
            }

            return false;
        }
    }

    public delegate void DBConnectionEditorOnApplyEventHandler(object sender, EventArgs e);
} 