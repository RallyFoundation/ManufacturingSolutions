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
using System.Linq;
using System.Text;

namespace DIS.Data.DataContract
{
    /// <summary>
    /// service config class
    /// </summary>
    public class ServiceConfig : IEquatable<ServiceConfig>
    {
        public string UserName { get; set; }
        public string UserKey { get; set; }
        public string ServiceHostUrl { get; set; }

        public bool Equals(ServiceConfig other)
        {
            return UserName == other.UserName
                && UserKey == other.UserKey
                && ServiceHostUrl == other.ServiceHostUrl;
        }
    }
}
