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
using DIS.Data.DataContract;
using OA3ToolReportKeyInfo = DIS.Data.DataContract.OA3ToolReportKeyInfo;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    public interface IKeyManager : ILocalKeyManager
    {
        #region Online keys

        // Search assigned keys, both in progress or not in progress are included.
        List<KeyInfo> GetAssignedKeys(int ssId);

        // DLS updates it's own keys status after the keys been reported to ULS.
        List<KeyOperationResult> UpdateKeysAfterReporting(List<KeyInfo> keys);

        // ULS receiving recall keys request from DLS.
        void ReceiveKeysForRecalling(List<KeyInfo> keys, int ssId);

        // DLS send recall keys request to ULS.
        List<KeyOperationResult> SendKeysForRecalling(List<KeyInfo> keys, Action<List<KeyInfo>> actionRecallKeys);

        // Invoked when getting keys and reporting cbrs
        void UpdateSyncState(List<KeyInfo> keys, bool isInProgress, KeyStoreContext context = null);

        // It will be invoked in FFKI when oa tool running 'oa3tool.exe /assembly' in command line.
        bool OaToolAssembleKey(KeyInfo key);

        // It will be invoked in FFKI when oa tool running 'oa3tool.exe /report' in command line.
        bool OaToolReportKey(KeyInfo key, KeyState keyState, string hardwareId, OemOptionalInfo oemOptionalInfo, string trackingInfo);

        // Will be invoked when oa tool run 'oa3tool.exe /report' in command line.
        // With SerialNumber support - Rally
        bool OaToolReportKey(KeyInfo key, KeyState keyState, string hardwareId, OemOptionalInfo oemOptionalInfo, string trackingInfo, string serialNumber);

        #endregion

        #region search recall keys

        // Search keys which can be recalled.
        List<KeyInfo> SearchRecallKeys(KeySearchCriteria criteria);

        List<KeyInfo> SearchRecallKeys(List<KeyGroup> keyGroups);

        List<KeyGroup> SearchRecallKeyGroups(KeySearchCriteria criteria);

        #endregion

        #region search online report keys

        List<KeyOperationResult> UpdateKeysAfterRetrieveCbrAck(Cbr cbr, bool isDuplicated = false, KeyStoreContext context = null);

        List<KeyInfo> UpdateKeysAfterRetrieveOhrAck(Ohr ohr, KeyStoreContext context = null);

        // Explaination needed here ...
        void UpdateKeysToCarbonCopy(List<KeyInfo> keys, bool shouldCarbonCopy, KeyStoreContext context = null);

        void UpdateReturnReportToCarbonCopy(List<ReturnReport> returnReports);

        void UpdateReturnReportToCarbonCopyCompleted(ReturnReport returnReport);

        List<KeyInfo> SearchCarbonCopyFulfilledKeys();

        List<KeyInfo> SearchCarbonCopyReportedKeys();

        List<KeyInfo> SearchCarbonCopyReturnedKeys();

        #endregion

        #region Sync keys

        //Send the sync notification to ULS/DLS
        void SendKeySyncNotifications(Func<int?, List<KeySyncNotification>, long[]> send);

        #endregion
    }
}
