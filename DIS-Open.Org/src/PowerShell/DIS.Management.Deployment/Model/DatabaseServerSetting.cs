using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DIS.Management.Deployment.Model
{
    public class DatabaseServerSetting
    {
        [XmlAttribute("isIncluded")]
        public bool IsIncluded { get; set; }

        [XmlElement("authMode")]
        public DatabaseAuthenticationMode AuthenticationMode { get; set; }

        [XmlElement("serverAddress")]
        public string ServerAddress { get; set; }

        [XmlElement("serverLoginName")]
        public string ServerLoginName { get; set; }

        [XmlElement("serverPassword")]
        public string ServerPassword { get; set; }

        [XmlElement("dbName")]
        public string DatabaseName { get; set; }
    }

    public enum DatabaseAuthenticationMode 
    {
        WindowsIntegrated = 0,
        Mixed = 1
    }
}
