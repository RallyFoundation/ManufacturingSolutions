using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace DISConfigurationCloud.Contract
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DISConfigurationCloud.MetaManagement")]
	public class Customer {

        [DataMember]
		public Configuration[] Configurations{ get;  set;}

        [DataMember]
        [XmlAttribute]
		public string ID{ get;  set;}

        [DataMember]
		public string Name{ get;  set;}

        [DataMember]
        [XmlArray("References")]
        [XmlArrayItem("ReferenceID")]
        public string[] ReferenceID { get; set; }

		public Customer()
        {

		}
	}
}