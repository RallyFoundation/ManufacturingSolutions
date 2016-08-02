using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using DISConfigurationCloud.StorageManagement;
//using DISConfigurationCloud.Utility;
using Platform.DAAS.OData.Facade;

namespace DISConfigurationCloud.UserControls
{
    public partial class DBBackupDetail : System.Web.UI.UserControl
    {
        private IDatabaseManager databaseManager;
        private DataTable backupInfoDataTable;

        [Editor]
        public bool IsAllowingSelection { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.GridViewDBBackupDetail.Columns[0].Visible = this.IsAllowingSelection;
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        public void BindData(string DeviceName, string DatabaseName, string DBConnectionString, bool IsFromCache)
        {
            if (IsFromCache)
            {
                this.backupInfoDataTable = this.Page.Cache["CurrentBackupInfo"] as DataTable;
            }
            else
            {
                this.getData(DeviceName, DatabaseName, DBConnectionString);
            }

            this.bindData();
        }

        public int GetSelectedBakcupSequence()
        {
            int returnValue = -1;

            RadioButton radioButton = null;

            foreach (GridViewRow row in this.GridViewDBBackupDetail.Rows)
            {
                radioButton = row.FindControl("RadioButtonSelect") as RadioButton;

                if ((radioButton != null) && (radioButton.Checked))
                {
                    returnValue = int.Parse(row.Cells[1].Text);
                    break;
                }
            }

            return returnValue;
        }

        private void getData(string deviceName, string databaseName, string dbConnectionString) 
        {
            if (this.databaseManager == null)
            {
                this.databaseManager = new DatabaseManager();
            }

            DataTable table = this.databaseManager.GetBackupInformation(deviceName, dbConnectionString) as DataTable;

            if (table != null)
            {
                string filterExpression = !String.IsNullOrEmpty(databaseName) ? String.Format("DatabaseName = '{0}'", databaseName) : "1 = 1";
                string sortExpression = "Position DESC";

                DataRow[] rows = table.Select(filterExpression, sortExpression);

                if (rows != null)
                {
                    this.backupInfoDataTable = table.Clone();

                    foreach (DataRow row in rows)
                    {
                        this.backupInfoDataTable.ImportRow(row);
                    }
                }
            }
            else
            {
                this.backupInfoDataTable = new DataTable();
            }

            this.Page.Cache["CurrentBackupInfo"] = this.backupInfoDataTable;
        }

        private void bindData() 
        {
            this.GridViewDBBackupDetail.DataSource = this.backupInfoDataTable;
            this.GridViewDBBackupDetail.DataBind();
        }
    }
}