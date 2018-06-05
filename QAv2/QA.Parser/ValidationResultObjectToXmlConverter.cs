using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using QA.Core;
using QA.Model;

namespace QA.Parser
{
    public class ValidationResultObjectToXmlConverter : IParser
    {
        public object Parse(object Data)
        {
            string resultXml = "<TestItems />";

            if (Data == null || (!(Data is object[])))
            {
                return null;
            }

            XmlDocument xmlDoc = null;

            XmlWriterSettings settings = new XmlWriterSettings() {
                 Encoding = Encoding.UTF8,
            };

            IDictionary<string, bool> resultSummary = (Data as object[])[0] as IDictionary<string, bool>;

            IDictionary<string, List<Result>> resultObjects = ((Data as object[])[1] as IDictionary<string, List<Result>>);

            IDictionary<string, object> resultDescription = null;

            IDictionary<string, string> nicInfo = null;

            ValidationRuleItem rule = null;

            Result result = null;

            string[] valueRange = null;

            using (StringWriter stringWriter = new StringWriter())
            {
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);

                xmlWriter.WriteStartDocument(true);

                //TestItems
                xmlWriter.WriteStartElement("TestItems");

                foreach (string fieldName in resultSummary.Keys)
                {
                    xmlWriter.WriteStartElement(fieldName);

                    //xmlWriter.WriteStartElement("Result");
                    //xmlWriter.WriteString(result.IsPassed.ToString());
                    //xmlWriter.WriteEndElement();

                    //Result
                    xmlWriter.WriteElementString("Result", resultSummary[fieldName].ToString());

                    for (int i = 0; i < resultObjects[fieldName].Count; i++)
                    {
                        result = resultObjects[fieldName][i];

                        rule = (result.RuleInstance as ValidationRuleItem);

                        switch (result.RuleType)
                        {
                            case RuleType.EqualTo:
                                {
                                    xmlWriter.WriteElementString("Expected", rule.FieldValue.ToString());
                                    break;
                                }
                            case RuleType.NotEqualTo:
                                {
                                    xmlWriter.WriteElementString("Unexpected", rule.FieldValue.ToString());
                                    break;
                                }
                            case RuleType.InRange:
                                {
                                    valueRange = rule.ExpectedValues;

                                    if ((valueRange != null) && (valueRange.Length > 0))
                                    {
                                        xmlWriter.WriteStartElement("Expected");
                                        for (int j = 0; j < valueRange.Length; j++)
                                        {
                                            xmlWriter.WriteString(valueRange[j].ToString());

                                            if (j != (valueRange.Length - 1))
                                            {
                                                xmlWriter.WriteString(",");
                                            }
                                        }
                                        xmlWriter.WriteEndElement();
                                    }

                                    break;
                                }
                            case RuleType.OutOfRange:
                                {
                                    valueRange = rule.UnexpectedValues;

                                    if ((valueRange != null) && (valueRange.Length > 0))
                                    {
                                        xmlWriter.WriteStartElement("Unexpected");
                                        for (int j = 0; j < valueRange.Length; j++)
                                        {
                                            xmlWriter.WriteString(valueRange[j].ToString());

                                            if (j != (valueRange.Length - 1))
                                            {
                                                xmlWriter.WriteString(",");
                                            }
                                        }
                                        xmlWriter.WriteEndElement();
                                    }
                                    break;
                                }
                            case RuleType.InAndOutOfRange:
                                {
                                    valueRange = rule.ExpectedValues;

                                    if ((valueRange != null) && (valueRange.Length > 0))
                                    {
                                        xmlWriter.WriteStartElement("Expected");
                                        for (int j = 0; j < valueRange.Length; j++)
                                        {
                                            xmlWriter.WriteString(valueRange[j].ToString());

                                            if (j != (valueRange.Length - 1))
                                            {
                                                xmlWriter.WriteString(",");
                                            }
                                        }
                                        xmlWriter.WriteEndElement();
                                    }

                                    valueRange = rule.UnexpectedValues;

                                    if ((valueRange != null) && (valueRange.Length > 0))
                                    {
                                        xmlWriter.WriteStartElement("Unexpected");
                                        for (int j = 0; j < valueRange.Length; j++)
                                        {
                                            xmlWriter.WriteString(valueRange[j].ToString());

                                            if (j != (valueRange.Length - 1))
                                            {
                                                xmlWriter.WriteString(",");
                                            }
                                        }
                                        xmlWriter.WriteEndElement();
                                    }

                                    break;
                                }
                            case RuleType.StringLength:
                                {
                                    xmlWriter.WriteElementString("Min", rule.MinValue.ToString());
                                    xmlWriter.WriteElementString("Max", rule.MaxValue.ToString());
                                    break;
                                }
                            case RuleType.Min:
                                {
                                    xmlWriter.WriteElementString("Min", rule.MinValue.ToString());
                                    break;
                                }
                            case RuleType.Max:
                                {
                                    xmlWriter.WriteElementString("Max", rule.MaxValue.ToString());
                                    break;
                                }
                            case RuleType.MinAndMax:
                                {
                                    xmlWriter.WriteElementString("Min", rule.MinValue.ToString());
                                    xmlWriter.WriteElementString("Max", rule.MaxValue.ToString());
                                    break;
                                }
                            case RuleType.NotNull:
                                break;
                            case RuleType.Reference:
                                {
                                    if (resultObjects[rule.ReferenceFieldName] != null && resultObjects[rule.ReferenceFieldName].Count > 0)
                                    {
                                        xmlWriter.WriteElementString(rule.ReferenceFieldName, resultObjects[rule.ReferenceFieldName][0].FieldValue.ToString());
                                    }
                                    
                                    break;
                                }
                            case RuleType.NumberSequenceComparison:
                                {
                                    xmlWriter.WriteElementString("Expected", rule.FieldValue.ToString());
                                    break;
                                }
                            case RuleType.Occurrence:
                                {
                                    //xmlWriter.WriteElementString("Expected", rule.FieldValue.ToString());
                                    xmlWriter.WriteElementString("Min", rule.MinValue.ToString());
                                    xmlWriter.WriteElementString("Max", rule.MaxValue.ToString());
                                    break;
                                }
                            default:
                                break;
                        }

                        if (i == 0)
                        {
                            //Value
                            xmlWriter.WriteStartElement("Value");

                            if (result.FieldValue != null)
                            {
                                if (rule.RuleType == RuleType.Occurrence)
                                {
                                    if (result.FieldValue is IDictionary<string, string>)
                                    {
                                        xmlWriter.WriteString((result.FieldValue as IDictionary<string, string>).Keys.Count.ToString());
                                    }
                                }
                                else
                                {
                                    xmlWriter.WriteString(result.FieldValue.ToString());
                                }
                            }

                            xmlWriter.WriteEndElement();                     
                        }
                        
                        //Detail
                        xmlWriter.WriteStartElement("Detail");

                        if ((result.Description != null) && (result.Description is IDictionary<string, object>))
                        {
                            resultDescription = (result.Description as IDictionary<string, object>);

                            foreach (string key in resultDescription.Keys)
                            {
                                xmlWriter.WriteElementString(key, resultDescription[key].ToString());
                            }
                        }

                        if (result.FieldName == "NIC")
                        {
                            if ((result.FieldValue != null) && (result.FieldValue is IDictionary<string, string>))
                            {
                                nicInfo = (result.FieldValue as IDictionary<string, string>);

                                foreach (string key in nicInfo.Keys)
                                {
                                    xmlWriter.WriteElementString(("PhysicalMedium_" + nicInfo[key]), key.ToString());
                                }
                            }
                        }

                        xmlWriter.WriteEndElement(); //Detail
                    }

                    xmlWriter.WriteEndElement();//FieldName
                }

                xmlWriter.WriteEndElement();//TestItems

                xmlWriter.WriteEndDocument();

                xmlWriter.Flush();

                xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(stringWriter.ToString());

                resultXml = xmlDoc.InnerXml;
            }

            return resultXml;
        }
    }
}
