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
using System.Windows;
using System.Windows.Data;
using DIS.Common.Utility;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.Behaviors
{
    /// <summary>
    /// Class to Convert Boolean value to Visibility Enum 
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
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
            Visibility rv = Visibility.Visible;
            try
            {
                //Parse Bool value and return corresponding Visibility Enum
                var x = bool.Parse(value.ToString());
                if (x)
                {
                    rv = Visibility.Visible;
                }
                else
                {
                    rv = Visibility.Collapsed;
                }
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

    public class ReasonCodeConverterVisibility : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility rv = Visibility.Visible;
            if (value == null || value.ToString() == Constants.CBRAckReasonCode.ActivationEnabled)
                rv = Visibility.Visible;
            else
                rv = Visibility.Collapsed;
            return rv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
