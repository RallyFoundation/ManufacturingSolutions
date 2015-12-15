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
    /// This class is used for validating the required field
    /// </summary>
    public class ValidRequiredField : ValidatorBase
    {
        #region Priviate & Protected member variables
        
        private const string _INVALID_REQUIRED_FIELD = "Required Field";
        
        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string Validate(object value)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                    return _INVALID_REQUIRED_FIELD;
            }
            catch (NullReferenceException)
            {
                return _INVALID_REQUIRED_FIELD;
            }
            return null;
        }

        #endregion
    }
}