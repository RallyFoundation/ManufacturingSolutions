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

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class is responsible for extracting the credentials 
    /// from a decoded basic authentication header string.
    /// </summary>
    internal class DecodedCredentialsExtractor {
        internal virtual DisCredentials Extract(string credentials) {
            if (!string.IsNullOrEmpty(credentials)) {
                string[] credentialTokens = credentials.Split(':');
                if (credentialTokens.Length == 2) {
                    return new DisCredentials(credentialTokens[0], credentialTokens[1]);
                }
            }

            throw new ArgumentException("The supplied credential string is invalid, it should comply to [username:password]", "credentials");
        }
    }
}