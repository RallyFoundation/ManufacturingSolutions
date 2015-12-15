using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using DataIntegrator.Helpers.Tracing;

namespace DataIntegrator.Helpers.XSLT
{
    class XSLTHelper
    {
        public XSLTHelper() 
        {
        }

        public XSLTHelper(bool enableTracing, string traceSourceName) 
        {
            this.EnableTracing = enableTracing;
            this.TraceSourceName = traceSourceName;
        }

        public bool EnableTracing { get; set; }

        public string TraceSourceName { get; set; }

        /// <summary>
        /// Creates a new instance of XsltCompiledTransform
        /// </summary>
        /// <param name="xsltFilePath">XSLT path</param>
        /// <returns>A new instance of XsltCompiledTransform</returns>
        public XslCompiledTransform CreateXsltTransform(string xsltFilePath) 
        {
            XslCompiledTransform transform = new XslCompiledTransform(true);
            transform.Load(xsltFilePath);

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { xsltFilePath, transform }, this.TraceSourceName);
            }

            return transform;
        }

        /// <summary>
        /// Transforms an XML document content to an XML document content in another format by specifying an XSLT path
        /// </summary>
        /// <param name="xmlString">Original XML document content</param>
        /// <param name="xsltFilePath">XSLT path</param>
        /// <returns>Transformed XML document content</returns>
        public string GetTransformedXmlStringByXsltDocument(string xmlString, string xsltFilePath)
        {
            string returnValue = xmlString;

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { xmlString, xsltFilePath}, this.TraceSourceName);
            }

            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlString);

            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xsltFilePath);

            StringWriter stringWriter = new StringWriter();

            transform.Transform(document, null, stringWriter);

            stringWriter.Flush();
            stringWriter.Close();

            returnValue = stringWriter.ToString();

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] {document.InnerXml, xsltFilePath, transform, returnValue }, this.TraceSourceName);
            }

            return returnValue;
        }

        /// <summary>
        /// Transforms an XML document content to an XML document content in another format by specifying an XSLT path
        /// </summary>
        /// <param name="xmlString">Original XML document content</param>
        /// <param name="xsltFilePath">XSLT path</param>
        /// <param name="parameters">A generic dictionary containing the XSLT parameters for XSLT transformation(optional)</param>
        /// <param name="extensionObjects">A generic dictionary containing the XSLT extended objects for XSLT transformation(optional)</param>
        /// <param name="outputEncodingName">The encoding name indicating the encoding of the output content. A table listing all available encoding names and their relative code page numbers can be found at :  http://msdn.microsoft.com/en-us/library/system.text.encoding.aspx </param>
        /// <returns>Transformed XML document content</returns>
        public string GetTransformedXmlStringByXsltDocument(string xmlString, string xsltFilePath, IDictionary<string, object> parameters, IDictionary<string, object> extensionObjects, string outputEncodingName)
        {
            string returnValue = xmlString;

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { xmlString, xsltFilePath, parameters, extensionObjects, outputEncodingName}, this.TraceSourceName);
            }

            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlString);

            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xsltFilePath);

            //StringWriter stringWriter = new StringWriter();

            XsltArgumentList xsltArgumentList = null;

            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    if (xsltArgumentList == null)
                    {
                        xsltArgumentList = new XsltArgumentList();
                    }

                    foreach (string key in parameters.Keys)
                    {
                        xsltArgumentList.AddParam(key, String.Empty, parameters[key]);
                    }
                }
            }

            if (extensionObjects != null)
            {
                if (extensionObjects.Count > 0)
                {
                    if (xsltArgumentList == null)
                    {
                        xsltArgumentList = new XsltArgumentList();
                    }

                    foreach (string key in extensionObjects.Keys)
                    {
                        xsltArgumentList.AddExtensionObject(key, extensionObjects[key]);
                    }
                }
            }

            //transform.Transform(document, xsltArgumentList, stringWriter);

            //stringWriter.Flush();
            //stringWriter.Close();

            //returnValue = stringWriter.ToString();

            MemoryStream stream = new MemoryStream();

            transform.Transform(document, xsltArgumentList, stream);

            stream.Flush();

            //byte[] outputBytes = stream.GetBuffer();

            //Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

            //returnValue = outputEncoding.GetString(outputBytes);

            byte[] bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);

            Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

            returnValue = outputEncoding.GetString(bytes);

            stream.Close();

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { document.InnerXml, transform, outputEncoding.BodyName, returnValue }, this.TraceSourceName);
            }

            return returnValue;
        }

        /// <summary>
        /// Transforms an XML document content to an XML document content in another format by specifying an XSLT dictionary key
        /// </summary>
        /// <param name="xmlString">Original XML document content</param>
        /// <param name="xsltDictionary">A generic dictionary containg instances of XslCompiledTransform</param>
        /// <param name="xsltKey">XSLT dictionary key pointing to an instances of XslCompiledTransform in the dictionary</param>
        /// <param name="parameters">A generic dictionary containing the XSLT parameters for XSLT transformation(optional)</param>
        /// <param name="extensionObjects">A generic dictionary containing the XSLT extended objects for XSLT transformation(optional)</param>
        /// <param name="outputEncodingName">The encoding name indicating the encoding of the output content. A table listing all available encoding names and their relative code page numbers can be found at :  http://msdn.microsoft.com/en-us/library/system.text.encoding.aspx </param>
        /// <returns>Transformed XML document content</returns>
        public string GetTransformedXmlStringByXsltDocument(string xmlString, IDictionary<string, XslCompiledTransform> xsltDictionary, string xsltKey, IDictionary<string, object> parameters, IDictionary<string, object> extensionObjects, string outputEncodingName)
        {
            string returnValue = String.Empty;

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { xmlString, xsltDictionary, xsltKey, parameters, extensionObjects, outputEncodingName }, this.TraceSourceName);
            }

            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlString);

            if ((document != null) && (xsltDictionary != null) && (xsltDictionary.Count > 0) && (xsltDictionary.ContainsKey(xsltKey)) && (xsltDictionary[xsltKey] != null))
            {
                XslCompiledTransform transform = xsltDictionary[xsltKey];

                //StringWriter stringWriter = new StringWriter();

                XsltArgumentList xsltArgumentList = null;

                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        if (xsltArgumentList == null)
                        {
                            xsltArgumentList = new XsltArgumentList();
                        }

                        foreach (string key in parameters.Keys)
                        {
                            xsltArgumentList.AddParam(key, String.Empty, parameters[key]);
                        }
                    }
                }

                if (extensionObjects != null)
                {
                    if (extensionObjects.Count > 0)
                    {
                        if (xsltArgumentList == null)
                        {
                            xsltArgumentList = new XsltArgumentList();
                        }

                        foreach (string key in extensionObjects.Keys)
                        {
                            xsltArgumentList.AddExtensionObject(key, extensionObjects[key]);
                        }
                    }
                }

                //transform.Transform(document, xsltArgumentList, stringWriter);

                //stringWriter.Flush();
                //stringWriter.Close();

                //returnValue = stringWriter.ToString();

                MemoryStream stream = new MemoryStream();

                transform.Transform(document, xsltArgumentList, stream);

                stream.Flush();

                //byte[] outputBytes = stream.GetBuffer();

                //Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

                //returnValue = outputEncoding.GetString(outputBytes);

                byte[] bytes = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(bytes, 0, bytes.Length);

                Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

                returnValue = outputEncoding.GetString(bytes);

                stream.Close();

                if (this.EnableTracing)
                {
                    TracingHelper.Trace(new object[] { document.InnerXml, transform, outputEncoding.BodyName, returnValue }, this.TraceSourceName);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Transforms an XML document content to an XML document content in another format by specifying an XSLT dictionary key
        /// </summary>
        /// <param name="document">An instance of type IXPathNavigable containing the original XML document content</param>
        /// <param name="xsltDictionary">A generic dictionary containg instances of XslCompiledTransform</param>
        /// <param name="xsltKey">XSLT dictionary key pointing to an instances of XslCompiledTransform in the dictionary</param>
        /// <param name="parameters">A generic dictionary containing the XSLT parameters for XSLT transformation(optional)</param>
        /// <param name="extensionObjects">A generic dictionary containing the XSLT extended objects for XSLT transformation(optional)</param>
        /// <returns>Transformed XML document content</returns>
        public string GetTransformedXmlStringByXsltDocument(IXPathNavigable document, IDictionary<string, XslCompiledTransform> xsltDictionary, string xsltKey, IDictionary<string, object> parameters, IDictionary<string, object> extensionObjects)
        {
            string returnValue = String.Empty;

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { document, xsltDictionary, xsltKey, parameters, extensionObjects}, this.TraceSourceName);
            }

            if ((document != null) && (xsltDictionary != null) && (xsltDictionary.Count > 0) && (xsltDictionary.ContainsKey(xsltKey)) && (xsltDictionary[xsltKey] != null))
            {
                XslCompiledTransform transform = xsltDictionary[xsltKey];

                StringWriter stringWriter = new StringWriter();

                XsltArgumentList xsltArgumentList = null;

                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        if (xsltArgumentList == null)
                        {
                            xsltArgumentList = new XsltArgumentList();
                        }

                        foreach (string key in parameters.Keys)
                        {
                            xsltArgumentList.AddParam(key, String.Empty, parameters[key]);
                        }
                    }
                }

                if (extensionObjects != null)
                {
                    if (extensionObjects.Count > 0)
                    {
                        if (xsltArgumentList == null)
                        {
                            xsltArgumentList = new XsltArgumentList();
                        }

                        foreach (string key in extensionObjects.Keys)
                        {
                            xsltArgumentList.AddExtensionObject(key, extensionObjects[key]);
                        }
                    }
                }

                transform.Transform(document, xsltArgumentList, stringWriter);

                stringWriter.Flush();
                stringWriter.Close();

                returnValue = stringWriter.ToString();

                if (this.EnableTracing)
                {
                    TracingHelper.Trace(new object[] { transform, returnValue }, this.TraceSourceName);
                }
            }

            return returnValue;
        }
    }
}
