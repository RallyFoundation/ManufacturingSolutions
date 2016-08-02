using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Platform.DAAS.OData.Core.Security
{
    [DataContract]
    public class AuthItem
    {
        [DataMember]
        public string Identity { get; set; }

        [DataMember]
        public string Crypto{ get; set; }

        [DataMember(IsRequired =false)]
        public string[] Arguments { get; set; }
    }
}
