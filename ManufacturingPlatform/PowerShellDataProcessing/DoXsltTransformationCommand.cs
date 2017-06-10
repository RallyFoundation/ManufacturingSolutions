using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            string outputEncoding = String.IsNullOrEmpty(OutputEncoding) ? "utf-8" : OutputEncoding;

            string result = XmlUtility.XmlTransform(XmlString, XsltPath, outputEncoding);

            this.WriteObject(result);
        }
    }
}
