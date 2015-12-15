using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DISConfigurationCloud.Client
{
    public class ModuleConfiguration
    {
        public static string ServicePoint;

        public static string UrlTest = "/Test/";

        public static string UrlGetCustomers = "/Customer/All/";

        public static string UrlGetConfiguration = "/Configuration/{0}/";

        public static string AuthorizationHeaderValue = "DIS:D!S@OMSG.msft";

        public static string EncodingName = "utf-8";

        public static bool IsTracingEnabled = true;

        public static string TraceSourceName = "DISConfigurationCloudClientTraceSource";

        public static string LocalCacheStore = "Cloud-Configs.xml";

        public static CachingPolicy CachingPolicy = Client.CachingPolicy.MergedRemoteFirst;

        public static string GetServicePoint(string serverAddress, string servicePoint) 
        {
            if ((!serverAddress.EndsWith("/")) && (!servicePoint.StartsWith("/")))
            {
                return serverAddress + "/" + servicePoint;
            }

            if (serverAddress.EndsWith("/") && servicePoint.StartsWith("/"))
            {
                return serverAddress + servicePoint.Substring(1);
            }

            return serverAddress + servicePoint;
        }
    }
}
