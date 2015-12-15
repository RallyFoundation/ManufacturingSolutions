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
    public interface ICbrRepository
    {
        List<Cbr> SearchCbrs(CbrSearchCriteria criteria);

        void InsertCbrAndCbrKeys(Cbr cbr, KeyStoreContext context = null);

        void UpdateCbr(Cbr cbr, KeyStoreContext context = null);

        void UpdateCbrAck(Cbr cbr, bool IsDuplicateImport = false, KeyStoreContext context = null);

        void UpdateCbrsDuplicated(Cbr cbr);

        void UpdateCbrKeys(long[] keyIds);

        Cbr GetCbr(Guid customerUniqueId);

        CbrDuplicated GetDuplicatedCbr(Guid customerUniqueId);

        void DeleteCbrsDuplicated(Cbr cbr);

        string GetDBConnectionString();
    }
}
