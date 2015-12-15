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
    /// MultiErrorResponse class is used to populate the MultiErrorResponse relevant information
    /// </summary>
    [DataContract(Name = "MultiErrorResponse", Namespace = "")]
    abstract public class MultiErrorResponse : ResponseBase
    {
        /// <summary>
        /// Gets or Sets the Errors
        /// </summary>
        [DataMember(Order = 1)]
        public Error[] Errors { get; set; }
    }
}
