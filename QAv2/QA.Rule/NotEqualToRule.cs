using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;

namespace QA.Rule
{
    public class NotEqualToRule : IRule
    {
        public string FieldName { get; set; }

        public string GroupName { get; set; }

        public string UnexpectedValue { get; set; }

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

                return ((string)Pairs[FieldName] != UnexpectedValue);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
