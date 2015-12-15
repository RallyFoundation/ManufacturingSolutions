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

namespace DIS.Business.Client
{
    /// <summary>
    /// The direction of outbound call
    /// </summary>
    [Flags]
    public enum CallDirection
    {
        None = 0,
        /// <summary>
        /// Connect to internal web service
        /// </summary>
        Internal = 1,
        /// <summary>
        /// Connect to Microsoft web service
        /// </summary>
        Microsoft = 2,
        /// <summary>
        /// Connect to up level system web service
        /// </summary>
        UpLevelSystem = 4,
        /// <summary>
        /// Connect to down level system web service
        /// </summary>
        DownLevelSystem = 8,
    }
}
