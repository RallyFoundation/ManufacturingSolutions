using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Contract;

namespace DIS.Services.WebServiceLibrary
{
    public static class ModuleConfiguration
    {
        public static IDictionary<string, string> DISCloudConfigurations;

        public static IDictionary<string, string> DISCloudCustomers;

        public static IDictionary<string, string> DISCloudBusinessReferences;

        public static string DefaultBusinessID = "DEFAULT_BUSINESS";

        public static void SetDISCouldParameters() 
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
        }

        public static void SyncConfigurations()
        {
            IManager manager = new Manager(DISConfigurationCloud.Client.ModuleConfiguration.IsTracingEnabled, DISConfigurationCloud.Client.ModuleConfiguration.TraceSourceName);

            Customer[] customers = manager.GetCustomers();

            if (customers != null)
            {
                SortedDictionary<string, string> configDict = new SortedDictionary<string, string>();

                SortedDictionary<string, string> custDict = new SortedDictionary<string, string>();

                SortedDictionary<string, string> bizRefDict = new SortedDictionary<string, string>();

                foreach (var customer in customers)
                {
                    //if ((!String.IsNullOrEmpty(customer.ReferenceID)) && (!bizRefDict.ContainsKey(customer.ReferenceID)))
                    //{
                    //    bizRefDict.Add(customer.ReferenceID, customer.ID);
                    //}

                    if (customer.ReferenceID != null)
                    {
                        foreach (string referenceID in customer.ReferenceID)
                        {
                            if ((!String.IsNullOrEmpty(referenceID)) && (!bizRefDict.ContainsKey(referenceID)))
                            {
                                bizRefDict.Add(referenceID, customer.ID);
                            }
                        }
                    }

                    if (customer.Configurations != null)
                    {
                        foreach (var configuration in customer.Configurations)
                        {
                            if (Data.DataContract.Constants.InstallType.ToString().ToLower() == configuration.ConfigurationType.ToString().ToLower())
                            {
                                if (!configDict.ContainsKey(configuration.ID))
                                {
                                    configDict.Add(configuration.ID, configuration.DbConnectionString);
                                }

                                if (!custDict.ContainsKey(customer.ID))
                                {
                                    custDict.Add(customer.ID, configuration.ID);
                                }
                            }
                        }
                    }
                }

                ModuleConfiguration.DISCloudConfigurations = configDict;

                ModuleConfiguration.DISCloudCustomers = custDict;

                ModuleConfiguration.DISCloudBusinessReferences = bizRefDict;
            }
        }
    }
}