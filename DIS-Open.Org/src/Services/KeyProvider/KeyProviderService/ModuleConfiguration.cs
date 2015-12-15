using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DIS.Business.Proxy;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Contract;

namespace DIS.Services.KeyProviderService
{
    public static class ModuleConfiguration
    {
        public static IDictionary<string, string> DISCloudConfigurations;

        public static IDictionary<string, string> DISCloudCustomers;

        public static void SyncConfigurations()
        {
            IManager manager = new Manager(DISConfigurationCloud.Client.ModuleConfiguration.IsTracingEnabled, DISConfigurationCloud.Client.ModuleConfiguration.TraceSourceName);

            Customer[] customers = manager.GetCustomers();

            if (customers != null)
            {
                SortedDictionary<string, string> configDict = new SortedDictionary<string, string>();

                SortedDictionary<string, string> custDict = new SortedDictionary<string, string>();

                foreach (var customer in customers)
                {
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
                                    custDict.Add(configuration.ID, customer.ID);
                                }
                            }
                        }
                    }
                }

                ModuleConfiguration.DISCloudConfigurations = configDict;

                ModuleConfiguration.DISCloudCustomers = custDict;

                KeyStoreProviderProxy.DISCloudConfigurations = configDict;
            }
        }
    }
}