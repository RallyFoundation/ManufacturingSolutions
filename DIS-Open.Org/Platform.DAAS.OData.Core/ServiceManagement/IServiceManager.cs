using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.ServiceManagement
{
    public interface IServiceManager
    {
        string AddApplication(Application Application);

        string UpdateApplication(Application Application);

        string AddService(Service Service);

        string UpdateService(Service Service);

        string AddServiceSubscription(ServiceSubscription Subscription);

        string UpdateServiceSubscription(ServiceSubscription Subscription);

        string AddServiceConsumption(ServiceConsumption Consumption);

        Application[] GetApplications();
        Application[] GetApplications(string Owner);

        Service[] GetServices(bool IsIncludingServiceMeta);
        Service[] GetServices(string ApplicationID);

        Service[] GetMyServices(string Subscriber);

        ServiceSubscription[] GetServiceSubscriptions(string ServiceID);

        ServiceConsumption[] GetServiceConsumptions(string ServiceID);

        //object[] GetEntityModels(Service Service, Application Application);

        object[] GetEntityModels(Service Service);

        string RegisterEntityModel(object Model, object Builder);

        string PublishService(Service service);
    }
}
