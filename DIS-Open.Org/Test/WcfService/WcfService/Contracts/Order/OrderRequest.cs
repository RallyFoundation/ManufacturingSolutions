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
    /// Data Contract for OrderRequest class,the revelant fields that the caller has to populate and submit.
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class OrderRequest
    {
        /// <summary>
        /// Gets or sets the reference number.
        /// </summary>
        /// <value>The reference number.</value>
        [DataMember(Order = 1)]
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the order type ID.
        /// </summary>
        /// <value>The order type ID.</value>
        [DataMember(Order = 2)]
        public int OrderTypeID { get; set; }

        /// <summary>
        /// Gets or sets the contract number.
        /// </summary>
        /// <value>The contract number.</value>
        [DataMember(Order = 3)]
        public string ContractNumber { get; set; }

        /// <summary>
        /// Gets or sets the OEMPO number.
        /// </summary>
        /// <value>The OEMPO number.</value>
        [DataMember(Order = 4)]
        public string OEMPONumber { get; set; }

        /// <summary>
        /// Gets or sets the OEMPO date.
        /// </summary>
        /// <value>The OEMPO date.</value>
        [DataMember(Order = 5)]
        public DateTime OEMPODate { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        /// <value>The order date.</value>
        [DataMember(Order = 6)]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the order line item.
        /// </summary>
        /// <value>The order line item.</value>
        [DataMember(Order = 7)]
        public OrderLineItem[] OrderLineItems { get; set; }

        /// <summary>
        /// Gets or sets the order participants.
        /// </summary>
        /// <value>The order participants.</value>
        [DataMember(Order = 8)]
        public OrderParticipant[] OrderParticipants { get; set; }
    }
}
