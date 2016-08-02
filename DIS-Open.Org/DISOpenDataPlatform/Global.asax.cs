using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace ODataPlatform
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitializeServiceManagementModule();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterScripts();
        }

        private void RegisterScripts()
        {
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-1.7.1.min.js",
                DebugPath = "~/Scripts/jquery-1.7.1.js",
                CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js",
                CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            });
        }

        private void InitializeServiceManagementModule()
        {
            string modulePath = System.Configuration.ConfigurationManager.AppSettings.Get("defaultCustomModulePath");

            string appRoot = AppDomain.CurrentDomain.BaseDirectory;

            if (String.IsNullOrEmpty(modulePath))
            {
                return;
            }

            if (!modulePath.EndsWith("\\"))
            {
                modulePath = modulePath + "\\";
            }

            if (modulePath.StartsWith("\\"))
            {
                modulePath = appRoot.EndsWith("\\") ? (appRoot + modulePath.Substring(1)) : (appRoot + modulePath);
            }
            else if (!modulePath.Contains(":"))
            {
                modulePath = appRoot.EndsWith("\\") ? (appRoot + modulePath) : (appRoot + "\\" + modulePath);
            }

            Platform.DAAS.OData.ServiceManager.ModuleConfiguration.Default_Model_Assembly_Path = modulePath;

            Platform.DAAS.OData.ServiceManager.ModuleConfiguration.Initialize();
        }
    }
}
