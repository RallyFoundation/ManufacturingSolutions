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
    /// SingleErrorResponse class is used to populate the SingleErrorResponse relevant information
    /// </summary>
    [DataContract(Name = "SingleErrorResponse", Namespace = "")]
    abstract public class SingleErrorResponse : ResponseBase
    {
        /// <summary>
        /// Gets or Sets the ErrorMessage
        /// </summary>
        [DataMember(Order = 1)]
        public string ErrorMessage { get; set; }
    }
}
