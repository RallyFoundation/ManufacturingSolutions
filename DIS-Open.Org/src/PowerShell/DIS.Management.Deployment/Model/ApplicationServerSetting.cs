using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DIS.Management.Deployment.Model
{
    public class ApplicationServerSetting
    {
        [XmlAttribute("isIncluded")]
        public bool IsIncluded { get; set; }

        [XmlElement("rootDirectory")]
        public string RootDirectory { get; set; }

        [XmlElement("httpPortNumber")]
        public int HTTPPortNumber { get; set; }

        [XmlElement("isSecured")]
        public bool IsSecured { get; set; }

        [XmlElement("appName")]
        public string ApplicationName { get; set; }

        [XmlElement("appPoolName")]
        public string ApplicationPoolName { get; set; }

        [XmlElement("appPoolIdentityType")]
        public ApplicationPoolIdentityType ApplicationPoolIdentityType { get; set; }

        [XmlElement("appPoolIdentityUserName")]
        public string ApplicationPoolIdentityUserName { get; set; }

        [XmlElement("appPoolIdentityPassword")]
        public string ApplicationPoolIdentityPassword { get; set; }
    }

    public enum ApplicationPoolIdentityType 
    {
        LocalSystem = 0,
        LocalService = 1,
        NetworkService = 2,
        SpecificUser = 3
    }
}
