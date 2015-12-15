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
using DIS.Business.Proxy;
using DIS.Data.DataContract;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    public class DisMembershipProvider : IMembershipProvider {
        private readonly CallDirectionExtractor callDirectionExtractor;
        private ISubsidiaryProxy ssProxy;
        private IHeadQuarterProxy hqProxy;

        private string cloudCustomerID;

        private string cloudConfigurationID;

        private string dbConnectionString;

        public DisMembershipProvider()
            : this(null, null) {
        }

        public DisMembershipProvider(ISubsidiaryProxy ssProxy, IHeadQuarterProxy hqProxy) {
            callDirectionExtractor = new CallDirectionExtractor();

            if (ssProxy == null)
                this.ssProxy = new SubsidiaryProxy();
            else
                this.ssProxy = ssProxy;

            if (hqProxy == null)
                this.hqProxy = new HeadQuarterProxy();
            else
                this.hqProxy = hqProxy;
        }

        public bool ValidateUser(string username, string password, InstallType installType) {
            switch (installType) {
                case InstallType.Dls:
                    return ssProxy.ValidateSubsidiary(username, password);
                case InstallType.Uls:
                    return hqProxy.ValidateHeadQuarter(username, password);
                default:
                    throw new NotSupportedException();
            }
        }

        public void SetCredentials(DisCredentials credentials, Message requestMessage) 
        {
            credentials.CallDirection = callDirectionExtractor.ExtractDirection(requestMessage);

            //Retrieve could customer ID from request header for supporting multiple cutomer context, and re-initialize ISubsidiaryProxy and IHeadQuarterProxy - Rally
            if (requestMessage != null)
            {
                HttpRequestMessageProperty requestMessageProperty = (HttpRequestMessageProperty)requestMessage.Properties[HttpRequestMessageProperty.Name];
                
                this.cloudCustomerID = requestMessageProperty.Headers.Get(DIS.Business.Client.ServiceClient.CustomerIdHeaderName);

                if (String.IsNullOrEmpty(this.cloudCustomerID))
                {
                    //string authHeader = requestMessageProperty.Headers.Get(DIS.Business.Client.ServiceClient.AuthorizationHeaderName);
                    //authHeader = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(authHeader));

                    //authHeader = (authHeader.Split(new string[] { ":" }, StringSplitOptions.None))[1];

                    if ((ModuleConfiguration.DISCloudBusinessReferences != null) && (ModuleConfiguration.DISCloudBusinessReferences.ContainsKey(credentials.Password)))
                    {
                        this.cloudCustomerID = ModuleConfiguration.DISCloudBusinessReferences[credentials.Password];
                    }
                }

                if (String.IsNullOrEmpty(this.cloudCustomerID))
                {
                    this.cloudCustomerID = ModuleConfiguration.DefaultBusinessID;
                }

                if ((!String.IsNullOrEmpty(this.cloudCustomerID)) && ((ModuleConfiguration.DISCloudCustomers == null) || ((!String.IsNullOrEmpty(this.cloudCustomerID)) && (ModuleConfiguration.DISCloudCustomers != null) && (!ModuleConfiguration.DISCloudCustomers.ContainsKey(this.cloudCustomerID)))))
                {
                    ModuleConfiguration.SyncConfigurations();
                }

                if ((!String.IsNullOrEmpty(this.cloudCustomerID)) && (ModuleConfiguration.DISCloudCustomers != null) && (ModuleConfiguration.DISCloudCustomers.ContainsKey(this.cloudCustomerID)))
                {
                    this.cloudConfigurationID = ModuleConfiguration.DISCloudCustomers[this.cloudCustomerID];
                }

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
                    this.ssProxy = new SubsidiaryProxy(this.dbConnectionString);
                    this.hqProxy = new HeadQuarterProxy(this.dbConnectionString);
                }
            }
        }
    }
}