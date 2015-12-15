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
    /// Data Contract class to describe the OrderLineItem within a OrderRequest
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class OrderLineItem
    {
        /// <summary>
        /// Gets or sets the OEM line item number.
        /// </summary>
        /// <value>The OEM line item number.</value>
        [DataMember(Order = 1)]
        public int OEMLineItemNumber { get; set; }

        /// <summary>
        /// Gets or sets the order line number.
        /// </summary>
        /// <value>The order line number.</value>
        [DataMember(Order = 2)]
        public int OrderLineNumber { get; set; }

        /// <summary>
        /// Gets or sets the licensable part number.
        /// </summary>
        /// <value>The licensable part number.</value>
        [DataMember(Order = 3)]
        public string LicensablePartNumber { get; set; }

        /// <summary>
        /// Gets or sets the OEM part number.
        /// </summary>
        /// <value>The OEM part number.</value>
        [DataMember(Order = 4)]
        public string OEMPartNumber { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        [DataMember(Order = 5)]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the requested ship date.
        /// </summary>
        /// <value>The requested ship date.</value>
        [DataMember(Order = 6)]
        public DateTime RequestedShipDate { get; set; }
    }
}
