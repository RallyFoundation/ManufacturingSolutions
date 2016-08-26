using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using Platform.DAAS.OData.Facade;

namespace DISOpenDataCloud
{
    public class StorageConfig
    {
        public static void SetSQLServerDatabaseManagementOptions()
        {
            Global.SetDatabaseStorageCustomizationOption(Global.Should("CustomizeDatabaseStorage"));
            Global.SetDatabasePhysicalFileLocation(ConfigurationManager.AppSettings.Get("DatabasePhysicalFileLocation"));
            Global.SetDatabaseBackupLocation(ConfigurationManager.AppSettings.Get("DatabaseBackupLocation"));
            Global.SetSQLCMDOuputLogFilePath(ConfigurationManager.AppSettings.Get("SQLCMDOuputLogFilePath"));
        }

        public static void SetDISDBCreationScript(Func<String, String> FilePathParser)
        {
            Global.SetDBCreationScipt(FilePathParser, ConfigurationManager.AppSettings.Get("DatabaseCreationScriptPath"));
        }
    }
}
