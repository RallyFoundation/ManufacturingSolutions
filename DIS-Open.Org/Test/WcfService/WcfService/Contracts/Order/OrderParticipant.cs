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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WcfService.Contracts
{
    /// <summary>
    /// Data Contract class to describe the OrderParticipant within a OrderRequest
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class OrderParticipant
    {
        /// <summary>
        /// Gets or sets the customer number.
        /// </summary>
        /// <value>The customer number.</value>
        [DataMember(Order = 1)]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// Gets or sets the partner function.
        /// </summary>
        /// <value>The partner function.</value>
        [DataMember(Order = 2)]
        public string PartnerFunction { get; set; }
    }
}
