using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Contract;

namespace OA3ToolConfGen
{
    public class ModuleConfiguration
    {
        public const string AppStateKey_DBConnectionString = "DBConnectionString";
        public const string AppStateKey_CloudConfigurationID = "CloudConfigurationID";
        public const string AppStateKey_CloudServicePoint = "CloudServicePoint";
        public const string AppStateKey_CloudUserName = "CloudUserName";
        public const string AppStateKey_CloudPassword = "CloudPassword";
        public const string AppStateKey_OA3ToolConfiguration = "OA3ToolConfiguration";
        public const string AppStateKey_CloudConfigurationSets = "CloudConfigurationSets";

        public static string OHRKey_ZPC_MODEL_SKU = "ZPC_MODEL_SKU";
        public static string OHRKey_ZFRM_FACTOR_CL1 = "ZFRM_FACTOR_CL1";
        public static string OHRKey_ZFRM_FACTOR_CL2 = "ZFRM_FACTOR_CL2";
        public static string OHRKey_ZSCREEN_SIZE = "ZSCREEN_SIZE";
        public static string OHRKey_ZTOUCH_SCREEN = "ZTOUCH_SCREEN";

        public static string OHRValue_ZFRM_FACTOR_CL1_Desktop = "Desktop";
        public static string OHRValue_ZFRM_FACTOR_CL1_Notebook = "Notebook";
        public static string OHRValue_ZFRM_FACTOR_CL1_Tablet = "Tablet";

        public static string OHRValue_ZFRM_FACTOR_CL2_Standard = "Standard";
        public static string OHRValue_ZFRM_FACTOR_CL2_AIO = "AIO";
        public static string OHRValue_ZFRM_FACTOR_CL2_Ultraslim = "Ultraslim";
        public static string OHRValue_ZFRM_FACTOR_CL2_Convertible = "Convertible";
        public static string OHRValue_ZFRM_FACTOR_CL2_Detachable = "Detachable";

        public static string OHRValue_ZTOUCH_SCREEN_Touch = "Touch";
        public static string OHRValue_ZTOUCH_SCREEN_Nontouch = "Non-touch";

        public static string OA3ToolConfigurationValue_ProtocolSequence = "ncacn_ip_tcp";
        public static string OA3ToolConfigurationValue_Options = "";

        public static Customer[] GetFactoryFloorConfigurationSets(string ServicePoint, string UserName, string Password) 
        {
            Customer[] returnValue = null;

            string url = ServicePoint;
            string authHeader = String.Format("{0}:{1}", UserName, Password);

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, (url.Length - 1));
            }

            url += "/Services/DISConfigurationCloud.svc";

            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = url;
            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = authHeader;
            DISConfigurationCloud.Client.ModuleConfiguration.CachingPolicy = CachingPolicy.RemoteOnly;

            Manager manager = new Manager(false, null);

            returnValue = manager.GetCustomers();

            returnValue = (returnValue != null) ? (returnValue.Where((o) => (o.Configurations.FirstOrDefault((c) => (c.ConfigurationType == ConfigurationType.FactoryFloor)) != null)).ToArray()) : null;

            return returnValue;
        }

        public static Configuration GetFactoryFloorConfigurationByBusinessID(Customer[] ConfigurationSets, string BusinessID) 
        {
            if ((ConfigurationSets != null) && (!String.IsNullOrEmpty(BusinessID)))
            {
                Customer configSet = ConfigurationSets.FirstOrDefault((o) => (o.ID == BusinessID));

                if ((configSet != null) && (configSet.Configurations != null))
	            {
                    return configSet.Configurations.FirstOrDefault((c) => (c.ConfigurationType == ConfigurationType.FactoryFloor));
	            }
            }

            return null;
        }

        public static Customer GetConfigurationSetByConfigurationID(Customer[] ConfigurationSets, string ConfigurationID) 
        {
            if ((ConfigurationSets != null) && (!String.IsNullOrEmpty(ConfigurationID))) 
            {
                return ConfigurationSets.FirstOrDefault((o) => (o.Configurations.FirstOrDefault((c) => (c.ID == ConfigurationID)) != null));
            }

            return null;
        }

        public static string GetDBConnectionString(string ServicePoint, string UserName, string Password, string ConfigurationID)
        {
            string returnValue = "";

            string url = ServicePoint;
            string authHeader = String.Format("{0}:{1}", UserName, Password);

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, (url.Length - 1));
            }

            url += "/Services/DISConfigurationCloud.svc";

            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = url;
            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = authHeader;
            DISConfigurationCloud.Client.ModuleConfiguration.CachingPolicy = CachingPolicy.RemoteOnly;

            Manager manager = new Manager(false, null);

            returnValue = manager.GetDBConnectionString(ConfigurationID);

            return returnValue;

        }
    }
}
