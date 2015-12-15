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

namespace DIS.Data.DataContract {
    /// <summary>
    /// Password versions used in DIS
    /// </summary>
    public enum PasswordVersion {
        /// <summary>
        /// Indicates SHA1 hash algorithm.
        /// </summary>
        Sha1 = 1,
        /// <summary>
        /// Indicates SHA512 hash algorithm with salt.
        /// </summary>
        Sha512WithSalt = 2,
    }
}
