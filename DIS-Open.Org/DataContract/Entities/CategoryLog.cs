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

namespace DIS.Data.DataContract {
    [DataContract]
    public class CategoryLog {
        [DataMember]
        public int CategoryLogId { get; set; }
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public int LogId { get; set; }
        public Category Category { get; set; }
        public Log Log { get; set; }
    }
}

