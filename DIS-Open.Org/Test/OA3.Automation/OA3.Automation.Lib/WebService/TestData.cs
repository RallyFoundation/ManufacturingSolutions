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
using System.IO;
using System.Xml;
using Microsoft.Http;
using OA3.Automation.Lib.Log;
using System.Xml.Linq;

namespace OA3.Automation.Lib.WebService
{
    /// <summary>
    /// Load the TestData
    /// </summary>
    public class TestData
    {
        //Load basic information of OEM Web services
        public static string OemInternalBaseUrl = string.Empty;
        public static string OemBaseUrl = string.Empty;
        public static string OemUserName = string.Empty;
        public static string OemPassword = string.Empty;
        public static string OemTrustedIssuer = string.Empty;


        //Load basic information of TPI Web services
        public static string TpiInternalBaseUrl = string.Empty;
        public static string TpiBaseUrl = string.Empty;
        public static string TpiUserName = string.Empty;
        public static string TpiPassword = string.Empty;
        public static string TpiTrustedIssuer = string.Empty;

        //Load OEM and TPI web services
        public static Dictionary<string, WebServiceUrl> OemInternalUrls = new Dictionary<string, WebServiceUrl>();
        public static Dictionary<string, WebServiceUrl> OemUrls = new Dictionary<string, WebServiceUrl>();
        public static Dictionary<string, WebServiceUrl> TpiInternalUrls = new Dictionary<string, WebServiceUrl>();
        public static Dictionary<string, WebServiceUrl> TpiUrls = new Dictionary<string, WebServiceUrl>();

        #region Request Object

        public static string OrderUniqueID = string.Empty;
        public static string OemAssigenedPdkID = string.Empty;
        public static List<string> TestProductKeyIDs = new List<string>();
        public static Dictionary<string, string> PDKSKUMapping = new Dictionary<string, string>();
        public static string ShipToCustomerNumber = string.Empty;

        #endregion

        static TestData()
        {
            InitializeStaticMembers();
            TextLog.LogMessage("Initialize Test Data Successfully.");
        }

        private static void InitializeStaticMembers()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("TestData.xml");
                Console.WriteLine("Loaded TestData.xml file.");

                //Load Basic Information
                LoadBaseInfo(doc);
                //Load Oem Urls
                LoadUrls(doc, "OemInternalWebService");
                LoadUrls(doc, "OemWebService");
                //Load Tpi Urls
                LoadUrls(doc, "TpiInternalWebService");
                LoadUrls(doc, "TpiWebService");
            }
            catch (DirectoryNotFoundException dEx)
            {
                //TODO
                Console.WriteLine("Load Test Data Failed! Exception: " + dEx.Message);
            }
            catch (Exception ex)
            {
                //TODO  
                Console.WriteLine("Initialize Test Data Failed! Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Load the OEM/TPI urls
        /// </summary>
        /// <param name="doc">Test Data Document</param>
        private static void LoadUrls(XmlDocument doc, string serviceName)
        {
            Dictionary<string, WebServiceUrl> urls = null;
            string baseUrl = null;
            switch (serviceName)
            {
                case "OemInternalWebService":
                    urls = OemInternalUrls;
                    baseUrl = OemInternalBaseUrl;
                    break;
                case "OemWebService":
                    urls = OemUrls;
                    baseUrl = OemBaseUrl;
                    break;
                case "TpiInternalWebService":
                    urls = TpiInternalUrls;
                    baseUrl = TpiInternalBaseUrl;
                    break;
                case "TpiWebService":
                    urls = TpiUrls;
                    baseUrl = TpiBaseUrl;
                    break;
            }

            //string xpath = isOem ? "/WebServices/Group[@Name='OEM']/Service" : "/WebServices/Group[@Name='TPI']/Service";
            string xpath = "/WebServices/Group[@Name='" + serviceName + "']/Service";
            XmlNodeList nodeList = doc.SelectNodes(xpath);

            foreach (XmlNode node in nodeList)
            {
                HttpMethod method = HttpMethod.GET;
                //Assign method value
                switch (node.Attributes["Method"].Value.ToLower())
                {
                    case "get":
                        method = HttpMethod.GET;
                        break;
                    case "post":
                        method = HttpMethod.POST;
                        break;
                }

                if (urls != null)
                urls.Add(node.Attributes["Name"].Value,
                    new WebServiceUrl(serviceName, method, baseUrl + node.Attributes["Url"].Value, node.InnerXml));

            }
        }

        /// <summary>
        /// Load Basic Info
        /// </summary>
        /// <param name="doc">Test Data Document</param>
        private static void LoadBaseInfo(XmlDocument doc)
        {
            XmlNodeList nodeList = doc.SelectNodes("/WebServices/Group");
            foreach (XmlNode node in nodeList)
            {
                switch (node.Attributes["Name"].Value)
                {
                    case "OemInternalWebService":
                        OemInternalBaseUrl = node.Attributes["BaseUri"].Value;
                        break;
                    case "OemWebService":
                        OemBaseUrl = node.Attributes["BaseUri"].Value;
                        OemUserName = node.Attributes["UserName"].Value;
                        OemPassword = node.Attributes["Password"].Value;
                        OemTrustedIssuer = node.Attributes["TrustedIssuer"].Value;
                        break;
                    case "TpiInternalWebService":
                        TpiInternalBaseUrl = node.Attributes["BaseUri"].Value;
                        break;
                    case "TpiWebService":
                        TpiBaseUrl = node.Attributes["BaseUri"].Value;
                        TpiUserName = node.Attributes["UserName"].Value;
                        TpiPassword = node.Attributes["Password"].Value;
                        TpiTrustedIssuer = node.Attributes["TrustedIssuer"].Value;
                        break;
                    default:
                        throw new Exception("Could not find basic information in Test Data");
                }
            }
        }

        /// <summary>
        /// Store the Test Order Unique ID
        /// </summary>
        /// <param name="response"></param>
        public static void UpdateOrderUniqueId(string response)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(response);
            OrderUniqueID = xDoc.InnerText;
            OemInternalUrls["GetOrderFulfillment"].Request = OrderUniqueID;
            Console.WriteLine("Update the OrderUniquestId as " + OrderUniqueID);
            TextLog.LogMessage("Update the OrderUniquestId as " + OrderUniqueID);
        }

        /// <summary>
        /// Store the Test Product Key
        /// </summary>
        /// <param name="response"></param>
        public static void UpdateTestProductKey(string response)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(response);

            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xDoc.NameTable);

            xmlnsManager.AddNamespace("i", "http://www.w3.org/2011/XMLSchema-instance");
            xmlnsManager.AddNamespace("oem", "http://schemas.ms.it.oem/digitaldistribution/2010/10");

            XmlNodeList xNodeList = xDoc.SelectNodes("//oem:Key", xmlnsManager);

            foreach (XmlNode node in xNodeList)
            {
                TestProductKeyIDs.Add(node["ProductKeyID"].InnerText);
                PDKSKUMapping.Add(node["ProductKeyID"].InnerText, node["SKUID"].InnerText);
            }
        }

        /// <summary>
        /// Store the OEM assigned key id
        /// </summary>
        /// <param name="response"></param>
        public static void UpdateAssignedPdkId(string response)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(response);

            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xDoc.NameTable);

            xmlnsManager.AddNamespace("i", "http://www.w3.org/2011/XMLSchema-instance");
            xmlnsManager.AddNamespace("oem", "http://schemas.ms.it.oem/digitaldistribution/2010/10");

            XmlNodeList xNodeList = xDoc.SelectNodes("//oem:TransferKey", xmlnsManager);

            if (xNodeList.Count != 0)
            {
                OemAssigenedPdkID = xNodeList[0]["ProductKeyId"].InnerText;
                Console.WriteLine("Get assigned key id : " + OemAssigenedPdkID);
                TextLog.LogMessage("Get assigned key id : " + OemAssigenedPdkID);
            }
            else
            {
                TextLog.LogMessage("There was no assigned key in Test environment, please check it again and re-run the case.");
                throw new Exception("There was no assigned key in Test environment, please check it again and re-run the case.");
            }
        }
    }
}
