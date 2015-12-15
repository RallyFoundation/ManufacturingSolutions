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
using System.Windows.Data;
using System.Globalization;
using DIS.Data.DataContract;
using System.Text.RegularExpressions;

namespace DIS.Presentation.KMT.Behaviors
{
    public class TouchScreenEnumHelper
    {
        public static string Convert(string value)
        {
            string result = value;
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    result = string.Empty;
                }
                else
                {
                    TouchEnum enumRet = OemOptionalInfo.ConvertTouchEnum(value);
                    result = EnumHelper.GetFieldDecription(typeof(TouchEnum), enumRet);
                }
            }
            catch (ApplicationException)
            {
                result = value;
            }

            return result;
        }
    }

    public class TouchScreenEnumConverter: IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string toConvert = string.Empty;
            if (value != null)
                toConvert = value.ToString();

            return TouchScreenEnumHelper.Convert(toConvert);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return null;
            else if (value.ToString() == "All")
                return value;
            else
                return OemOptionalInfo.ConvertTouchEnum(value.ToString());
        }
    }
}
