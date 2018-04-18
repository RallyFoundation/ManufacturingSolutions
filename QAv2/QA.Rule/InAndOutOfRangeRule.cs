using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;

namespace QA.Rule
{
    public class InAndOutOfRangeRule : IRule
    {
        public string FieldName { get; set; }

        public string[] ExpectedValueRange { get; set; }

        public string[] UnexpectedValueRange { get; set; }

        public bool Check(IDictionary<string, object> Pairs)
        {
            try
            {
                if (Pairs == null)
                {
                    return false;
                }

                if (!Pairs.ContainsKey(FieldName))
                {
                    return false;
                }

                return ExpectedValueRange.Contains((string)Pairs[FieldName]) && !UnexpectedValueRange.Contains((string)Pairs[FieldName]);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
