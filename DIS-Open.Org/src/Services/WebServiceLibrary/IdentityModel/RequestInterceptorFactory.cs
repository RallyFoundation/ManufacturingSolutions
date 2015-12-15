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
using System.Web;
using System.Web.Security;
using Microsoft.ServiceModel.Web;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    public static class RequestInterceptorFactory {
        public static RequestInterceptor Create(string realm, IMembershipProvider membershipProvider) {
            var basicAuthenticationCredentialsExtractor = new BasicAuthenticationCredentialsExtractor(new Base64Decoder(), new DecodedCredentialsExtractor());
            var httpRequestAuthorizationExtractor = new AuthorizationStringExtractor();
            var responseMessageFactory = new ResponseMessageFactory(realm);
            var serviceSecurityContextFactory = new ServiceSecurityContextFactory(new AuthorizationPolicyFactory());
            var basicAuthenticationManager = new BasicAuthenticationManager(basicAuthenticationCredentialsExtractor, httpRequestAuthorizationExtractor, membershipProvider, responseMessageFactory, serviceSecurityContextFactory);
            return new BasicAuthenticationInterceptor(basicAuthenticationManager);
        }
    }
}