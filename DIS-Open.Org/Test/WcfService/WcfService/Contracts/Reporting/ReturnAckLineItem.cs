using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace WcfService.Contracts
{
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ReturnAckLineItem
    {
        [DataMember(Order = 1)]
        public int MSReturnLineNumber { get; set; }

        [DataMember(Order = 2)]
        public string OEMRMALineNumber { get; set; }

        [DataMember(Order = 3)]
        public string ReturnTypeID { get; set; }

        [DataMember(Order = 4)]
        public long ProductKeyID { get; set; }

        [DataMember(Order = 5)]
        public string LicensablePartNumber { get; set; }

        [DataMember(Order = 6)]
        public string ReturnReasonCode { get; set; }

        [DataMember(Order = 7)]
        public string ReturnReasonCodeDescription { get; set; }
    }
}