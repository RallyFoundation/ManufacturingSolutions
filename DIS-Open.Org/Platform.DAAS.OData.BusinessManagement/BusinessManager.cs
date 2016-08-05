using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Core.BusinessManagement;
using Platform.DAAS.OData.Core.DomainModel;

namespace Platform.DAAS.OData.BusinessManagement
{
    public class BusinessManager : IBusinessManager
    {
        public string AddBusinessConfiguration(Core.DomainModel.Business Business)
        {
            if (!BusinessRule.CheckConfigurationCount(Business))
            {
                throw new ConfigurationCountLimitExceededException("1 configuration set should not have more than 3 configurations!");
            }

            if (!BusinessRule.CheckConfigurationType(Business))
            {
                throw new DuplicatedConfigurationTypeException("1 configuration set should not have 2 or more configurations that are of the same type!");
            }

            Dictionary<string, string> suspectConfs = null;

            if (!BusinessRule.CheckConfigurationDBConnectionString(Business, out suspectConfs))
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

            if (!BusinessRule.ValidateConfigurationDBConnectionString(Business))
            {
                throw new InvalidConnectionStringException("Connection string is invalid! Make sure each field of the connection string has its value assigned, and also make sure loopback address is not used.");
            }

            using (DataModelContainer container = new DataModelContainer())
            {
                Business biz = new Business()
                {
                    Id = Business.ID,
                    Name = Business.Name
                };

                //if (String.IsNullOrEmpty(Customer.ReferenceID))
                //{
                //    cust.ReferenceId = Customer.ReferenceID;
                //}

                if (Business.ReferenceID != null)
                {
                    biz.ReferenceId = "";

                    for (int i = 0; i < Business.ReferenceID.Length; i++)
                    {
                        biz.ReferenceId += Business.ReferenceID[i];

                        if (i != (Business.ReferenceID.Length - 1))
                        {
                            biz.ReferenceId += ",";
                        }
                    }
                }

                container.Businesses.Add(biz);

                foreach (var conf in Business.Configurations)
                {
                    if (conf != null)
                    {
                        Configuration configuration = new Configuration()
                        {
                            Business = biz,
                            Id = conf.ID,
                            BusinessId = biz.Id,
                            DbConnectionString = conf.DbConnectionString,
                            TypeId = (int)(conf.ConfigurationType)
                        };

                        container.Configurations.Add(configuration);
                    }
                }

                container.SaveChanges();
            }

            return Business.ID;
        }

        public int UpdateBusinessConfiguration(Core.DomainModel.Business Business)
        {
            if (!BusinessRule.CheckConfigurationCount(Business))
            {
                throw new ConfigurationCountLimitExceededException("1 configuration set should not have more than 3 configurations!");
            }

            if (!BusinessRule.CheckConfigurationType(Business))
            {
                throw new DuplicatedConfigurationTypeException("1 configuration set should not have 2 or more configurations that are of the same type!");
            }

            Dictionary<string, string> suspectConfs = null;

            if (!BusinessRule.CheckConfigurationDBConnectionString(Business, out suspectConfs))
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

            if (!BusinessRule.ValidateConfigurationDBConnectionString(Business))
            {
                throw new InvalidConnectionStringException("Connection string is invalid! Make sure each field of the connection string has its value assigned, and also make sure loopback address is not used.");
            }

            int returnValue = -9;

            using (DataModelContainer container = new DataModelContainer())
            {
                Business biz = container.Businesses.First((o) => (o.Id.ToLower() == Business.ID.ToLower()));

                biz.Name = Business.Name;

                //cust.ReferenceId = Customer.ReferenceID;

                if (Business.ReferenceID != null)
                {
                    biz.ReferenceId = "";

                    for (int i = 0; i < Business.ReferenceID.Length; i++)
                    {
                        biz.ReferenceId += Business.ReferenceID[i];

                        if (i != (Business.ReferenceID.Length - 1))
                        {
                            biz.ReferenceId += ",";
                        }
                    }
                }

                var confs = container.Configurations.Where((o) => (o.BusinessId == Business.ID)).ToArray();

                List<Core.DomainModel.Configuration> bizConfList = new List<Core.DomainModel.Configuration>(Business.Configurations);

                for (int i = 0; i < confs.Length; i++)
                {
                    for (int j = 0; j < bizConfList.Count; j++)
                    {
                        if ((confs[i] != null) && (bizConfList[j] != null))
                        {
                            if (confs[i].Id.ToLower() == bizConfList[j].ID.ToLower())
                            {
                                confs[i].DbConnectionString = bizConfList[j].DbConnectionString;
                                confs[i].TypeId = ((int)(bizConfList[j].ConfigurationType));

                                bizConfList.RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }

                if (bizConfList.Count > 0)
                {
                    foreach (var conf in bizConfList)
                    {
                        if (conf != null)
                        {
                            Configuration configuration = new Configuration()
                            {
                                Business = biz,
                                Id = conf.ID,
                                BusinessId = biz.Id,
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

        Core.DomainModel.Business IBusinessManager.GetBusiness(string BusinessID)
        {
            Core.DomainModel.Business business = new Core.DomainModel.Business();

            using (DataModelContainer container = new DataModelContainer())
            {
                var biz = container.Businesses.First((o) => (o.Id.ToLower() == BusinessID.ToLower()));

                business.ID = biz.Id;
                business.Name = biz.Name;
                //customer.ReferenceID = cust.ReferenceId;

                if (!String.IsNullOrEmpty(biz.ReferenceId))
                {
                    business.ReferenceID = biz.ReferenceId.Split(new string[] { "," }, StringSplitOptions.None);
                }

                var confs = container.Configurations.Where((o) => (o.BusinessId.ToLower() == BusinessID.ToLower()));

                List<Core.DomainModel.Configuration> configurations = new List<Core.DomainModel.Configuration>();

                foreach (var conf in confs)
                {
                    configurations.Add(new Core.DomainModel.Configuration()
                    {
                        ID = conf.Id,
                        ConfigurationType = (ConfigurationType)(conf.TypeId),
                        DbConnectionString = conf.DbConnectionString
                    });
                }

                business.Configurations = configurations.ToArray();
            }

            return business;
        }

        Core.DomainModel.Configuration IBusinessManager.GetConfiguration(string ConfigurationID)
        {
            Core.DomainModel.Configuration configuration = new Core.DomainModel.Configuration();

            using (DataModelContainer container = new DataModelContainer())
            {
                var conf = container.Configurations.First((o) => (o.Id.ToLower() == ConfigurationID.ToLower()));

                configuration.ID = conf.Id;
                configuration.ConfigurationType = ((ConfigurationType)(conf.TypeId));
                configuration.DbConnectionString = conf.DbConnectionString;
            }

            return configuration;
        }

        Core.DomainModel.Configuration IBusinessManager.GetConfiguration(string BusinessID, ConfigurationType ConfigurationType)
        {
            Core.DomainModel.Configuration configuration = new Core.DomainModel.Configuration();

            using (DataModelContainer container = new DataModelContainer())
            {
                var conf = container.Configurations.First((o) => ((o.BusinessId.ToLower() == BusinessID.ToLower()) && (o.TypeId == (int)(ConfigurationType))));

                configuration.ID = conf.Id;
                configuration.ConfigurationType = ConfigurationType;
                configuration.DbConnectionString = conf.DbConnectionString;
            }

            return configuration;
        }

        Core.DomainModel.Business[] IBusinessManager.ListBusiness()
        {
            List<Core.DomainModel.Business> businesses = new List<Core.DomainModel.Business>();
            List<Core.DomainModel.Configuration> configurations = null;

            using (DataModelContainer container = new DataModelContainer())
            {
                Core.DomainModel.Business business = null;

                foreach (var biz in container.Businesses)
                {
                    business = new Core.DomainModel.Business()
                    {
                        ID = biz.Id,
                        Name = biz.Name,
                        //ReferenceID = cust.ReferenceId
                    };

                    if (!String.IsNullOrEmpty(biz.ReferenceId))
                    {
                        business.ReferenceID = biz.ReferenceId.Split(new string[] { "," }, StringSplitOptions.None);
                    }

                    var confs = container.Configurations.Where((o) => (o.BusinessId.ToLower() == biz.Id.ToLower())).ToArray();

                    if ((confs != null) && (confs.Length > 0))
                    {
                        configurations = new List<Core.DomainModel.Configuration>();

                        foreach (var conf in confs)
                        {
                            configurations.Add(new Core.DomainModel.Configuration()
                            {
                                ID = conf.Id,
                                ConfigurationType = ((ConfigurationType)(conf.TypeId)),
                                DbConnectionString = conf.DbConnectionString
                            });
                        }

                        business.Configurations = configurations.ToArray();
                    }

                    businesses.Add(business);
                }
            }

            return businesses.ToArray();
        }

        Core.DomainModel.Business[] IBusinessManager.ListBusiness(bool IsIncludingConfigurations)
        {
            List<Core.DomainModel.Business> businesses = new List<Core.DomainModel.Business>();
            List<Core.DomainModel.Configuration> configurations = null;

            using (DataModelContainer container = new DataModelContainer())
            {
                Core.DomainModel.Business business = null;

                foreach (var biz in container.Businesses)
                {
                    business = new Core.DomainModel.Business()
                    {
                        ID = biz.Id,
                        Name = biz.Name,
                        //ReferenceID = cust.ReferenceId
                    };

                    if (!String.IsNullOrEmpty(biz.ReferenceId))
                    {
                        business.ReferenceID = biz.ReferenceId.Split(new string[] { "," }, StringSplitOptions.None);
                    }

                    if (IsIncludingConfigurations)
                    {
                        var confs = container.Configurations.Where((o) => (o.BusinessId.ToLower() == biz.Id.ToLower())).ToArray();

                        if ((confs != null) && (confs.Length > 0))
                        {
                            configurations = new List<Core.DomainModel.Configuration>();

                            foreach (var conf in confs)
                            {
                                configurations.Add(new Core.DomainModel.Configuration()
                                {
                                    ID = conf.Id,
                                    ConfigurationType = ((ConfigurationType)(conf.TypeId)),
                                    DbConnectionString = conf.DbConnectionString
                                });
                            }

                            business.Configurations = configurations.ToArray();
                        }
                    }

                    businesses.Add(business);
                }
            }

            return businesses.ToArray();
        }
    }
}
