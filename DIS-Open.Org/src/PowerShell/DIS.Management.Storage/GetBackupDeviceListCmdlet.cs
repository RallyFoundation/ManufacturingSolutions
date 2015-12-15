using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DISConfigurationCloud.StorageManagement;

namespace DIS.Management.Storage
{
    [Cmdlet(VerbsCommon.Get, "BackupDeviceList")]
    public class GetBackupDeviceListCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The name of the server / instance hosting the database.")]
        public string DBServerName { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The user name for logging on to the server / instance hosting the database.")]
        public string DBUserName { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The passwrod of the user name for logging on to the server / instance hosting the database.")]
        public string DBPassword { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The the name of the database.")]
        public string DBName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            DatabaseManager databaseManager = new DatabaseManager();

            string dbConnectionString = DatabaseManager.BuildConnectionString(this.DBServerName, this.DBName, this.DBUserName, this.DBPassword);

            this.WriteObject(databaseManager.ListBackupDevices(dbConnectionString));
        }
    }
}
