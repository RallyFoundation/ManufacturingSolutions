using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;

namespace QA.Rule
{
    public class MinAndMaxRule : IRule
    {
        public string FieldName { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

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

                return ((int)Pairs[FieldName] <= MaxValue) && ((int)Pairs[FieldName] >= MinValue);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
