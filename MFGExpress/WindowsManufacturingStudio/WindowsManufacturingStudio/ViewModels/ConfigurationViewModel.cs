using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace WindowsManufacturingStudio.ViewModels
{
    [XmlRoot(ElementName = "configurationItems")]
    public class ConfigurationViewModel
    {
        [JsonProperty("image-server-host")]
        [XmlElement(ElementName = "imageServerAddress")]
        public string ImageServerAddress { get; set; }

        [JsonProperty("image-server-port")]
        [XmlElement(ElementName = "imageServerPort")]
        public int ImageServerPort { get; set; }

        [JsonProperty("image-server-username")]
        [XmlElement(ElementName = "imageServerUserName")]
        public string ImageServerUserName { get; set; }

        [JsonProperty("image-server-password")]
        [XmlElement(ElementName = "imageServerPassword")]
        public string ImageServerPassword { get; set; }

        [JsonProperty("wds-api-service-point")]
        [XmlElement(ElementName = "wdsApiServicePoint")]
        public string WDSApiServicePoint { get; set; }

        [JsonProperty("http-server-host")]
        [XmlElement(ElementName = "httpServerAddress")]
        public string HttpServerAddress { get; set; }

        [JsonProperty("http-server-port")]
        [XmlElement(ElementName = "httpServerPort")]
        public int HttpServerPort { get; set; }

        [JsonProperty("web-socket-server-host")]
        [XmlElement(ElementName = "webSocketServerAddress")]
        public string WebSocketServerAddress { get; set; }

        [JsonProperty("web-socket-server-port")]
        [XmlElement(ElementName = "webSocketServerPort")]
        public int WebSocketServerPort { get; set; }

        [JsonProperty("nic-name")]
        [XmlElement(ElementName = "nicName")]
        public string NICName { get; set; }

        [JsonProperty("client-identifier-type")]
        [XmlElement(ElementName = "clientIdentifierType")]
        public int ClientIdentifierType { get; set; }

        [JsonProperty("client-identifier-value")]
        [XmlElement(ElementName = "clientIdentifierValue")]
        public string ClientIdentifierValue { get; set; }

        [JsonProperty("image-identifier-type")]
        [XmlElement(ElementName = "imageIdentifierType")]
        public int ImageIdentifierType { get; set; }

        [JsonProperty("image-identifier-value")]
        [XmlElement(ElementName = "imageIdentifierValue")]
        public string ImageIdentifierValue { get; set; }

        [JsonProperty("image-destination")]
        [XmlElement(ElementName = "imageDestination")]
        public string ImageDestination { get; set; }
    }
}
