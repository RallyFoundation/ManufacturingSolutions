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
using DIS.Business.Client;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class is responsible for extracting the  
    /// authentication string from the http message header.
    /// </summary>
    internal class AuthorizationStringExtractor {
        internal virtual bool TryExtractAuthorizationHeader(Message message, out string authenticationString) {
            HttpRequestMessageProperty requestMessageProperty = (HttpRequestMessageProperty)message.Properties[HttpRequestMessageProperty.Name];
            authenticationString = requestMessageProperty.Headers[ServiceClient.AuthorizationHeaderName];
            return !string.IsNullOrEmpty(authenticationString);
        }
    }
}