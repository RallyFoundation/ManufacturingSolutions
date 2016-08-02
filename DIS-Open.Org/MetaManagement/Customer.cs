using System.Xml;
using System.Xml.Serialization;

namespace DISConfigurationCloud.MetaManagement {
	public class Customer {

		public Configuration[] Configurations{ get;  set;} 

        [XmlAttribute]
		public string ID{ get;  set;} 
		public string Name{ get;  set;}

        [XmlArray("References")]
        [XmlArrayItem("ReferenceID")]
        public string[] ReferenceID { get; set; }

		public Customer()
        {

		}

		~Customer()
        {

		}
	}
}