using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DISConfigurationCloud.DataModel;

namespace DISConfigurationCloud.MetaManagement
{
    public class MetaManager : IMetaManager
    {
        public string AddCustomerConfiguration(Customer Customer)
        {
            if (!BusinessRule.CheckConfigurationCount(Customer)) 
            {
                throw new ConfigurationCountLimitExceededException("1 configuration set should not have more than 3 configurations!");
            }

            if (!BusinessRule.CheckConfigurationType(Customer)) 
            {
                throw new DuplicatedConfigurationTypeException("1 configuration set should not have 2 or more configurations that are of the same type!");
            }

            Dictionary<string, string> suspectConfs = null;

            if (!BusinessRule.CheckConfigurationDBConnectionString(Customer, out suspectConfs)) 
            {
                throw new DuplicatedDatabaseException("1 configuration set should not have 2 or more configurations that are using the same database!");
            }
            else if((suspectConfs != null) && (suspectConfs.Count > 0))
            {
                string message = "Suspect databases found in the bindings of the following configurations, please double check to make sure that they are not the same database:";

                string suspectConfListTemplate = "Business Name: \"{0}\", Server Name: \"{1}\"; ";

                message += System.Environment.NewLine;

                foreach (string bizName in suspectConfs.Keys)
                {
                    message += string.Format(suspectConfListTemplate, bizName, suspectConfs[bizName]);
                    message += System.Environment.NewLine;
                }

                throw new DuplicatedDatabaseException(message);
            }

            if (!BusinessRule.ValidateConfigurationDBConnectionString(Customer))
            {
                throw new InvalidConnectionStringException("Connection string is invalid! Make sure each field of the connection string has its value assigned, and also make sure loopback address is not used.");
            }

            using (DataModelContainer container = new DataModelContainer())
            {
                DataModel.Customer cust = new DataModel.Customer()
                {
                   Id = Customer.ID,
                   Name = Customer.Name
                };

                //if (String.IsNullOrEmpty(Customer.ReferenceID))
                //{
                //    cust.ReferenceId = Customer.ReferenceID;
                //}

                if (Customer.ReferenceID != null)
                {
                    cust.ReferenceId = "";

                    for (int i = 0; i < Customer.ReferenceID.Length; i++)
                    {
                        cust.ReferenceId += Customer.ReferenceID[i];

                        if (i != (Customer.ReferenceID.Length - 1))
                        {
                            cust.ReferenceId += ",";
                        }
                    }
                }

                container.Customers.Add(cust);

                foreach (var conf in Customer.Configurations)
                {
                    if (conf != null)
                    {
                        DataModel.Configuration configuration = new DataModel.Configuration()
                        {
                            Customer = cust,
                            Id = conf.ID,
                            CustomerId = cust.Id,
                            DbConnectionString = conf.DbConnectionString,
                            TypeId = (int)(conf.ConfigurationType)
                        };

                        container.Configurations.Add(configuration);
                    }
                }

                container.SaveChanges();
            }

            return Customer.ID;
        }

        public int UpdateCustomerConfiguration(Customer Customer)
        {
            if (!BusinessRule.CheckConfigurationCount(Customer))
            {
                throw new ConfigurationCountLimitExceededException("1 configuration set should not have more than 3 configurations!");
            }

            if (!BusinessRule.CheckConfigurationType(Customer))
            {
                throw new DuplicatedConfigurationTypeException("1 configuration set should not have 2 or more configurations that are of the same type!");
            }

            Dictionary<string, string> suspectConfs = null;

            if (!BusinessRule.CheckConfigurationDBConnectionString(Customer, out suspectConfs))
            {
                throw new DuplicatedDatabaseException("1 configuration set should not have 2 or more configurations that are using the same database!");
            }
            else if ((suspectConfs != null) && (suspectConfs.Count > 0))
            {
                string message = "Suspect databases found in the bindings of the following configurations, please double check to make sure that they are not the same database:";

                string suspectConfListTemplate = "Business Name: \"{0}\", Server Name: \"{1}\"; ";

                message += System.Environment.NewLine;

                foreach (string bizName in suspectConfs.Keys)
                {
                    message += string.Format(suspectConfListTemplate, bizName, suspectConfs[bizName]);
                    message += System.Environment.NewLine;
                }

                throw new DuplicatedDatabaseException(message);
            }

            if (!BusinessRule.ValidateConfigurationDBConnectionString(Customer))
            {
                throw new InvalidConnectionStringException("Connection string is invalid! Make sure each field of the connection string has its value assigned, and also make sure loopback address is not used.");
            }

            int returnValue = -9;

            using (DataModel.DataModelContainer container = new DataModelContainer())
            {
                DataModel.Customer cust = container.Customers.First((o) => (o.Id.ToLower() == Customer.ID.ToLower()));

                cust.Name = Customer.Name;

                //cust.ReferenceId = Customer.ReferenceID;

                if (Customer.ReferenceID != null)
                {
                    cust.ReferenceId = "";

                    for (int i = 0; i < Customer.ReferenceID.Length; i++)
                    {
                        cust.ReferenceId += Customer.ReferenceID[i];

                        if (i != (Customer.ReferenceID.Length - 1))
                        {
                            cust.ReferenceId += ",";
                        }
                    }
                }

                var confs = container.Configurations.Where((o)=>(o.CustomerId == Customer.ID)).ToArray();

                List<Configuration> customerConfList = new List<Configuration>(Customer.Configurations);

                for (int i = 0; i < confs.Length; i++)
                {
                    for (int j = 0; j < customerConfList.Count; j++)
                    {
                        if ((confs[i] != null) && (customerConfList[j] != null))
                        {
                            if (confs[i].Id.ToLower() == customerConfList[j].ID.ToLower())
                            {
                                confs[i].DbConnectionString = customerConfList[j].DbConnectionString;
                                confs[i].TypeId = ((int)(customerConfList[j].ConfigurationType));

                                customerConfList.RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }

                if (customerConfList.Count > 0)
                {
                    foreach (var conf in customerConfList)
                    {
                        if (conf != null)
                        {
                            DataModel.Configuration configuration = new DataModel.Configuration()
                            {
                                Customer = cust,
                                Id = conf.ID,
                                CustomerId = cust.Id,
                                DbConnectionString = conf.DbConnectionString,
                                TypeId = (int)(conf.ConfigurationType)
                            };

                            container.Configurations.Add(configuration);
                        }
                    }
                }

                returnValue = container.SaveChanges();
            }

            return returnValue;
        }

        public Customer[] ListCustomers()
        {
            List<Customer> customers = new List<Customer>();
            List<Configuration> configurations = null;

            using (DataModel.DataModelContainer container = new DataModelContainer())
            {
                Customer customer = null;

                foreach (var cust in container.Customers)
                {
                    customer = new Customer() 
                    { 
                         ID = cust.Id,
                         Name = cust.Name,
                         //ReferenceID = cust.ReferenceId
                    };

                    if (!String.IsNullOrEmpty(cust.ReferenceId))
                    {
                        customer.ReferenceID = cust.ReferenceId.Split(new string[] { "," }, StringSplitOptions.None);
                    }

                    var confs = container.Configurations.Where((o)=>(o.CustomerId.ToLower() == cust.Id.ToLower())).ToArray();

                    if ((confs != null) && (confs.Length > 0))
                    {
                        configurations = new List<Configuration>();

                        foreach (var conf in confs)
                        {
                            configurations.Add(new Configuration() 
                            {
                                 ID = conf.Id,
                                 ConfigurationType = ((ConfigurationType)(conf.TypeId)),
                                 DbConnectionString = conf.DbConnectionString
                            });
                        }

                        customer.Configurations = configurations.ToArray();
                    }

                    customers.Add(customer);
                }
            }

            return customers.ToArray();
        }

        public Customer[] ListCustomers(bool IsIncludingConfigurations)
        {
            List<Customer> customers = new List<Customer>();
            List<Configuration> configurations = null;

            using (DataModel.DataModelContainer container = new DataModelContainer())
            {
                Customer customer = null;

                foreach (var cust in container.Customers)
                {
                    customer = new Customer()
                    {
                        ID = cust.Id,
                        Name = cust.Name,
                        //ReferenceID = cust.ReferenceId
                    };

                    if (!String.IsNullOrEmpty(cust.ReferenceId))
                    {
                        customer.ReferenceID = cust.ReferenceId.Split(new string[] { "," }, StringSplitOptions.None);
                    }

                    if (IsIncludingConfigurations)
                    {
                        var confs = container.Configurations.Where((o) => (o.CustomerId.ToLower() == cust.Id.ToLower())).ToArray();

                        if ((confs != null) && (confs.Length > 0))
                        {
                            configurations = new List<Configuration>();

                            foreach (var conf in confs)
                            {
                                configurations.Add(new Configuration()
                                {
                                    ID = conf.Id,
                                    ConfigurationType = ((ConfigurationType)(conf.TypeId)),
                                    DbConnectionString = conf.DbConnectionString
                                });
                            }

                            customer.Configurations = configurations.ToArray();
                        }
                    }

                    customers.Add(customer);
                }
            }

            return customers.ToArray();
        }

        public Customer GetCustomer(string CustomerID)
        {
            Customer customer = new Customer();

            using (DataModel.DataModelContainer container = new DataModelContainer())
            {
               var cust = container.Customers.First((o) => (o.Id.ToLower() == CustomerID.ToLower()));

               customer.ID = cust.Id;
               customer.Name = cust.Name;
               //customer.ReferenceID = cust.ReferenceId;

               if (!String.IsNullOrEmpty(cust.ReferenceId))
               {
                   customer.ReferenceID = cust.ReferenceId.Split(new string[] { "," }, StringSplitOptions.None);
               }

               var confs = container.Configurations.Where((o) => (o.CustomerId.ToLower() == CustomerID.ToLower()));

               List<Configuration> configurations = new List<Configuration>();

               foreach (var conf in confs)
               {
                   configurations.Add(new Configuration()
                   {
                       ID = conf.Id,
                       ConfigurationType = (ConfigurationType)(conf.TypeId),
                       DbConnectionString = conf.DbConnectionString
                   });
               }

               customer.Configurations = configurations.ToArray();
            }

            return customer;
        }


        public Configuration GetCustomerConfiguration(string CustomerID, ConfigurationType ConfigurationType)
        {
            Configuration configuration = new Configuration();

            using (DataModel.DataModelContainer container = new DataModelContainer())
            {
                var conf = container.Configurations.First((o) => ((o.CustomerId.ToLower() == CustomerID.ToLower()) && (o.TypeId == (int)(ConfigurationType))));

                configuration.ID = conf.Id;
                configuration.ConfigurationType = ConfigurationType;
                configuration.DbConnectionString = conf.DbConnectionString;
            }

            return configuration;
        }

        public Configuration GetCustomerConfiguration(string ConfigurationID)
        {
            Configuration configuration = new Configuration();

            using (DataModel.DataModelContainer container = new DataModelContainer())
            {
                var conf = container.Configurations.First((o) => (o.Id.ToLower() == ConfigurationID.ToLower()));

                configuration.ID = conf.Id;
                configuration.ConfigurationType = ((ConfigurationType)(conf.TypeId));
                configuration.DbConnectionString = conf.DbConnectionString;
            }

            return configuration;
        }
    }
}
