using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIS.Management.Deployment.Model;
using DISConfigurationCloud.Utility;
using DISConfigurationCloud.Contract;
using DISConfigurationCloud.Client;

namespace PowerShellModuleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Installation installation = new Installation()
            {
                ApplicationRoot = @"C:\Program Files (x86)\DIS Solution\",
                InstallationMode = InstallationMode.Decentralized,
                InstallationType = InstallationType.FactoryFloor,

                Components = new Component[]
                {
                     new Component()
                     {
                         Name = "DISConfigurationCloud",
                         AppSetting = new ApplicationSetting()
                         {
                              DatabaseBackupLocation = @"C:\DISConfigurationCloud\Backup\"
                         },
                         AppServerSetting = new ApplicationServerSetting()
                         {
                              IsIncluded = true,
                              RootDirectory = "DISConfigurationCloud", 
                              HTTPPortNumber = 8818,
                              IsSecured = true,
                              ApplicationName = "DISConfigurationCloud",
                              ApplicationPoolName = "DISConfigurationCloudAppPool",
                              ApplicationPoolIdentityType = ApplicationPoolIdentityType.LocalSystem
                         },
                         DBServerSetting = new DatabaseServerSetting()
                         {
                              IsIncluded = true,
                              AuthenticationMode = DatabaseAuthenticationMode.Mixed,
                              ServerAddress = "Win-Server-12",
                              ServerLoginName = "DIS",
                              ServerPassword = "D!S@OMSG.msft",
                              DatabaseName = "DISConfigurationCloud"
                         }
                     },

                     new Component()
                     {
                         Name = "KMT",
                         AppSetting = new ApplicationSetting()
                         {
                              IsTPIUsingCarbonCopy = 1,
                              IsTPIInCentralizeMode = 0,
                              InstallationType = InstallationType.FactoryFloor,
                              CloudConfigurationCacheStore = "Cloud-Configs.xml",
                              CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.MergedAll,
                              ConfigurationCloudServerAddress = "http://localhost:8818",
                              ConfigurationCloudUserName = "DIS",
                              ConfigurationCloudPassword = "D!S@OMSG.msft"
                         }
                     },

                     new Component()
                     {
                         Name = "DataPollingService",
                         AppSetting = new ApplicationSetting()
                         {
                              InstallationType = InstallationType.FactoryFloor,
                              CloudConfigurationCacheStore = "Cloud-Configs.xml",
                              CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.MergedAll,
                              ConfigurationCloudServerAddress = "http://localhost:8818",
                              ConfigurationCloudUserName = "DIS",
                              ConfigurationCloudPassword = "D!S@OMSG.msft"
                         }
                     },

                     new Component()
                     {
                         Name = "KeyProviderService",
                         AppSetting = new ApplicationSetting()
                         {
                              InstallationType = InstallationType.FactoryFloor,
                              CloudConfigurationCacheStore = "Cloud-Configs.xml",
                              CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.MergedAll,
                              ConfigurationCloudServerAddress = "http://localhost:8818",
                              ConfigurationCloudUserName = "DIS",
                              ConfigurationCloudPassword = "D!S@OMSG.msft"
                         }
                     },

                     new Component()
                     {
                         Name = "InternalAPI",
                         AppSetting = new ApplicationSetting()
                         {
                              InstallationType = InstallationType.FactoryFloor,
                              CloudConfigurationCacheStore = "Cloud-Configs.xml",
                              CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.MergedAll,
                              ConfigurationCloudServerAddress = "http://localhost:8818",
                              ConfigurationCloudUserName = "DIS",
                              ConfigurationCloudPassword = "D!S@OMSG.msft"
                         },
                         AppServerSetting = new ApplicationServerSetting()
                         {
                              IsIncluded = true,
                              RootDirectory = "Web Services\\Internal Web Service", 
                              HTTPPortNumber = 8012,
                              IsSecured = true,
                              ApplicationName = "InternalWebService",
                              ApplicationPoolName = "InternalWebServiceAppPool",
                              ApplicationPoolIdentityType = ApplicationPoolIdentityType.SpecificUser,
                              ApplicationPoolIdentityUserName = ".\\DIS",
                              ApplicationPoolIdentityPassword = "D!S@OMSG.msft"
                         }
                     },

                     new Component()
                     {
                         Name = "ExternalAPI",
                         AppSetting = new ApplicationSetting()
                         {
                              InstallationType = InstallationType.FactoryFloor,
                              CloudConfigurationCacheStore = "Cloud-Configs.xml",
                              CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.MergedAll,
                              ConfigurationCloudServerAddress = "http://localhost:8818",
                              ConfigurationCloudUserName = "DIS",
                              ConfigurationCloudPassword = "D!S@OMSG.msft"
                         },
                         AppServerSetting = new ApplicationServerSetting()
                         {
                              IsIncluded = true,
                              RootDirectory = "Web Services\\External Web Service", 
                              HTTPPortNumber = 8011,
                              IsSecured = true,
                              ApplicationName = "ExternalWebService",
                              ApplicationPoolName = "ExternalWebServiceAppPool",
                              ApplicationPoolIdentityType = ApplicationPoolIdentityType.LocalSystem
                         }
                     }
                 }
            };

            XmlUtility xmlUtility = new XmlUtility();

            Type[] types = new Type[] 
            {
                typeof(Component),
                typeof(Component[]),
                typeof(ApplicationServerSetting),
                typeof(DatabaseServerSetting),
                typeof(ApplicationSetting),
                typeof(ApplicationPoolIdentityType),
                typeof(DatabaseAuthenticationMode),
                typeof(InstallationType),
                typeof(InstallationMode),
                typeof(CachingPolicy)
            };

            string xml = xmlUtility.XmlSerialize(installation, types, "utf-8");

            //xml = xml.Insert(xml.IndexOf("?>"), " encoding=\"utf-8\"");

            //xml = xml.Substring(0, (xml.LastIndexOf(">") + 1));

            Console.Write(xml);

            string path = "unattend.xml";

            using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8))
                {
                    writer.Write(xml);
                }
            }

            Console.Read();
        }
    }
}
