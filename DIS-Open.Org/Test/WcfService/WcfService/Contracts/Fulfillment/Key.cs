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

namespace WcfService.Contracts
{
    /// <summary>
    /// Key class has the revelant Base fields for BindingRequest, that the caller has to populate and submit.
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class Key 
    {
        /// <summary>
        /// Gets or Sets the Product Key
        /// </summary>
        [DataMember(Order = 1)]
        public string ProductKey { get; set; }
        /// <summary>
        /// Gets or Sets the ProductKeyId
        /// </summary>
        [DataMember(Order = 2)]
        public long ProductKeyID { get; set; }

        [DataMember(Order = 3)]
        public string SKUID { get; set; }
    }
}
