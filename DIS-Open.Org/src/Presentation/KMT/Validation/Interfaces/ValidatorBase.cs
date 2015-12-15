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

namespace DIS.Presentation.KMT.Validation.Interfaces
{
    /// <summary>
    /// ValidatorBase abstract class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public abstract class ValidatorBase : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract string Validate(object value);
    }

}