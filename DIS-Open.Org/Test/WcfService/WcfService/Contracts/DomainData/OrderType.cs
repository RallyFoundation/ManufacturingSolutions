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
    /// OrderType class is used to populate the OrderType relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class OrderType
    {
        /// <summary>
        /// Gets or Sets the OrderTypeID
        /// </summary>
        [DataMember(Order = 1)]
        public int OrderTypeID { get; set; }
        /// <summary>
        /// Gets or Sets the OrderTypeName(Ex: COGS,Sample ) 
        /// </summary>
        [DataMember(Order = 2)]
        public string OrderTypeName { get; set; }
    }
}
