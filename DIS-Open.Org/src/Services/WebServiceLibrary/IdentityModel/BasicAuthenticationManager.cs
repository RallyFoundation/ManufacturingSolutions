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
using System.ServiceModel.Security;
using System.Web;
using System.Web.Security;
using DIS.Business.Client;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class is responsible for managing basic authentication when using WCF REST.
    /// </summary>
    internal class BasicAuthenticationManager {
        private readonly BasicAuthenticationCredentialsExtractor basicAuthenticationCredentialsExtractor;
        private readonly AuthorizationStringExtractor httpRequestAuthorizationExtractor;
        private readonly IMembershipProvider membershipProvider;
        private readonly ResponseMessageFactory responseMessageFactory;
        private readonly ServiceSecurityContextFactory serviceSecurityContextFactory;

        internal BasicAuthenticationManager(BasicAuthenticationCredentialsExtractor basicAuthenticationCredentialsExtractor,
                AuthorizationStringExtractor httpRequestAuthorizationExtractor,
                IMembershipProvider membershipProvider,
                ResponseMessageFactory responseMessageFactory,
                ServiceSecurityContextFactory serviceSecurityContextFactory) {
            this.basicAuthenticationCredentialsExtractor = basicAuthenticationCredentialsExtractor;
            this.httpRequestAuthorizationExtractor = httpRequestAuthorizationExtractor;
            this.membershipProvider = membershipProvider;
            this.responseMessageFactory = responseMessageFactory;
            this.serviceSecurityContextFactory = serviceSecurityContextFactory;
        }

        internal bool AuthenticateRequest(Message requestMessage) {
            string authenticationString;
            if (httpRequestAuthorizationExtractor.TryExtractAuthorizationHeader(requestMessage, out authenticationString)) {
                DisCredentials credentials = basicAuthenticationCredentialsExtractor.Extract(authenticationString);
                membershipProvider.SetCredentials(credentials, requestMessage);
                if (membershipProvider.ValidateUser(credentials.UserName, credentials.Password, credentials.InstallType)) {
                    AddSecurityContextToMessage(requestMessage, credentials);
                    return true;
                }
            }
            return false;
        }

        private void AddSecurityContextToMessage(Message requestMessage, DisCredentials credentials) {
            if (requestMessage.Properties.Security == null) {
                requestMessage.Properties.Security = new SecurityMessageProperty();
            }
            requestMessage.Properties.Security.ServiceSecurityContext = serviceSecurityContextFactory.Create(credentials);
        }

        internal Message CreateInvalidAuthenticationRequest() {
            return responseMessageFactory.CreateInvalidAuthorizationMessage();
        }
    }
}