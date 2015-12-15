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
    /// ResponseBase class has the fields for response
    /// </summary>
    [DataContract(Name = "ResponseBase", Namespace = "")]
    abstract public class ResponseBase
    {
        /// <summary>
        /// Gets or Sets the Succeeded
        /// </summary>
        [DataMember(Order = 1)]
        public bool Succeeded { get; set; }
    }
}
