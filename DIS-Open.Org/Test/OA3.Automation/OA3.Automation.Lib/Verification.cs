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
using Microsoft.Http;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OA3.Automation.Lib.Log;

namespace OA3.Automation.Lib
{
    /// <summary>
    /// Verification Class to assert test result
    /// </summary>
    public class Verification
    {
        public static void AssertHttpResponse(HttpResponseMessage response,string methodName)
        {
            bool succeed = false;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                succeed = true;
                TextLog.LogMessage(methodName + ": Test Case Pass.");
                Console.WriteLine(methodName + ": Get Response successfully.");
            }
            else
            {
                succeed = false;
                TextLog.LogMessage(methodName + ": Test Case Pass.");
                Console.WriteLine(methodName + ": Get Response failed.");
            }

            Assert.AreEqual(true, succeed);
        }
    } 
}
