using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Management.Automation;
using Newtonsoft.Json;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.Get, "JsonFromXml")]
    public class GetJsonFromXmlCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The content of the xml document to be transformed.")]
        public string XmlString { get; set; }

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "Whether to indent the JSON in the output.")]
        [PSDefaultValue(Value = true)]
        public SwitchParameter Indent { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "Whether to omit the root onject in the JSON in the output.")]
        [PSDefaultValue(Value = false)]
        public SwitchParameter OmitRoot { get; set; }
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlString);

            string json = JsonConvert.SerializeXmlNode(xmlDoc, (Indent ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None), OmitRoot);

            this.WriteObject(json);
        }
    }
}
