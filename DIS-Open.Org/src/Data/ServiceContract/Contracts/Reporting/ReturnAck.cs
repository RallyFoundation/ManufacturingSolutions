//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
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
    /// ReturnAck class the revelant fields that the caller has to populate and submit.
    /// </summary>
    [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
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
