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
using System.Text;
using DIS.Common.Utility;
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataContract;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    public partial class LocalKeyManager : ILocalKeyManager
    {
        public int? CurrentHeadQuarterId { get; set; }
        public HeadQuarter CurrentHeadQuarter { get; set; }

        #region Private members & construct

        protected IKeyRepository keyRepository;
        protected IMiscRepository miscRepository;
        protected IReturnKeyRepository returnKeyRepository;
        protected IKeyTypeConfigurationRepository keyTypeRepository;
        protected ISubsidiaryRepository subsRepository;

        public LocalKeyManager(int? currentHeadQuarterId, string dbConnectionString)
        {
            this.CurrentHeadQuarterId = currentHeadQuarterId;

            if (currentHeadQuarterId == null)
            {
                CurrentHeadQuarter = null;
            }
            else
            {
                HeadQuarterRepository hqRepository = new HeadQuarterRepository(dbConnectionString);

                this.CurrentHeadQuarter = hqRepository.GetHeadQuarter(currentHeadQuarterId.Value);
            }

            this.keyRepository = new KeyRepository(dbConnectionString);

            this.miscRepository = new MiscRepository(dbConnectionString);

            this.returnKeyRepository = new ReturnKeyRepository(dbConnectionString);

            this.keyTypeRepository = new KeyTypeConfigurationRepository(dbConnectionString);

            this.subsRepository = new SubsidiaryRepository(dbConnectionString);
        }

        public LocalKeyManager(int? currentHeadQuarterId)
            : this(currentHeadQuarterId, null, null, null, null, null)
        {
        }

        public LocalKeyManager(int? currentHeadQuarterId,
            IKeyRepository keyRepository, IMiscRepository miscRepository, IReturnKeyRepository returnKeyRepository, KeyTypeConfigurationRepository keyTypeRepository, ISubsidiaryRepository subsRepository)
        {
            CurrentHeadQuarterId = currentHeadQuarterId;
            if (currentHeadQuarterId == null)
                CurrentHeadQuarter = null;
            else
            {
                HeadQuarterRepository hqRepository = new HeadQuarterRepository();
                CurrentHeadQuarter = hqRepository.GetHeadQuarter(currentHeadQuarterId.Value);
            }
            if (keyRepository == null)
                this.keyRepository = new KeyRepository();
            else
                this.keyRepository = keyRepository;

            if (miscRepository == null)
                this.miscRepository = new MiscRepository();
            else
                this.miscRepository = miscRepository;

            if (returnKeyRepository == null)
                this.returnKeyRepository = new ReturnKeyRepository();
            else
                this.returnKeyRepository = returnKeyRepository;
            if (keyTypeRepository == null)
                this.keyTypeRepository = new KeyTypeConfigurationRepository();
            else
                this.keyTypeRepository = keyTypeRepository;

            if (subsRepository == null)
                this.subsRepository = new SubsidiaryRepository();
            else
                this.subsRepository = subsRepository;
        }

        #endregion

        #region Carbon copy

        public void GetAndSaveCarbonCopyFulfilledKeys(List<KeyInfo> keys, int ssId)
        {
            List<KeyInfo> keysToAdd = new List<KeyInfo>();

            List<KeyInfo> keysInDb = GetKeysInDb(keys);
            keys.ForEach(k =>
            {
                KeyInfo currentKey = GetKey(k.KeyId, keysInDb);
                if (currentKey == null)
                {
                    try
                    {
                        k.OemReceivingFulfilledCarbonCopyKey();
                        k.KeyInfoEx = new KeyInfoEx()
                        {
                            IsInProgress = false,
                            KeyInfo = k,
                            KeyType = k.KeyInfoEx.KeyType,
                            ShouldCarbonCopy = true,
                            SsId = ssId
                        };
                        keysToAdd.Add(k);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    }
                }
                else
                {
                    //ignore
                }
            });

            if (keysToAdd.Count > 0)
                keyRepository.InsertKeys(keysToAdd);

        }

        public void GetAndSaveCarbonCopyReportedKeys(List<KeyInfo> keys)
        {
            if (keys.Any(k => k.KeyState != KeyState.ActivationDenied && k.KeyState != KeyState.ActivationEnabled))
                throw new ApplicationException("Retrieved keys are invalid.");

            List<KeyInfo> keysInDb = GetKeysInDb(keys);
            List<KeyInfo> keysToUpdate = new List<KeyInfo>();
            foreach (KeyInfo key in keys)
            {
                KeyInfo currentKey = GetKey(key.KeyId, keysInDb);
                if (currentKey != null)
                {
                    try
                    {
                        currentKey.OemReceivingReportedCarbonCopyKey(key.KeyState == KeyState.ActivationEnabled);
                        currentKey.HardwareHash = key.HardwareHash;
                        currentKey.OemOptionalInfo = key.OemOptionalInfo;
                        currentKey.TrackingInfo = key.TrackingInfo;

                        //Add serial number mapping support - Rally Sept. 23, 2014
                        currentKey.SerialNumber = key.SerialNumber;

                        keysToUpdate.Add(currentKey);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    }
                }
                else
                {
                    MessageLogger.LogSystemError(MessageLogger.GetMethodName(),
                        string.Format("Carbon copy key {0} update is failed.", key.ProductKey), this.keyRepository.GetDBConnectionString());
                }
            }
            if (keysToUpdate.Count > 0)
            {
                keyRepository.UpdateKeys(keysToUpdate);
            }
        }

        public void GetAndSaveCarbonCopyReturnReportedKeys(List<KeyInfo> keys)
        {
            if (keys.Any(k => k.KeyState != KeyState.Returned))
                throw new ApplicationException("Retrieved keys are invalid.");

            List<KeyInfo> keysInDb = GetKeysInDb(keys);
            List<KeyInfo> keysToUpdate = new List<KeyInfo>();
            foreach (KeyInfo key in keys)
            {
                KeyInfo currentKey = GetKey(key.KeyId, keysInDb);
                if (currentKey != null)
                {
                    try
                    {
                        currentKey.OemReceivingReturnReportedCarbonCopyKey();
                        keysToUpdate.Add(currentKey);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    }
                }
                else
                {
                    MessageLogger.LogSystemError(MessageLogger.GetMethodName(),
                        string.Format("Carbon copy key {0} update is failed.", key.KeyId), this.keyRepository.GetDBConnectionString());
                }
            }
            if (keysToUpdate.Count > 0)
            {
                keyRepository.UpdateKeys(keysToUpdate);
            }
        }

        public void GetAndSaveCarbonCopyReturnReport(ReturnReport returnReport)
        {
            returnReport.ReturnReportStatus = ReturnReportStatus.Completed;
            ReturnReport returndb = returnKeyRepository.GetReturnKeyByCustomerId(returnReport.CustomerReturnUniqueId);
            if (returndb == null)
                returnKeyRepository.InsertReturnReportAndKeys(returnReport);
            else
            {
                returnReport.ReturnReportKeys = null;
                returnKeyRepository.UpdateReturnKeyAck(returnReport, null);
            }
        }

        public long[] UpdateKeyStateAfterRecieveSyncNotification(List<KeySyncNotification> keySyncNotifications)
        {
            List<KeyInfo> keysInDb = GetKeysInDb(keySyncNotifications.Select(k => k.KeyId));

            if (keysInDb == null || keysInDb.Count == 0)
                throw new ApplicationException("Update keys are not found.");

            List<KeyOperationResult> results = new List<KeyOperationResult>();

            foreach (KeySyncNotification sync in keySyncNotifications)
            {
                KeyInfo key = GetKey(sync.KeyId, keysInDb);
                KeyOperationResult result = new KeyOperationResult
                {
                    Failed = false,
                    KeyInDb = key,
                };
                try
                {
                    key.DlsRecieveSync(sync.KeyState);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    result.Failed = true;
                    result.FailedType = KeyErrorType.StateInvalid;
                }
                results.Add(result);
            }

            List<KeyInfo> keysToUpdate = results.Where(k => !k.Failed).Select(k => k.KeyInDb).ToList();

            if (keysToUpdate.Count > 0)
            {
                keyRepository.UpdateKeys(keysToUpdate);
                if ((Constants.InstallType & InstallType.Uls) != 0)
                {
                    InsertOrUpdateKeySyncNotifiction(keysToUpdate);
                }
            }
            return keysToUpdate.Select(k => k.KeyId).ToArray();
        }

        #endregion

        #region Save and update keys methods

        public List<KeyOperationResult> UpdateOemOptionInfo(List<KeyInfo> keys, OemOptionalInfo optionalInfo)
        {
            ValidateIfEmpty(keys);
            if (keys.Any(k => k.KeyState != KeyState.Bound))
                throw new ApplicationException("States are invalid when saving keys.");

            var result = new List<KeyOperationResult>();

            foreach (var key in keys)
            {
                KeyErrorType keyErrorType = KeyErrorType.None;
                KeyInfo currentKey = keyRepository.GetKey(key.KeyId);
                if (currentKey == null)
                    keyErrorType = KeyErrorType.NotFound;
                else if (currentKey.KeyState != KeyState.Bound)
                    keyErrorType = KeyErrorType.StateInvalid;
                else
                    keyRepository.UpdateKey(key, null, null, null, optionalInfo);

                result.Add(new KeyOperationResult()
                {
                    Failed = (keyErrorType != KeyErrorType.None),
                    FailedType = keyErrorType,
                    Key = key,
                    KeyInDb = currentKey
                });
            }
            return result;
        }

        public List<KeyOperationResult> SaveKeysAfterGetting(List<KeyInfo> keys, bool isInProgress, bool? shouldBeCarbonCopy = null, KeyStoreContext context = null)
        {
            ValidateIfEmpty(keys);

            List<KeyInfo> keysToAdd = new List<KeyInfo>();
            List<KeyOperationResult> result = new List<KeyOperationResult>();

            List<KeyInfo> keysInDb = GetKeysInDb(keys);
            List<KeyTypeConfiguration> keyTypeConfigs = keyTypeRepository.GetKeyTypeConfigurations(CurrentHeadQuarterId);
            foreach (var key in keys)
            {
                KeyTypeConfiguration keyTypeConfig = keyTypeConfigs.SingleOrDefault(k => k.LicensablePartNumber == key.LicensablePartNumber);
                KeyErrorType keyErrorType = KeyErrorType.None;
                if (key.KeyInfoEx == null)
                {
                    key.KeyInfoEx = new KeyInfoEx();
                }
                key.KeyInfoEx.IsInProgress = isInProgress;
                key.KeyInfoEx.KeyInfo = key;
                key.KeyInfoEx.HqId = CurrentHeadQuarterId;
                key.KeyInfoEx.ShouldCarbonCopy = shouldBeCarbonCopy;
                if (key.KeyInfoEx.KeyType == null)
                {
                    key.KeyInfoEx.KeyType = (keyTypeConfig == null ? null : keyTypeConfig.KeyType);
                }
                var currentKey = GetKey(key.KeyId, keysInDb);
                try
                {
                    key.RetrievingKeys();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    keyErrorType = KeyErrorType.StateInvalid;
                }
                if (currentKey == null)
                    keysToAdd.Add(key);
                else
                {
                    MessageLogger.LogSystemError(MessageLogger.GetMethodName(),
                        string.Format("Key {0} already exist.", key.ProductKey), this.keyRepository.GetDBConnectionString());
                    keyErrorType = KeyErrorType.Invalid;
                }
                result.Add(new KeyOperationResult
                {
                    Failed = (keyErrorType != KeyErrorType.None),
                    FailedType = keyErrorType,
                    Key = key,
                    KeyInDb = currentKey
                });
            }

            if (keysToAdd.Count > 0)
                keyRepository.InsertKeys(keysToAdd, context);
            return result;
        }

        // Invoked by ULS.
        public List<KeyOperationResult> ReceiveSyncNotification(List<KeyInfo> keys)
        {
            ValidateIfEmpty(keys);

            List<KeyInfo> keysInDb = GetKeysInDb(keys);
            List<KeyInfo> keysToUpdate = new List<KeyInfo>();
            foreach (KeyInfo key in keys)
            {
                KeyInfo currentKey = GetKey(key.KeyId, keysInDb);
                if (currentKey == null)
                    throw new ApplicationException(string.Format("Key {0} cannot be found when syncing keys.", key.ProductKey));

                try
                {
                    currentKey.UlsReceivingFulfilledKeySync();
                    keysToUpdate.Add(currentKey);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                }
            }

            if (keysToUpdate.Count > 0)
                keyRepository.UpdateKeys(keysToUpdate, false, null);

            return keysToUpdate.Select(k => new KeyOperationResult()
            {
                Failed = false,
                Key = new KeyInfo() { KeyId = k.KeyId, KeyInfoEx = k.KeyInfoEx },
                FailedType = KeyErrorType.None
            }).ToList();
        }

        public List<KeyOperationResult> ReceiveBoundKeys(List<KeyInfo> keys, int fromSsId)
        {
            if (keys.Any(k => k.KeyState != KeyState.Bound))
                throw new ApplicationException("Received keys are invalid.");

            KeyInfoComparer comparer = new KeyInfoComparer();

            List<KeyInfo> succeedKeys = UpdateKeysAfterBeingReported(keys, fromSsId);
            return keys.Select(k => new KeyOperationResult()
            {
                Failed = !succeedKeys.Contains(k, comparer),
                Key = new KeyInfo() { KeyId = k.KeyId },
                FailedType = succeedKeys.Contains(k, comparer) ? KeyErrorType.None : KeyErrorType.SsIdInvalid
            }).ToList();
        }

        //ULS recieved report from DLS, invoked by ULS
        protected List<KeyInfo> UpdateKeysAfterBeingReported(List<KeyInfo> keys, int fromSsId)
        {
            ValidateIfEmpty(keys);

            List<KeyInfo> succeedKeys = new List<KeyInfo>();
            List<KeyInfo> keysToUpdate = new List<KeyInfo>();
            var dbKeys = GetKeysInDb(keys);
            foreach (var key in keys)
            {
                var dbKey = GetKey(key.KeyId, dbKeys);
                if (dbKey.KeyInfoEx.SsId == fromSsId)
                {
                    KeyState[] validStates = new KeyState[] {
                        KeyState.Bound,
                        KeyState.NotifiedBound,
                        KeyState.ReportedBound,
                        KeyState.ActivationEnabled,
                        KeyState.ActivationDenied,
                        KeyState.ReportedReturn,
                        KeyState.Returned
                    };
                    try
                    {
                        if (!validStates.Contains(dbKey.KeyState))
                        {
                            dbKey.UlsReceivingBoundKey();
                            dbKey.HardwareHash = key.HardwareHash;
                            dbKey.OemOptionalInfo = key.OemOptionalInfo;
                            dbKey.TrackingInfo = key.TrackingInfo;

                            //Add serial number mapping support - Rally Sept. 23, 2014
                            dbKey.SerialNumber = key.SerialNumber;

                            keysToUpdate.Add(dbKey);
                        }
                        succeedKeys.Add(dbKey);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    }
                }
            }
            if (keysToUpdate.Any())
                keyRepository.UpdateKeys(keysToUpdate);
            return succeedKeys;
        }

        protected void UpdateKeysAfterReportBinding(Cbr cbr, KeyStoreContext context)
        {
            List<KeyInfo> keys = GetKeysInDb(cbr.CbrKeys.Select(k => k.KeyId));
            if (keys != null && keys.Count > 0)
            {
                foreach (KeyInfo key in keys)
                {
                    try
                    {
                        key.UlsReportingBoundKeyToMs();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    }
                }
                keyRepository.UpdateKeys(keys, false, null, context: context);
            }
        }

        protected void UpdateKeysAfterReportOhr(Ohr ohr, KeyStoreContext context)
        {
            List<KeyInfo> keys = GetKeysInDb(ohr.Keys.Select(k => k.KeyId));
            if (keys != null && keys.Count > 0)
            {
                foreach (KeyInfo key in keys)
                {
                    try
                    {
                        key.UlsReportingOhrToMs();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    }
                }
                keyRepository.UpdateKeys(keys, false, null, context: context);
            }
        }

        protected void UpdateKeysAfterReturnReport(List<ReturnReportKey> returnReportKeys, KeyStoreContext context)
        {
            if (returnReportKeys != null && returnReportKeys.Count > 0)
            {
                List<KeyInfo> keysToUpdate = GetKeysInDb(returnReportKeys.Select(k => k.KeyId));
                foreach (var key in keysToUpdate)
                {
                    try
                    {
                        key.ULsReturningKey();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                    }
                }

                keyRepository.UpdateKeys(keysToUpdate, false, null, context: context);
            }
        }

        public List<KeyOperationResult> UpdateKeysAfterRetrieveReturnReportAck(ReturnReport returnReport, KeyStoreContext context = null)
        {
            var keys = returnReport.ReturnReportKeys;
            List<KeyInfo> keysInDb = GetKeysInDb(keys.Select(k => k.KeyId));
            if (keysInDb == null || keysInDb.Count == 0)
                throw new ApplicationException("Update keys after retrieve cbr ack are not found.");

            List<KeyOperationResult> results = keys.Select(key => GenerateKeyOperationResult(
                new KeyInfo()
                {
                    KeyId = key.KeyId,
                    KeyState = KeyState.Returned
                },
                keysInDb, (k1, k2) => ValidateKeyAfterRetrieveAck(k1, k2))).ToList();

            List<KeyInfo> validKeys = results.Where(r => !r.Failed).Select(r => r.KeyInDb ?? r.Key).ToList();
            foreach (KeyInfo key in validKeys)
            {
                try
                {
                    key.UlsReceivingReturnAck(keys.Single(k => k.KeyId == key.KeyId));
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
                //Insert key sync notification data except fulfilled keys return
                List<long> fulfilledKeyIds = keys.Where(k => k.PreProductKeyStateId == (byte)KeyState.Fulfilled).Select(k => k.KeyId).ToList();
                List<KeyInfo> keysToSync = keysToUpdate.Where(k => k.KeyState == KeyState.Returned && (!fulfilledKeyIds.Contains(k.KeyId))).ToList();
                if (keysToSync.Count > 0)
                    InsertOrUpdateKeySyncNotifiction(keysToSync, context);
            }
            results.ForEach(k => k.Key.ProductKey = (k.KeyInDb == null ? null : k.KeyInDb.ProductKey));
            return results;
        }

        protected KeyErrorType ValidateKeyAfterRetrieveAck(KeyInfo key, KeyInfo keyInDb)
        {
            if (keyInDb == null)
                return KeyErrorType.NotFound;
            else
                return KeyErrorType.None;
        }

        public List<KeyInfo> SearchExpiredKeys(int expiredTime)
        {
            List<KeyState> keyStates = new List<KeyState>();
            keyStates.Add(KeyState.Assigned);
            keyStates.Add(KeyState.Bound);
            keyStates.Add(KeyState.Consumed);
            keyStates.Add(KeyState.Fulfilled);
            keyStates.Add(KeyState.Invalid);
            keyStates.Add(KeyState.NotifiedBound);
            keyStates.Add(KeyState.ReportedBound);
            keyStates.Add(KeyState.ReportedReturn);
            keyStates.Add(KeyState.Retrieved);

            return keyRepository.SearchKeys(ConvertSearchCriteria(new KeySearchCriteria() { KeyStates = keyStates, DateTo = DateTime.Today.AddDays(-expiredTime) }));
        }

        #endregion

        #region Private Methods

        protected void ValidateIfEmpty(List<KeyInfo> keys)
        {
            if (keys == null || keys.Count == 0)
                throw new ApplicationException("Passed in keys list is empty.");
        }

        protected KeySearchCriteria ConvertSearchCriteria(KeySearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
                throw new ApplicationException("Search criteria is null.");

            var criteria = (KeySearchCriteria)searchCriteria.Clone();
            criteria.HqId = CurrentHeadQuarterId;
            if (criteria.DateFrom.HasValue && criteria.DateFrom.Value < SqlDateTime.MinValue.Value)
                criteria.DateFrom = SqlDateTime.MinValue.Value;
            if (criteria.DateTo.HasValue)
                criteria.DateTo = (criteria.DateTo.Value > SqlDateTime.MaxValue.Value ?
                    SqlDateTime.MaxValue.Value : criteria.DateTo.Value.AddDays(1));
            if (criteria.OemRmaDateTo.HasValue)
                criteria.OemRmaDateTo = (criteria.OemRmaDateTo.Value > SqlDateTime.MaxValue.Value ?
                    SqlDateTime.MaxValue.Value : criteria.OemRmaDateTo.Value.AddDays(1));
            if (criteria.PageSize < 0)
                criteria.PageSize = KeySearchCriteria.DefaultPageSize;
            else if (criteria.PageSize == 0)
                criteria.PageSize = int.MaxValue;
            return criteria;
        }

        protected KeyInfo GetKey(long keyId, List<KeyInfo> keysInDb)
        {
            return keysInDb.SingleOrDefault(k => k.KeyId == keyId);
        }

        protected List<KeyInfo> GetKeysInDb(IEnumerable<KeyInfo> keys)
        {
            return keyRepository.GetKeys(keys.Select(k => k.KeyId).ToArray());
        }

        protected List<KeyInfo> GetKeysInDb(IEnumerable<long> keyIds)
        {
            return keyRepository.GetKeys(keyIds.ToArray());
        }

        protected void InsertOrUpdateKeySyncNotifiction(List<KeyInfo> keys, KeyStoreContext context = null)
        {
            var keysToSync = new List<KeyInfo>();
            var keysGroupBySsId = keys.Where(k => k.KeyInfoEx.SsId != null).GroupBy(k => k.KeyInfoEx.SsId);
            var validSubsidiaries = GetValidSubsidiaries();
            foreach (var keysBySsId in keysGroupBySsId)
            {
                if (IsValidSubsidary(validSubsidiaries, keysBySsId.Key.Value))
                {
                    keysToSync.AddRange(keysBySsId);
                }
            }
            miscRepository.InsertOrUpdateKeySyncNotifiction(keysToSync, context);
        }

        private int[] GetValidSubsidiaries()
        {
            return subsRepository.GetSubsidiaries().Where(s => IsValidSubsidaryServiceHostUrl(s.ServiceHostUrl))
                .Select(s => s.SsId).ToArray();
        }

        private bool IsValidSubsidary(int[] validSubsidiaries, int ssId)
        {
            return validSubsidiaries.Contains(ssId);
        }

        private bool IsValidSubsidaryServiceHostUrl(string serviceHostUrl)
        {
            if (!string.IsNullOrEmpty(serviceHostUrl))
            {
                try
                {
                    Uri serviceUrl = new Uri(serviceHostUrl);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        #endregion
    }
}
