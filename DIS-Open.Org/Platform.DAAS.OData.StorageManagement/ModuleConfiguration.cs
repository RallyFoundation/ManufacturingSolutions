using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform.DAAS.OData.StorageManagement
{
    public class ModuleConfiguration
    {
        public static string SQLScriptFile_CreateDB = ".\\KeyStore.publish.sql";

        public static string DefaulDatabasePhysicalFileLocation = ".\\";

        public static string DefaultDatabaseBackupLocation = ".\\";

        public static string DefaultSQLCMDOuputLogFilePath = ".\\SQLCMD-OUT.log";

        public static bool IsCustomizingDatabaseStorage = false;
    }
}
