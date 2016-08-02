using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Core.HTTP;
using Platform.DAAS.OData.Core.Logging;
using Platform.DAAS.OData.Core.Security;
using Platform.DAAS.OData.Core.ServiceManagement;
using Platform.DAAS.OData.Authentication;
using Platform.DAAS.OData.HTTP;
using Platform.DAAS.OData.Logging;
using Platform.DAAS.OData.Model;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.ServiceManager;

namespace Platform.DAAS.OData.Facade
{
    public class Provider
    {
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
    }
}
