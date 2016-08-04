using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Platform.DAAS.OData.Core.DomainModel
{
    public class Business
    {
        [XmlAttribute]
        public string ID { get; set; }
        public string Name { get; set; }

        [XmlArray("References")]
        [XmlArrayItem("ReferenceID")]
        public string[] ReferenceID { get; set; }

        public Configuration[] Configurations { get; set; }

        public BusinessType BusinessType { get; set; }
    }

    public enum BusinessType
    {
        Licensing = 0,
        Manufacturing = 1,
        Extended = 2
    }
}
