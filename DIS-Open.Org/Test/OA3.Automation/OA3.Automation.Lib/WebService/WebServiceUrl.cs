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
using System.Xml;
using System.IO;
using OA3.Automation.Lib.Log;

namespace OA3.Automation.Lib.WebService
{
    /// <summary>
    /// Represent the web service
    /// </summary>
    public class WebServiceUrl
    {
        #region Private Fields
        private string name = string.Empty;
        private HttpMethod method = HttpMethod.GET;
        private string url = string.Empty;
        private string request = string.Empty;
        private bool isOemWebService = true;
        #endregion

        #region Properties
        public string Name 
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public HttpMethod Method
        {
            get
            {
                return method;
            }
            set
            {
                method = value;
            }
        }

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        public bool IsOem
        {
            get
            {
                return isOemWebService;
            }
            set
            {
                isOemWebService = value;
            }
        }

        public string Request
        {
            get
            {
                request = UpdateRequest(request);
                return request;
            }
            set
            {
                request = value;
            }
        }

        /// <summary>
        /// Update the request data
        /// </summary>
        /// <param name="request"></param>
        private string UpdateRequest(string request)
        {
            switch (name)
            {
                //case "PostOrder":
                //    request = UpdateOrderRequest(request);
                //    break;
                case "ReportCBR":
                case "ReportUnUsedKeys":
                    request = UpdateReportRequest(request);
                    break;
                case "ReportKeys":
                    request = UpdateTransferKeyRequest(request);
                    break;
                case "SyncAllocatedKeys":
                    request = UpdateKeysRequest(request);
                    break;
                default:
                    break;
            }

            return request;
        }

        /// <summary>
        /// Update the key id for request of SyncAllocatedKeys.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string UpdateKeysRequest(string request)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(request);

            //XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xDoc.NameTable);

            //xmlnsManager.AddNamespace("i", "http://www.w3.org/2011/XMLSchema-instance");
            //xmlnsManager.AddNamespace("oem", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");

            //xDoc.SelectSingleNode("//oem:long").InnerText = TestData.OemAssigenedPdkID;
            xDoc.FirstChild.FirstChild.InnerText = TestData.OemAssigenedPdkID;
            TextLog.LogMessage("The product key id for Sync allocated is: " + TestData.OemAssigenedPdkID);

            return xDoc.InnerXml;
        }

        private string UpdateTransferKeyRequest(string request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Key field for Report request (CBR and Unused Key)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string UpdateReportRequest(string request)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(request);

            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xDoc.NameTable);

            xmlnsManager.AddNamespace("i", "http://www.w3.org/2011/XMLSchema-instance");
            xmlnsManager.AddNamespace("oem", "http://schemas.ms.it.oem/digitaldistribution/2010/10");

            switch (name)
            { 
                case "ReportCBR":
                    UpdateKeyFields(ref xDoc, xmlnsManager, 0);
                    break;
                case "ReportUnUsedKeys":
                    UpdateKeyFields(ref xDoc, xmlnsManager, 1);
                    break;
                default:
                    break;
            }

            return xDoc.InnerXml;
        }

        /// <summary>
        /// Update Key Fields
        /// </summary>
        /// <param name="xDoc"></param>
        /// <param name="xmlnsManager"></param>
        /// <param name="i"></param>
        private void UpdateKeyFields(ref XmlDocument xDoc, XmlNamespaceManager xmlnsManager, int i)
        {
            //Update ProductKeyID node
            XmlNode nodeProductKeyID = xDoc.SelectSingleNode("//oem:ProductKeyID", xmlnsManager);
            nodeProductKeyID.InnerText = TestData.TestProductKeyIDs[i];
            //Update SKUID Node
            XmlNode nodeSKUID = xDoc.SelectSingleNode("//oem:SKUID", xmlnsManager);
            nodeSKUID.InnerText = TestData.PDKSKUMapping[TestData.TestProductKeyIDs[i]];
        }

        private string UpdateOrderRequest(string request)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Constractors
        public WebServiceUrl()
        { }

        public WebServiceUrl(string name, string url, bool isOem)
            : this(name, HttpMethod.GET, url,string.Empty, isOem)
        { }

        public WebServiceUrl(string name, HttpMethod method, string url,string request,bool isOem)
        {
            this.Name = name;
            this.Method = method;
            this.Url = url;
            this.Request = request;
            this.isOemWebService = isOem;
        }

        public WebServiceUrl(string name, HttpMethod method, string url, string request)
        {
            this.Name = name;
            this.Method = method;
            this.Url = url;
            this.Request = request;
        }
        #endregion
    }
}
