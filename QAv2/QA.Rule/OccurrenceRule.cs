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
                RuleType = RuleType.Occurrence
            };

            Result = result;

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

                if (!(Pairs[FieldName] is ICollection))
                {
                    result.IsPassed = false;
                    return false;
                }

                result.FieldValue = Pairs[FieldName];

                int occurrence = ((ICollection)Pairs[FieldName]).Count;

                result.IsPassed = ((occurrence > MinValue) && (occurrence <= MaxValue));

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
