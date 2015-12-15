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
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Web;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class is responsible for creating the http response message when a 
    /// client does not send the appropriate authentication header.
    /// </summary>
    internal class ResponseMessageFactory {
        private const string BasicAuthenticationHeaderName = "WWW-Authenticate";
        private readonly string realm;

        internal ResponseMessageFactory(string realm) {
            this.realm = realm;
        }

        internal virtual Message CreateInvalidAuthorizationMessage() {
            var responseMessage = Message.CreateMessage(MessageVersion.None, null);
            HttpResponseMessageProperty responseProperty = CreateResponseProperty();
            responseMessage.Properties.Add(HttpResponseMessageProperty.Name, responseProperty);
            return responseMessage;
        }

        private HttpResponseMessageProperty CreateResponseProperty() {
            var responseProperty = new HttpResponseMessageProperty();
            responseProperty.StatusCode = HttpStatusCode.Unauthorized;
            responseProperty.Headers.Add(BasicAuthenticationHeaderName, string.Format(CultureInfo.InvariantCulture, "Basic realm=\"{0}\"", realm));
            return responseProperty;
        }
    }
}