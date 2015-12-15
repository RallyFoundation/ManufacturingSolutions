using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DISConfigurationCloud.StorageManagement;

namespace DIS.Management.Storage
{
    [Cmdlet("Test", "DBConnection")]
    public class TestDBConnectionCmdlet :Cmdlet
    {
        [Parameter(Position = 0, Mandatory=true, HelpMessage="The ADO.NET connection string to be used to connect to the database.")]
        public string DBConnectionString { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            DatabaseManager databaseManager = new DatabaseManager();

            bool result = false;

            try
            {
                result = databaseManager.TestConnection(this.DBConnectionString);
            }
            catch (Exception ex)
            {
                result = false;
                this.WriteVerbose(ex.ToString());
            }
            
            this.WriteObject(result);
        }
    }
}
