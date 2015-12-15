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
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository
{
    public interface IOhrRepository
    {
        List<Ohr> SearchConfirmedKeys();

        List<Ohr> SearchOhr(OhrStatus? status);

        void InsertOhr(Ohr ohr, KeyStoreContext context = null);

        void UpdateOhr(Ohr ohr, KeyStoreContext context = null);

        void UpdateOhrAck(Ohr ohr, KeyStoreContext context = null);

        Ohr GetOhrByCustomerUniqueId(Guid customerUniqueId);

        string GetDBConnectionString();

    }
}
