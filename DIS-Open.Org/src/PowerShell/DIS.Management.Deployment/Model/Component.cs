using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DIS.Management.Deployment.Model
{
    public class Component
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("dbServer")]
        public DatabaseServerSetting DBServerSetting { get; set; }
        
        [XmlElement("appServer")]
        public ApplicationServerSetting AppServerSetting { get; set; }

        [XmlElement("appConfig")]
        public ApplicationSetting AppSetting { get; set; }
    }
}
