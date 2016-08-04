using System.Xml;
using System.Xml.Serialization;

namespace Platform.DAAS.OData.Core.DomainModel
{
	public class Configuration 
    {
        [XmlAttribute]
        public string ID { get; set; } 
		public string DbConnectionString{ get;  set;}

        [XmlAttribute("Type")]
        public ConfigurationType ConfigurationType { get; set; }
	}

    public enum ConfigurationType 
    {
        OEM = 0,
        TPI = 1,
        FactoryFloor = 2
    }
}