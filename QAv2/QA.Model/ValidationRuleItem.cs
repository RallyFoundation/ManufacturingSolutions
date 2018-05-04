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
        [DataMember(Name = "FieldName")]
        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        [DataMember(Name = "GroupName")]
        [JsonProperty("GroupName")]
        public string GroupName { get; set; }

        [DataMember(Name ="RuleType")]
        [JsonProperty("RuleType")]
        public RuleType RuleType { get; set; }

        [DataMember(Name = "FieldValue")]
        [JsonProperty("FieldValue")]
        public object FieldValue { get; set; }

        [DataMember(Name = "FieldAltValue")]
        [JsonProperty("FieldAltValue")]
        public object FieldAltValue { get; set; }

        [DataMember(Name = "MinValue")]
        [JsonProperty("MinValue")]
        public int MinValue { get; set; }

        [DataMember(Name = "MaxValue")]
        [JsonProperty("MaxValue")]
        public int MaxValue { get; set; }

        [DataMember(Name = "ExpectedValues")]
        [JsonProperty("ExpectedValues")]
        public string[] ExpectedValues { get; set; }

        [DataMember(Name = "UnexpectedValues")]
        [JsonProperty("UnexpectedValues")]
        public string[] UnexpectedValues { get; set; }
    }
}
