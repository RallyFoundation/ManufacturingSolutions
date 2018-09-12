using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;
using QA.Model;
using QA.Utility;

namespace QA.Mapper
{
    public class RuleJsonMapper : IMapper
    {
        public object Map(object Data)
        {
            ValidationRuleItem[] returnValue = null;

            byte[] bytes = (byte[])(Data);

            if ((bytes != null) && (bytes.Length > 0))
            {
                string jsonString = Encoding.UTF8.GetString(bytes);

                returnValue = JsonUtility.GetObjectFromJson(jsonString, typeof(ValidationRuleItem[]), false, "[", "]") as ValidationRuleItem[];
            }

            return returnValue;
        }
    }
}
