using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DISConfigurationCloud.StorageManagement;

namespace DIS.Management.Storage
{
    [Cmdlet(VerbsCommon.Remove, "Database")]
    public class DeleteDatabaseCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The ADO.NET connection string to be used to connect to the database.")]
        public string DBConnectionString { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The the name of the database to remove.")]
        public string DBName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            DatabaseManager manager = new DatabaseManager();

            int result = manager.DropDatabase(this.DBConnectionString, this.DBName);

            this.WriteObject(result);
        }
    }
}
