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

using System.ServiceProcess;

namespace DIS.Services.DataPolling
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = DISConfigurationCloud.Client.ModuleConfiguration.GetServicePoint(System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudServerAddress"), System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudServicePoint"));
            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudAuthHeader");
            DISConfigurationCloud.Client.ModuleConfiguration.IsTracingEnabled = bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsConfigurationCloudClientTracingEnabled"));
            DISConfigurationCloud.Client.ModuleConfiguration.TraceSourceName = System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudClientTraceSourceName");

            //DISConfigurationCloud.Client.ModuleConfiguration.CachingPolicy = ((DISConfigurationCloud.Client.CachingPolicy)(int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("CloudConfigCachePolicy"))));

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

            ServiceBase[] ServicesToRun = new ServiceBase[] 
            { 
                new DataPollingService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
