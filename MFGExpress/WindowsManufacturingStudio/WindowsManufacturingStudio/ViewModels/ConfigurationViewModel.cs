using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindowsManufacturingStudio.ViewModels
{
    [XmlRoot(ElementName = "configurationItems")]
    public class ConfigurationViewModel
    {
        [XmlElement(ElementName = "imageServerAddress")]
        public string ImageServerAddress { get; set; }

        [XmlElement(ElementName = "imageServerUserName")]
        public string ImageServerUserName { get; set; }

        [XmlElement(ElementName = "imageServerPassword")]
        public string ImageServerPassword { get; set; }

        [XmlElement(ElementName = "wdsApiServicePoint")]
        public string WDSApiServicePoint { get; set; }
    }
}
