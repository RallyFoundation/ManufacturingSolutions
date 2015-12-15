using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DISConfigurationCloud.Contract
{
    public class BusinessRule
    {
        static void ParseConnectionString(string ConnectionString, out string ServerName, out string DatabaseName, out string UserName, out string Password)
        {
            string[] fields = ConnectionString.Split(new string[] { ";" }, StringSplitOptions.None);

            ServerName = null;
            DatabaseName = null;
            UserName = null;
            Password = null;

            if ((fields != null) && (fields.Length == 4))
            {
                string[] pair = null;

                for (int i = 0; i < fields.Length; i++)
                {
                    pair = fields[i].Split(new string[] { "=" }, StringSplitOptions.None);

                    switch (i)
                    {
                        case 0:
                            {
                                ServerName = pair[1];
                                break;
                            }
                        case 1:
                            {
                                DatabaseName = pair[1];
                                break;
                            }
                        case 2:
                            {
                                UserName = pair[1];
                                break;
                            }
                        case 3:
                            {
                                Password = pair[1];
                                break;
                            }
                    }
                }
            }
        }

        static string BuildConnectionString(string ServerName, string DatabaseName, string UserName, string Password)
        {
            return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServerName, DatabaseName, UserName, Password);
        }
        public static bool CheckConfigurationCount(Customer customer) 
        {
            if ((customer != null) && (customer.Configurations != null))
            {
                if (customer.Configurations.Length > 3)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckConfigurationType(Customer customer)
        {
            if ((customer != null) && (customer.Configurations != null))
            {
                List<Configuration> configurations = new List<Configuration>(customer.Configurations);

                configurations.Sort(new CustomerConfigurationTypeComparer());

                for (int i = 0; i < configurations.Count; i++)
                {
                    if (i != (configurations.Count - 1))
                    {
                        if (configurations[i].ConfigurationType == configurations[i + 1].ConfigurationType)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static bool CheckConfigurationDBConnectionString(Customer customer, out Dictionary<string, string> suspectConfigurations)
        {
            suspectConfigurations = null;

            if ((customer != null) && (customer.Configurations != null))
            {
                List<Configuration> configurations = new List<Configuration>(customer.Configurations);

                configurations.Sort(new CustomerConfigurationDBConnectionStringComparer());

                string databaseName = "", serverName = "", userName = "", password = "", existingDBName = "", existingServerName = "";

                for (int i = 0; i < configurations.Count; i++)
                {
                    if (i != (configurations.Count - 1))
                    {
                        if (configurations[i].DbConnectionString.ToLower() == configurations[i + 1].DbConnectionString.ToLower())
                        {
                            ParseConnectionString(configurations[i].DbConnectionString.ToLower(), out serverName, out databaseName, out userName, out password);

                            if (suspectConfigurations == null)
                            {
                                suspectConfigurations = new Dictionary<string, string>();
                            }

                            if (!suspectConfigurations.ContainsKey(customer.Name))
                            {
                                suspectConfigurations.Add(customer.Name, serverName);
                            }

                            return false;
                        }
                        else
                        {
                            ParseConnectionString(configurations[i].DbConnectionString.ToLower(), out serverName, out databaseName, out userName, out password);
                            ParseConnectionString(configurations[i + 1].DbConnectionString.ToLower(), out existingServerName, out existingDBName, out userName, out password);

                            if (suspectConfigurations == null)
                            {
                                suspectConfigurations = new Dictionary<string, string>();
                            }

                            if ((serverName == existingServerName) && (databaseName == existingDBName))
                            {
                                if (!suspectConfigurations.ContainsKey(customer.Name))
                                {
                                    suspectConfigurations.Add(customer.Name, serverName);
                                }

                                return false;
                            }
                            else if (databaseName == existingDBName)
                            {
                                if (!suspectConfigurations.ContainsKey(customer.Name))
                                {
                                    suspectConfigurations.Add(customer.Name, existingServerName);
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public static bool ValidateConfigurationDBConnectionString(Customer customer) 
        {
            if ((customer != null) && (customer.Configurations != null))
            {
                List<Configuration> configurations = new List<Configuration>(customer.Configurations);

                string dbConnectionString = "";

                string[] fields = null;
                string[] subFields = null;

                string[] separator = new string[] { ";" };

                string[] subSeparator = new string[] { "=" };

                for (int i = 0; i < configurations.Count; i++)
                {
                    dbConnectionString = configurations[i].DbConnectionString.ToLower();

                    fields = dbConnectionString.Split(separator, StringSplitOptions.None);

                    if ((fields == null) || (fields.Length < 4))
                    {
                        return false;
                    }

                    subFields = fields[0].Split(subSeparator, StringSplitOptions.None);

                    if ((subFields == null) || (subFields.Length < 2))
                    {
                        return false;
                    }

                    if (subFields[1].StartsWith(".") || subFields[1].StartsWith("localhost") || subFields[1].StartsWith("(local)") || (subFields[1].StartsWith("127.")) || (subFields[1].StartsWith("(.)")))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsCustomerIDUnique(List<Customer> customers) 
        {
            if ((customers != null) && (customers.Count > 0))
            {
                foreach (var customer in customers)
                {
                    if (customers.Count((c) => (c.ID == customer.ID)) > 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool IsCustomerNameUnique(List<Customer> customers)
        {
            if ((customers != null) && (customers.Count > 0))
            {
                foreach (var customer in customers)
                {
                    if (customers.Count((c) => (c.Name == customer.Name)) > 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool IsConfigurationIDUnique(List<Customer> customers) 
        {
            if ((customers != null) && (customers.Count > 0))
            {
                foreach (var customer in customers)
                {
                    foreach (var config in customer.Configurations)
                    {
                        if (customers.Count((c) => (c.Configurations.Count((f)=>(f.ID == config.ID)) > 0)) > 1)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static bool IsDatabaseInUse(Customer[] customers, string dbConnectionString, out string databaseName, out string serverName, out Dictionary<string, string> suspectConfigurations)
        {
            databaseName = "";
            serverName = "";
            suspectConfigurations = null;
            string userName = "", password = "", existingDBName = "", existingServerName = "";

            if ((customers != null) && (customers.Length > 0))
            {
                foreach (var cust in customers)
                {
                    foreach (var custConf in cust.Configurations)
                    {
                        if (custConf.DbConnectionString.ToLower() == dbConnectionString.ToLower())
                        {
                            ParseConnectionString(dbConnectionString, out serverName, out databaseName, out userName, out password);

                            if (suspectConfigurations == null)
                            {
                                suspectConfigurations = new Dictionary<string, string>();
                            }

                            if (!suspectConfigurations.ContainsKey(cust.Name))
                            {
                                suspectConfigurations.Add(cust.Name, serverName);
                            }

                            return true;
                        }
                        else
                        {
                            ParseConnectionString(dbConnectionString, out serverName, out databaseName, out userName, out password);

                            ParseConnectionString(custConf.DbConnectionString, out existingServerName, out existingDBName, out userName, out password);

                            if (suspectConfigurations == null)
                            {
                                suspectConfigurations = new Dictionary<string, string>();
                            }

                            if ((databaseName.ToLower() == existingDBName.ToLower()) && (serverName.ToLower() == existingServerName.ToLower()))
                            {
                                if (!suspectConfigurations.ContainsKey(cust.Name))
                                {
                                    suspectConfigurations.Add(cust.Name, serverName);
                                }

                                return true;
                            }
                            else if (databaseName.ToLower() == existingDBName.ToLower())
                            {
                                if (!suspectConfigurations.ContainsKey(cust.Name))
                                {
                                    suspectConfigurations.Add(cust.Name, existingServerName);
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
