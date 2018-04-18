using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace QA.Model
{
    [DataContract]
    public class ValidationRuleItem
    {
        [DataMember(Name = "FiledName")]
        public string FiledName { get; set; }

        [DataMember(Name ="RuleType")]
        public RuleType RuleType { get; set; }

        [DataMember(Name = "FieldValue")]
        public object FieldValue { get; set; }

        [DataMember(Name = "FieldAltValue")]
        public object FieldAltValue { get; set; }
    }
}
