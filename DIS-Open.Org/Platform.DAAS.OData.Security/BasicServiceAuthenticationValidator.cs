using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using Platform.DAAS.OData.Security;

namespace Platform.DAAS.OData.Authentication
{
    public class BasicServiceAuthenticationValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            //if (!FormsAuthentication.Authenticate(userName, password))
            if(!new AuthManager().Authenticate(userName, password))
            {
                throw new SecurityTokenException("Authentication Failed!");
            }
        }
    }
}