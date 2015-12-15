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
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DIS.Data.DataContract
{
    [DataContract]
    public class KeySyncNotification
    {
        [DataMember, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long KeyId { get; set; }
        [DataMember]
        public byte KeyStateId { get { return (byte)KeyState; } set { KeyState = (KeyState)value; } }
        [DataMember]
        public DateTime CreateDate { get; set; }
        [NotMapped]
        public KeyState KeyState { get; set; }
    }
}
