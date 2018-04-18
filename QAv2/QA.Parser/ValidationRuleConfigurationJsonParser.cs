using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;
using QA.Model;
using QA.Utility;

namespace QA.Parser
{
    public class ValidationRuleConfigurationJsonParser : IParser
    {
        public object Parse(object Data)
        {
            ValidationRuleItem[] returnValue = null;

            byte[] bytes = (byte[])(Data);

            if ((bytes != null) && (bytes.Length > 0))
            {
               returnValue = JsonUtility.JsonDeserialize(bytes, typeof(ValidationRuleItem[]), new Type[] {typeof(ValidationRuleItem)}, "root") as ValidationRuleItem[];
            }

            return returnValue;
        }
    }
}
