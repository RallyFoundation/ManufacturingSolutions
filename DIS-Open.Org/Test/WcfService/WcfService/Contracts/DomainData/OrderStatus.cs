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

namespace WcfService.Contracts.DomainData
{
    /// <summary>
    /// OrderStatus class is used to populate the OrderStatus relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class OrderStatus
    {
        /// <summary>
        /// Gets or Sets the OrderStatusID
        /// </summary>
        [DataMember(Order = 1)]
        public int OrderStatusID { get; set; }
        /// <summary>
        /// Gets or Sets the OrderStatusName
        /// </summary>
        [DataMember(Order = 2)]
        public string OrderStatusName { get; set; }
    }
}
