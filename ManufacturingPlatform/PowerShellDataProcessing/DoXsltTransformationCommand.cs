using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Platform.DAAS.OData.Utility;

namespace PowerShellDataProcessing
{
    [Cmdlet("Do", "XsltTransformation")]
    public class DoXsltTransformationCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The content of the xml document to be transformed.")]
        public string XmlString { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The path of the xslt file to transform the xml document.")]
        public string XsltPath { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The encoding for the transformed xml document.")]
        public string OutputEncoding { get; set; }

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "The argument(s) to be passed to the xslt to use for the transformation.")]
        public Dictionary<string, object> XsltArguments { get; set; }

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "The extension object(s) to be passed to the xslt to use for the transformation.")]
        public Dictionary<string, object> XsltExtendedObjects { get; set; }
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            //string outputEncoding = String.IsNullOrEmpty(OutputEncoding) ? System.Text.Encoding.Default.EncodingName : OutputEncoding;

            string result = XmlUtility.GetTransformedXmlStringByXsltDocument(XmlString, XsltPath, XsltArguments, XsltExtendedObjects, OutputEncoding);

            this.WriteObject(result);
        }
    }
}
