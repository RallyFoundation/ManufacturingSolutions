using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QA.Core;

namespace QA.Mapper
{
    public class Decoded4KHHXmlMapper : IMapper
    {
        public object Map(object Data)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();

            if (Data != null)
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.LoadXml(Data.ToString());

                //string xPath = "//p[(@n != 'MacAddress') and (@n != 'PhysicalMedium')]";

                string xPath = "//p[(@n != 'MacAddress') and (@n != 'PhysicalMedium') and (@n != 'TPM EkPub')]";

                XmlNodeList nodes = xmlDocument.SelectNodes(xPath);

                string name, value;

                if ((nodes != null) && (nodes.Count > 0))
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        name = nodes[i].Attributes["n"].Value;
                        name = name.Trim();
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
                        name = name.Trim();
                        value = nodes[i].Attributes["v"].Value;

                        if (!nicInfo.ContainsKey(name))
                        {
                            nicInfo.Add(name, value);
                        }
                        else
                        {
                            nicInfo.Add(String.Format("{0}_{1}_{2}", name, value, i), value);
                        }

                        i++;
                    }

                    result.Add("NIC", nicInfo);
                }
                else
                {
                    result.Add("NIC", null);
                }

                xPath = "//p[(@n = 'TPM EkPub')]";
                nodes = xmlDocument.SelectNodes(xPath);

                if ((nodes != null) && (nodes.Count > 0))
                {
                    Dictionary<string, string> tpmEkPubInfo = new Dictionary<string, string>();

                    for (int i = 0; i < nodes.Count; i++)
                    {
                        name = "TPMEkPub_" + i.ToString();
                        value = nodes[i].Attributes["v"].Value;

                        tpmEkPubInfo.Add(name, value);

                        i++;
                    }

                    result.Add("TPMEkPub", tpmEkPubInfo);
                }
                else
                {
                    result.Add("TPMEkPub", null);
                }
            }

            return result;
        }
    }
}
