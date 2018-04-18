using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using QA.Utility;

namespace PowerShellDataProcessing
{
    [Cmdlet("Do", "XmlSchemaValidation")]
    public class DoXmlSchemaValidationCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The content of the xml document to be validated.")]
        public string XmlString { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The content of the xml schema to validate the xml.")]
        public string XmlSchemaString { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The format for outputting the validation result. Available values: xml, json, text")]
        public string OutputFormat { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            string outputFormat = String.IsNullOrEmpty(OutputFormat) ? OutputFormat.ToLower() : "";

            string result = XmlUtility.ValidateXML(XmlString, XmlSchemaString, outputFormat);

            this.WriteObject(result);
        }
    }
}
