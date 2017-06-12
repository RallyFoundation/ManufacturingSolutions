using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Management.Automation;
using Platform.DAAS.OData.Utility;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.Get, "TransformedXml")]
    public class GetTransformedXmlCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The xml document to be transformed.")]
        public XmlDocument XmlDocument { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The path of the xslt file to transform the xml document.")]
        public string XsltPath { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The encoding for the transformed xml document.")]
        public string OutputEncoding { get; set; }
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            //string outputEncoding = String.IsNullOrEmpty(OutputEncoding) ? System.Text.Encoding.Default.EncodingName : OutputEncoding;
            string xmlString = XmlDocument.InnerXml;

            string result = XmlUtility.GetTransformedXmlStringByXsltDocument(xmlString, XsltPath, null, null, OutputEncoding);

            //XmlDocument resultDoc = new XmlDocument();
            //resultDoc.LoadXml(result);

            //this.WriteObject(resultDoc);

            this.WriteObject(result);
        }
    }
}
