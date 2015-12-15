using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DISConfigurationCloud.StorageManagement;

namespace DIS.Management.Storage
{
    [Cmdlet(VerbsCommon.New, "Database")]
    public class CreateDatabaseCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The name of the server / instance hosting the database.")]
        public string DBServerName { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The user name for logging on to the server / instance hosting the database.")]
        public string DBUserName { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The passwrod of the user name for logging on to the server / instance hosting the database.")]
        public string DBPassword { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The the name of the database.")]
        public string DBName { get; set; }

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "The the full path of the sqlcmd script file for creating the database.")]
        public string DBCreationScriptPath { get; set; }

        protected override void ProcessRecord()
        {
            DatabaseManager databaseManager = new DatabaseManager();

            if (!String.IsNullOrEmpty(this.DBCreationScriptPath))
            {
                ModuleConfiguration.SQLScriptFile_CreateDB = this.DBCreationScriptPath;

                string output = databaseManager.CreateDatabase(this.DBServerName, this.DBName, this.DBUserName, this.DBPassword);

                this.WriteObject(output);
            }
            else 
            {
                int result = databaseManager.CreateDatabase(this.DBName, DatabaseManager.BuildConnectionString(this.DBServerName, "master", this.DBUserName, this.DBPassword));

                this.WriteObject(result);
            }
        }
    }
}
