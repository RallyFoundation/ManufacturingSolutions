using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DISConfigurationCloud.Contract;

namespace DISConfigurationCloud.Client.Helpers
{
    class CachingHelper
    {
        public Customer[] ProcessCustomerCache(Customer[] remoteCustomers) 
        {
            Customer[] localCustomers = this.getCustomersFromCache();

            return this.computeCustomersWithCache(remoteCustomers, localCustomers);
        }
        private Customer[] getCustomersFromCache() 
        {
            string localCacheStore = ModuleConfiguration.LocalCacheStore;

            localCacheStore = Path.GetFullPath(localCacheStore);

            string customerCacheXml = null;

            using (FileStream fileStream = new FileStream(localCacheStore, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.GetEncoding(ModuleConfiguration.EncodingName)))
                {
                    customerCacheXml = streamReader.ReadToEnd();
                }
            }

            Customer[] customersInCache = Utility.XmlDeserialize(customerCacheXml, typeof(Customer[]), new Type[] { typeof(Customer), typeof(Configuration), typeof(Configuration[]), typeof(ConfigurationType) }, ModuleConfiguration.EncodingName) as Customer[];

            return customersInCache;
        }

        private Customer[] computeCustomersWithCache(Customer[] remoteCustomers, Customer[] customersInCache) 
        {
            switch (ModuleConfiguration.CachingPolicy)
            {
                case CachingPolicy.RemoteOnly:
                    {
                        return remoteCustomers;
                    }
                case CachingPolicy.LocalOnly:
                    { 
                        return customersInCache; 
                    }
                //case CachingPolicy.MergedAll:
                //    {
                //        List<Customer> customers = new List<Customer>(remoteCustomers);

                //        foreach (var customerInCache in customersInCache)
                //        {
                //            customers.Add(customerInCache);
                //        }

                //        return customers.ToArray();
                //    }
                //case CachingPolicy.MergedRemoteFirst:
                //    {
                //        List<Customer> customers = new List<Customer>(remoteCustomers);

                //        foreach (var customerInCache in customersInCache)
                //        {
                //            if (customers.First((c) => (c.ID == customerInCache.ID)) == null)
                //            {
                //                customers.Add(customerInCache);
                //            }
                //        }

                //        return customers.ToArray();
                //    }
                //case CachingPolicy.MergedLocalFirst:
                //    {
                //        List<Customer> customers = new List<Customer>(customersInCache);

                //        foreach (var remoteCustomer in remoteCustomers)
                //        {
                //            if (customers.First((c) => (c.ID == remoteCustomer.ID)) == null)
                //            {
                //                customers.Add(remoteCustomer);
                //            }
                //        }

                //        return customers.ToArray();
                //    }
                //case CachingPolicy.IntersectedRemoteFirst:
                //    {
                //        List<Customer> customers = new List<Customer>();

                //        foreach (var remoteCustomer in remoteCustomers)
                //        {
                //            foreach (var customerInCache in customersInCache)
                //            {
                //                if (remoteCustomer.ID == customerInCache.ID)
                //                {
                //                    customers.Add(remoteCustomer);
                //                }
                //            }
                //        }

                //        return customers.ToArray();
                //    }
                //case CachingPolicy.IntersectedLocalFirst: 
                //    {
                //        List<Customer> customers = new List<Customer>();

                //        foreach (var customerInCache in customersInCache)
                //        {
                //            foreach (var remoteCustomer in remoteCustomers)
                //            {
                //                if (remoteCustomer.ID == customerInCache.ID)
                //                {
                //                    customers.Add(customerInCache);
                //                }
                //            }
                //        }

                //        return customers.ToArray();
                //    }
                default:
                    {
                        break;
                    }
            }

            //return null;

            return remoteCustomers;
        }
    }
}
