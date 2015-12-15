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

namespace DIS.Presentation.KMT.View.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigurationPage
    {
        /// <summary>
        /// 
        /// </summary>
        void Save();

        /// <summary>
        /// 
        /// </summary>
        bool CanSave { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsSaved { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsBusy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler IsBusyChanged;
    }
}
