using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DISConfigurationCloud.StorageManagement;

namespace DIS.Management.Storage
{
    [Cmdlet(VerbsCommon.Get, "DatabaseList")]
    public class GetDatabaseListCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The ADO.NET connection string to be used to connect to the database.")]
        public string DBConnectionString { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            DatabaseManager databaseManager = new DatabaseManager();

            string[] databases = databaseManager.ListDatabases(this.DBConnectionString);

            this.WriteObject(databases);
        }
    }
}
