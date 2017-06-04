using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.DynamicData;
using Platform.DAAS.OData.Model;

namespace ODataPlatform
{
    public class RouteConfig
    {
        private static MetaModel s_defaultModel = new MetaModel();
        public static MetaModel DefaultModel
        {
            get
            {
                return s_defaultModel;
            }
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            //                    IMPORTANT: DATA MODEL REGISTRATION 
            // Uncomment this line to register an ADO.NET Entity Framework model for ASP.NET Dynamic Data.
            // Set ScaffoldAllTables = true only if you are sure that you want all tables in the
            // data model to support a scaffold (i.e. templates) view. To control scaffolding for
            // individual tables, create a partial class for the table and apply the
            // [ScaffoldTable(true)] attribute to the partial class.
            // Note: Make sure that you change "YourDataContextType" to the name of the data context
            // class in your application.
            // See http://go.microsoft.com/fwlink/?LinkId=257395 for more information on how to register Entity Data Model with Dynamic Data            
            // DefaultModel.RegisterContext(() =>
            // {
            //    return ((IObjectContextAdapter)new YourDataContextType()).ObjectContext;
            // }, new ContextConfiguration() { ScaffoldAllTables = false });

            DefaultModel.RegisterContext
            (
                () =>
                {
                    return ((System.Data.Entity.Infrastructure.IObjectContextAdapter)(new DataModelContainer())).ObjectContext;
                },
                new ContextConfiguration() { ScaffoldAllTables = true }
            //new ContextConfiguration() { ScaffoldAllTables = false }
            );

            // The following registration should be used if YourDataContextType does not derive from DbContext
            // DefaultModel.RegisterContext(typeof(YourDataContextType), new ContextConfiguration() { ScaffoldAllTables = false });

            // The following statement supports separate-page mode, where the List, Detail, Insert, and 
            // Update tasks are performed by using separate pages. To enable this mode, uncomment the following 
            // route definition, and comment out the route definitions in the combined-page mode section that follows.
            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                //Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Constraints = new RouteValueDictionary(new { action = "List|Details" }),
                Model = DefaultModel
            });

            // The following statements support combined-page mode, where the List, Detail, Insert, and
            // Update tasks are performed by using the same page. To enable this mode, uncomment the
            // following routes and comment out the route definition in the separate-page mode section above.
            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.List,
            //    ViewName = "ListDetails",
            //    Model = DefaultModel
            //});

            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.Details,
            //    ViewName = "ListDetails",
            //    Model = DefaultModel
            //});
        }
    }
}
