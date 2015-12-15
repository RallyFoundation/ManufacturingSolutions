using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace DISConfigurationCloud
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            DISConfigurationCloud.StorageManagement.ModuleConfiguration.IsCustomizingDatabaseStorage = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsCustomizingDatabaseStorage"));

            DISConfigurationCloud.StorageManagement.ModuleConfiguration.SQLScriptFile_CreateDB = Server.MapPath("~/Scripts/KeyStore.publish.sql");

            DISConfigurationCloud.StorageManagement.ModuleConfiguration.DefaulDatabasePhysicalFileLocation = System.Configuration.ConfigurationManager.AppSettings.Get("DatabasePhysicalFileLocation");

            DISConfigurationCloud.StorageManagement.ModuleConfiguration.DefaultDatabaseBackupLocation = System.Configuration.ConfigurationManager.AppSettings.Get("DatabaseBackupLocation");

            DISConfigurationCloud.StorageManagement.ModuleConfiguration.DefaultSQLCMDOuputLogFilePath = System.Configuration.ConfigurationManager.AppSettings.Get("SQLCMDOuputLogFilePath");

            DISConfigurationCloud.Utility.TracingUtility.DefaultTraceSourceName = "DISConfigurationCloudTraceSource";
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
