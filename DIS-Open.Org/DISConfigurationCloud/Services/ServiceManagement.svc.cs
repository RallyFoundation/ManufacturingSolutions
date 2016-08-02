using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Platform.DAAS.OData.Core.Security;
using DataModel = Platform.DAAS.OData.Model;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.Security.Authorization;
using Platform.DAAS.OData.Core.ServiceManagement;
using Platform.DAAS.OData.Facade;

namespace ODataPlatform.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceManagement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceManagement.svc or ServiceManagement.svc.cs at the Solution Explorer and start debugging.
    public class ServiceManagement : IServiceManagement
    {
        IServiceManager serviceManager = Provider.ServiceManager();

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master })]
        public string AddApplication(Application Application)
        {
            return this.serviceManager.AddApplication(Application);
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master })]
        public string AddService(Service Service)
        {
            //Application[] applications = this.serviceManager.GetApplications();

            //if (applications.First((a)=> a.ID == Service.Application.ID) == null)
            //{
            //    return "0";
            //}

            return this.serviceManager.AddService(Service);
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master })]
        public Application[] GetApplications()
        {
            return this.serviceManager.GetApplications();
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master, RoleManager.SystemRole_Operator })]
        public Application[] GetMyApplications(string Owner)
        {
            return this.serviceManager.GetApplications(Owner);
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master, RoleManager.SystemRole_Operator })]
        public Service[] GetMyServices(string Subscriber)
        {
            return this.serviceManager.GetMyServices(Subscriber);
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master })]
        public ServiceConsumption[] GetServiceConsumptions(string ServiceID)
        {
            return this.serviceManager.GetServiceConsumptions(ServiceID);
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master })]
        public ServiceSubscription[] GetServiceSubscriptions(string ServiceID)
        {
            return this.serviceManager.GetServiceSubscriptions(ServiceID);
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master, RoleManager.SystemRole_Operator })]
        public string PublishService(string ServiceID)
        {
            Application[] applications = this.serviceManager.GetApplications();

            Service service = applications.First((a) => a.Services.First((s) => s.ID.ToString() == ServiceID) != null).Services.First((c)=> c.ID.ToString() == ServiceID);

            return this.serviceManager.PublishService(service);
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master })]
        public string PublishServices()
        {
            Application[] applications = this.serviceManager.GetApplications();

            string result = "";

            foreach (var application in applications)
            {
                foreach (var service in application.Services)
                {
                   result += this.serviceManager.PublishService(service);
                   result += ",";
                }
            }

            return result;
        }

        [Authorization(IsRequiringAuthentication = true, Roles = new string[] { RoleManager.SystemRole_SupperUser, RoleManager.SystemRole_Master, RoleManager.SystemRole_Operator })]
        public string SubscribeService(ServiceSubscription Subscription)
        {
            Application[] applications = this.serviceManager.GetApplications();

            if (applications.First((a) => a.ID == Subscription.Application.ID) == null)
            {
                return "0";
            }

            Subscription.CreationTime = DateTime.Now;

            return this.serviceManager.AddServiceSubscription(Subscription);
        }

        [Authorization(IsRequiringAuthentication = true)]
        public string Test()
        {
            return "Hello!";
        }

        [Authorization(IsRequiringAuthentication = true)]
        public string UpdateService(string ServiceID, Service Service)
        {
            Service.ID = ServiceID;

            return this.serviceManager.UpdateService(Service);
        }
    }
}
