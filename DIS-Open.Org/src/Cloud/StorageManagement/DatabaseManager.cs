using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace DISConfigurationCloud.StorageManagement
{
    public class DatabaseManager : IDatabaseManager
    {
        public static void ParseConnectionString(string ConnectionString, out string ServerName, out string DatabaseName, out string UserName, out string Password) 
        {
            string[] fields = ConnectionString.Split(new string[] { ";" }, StringSplitOptions.None);

            ServerName = null;
            DatabaseName = null;
            UserName = null;
            Password = null;

            if ((fields != null) && (fields.Length == 4))
            {
                string[] pair = null;

                for (int i = 0; i < fields.Length; i++)
                {
                    pair = fields[i].Split(new string[] { "=" }, StringSplitOptions.None);

                    switch (i)
                    {
                        case 0:
                            {
                                ServerName = pair[1];
                                break;
                            }
                        case 1:
                            {
                                DatabaseName = pair[1];
                                break;
                            }
                        case 2:
                            {
                                UserName = pair[1];
                                break;
                            }
                        case 3:
                            {
                                Password = pair[1];
                                break;
                            }
                    }
                }
            }
        }

        public static string BuildConnectionString(string ServerName, string DatabaseName, string UserName, string Password)
        {
            return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServerName, DatabaseName, UserName, Password);
        }
        public int CreateDatabase(string DatabaseName, string ConnectionString)
        {
            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            string script = String.Format("CREATE DATABASE {0}", DatabaseName);

            return server.ConnectionContext.ExecuteNonQuery(script);
        }

        public string[] ListDatabases(string ConnectionString)
        {
            List<string> databaseNames = new List<string>();

            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            DatabaseCollection databases = server.Databases;

            for (int i = 0; i < databases.Count; i++)
            {
                if (!databases[i].IsSystemObject)
                {
                    databaseNames.Add(databases[i].Name);
                }
            }

            return databaseNames.ToArray();
        }


        public int CreateBackupDevice(string BackupDeviceName, string ConnectionString)
        {
            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            string script = "EXEC SP_AddumpDevice 'DISK', '{0}', '{1}'";

            string backupDeviceFilePath = ModuleConfiguration.DefaultDatabaseBackupLocation;

            if (!backupDeviceFilePath.EndsWith("\\"))
            {
                backupDeviceFilePath += "\\";
            }

            backupDeviceFilePath += BackupDeviceName;
            backupDeviceFilePath += ".bak";

            script = String.Format(script, BackupDeviceName, backupDeviceFilePath);

            return server.ConnectionContext.ExecuteNonQuery(script);
        }


        public int BackupDatabase(string BackupDeviceName, string DatabaseName, string ConnectionString)
        {
            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            string script = "BACKUP DATABASE {0} TO {1}";

            script = String.Format(script, DatabaseName, BackupDeviceName);

            return server.ConnectionContext.ExecuteNonQuery(script);
        }

        public int RestoreDatabase(string BackupDeviceName, string FileSequence, string DatabaseName, string ConnectionString)
        {
            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            string script = "RESTORE DATABASE {0} FROM {1} WITH FILE = {2}, REPLACE";

            script = String.Format(script, DatabaseName, BackupDeviceName, FileSequence);

            return server.ConnectionContext.ExecuteNonQuery(script);
        }

        public string[] ListBackupDevices(string ConnectionString)
        {
            List<string> backupNames = new List<string>();

            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            BackupDeviceCollection backups = server.BackupDevices;

            for (int i = 0; i < backups.Count; i++)
            {
                backupNames.Add(backups[i].Name);
            }

            return backupNames.ToArray();
        }

        public string GetConnectionString(string ServerName, string DatabaseName, string UserName, string Password)
        {
            return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServerName, DatabaseName, UserName, Password);
        }

        public bool TestConnection(string ConnectionString)
        {
            bool returnValue = false;

            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();

            returnValue = (connection.Database != null);

            connection.Close();

            return returnValue;
        }


        public object GetBackupInformation(string BackupDeviceName, string ConnectionString)
        {
            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            BackupDeviceCollection backups = server.BackupDevices;

            System.Data.DataTable backupHeader = null;

            for (int i = 0; i < backups.Count; i++)
            {
                if (backups[i].Name.ToLower() == BackupDeviceName.ToLower())
                {
                    backupHeader = backups[i].ReadBackupHeader();

                    break;
                }
            }

            return backupHeader;
        }


        public string CreateDatabase(string ServerName, string DatabaseName, string UserName, string Password)
        {
            //string sqlcmdCommandArguments = String.Format("-S {0} -U {1} -P {2} -v DatabaseName=\"{3}\" DefaultFilePrefix=\"{4}\" DefaultDataPath=\"{5}\" DefaultLogPath=\"{6}\" -i {7} -o {8}", ServerName, UserName, Password, DatabaseName, DatabaseName, ModuleConfiguration.DefaulDatabasePhysicalFileLocation, ModuleConfiguration.DefaulDatabasePhysicalFileLocation, ModuleConfiguration.SQLScriptFile_CreateDB, ModuleConfiguration.DefaultSQLCMDOuputLogFilePath);

            string sqlcmdCommandArguments = String.Format("-S {0} -U {1} -P {2} -v DatabaseName=\"{3}\" -i \"{4}\"", ServerName, UserName, Password, DatabaseName, ModuleConfiguration.SQLScriptFile_CreateDB);

            if (ModuleConfiguration.IsCustomizingDatabaseStorage)
            {
                sqlcmdCommandArguments = String.Format("-S {0} -U {1} -P {2} -v DatabaseName=\"{3}\" DefaultFilePrefix=\"{4}\" DefaultDataPath=\"{5}\" DefaultLogPath=\"{6}\" -i \"{7}\"", ServerName, UserName, Password, DatabaseName, DatabaseName, ModuleConfiguration.DefaulDatabasePhysicalFileLocation, ModuleConfiguration.DefaulDatabasePhysicalFileLocation, ModuleConfiguration.SQLScriptFile_CreateDB);
            }

            //System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo() 
            //{
            //     FileName = "sqlcmd",
            //     Arguments = sqlcmdCommandArguments,
            //     UseShellExecute = false,
            //     RedirectStandardError = true,
            //     RedirectStandardOutput = true,
            //};

            System.Diagnostics.Process process = new Process();

            process.StartInfo.FileName = "sqlcmd";
            process.StartInfo.Arguments = sqlcmdCommandArguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            string result = process.StandardOutput.ReadToEnd();

            //process.CloseMainWindow();
            process.Close();

            return result;
        }


        public int DropDatabase(string ConnectionString, string DatabaseName)
        {
            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            server.KillDatabase(DatabaseName);

            return 1;
        }


        public IDictionary<string, IDictionary<string, string[]>> ListDatabaseTables(string ConnectionString)
        {
            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            IDictionary<string, IDictionary<string, string[]>> databaseDictionary = new SortedDictionary<string, IDictionary<string, string[]>>();

            IDictionary<string, string[]> tableDictionary = null;

            List<string> columnNameList = null;

            DatabaseCollection databases = server.Databases;

            for (int i = 0; i < databases.Count; i++)
            {
                if (!databases[i].IsSystemObject)
                {
                    tableDictionary = new SortedDictionary<string, string[]>();

                    for (int j = 0; j < databases[i].Tables.Count; j++)
                    {
                        if (!databases[i].Tables[j].IsSystemObject)
                        {
                            columnNameList = new List<string>();

                            for (int k = 0; k < databases[i].Tables[j].Columns.Count; k++)
                            {
                                columnNameList.Add(databases[i].Tables[j].Columns[k].Name);
                            }

                            tableDictionary.Add(databases[i].Tables[j].Name, columnNameList.ToArray());
                        }
                    }

                    databaseDictionary.Add(databases[i].Name, tableDictionary);
                }
            }

            return databaseDictionary;
        }


        public IDictionary<string, IDictionary<string, string[]>> ListDatabaseTables(string ConnectionString, string DatabaseName)
        {
            Server server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));

            IDictionary<string, IDictionary<string, string[]>> databaseDictionary = new SortedDictionary<string, IDictionary<string, string[]>>();

            IDictionary<string, string[]> tableDictionary = null;

            List<string> columnNameList = null;

            DatabaseCollection databases = server.Databases;

            for (int i = 0; i < databases.Count; i++)
            {
                if ((!databases[i].IsSystemObject) && (databases[i].Name.ToLower() == DatabaseName.ToLower()))
                {
                    tableDictionary = new SortedDictionary<string, string[]>();

                    for (int j = 0; j < databases[i].Tables.Count; j++)
                    {
                        if (!databases[i].Tables[j].IsSystemObject)
                        {
                            columnNameList = new List<string>();

                            for (int k = 0; k < databases[i].Tables[j].Columns.Count; k++)
                            {
                                columnNameList.Add(databases[i].Tables[j].Columns[k].Name);
                            }

                            tableDictionary.Add(databases[i].Tables[j].Name, columnNameList.ToArray());
                        }
                    }

                    databaseDictionary.Add(databases[i].Name, tableDictionary);

                    break;
                }
            }

            return databaseDictionary;
        }
    }
}
