using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DISConfigurationCloud.Client;

namespace DIS.Management.Deployment.Model
{
    public class ApplicationSetting
    {
        [XmlElement("installationType")]
        public InstallationType InstallationType { get; set; }

        [XmlElement("isTPIInCentralizeMode")]
        public int IsTPIInCentralizeMode { get; set; }

        [XmlElement("isTPIUsingCarbonCopy")]
        public int IsTPIUsingCarbonCopy { get; set; }

        [XmlElement("isConfigurationCloudClientTracingEnabled")]
        public bool IsConfigurationCloudClientTracingEnabled { get; set; }

        [XmlElement("configurationCloudClientTraceSourceName")]
        public string ConfigurationCloudClientTraceSourceName { get; set; }

        [XmlElement("configurationCloudServerAddress")]
        public string ConfigurationCloudServerAddress { get; set; }

        [XmlElement("configurationCloudUserName")]
        public string ConfigurationCloudUserName { get; set; }

        [XmlElement("configurationCloudPassword")]
        public string ConfigurationCloudPassword { get; set; }

        [XmlElement("cloudConfigurationCacheStore")]
        public string CloudConfigurationCacheStore { get; set; }

        [XmlElement("cloudConfigurationCachingPolicy")]
        public CachingPolicy CloudConfigurationCachingPolicy { get; set; }

        [XmlElement("servicePortNumber")]
        public int ServicePortNumber { get; set; }

        [XmlElement("serviceProtocalName")]
        public string ServiceProtocalName { get; set; }

        [XmlElement("databaseBackupLocation")]
        public string DatabaseBackupLocation { get; set; }
    }
}
