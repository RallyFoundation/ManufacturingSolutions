using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.OData.Builder;
using DataModel = Platform.DAAS.OData.Model;
using Platform.DAAS.OData.Core.ServiceManagement;

namespace Platform.DAAS.OData.ServiceManager
{
    public class ServieManager : IServiceManager
    {
        public string AddApplication(Application Application)
        {
            Application.ID = Guid.NewGuid().ToString();

            DataModel.Application model = new DataModel.Application()
            {
                ID = Application.ID,
                Name = Application.Name,
                Description = Application.Description,
                Owner = Application.Owner,
                Status = Application.Status
            };

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                model = container.Applications.Add(model);
                container.SaveChanges();
            }

            return model.ID.ToString();
        }

        public string AddService(Service Service)
        {
            Service.ID = Guid.NewGuid().ToString();

            DataModel.Service model = new DataModel.Service()
            {
                ID = Service.ID,
                Name = Service.Name,
                ResourceName = Service.ServiceMeta.ResourceName,
                Description = Service.Description,
                ApplicationID = Service.Application.ID,
                Binary = Service.ServiceMeta.ServiceBinary,
                DBConnectionString = Service.ServiceMeta.DBConnectionString,
                Parameters = Service.ServiceMeta.Parameters,
                DBType = (int)(Service.ServiceMeta.PersistencyType),
                ServiceType = (int)(Service.ServiceType),
                ModelMeta = Service.ServiceMeta.ModelMeta,
                ServiceCode = Service.ServiceMeta.ServiceCode,
                Address = Service.ServiceMeta.Address,
                Port = Service.ServiceMeta.Port.ToString(),
                UserName = Service.ServiceMeta.UserName,
                Password = Service.ServiceMeta.Password,
                DomainName = Service.ServiceMeta.DomainName,
                ContentType = Service.ServiceMeta.ContentType,
                Charset = Service.ServiceMeta.Charset,
                Url = Service.ServiceMeta.Url,
                Size = Service.ServiceMeta.Size,
                Status = Service.Status,
                Version = Service.Version,
                CreationTime = DateTime.Now  
            };

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                model = container.Services.Add(model);

                container.SaveChanges();
            }

            return model.ID.ToString();
        }

        public string AddServiceConsumption(ServiceConsumption Consumption)
        {
            Consumption.ID = Guid.NewGuid().ToString();

            DataModel.ServiceConsumption model = new DataModel.ServiceConsumption()
            {
                ID = Consumption.ID,
                ServiceID = Consumption.Service.ID,
                Consumer = Consumption.Consumer,
                UrlReferrer = Consumption.UrlReferrer,
                Result = Consumption.Result,
                CreationTime = DateTime.Now
            };

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                model = container.ServiceConsumptions.Add(model);

                container.SaveChanges();
            }

            return model.ID;
        }

        public string AddServiceSubscription(ServiceSubscription Subscription)
        {
            Subscription.ID = Guid.NewGuid().ToString();

            DataModel.ServiceSubscription model = new Model.ServiceSubscription()
            {
                ID = Subscription.ID,
                ServiceID = Subscription.Service.ID,
                Subscriber = Subscription.Subscriber,
                Status = Subscription.Status,
                CreationTime = DateTime.Now
            };

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                model = container.ServiceSubscriptions.Add(model);

                container.SaveChanges();
            }

            return model.ID.ToString();
        }

        public Application[] GetApplications()
        {
            List<Application> applications = new List<Application>();

            Application application = null;

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                var applicationModels = container.Applications;

                if (applicationModels != null)
                {
                    foreach (var applicationModel in applicationModels.ToArray())
                    {
                        application = new Application()
                        {
                            ID = applicationModel.ID.ToString(),
                            Name = applicationModel.Name,
                            Description = applicationModel.Description,
                            Owner = applicationModel.Owner,
                            Status = applicationModel.Status
                        };

                        if (applicationModel.Services != null)
                        {
                            application.Services = new List<Service>();

                            foreach (var serviceModel in applicationModel.Services)
                            {
                                application.Services.Add(new Service()
                                {
                                    ID = serviceModel.ID.ToString(),
                                    //Binary = serviceModel.Binary,
                                    //DBConnectionString = serviceModel.DBConnectionString,
                                    //DBType = (DBType)(Enum.Parse(typeof(DBType), serviceModel.DBType)),
                                    Description = serviceModel.Description,
                                    Name = serviceModel.Name,
                                    //ResourceName = serviceModel.ResourceName,
                                    //Url = serviceModel.Url,
                                    //ModelMeta = serviceModel.ModelMeta,
                                    //ServiceMeta = serviceModel.ServiceMeta, 

                                    ServiceMeta = new ServiceMeta()
                                    {
                                        DBConnectionString = serviceModel.DBConnectionString,
                                        PersistencyType = (PersistencyType)(serviceModel.DBType),
                                        ResourceName = serviceModel.ResourceName,
                                        ModelMeta = serviceModel.ModelMeta,
                                        ServiceCode = serviceModel.ServiceCode,
                                        ServiceBinary = serviceModel.Binary,
                                        Url = serviceModel.Url,
                                        Address = serviceModel.Address,
                                        Port = int.Parse(serviceModel.Port),
                                        UserName = serviceModel.UserName,
                                        Password = serviceModel.Password,
                                        Charset = serviceModel.Charset,
                                        ContentType = serviceModel.ContentType,
                                        DomainName = serviceModel.DomainName,
                                        Parameters = serviceModel.Parameters,
                                        Size = serviceModel.Size.Value
                                    },

                                    Status = serviceModel.Status,
                                    Version = serviceModel.Version,
                                    Application = new Application()
                                    {
                                        ID = applicationModel.ID.ToString(),
                                        Name = applicationModel.Name
                                    }
                                });
                            }
                        }

                        applications.Add(application);
                    }
                }
            }

            return applications.ToArray();
        }

        public Application[] GetApplications(string Owner)
        {
            List<Application> applications = new List<Application>();

            Application application = null;

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                var applicationModels = container.Applications.First(a => a.Owner.ToLower() == Owner.ToLower()) != null ? container.Applications.Where(a => a.Owner.ToLower() == Owner.ToLower()).ToArray() : null;

                if (applicationModels != null)
                {
                    foreach (var applicationModel in applicationModels)
                    {
                        application = new Application()
                        {
                            ID = applicationModel.ID.ToString(),
                            Name = applicationModel.Name,
                            Description = applicationModel.Description,
                            Owner = applicationModel.Owner,
                            Status = applicationModel.Status
                        };

                        if (applicationModel.Services != null)
                        {
                            application.Services = new List<Service>();

                            foreach (var serviceModel in applicationModel.Services)
                            {
                                application.Services.Add(new Service()
                                {
                                    ID = serviceModel.ID.ToString(),
                                    //Binary = serviceModel.Binary,
                                    //DBConnectionString = serviceModel.DBConnectionString,
                                    //DBType = (DBType)(Enum.Parse(typeof(DBType), serviceModel.DBType)),
                                    Description = serviceModel.Description,
                                    Name = serviceModel.Name,
                                    //ResourceName = serviceModel.ResourceName,
                                    //Url = serviceModel.Url,
                                    //ModelMeta = serviceModel.ModelMeta,
                                    //ServiceMeta = serviceModel.ServiceMeta,

                                    ServiceMeta = new ServiceMeta()
                                    {
                                        DBConnectionString = serviceModel.DBConnectionString,
                                        PersistencyType = (PersistencyType)(serviceModel.DBType),
                                        ResourceName = serviceModel.ResourceName,
                                        ModelMeta = serviceModel.ModelMeta,
                                        ServiceCode = serviceModel.ServiceCode,
                                        ServiceBinary = serviceModel.Binary,
                                        Url = serviceModel.Url,
                                        Address = serviceModel.Address,
                                        Port = int.Parse(serviceModel.Port),
                                        UserName = serviceModel.UserName,
                                        Password = serviceModel.Password,
                                        Charset = serviceModel.Charset,
                                        ContentType = serviceModel.ContentType,
                                        DomainName = serviceModel.DomainName,
                                        Parameters = serviceModel.Parameters,
                                        Size = serviceModel.Size.Value
                                    },

                                    Status = serviceModel.Status,
                                    Version = serviceModel.Version,
                                    Application = new Application()
                                    {
                                        ID = applicationModel.ID.ToString(),
                                        Name = applicationModel.Name
                                    }
                                });
                            }
                        }

                        applications.Add(application);
                    }
                }
            }

            return applications.ToArray();
        }

        public Service[] GetMyServices(string Subscriber)
        {
            List<Service> services = null;

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                var subscriptionModels = container.ServiceSubscriptions.Where((ss) => ss.Subscriber.ToLower() == Subscriber.ToLower()).ToArray();

                if (subscriptionModels != null)
                {
                    services = new List<Service>();

                    foreach (var subscriptionModel in subscriptionModels)
                    {
                        if (subscriptionModel.Service != null)
                        {
                            services.Add(new Service()
                            {
                                Application = new Application()
                                {
                                    ID = subscriptionModel.Service.ApplicationID.ToString(),
                                    Name = subscriptionModel.Service.Application.Name,
                                    Owner = subscriptionModel.Service.Application.Owner,
                                    Description = subscriptionModel.Service.Application.Description,
                                    Status = subscriptionModel.Service.Application.Status
                                },
                                ID = subscriptionModel.Service.ID.ToString(),
                                //Binary = subscriptionModel.Service.Binary,
                                //DBConnectionString = subscriptionModel.Service.DBConnectionString,
                                //DBType = (DBType)(Enum.Parse(typeof(DBType), subscriptionModel.Service.DBType)),
                                Description = subscriptionModel.Service.Description,
                                Name = subscriptionModel.Service.Name,
                                //ResourceName = subscriptionModel.Service.ResourceName,
                                //Url = subscriptionModel.Service.Url,
                                //ModelMeta = subscriptionModel.Service.ModelMeta,
                                //ServiceMeta = subscriptionModel.Service.ServiceMeta,

                                ServiceMeta = new ServiceMeta()
                                {
                                    DBConnectionString = subscriptionModel.Service.DBConnectionString,
                                    PersistencyType = (PersistencyType)(subscriptionModel.Service.DBType),
                                    ResourceName = subscriptionModel.Service.ResourceName,
                                    ModelMeta = subscriptionModel.Service.ModelMeta,
                                    ServiceCode = subscriptionModel.Service.ServiceCode,
                                    ServiceBinary = subscriptionModel.Service.Binary,
                                    Url = subscriptionModel.Service.Url,
                                    Address = subscriptionModel.Service.Address,
                                    Port = int.Parse(subscriptionModel.Service.Port),
                                    UserName = subscriptionModel.Service.UserName,
                                    Password = subscriptionModel.Service.Password,
                                    Charset = subscriptionModel.Service.Charset,
                                    ContentType = subscriptionModel.Service.ContentType,
                                    DomainName = subscriptionModel.Service.DomainName,
                                    Parameters = subscriptionModel.Service.Parameters,
                                    Size = subscriptionModel.Service.Size.Value
                                },

                                Status = subscriptionModel.Service.Status,
                                Version = subscriptionModel.Service.Version
                            });
                        }
                    }
                }
            }

            return services != null ? services.ToArray() : null;
        }

        public Service[] GetServices(string ApplicationID)
        {
            List<Service> services = null;

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                var serviceModels = container.Services.Where((ss) => ss.ApplicationID.ToString() == ApplicationID).ToArray();

                if (serviceModels != null)
                {
                    services = new List<Service>();

                    foreach (var serviceModel in serviceModels)
                    {
                        if (serviceModel != null)
                        {
                            services.Add(new Service()
                            {
                                Application = new Application()
                                {
                                    ID = serviceModel.ApplicationID.ToString(),
                                    Name = serviceModel.Application.Name,
                                    Owner = serviceModel.Application.Owner,
                                    Description = serviceModel.Application.Description,
                                    Status = serviceModel.Application.Status
                                },
                                ID = serviceModel.ID.ToString(),
                                //Binary = subscriptionModel.Service.Binary,
                                //DBConnectionString = subscriptionModel.Service.DBConnectionString,
                                //DBType = (DBType)(Enum.Parse(typeof(DBType), subscriptionModel.Service.DBType)),
                                Description = serviceModel.Description,
                                Name = serviceModel.Name,
                                //ResourceName = subscriptionModel.Service.ResourceName,
                                //Url = subscriptionModel.Service.Url,
                                //ModelMeta = subscriptionModel.Service.ModelMeta,
                                //ServiceMeta = subscriptionModel.Service.ServiceMeta,

                                ServiceMeta = new ServiceMeta()
                                {
                                    DBConnectionString = serviceModel.DBConnectionString,
                                    PersistencyType = (PersistencyType)(serviceModel.DBType),
                                    ResourceName = serviceModel.ResourceName,
                                    ModelMeta = serviceModel.ModelMeta,
                                    ServiceCode = serviceModel.ServiceCode,
                                    ServiceBinary = serviceModel.Binary,
                                    Url = serviceModel.Url,
                                    Address = serviceModel.Address,
                                    Port = int.Parse(serviceModel.Port),
                                    UserName = serviceModel.UserName,
                                    Password = serviceModel.Password,
                                    Charset = serviceModel.Charset,
                                    ContentType = serviceModel.ContentType,
                                    DomainName = serviceModel.DomainName,
                                    Parameters = serviceModel.Parameters,
                                    Size = serviceModel.Size.Value
                                },

                                Status = serviceModel.Status,
                                Version = serviceModel.Version
                            });
                        }
                    }
                }
            }

            return services != null ? services.ToArray() : null;
        }

        public string PublishService(Service Service)
        {
            throw new NotImplementedException();
        }

        public object[] GetEntityModels(Service Service)
        {
            string assemblyName = String.Format("DAAS.{0}.{1}.{2}.dll", Service.Application.Name, Service.Name, Service.ServiceMeta.ResourceName);

            string assemblyPath = ModuleConfiguration.Default_Model_Assembly_Path + assemblyName; //String.Format("{0}{1}\\{2}\\{3}", ModuleConfiguration.Default_Model_Assembly_Path, Service.Application.Name, Service.Name, assemblyName); //String.Format("{0}{1}\\{2}\\{3}", ModuleConfiguration.Default_Model_Assembly_Path, Service.Application.Name, Service.Name, assemblyName); //AppDomain.CurrentDomain.BaseDirectory + ModuleConfiguration.Default_Model_Assembly_Path + assemblyName;

            if (System.IO.File.Exists(assemblyPath))
            {
                //AssemblyName assemblyRef = new AssemblyName(assembly.FullName);

                //Assembly assembly = AppDomain.CurrentDomain.Load(assemblyRef);

                Assembly assembly = Assembly.LoadFrom(assemblyPath);

                //byte[] assemblyBytes = null;

                //using (System.IO.FileStream stream = new System.IO.FileStream(assemblyPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                //{
                //    assemblyBytes = new byte[stream.Length];

                //    stream.Read(assemblyBytes, 0, ((int)(stream.Length)));
                //}

                //Assembly assembly = AppDomain.CurrentDomain.Load(assemblyBytes);

                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();

                    //ODataEntityModel entityModel = null;

                    IODataEntityModel<ODataModelBuilder> entityModel = null;

                    List<object> entityModels = new List<object>();

                    foreach (var type in types)
                    {
                        var interfaces = type.GetInterfaces();

                        if (interfaces != null)
                        {
                            foreach (var interfaceType in interfaces)
                            {
                                if (interfaceType == typeof(IODataEntityModel<ODataModelBuilder>))//if (type.BaseType == typeof(ODataEntityModel))
                                {
                                    //entityModel = Activator.CreateInstance(type) as ODataEntityModel;

                                    entityModel = Activator.CreateInstance(type) as IODataEntityModel<ODataModelBuilder>;

                                    entityModels.Add(entityModel);
                                }
                            }
                        }
                    }

                    return entityModels.ToArray();
                }
            }

            return null;
        }

        public string RegisterEntityModel(object Model, object Builder)
        {
            if ((Model is IODataEntityModel<ODataModelBuilder>) && (Builder is ODataModelBuilder))
            {
                (Model as IODataEntityModel<ODataModelBuilder>).Register(Builder as ODataModelBuilder);

                return "1";
            }

            return "0";
        }

        public ServiceSubscription[] GetServiceSubscriptions(string ServiceID)
        {
            List<ServiceSubscription> serivceSubscriptions = new List<ServiceSubscription>();

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                var serviceSubscriptionModels = container.ServiceSubscriptions.Where(ss => ss.ServiceID.ToString() == ServiceID);

                if (serviceSubscriptionModels != null)
                {
                    foreach (var serviceSubscriptionModel in serviceSubscriptionModels.ToArray())
                    {
                        serivceSubscriptions.Add(new ServiceSubscription()
                        {
                            CreationTime = serviceSubscriptionModel.CreationTime,
                            ID = serviceSubscriptionModel.ID.ToString(),
                            Status = serviceSubscriptionModel.Status,
                            Subscriber = serviceSubscriptionModel.Subscriber,
                            Service = new Service()
                            {
                                ID = serviceSubscriptionModel.Service.ID.ToString(),
                                Name = serviceSubscriptionModel.Service.Name,
                                Description = serviceSubscriptionModel.Service.Description,
                                //ResourceName = serviceSubscriptionModel.Service.ResourceName,
                                //Url = serviceSubscriptionModel.Service.Url,
                                //DBType = ((DBType)(Enum.Parse(typeof(DBType), serviceSubscriptionModel.Service.DBType))),

                                ServiceMeta = new ServiceMeta()
                                {
                                    //DBConnectionString = serviceSubscriptionModel.Service.DBConnectionString,
                                    PersistencyType = (PersistencyType)(serviceSubscriptionModel.Service.DBType),
                                    ResourceName = serviceSubscriptionModel.Service.ResourceName,
                                    //ModelMeta = serviceSubscriptionModel.Service.ModelMeta,
                                    //ServiceCode = serviceSubscriptionModel.Service.ServiceCode,
                                    //ServiceBinary = serviceSubscriptionModel.Service.Binary,
                                    Url = serviceSubscriptionModel.Service.Url,
                                    //Address = serviceSubscriptionModel.Service.Address,
                                    //Port = int.Parse(serviceSubscriptionModel.Service.Port),
                                    //UserName = serviceSubscriptionModel.Service.UserName,
                                    //Password = serviceSubscriptionModel.Service.Password,
                                    //Charset = serviceSubscriptionModel.Service.Charset,
                                    //ContentType = serviceSubscriptionModel.Service.ContentType,
                                    //DomainName = serviceSubscriptionModel.Service.DomainName,
                                    //Parameters = serviceSubscriptionModel.Service.Parameters,
                                    //Size = serviceSubscriptionModel.Service.Size.Value
                                },

                                Status = serviceSubscriptionModel.Service.Status,
                                Version = serviceSubscriptionModel.Service.Version,
                                Application = new Application()
                                {
                                    ID = serviceSubscriptionModel.Service.Application.ID.ToString(),
                                    Name = serviceSubscriptionModel.Service.Application.Name,
                                    Description = serviceSubscriptionModel.Service.Application.Description,
                                    Owner = serviceSubscriptionModel.Service.Application.Owner,
                                    Status = serviceSubscriptionModel.Service.Application.Status
                                }
                            }
                        });
                    }
                }
            }

            return serivceSubscriptions.ToArray();
        }

        public ServiceConsumption[] GetServiceConsumptions(string ServiceID)
        {
            List<ServiceConsumption> serivceConsumptions = new List<ServiceConsumption>();

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                var serviceConsumptionModels = container.ServiceConsumptions.Where(sc => sc.ServiceID.ToString() == ServiceID);

                if (serviceConsumptionModels != null)
                {
                    foreach (var serviceConsumptionModel in serviceConsumptionModels.ToArray())
                    {
                        serivceConsumptions.Add(new ServiceConsumption()
                        {
                            ID = serviceConsumptionModel.ID.ToString(),
                            Consumer = serviceConsumptionModel.Consumer,
                            Result = serviceConsumptionModel.Result,
                            UrlReferrer = serviceConsumptionModel.UrlReferrer,
                            CreationTime = serviceConsumptionModel.CreationTime,
                            Service = new Service()
                            {
                                ID = serviceConsumptionModel.Service.ID.ToString(),
                                Name = serviceConsumptionModel.Service.Name,
                                Description = serviceConsumptionModel.Service.Description,
                                //ResourceName = serviceConsumptionModel.Service.ResourceName,
                                //Url = serviceConsumptionModel.Service.Url,
                                //DBType = ((DBType)(Enum.Parse(typeof(DBType), serviceConsumptionModel.Service.DBType))),

                                ServiceMeta = new ServiceMeta()
                                {
                                    ResourceName = serviceConsumptionModel.Service.ResourceName,
                                    Url = serviceConsumptionModel.Service.Url,
                                    PersistencyType = (PersistencyType)(serviceConsumptionModel.Service.DBType)
                                },

                                Status = serviceConsumptionModel.Service.Status,
                                Version = serviceConsumptionModel.Service.Version,
                                Application = new Application()
                                {
                                    ID = serviceConsumptionModel.Service.Application.ID.ToString(),
                                    Name = serviceConsumptionModel.Service.Application.Name,
                                    Description = serviceConsumptionModel.Service.Application.Description,
                                    Owner = serviceConsumptionModel.Service.Application.Owner,
                                    Status = serviceConsumptionModel.Service.Application.Status
                                }
                            }
                        });
                    }
                }
            }

            return serivceConsumptions.ToArray();
        }

        public Service[] GetServices(bool IsIncludingServiceMeta)
        {
            List<Service> services = null;

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                var serviceModels = container.Services;

                if (serviceModels != null)
                {
                    services = new List<Service>();

                    foreach (var serviceModel in serviceModels)
                    {
                        if (serviceModel != null)
                        {
                            services.Add(new Service()
                            {
                                Application = new Application()
                                {
                                    ID = serviceModel.ApplicationID.ToString(),
                                    Name = serviceModel.Application.Name,
                                    Owner = serviceModel.Application.Owner,
                                    Description = serviceModel.Application.Description,
                                    Status = serviceModel.Application.Status
                                },

                                ID = serviceModel.ID.ToString(),
                                Description = serviceModel.Description,
                                Name = serviceModel.Name,

                                ServiceMeta = !IsIncludingServiceMeta ? null : new ServiceMeta()
                                {
                                    DBConnectionString = serviceModel.DBConnectionString,
                                    PersistencyType = (PersistencyType)(serviceModel.DBType),
                                    ResourceName = serviceModel.ResourceName,
                                    ModelMeta = serviceModel.ModelMeta,
                                    ServiceCode = serviceModel.ServiceCode,
                                    ServiceBinary = serviceModel.Binary,
                                    Url = serviceModel.Url,
                                    Address = serviceModel.Address,
                                    Port = int.Parse(serviceModel.Port),
                                    UserName = serviceModel.UserName,
                                    Password = serviceModel.Password,
                                    Charset = serviceModel.Charset,
                                    ContentType = serviceModel.ContentType,
                                    DomainName = serviceModel.DomainName,
                                    Parameters = serviceModel.Parameters,
                                    Size = serviceModel.Size.Value
                                },

                                Status = serviceModel.Status,
                                Version = serviceModel.Version
                            });
                        }
                    }
                }
            }

            return services != null ? services.ToArray() : null;
        }

        public string UpdateApplication(Application Application)
        {
            int result = -9;

            using (DataModel.DataModelContainer container = new Model.DataModelContainer())
            {
                var model = container.Applications.FirstOrDefault(a => a.ID.ToLower() == Application.ID.ToLower());

                model.Description = Application.Description;
                model.Owner = Application.Owner;
                model.Status = Application.Status;

                result = container.SaveChanges();
            }

            return result.ToString();
        }

        public string UpdateService(Service Service)
        {
            int result = -9;

            using (DataModel.DataModelContainer container = new Model.DataModelContainer())
            {
                var model = container.Services.FirstOrDefault(s => s.ID.ToLower() == Service.ID.ToLower());

                
                model.Address = Service.ServiceMeta.Address;
                model.ApplicationID = Service.Application.ID;
                //model.Binary = Service.ServiceMeta.ServiceBinary;
                model.Charset = Service.ServiceMeta.Charset;
                model.ContentType = Service.ServiceMeta.ContentType;
                model.DBConnectionString = Service.ServiceMeta.DBConnectionString;
                model.DBType = (int)(Service.ServiceMeta.PersistencyType);
                model.Description = Service.Description;
                model.DomainName = Service.ServiceMeta.DomainName;
                //model.ModelMeta = Service.ServiceMeta.ModelMeta;
                model.Parameters = Service.ServiceMeta.Parameters;
                model.Password = Service.ServiceMeta.Password;
                model.Port = Service.ServiceMeta.Port.ToString();
                model.ResourceName = Service.ServiceMeta.ResourceName;
                //model.ServiceCode = Service.ServiceMeta.ServiceCode;
                model.Url = Service.ServiceMeta.Url;
                model.UserName = Service.ServiceMeta.UserName;
                model.Status = Service.Status;
                model.Version = Service.Version;

                result = container.SaveChanges();
            }

            return result.ToString();
        }

        public string UpdateServiceSubscription(ServiceSubscription Subscription)
        {
            int result = -9;

            using (DataModel.DataModelContainer container = new DataModel.DataModelContainer())
            {
                var model = container.ServiceSubscriptions.FirstOrDefault(ss => ss.ID.ToLower() == Subscription.ID.ToLower());

                model.ServiceID = Subscription.Service.ID;
                model.Status = Subscription.Status;
                model.Subscriber = Subscription.Subscriber;

                container.SaveChanges();
            }

            return result.ToString();
        }

        //public object[] GetEntityModels(Service Service, Application Application)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
