using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QA.Core;

namespace QA.Parser
{
    public class Decoded4KHHXmlParser : IParser
    {
        public object Parse(object Data)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();

            if (Data != null)
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.LoadXml(Data.ToString());

                string xPath = "//p[(@n != 'MacAddress') and (@n != 'PhysicalMedium')]";

                XmlNodeList nodes = xmlDocument.SelectNodes(xPath);

                string name, value;

                if ((nodes != null) && (nodes.Count > 0))
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        name = nodes[i].Attributes["n"].Value;
                        value = nodes[i].Attributes["v"].Value;

                        result.Add(name, value);
                    }
                }

                xPath = "//p[(@n = 'MacAddress') or (@n = 'PhysicalMedium')]";

                nodes = xmlDocument.SelectNodes(xPath);

                if ((nodes != null) && (nodes.Count > 0))
                {
                    Dictionary<string, string> nicInfo = new Dictionary<string, string>();

                    for (int i = 0; i < nodes.Count; i++)
                    {
                        name = nodes[(i + 1)].Attributes["v"].Value;
                        value = nodes[i].Attributes["v"].Value;

                        nicInfo.Add(name, value);

                        i++;
                    }

                    result.Add("NIC", nicInfo);
                }
            }

            return result;
        }
    }
}
