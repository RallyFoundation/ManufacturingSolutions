using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Management.Automation;
using DISConfigurationCloud.Contract;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Utility;

namespace DIS.Management.Deployment
{
    [Cmdlet(VerbsCommon.New, "DISConfigurationCloudCache", DefaultParameterSetName = "Common")]
    public class NewDISConfigurationCloudCacheCmdlet : Cmdlet
    {
        private bool isDefaultBusiness = false;

        private bool isEmptyCache = false;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Common", HelpMessage = "The unique identifier that identifies the business.")]
        public string BusinessID { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "Common", HelpMessage = "The name of the business.")]
        public string BusinessName { get; set; }

        [Parameter(Position = 2, Mandatory = true, ParameterSetName = "Common", HelpMessage = "The type of the configuration.")]
        [Parameter(Position = 2, Mandatory = true, ParameterSetName = "DefaultBusiness", HelpMessage = "The type of the configuration.")]
        public ConfigurationType ConfigurationType { get; set; }

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = "Common", HelpMessage = "The value for the DB connection stirng in the configuration set.")]
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "DefaultBusiness", HelpMessage = "The value for the DB connection stirng in the configuration set.")]
        [Parameter(Position = 1, Mandatory = false,ParameterSetName = "EmptyCache", HelpMessage = "The value for the DB connection stirng in the configuration set.")]
        public string DBConnectionString { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The path to the file to which the cache is saved.")]
        public string CacheFilePath { get; set; }

        [Parameter(Position = 0, ParameterSetName = "DefaultBusiness", HelpMessage = "If specified, a cache containing a configuration set for a default business will be created.")]
        public bool Default { get { return this.isDefaultBusiness; } set { this.isDefaultBusiness = true; } }

        [Parameter(Position = 0, ParameterSetName = "EmptyCache", HelpMessage = "If specified, a cache containing no configuration set will be created.")]
        public bool Empty { get { return this.isEmptyCache; } set { this.isEmptyCache = true; } }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            List<Customer> customers = new List<Customer>();

            Customer customer = null;

            XmlUtility xmlUtility = new XmlUtility();

            Type[] types = new Type[]
            {
                typeof(Customer),
                typeof(Configuration),
                typeof(Configuration[]),
                typeof(ConfigurationType)
            };

            string customerXml;

            if(this.isDefaultBusiness)
            {
                customer = new Customer()
                {
                    ID = "DEFAULT_BUSINESS",
                    Name = "DefaultBusiness",
                    Configurations = new Configuration[] 
                    {
                        new Configuration()
                        {
                            ConfigurationType = this.ConfigurationType,
                            DbConnectionString = this.DBConnectionString,
                            ID = Guid.NewGuid().ToString()
                        }
                    }
                };
            }
            else if (!this.isEmptyCache)
            {
                customer = new Customer()
                {
                    ID = this.BusinessID,
                    Name = this.BusinessName,
                    Configurations = new Configuration[] 
                    {
                        new Configuration()
                        {
                            ConfigurationType = this.ConfigurationType,
                            DbConnectionString = this.DBConnectionString,
                            ID = Guid.NewGuid().ToString()
                        }
                    }
                };
            }

            if (customer != null)
            {
                customers.Add(customer);
            }

            customerXml = xmlUtility.XmlSerialize(customers, types, "utf-8");

            using (FileStream stream = new FileStream(this.CacheFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(customerXml);
                }
            }

            this.WriteObject(customers);
        }
    }
}
