using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.Facade;

namespace ODataPlatform.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string userName = this.LoginUser.UserName;
            string password = this.LoginUser.Password;

            if (Roles.IsUserInRole(userName, RoleManager.SystemRole_ForbiddenUser))
            {
                e.Authenticated = false;
            }
            else if (Provider.AuthManager().Authenticate(userName, password))
            {
                e.Authenticated = true;

                Provider.Logger().LogUserOperation(userName, String.Format("User {0} logged in : {1}", userName, DateTime.Now));
            }

            //if (Roles.IsUserInRole(userName, RoleManager.SystemRole_ForbiddenUser))
            //{
            //    e.Authenticated = false;
            //}
            //else if (Membership.ValidateUser(userName, password))
            //{
            //    e.Authenticated = true;
            //}

            //if (Membership.ValidateUser(userName, password))
            //{
            //    e.Authenticated = true;
            //}
        }
    }
}