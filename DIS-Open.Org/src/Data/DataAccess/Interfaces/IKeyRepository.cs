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
using System.Data.Objects;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository
{
    /// <summary>
    /// Interface for Key Repository classd
    /// </summary>
    public interface IKeyRepository
    {
        KeyInfo GetKey(long keyId);

        List<KeyInfo> GetKeys(long[] keyIds);

        List<KeyGroup> SearchKeyGroups(params KeySearchCriteria[] criterias);

        PagedList<KeyInfo> SearchKeys(params KeySearchCriteria[] criterias);

        List<KeyInfo> SearchKeys(List<KeyGroup> keyGroups);

        void InsertKey(KeyInfo key, KeyStoreContext context = null);

        void InsertKeys(List<KeyInfo> keys, KeyStoreContext context = null);

        void UpdateKey(KeyInfo key, bool? isInProgress, int? ssId, string hardwareId, OemOptionalInfo oemOptionalInfo, string trackingInfo = null, bool shouldUpdateTrackingInfoIfNull = false);

        //With serial number support - Rally
        void UpdateKey(KeyInfo key, bool? isInProgress, int? ssId, string hardwareId, OemOptionalInfo oemOptionalInfo, string trackingInfo, string serialNumber, bool shouldUpdateTrackingInfoIfNull);

        void UpdateKeys(List<KeyInfo> keys, bool? isInProgress, int? ssId, bool shouldUpdateSsIdIfNull = false, bool? shouldCarbonCopy = null, KeyStoreContext context = null);

        void DeleteKeys(long[] keyIds);

        void UpdateKeys(List<KeyInfo> keys, KeyStoreContext context = null);

        string GetDBConnectionString();
    }
}
