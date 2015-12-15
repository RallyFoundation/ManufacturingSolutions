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
using DIS.Business.Library;
using DIS.Data.DataContract;

namespace DIS.Business.Proxy
{
    public interface IKeyProxy : ILocalKeyManager
    {
        #region Fulfillment

        List<FulfillmentInfo> GetFulfillments(List<string> fulfillmentNumbers);

        List<FulfillmentInfo> GetFailedFulfillments(bool shouldIncludeExpired);

        List<Ohr> GetConfirmedOhrs();

        void UpdateOhrAfterNotification(List<Ohr> ohrs);

        #endregion

        #region Online keys

        // Get keys from MS
        int FulfillOrder();

        // Get keys from ULS
        int GetKeysFromUls();

        List<KeyInfo> GetAssignedKeys(int ssId);

        // Invoked by DLS
        List<KeyOperationResult> SendKeysForRecalling(List<KeyInfo> keys);

        List<KeyOperationResult> SendKeysForRecalling(List<KeyGroup> groupKeys);

        List<KeyInfo> SearchRecallKeys(KeySearchCriteria criteria);

        List<KeyGroup> SearchRecallKeyGroups(KeySearchCriteria criteria);

        // Invoked by ULS
        void ReceiveKeysForRecalling(List<KeyInfo> keys, int ssId);

        // Invoked by DLS
        List<KeyOperationResult> SendBoundKeys(List<KeyInfo> keys);

        List<KeyOperationResult> SendBoundKeys(List<KeyGroup> groupKeys);

        void ReceiveSyncNotification(List<KeyInfo> keys);

        List<KeyOperationResult> SendOhrKeys(List<KeyInfo> keys);

        #endregion

        #region Cbrs

        List<Cbr> GetCbrsDuplicated();

        List<Cbr> GetFailedCbrs();

        #endregion

        #region offline import/export keyfile

        //Export fulfilled, bouned, or keys to file
        List<KeyOperationResult> ExportKeys(ExportParameters exportParameters);

        List<KeyOperationResult> ImportDLSBoundKeys(string filePath, bool IsCheckFileSignature);

        //List<KeyOperationResult> ImportDuplicatedCbr(string outputPath);

        //Import offline CBR result from microsoft
        List<KeyOperationResult> ImportCbr(string path, bool isDuplicated = false);

        //import Return keys.Ack from MS
        List<KeyOperationResult> ImportReturnAckKeys(string filePath);

        #endregion

        #region Carbon copy keys

        void SendCarbonCopyFulfilledKeys();

        void SendCarbonCopyReportedKeys();

        void SendCarbonCopyReturnReportedKeys();

        void SendCarbonCopyReturnReport();

        #endregion

        #region Sync keys

        //Send the sync notification to ULS/DLS
        void SendKeySyncNotifications();

        #endregion

        #region Data polling service

        void AutomaticReportKeys();

        void AutomaticGetKeys();

        void DoRecurringTasks();

        void SearchSubmitted();

        #endregion

        List<KeyInfo> GetBoundKeysWithoutOhrData();

        string GetKeyReportMessageTranformationXSLT();

        void SetKeyReportMessageTranformationXSLT(string value);
    }
}
