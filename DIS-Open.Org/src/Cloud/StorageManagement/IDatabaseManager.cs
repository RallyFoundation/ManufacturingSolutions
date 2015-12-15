using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DISConfigurationCloud.StorageManagement
{
    public interface IDatabaseManager
    {
        int CreateDatabase(string DatabaseName, string ConnectionString);

        string CreateDatabase(string ServerName, string DatabaseName, string UserName, string Password);

        int CreateBackupDevice(string BackupDeviceName, string ConnectionString);

        int BackupDatabase(string BackupDeviceName, string DatabaseName, string ConnectionString);

        int RestoreDatabase(string BackupDeviceName, string FileSequence, string DatabaseName, string ConnectionString);

        string[] ListDatabases(string ConnectionString);

        string[] ListBackupDevices(string ConnectionString);

        object GetBackupInformation(string BackupDeviceName, string ConnectionString);

        string GetConnectionString(string ServerName, string DatabaseName, string UserName, string Password);

        bool TestConnection(string ConnectionString);

        int DropDatabase(string ConnectionString, string DatabaseName);

        IDictionary<string, IDictionary<string, string[]>> ListDatabaseTables(string ConnectionString);

        IDictionary<string, IDictionary<string, string[]>> ListDatabaseTables(string ConnectionString, string DatabaseName);
    }
}
