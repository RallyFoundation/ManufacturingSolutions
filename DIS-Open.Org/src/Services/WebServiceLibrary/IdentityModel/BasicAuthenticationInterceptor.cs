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
using Microsoft.ServiceModel.Web;
using System.Net;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class is responsible for the interception of a http request and perform basic 
    /// authentication of that request.
    /// </summary>
    internal class BasicAuthenticationInterceptor : RequestInterceptor {
        private readonly BasicAuthenticationManager manager;

        public BasicAuthenticationInterceptor(BasicAuthenticationManager manager)
            : base(false) {
            this.manager = manager;
        }

        public override void ProcessRequest(ref RequestContext requestContext)
        {
            if (!manager.AuthenticateRequest(requestContext.RequestMessage))
            {
                requestContext.Reply(manager.CreateInvalidAuthenticationRequest());
                requestContext = null;
            }
        }
    }
}