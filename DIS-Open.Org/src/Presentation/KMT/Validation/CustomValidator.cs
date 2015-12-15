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
using DIS.Presentation.KMT.Validation.Interfaces;

namespace DIS.Presentation.KMT.Validation
{
    /// <summary>
    /// CustomValidator for validation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class CustomValidator : Attribute, ICustomValidator {
        /// <summary>
        /// 
        /// </summary>
        public string MethodName { get ; set; }
    }
}