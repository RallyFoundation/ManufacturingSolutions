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
using System.Text;
using System.Security.Principal;
using DIS.Data.DataContract;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    public class DisIdentity : IIdentity {
        private string name;
        private InstallType installType;

        public string AuthenticationType {
            get { return string.Empty; }
        }

        public bool IsAuthenticated {
            get { return true; }
        }

        public string Name {
            get { return name; }
        }

        public string UlsName {
            get { return installType == InstallType.Uls ? Name : null; }
        }

        public string DlsName {
            get { return installType == InstallType.Dls ? Name : null; }
        }

        public DisIdentity(string userName, InstallType installType) {
            this.name = userName;
            this.installType = installType;
        }
    }
}
