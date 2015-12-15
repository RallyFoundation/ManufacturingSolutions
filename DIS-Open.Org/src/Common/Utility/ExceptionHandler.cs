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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DIS.Common.Utility
{
    /// <summary>
    /// Exception handler
    /// </summary>
    public class ExceptionHandler
    {
        ///// <summary>
        ///// Handle an exception
        ///// </summary>
        ///// <param name="ex"></param>
        //public static void HandleException(Exception ex)
        //{
        //    ExceptionPolicy.HandleException(ex, "ExceptionPolicy");
        //}

        /// <summary>
        /// Handle an exception
        /// </summary>
        /// <param name="ex"></param>
        public static void HandleException(Exception ex, string dbConnectionString)
        {
            DISLoggableException disLoggableException = new DISLoggableException(ex.Message, ex);

            disLoggableException.DBConnectionString = dbConnectionString;

            ExceptionPolicy.HandleException(disLoggableException, "ExceptionPolicy");
        }
    }
}
