using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using Platform.DAAS.OData.Core.Security;

namespace ODataPlatform.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuth" in both code and config file together.
    [ServiceContract]
    public interface IAuth
    {
        //[OperationContract]
        //void DoWork();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Identity/Validate/")]
        string Validate(AuthItem AuthItem);
    }
}
