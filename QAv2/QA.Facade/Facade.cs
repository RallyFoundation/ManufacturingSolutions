using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using QA.Core;
//using QA.Parser;
using QA.Rule;
using QA.Utility;
using QA.Model;
using QA.Mapper;
using QA.Reducer;

namespace QA.Facade
{
    public class Facade
    {
        public static Dictionary<string, Dictionary<string, List<IRule>>> Rules;
        public static Dictionary<string, object> Data;
        public static Dictionary<string, bool> Results;
        public static Dictionary<string, Dictionary<string, List<bool>>> GroupedResults;
        public static Dictionary<string, List<Result>> ResultDetails;

        public static void InitializeRules()
        {
            string ruleConfPath = Global.DefaultRuleConfigPath;

            //IParser validationRuleJsonParser = new ValidationRuleConfigurationJsonParser();

            IMapper ruleJsonMapper = new RuleJsonMapper();

            byte[] ruleBytes = new byte[1024];

            string ruleString = "";

            using (FileStream stream = new FileStream(ruleConfPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    ruleString = reader.ReadToEnd();
                }
            }

            ruleString = ruleString.Substring(ruleString.IndexOf("["));
            ruleString = ruleString.Substring(0, (ruleString.LastIndexOf("]") + 1));
            ruleBytes = Encoding.UTF8.GetBytes(ruleString);

            ValidationRuleItem[] ruleItems = ruleJsonMapper.Map(ruleBytes) as ValidationRuleItem[]; //validationRuleJsonParser.Parse(ruleBytes) as ValidationRuleItem[];

            if (ruleItems != null)
            {
                Rules = new Dictionary<string, Dictionary<string, List<IRule>>>();

                IRule rule = null;

                for (int i = 0; i < ruleItems.Length; i++)
                {
                    rule = null;

                    if (ruleItems[i] != null)
                    {
                        switch (ruleItems[i].RuleType)
                        {
                            case RuleType.EqualTo:
                                {
                                    rule = new EqualToRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        ExpectedValue = ruleItems[i].FieldValue.ToString()
                                    };
                                    break;
                                }
                            case RuleType.NotEqualTo:
                                {
                                    rule = new NotEqualToRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        UnexpectedValue = ruleItems[i].FieldValue.ToString()
                                    };
                                    break;
                                }
                            case RuleType.InRange:
                                {
                                    rule = new InRangeRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        ExpectedValueRange = ruleItems[i].ExpectedValues
                                    };
                                    break;
                                }
                            case RuleType.OutOfRange:
                                {
                                    rule = new OutOfRangeRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        UnexpectedValueRange = ruleItems[i].UnexpectedValues
                                    };
                                    break;
                                }
                            case RuleType.InAndOutOfRange:
                                {
                                    rule = new InAndOutOfRangeRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        ExpectedValueRange = ruleItems[i].ExpectedValues,
                                        UnexpectedValueRange = ruleItems[i].UnexpectedValues
                                    };
                                    break;
                                }
                            case RuleType.StringLength:
                                {
                                    rule = new StringLengthRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        MinValue = ruleItems[i].MinValue,
                                        MaxValue = ruleItems[i].MaxValue
                                    };
                                    break;
                                }
                            case RuleType.Min:
                                {
                                    rule = new MinRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        MinValue = ruleItems[i].MinValue
                                    };
                                    break;
                                }
                            case RuleType.Max:
                                {
                                    rule = new MaxRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        MaxValue = ruleItems[i].MaxValue
                                    };
                                    break;
                                }
                            case RuleType.MinAndMax:
                                {
                                    rule = new MinAndMaxRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        MinValue = ruleItems[i].MinValue,
                                        MaxValue = ruleItems[i].MaxValue
                                    };
                                    break;
                                }
                            case RuleType.NotNull:
                                {
                                    rule = new NotNullRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName
                                    };
                                    break;
                                }
                            case RuleType.Reference:
                                {
                                    rule = new ReferenceRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        ReferenceFieldName = ruleItems[i].ReferenceFieldName,
                                        ExpectedValue = ruleItems[i].FieldValue.ToString()
                                    };
                                    break;
                                }
                            case RuleType.NumberSequenceComparison:
                                {
                                    rule = new NumberSequenceComparisonRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        Separator = ruleItems[i].SequenceSeparator,
                                        Index = ruleItems[i].SequenceIndex,
                                        ExpectedValue = ruleItems[i].FieldValue.ToString()
                                    };
                                    break;
                                }
                            case RuleType.Occurrence:
                                {
                                    rule = new OccurrenceRule()
                                    {
                                        FieldName = ruleItems[i].FieldName,
                                        GroupName = ruleItems[i].GroupName,
                                        MinValue = ruleItems[i].MinValue,
                                        MaxValue = ruleItems[i].MaxValue,
                                    };

                                    break;
                                }
                            default:
                                break;
                        }

                        if (rule != null)
                        {
                            rule.QuotedFields = ruleItems[i].QuotedFields;

                            if (!Rules.ContainsKey(rule.FieldName))
                            {
                                Rules.Add(rule.FieldName, new Dictionary<string, List<IRule>>()
                                {
                                    { rule.GroupName, new List<IRule>(new IRule[] { rule }) }
                                });
                            }
                            else if(!Rules[rule.FieldName].ContainsKey(rule.GroupName))
                            {
                                Rules[rule.FieldName].Add(rule.GroupName, new List<IRule>(new IRule[] { rule }));
                            }
                            else
                            {
                                Rules[rule.FieldName][rule.GroupName].Add(rule);
                            }
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

            //IParser parser = new Decoded4KHHXmlParser();
            //Data = parser.Parse(data) as Dictionary<string, object>;

            IMapper mapper = new Decoded4KHHXmlMapper();
            Data = mapper.Map(data) as Dictionary<string, object>;
        }

        public static void ValidateData()
        {
            if ((Data != null) && (Rules != null))
            {
                Results = new Dictionary<string, bool>();
                GroupedResults = new Dictionary<string, Dictionary<string, List<bool>>>();
                ResultDetails = new Dictionary<string, List<Result>>();

                bool result;

                object resultDetail = null;

                foreach (string field in Data.Keys)
                {
                    if (Rules.ContainsKey(field))
                    {
                        foreach (string group in Rules[field].Keys)
                        {
                            if (Rules[field].ContainsKey(group))
                            {
                                foreach (IRule rule in Rules[field][group])
                                {
                                    result = rule.Check(Data, out resultDetail);

                                    if (!GroupedResults.ContainsKey(field))
                                    {
                                        GroupedResults.Add(field, new Dictionary<string, List<bool>>() {{ group, new List<bool>() { result} }});
                                    }
                                    else if (!GroupedResults[field].ContainsKey(group))
                                    {
                                        GroupedResults[field].Add(group, new List<bool>() { result });
                                    }
                                    else
                                    {
                                        GroupedResults[field][group].Add(result);
                                    }

                                    if (!ResultDetails.ContainsKey(field))
                                    {
                                        ResultDetails.Add(field, new List<Result>());
                                    }

                                    if (resultDetail != null)
                                    {
                                        ResultDetails[field].Add((resultDetail as Result));
                                    }
                                }
                            }                          
                        }
                    }             
                }

                IReducer reducer = new ResultReducer();

                reducer.Reduce(Results, GroupedResults);

                IReducer oa3ToolReducer = new OA3ToolReducer();
                oa3ToolReducer.Reduce(Results, ResultDetails);
            }
        }

        //public static string GetResultXml(object[] ResultObjects)
        //{
        //    IParser parser = new ValidationResultObjectToXmlConverter();
        //    object result = parser.Parse(ResultObjects);
        //    return result.ToString();
        //}

        public static string OutputResultXml()
        {
            IReducer xmlReducer = new ValidationResultXmlReducer();
            object result = xmlReducer.Reduce(Results, ResultDetails);
            return result.ToString();
        }

        public static void SetData(string FieldName, object FieldValue)
        {
            if (Data == null)
            {
                Data = new Dictionary<string, object>();
            }

            if (!Data.ContainsKey(FieldName))
            {
                Data.Add(FieldName, FieldValue);
            }
            else
            {
                Data[FieldName] = FieldValue;
            }
        }

        public static void AddRule(ValidationRuleItem RuleItem)
        {
            IRule rule = null;

            switch (RuleItem.RuleType)
            {
                case RuleType.EqualTo:
                    {
                        rule = new EqualToRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            ExpectedValue = RuleItem.FieldValue.ToString()
                        };
                        break;
                    }
                case RuleType.NotEqualTo:
                    {
                        rule = new NotEqualToRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            UnexpectedValue = RuleItem.FieldValue.ToString()
                        };
                        break;
                    }
                case RuleType.InRange:
                    {
                        rule = new InRangeRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            ExpectedValueRange = RuleItem.ExpectedValues
                        };
                        break;
                    }
                case RuleType.OutOfRange:
                    {
                        rule = new OutOfRangeRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            UnexpectedValueRange = RuleItem.UnexpectedValues
                        };
                        break;
                    }
                case RuleType.InAndOutOfRange:
                    {
                        rule = new InAndOutOfRangeRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            ExpectedValueRange = RuleItem.ExpectedValues,
                            UnexpectedValueRange = RuleItem.UnexpectedValues
                        };
                        break;
                    }
                case RuleType.StringLength:
                    {
                        rule = new StringLengthRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            MinValue = RuleItem.MinValue,
                            MaxValue = RuleItem.MaxValue
                        };
                        break;
                    }
                case RuleType.Min:
                    {
                        rule = new MinRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            MinValue = RuleItem.MinValue
                        };
                        break;
                    }
                case RuleType.Max:
                    {
                        rule = new MaxRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            MaxValue = RuleItem.MaxValue
                        };
                        break;
                    }
                case RuleType.MinAndMax:
                    {
                        rule = new MinAndMaxRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            MinValue = RuleItem.MinValue,
                            MaxValue = RuleItem.MaxValue
                        };
                        break;
                    }
                case RuleType.NotNull:
                    {
                        rule = new NotNullRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName
                        };
                        break;
                    }
                case RuleType.Reference:
                    {
                        rule = new ReferenceRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            ReferenceFieldName = RuleItem.ReferenceFieldName,
                            ExpectedValue = RuleItem.FieldValue.ToString()
                        };
                        break;
                    }
                case RuleType.NumberSequenceComparison:
                    {
                        rule = new NumberSequenceComparisonRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            Separator = RuleItem.SequenceSeparator,
                            Index = RuleItem.SequenceIndex,
                            ExpectedValue = RuleItem.FieldValue.ToString()
                        };
                        break;
                    }
                case RuleType.Occurrence:
                    {
                        rule = new OccurrenceRule()
                        {
                            FieldName = RuleItem.FieldName,
                            GroupName = RuleItem.GroupName,
                            MinValue = RuleItem.MinValue,
                            MaxValue = RuleItem.MaxValue,
                        };

                        break;
                    }
                default:
                    break;
            }

            if (Rules == null)
            {
                Rules = new Dictionary<string, Dictionary<string, List<IRule>>>();
            }

            if (rule != null)
            {
                rule.QuotedFields = RuleItem.QuotedFields;

                if (!Rules.ContainsKey(rule.FieldName))
                {
                    Rules.Add(rule.FieldName, new Dictionary<string, List<IRule>>()
                                {
                                    { rule.GroupName, new List<IRule>(new IRule[] { rule }) }
                                });
                }
                else if (!Rules[rule.FieldName].ContainsKey(rule.GroupName))
                {
                    Rules[rule.FieldName].Add(rule.GroupName, new List<IRule>(new IRule[] { rule }));
                }
                else
                {
                    Rules[rule.FieldName][rule.GroupName].Add(rule);
                }
            }
        }
    }
}
