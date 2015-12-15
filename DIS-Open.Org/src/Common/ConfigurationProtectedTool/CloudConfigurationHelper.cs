using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DISConfigurationCloud.Contract;
using DISConfigurationCloud.StorageManagement;

namespace DIS.Common.ConfigurationProtectedTool
{
    class CloudConfigurationHelper
    {
        public static void BuildConfigurationSets(string[] args, string ConfigurationStore) 
        {
            List<Customer> configurationSets = null;

            if (File.Exists(ConfigurationStore))
            {
                try
                {
                    configurationSets = LoadConfigurationSets(ConfigurationStore);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("Errors encountered loading configuration sets (a new one will be created instead): {0}", ex.ToString()));
                }
            }
            else
            {
                configurationSets = new List<Customer>();
            }

            if (configurationSets == null)
            {
                configurationSets = new List<Customer>();
            }

            if (!CloudConfigurationHelper.ValidateConfigurationSets(configurationSets))
            {
                Console.WriteLine("Integrity check failed for the configuration set loaded, please check the configuration store of \"{0}\".", ConfigurationStore);

                return;
            }

            string businessID, businessName, businessReferenceID, isContinueCreating = "Y";
            Customer configurationSet = null;

            do
            {
                Console.WriteLine("Please enter business ID (Enter for default):");
                businessID = Console.ReadLine();

                while ((!String.IsNullOrEmpty(businessID)) && (configurationSets.FirstOrDefault((o)=>(o.ID.ToLower() == businessID.ToLower())) != null))
                {
                    Console.WriteLine(String.Format("Another configuration set with the same business ID of \"{0}\" already exists, please choose a different business ID (Enter for default):", businessID));
                    businessID = Console.ReadLine();
                }

                Console.WriteLine("Please enter business name:");
                businessName = Console.ReadLine();

                while ((String.IsNullOrEmpty(businessName)) || (configurationSets.FirstOrDefault((o) => (o.Name.ToLower() == businessName.ToLower())) != null))
                {
                    if (String.IsNullOrEmpty(businessName))
                    {
                        Console.WriteLine("Business name can NOT be null! Please re-enter business name:");
                    }
                    else
                    {
                        Console.WriteLine(String.Format("Another configuration set with the same business name of \"{0}\" already exists, please choose a different business name:", businessName));
                    }

                    businessName = Console.ReadLine();
                }

                configurationSet = CreateConfigurationSet(businessID, businessName);

                Console.WriteLine("Please enter business reference ID (Optional, you can press Enter to ignore and skip; Business reference ID is case sensitive. If there are more than one business reference ID to supply, please separate each with a comma (\",\")):");
                businessReferenceID = Console.ReadLine();

                while ((!String.IsNullOrEmpty(businessReferenceID)) && (configurationSets.FirstOrDefault((o) => ((o.ReferenceID != null) && (o.ReferenceID.Contains(businessReferenceID)))) != null))
                {
                    Console.WriteLine(String.Format("The businesss reference ID of \"{0}\" has been bound to another configuration set, please choose a different business reference ID:", businessReferenceID));

                    businessReferenceID = Console.ReadLine();
                }

                if (!String.IsNullOrEmpty(businessReferenceID))
                {
                    string[] bizRefs = businessReferenceID.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);

                    while (!String.IsNullOrEmpty(businessReferenceID) && (bizRefs != null) && (configurationSets.Count((c)=>((c.ReferenceID != null) && (c.ReferenceID.Count((r)=>(bizRefs.Contains(r))) > 0))) > 0))
                    {
                        foreach (string bizRef in bizRefs)
                        {
                            foreach (var confSet in configurationSets)
                            {
                                if ((confSet.ReferenceID != null) && (confSet.ReferenceID.Contains(bizRef)))
                                {
                                    Console.WriteLine(String.Format("The businesss reference ID of \"{0}\" has been bound to another configuration set (ID: {1}, Name: {2}), please choose a different business reference ID:", bizRef, confSet.ID, confSet.Name));
                                }
                            }
                        }

                        businessReferenceID = Console.ReadLine();

                        if (!String.IsNullOrEmpty(businessReferenceID))
                        {
                            bizRefs = businessReferenceID.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        else
                        {
                            bizRefs = null;
                        }
                    }

                    configurationSet.ReferenceID = bizRefs;
                }

                configurationSets.Add(configurationSet);

                Console.WriteLine("Create another configuration set? (Y: Yes (default); N: No.)");

                isContinueCreating = Console.ReadLine();
                isContinueCreating = isContinueCreating.ToUpper();
            }
            while((configurationSet == null) || (isContinueCreating != "N"));

            if (!CloudConfigurationHelper.ValidateConfigurationSets(configurationSets))
            {
                Console.WriteLine("Integrity check failed for the configuration set loaded, please check the configuration store of \"{0}\", and the configurations will not be saved.", ConfigurationStore);

                return;
            }

            SaveConfigurationSets(ConfigurationStore, configurationSets);
        }

        public static bool ValidateConfigurationSets(List<Customer> configurationSet) 
        {
            bool result = true;

            if (!BusinessRule.IsCustomerIDUnique(configurationSet))
            {
                result = false;

                Console.WriteLine("Duplicate Business ID found!");
            }

            if (!BusinessRule.IsCustomerNameUnique(configurationSet))
            {
                result = false;

                Console.WriteLine("Duplicate Business Name found!");
            }

            if (!BusinessRule.IsConfigurationIDUnique(configurationSet))
            {
                result = false;

                Console.WriteLine("Duplicate Configuration ID found!");
            }

            foreach (var biz in configurationSet)
            {
                if (!BusinessRule.CheckConfigurationCount(biz))
                {
                    Console.WriteLine("1 configuration set should not have more than 3 configurations!");
                    result = false;
                }

                if (!BusinessRule.CheckConfigurationType(biz))
                {
                    Console.WriteLine("1 configuration set should not have 2 or more configurations that are of the same type!");
                    result = false;
                }


                string dbName = "", serverName = "";
                Dictionary<string, string> suspectConfs = null;
                string suspectConfListTemplate = "Business Name: \"{0}\", Server Name: \"{1}\"; ";
                string message = "";

                if (!BusinessRule.CheckConfigurationDBConnectionString(biz, out suspectConfs))
                {
                    Console.WriteLine("1 configuration set should not have 2 or more configurations that are using the same database!");
                    Console.WriteLine(string.Format("Business ID: {0}; Business Name: {1}", biz.ID, biz.Name));
                    result = false;
                }
                else if ((suspectConfs != null) && (suspectConfs.Count > 0)) 
                {
                    message = "Suspect databases found in the bindings of the following configurations, please double check to make sure that they are not the same database:";

                    message += System.Environment.NewLine;

                    foreach (string bizName in suspectConfs.Keys)
                    {
                        message += string.Format(suspectConfListTemplate, bizName, suspectConfs[bizName]);
                        message += System.Environment.NewLine;
                    }

                    Console.WriteLine(message);
                    result = false;
                }

                foreach (var conf in biz.Configurations)
                {
                    if ((BusinessRule.IsDatabaseInUse(configurationSet.Where((o)=>(o.ID != biz.ID)).ToArray(), conf.DbConnectionString, out serverName, out dbName, out suspectConfs)) || ((suspectConfs != null) && (suspectConfs.Count > 0)))
                    {
                       Console.WriteLine(string.Format("Business ID: {0}; Business Name: {1}", biz.ID, biz.Name));

                       message = String.Format("Database \"{0}\" on server \"{1}\" has been bound to another configuration! Please double check!", dbName, serverName);

                       message += System.Environment.NewLine;

                       foreach (string bizName in suspectConfs.Keys)
                       {
                           message += string.Format(suspectConfListTemplate, bizName, suspectConfs[bizName]);
                           message += System.Environment.NewLine;
                       }

                       Console.WriteLine(message);
                       result = false;
                    }
                }

                if (!BusinessRule.ValidateConfigurationDBConnectionString(biz))
                {
                    Console.WriteLine("Connection string is invalid! Make sure each field of the connection string has its value assigned, and also make sure loopback address is not used.");
                    result = false;
                }
            }

            return result;
        }
        public static List<Customer> LoadConfigurationSets(string ConfigurationStore) 
        {
            string configXml = "";

            using (FileStream fileStream = new FileStream(ConfigurationStore, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.GetEncoding(DISConfigurationCloud.Client.ModuleConfiguration.EncodingName)))
                {
                    configXml = streamReader.ReadToEnd();
                }
            }

           return Utility.Serializer.FromXml<List<Customer>>(configXml);
        }

        public static void SaveConfigurationSets(string ConfigurationStore, List<Customer> ConfigurationSets) 
        {
            Utility.Serializer.WriteToXml(ConfigurationSets, ConfigurationStore);
        }
        public static Customer CreateConfigurationSet(string BusinessID, string BusinessName) 
        {
            Customer configurationSet = new Customer() 
            {
                ID = (!String.IsNullOrEmpty(BusinessID)) ? BusinessID : Guid.NewGuid().ToString(),
                Name = BusinessName
            };

            bool isContinuing = true;

            while (((configurationSet.Configurations == null) || (configurationSet.Configurations.Length < 3)) && (isContinuing))
            {
                int result = -99, confType;
                string dbServerAddress, dbUserName, dbPassword, dbName;
                bool parsed;
                string isContinueCreating = "Y";
                string confTypeString;

                string installType = System.Configuration.ConfigurationManager.AppSettings.Get("InstallType");

                do
                {
                    Console.WriteLine("Please enter DB server address:");
                    dbServerAddress = Console.ReadLine();

                    Console.WriteLine("Please enter DB server username:");
                    dbUserName = Console.ReadLine();

                    Console.WriteLine("Please enter DB server password:");
                    dbPassword = Console.ReadLine();

                    Console.WriteLine("Please enter DB name:");
                    dbName = Console.ReadLine();

                    do
                    {
                        Console.WriteLine("Please enter configuration type (0: OEM; 1: TPI; 2: Factory Floor; Enter: Default):");

                        confTypeString = Console.ReadLine();

                        if (String.IsNullOrEmpty(confTypeString))
                        {
                            parsed = true;

                            if (installType.ToLower() == "oem")
                            {
                                confType = 0;
                            }
                            else if(installType.ToLower() == "tpi")
                            {
                                confType = 1;
                            }
                            else
                            {
                                confType = 2;
                            }

                            Console.WriteLine(String.Format("Default Configuration Type Selected : {0}", installType));
                        }
                        else
                        {
                            parsed = (int.TryParse(confTypeString, out confType) && ((confType >= 0) && (confType <= 2)));
                        }
                    }
                    while (!parsed);
  

                    result = CreateConfiguration(configurationSet, dbServerAddress, dbUserName, dbPassword, dbName, confType);

                    if (result > 0)
                    {
                        if (configurationSet.Configurations.Length <= 2)
                        {
                            Console.WriteLine("Create an extra configuration ? (Y: Yes (default); N: No.)");

                            isContinueCreating = Console.ReadLine();
                            isContinueCreating = isContinueCreating.ToUpper();

                            isContinuing = (isContinueCreating != "N");
                        }
                        else if (configurationSet.Configurations.Length == 3)
                        {
                            isContinueCreating = "N";
                            isContinuing = false;
                        }
                    }
                }
                while ((result <= 0) || (isContinueCreating != "N"));
            }

            return configurationSet;
        }

        public static int CreateConfiguration(Customer ConfigurationSet, string DBServerName, string DBUserName, string DBPassword, string DBName, int ConfigurationType) 
        {
            if (ConfigurationSet != null)
            {
                if (ConfigurationSet.Configurations == null)
                {
                    ConfigurationSet.Configurations = new Configuration[]{};
                }

                if ((ConfigurationSet.Configurations != null) && (ConfigurationSet.Configurations.Length > 2))
                {
                    Console.WriteLine("1 configuration set should not have more than 3 configurations!");

                    return -1;
                }

                string dbConnectionString = DatabaseManager.BuildConnectionString(DBServerName, "master", DBUserName, DBPassword);

                string[] dbNames = null;

                DatabaseManager databaseManager = new DatabaseManager();

                bool isDBConnectionStringValid = false;

                try
                {
                    isDBConnectionStringValid = databaseManager.TestConnection(dbConnectionString);
                }
                catch (Exception ex)
                {
                    isDBConnectionStringValid = false;

                    string errorMessage = String.Format("Error(s) occurred connecting to database with the information supplied (DB Server Name: {0}; DB User Name: {1}; DB Password: {2}). Please verify the database connection information you supplied and try again! Error detail: {3}", DBServerName, DBUserName, DBPassword, ex.Message);

                    Console.WriteLine(errorMessage);

                    //Console.WriteLine(ex.ToString());
                }

                if (!isDBConnectionStringValid)
                {
                    Console.WriteLine("Invalid DB connection string!");

                    return -2;
                }
                else
                {
                    dbNames = databaseManager.ListDatabases(dbConnectionString);
                }

                foreach (var configuration in ConfigurationSet.Configurations)
                {
                    if (configuration.ConfigurationType == ((ConfigurationType)(ConfigurationType)))
                    {
                        Console.WriteLine("1 configuration set should not have 2 or more configurations that are of the same type!");

                        return -3;
                    }

                    if (configuration.DbConnectionString.ToLower() == dbConnectionString.ToLower())
                    {
                        Console.WriteLine("1 configuration set should not have 2 or more configurations that are using the same database!");

                        return -4;
                    }
                }

                dbConnectionString = DatabaseManager.BuildConnectionString(DBServerName, DBName, DBUserName, DBPassword);

                if ((dbNames == null) || (String.IsNullOrEmpty(dbNames.FirstOrDefault((o)=>(o.ToLower() == DBName.ToLower())))))
                {
                    string scriptFilePath = AppDomain.CurrentDomain.BaseDirectory;

                    if (!scriptFilePath.EndsWith("\\"))
                    {
                        scriptFilePath += "\\";
                    }

                    scriptFilePath += "KeyStore.publish.sql";

                    DISConfigurationCloud.StorageManagement.ModuleConfiguration.SQLScriptFile_CreateDB = scriptFilePath;

                    string dbCreationOutput = databaseManager.CreateDatabase(DBServerName, DBName, DBUserName, DBPassword);

                    Console.WriteLine(dbCreationOutput);
                }

                Configuration config = new Configuration() 
                {
                     ConfigurationType = (DISConfigurationCloud.Contract.ConfigurationType)(ConfigurationType),
                     DbConnectionString = dbConnectionString,
                     ID = Guid.NewGuid().ToString()
                };

                List<Configuration> configs = new List<Configuration>(ConfigurationSet.Configurations);

                configs.Add(config);

                ConfigurationSet.Configurations = configs.ToArray();

                return 1;
            }
            else
            {
                Console.WriteLine("A configuration must belong to a configuration set!");

                return 0;
            }
        }
    }
}
