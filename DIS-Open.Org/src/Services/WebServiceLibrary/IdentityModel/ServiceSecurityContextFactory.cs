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
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class is responsible for creating a service security context that gets attached
    /// to the incoming request if successfully validated.
    /// </summary>
    internal class ServiceSecurityContextFactory {
        private readonly AuthorizationPolicyFactory authorizationPolicyFactory;

        public ServiceSecurityContextFactory(AuthorizationPolicyFactory authorizationPolicyFactory) {
            this.authorizationPolicyFactory = authorizationPolicyFactory;
        }

        internal ServiceSecurityContext Create(DisCredentials credentials) {
            List<IAuthorizationPolicy> authorizationPolicies = new List<IAuthorizationPolicy>();
            authorizationPolicies.Add(authorizationPolicyFactory.Create(credentials));
            return new ServiceSecurityContext(authorizationPolicies.AsReadOnly());
        }
    }
}