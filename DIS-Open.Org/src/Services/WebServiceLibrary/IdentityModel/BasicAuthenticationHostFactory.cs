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
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using Microsoft.ServiceModel.Web;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    /// <summary>
    /// This class is responsible for creating a servicehost that includes a basic 
    /// authentication request interceptor.
    /// </summary>
    public abstract class BasicAuthenticationHostFactory : ServiceHostFactory {
        private const string realm = "DISWebService";
        protected abstract IMembershipProvider membershipProvider { get; }

        protected override ServiceHost CreateServiceHost(Type serviceType,
                Uri[] baseAddresses) {
            WebServiceHost2 serviceHost = new WebServiceHost2(serviceType, true, baseAddresses);
            serviceHost.Interceptors.Add(RequestInterceptorFactory.Create(realm, membershipProvider));
            return serviceHost;
        }
    }
}