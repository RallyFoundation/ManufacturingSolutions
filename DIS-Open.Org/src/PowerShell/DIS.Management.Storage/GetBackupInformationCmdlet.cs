using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DISConfigurationCloud.StorageManagement;

namespace DIS.Management.Storage
{
    [Cmdlet(VerbsCommon.Get, "BackupInformation")]
    public class GetBackupInformationCmdlet :Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The ADO.NET connection string to be used to connect to the database.")]
        public string DBConnectionString { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The the name of the backup device.")]
        public string BackupDeviceName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            DatabaseManager databaseManager = new DatabaseManager();

            this.WriteObject(databaseManager.GetBackupInformation(this.BackupDeviceName, this.DBConnectionString));
        }
    }
}
