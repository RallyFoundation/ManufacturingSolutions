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
    /// Used for validating numeric field
    /// </summary>
    public class ValidNumericField : ValidatorBase
    {
        #region Priviate & Protected member variables

        private const string _INVALID_NUMERIC_FIELD = "Must be Numeric.";

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string Validate(object value)
        {
            string input = value as string;
            long result;
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    var status = long.TryParse(input, out result);
                    if (!status)
                        return _INVALID_NUMERIC_FIELD;
                }
            }
            catch (NullReferenceException)
            {
                return _INVALID_NUMERIC_FIELD;
            }
            return null;
        }

        #endregion
    }
}