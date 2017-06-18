﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
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

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "The argument(s) to be passed to the xslt to use for the transformation.")]
        public Dictionary<string, object> XsltArguments {get; set;}

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "The extension object(s) to be passed to the xslt to use for the transformation.")]
        public Dictionary<string, object> XsltExtendedObjects { get; set; }
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            //string outputEncoding = String.IsNullOrEmpty(OutputEncoding) ? System.Text.Encoding.Default.EncodingName : OutputEncoding;
            //string xmlString = XmlDocument.InnerXml;

            //string result = XmlUtility.GetTransformedXmlStringByXsltDocument(xmlString, XsltPath, null, null, OutputEncoding);

            //XmlDocument resultDoc = new XmlDocument();
            //resultDoc.LoadXml(result);

            //this.WriteObject(resultDoc);

            //this.WriteObject(result);

            XslCompiledTransform transform = new XslCompiledTransform();

            //transform.OutputSettings.Encoding = OutputEncoding;
            //transform.OutputSettings.CloseOutput = true;
            //transform.OutputSettings.WriteEndDocumentOnClose = true;

            transform.Load(XsltPath);

            MemoryStream stream = new MemoryStream();

            XsltArgumentList arguments = null;

            if (XsltArguments != null && XsltArguments.Count > 0)
            {
                if (arguments == null)
                {
                    arguments = new XsltArgumentList();
                }

                foreach (string key in XsltArguments.Keys)
                {
                    arguments.AddParam(key, String.Empty, XsltArguments[key]);
                }
            }

            if (XsltExtendedObjects != null)
            {
                if (arguments == null)
                {
                    arguments = new XsltArgumentList();
                }

                foreach (string key in XsltExtendedObjects.Keys)
                {
                    arguments.AddExtensionObject(key, XsltExtendedObjects[key]);
                }
            }

            transform.Transform(XmlDocument, arguments, stream);

            byte[] bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);

            //XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings() { CloseInput = true });

            //XmlDocument resultXmlDoc = new XmlDocument();

            //resultXmlDoc.Load(reader);

            //this.WriteObject(resultXmlDoc);

            Encoding outputEncoding = String.IsNullOrEmpty(OutputEncoding) ? System.Text.Encoding.Default : System.Text.Encoding.GetEncoding(OutputEncoding);

            string result = outputEncoding.GetString(bytes);

            stream.Close();

            //this.WriteObject(result);

            result = result.Substring(result.IndexOf("<"));
            result = result.Substring(0, result.LastIndexOf(">") + 1);

            XmlDocument resultXmlDoc = new XmlDocument();

            resultXmlDoc.LoadXml(result);

            this.WriteObject(resultXmlDoc);
        }
    }
}