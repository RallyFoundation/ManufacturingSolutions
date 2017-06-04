using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using System.IO;
//using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using Platform.DAAS.OData.Core.Security;
using Platform.DAAS.OData.Facade;
using Platform.DAAS.OData.Utility;
using Platform.DAAS.OData.Core.Logging;
using ResourceIntegrator;

namespace Platform.DAAS.OData.Security.Extension
{
    public class CasAuthorizeAttribute : AuthorizeAttribute, IAuthorizable
    {
        public string DataType
        {
            get; set;
        }

        public bool IsValidatingDataScope
        {
            get; set;
        }

        public string Operation
        {
            get; set;
        }

        public bool ShouldByPassSupperUser
        {
            get; set;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string ticket = filterContext.RequestContext.HttpContext.Request.QueryString.Get("ticket");

            HttpCookie casCookie = HttpContext.Current.Request.Cookies.Get("CASTicket");

            string userDataString = (casCookie != null) ? casCookie.Value : null;

            string[] userData = !String.IsNullOrEmpty(userDataString) ? userDataString.Split(new String[] { ":" }, StringSplitOptions.None) : null;

            string loginRedirectUrl = String.Format("{0}login?service={1}", ModuleConfiguration.DefaultCasUrl, ModuleConfiguration.DefaultCasAuthorizedUrl);

            string serviceValidationUrl = String.Format("{0}serviceValidate?ticket={1}&service={2}", ModuleConfiguration.DefaultCasUrl, ticket, ModuleConfiguration.DefaultCasAuthorizedUrl);

            ILogger logger = Provider.Logger();
            ITracer tracer = Provider.Tracer();

            if ((String.IsNullOrEmpty(ticket)) && (userData == null))//((String.IsNullOrEmpty(ticket)) && (!filterContext.HttpContext.User.Identity.IsAuthenticated))
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.Redirect(loginRedirectUrl, true);
                return;
            }
            else if ((!String.IsNullOrEmpty(ticket)) && (userData == null)) //((!String.IsNullOrEmpty(ticket)) && (!filterContext.HttpContext.User.Identity.IsAuthenticated))
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = (s, c, ch, p) => { return true; };

                //using (Stream stream = new WebClient().OpenRead(serviceValidationUrl))
                //{
                //    using (StreamReader reader = new StreamReader(stream))
                //    {
                //        result = reader.ReadToEnd();
                //    }
                //}

                bool enableTracing = Facade.Global.Should("EnableCASTracing");

                string traceSource = "CASTraceSource";

                ResourceRouter router = new ResourceRouter(enableTracing, traceSource);

                object responseData = router.Get(serviceValidationUrl, new ResourceIntegrator.Authentication() { Type = AuthenticationType.Custom }, new Dictionary<string, string>() { });

                string result = responseData != null ? responseData.ToString() : null;

                if (!String.IsNullOrEmpty(result))
                {
                    tracer.Trace(new object[] {serviceValidationUrl, result }, null);

                    try
                    {
                        ServiceResponse response = XmlUtility.XmlDeserialize(result, typeof(ServiceResponse), new Type[] { typeof(ServiceResponseAuthenticationSuccess) }, "UTF-8") as ServiceResponse;

                        if (response.SuccessItems != null && response.SuccessItems.Length > 0)
                        {
                            CasUserInfo user = new CasUserInfo()
                            {
                                UserId = response.SuccessItems[0].Attributes.UserId,
                                UserName = response.SuccessItems[0].User,
                                Email = response.SuccessItems[0].Attributes.Email,
                                NickName = response.SuccessItems[0].Attributes.NickName,
                                Phone = response.SuccessItems[0].Attributes.Phone,
                                UserType = response.SuccessItems[0].Attributes.UserType
                            };//String.IsNullOrEmpty(response.SuccessItems[0].Attributes) ? null : JsonUtility.JsonDeserialize(System.Text.Encoding.UTF8.GetBytes(response.SuccessItems[0].Attributes), typeof(CasUserInfo), new Type[] { typeof(CasUserInfo) }, "root") as CasUserInfo;

                            if (user == null)
                            {
                                filterContext.HttpContext.Response.StatusCode = 404;
                                return;
                            }

                            string webApiUrl = String.Format("{0}api/Account/Cas/Auth/", ModuleConfiguration.DefaultAccountServiceUrl);

                            byte[] userBytes = JsonUtility.JsonSerialize(user, new Type[] { typeof(CasUserInfo) }, "root");
                            string userJson = System.Text.Encoding.UTF8.GetString(userBytes);

                            object tokenResult = router.Post(webApiUrl, userJson, new ResourceIntegrator.Authentication() { Type = AuthenticationType.Custom }, new Dictionary<string, string>() { { "Content-Type", "application/json;charset=utf-8" }, { "Accept", "application/json" } });

                            BearerToken bearerToken = tokenResult == null ? null : JsonUtility.JsonDeserialize(System.Text.Encoding.UTF8.GetBytes(tokenResult.ToString()), typeof(BearerToken), new Type[] { typeof(BearerToken) }, "root") as BearerToken;

                            if (bearerToken == null)
                            {
                                filterContext.HttpContext.Response.StatusCode = 404;
                                return;
                            }

                            if (bearerToken != null)
                            {
                                ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>()
                                {
                                    new Claim(ClaimTypes.Name, response.SuccessItems[0].User),
                                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"),
                                    new Claim("AspNet.Identity.SecurityStamp", ticket)
                                }, "Cookies");

                                GenericPrincipal principal = new GenericPrincipal(identity, null);

                                HttpContext.Current.User = principal;

                                HttpContext.Current.Response.Cookies.Add(new HttpCookie("CASTicket", String.Format("{0}:{1}", principal.Identity.Name, ticket)));

                                HttpContext.Current.Response.Cookies.Add(new HttpCookie("CASAccountToken", String.Format("{0} {1}", bearerToken.TokenType, bearerToken.AccessToken)));

                                filterContext.HttpContext.Response.StatusCode = 200;
                            }
                        }

                        if (response.FailureItems != null && response.FailureItems.Length > 0)
                        {
                            foreach (var item in response.FailureItems)
                            {
                                logger.LogSystemError(item.Code, item.Value);
                            }
                        }  
                    }
                    catch (Exception ex)
                    {
                        Provider.ExceptionHandler().HandleException(ex);
                    }
                }
            }
            else if ((userData != null) && (userData.Length > 1))
            {
                ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, userData[0]),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"),
                    new Claim("AspNet.Identity.SecurityStamp", userData[1])
                }, "Cookies");

                GenericPrincipal principal = new GenericPrincipal(identity, null);
                HttpContext.Current.User = principal;
                filterContext.HttpContext.Response.StatusCode = 200;
            }

            //base.OnAuthorization(filterContext);
        }
    }
}
