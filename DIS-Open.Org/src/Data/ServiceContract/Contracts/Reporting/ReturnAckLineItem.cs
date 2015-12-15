// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Runtime.Serialization;

namespace DIS.Data.ServiceContract
{
    /// <summary>
    /// ReturnAckLineItem class the revelant fields that the caller has to populate and submit.
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ReturnAckLineItem
    {
        [DataMember(Order = 1)]
        public int MSReturnLineNumber { get; set; }

        [DataMember(Order = 2)]
        public int OEMRMALineNumber { get; set; }

        [DataMember(Order = 3)]
        public string ReturnTypeID{ get; set; }

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
