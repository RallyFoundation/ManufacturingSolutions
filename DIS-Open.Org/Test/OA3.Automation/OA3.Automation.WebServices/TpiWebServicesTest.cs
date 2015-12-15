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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Http;
using OA3.Automation.Lib.WebService;
using OA3.Automation.Lib;
using System.Reflection;

namespace OA3.Automation.WebServices
{
    /// <summary>
    /// Summary description for TpiWebServicesTest
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Data\TestData.xml")]
    public class TpiWebServicesTest
    {
        public TpiWebServicesTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetKeysInternal()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.TpiInternalUrls["GetKeys"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("The response content as follow:");
            Console.WriteLine(response.Content.ReadAsString());
        }

        [TestMethod]
        public void SyncAllocatedKeysInternal()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.TpiInternalUrls["SyncAllocatedKeys"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void ReportKeysInternal()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.TpiInternalUrls["ReportKeys"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void GetKeys()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.TpiUrls["GetKeys"], TestData.TpiUserName, TestData.TpiPassword);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("The response content as follow:");
            Console.WriteLine(response.Content.ReadAsString());
        }

        [TestMethod]
        public void SyncAllocatedKeys()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.TpiUrls["SyncAllocatedKeys"], TestData.TpiUserName, TestData.TpiPassword);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void ReportKeys()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.TpiUrls["ReportKeys"], TestData.TpiUserName, TestData.TpiPassword);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }
    }
}
