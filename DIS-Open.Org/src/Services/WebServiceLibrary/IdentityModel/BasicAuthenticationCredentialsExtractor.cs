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
    /// This class is responsible for extracting and decoding the 
    /// credentials from the encoded authorization string.
    /// </summary>
    internal class BasicAuthenticationCredentialsExtractor {
        private readonly Base64Decoder decoder;
        private readonly DecodedCredentialsExtractor extractor;

        internal BasicAuthenticationCredentialsExtractor(Base64Decoder decoder, DecodedCredentialsExtractor extractor) {
            this.decoder = decoder;
            this.extractor = extractor;
        }

        internal virtual DisCredentials Extract(string basicAuthenticationCredentials) {
            string authenticationString = RemoveBasicFromAuthenticationString(basicAuthenticationCredentials);
            return extractor.Extract(decoder.Decode(authenticationString));
        }

        private static string RemoveBasicFromAuthenticationString(string basicAuthenticationCredentials) {
            return basicAuthenticationCredentials.Replace("Basic", string.Empty);
        }
    }
}