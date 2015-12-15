#region Declaratives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace WcfService.Contracts
{
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ReturnLineItem
    {
        [DataMember(Order = 1)]
        public int OEMRMALineNumber { get; set; }

        [DataMember(Order = 2)]
        public string ReturnTypeID { get; set; }

        [DataMember(Order = 3)]
        public long ProductKeyID { get; set; }
    }
}