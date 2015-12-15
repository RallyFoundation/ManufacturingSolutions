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

using System.Runtime.Serialization;

namespace DIS.Data.ServiceContract
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
