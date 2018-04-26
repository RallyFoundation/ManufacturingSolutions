using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using QA.Core;
using QA.Parser;
using QA.Rule;
using QA.Utility;
using QA.Model;

namespace QA.Facade
{
    public class Facade
    {
        public static Dictionary<string, IRule> Rules;

        public static Dictionary<string, object> Data;

        public static Dictionary<string, bool> Result;

        public static void InitializeRules()
        {
            string ruleConfPath = Global.DefaultRuleConfigPath;

            //XmlDocument xmlDocument = new XmlDocument();

            //xmlDocument.Load(ruleConfPath);

            //List<IRule> rules = XmlUtility.XmlDeserialize(xmlDocument.InnerXml, typeof(List<IRule>), new Type[] {typeof(EqualToRule), typeof(NotEqualToRule), typeof(NotNullRule), typeof(MaxRule), typeof(MinRule), typeof(MinAndMaxRule), typeof(StringLengthRule), typeof(InRangeRule), typeof(OutOfRangeRule), typeof(InAndOutOfRangeRule) }, "utf-8") as List<IRule>;

            //if (rules != null)
            //{
            //    Rules = new Dictionary<string, IRule>();

            //    foreach (var rule in rules)
            //    {
            //        Rules.Add(rule.FieldName, rule);
            //    }
            //}

            IParser validationRuleJsonParser = new ValidationRuleConfigurationJsonParser();

            byte[] ruleBytes = new byte[1024];

            string ruleString = "";

            using (FileStream stream = new FileStream(ruleConfPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //ruleBytes = new byte[stream.Length];
                //stream.Read(ruleBytes, 0, ((int)(stream.Length)));

                using (StreamReader reader = new StreamReader(stream))
                {
                    ruleString = reader.ReadToEnd();
                }
            }

            ruleString = ruleString.Substring(ruleString.IndexOf("["));

            ruleString = ruleString.Substring(0, (ruleString.LastIndexOf("]") + 1));

            ruleBytes = Encoding.UTF8.GetBytes(ruleString);

            ValidationRuleItem[] ruleItems = validationRuleJsonParser.Parse(ruleBytes) as ValidationRuleItem[];

            if (ruleItems != null)
            {
                Rules = new Dictionary<string, IRule>();

                int min = -1, max = -1;

                for (int i = 0; i < ruleItems.Length; i++)
                {
                    if ((ruleItems[i] != null) && (!Rules.ContainsKey(ruleItems[i].FieldName)))
                    {
                        switch (ruleItems[i].RuleType)
                        {
                            case RuleType.EqualTo:
                                {
                                    EqualToRule rule = new EqualToRule(){
                                        FieldName = ruleItems[i].FieldName,
                                        ExpectedValue = ruleItems[i].FieldValue.ToString()
                                    };

                                    Rules.Add(rule.FieldName, rule);

                                    break;
                                }
                            case RuleType.NotEqualTo:
                                {
                                    NotEqualToRule rule = new NotEqualToRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        UnexpectedValue = ruleItems[i].FieldValue.ToString()
                                    };

                                    Rules.Add(rule.FieldName, rule);
                                    break;
                                }
                            case RuleType.InRange:
                                {
                                    InRangeRule rule = new InRangeRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        ExpectedValueRange = ruleItems[i].ExpectedValues
                                    };

                                    Rules.Add(rule.FieldName, rule);
                                    break;
                                }
                            case RuleType.OutOfRange:
                                {
                                    OutOfRangeRule rule = new OutOfRangeRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        UnexpectedValueRange = ruleItems[i].UnexpectedValues
                                    };

                                    Rules.Add(rule.FieldName, rule);
                                    break;
                                }
                            case RuleType.InAndOutOfRange:
                                {
                                    InAndOutOfRangeRule rule = new InAndOutOfRangeRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        ExpectedValueRange = ruleItems[i].ExpectedValues,
                                        UnexpectedValueRange = ruleItems[i].UnexpectedValues
                                    };

                                    Rules.Add(rule.FieldName, rule);
                                    break;
                                }
                            case RuleType.StringLength:
                                {
                                    if (int.TryParse(ruleItems[i].FieldValue.ToString(), out min) && int.TryParse(ruleItems[i].FieldAltValue.ToString(), out max))
                                    {
                                        StringLengthRule rule = new StringLengthRule()
                                        {
                                            FieldName = ruleItems[i].FieldName,
                                            MinValue = min,
                                            MaxValue = max
                                        };

                                        Rules.Add(rule.FieldName, rule);
                                    }

                                    break;
                                }
                            case RuleType.Min:
                                {
                                    if (int.TryParse(ruleItems[i].FieldValue.ToString(), out min))
                                    {
                                        MinRule rule = new MinRule()
                                        {
                                            FieldName = ruleItems[i].FieldName,
                                            MinValue = min
                                        };

                                        Rules.Add(rule.FieldName, rule);
                                    }
                                    
                                    break;
                                }
                            case RuleType.Max:
                                {
                                    if (int.TryParse(ruleItems[i].FieldAltValue.ToString(), out max))
                                    {
                                        MaxRule rule = new MaxRule()
                                        {
                                            FieldName = ruleItems[i].FieldName,
                                            MaxValue = max
                                        };

                                        Rules.Add(rule.FieldName, rule);
                                    }
                                    
                                    break;
                                }
                            case RuleType.MinAndMax:
                                {
                                    if ((int.TryParse(ruleItems[i].FieldValue.ToString(), out min) && int.TryParse(ruleItems[i].FieldAltValue.ToString(), out max)))
                                    {
                                        MinAndMaxRule rule = new MinAndMaxRule()
                                        {
                                            FieldName = ruleItems[i].FieldName,
                                            MinValue = min,
                                            MaxValue = max
                                        };

                                        Rules.Add(rule.FieldName, rule);
                                    }
                                    
                                    break;
                                }
                            case RuleType.NotNull:
                                {
                                    NotNullRule rule = new NotNullRule()
                                    {
                                        FieldName = ruleItems[i].FieldName
                                    };

                                    Rules.Add(rule.FieldName, rule);
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public static void InstantiateInputData()
        {
            string data;

            using (FileStream stream = new FileStream(Global.DefaultDataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = reader.ReadToEnd();
                }
            }

            IParser parser = new Decoded4KHHXmlParser();

            Data = parser.Parse(data) as Dictionary<string, object>;
        }

        public static void ValidateData()
        {
            if ((Data != null) && (Rules != null))
            {
                Result = new Dictionary<string, bool>();

                bool result;

                foreach (string key in Data.Keys)
                {
                    result = Rules[key].Check(Data);
                    Result.Add(key, result);
                }
            }
        }
    }
}
