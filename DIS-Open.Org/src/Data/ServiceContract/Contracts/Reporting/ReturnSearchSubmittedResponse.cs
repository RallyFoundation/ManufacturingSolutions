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
    [DataContract(Name = "ReturnSearchSubmittedResponse", Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
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
