//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using DIS.Common.Utility;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionHandler.HandleException(e.Exception, KmtConstants.CurrentDBConnectionString);
            e.Exception.ShowDialog();
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public App()
        {
            Thread.CurrentThread.CurrentUICulture = KmtConstants.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = KmtConstants.CurrentCulture;

            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = DISConfigurationCloud.Client.ModuleConfiguration.GetServicePoint(System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudServerAddress"), System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudServicePoint"));
            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudAuthHeader");
            DISConfigurationCloud.Client.ModuleConfiguration.IsTracingEnabled = bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsConfigurationCloudClientTracingEnabled"));
            DISConfigurationCloud.Client.ModuleConfiguration.TraceSourceName = System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudClientTraceSourceName");

            int cachingPolicyValue = 0;

            if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings.Get("CloudConfigCachePolicy"), out cachingPolicyValue))
            {
                DISConfigurationCloud.Client.ModuleConfiguration.CachingPolicy = ((DISConfigurationCloud.Client.CachingPolicy)(cachingPolicyValue));
            }
            else
            {
                DISConfigurationCloud.Client.ModuleConfiguration.CachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly;
            }
            
            DISConfigurationCloud.Client.ModuleConfiguration.LocalCacheStore = System.Configuration.ConfigurationManager.AppSettings.Get("CloudConfigCacheStore");

            //XSLT is currently not used, so comment out until one of them are useful. - Rally Dec 22, 2014
            //KmtConstants.XSLT_ULSKeyReportCompitable = System.AppDomain.CurrentDomain.BaseDirectory + @"\XSLT\TransformULSKeyReportCompitable.xslt";
            //KmtConstants.XSLT_DLSKeyImportCompitable = System.AppDomain.CurrentDomain.BaseDirectory + @"\XSLT\TransformDLSKeyImportCompitable.xslt";
            //KmtConstants.XSLT_ULSKeyExportCompitable = System.AppDomain.CurrentDomain.BaseDirectory + @"\XSLT\TransformULSKeyExportCompitable.xslt";
        }
    }
}
