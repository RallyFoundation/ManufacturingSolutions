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
using System.Security.Principal;
using System.Web;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class is repsonsible for creating a authorization policy based on the 
    /// given credentials.
    /// </summary>
    internal class AuthorizationPolicyFactory {
        public virtual IAuthorizationPolicy Create(DisCredentials credentials) {
            IIdentity identity = new DisIdentity(credentials.UserName, credentials.InstallType);
            IPrincipal genericPrincipal = new GenericPrincipal(identity, new string[] { });
            return new PrincipalAuthorizationPolicy(genericPrincipal);
        }
    }
}