using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
//using ODataTest.Models;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Platform.DAAS.OData.Core.ServiceManagement;
using Platform.DAAS.OData.Facade;

namespace ODataPlatform 
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            //builder.EntitySet<Product>("Products");
            RegisterODataServiceModels(builder);

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                //routePrefix: null,
                routePrefix: Global.ODATA_RESOURCE_ROUTE_PREFIX,
                model: builder.GetEdmModel());
        }

        static void RegisterODataServiceModels(ODataModelBuilder builder)
        {
            IServiceManager serviceManager = Provider.ServiceManager();

            Application[] apps = serviceManager.GetApplications();

            object[] models = null;

            if (apps != null)
            {
                foreach (var app in apps)
                {
                    if (app != null)
                    {
                        foreach (var service in app.Services)
                        {
                            try
                            {
                                //models = serviceManager.GetEntityModels(service, app);

                                models = serviceManager.GetEntityModels(service);
                            }
                            catch (Exception ex)
                            {
                                models = null;
                            }

                            if (models != null)
                            {
                                foreach (var model in models)
                                {
                                    if (model is IODataEntityModel<ODataModelBuilder>)
                                    {
                                        //(model as IODataEntityModel<ODataModelBuilder>).Register(builder);

                                        //builder.AddEntitySet(model.GetType().Name + "s", builder.AddEntityType(model.GetType()));

                                        builder.AddEntitySet(model.GetType().Name, builder.AddEntityType(model.GetType()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
