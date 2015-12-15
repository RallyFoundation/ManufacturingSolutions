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
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using OA3.Automation.Lib.Log;
using System.Reflection;

namespace OA3.Automation.Lib.WebService
{
    public class ServiceClient
    {
        private static bool isOem = true;
        private const string requestHeader = @"<?xml version=""1.0"" encoding=""UTF-8""?>";

        static ServiceClient()
        {
            //Register event for cert validation.
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(CustomXertificateValidation);
        }

        /// <summary>
        /// Static method to request a webservice and get the response content
        /// </summary>
        /// <param name="service">Request Object</param>
        /// <returns>Response Object</returns>
        public static HttpResponseMessage ExecuteServiceMethod(WebServiceUrl service, string userName, string password)
        {

            HttpResponseMessage response = null;
            isOem = service.IsOem;

            try
            {
                using (var webClient = new HttpClient())
                {
                    webClient.DefaultHeaders.Add("Content-Type", "application/xml;charset=utf-8");
                    webClient.DefaultHeaders.Add("Authorization", GetAuthHeader(userName, password));

                    if (service.Method == HttpMethod.POST)
                    {
                        response = webClient.Send(service.Method, service.Url,
                            HttpContent.Create(requestHeader + service.Request, "application/xml"));

                    }
                    else
                    {
                        if (service.Request != null && service.Request.ToString().Length > 0)
                        {
                            service.Url += string.Format("/{0}", service.Request.ToString());
                        }
                        response = webClient.Get(service.Url);
                    }
                }
            }
            catch (Exception e)
            {
                TextLog.LogMessage("Faile to request the web service with the exception : " + e.Message);
                throw ;
            }

            return response;

        }

        public static HttpResponseMessage ExecuteServiceMethod(WebServiceUrl service)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var webClient = new HttpClient())
                {
                    if (service.Method == HttpMethod.POST)
                    {
                        webClient.DefaultHeaders.Add("Content-Type", "application/xml;charset=utf-8");
                        webClient.DefaultHeaders.Add("Authorization", "Basic Og==");
                        response = webClient.Send(HttpMethod.POST, service.Url,
                            HttpContent.Create(requestHeader + service.Request, "application/xml"));
                    }
                    else
                    {
                        if (service.Request != null && service.Request.ToString().Length > 0)
                        {
                            service.Url += string.Format("/{0}", service.Request.ToString());
                        }
                        response = webClient.Get(service.Url);
                    }
                }
            }
            catch (Exception e)
            {
                TextLog.LogMessage("Faile to request the web service with the exception : " + e.Message);
                throw ;
            }
            return response;
        }


        /// <summary>
        /// Create Service Auth Header
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string GetAuthHeader(string userName, string password)
        {
            var encodedCred = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                string.Format("{0}:{1}", userName, password)));
            return string.Format("Basic {0}", encodedCred);
        }

        /// <summary>
        /// Validate the cert for request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static bool CustomXertificateValidation(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            if (error == SslPolicyErrors.None)
            {
                TextLog.LogMessage("No need to do cert authorization.");
                return true;
            }

            if (isOem)
            {
                if (cert.Issuer.Equals("CN=" + TestData.OemTrustedIssuer, StringComparison.OrdinalIgnoreCase))
                {
                    TextLog.LogMessage("Pass cert authorization.");
                    return true;
                }
            }
            else
            {
                if (cert.Issuer.Equals("CN=" + TestData.TpiTrustedIssuer, StringComparison.OrdinalIgnoreCase))
                {
                    TextLog.LogMessage("Pass cert authorization.");
                    return true;
                }
            }
            TextLog.LogMessage("Fail to cert authorization.");
            return false;
        }
    }
}
