using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Concurrent;
using Platform.DAAS.OData.Core.HTTP;
using Platform.DAAS.OData.Core.Logging;
using Platform.DAAS.OData.Core.Security;
using Platform.DAAS.OData.Core.ServiceManagement;
using Platform.DAAS.OData.Core.BusinessManagement;
using Platform.DAAS.OData.Core.StorageManagement;
using Platform.DAAS.OData.Core.Caching;
using Platform.DAAS.OData.Core.VamtManagement;
using Platform.DAAS.OData.Core.WdsManagement;
using Platform.DAAS.OData.Authentication;
using Platform.DAAS.OData.HTTP;
using Platform.DAAS.OData.Logging;
using Platform.DAAS.OData.Model;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.ServiceManager;
using Platform.DAAS.OData.BusinessManagement;
using Platform.DAAS.OData.StorageManagement;
using Platform.DAAS.OData.Caching;
using Platform.DAAS.OData.VamtManagement;
using Platform.DAAS.OData.WdsManagement;

namespace Platform.DAAS.OData.Facade
{
    public class Provider
    {
        static ConcurrentDictionary<string, object> Managers = new ConcurrentDictionary<string, object>();

        public static ICacheManager GetCacheManager()
        {
            ICacheManager manager = (Managers.ContainsKey("CacheManager") && Managers["CacheManager"] != null) ? (Managers["CacheManager"] as ICacheManager) : null;

            if (manager == null || manager.HadExceptions())
            {
                manager = new CacheManagerRedis(ConfigurationManager.AppSettings.Get("CacheServerAddress"), int.Parse(ConfigurationManager.AppSettings.Get("CacheServerPort")), ConfigurationManager.AppSettings.Get("CacheServerPassword"), long.Parse(ConfigurationManager.AppSettings.Get("CacheServerDBIndex")));

                //manager = new CacheManagerMemcached();

                if (!Managers.ContainsKey("CacheManager"))
                {
                    Managers.TryAdd("CacheManager", manager);
                }
                else
                {
                    Managers["CacheManager"] = manager;
                }
            }

            return manager;
        }

        internal static void SetCacheManager()
        {
            ICacheManager manager = new CacheManagerRedis(ConfigurationManager.AppSettings.Get("CacheServerAddress"), int.Parse(ConfigurationManager.AppSettings.Get("CacheServerPort")), ConfigurationManager.AppSettings.Get("CacheServerPassword"), long.Parse(ConfigurationManager.AppSettings.Get("CacheServerDBIndex"))); //new CacheManagerMemcached(); 

            if (!Managers.ContainsKey("CacheManager"))
            {
                Managers.TryAdd("CacheManager", manager);
            }
            else
            {
                Managers["CacheManager"] = manager;
            }
        }


        public static ICacheManager GetWdsCacheManager()
        {
            ICacheManager manager = (Managers.ContainsKey("WdsCacheManager") && Managers["WdsCacheManager"] != null) ? (Managers["WdsCacheManager"] as ICacheManager) : null;

            if (manager == null || manager.HadExceptions())
            {
                manager = new CacheManagerRedis(ConfigurationManager.AppSettings.Get("WdsCacheServerAddress"), int.Parse(ConfigurationManager.AppSettings.Get("WdsCacheServerPort")), ConfigurationManager.AppSettings.Get("WdsCacheServerPassword"), long.Parse(ConfigurationManager.AppSettings.Get("WdsCacheServerDBIndex")));

                //manager = new CacheManagerMemcached();

                if (!Managers.ContainsKey("WdsCacheManager"))
                {
                    Managers.TryAdd("WdsCacheManager", manager);
                }
                else
                {
                    Managers["WdsCacheManager"] = manager;
                }
            }

            return manager;
        }

        internal static void SetWdsCacheManager()
        {
            ICacheManager manager = new CacheManagerRedis(ConfigurationManager.AppSettings.Get("WdsCacheServerAddress"), int.Parse(ConfigurationManager.AppSettings.Get("WdsCacheServerPort")), ConfigurationManager.AppSettings.Get("WdsCacheServerPassword"), long.Parse(ConfigurationManager.AppSettings.Get("WdsCacheServerDBIndex"))); //new CacheManagerMemcached(); 

            if (!Managers.ContainsKey("WdsCacheManager"))
            {
                Managers.TryAdd("WdsCacheManager", manager);
            }
            else
            {
                Managers["WdsCacheManager"] = manager;
            }
        }

        internal static void ResetManagers()
        {
            Managers = new ConcurrentDictionary<string, object>();
        }

        public static IResourceRouter ResourceRouter()
        {
            return new ResourceRouter(new ExceptionHandler());
        }

        public static ILogger Logger()
        {
            return new Logger();
        }

        public static ITracer Tracer()
        {
            return new Tracer();
        }

        public static IExHandler ExceptionHandler()
        {
            return new ExceptionHandler();
        }

        public static IAuthManager AuthManager()
        {
            return new AuthManager();
        }

        public static IRoleManager RoleManager()
        {
            return new RoleManager();
        }

        public static IHashManager HashManager()
        {
            return new HashManager();
        }

        public static IEncryptionManager EncryptionManager()
        {
            return new EncryptionManager();
        }

        public static IServiceManager ServiceManager()
        {
            return new ServieManager();
        }

        public static ISecurityManager SecurityManager()
        {
            return new SecurityManager();
        }

        public static IBusinessManager BusinessManager()
        {
            return new BusinessManager();
        }

        public static IDatabaseManager SQLServerDatabaseManager()
        {
            return new SQLServerDatabaseManager();
        }

        public static IWdsManager WdsManager()
        {
            return new WdsManager();
        }

        public static IVamtManager VamtManager()
        {
            return new VamtManager(ConfigurationManager.AppSettings.Get("VamtPSModulePath"));
        }
    }
}
