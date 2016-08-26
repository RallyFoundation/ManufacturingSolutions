using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Facade
{
    public class Global
    {
        public const string HTTP_HEADER_NAME_SERVICE_ID = "DAAS-SERVICE-ID";

        public const string HTTP_HEADER_NAME_APP_ID = "DAAS-APP-ID";

        public const string HTTP_HEADER_NAME_DB_CONNECTION = "DAAS-DB-CONNECTION";

        public const string HTTP_HEADER_NAME_RESOURCE_NAME = "DAAS-RESOURCE-NAME";

        public static string ODATA_RESOURCE_ROUTE_PREFIX = "OData/Resource";

        public static void SetDBCreationScipt(Func<String, String> FilePathParser, string FilePath)
        {
            Platform.DAAS.OData.StorageManagement.ModuleConfiguration.SQLScriptFile_CreateDB = FilePathParser(FilePath);
        }

        public static void SetDatabaseStorageCustomizationOption(bool ShouldOrNot)
        {
            Platform.DAAS.OData.StorageManagement.ModuleConfiguration.IsCustomizingDatabaseStorage = ShouldOrNot;
        }

        public static void SetDatabasePhysicalFileLocation(string Location)
        {
            Platform.DAAS.OData.StorageManagement.ModuleConfiguration.DefaulDatabasePhysicalFileLocation = Location;
        }

        public static void SetDatabaseBackupLocation(string Location)
        {
            Platform.DAAS.OData.StorageManagement.ModuleConfiguration.DefaultDatabaseBackupLocation = Location;
        }

        public static void SetSQLCMDOuputLogFilePath(string FilePath)
        {
            Platform.DAAS.OData.StorageManagement.ModuleConfiguration.DefaultSQLCMDOuputLogFilePath = FilePath;
        }

        public static void RegisterAuthorizationMeta(string ConfigFilePath)
        {
            if (!String.IsNullOrEmpty(ConfigFilePath))
            {
                Platform.DAAS.OData.Security.ModuleConfiguration.DefaultResourceACConfigurationFilePath = ConfigFilePath;
            }

            Platform.DAAS.OData.Security.ModuleConfiguration.Regiser();
        }

        public static void SetAuthorizationStoreConnectionName(string StoreConnectionName)
        {
            Platform.DAAS.OData.Security.ModuleConfiguration.DefaultAuthorizationStoreConnectionName = StoreConnectionName;
        }

        public static void SetIdentityStoreConnectionName(string StoreConnectionName)
        {
            Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName = StoreConnectionName;
        }

        public static void SetObsoleteOperationMetaOption(bool ShouldOrNot)
        {
            Platform.DAAS.OData.Security.ModuleConfiguration.ShouldDeleteObsoleteOperationsOnRegistration = ShouldOrNot;
        }

        public static void SetObsoleteDataScopeMetaOption(bool ShouldOrNot)
        {
            Platform.DAAS.OData.Security.ModuleConfiguration.ShouldDeleteObsoleteDataScopesOnRegistration = ShouldOrNot;
        }

        public static void SetObsoleteRoleMetaOption(bool ShouldOrNot)
        {
            Platform.DAAS.OData.Security.ModuleConfiguration.ShouldDeleteObsoleteRolesOnRegistration = ShouldOrNot;
        }

        public static void SetObsoleteUserMetaOption(bool ShouldOrNot)
        {
            Platform.DAAS.OData.Security.ModuleConfiguration.ShouldDeleteObsoleteUsersOnRegistration = ShouldOrNot;
        }

        public static void SetTracer(string TraceSourceName)
        {
            Platform.DAAS.OData.Logging.Tracer.DefaultTraceSourceName = TraceSourceName;
        }

        public static void SetLogger(string LoggerName)
        {
            Platform.DAAS.OData.Logging.Logger.DefaultLoggerName = LoggerName;
        }

        public static void SetExceptionHandler(string PolicyName)
        {
            Platform.DAAS.OData.Logging.ExceptionHandler.DefaultPolicyName = PolicyName;
        }

        public static bool Should(string ConfigName)
        {
            string flag = System.Configuration.ConfigurationManager.AppSettings.Get(ConfigName);
            return ((!String.IsNullOrEmpty(flag)) && ((flag.ToLower() == "true") || (flag == "1")));
        }
    }
}
