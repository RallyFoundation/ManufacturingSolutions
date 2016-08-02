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
    public partial class Register : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            if (!Roles.IsUserInRole(RoleManager.SystemRole_SupperUser))
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            //FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

            //(new RoleManager()).SetUserRole(this.RegisterUser.UserName, RoleManager.SystemRole_ForbiddenUser);

            Provider.RoleManager().SetUserRole(this.RegisterUser.UserName, RoleManager.SystemRole_ForbiddenUser);

            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (String.IsNullOrEmpty(continueUrl))
            {
                //continueUrl = "~/";
                continueUrl = "~/Default.aspx";
            }

            Response.Redirect(continueUrl);
        }
    }
}