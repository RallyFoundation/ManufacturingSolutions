using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using DISConfigurationCloud.MetaManagement;

namespace DISConfigurationCloud.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDISConfigurationCloud" in both code and config file together.
    [ServiceContract]
    public interface IDISConfigurationCloud
    {
        //[OperationContract]
        //[WebInvoke]
        //void DoWork();

        [OperationContract]
        [WebGet(UriTemplate = "/Test/")]
        string Test();

        [OperationContract]
        [WebInvoke(Method="POST",UriTemplate="/Customer/New/")]
        string AddCustomer(Customer Customer);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/Customer/{CustomerID}/")]
        string UpdateCustomer(string CustomerID, Customer Customer);

        [OperationContract]
        [WebGet(UriTemplate = "/Customer/{CustomerID}/" )]
        Customer GetCustomer(string CustomerID);

        [OperationContract]
        [WebGet(UriTemplate = "/Customer/All/")]
        Customer[] GetCustomers();

        [OperationContract(Name="GetCustomerConfiguration")]
        [WebGet(UriTemplate = "/Customer/{CustomerID}/Configuration/{ConfigurationType}/")]
        Configuration GetCustomerConfiguration(string CustomerID, string ConfigurationType);

        [OperationContract]
        [WebGet(UriTemplate = "/Configuration/{ConfigurationID}/")]
        Configuration GetConfiguration(string ConfigurationID);
        
    }
}
