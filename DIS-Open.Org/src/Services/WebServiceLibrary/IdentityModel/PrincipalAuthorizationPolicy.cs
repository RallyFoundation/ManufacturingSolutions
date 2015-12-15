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
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class implements an AuthorizationPolicy, this policy is assigned to the
    /// incoming request when it is successfully authenticated.
    /// </summary>
    internal class PrincipalAuthorizationPolicy : IAuthorizationPolicy {
        private readonly IPrincipal principal;

        private readonly string policyId = Guid.NewGuid().ToString();

        public PrincipalAuthorizationPolicy(IPrincipal principal) {
            this.principal = principal;
        }

        public string Id {
            get { return policyId; }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state) {
            evaluationContext.AddClaimSet(this, new DefaultClaimSet(Claim.CreateNameClaim(principal.Identity.Name)));
            evaluationContext.Properties["Identities"] = new List<IIdentity>(new[] { principal.Identity });
            evaluationContext.Properties["Principal"] = principal;
            return true;
        }

        public ClaimSet Issuer {
            get { return ClaimSet.System; }
        }
    }
}