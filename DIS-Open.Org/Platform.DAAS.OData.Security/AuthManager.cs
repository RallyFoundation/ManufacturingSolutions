using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Platform.DAAS.OData.Core.Security;
using Platform.DAAS.OData.Core.ServiceManagement;

namespace Platform.DAAS.OData.Security
{
    public class AuthManager : IAuthManager
    {
        public bool Authenticate(string UserName, string Password)
        {
            return Membership.ValidateUser(UserName, Password);
        }

        public bool CheckAccessToken(string TokenPrefix, string TokenValue, object[] Arguments)
        {
            switch (TokenPrefix.ToLower())
            {
                case "simple":
                    {
                        string systemToken = System.Configuration.ConfigurationManager.AppSettings.Get("accessToken");

                        return (TokenValue.ToLower() == systemToken.ToLower());
                    }
                case "daas":
                    {
                        if ((Arguments != null) && (Arguments.Length == 3) && (Arguments[0] != null) && (Arguments[0] is IServiceManager))
                        {
                            IServiceManager serviceManager = (Arguments[0] as IServiceManager);

                            string applicationID = (string)Arguments[1], serviceID = (string)Arguments[2];

                            ServiceSubscription[] serviceSubscriptions = serviceManager.GetServiceSubscriptions(serviceID);

                            return (serviceSubscriptions.FirstOrDefault(ss => ss.Application.ID.ToLower() == applicationID.ToLower() && ss.Token.ToLower() == TokenValue.ToLower()) != null);
                        }

                        break;
                    }
                default:
                    break;
            }

            return false;
        }
    }
}
