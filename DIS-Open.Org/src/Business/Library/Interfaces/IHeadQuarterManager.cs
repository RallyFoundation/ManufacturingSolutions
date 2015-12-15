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

namespace DIS.Business.Library
{
    public interface IHeadQuarterManager
    {
        List<HeadQuarter> GetHeadQuarters();
        List<HeadQuarter> GetHeadQuarters(User user);
        HeadQuarter GetHeadQuarter(int hqId);
        HeadQuarter GetHeadQuarter(string userName);
        void InsertHeadQuarter(HeadQuarter headQuarter);
        void UpdateHeadQuarter(HeadQuarter headQuarter);
        void DeleteHeadQuarter(int headQuarterId);
        void SetAsDefault(User user, HeadQuarter headQuarter);
        bool ValidateHeadQuarter(string userName, string userKey);
    }
}
