using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Claims;
using System.Security.Principal;

namespace Platform.DAAS.OData.Security.Extension
{
    public class CasAuthModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.LogRequest += new EventHandler(OnLogRequest);
            context.BeginRequest += Context_BeginRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            HttpApplication application = ((HttpApplication)(sender));
            HttpContext context = application.Context;

            HttpCookie casCookie = context.Request.Cookies.Get("CASTicket");

            string userDataString = (casCookie != null) ? casCookie.Value : null;

            string[] userData = !String.IsNullOrEmpty(userDataString) ? userDataString.Split(new String[] { ":" }, StringSplitOptions.None) : null;

            if ((userData != null) && (userData.Length > 1))
            {
                ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, userData[0]),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"),
                    new Claim("AspNet.Identity.SecurityStamp", userData[1])
                }, "Cookies");

                GenericPrincipal principal = new GenericPrincipal(identity, null);
                HttpContext.Current.User = principal;
            }
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }
    }
}
