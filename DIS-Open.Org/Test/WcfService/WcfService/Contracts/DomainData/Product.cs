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
    /// Product class is used to populate the Product relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class Product
    {
        /// <summary>
        /// Gets or Sets the Licensable Number
        /// </summary>
        [DataMember(Order = 1)]
        public string MaterialNumber { get; set; }
        /// <summary>
        /// Gets or Sets the Part Name
        /// </summary>
        [DataMember(Order = 2)]
        public string ItemName { get; set; }
        /// <summary>
        /// Gets or Sets the Licensable Status
        /// </summary>
        [DataMember(Order = 3)]
        public string StatusCode { get; set; }
        /// <summary>
        /// Gets or Sets the Item Legal Name
        /// </summary>
        [DataMember(Order = 4)]
        public string ItemLegalName { get; set; }
        /// <summary>
        /// Gets or Sets the Product Legal Name
        /// </summary>
        [DataMember(Order = 5)]
        public string ProductFamilyCode { get; set; }
        /// <summary>
        /// Gets or Sets the End Of Life of a product
        /// </summary>
        [DataMember(Order = 6)]
        public string EOL { get; set; }
    }
}
