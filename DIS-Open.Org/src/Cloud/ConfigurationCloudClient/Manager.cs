using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using DISConfigurationCloud.Contract;
using DISConfigurationCloud.ResourceIntegrator;
using DISConfigurationCloud.Client.Helpers;

namespace DISConfigurationCloud.Client
{
    public class Manager : IManager
    {
        IResourceRouter router;

        public Manager (bool IsTracingEnabled, string TraceSourceName)
        {
            this.router = new ResourceRouter(IsTracingEnabled, TraceSourceName);
        }
        public Contract.Customer[] GetCustomers()
        {
            Customer[] customers = null;

            if (ModuleConfiguration.CachingPolicy != CachingPolicy.LocalOnly)
            {
                string url = ModuleConfiguration.ServicePoint + ModuleConfiguration.UrlGetCustomers;

                object result = this.router.Get(url, new Authentication() { Type = AuthenticationType.Custom }, new Dictionary<string, string>() { { HttpRequestHeader.Authorization.ToString(), ModuleConfiguration.AuthorizationHeaderValue } });

                if (result != null)
                {
                    customers = new DataContractSerializer(typeof(Customer[]), "ArrayOfCustomer", "http://schemas.datacontract.org/2004/07/DISConfigurationCloud.MetaManagement").ReadObject(new MemoryStream(System.Text.Encoding.GetEncoding(ModuleConfiguration.EncodingName).GetBytes(result.ToString()))) as Customer[]; //Utility.XmlDeserialize(result.ToString(), typeof(Customer[]), new Type[] { typeof(Customer), typeof(Configuration), typeof(ConfigurationType), typeof(Configuration[]) }, ModuleConfiguration.EncodingName) as Customer[];
                }
            }

            if (ModuleConfiguration.CachingPolicy != CachingPolicy.RemoteOnly)
            {
                CachingHelper cachingHelper = new CachingHelper();

                customers = cachingHelper.ProcessCustomerCache(customers);
            }

            return customers;
        }

        public Contract.Configuration GetConfiguration(string ConfigurationID)
        {
            Configuration configuration = null;

            string url = ModuleConfiguration.ServicePoint + ModuleConfiguration.UrlGetConfiguration;

            url = String.Format(url, ConfigurationID);

            object result = this.router.Get(url, new Authentication() { Type = AuthenticationType.Custom }, new Dictionary<string, string>() { { HttpRequestHeader.Authorization.ToString(), ModuleConfiguration.AuthorizationHeaderValue } });

            if (result != null)
            {
                configuration = new DataContractSerializer(typeof(Configuration), "Configuration", "http://schemas.datacontract.org/2004/07/DISConfigurationCloud.MetaManagement").ReadObject(new MemoryStream(System.Text.Encoding.GetEncoding(ModuleConfiguration.EncodingName).GetBytes(result.ToString()))) as Configuration; //Utility.XmlDeserialize(result.ToString(), typeof(Configuration), new Type[] { typeof(ConfigurationType) }, ModuleConfiguration.EncodingName) as Configuration;
            }

            return configuration;
        }

        public string GetDBConnectionString(string ConfigurationID)
        {
            Configuration configuration = this.GetConfiguration(ConfigurationID);

            if (configuration != null)
            {
                return configuration.DbConnectionString;
            }

            return null;
        }


        public string Test()
        {
            string url = ModuleConfiguration.ServicePoint + ModuleConfiguration.UrlTest;

            object result = this.router.Get(url, new Authentication() { Type = AuthenticationType.Custom }, new Dictionary<string, string>() { { HttpRequestHeader.Authorization.ToString(), ModuleConfiguration.AuthorizationHeaderValue } });

            if (result != null)
            {
                //return result.ToString();

                return new DataContractSerializer(typeof(String), "string", "http://schemas.microsoft.com/2003/10/Serialization/").ReadObject(new MemoryStream(System.Text.Encoding.GetEncoding(ModuleConfiguration.EncodingName).GetBytes(result.ToString()))) as String;
            }

            return null;
        }
    }
}
