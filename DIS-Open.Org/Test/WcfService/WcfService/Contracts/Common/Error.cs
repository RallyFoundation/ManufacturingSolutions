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
    /// Error class is used to populate the Error relevant information
    /// </summary>
    [DataContract(Name = "Error", Namespace = "")]
    public class Error
    {
        /// <summary>
        /// Gets or Sets the ErrorCode
        /// </summary>
        [DataMember(Order = 1)]
        public string ErrorCode { get; set; }
        /// <summary>
        /// Gets or Sets the ErrorMessage
        /// </summary>
        [DataMember(Order = 2)]
        public string ErrorMessage{ get; set; }
    }
}
