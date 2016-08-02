using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISConfigurationCloud.MetaManagement
{
    public static class ModuleConfiguration
    {
        public static IDictionary<string, string> DISCloudConfigurations;

        public static IDictionary<string, string> DISCloudCustomers;

        public static IDictionary<string, string> DISCloudBusinessReferences;

        public static string DefaultBusinessID = "DEFAULT_BUSINESS";


        public static void SetCache()
        {
            MetaManager manager = new MetaManager();

            Customer[] customers = manager.ListCustomers();

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
                            if (!configDict.ContainsKey(configuration.ID))
                            {
                                configDict.Add(configuration.ID, configuration.DbConnectionString);
                            }

                            if (!custDict.ContainsKey(configuration.ID))//if (!custDict.ContainsKey(customer.ID))
                            {
                                //custDict.Add(customer.ID,  configuration.ID);
                                custDict.Add(configuration.ID, customer.ID);
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