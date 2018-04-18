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

        public void InitializeRules()
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

            using (FileStream stream = new FileStream(ruleConfPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ruleBytes = new byte[stream.Length];
                stream.Read(ruleBytes, 0, ((int)(stream.Length)));
            }

             ValidationRuleItem[] ruleItems = validationRuleJsonParser.Parse(ruleBytes) as ValidationRuleItem[];

            if (ruleItems != null)
            {
                for (int i = 0; i < ruleItems.Length; i++)
                {
                    if (ruleItems[i] != null)
                    {
                        switch (ruleItems[i].RuleType)
                        {
                            case RuleType.EqualTo:
                                break;
                            case RuleType.NotEqualTo:
                                break;
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
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public void InstantiateInputData()
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

        public void ValidateData()
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
