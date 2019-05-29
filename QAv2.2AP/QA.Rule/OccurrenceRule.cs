using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;
using QA.Model;

namespace QA.Rule
{
    public class OccurrenceRule : IRule
    {
        public string FieldName { get; set; }
        public string GroupName { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string[] QuotedFields { get; set; }

        public bool Check(IDictionary<string, object> Pairs, out object Result)
        {
            Result result = new Result()
            {
                FieldName = FieldName,
                RuleType = RuleType.Occurrence,
                RuleInstance = new ValidationRuleItem()
                {
                    FieldName = FieldName,
                    GroupName = GroupName,
                    RuleType = RuleType.Occurrence,
                    MaxValue = MaxValue,
                    MinValue = MinValue,
                    QuotedFields = QuotedFields
                }
            };

            Result = result;

            try
            {
                if (Pairs == null)
                {
                    result.IsPassed = false;
                    result.FieldValue = -1;
                    return false;
                }

                if (!Pairs.ContainsKey(FieldName))
                {
                    result.IsPassed = false;
                    result.FieldValue = new Dictionary<string, string>();
                    return false;
                }

                if (Pairs[FieldName] == null)
                {
                    result.IsPassed = false;
                    result.FieldValue = new Dictionary<string, string>();
                    return false;
                }

                //if (!(Pairs[FieldName] is ICollection))
                //{
                //    result.IsPassed = false;
                //    result.FieldValue = -2;
                //    return false;
                //}

                result.FieldValue = Pairs[FieldName];

                //int occurrence = ((ICollection)Pairs[FieldName]).Count;

                int occurrence = (Pairs[FieldName] is ICollection) ? ((ICollection)Pairs[FieldName]).Count : 1;

                result.IsPassed = ((occurrence >= MinValue) && (occurrence <= MaxValue));

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
