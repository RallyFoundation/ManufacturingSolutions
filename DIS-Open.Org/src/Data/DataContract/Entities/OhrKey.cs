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
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DIS.Data.DataContract
{
    [DataContract]
    public class OhrKey
    {
        [DataMember]
        public System.Guid CustomerUpdateUniqueID { get; set; }
        [DataMember]
        public long KeyId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string ReasonCode { get; set; }
        [DataMember]
        public string ReasonCodeDescription { get; set; }
        public Ohr Ohr { get; set; }
        public KeyInfo KeyInfo { get; set; }

        public OhrKey()
        {
        }

        public OhrKey(long keyId)
            : this(keyId, OhrName.ProductKeyID, keyId.ToString())
        {
        }

        public OhrKey(long keyId, string name, string value)
        {
            KeyId = keyId;
            Name = name;
            Value = value;
        }
    }
}
