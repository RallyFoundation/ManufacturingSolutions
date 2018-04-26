using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace QA.Model
{
    [DataContract]
    [JsonObject]
    public class ValidationRuleItem
    {
        [DataMember(Name = "FiledName")]
        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        [DataMember(Name ="RuleType")]
        [JsonProperty("RuleType")]
        public RuleType RuleType { get; set; }

        [DataMember(Name = "FieldValue")]
        [JsonProperty("FieldValue")]
        public object FieldValue { get; set; }

        [DataMember(Name = "FieldAltValue")]
        [JsonProperty("FieldAltValue")]
        public object FieldAltValue { get; set; }
    }
}
