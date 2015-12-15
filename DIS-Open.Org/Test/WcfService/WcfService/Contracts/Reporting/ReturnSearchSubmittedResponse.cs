using System;
using System.Runtime.Serialization;

namespace WcfService.Contracts
{
    [DataContract(Name = "SearchSubmittedResponse", Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ReturnSearchSubmittedResponse
    {
        [DataMember(Order = 1)]
        public Guid ReturnUniqueID { get; set; }
        [DataMember(Order = 2)]
        public DateTime ReturnReceiptDateUTC { get; set; }
        [DataMember(Order = 3)]
        public string OEMRMANumber { get; set; }
        [DataMember(Order = 4)]
        public DateTime OEMRMADateUTC { get; set; }
    }
}