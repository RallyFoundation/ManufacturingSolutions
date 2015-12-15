using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Xml.Schema;

/// <summary>
/// Summary description for XMLUtility
/// </summary>
public class XMLUtility
{
	public XMLUtility()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static SchemaValidationListener NewSchemaValidationListener()
    {
        return new SchemaValidationListener();
    }

    public static bool IsXmlValid(string xml, string schemaUri, ValidationEventHandler schemaValidationEventHandler)
    {
        bool returnValue = false;

        if ((!String.IsNullOrEmpty(schemaUri)) && (!String.IsNullOrEmpty(xml)))
        {
            XmlReader xmlSchemaReader = XmlReader.Create(schemaUri);

            if (xmlSchemaReader != null)
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ConformanceLevel = ConformanceLevel.Auto;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;

                if (schemaValidationEventHandler != null)
                {
                    settings.ValidationEventHandler += schemaValidationEventHandler;
                }

                XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();

                xmlSchemaSet.Add(null, xmlSchemaReader);

                xmlSchemaSet.Compile();

                settings.Schemas = xmlSchemaSet;

                StringReader xmlDocumentStringReader = new StringReader(xml);

                XmlReader xmlDocumentReader = XmlReader.Create(xmlDocumentStringReader, settings);

                while (xmlDocumentReader.Read())
                {
                }

                #region Tracing
                FileStream stream = new FileStream(schemaUri, FileMode.Open, FileAccess.Read, FileShare.Read);

                StreamReader reader = new StreamReader(stream);

                reader.Close();
                stream.Close();
                #endregion

                returnValue = true;
            }
        }

        return returnValue;
    }

    public static string ValidateXML(string xml, string schema)
    {
        string returnValue = String.Empty;

        if ((!String.IsNullOrEmpty(xml)) && (!String.IsNullOrEmpty(schema)))
        {
            StringReader stringReader = new StringReader(schema);

            XmlReader xmlSchemaReader = XmlReader.Create(stringReader);

            if (xmlSchemaReader != null)
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ConformanceLevel = ConformanceLevel.Auto;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;

                SchemaValidationListener listener = new SchemaValidationListener();

                settings.ValidationEventHandler += listener.OnSchemaValidating;

                XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();

                xmlSchemaSet.Add(null, xmlSchemaReader);

                xmlSchemaSet.Compile();

                settings.Schemas = xmlSchemaSet;

                StringReader xmlDocumentStringReader = new StringReader(xml);

                XmlReader xmlDocumentReader = XmlReader.Create(xmlDocumentStringReader, settings);

                while (xmlDocumentReader.Read())
                {
                }

                returnValue = String.Format("Error count:{0};\r\n Error messages:\r\n{1}", listener.ErrorCount, listener.ErrorMessage);
            }
        }

        return returnValue;
    }

    public static string XmlSerialize(object objectToSerialize, Type[] extraTypes)
    {
        string returnValue = String.Empty;

        XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType(), extraTypes);

        MemoryStream stream = new MemoryStream();

        serializer.Serialize(stream, objectToSerialize);

        byte[] bytes = stream.GetBuffer();

        returnValue = Encoding.UTF8.GetString(bytes);

        stream.Close();

        return returnValue;
    }

    public static string XmlTransform(string xml, string xsltUri)
    {
        string returnValue = String.Empty;

        XslCompiledTransform transform = new XslCompiledTransform();

        transform.Load(xsltUri);

        XmlDocument document = new XmlDocument();
        document.LoadXml(xml);

        MemoryStream stream = new MemoryStream();

        transform.Transform(document, null, stream);

        byte[] bytes = stream.GetBuffer();

        returnValue = Encoding.UTF8.GetString(bytes);

        stream.Close();

        return returnValue;
    }

    public static object XmlDeserialize(string xml, Type type, Type[] extraTypes)
    {
        object returnValue = null;

        XmlSerializer serializer = new XmlSerializer(type, extraTypes);

        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        returnValue = serializer.Deserialize(stream);

        stream.Close();

        return returnValue;
    }
}