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
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using DIS.Common.Utility;

namespace DIS.Services.WebServiceLibrary
{
    public abstract class ServiceBase
    {
        private string customerId;
        private string configurationId;
        private string dbConnectionString;

        public string CustomerID 
        {
            get { return this.customerId; }
            set { this.customerId = value; }
        }

        public string ConfigurationID 
        {
            get { return this.configurationId; }
            set { this.configurationId = value; }
        }

        public string DBConnectionString 
        {
            get { return this.dbConnectionString; }
            set { this.dbConnectionString = value; }
        }
        protected void LogServiceCall(string methodName)
        {
            string title = string.Format("{0} Called", this.GetType().Name);
            string invoker = "[unknown]";
            ServiceSecurityContext ssContext = ServiceSecurityContext.Current;
            OperationContext opContext = OperationContext.Current;
            if (ssContext != null)
            {
                invoker = ssContext.PrimaryIdentity.Name;
            }
            else if (opContext != null)
            {
                MessageProperties prop = opContext.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                if (endpoint != null)
                    invoker = endpoint.Address;
            }

            MessageLogger.LogSystemRunning(title, string.Format("{0} service was called by {1}.", methodName, invoker), this.dbConnectionString);
        }

        protected string GetRequestHeader(string headerName)
        {
            return WebOperationContext.Current.IncomingRequest.Headers.Get(headerName);
        }
    }
}
