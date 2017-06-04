using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using Platform.DAAS.OData.Core.Security;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.Security.Authorization;
using Platform.DAAS.OData.Facade;

namespace ODataPlatform.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Auth" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Auth.svc or Auth.svc.cs at the Solution Explorer and start debugging.
    public class Auth : IAuth
    {
        //public void DoWork()
        //{
        //}

        //private IAuthManager authManager = Provider.AuthManager();
        private IEncryptionManager encryptionManager = Provider.EncryptionManager();

        public string Validate(AuthItem AuthItem)
        {
            string result = "";

            //if (this.authManager.Authenticate(AuthItem.Identity, AuthItem.Crypto))
            //{
            //    result = String.Format("{0}:{1}", AuthItem.Identity, AuthItem.Crypto);

            //    OperationContext context = OperationContext.Current;
            //    MessageProperties prop = context.IncomingMessageProperties;
            //    RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            //    string ip = endpoint.Address;

            //    string userAgent = System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.UserAgent;

            //    byte[] data = Encoding.Default.GetBytes(result);

            //    byte[] key = Encoding.Default.GetBytes(ip);

            //    byte[] iv = Encoding.Default.GetBytes(userAgent);

            //    byte[] encrytionBytes = this.encryptionManager.AesEncryption(data, key, iv);

            //    result = Encoding.Default.GetString(encrytionBytes);
            //}

            return result;
        }
    }
}
