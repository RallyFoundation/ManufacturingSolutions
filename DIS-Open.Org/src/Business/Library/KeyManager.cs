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
using System.Data.SqlTypes;
using System.Linq;
using DIS.Common.Utility;
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataContract;
using OA3ToolReportKeyInfo = DIS.Data.DataContract.OA3ToolReportKeyInfo;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    /// <summary>
    /// Key Manager DIS.Business.Library class for Db and service calls   
    /// </summary>
    public class KeyManager : LocalKeyManager, IKeyManager
    {
        #region Private members & construct

        public KeyManager(int? CurrentHeadQuarterId)
            : base(CurrentHeadQuarterId)
        {
        }

        public KeyManager(int? CurrentHeadQuarterId, string dbConnectionString)
            : base(CurrentHeadQuarterId, dbConnectionString)
        {
        }

        #endregion

        #region Recall keys

        public List<KeyInfo> SearchRecallKeys(KeySearchCriteria criteria)
        {
            return keyRepository.SearchKeys(
                GetRecallKeySearchCriteria(criteria));
        }

        public List<KeyGroup> SearchRecallKeyGroups(KeySearchCriteria criteria)
        {
            return keyRepository.SearchKeyGroups(
                GetRecallKeySearchCriteria(criteria));
        }

        public List<KeyInfo> SearchRecallKeys(List<KeyGroup> keyGroups)
        {
            return keyRepository.SearchKeys(keyGroups);
        }

        public List<KeyOperationResult> SendKeysForRecalling(List<KeyInfo> keys,
            Action<List<KeyInfo>> actionRecallKeys)
        {
            if (keys.Count > Constants.BatchLimit)
                throw new ApplicationException("Keys to be recalled exceed batch limit.");
            if (keys.Any(k => k.KeyState != KeyState.Fulfilled))
                throw new ApplicationException("The key whose state isn't fulfilled cannot be recalled.");
            List<KeyInfo> keysIndb = base.GetKeysInDb(keys);
            if (keysIndb.Any(k => k.KeyState != KeyState.Fulfilled))
                throw new DisException("ExportKeys_DataChangeError");

            UpdateSyncState(keys, true);
            var results = new List<KeyOperationResult>();
            try
            {
                actionRecallKeys(keys);
                keyRepository.DeleteKeys(keys.Select(k => k.KeyId).ToArray());
                results.AddRange(keys.Select(k => new KeyOperationResult()
                {
                    Failed = false,
                    Key = k
                }));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                results.AddRange(keys.Select(k => new KeyOperationResult()
                {
                    Failed = true,
                    FailedType = KeyErrorType.NetworkFailure,
                    Key = k
                }));
            }
            return results;
        }

        public void ReceiveKeysForRecalling(List<KeyInfo> keys, int ssId)
        {
            List<KeyInfo> dbKeys = keyRepository.SearchKeys(new KeySearchCriteria()
            {
                KeyIds = keys.Select(k => k.KeyId).ToList(),
                SsId = ssId,
                PageSize = Constants.BatchLimit
            }).ToList();

            if (dbKeys.Count < keys.Count)
                throw new ApplicationException("There are some keys which not available to recall.");

            List<KeyInfo> validKeys = new List<KeyInfo>();
            foreach (KeyInfo key in dbKeys)
            {
                try
                {
                    key.UlsReceivingRecallRequest();
                    validKeys.Add(key);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                }
            }
            keyRepository.UpdateKeys(validKeys, false, null, true);
        }

        #endregion

        #region Get keys
        // Search assigned keys, both in progress or not in progress are included.
        public List<KeyInfo> GetAssignedKeys(int ssId)
        {
            List<KeyInfo> keysGet = keyRepository.SearchKeys(new KeySearchCriteria()
            {
                KeyState = KeyState.Assigned,
                SsId = ssId,
                PageSize = Constants.BatchLimit
            });
            UpdateKeysInProgress(keysGet, true);
            return keysGet;
        }

        protected int GetAndSaveKeys(Func<List<KeyInfo>> getKeys, Action<List<KeyInfo>> syncAllocatedKeys)
        {
            int total = 0;
            List<KeyInfo> keys;
            while (true)
            {
                // send syncs first
                PagedList<KeyInfo> keysToSync = keyRepository.SearchKeys(new KeySearchCriteria()
                {
                    PageSize = Constants.BatchLimit,
                    KeyState = KeyState.Fulfilled,
                    IsInProgress = true,
                });
                if (keysToSync.Count > 0)
                {
                    syncAllocatedKeys(keysToSync.ToSyncServiceContract());
                    UpdateSyncState(keysToSync, false);
                }
                else
                { // if there is no keys to sync, start getting keys once
                    keys = getKeys();
                    if (keys.Any())
                    {
                        if (keys.Any(k => k.KeyState != KeyState.Assigned))
                        {
                            keys = keys.Where(k => k.KeyState == KeyState.Assigned).ToList();
                            ExceptionHandler.HandleException(new ApplicationException("Retrieved keys are invalid."), this.keyRepository.GetDBConnectionString());
                        }

                        SaveKeysAfterGetting(keys, true);
                        total += keys.Count;
                    }
                    else
                        return total;
                }
            }
        }

        //invoke during get keys and report cbrs
        public void UpdateSyncState(List<KeyInfo> keys, bool isInProgress, KeyStoreContext context = null)
        {
            keyRepository.UpdateKeys(keys, isInProgress, null, context:context);
        }

        #endregion

        #region Report keys

        //DLS report to ULS, invoked by DLS
        public List<KeyOperationResult> UpdateKeysAfterReporting(List<KeyInfo> keys)
        {
            ValidateIfEmpty(keys);

            List<KeyOperationResult> result = new List<KeyOperationResult>();
            foreach (KeyInfo key in keys)
            {
                try
                {
                    key.DlsReportingBoundKeyToUls();
                    result.Add(new KeyOperationResult()
                    {
                        Failed = false,
                        Key = key,
                        KeyInDb = key,
                    });
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    result.Add(new KeyOperationResult()
                    {
                        Failed = true,
                        Key = key,
                        KeyInDb = key,
                        FailedType = KeyErrorType.StateInvalid
                    });
                }
            }

            keyRepository.UpdateKeys(result.Where(r => !r.Failed).Select(r => r.KeyInDb ?? r.Key).ToList(), false, null);
            return result;
        }

        public List<KeyOperationResult> UpdateKeysAfterRetrieveCbrAck(Cbr cbr, bool isDuplicated = false, KeyStoreContext context = null)
        {
            var keys = cbr.CbrKeys;
            List<KeyInfo> keysInDb = GetKeysInDb(keys.Select(k => k.KeyId).ToArray());
            if (keysInDb == null || keysInDb.Count == 0)
                throw new ApplicationException("Update keys after retrieve cbr ack are not found.");

            List<KeyOperationResult> results = keys.Select(key => GenerateKeyOperationResult(
                 new KeyInfo()
                 {
                     KeyId = key.KeyId,
                     KeyState = (key.ReasonCode == Constants.CBRAckReasonCode.ActivationEnabled ? KeyState.ActivationEnabled : KeyState.ActivationDenied)
                 },
                keysInDb, (k1, k2) => ValidateKeyAfterRetrieveAck(k1, k2))).ToList();

            List<KeyInfo> validKeys = results.Where(r => !r.Failed).Select(r => r.KeyInDb ?? r.Key).ToList();
            foreach (KeyInfo key in validKeys)
            {
                try
                {
                    key.UlsReceivingCbrAck(keys.Single(k => k.KeyId == key.KeyId).ReasonCode == Constants.CBRAckReasonCode.ActivationEnabled, isDuplicated);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    KeyOperationResult result = results.Single(r => r.Key.KeyId == key.KeyId);
                    result.Failed = true;
                    result.FailedType = KeyErrorType.StateInvalid;
                }
            }
            List<KeyInfo> keysToUpdate = results.Where(r => !r.Failed).Select(r => r.KeyInDb ?? r.Key).ToList();
            if (keysToUpdate.Count > 0)
            {
                keyRepository.UpdateKeys(keysToUpdate, false, null, false, null, context);
                //Insert key sync notification data
                InsertOrUpdateKeySyncNotifiction(keysToUpdate, context);
            }
            results.ForEach(k => k.Key.ProductKey = (k.KeyInDb == null ? null : k.KeyInDb.ProductKey));
            return results;
        }

        public List<KeyInfo> UpdateKeysAfterRetrieveOhrAck(Ohr ohr, KeyStoreContext context = null)
        {
            var keys = ohr.Keys;
            List<KeyInfo> keysInDb = GetKeysInDb(keys.Select(k => k.KeyId).ToArray());
            if (keysInDb == null || keysInDb.Count == 0)
                throw new ApplicationException("Update keys after retrieve ohr ack are not found.");

            foreach (KeyInfo key in keysInDb)
            {
                try
                {
                    key.UlsReceivingOhrAck();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                }
            }

            if (keysInDb.Count > 0)
            {
                foreach (var key in keysInDb)
                {
                    UpdateKeysOhrDataAfterRetrieveOhrAck(key, ohr);
                }
                keyRepository.UpdateKeys(keysInDb);
            }

            return keysInDb;

        }

        private void UpdateKeysOhrDataAfterRetrieveOhrAck(KeyInfo key, Ohr ohr)
        {
            if (ohr.OhrKeys.Any(o => 
                o.KeyId == key.KeyId && 
                o.Name == OhrName.ProductKeyID &&
                !string.IsNullOrEmpty(o.ReasonCode) && o.ReasonCode != Constants.CBRAckReasonCode.ActivationEnabled))
                return;

            var toUpdateKeys = ohr.OhrKeys.Where(o =>
                (o.KeyId == key.KeyId) &&
                (
                o.Name == OemOptionalInfo.ZFrmFactorCl1Name ||
                o.Name == OemOptionalInfo.ZFrmFactorCl2Name ||
                o.Name == OemOptionalInfo.ZTouchScreenName ||
                o.Name == OemOptionalInfo.ZScreenSizeName ||
                o.Name == OemOptionalInfo.ZPcModelSkuName) && 
                (string.IsNullOrEmpty(o.ReasonCode) || o.ReasonCode == Constants.CBRAckReasonCode.ActivationEnabled)).ToList();
            
            if (toUpdateKeys.Any())
            {
                foreach (var o in toUpdateKeys)
                {
                    key.UpdateOhrData(o.Name, o.Value);
                }
            }
        }

        #endregion

        #region OaTool

        // Will be invoked when oa tool run 'oa3tool.exe /assembly' in command line.
        public bool OaToolAssembleKey(KeyInfo key)
        {
            bool isFailed = false;
            try
            {
                key.FactoryFloorAssembleKey();
                keyRepository.UpdateKey(key, null, null, null, null);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                isFailed = true;
            }
            return !isFailed;
        }

        // Will be invoked when oa tool run 'oa3tool.exe /report' in command line.
        public bool OaToolReportKey(KeyInfo key, KeyState keyState, string hardwareId, OemOptionalInfo oemOptionalInfo, string trackingInfo)
        {
            bool isFailed = false;

            if (keyState != KeyState.Bound && keyState != KeyState.Returned)
                throw new NotSupportedException(string.Format("oa3tool.exe to {0} is not supported", keyState));

            if (keyState == KeyState.Returned)
            {
                var keyInDb = GetKeysInDb(new[] { key }).First();
                if (keyInDb.KeyInfoEx.IsInProgress == true)
                    throw new NotSupportedException(string.Format("key {0} has in progress", key.KeyId));
            }

            try
            {
                bool isBound = keyState == KeyState.Bound;
                key.FactoryFloorBoundKey(isBound);
                if (isBound)
                    keyRepository.UpdateKey(key, null, null, hardwareId, oemOptionalInfo, trackingInfo, true);
                else
                    keyRepository.UpdateKey(key, null, null, string.Empty, new OemOptionalInfo(), null, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                isFailed = true;
            }
            return !isFailed;
        }

        // Will be invoked when oa tool run 'oa3tool.exe /report' in command line.
        // with SerialNumber support -Rally
        public bool OaToolReportKey(KeyInfo key, KeyState keyState, string hardwareId, OemOptionalInfo oemOptionalInfo, string trackingInfo, string serialNumber)
        {
            bool isFailed = false;

            if (keyState != KeyState.Bound && keyState != KeyState.Returned)
                throw new NotSupportedException(string.Format("oa3tool.exe to {0} is not supported", keyState));

            if (keyState == KeyState.Returned)
            {
                var keyInDb = GetKeysInDb(new[] { key }).First();
                if (keyInDb.KeyInfoEx.IsInProgress == true)
                    throw new NotSupportedException(string.Format("key {0} has in progress", key.KeyId));
            }

            try
            {
                bool isBound = keyState == KeyState.Bound;
                key.FactoryFloorBoundKey(isBound);
                if (isBound)
                    keyRepository.UpdateKey(key, null, null, hardwareId, oemOptionalInfo, trackingInfo, serialNumber, true);
                else
                    keyRepository.UpdateKey(key, null, null, string.Empty, new OemOptionalInfo(), null, null, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                isFailed = true;
            }
            return !isFailed;
        }

        #endregion

        #region Carbon Copy keys

        public void UpdateKeysToCarbonCopy(List<KeyInfo> keys, bool shouldCarbonCopy, KeyStoreContext context = null)
        {
            if (keys != null && keys.Count > 0)
                keyRepository.UpdateKeys(keys, null, null, false, shouldCarbonCopy, context);
        }

        public List<KeyInfo> SearchCarbonCopyFulfilledKeys()
        {
            return keyRepository.SearchKeys(new KeySearchCriteria()
            {
                KeyType = KeyType.All,
                ShouldCarbonCopy = true,
                PageSize = Constants.BatchLimit,
            });
        }

        public List<KeyInfo> SearchCarbonCopyReportedKeys()
        {
            return keyRepository.SearchKeys(new KeySearchCriteria()
            {
                KeyStates = new List<KeyState> { KeyState.ActivationDenied, KeyState.ActivationEnabled },
                ShouldCarbonCopy = true,
                PageSize = Constants.BatchLimit,
            });
        }

        public List<KeyInfo> SearchCarbonCopyReturnedKeys()
        {
            return keyRepository.SearchKeys(new KeySearchCriteria()
            {
                KeyStates = new List<KeyState> { KeyState.Returned },
                ShouldCarbonCopy = true,
                PageSize = Constants.BatchLimit,
            });
        }

        #endregion

        #region Sync keys

        public void SendKeySyncNotifications(Func<int?, List<KeySyncNotification>, long[]> send)
        {
            List<KeySyncNotification> syncs = miscRepository.SearchKeySyncNotification(CurrentHeadQuarterId);
            if (syncs.Count > 0)
            {
                List<KeyInfo> keys = GetKeysInDb(syncs.Select(s => s.KeyId));
                var keysGroupBySsId = keys.GroupBy(k => k.KeyInfoEx.SsId);
                long[] keyIdsToUpdate = new long[] { };
                foreach (var keysBySsId in keysGroupBySsId)
                {
                    long[] succKeyIds = send(keysBySsId.Key, GetSyncs(syncs, keysBySsId.ToList()));
                    Array.Resize(ref keyIdsToUpdate, keyIdsToUpdate.Length + succKeyIds.Length);
                    Array.Copy(succKeyIds, keyIdsToUpdate, succKeyIds.Length);
                }
                miscRepository.DeleteKeySyncNotification(keyIdsToUpdate);
            }
        }

        #endregion

        #region Private methods

        private void UpdateKeysInProgress(List<KeyInfo> keys, bool inProgress)
        {
            keyRepository.UpdateKeys(keys, inProgress, null);
        }

        private KeySearchCriteria GetRecallKeySearchCriteria(KeySearchCriteria criteria)
        {
            criteria.KeyState = KeyState.Fulfilled;
            criteria.IsInProgress = false;
            criteria.HqId = CurrentHeadQuarterId; ;
            return ConvertSearchCriteria(criteria);
        }

        private List<KeySyncNotification> GetSyncs(List<KeySyncNotification> syncs, List<KeyInfo> keys)
        {
            return (from s in syncs
                    join key in keys on s.KeyId equals key.KeyId
                    select s).ToList();
        }

        #endregion
    }
}
