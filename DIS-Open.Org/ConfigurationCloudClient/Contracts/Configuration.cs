using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace DISConfigurationCloud.Contract
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DISConfigurationCloud.MetaManagement")]
	public class Configuration 
    {
        [DataMember]
        [XmlAttribute]
        public string ID { get; set; }

        [DataMember]
		public string DbConnectionString{ get;  set;}

        [DataMember]
        [XmlAttribute("Type")]
        public ConfigurationType ConfigurationType { get; set; }

		public Configuration()
        {

		}
	}

    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DISConfigurationCloud.MetaManagement")]
    public enum ConfigurationType 
    {
        [EnumMember]
        OEM = 0,
        [EnumMember]
        TPI = 1,
        [EnumMember]
        FactoryFloor = 2
    }
}