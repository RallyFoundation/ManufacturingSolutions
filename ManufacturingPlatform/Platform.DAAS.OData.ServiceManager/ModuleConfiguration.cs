using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Core.ServiceManagement;
using Platform.DAAS.OData.Core.DomainModel;

namespace Platform.DAAS.OData.ServiceManager
{
    public class ModuleConfiguration
    {
        public static string Default_Model_Assembly_Path = @"C:\DAAS\ODataPlatform\CustomModules\";

        public static SortedDictionary<string, string> ServiceDBConnectionStrings = null;

        public static SortedDictionary<string, string> ServiceResourceNames = null;

        public static void Initialize()
        {
            ServiceDBConnectionStrings = new SortedDictionary<string, string>();

            ServiceResourceNames = new SortedDictionary<string, string>();

            ServieManager serviceManager = new ServieManager();

            Application[] apps = serviceManager.GetApplications();

            if (apps != null)
            {
                foreach (var app in apps)
                {
                    if (app.Services != null)
                    {
                        foreach (var service in app.Services)
                        {
                            if (!ServiceDBConnectionStrings.ContainsKey(service.ID))
                            {
                                ServiceDBConnectionStrings.Add(service.ID, service.ServiceMeta.DBConnectionString);
                            }

                            if (!ServiceResourceNames.ContainsKey(service.ID))
                            {
                                ServiceResourceNames.Add(service.ID, service.ServiceMeta.ResourceName);
                            }
                        }
                    }
                }
            }
        }
    }
}
