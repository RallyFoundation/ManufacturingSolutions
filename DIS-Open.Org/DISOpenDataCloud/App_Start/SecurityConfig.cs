using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Facade;
using System.Configuration;

namespace DISOpenDataCloud
{
    public class SecurityConfig
    {
        public static void RegisterAuthorizationMeta(string ConfigFilePath)
        {
            string configValue = ConfigurationManager.AppSettings.Get("RegisterAuthOnStartup");

            bool shouldDoRegister = (!String.IsNullOrEmpty(configValue)) && (configValue.ToLower() == "true" || configValue == "1");

            if (shouldDoRegister)
            {
                Platform.DAAS.OData.Facade.Global.RegisterAuthorizationMeta(ConfigFilePath);
            }
        }

        public static void SetStoreConnectionName()
        {
            Platform.DAAS.OData.Facade.Global.SetAuthorizationStoreConnectionName("DefaultConnection");
            Platform.DAAS.OData.Facade.Global.SetIdentityStoreConnectionName("DefaultConnection");
        }

        public static void SetObsoleteMetaOptions()
        {
            Platform.DAAS.OData.Facade.Global.SetObsoleteDataScopeMetaOption(true);
            Platform.DAAS.OData.Facade.Global.SetObsoleteOperationMetaOption(true);
            Platform.DAAS.OData.Facade.Global.SetObsoleteRoleMetaOption(false);
            Platform.DAAS.OData.Facade.Global.SetObsoleteUserMetaOption(false);
        }
    }
}
