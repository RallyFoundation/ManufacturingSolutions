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

using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyExpired
    {
        /// <summary>
        /// 
        /// </summary>
        public KeyInfo keyInfo { get; set; }

        private int overDays = 0;

        /// <summary>
        /// 
        /// </summary>
        public int OverDays
        {
            get { return overDays; }
            set { overDays = value; }
        }
    }
}
