using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WcfService.Contracts
{
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ReturnAck
    {
        [DataMember(Order = 1)]
        public Guid ReturnUniqueID { get; set; }

        [DataMember(Order = 2)]
        public string MSReturnNumber { get; set; }

        [DataMember(Order = 3)]
        public DateTime ReturnDateUTC { get; set; }

        [DataMember(Order = 4)]
        public string OEMRMANumber { get; set; }

        [DataMember(Order = 5)]
        public DateTime OEMRMADateUTC { get; set; }

        [DataMember(Order = 6)]
        public string SoldToCustomerID { get; set; }

        [DataMember(Order = 7)]
        public string SoldToCustomerName { get; set; }

        [DataMember(Order = 8)]
        public ReturnAckLineItem[] ReturnAckLineItems { get; set; }
    }
}