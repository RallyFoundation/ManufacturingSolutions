using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DataModel = Platform.DAAS.OData.Model;
using Platform.DAAS.OData.Core.ServiceManagement;
using Platform.DAAS.OData.Facade;
using Platform.DAAS.OData.Core.DomainModel;

namespace ODataPlatform.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceManagement" in both code and config file together.
    [ServiceContract]
    public interface IServiceManagement
    {
        //[OperationContract]
        //void DoWork();

        [OperationContract]
        [WebGet(UriTemplate = "/Test/Hello/")]
        string Test();

        [OperationContract]
        [WebGet(UriTemplate = "/Application/All/")]
        Application[] GetApplications();

        [OperationContract]
        [WebGet(UriTemplate = "/Application/My/{Owner}/")]
        Application[] GetMyApplications(string Owner);

        [OperationContract]
        [WebGet(UriTemplate = "/Service/Subscription/{Subscriber}/")]
        Service[] GetMyServices(string Subscriber);

        [OperationContract]
        [WebGet(UriTemplate = "/Subscription/Service/{ServiceID}/")]
        ServiceSubscription[] GetServiceSubscriptions(string ServiceID);

        [OperationContract]
        [WebGet(UriTemplate = "/Consumption/Service/{ServiceID}/")]
        ServiceConsumption[] GetServiceConsumptions(string ServiceID);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Application/New/")]
        string AddApplication(Application Application);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Service/New/")]
        string AddService(Service Service);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/Service/{ServiceID}/")]
        string UpdateService(string ServiceID, Service Service);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Subscription/New/")]
        string SubscribeService(ServiceSubscription Subscription);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Service/Publish/{ServiceID}/")]
        string PublishService(string ServiceID);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Service/Publish/All/")]
        string PublishServices();
    }
}
