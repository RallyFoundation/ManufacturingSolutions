using System;
using System.Runtime.Serialization;

namespace WcfService.Contracts
{
    [DataContract(Name = "SearchSubmittedResponse", Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class CbrSearchSubmittedResponse
    {
        [DataMember(Order = 1)]
        public Guid MSReportUniqueID { get; set; }
        [DataMember(Order = 2)]
        public Guid CustomerReportUniqueID { get; set; }
        [DataMember(Order = 3)]
        public DateTime ReportReceiptDateUTC { get; set; }
    }
}