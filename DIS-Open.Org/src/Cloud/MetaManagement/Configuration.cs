using System.Xml;
using System.Xml.Serialization;

namespace DISConfigurationCloud.MetaManagement 
{
	public class Configuration 
    {
        [XmlAttribute]
        public string ID { get; set; } 
		public string DbConnectionString{ get;  set;}

        [XmlAttribute("Type")]
        public ConfigurationType ConfigurationType { get; set; }

		public Configuration()
        {

		}

		~Configuration()
        {

		}
	}

    public enum ConfigurationType 
    {
        OEM = 0,
        TPI = 1,
        FactoryFloor = 2
    }
}