using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;
using QA.Model;

namespace QA.Rule
{
    public class MinRule : IRule
    {
        public string FieldName { get; set; }

        public string GroupName { get; set; }

        public int MinValue { get; set; }

        public string[] QuotedFields { get; set; }

        public bool Check(IDictionary<string, object> Pairs, out object Result)
        {
            Result result = new Result()
            {
                FieldName = FieldName,
                RuleType = RuleType.Min,
                RuleInstance = new ValidationRuleItem()
                {
                    FieldName = FieldName,
                    GroupName = GroupName,
                    RuleType = RuleType.Min,
                    MinValue = MinValue,
                    QuotedFields = QuotedFields
                }
            };

            Result = result;

            int fieldVal = -1;

            try
            {
                if (Pairs == null)
                {
                    result.IsPassed = false;
                    return false;
                }

                if (!Pairs.ContainsKey(FieldName))
                {
                    result.IsPassed = false;
                    return false;
                }

                if (!int.TryParse(Pairs[FieldName].ToString(), out fieldVal))
                {
                    return false;
                }

                result.FieldValue = fieldVal;
                result.IsPassed = (fieldVal >= MinValue);

                if (QuotedFields != null)
                {
                    result.Description = new Dictionary<string, object>();

                    foreach (string field in QuotedFields)
                    {
                        if (Pairs.ContainsKey(field))
                        {
                            (result.Description as Dictionary<string, object>).Add(field, Pairs[field]);
                        }
                    }
                }

                return result.IsPassed;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
