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
    public class HeadQuarter
    {
        public HeadQuarter()
        {
            this.UserHeadQuarters = new List<UserHeadQuarter>();
        }

        public int HeadQuarterId { get; set; }
        public string DisplayName { get; set; }
        public string CertSubject { get; set; }
        public string CertThumbPrint { get; set; }
        public string ServiceHostUrl { get; set; }
        public string UserName { get; set; }
        public string AccessKey { get; set; }
        public string Description { get; set; }
        public bool IsCentralizedMode { get; set; }
        public bool IsCarbonCopy { get; set; }
        public ICollection<KeyInfoEx> KeyInfoExes { get; set; }
        public ICollection<UserHeadQuarter> UserHeadQuarters { get; set; }

    }
}

