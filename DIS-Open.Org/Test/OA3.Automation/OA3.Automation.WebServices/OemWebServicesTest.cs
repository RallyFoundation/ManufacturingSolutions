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
using OA3.Automation.Lib;
using OA3.Automation.Lib.WebService;
using Microsoft.Http;
using OA3.Automation.Lib.Log;
using System.Reflection;
using System.Xml;
using System.Threading;

namespace OA3.Automation.WebServices
{
    /// <summary>
    /// Summary description for OemWebServicesTest
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Data\TestData.xml")]
    public class OemWebServicesTest
    {
        public OemWebServicesTest()
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
        public void GetContract()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetContract"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void GetOrderType()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetOrderType"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void GetOrderStatus()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetOrderStatus"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void GetCustomerPartNumberMapping()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetCustomerPartNumberMapping"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void GetProduct()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetProduct"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void GetCustomerPartnerFunction()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetCustomerPartnerFunction"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void GetPartnerFunction()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetPartnerFunction"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void GetCustomer()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetCustomer"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void PostOrder()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["PostOrder"]);
            //Update Test Order Unique Id which will be used for other case
            TestData.UpdateOrderUniqueId(response.Content.ReadAsString());
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void GetOrderFulfillment()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["GetOrderFulfillment"]);
            //Update Test Product Key
            TestData.UpdateTestProductKey(response.Content.ReadAsString());
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void ReportCBR()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["ReportCBR"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void ReportUnUsedKeys()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemInternalUrls["ReportUnUsedKeys"]);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Prerequesite: We need assign keys to TPI first, and make sure 
        /// that the TPI data polling is not running.
        /// </summary>
        [TestMethod]
        public void GetKeysForTpi()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemUrls["GetKeysForTpi"], TestData.OemUserName, TestData.OemPassword);
            TestData.UpdateAssignedPdkId(response.Content.ReadAsString());
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
            //TextLog.LogMessage("The response content as follow:" + response.Content.ReadAsString());
        }

        [TestMethod]
        public void SyncAllocatedKeys()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemUrls["SyncAllocatedKeys"], TestData.OemUserName, TestData.OemPassword);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void ReportKeys()
        {
            HttpResponseMessage response;
            response = ServiceClient.ExecuteServiceMethod(TestData.OemUrls["ReportKeys"], TestData.OemUserName, TestData.OemPassword);
            Verification.AssertHttpResponse(response, MethodBase.GetCurrentMethod().Name);
        }

        
    }
}
