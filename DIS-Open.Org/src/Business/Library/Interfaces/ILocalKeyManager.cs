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
using OA3ToolReportKeyInfo = DIS.Data.DataContract.OA3ToolReportKeyInfo;
using DIS.Data.ServiceContract;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    public interface ILocalKeyManager
    {
        int? CurrentHeadQuarterId { get; set; }

        PagedList<KeyInfo> SearchKeys(KeySearchCriteria searchCriteria);

        List<KeyInfo> SearchExpiredKeys(int expiredTime);

        #region Offline keys

        // Offline export, assign, use this method
        List<KeyGroup> SearchFulfilledKeyGroups(KeySearchCriteria searchCriteria);

        // Offline export, assign, use this method
        List<KeyInfo> SearchFulfilledKeys(KeySearchCriteria searchCriteria);

        List<KeyGroup> SearchBoundKeyGroups(KeySearchCriteria searchCriteria);

        List<KeyInfo> SearchBoundKeys(KeySearchCriteria searchCriteria);

        List<KeyOperationResult> ExportFulfilledKeys(ExportParameters exportParameters);

        List<KeyOperationResult> ExportBoundKeys(ExportParameters exportParameters);

        List<KeyOperationResult> ExportToolKeys(ExportParameters exportParameters);

        List<KeyOperationResult> ExportReturnKeys(ReturnReport request, ExportParameters exportParameters);

        List<KeyInfo> GetLogFileKeys(int logId);

        List<KeyOperationResult> ReExportKeys(int logId, string filePath);

        // DLS imports fulfilled keys from ULS.
        List<KeyOperationResult> ImportULSFulfilledKeys(string filePath, HeadQuarter currentHeadQuarter, bool isCheckFileKey);

        // ULS imports bound keys from DLS.
        List<KeyOperationResult> ImportDLSBoundKeys(string filePath, List<Subsidiary> subsidiaries, bool isCheckFileKey);

        // FFKI imports bound keys from OA3Tool.
        List<KeyOperationResult> ImportToolKey(string filePath);

        List<KeyOperationResult> ImportReturnAckKeys(string filePath, KeyStoreContext context = null);

        void InsertExportLog(KeyExportLog exportlog);

        List<KeyExportLog> SearchExportLogs(ExportLogSearchCriteria exportLogSearchCriteria);

        // Offline-CBR to microsoft
        List<KeyOperationResult> ExportCbr(ExportParameters exportParameters, Func<List<KeyInfo>, string> generateCbrToFile);

        // Offline OHR Data to microsoft
        List<KeyOperationResult> ExportOHRData(ExportParameters exportParameters, Func<List<KeyInfo>, string> generateOHRDataToFile);

        List<KeyOperationResult> UpdateOemOptionInfo(List<KeyInfo> keys, OemOptionalInfo optionalInfo);

        /// <summary>
        /// Validate key files in batch - Rally Dec 1, 2014
        /// </summary>
        /// <param name="originalFiles"></param>
        /// <returns>Validation results</returns>
        List<KeyOperationResult> ValidateKeyImportOriginalFiles(string[] originalFiles);

        string GetKeyExportMessageTranformationXSLT();

        void SetKeyExportMessageTranformationXSLT(string value);

        string GetKeyImportMessageTranformationXSLT();

        void SetKeyImportMessageTranformationXSLT(string value);

        #endregion

        #region Assign & unassign keys

        // Search keys which can be assigned.
        List<KeyInfo> SearchAssignKeys(KeySearchCriteria searchCriteria);

        List<KeyGroup> SearchAssignKeyGroups(KeySearchCriteria searchCriteria);

        List<KeyInfo> SearchAssignKeys(List<KeyGroup> keyGroups);

        List<KeyInfo> SearchUnassignKeys(KeySearchCriteria searchCriteria);

        List<KeyOperationResult> AssignKeys(List<KeyInfo> keys, int ssId);

        List<KeyOperationResult> AssignKeys(List<KeyGroup> groupKeys, int ssId);

        List<KeyOperationResult> UnassignKeys(List<KeyInfo> keys);

        #endregion

        #region Revert keys

        // Search keys which can be reverted.
        // Only available in FFKI. 
        // Only Consumed and Bound keys can be reverted. 
        List<KeyInfo> SearchKeysToRevert(KeySearchCriteria searchCriteria);

        List<KeyOperationResult> RevertKeys(List<KeyInfo> keys, string operationMsg, string @operator);

        #endregion

        #region Duplicated keys

        // Wait, ignore or change state of duplicated keys.
        void HandleKeysDuplicated(List<KeyDuplicated> keys, string @operator, string message);

        //List<KeyOperationHistory> GetOperationHistories(KeySearchCriteria criteria);

        List<KeyDuplicated> GetKeysDuplicated();

        List<KeyOperationResult> ExportDuplicatedCbr(Cbr cbr, string outputPath, string @operator);

        List<KeyOperationResult> ImportDuplicatedCbr(string outputPath, Action<long[]> action);

        List<KeyOperationHistory> SearchOperationHistories(KeySearchCriteria criteria);

        #endregion

        #region Fulfillment

        // DLS download and save keys from ULS.
        List<KeyOperationResult> SaveKeysAfterGetting(List<KeyInfo> keys, bool isInProgress, bool? shouldBeCarbonCopy = null, KeyStoreContext context = null);

        // ULS receiving sync notification from DLS.
        List<KeyOperationResult> ReceiveSyncNotification(List<KeyInfo> keys);

        #endregion

        #region Carbon copy keys

        // ULS receive and save carbon copy fulfilled keys from DLS.
        void GetAndSaveCarbonCopyFulfilledKeys(List<KeyInfo> keys ,int ssId);

        // ULS receive and save carbon copy reported keys from DLS.
        void GetAndSaveCarbonCopyReportedKeys(List<KeyInfo> keys);

        // ULS receive and save carbon copy return reported keys from DLS.
        void GetAndSaveCarbonCopyReturnReportedKeys(List<KeyInfo> keys);

        // ULS receive and save carbon copy return report from DLS.
        void GetAndSaveCarbonCopyReturnReport(ReturnReport returnReport);

        #endregion

        #region Report binding keys

        List<KeyInfo> SearchBoundKeysToReport(KeySearchCriteria searchCriteria);

        List<KeyGroup> SearchBoundKeyGroupsToReport(KeySearchCriteria searchCriteria);

        List<KeyInfo> SearchBoundKeysToReport(List<KeyGroup> keyGroups);

        List<KeyInfo> SearchBoundKeysToMs(KeySearchCriteria searchCriteria);

        List<KeyInfo> SearchOhrKeysToMs(KeySearchCriteria searchCriteria);

        List<KeyGroup> SearchBoundKeyGroupsToMs(KeySearchCriteria searchCriteria);

        List<KeyInfo> SearchBoundKeysToMs(List<KeyGroup> keyGroups);

        // ULS receive bound keys from DLS.
        List<KeyOperationResult> ReceiveBoundKeys(List<KeyInfo> keys, int fromSsId);

        #endregion

        #region Sync down/up keys state

        //Update the keys in notification to db, and send IDs back to caller
        long[] UpdateKeyStateAfterRecieveSyncNotification(List<KeySyncNotification> keySyncNotifications);

        #endregion

        #region Return Keys

        List<KeyInfo> SearchToReturnKeys(KeySearchCriteria searchCriteria);

        ReturnReport GetFirstSentReturnReport();

        List<ReturnReport> GetReportedReturnReports();

        List<ReturnReport> GetReadyReturnReports();

        List<ReturnReport> GetFailedReturnReports();

        List<ReturnReport> GetCompletedReturnReports();

        void UpdateReturnReportIfSendingFailed(ReturnReport returnReport);

        void UpdateReturnReportIfSearchResultEmpty(ReturnReport returnReport);

        void UpdateReturnsAfterAckReady(List<ReturnReport> returnReports);

        ReturnReport UpdateReturnAfterAckRetrieved(ReturnReport returnReport, KeyStoreContext context);

        void UpdateReturnWhenAckFailed(ReturnReport returnReport);

        List<KeyOperationResult> SaveGeneratedReturnReport(ReturnReport returnReport);

        #endregion
    }
}
