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
                //returnValue = JsonUtility.JsonDeserialize(bytes, typeof(ValidationRuleItem[]), new Type[] {typeof(ValidationRuleItem), typeof(Int32)}, "root") as ValidationRuleItem[];

                string jsonString = Encoding.UTF8.GetString(bytes);

                returnValue = JsonUtility.GetObjectFromJson(jsonString, typeof(ValidationRuleItem[]), false, "[", "]") as ValidationRuleItem[];
            }

            return returnValue;
        }
    }
}
