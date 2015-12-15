﻿//*********************************************************
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
using System.Runtime.Serialization;

namespace DIS.Data.ServiceContract
{
    /// <summary>
    /// ReturnLineItem class the revelant fields that the caller has to populate and submit.
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ReturnLineItem
    {
        [DataMember(Order = 1)]
        public int OEMRMALineNumber { get; set; }

        [DataMember(Order = 2)]
        public string ReturnTypeID { get; set; }

        [DataMember(Order = 3)]
        public long ProductKeyID { get; set; }

    }
}
