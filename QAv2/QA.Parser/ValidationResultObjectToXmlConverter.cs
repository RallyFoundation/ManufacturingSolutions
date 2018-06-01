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

            if (Data == null || (!(Data is IDictionary<string, ICollection<Result>>)))
            {
                return null;
            }

            XmlWriterSettings settings = new XmlWriterSettings() {
                 Encoding = Encoding.UTF8,
            };

            IDictionary<string, bool> resultSummary = (Data as object[])[0] as IDictionary<string, bool>;

            IDictionary<string, IList<Result>> resultObjects = ((Data as object[])[1] as IDictionary<string, IList<Result>>);

            IDictionary<string, object> resultDescription = null;

            ValidationRuleItem rule = null;

            Result result = null;

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
                                break;
                            case RuleType.OutOfRange:
                                break;
                            case RuleType.InAndOutOfRange:
                                break;
                            case RuleType.StringLength:
                                break;
                            case RuleType.Min:
                                break;
                            case RuleType.Max:
                                break;
                            case RuleType.MinAndMax:
                                break;
                            case RuleType.NotNull:
                                break;
                            case RuleType.Reference:
                                break;
                            case RuleType.NumberSequenceComparison:
                                break;
                            case RuleType.Occurrence:
                                break;
                            default:
                                break;
                        }

                        if (i == 0)
                        {
                            //Value
                            xmlWriter.WriteElementString("Value", result.FieldValue.ToString());
                        }
                        

                        //Detail
                        xmlWriter.WriteStartElement("Detail");

                        if ((result.Description != null) && (result.Description is IDictionary<string, object>))
                        {
                            resultDescription = (resultDescription as IDictionary<string, object>);

                            foreach (string key in resultDescription.Keys)
                            {
                                xmlWriter.WriteElementString(key, resultDescription[key].ToString());
                            }
                        }

                        xmlWriter.WriteEndElement(); //Detail
                    }

                    xmlWriter.WriteEndElement();//FieldName
                }

                xmlWriter.WriteEndElement();//TestItems

                xmlWriter.WriteEndDocument();
            }

            return resultXml;
        }
    }
}
