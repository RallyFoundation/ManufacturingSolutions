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
    /// <summary>
    /// Interface of SubsidiaryRepository
    /// </summary>
    public interface ISubsidiaryRepository
    {
        Subsidiary GetSubsidiary(int ssId);

        Subsidiary GetSubsidiary(string userName);

        List<Subsidiary> GetSubsidiaries(string userName, string userKey);
        
        List<Subsidiary> GetSubsidiaries();
      
        void InsertSubsidiary(Subsidiary subsidiary);
      
        void UpdateSubsidiary(Subsidiary subsidiary);

        void DeleteSubsidiary(int subsidiaryId);

        string GetDBConnectionString();
    }
}
