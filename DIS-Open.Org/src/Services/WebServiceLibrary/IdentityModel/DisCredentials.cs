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
using DIS.Data.DataContract;
using DIS.Business.Client;

namespace DIS.Services.WebServiceLibrary.IdentityModel
{
    /// <summary>
    /// This class hold the credentials of a user.
    /// </summary>
    public class DisCredentials {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public CallDirection CallDirection { get; set; }

        public InstallType InstallType {
            get {
                switch (CallDirection) {
                    case CallDirection.DownLevelSystem:
                        return InstallType.Uls;
                    case CallDirection.UpLevelSystem:
                        return InstallType.Dls;
                    case CallDirection.None:
                        return Constants.InstallType;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public DisCredentials(string userName, string password) {
            this.UserName = userName;
            this.Password = password;
        }
    }
}