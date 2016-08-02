using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using Platform.DAAS.OData.Core.Security;
using Platform.DAAS.OData.Core.Logging;
using Platform.DAAS.OData.Core.ServiceManagement;

namespace Platform.DAAS.OData.Facade
{
    public class EnhancedODataModule : IHttpModule
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = ((HttpApplication)(sender));
            HttpContext context = application.Context;

            string requestPath = context.Request.Path;

            string resourceName = "";

            if (requestPath.ToLower().StartsWith( "/" + Global.ODATA_RESOURCE_ROUTE_PREFIX.ToLower()))
            {
                resourceName = requestPath.Replace(("/" + Global.ODATA_RESOURCE_ROUTE_PREFIX + "/"), "");

                if (resourceName.IndexOf("/") >= 0)
                {
                    resourceName = resourceName.Substring(0, resourceName.IndexOf("/"));
                }
                
                //if (resourceName.EndsWith("/"))
                //{
                //    resourceName = resourceName.Substring(0, resourceName.Length - 1);
                //}

                if (resourceName.IndexOf("(") > 0)
                {
                    resourceName = resourceName.Substring(0, resourceName.IndexOf("("));
                }

                if ((context.Request.Headers[Global.HTTP_HEADER_NAME_SERVICE_ID] != null) && (context.Request.Headers[Global.HTTP_HEADER_NAME_APP_ID] != null))
                {
                    string serviceID = context.Request.Headers[Global.HTTP_HEADER_NAME_SERVICE_ID];
                    string appID = context.Request.Headers[Global.HTTP_HEADER_NAME_APP_ID];

                    if (!Platform.DAAS.OData.ServiceManager.ModuleConfiguration.ServiceResourceNames.ContainsKey(serviceID))
                    {
                        context.Response.StatusCode = 406;

                        context.Response.Write("406: Not Acceptable");

                        context.Response.End();
                    }

                    if (context.Request.Headers[HttpRequestHeader.Authorization.ToString()] != null)
                    {
                        string token = context.Request.Headers[HttpRequestHeader.Authorization.ToString()];

                        string[] credential = token.Split(new string[] { ":" }, StringSplitOptions.None);

                        IAuthManager authManager = Provider.AuthManager();

                        //if (!authManager.Authenticate(credential[0], credential[1]))
                        if (!authManager.CheckAccessToken(credential[0], credential[1], new object[] { Provider.ServiceManager(), appID, serviceID }))
                        {
                            context.Response.StatusCode = 401;

                            context.Response.Write("401: Unauthorized");

                            context.Response.End();
                        }
                        else
                        {
                            if (Platform.DAAS.OData.ServiceManager.ModuleConfiguration.ServiceResourceNames[serviceID].ToLower() != resourceName.ToLower())
                            {
                                context.Response.StatusCode = 404;

                                context.Response.Write("404: Not Found");

                                context.Response.End();
                            }

                            if (!Platform.DAAS.OData.ServiceManager.ModuleConfiguration.ServiceDBConnectionStrings.ContainsKey(serviceID))
                            {
                                context.Response.StatusCode = 406;

                                context.Response.Write("406: Not Acceptable");

                                context.Response.End();
                            }
                            else
                            {
                                context.Request.Headers.Add(Global.HTTP_HEADER_NAME_DB_CONNECTION, Platform.DAAS.OData.ServiceManager.ModuleConfiguration.ServiceDBConnectionStrings[serviceID]);

                                context.Request.Headers.Add(Global.HTTP_HEADER_NAME_RESOURCE_NAME, Platform.DAAS.OData.ServiceManager.ModuleConfiguration.ServiceResourceNames[serviceID]);

                                Provider.Logger().LogServiceOperation(credential[0], String.Format("Url:{0}; User ID:{1}; Service ID: {2}; Resource Name: {3}; Timestamp: {4}; Url Referrer: {5}; User Agent: {6}; User Host Address: {7}; User Host Name: {8}; User Languages: {9}", context.Request.Url, credential[0], serviceID, resourceName, DateTime.Now, context.Request.UrlReferrer, context.Request.UserAgent, context.Request.UserHostAddress, context.Request.UserHostName, context.Request.UserLanguages));
                            }
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401;

                        context.Response.Write("401: Unauthorized");

                        context.Response.End();
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;

                    context.Response.Write("401: Unauthorized");

                    context.Response.End();
                }
            }
        }
    }
}
