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
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Security;
using System.ServiceModel.Web;
using DIS.Business.Proxy;
using DIS.Data.DataContract;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    public class InternalMembershipProvider : IMembershipProvider
    {
        private IUserProxy userProxy;

        private string cloudConfigurationID;

        private string dbConnectionString;

        public InternalMembershipProvider()
            : this(null) {
        }

        public InternalMembershipProvider(IUserProxy userProxy) 
        {
            if (userProxy == null)
                this.userProxy = new UserProxy();
            else
                this.userProxy = userProxy;
        }

        public bool ValidateUser(string username, string password, InstallType installType) 
        {
            return this.userProxy.ValidateUser(username, password);
        }

        public void SetCredentials(DisCredentials credentials, Message requestMessage) 
        {
            // Do nothing

            //Retrieve could configuration ID from request header for supporting multiple cutomer context, and re-initialize IUserProxy - Rally
            if (requestMessage != null)
            {
                HttpRequestMessageProperty requestMessageProperty = (HttpRequestMessageProperty)requestMessage.Properties[HttpRequestMessageProperty.Name];
                this.cloudConfigurationID = requestMessageProperty.Headers.Get(DIS.Business.Client.ServiceClient.ConfigurationIdHeaderName);

                if ((!String.IsNullOrEmpty(this.cloudConfigurationID)) && ((ModuleConfiguration.DISCloudConfigurations == null) || ((!String.IsNullOrEmpty(this.cloudConfigurationID)) && (ModuleConfiguration.DISCloudConfigurations != null) && (!ModuleConfiguration.DISCloudConfigurations.ContainsKey(this.cloudConfigurationID)))))
                {
                    ModuleConfiguration.SyncConfigurations();
                }

                if ((!String.IsNullOrEmpty(this.cloudConfigurationID)) && (ModuleConfiguration.DISCloudConfigurations != null) && (ModuleConfiguration.DISCloudConfigurations.ContainsKey(this.cloudConfigurationID)))
                {
                    this.dbConnectionString = ModuleConfiguration.DISCloudConfigurations[this.cloudConfigurationID];
                }

                if ((!String.IsNullOrEmpty(this.cloudConfigurationID)) && (!String.IsNullOrEmpty(this.dbConnectionString)))
                {
                    this.userProxy = new UserProxy(dbConnectionString);
                }
            }
        }
    }
}