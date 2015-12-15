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
using System.ComponentModel;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataErrorInfoViewModelBase : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Error
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string this[string columnName]
        {
            get { return GetValidationError(columnName); }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                {
                    if (GetValidationError(property) != null)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract string[] ValidatedProperties { get; }

        /// <summary>
        /// 
        /// </summary>
        protected string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            return ValidateProperties(propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract string ValidateProperties(string propertyName);

        /// <summary>
        /// 
        /// </summary>
        protected string GetPropertyValidationMessage(bool isValid, string errorMessage)
        {
            return isValid ? null : errorMessage;
        }
    }
}
