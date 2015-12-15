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
using System.Windows.Data;
using DIS.Common.Utility;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.Behaviors
{
    /// <summary>
    /// 
    /// </summary>
    public class BoolToYesNoConverter : IValueConverter
    {
        #region Public methods

        /// <summary>
        /// Convert Boolean to Visibility Enum
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="targetType">Target Type</param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string rv = MergedResources.KeyManager_DisplayYes;
            try
            {
                var x = bool.Parse(value.ToString());
                if (x)
                    rv = MergedResources.KeyManager_DisplayYes;
                else
                    rv = MergedResources.KeyManager_DisplayNo;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
            }
            return rv;
        }

        /// <summary>
        /// Convert back to bool Type
        /// </summary>
        /// <param name="value">Visibility object</param>
        /// <param name="targetType">Target Type</param>
        /// <param name="parameter"></param>
        /// <param name="culture">Culture Info</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
