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

#region Declaratives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace WcfService.Contracts {
    /// <summary>
    /// ReturnRequest class the revelant fields that the caller has to populate and submit.
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ReturnRequest {

        [DataMember(Order = 1)]
        public string OEMRMANumber { get; set; }

        [DataMember(Order = 2)]
        public DateTime OEMRMADate { get; set; }

        [DataMember(Order = 3)]
        public string SoldToCustomerID { get; set; }

        [DataMember(Order = 4)]
        public bool ReturnNoCredit { get; set; }

        [DataMember(Order = 5)]
        public ReturnLineItem[] ReturnLineItems { get; set; }
    }
}
