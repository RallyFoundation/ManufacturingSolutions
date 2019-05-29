using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using System.Management.Automation;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.Format, "Report")]
    public class FormatReportCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The path to the xml document to be transformed.")]
        public string XmlPath { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The path of the xslt file to transform the xml document.")]
        public string XsltPath { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The encoding for the transformed xml document.")]
        public string OutputEncoding { get; set; }

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "The argument(s) to be passed to the xslt to use for the transformation.")]
        public Dictionary<string, object> XsltArguments { get; set; }

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "The extension object(s) to be passed to the xslt to use for the transformation.")]
        public Dictionary<string, object> XsltExtendedObjects { get; set; }

        [Parameter(Position = 5, Mandatory = false, HelpMessage = "The name for the argument that represents the anchor argument to be passed to the xslt to point to a section of the xml document for the transformation.")]
        public string AnchorArgumentName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            FormReport report = new FormReport()
            {
                XmlUri = XmlPath,
                XsltUri = XsltPath,
                XsltArguments = XsltArguments,
                XsltExtendedObjects = XsltExtendedObjects
            };

            if (!String.IsNullOrEmpty(OutputEncoding))
            {
                report.Encoding = OutputEncoding;
            }

            if (!String.IsNullOrEmpty(AnchorArgumentName))
            {
                report.AnchorParamName = AnchorArgumentName;
            }

            report.Format();

            report.ShowDialog();

            this.WriteObject(report);
        }
    }
}
