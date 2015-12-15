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
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using DIS.Data.DataContract;
using System.Data.Linq.SqlClient;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace DIS.Data.DataAccess.Repository
{
    /// <summary>
    /// Key Repository class implementing interface IKeyRepository
    /// </summary>
    public class KeyRepository : RepositoryBase, IKeyRepository
    {
        public KeyRepository() : base() 
        {

        }

        public KeyRepository(string ConnectionString) : base(ConnectionString) 
        {

        }
        public KeyInfo GetKey(long keyId)
        {
            using (var context = GetContext())
            {
                KeyInfo key = GetKey(context, keyId);
                return key;
            }
        }

        public List<KeyInfo> GetKeys(long[] keyIds)
        {
            List<KeyInfo> ret = new List<KeyInfo>();
            using (var context = GetContext())
            {
                ret = GetKeys(context, keyIds);
            }
            return ret;
        }

        public List<KeyGroup> SearchKeyGroups(params KeySearchCriteria[] criterias)
        {
            if (criterias == null || criterias.Length == 0)
                throw new ApplicationException("Search criteria is null.");

            using (var context = GetContext())
            {
                IQueryable<KeyInfo> query = null;
                foreach (KeySearchCriteria c in criterias)
                {
                    KeySearchCriteria criteria = c;
                    var subQuery = GetQuery(context, criteria);
                    query = query == null ? subQuery : query.Union(subQuery);
                }
                List<KeyGroup> result = query.GroupBy(k => new { k.LicensablePartNumber, OemPartNumber = (string.IsNullOrEmpty(k.OemPartNumber) ? null : k.OemPartNumber), k.OemPoNumber, k.KeyInfoEx.KeyTypeId })
                    .Select(g => new KeyGroup()
                    {
                        OEMPONumber = g.Key.OemPoNumber,
                        OEMPartNumber = g.Key.OemPartNumber,
                        MsLicensablePartNumber = g.Key.LicensablePartNumber,
                        KeyTypeId = g.Key.KeyTypeId,
                        AvailableKeysCount = g.Count()
                    }).ToList();
                result.ForEach(kg => kg.Criterias = criterias.Select(c => (KeySearchCriteria)c.Clone()).ToArray());
                return result;
            }
        }

        public PagedList<KeyInfo> SearchKeys(params KeySearchCriteria[] criterias)
        {
            if (criterias == null || criterias.Length == 0)
                throw new ApplicationException("Search criteria is null.");

            var groups = criterias.GroupBy(c => new { c.PageSize, c.StartIndex, c.SortBy, c.SortByDesc });
            if (groups.Count() != 1)
                throw new ApplicationException("Paging parameters are invalid.");
            var commonCriteria = groups.Single().Key;

            using (var context = GetContext())
            {
                IQueryable<KeyInfo> query = null;
                foreach (KeySearchCriteria c in criterias)
                {
                    KeySearchCriteria criteria = c;
                    var subQuery = GetQuery(context, criteria);
                    query = query == null ? subQuery : query.Union(subQuery);
                }
                query = query.SortBy(commonCriteria.SortBy, commonCriteria.SortByDesc);

                return new PagedList<KeyInfo>(query, commonCriteria.StartIndex, commonCriteria.PageSize);
            }
        }

        public List<KeyInfo> SearchKeys(List<KeyGroup> keyGroups)
        {
            if (keyGroups == null)
                throw new ApplicationException("Key groups cannot be null.");

            const int keyGroupLimit = 10;
            List<KeyInfo> result = new List<KeyInfo>();
            for (int i = 0; i < keyGroups.Count; i += keyGroupLimit)
            {
                result.AddRange(SearchKeysInternal(keyGroups.Skip(i).Take(keyGroupLimit).ToList()));
            }
            return result;
        }

        /// <summary>
        /// Solve the error with EntityFrameWork: "Some part of your SQL statement is nested too deeply." 
        /// Rewrite the query and break it up into smaller queries.
        /// </summary>
        /// <param name="keyGroups"></param>
        /// <returns></returns>
        private List<KeyInfo> SearchKeysInternal(List<KeyGroup> keyGroups)
        {
            List<KeySearchCriteria> criterias = keyGroups.SelectMany(g => g.ToSearchCriterias()).ToList();
            using (var context = GetContext())
            {
                IQueryable<KeyInfo> query = null;
                var criteriaGroups = criterias.GroupBy(c => new { OEMPONumber = c.OemPoNumber, OEMPartNumber = c.OemPartNumber, MSPartNumber = c.MsPartNumber, c.PageSize });
                foreach (var g in criteriaGroups)
                {
                    IQueryable<KeyInfo> keyGroupQuery = null;
                    foreach (KeySearchCriteria c in g)
                    {
                        KeySearchCriteria criteria = c;
                        if (string.IsNullOrEmpty(c.OemPartNumber))
                            criteria.HasOemPartNumberNull = true;
                        var subQuery = GetQuery(context, criteria);
                        keyGroupQuery = keyGroupQuery == null ? subQuery : keyGroupQuery.Union(subQuery);
                    }
                    keyGroupQuery = keyGroupQuery.SortBy("KeyId,FulfilledDateUtc", false);
                    keyGroupQuery = keyGroupQuery.Take(g.Key.PageSize);
                    query = query == null ? keyGroupQuery : query.Union(keyGroupQuery);
                }
                return query.ToList();
            }
        }

        private IQueryable<KeyInfo> GetQuery(KeyStoreContext context, KeySearchCriteria criteria)
        {
            DbQuery<KeyInfo> dbQuery = context.KeyInfoes.Include("KeyInfoEx.Subsidiary");
            if (criteria.ShouldIncludeHistories)
                dbQuery = dbQuery.Include("KeyHistories");
            if (criteria.ShouldIncludeReturnReport)
                dbQuery = dbQuery.Include("ReturnReportKeys.ReturnReport");
           
             

            IQueryable<KeyInfo> query = dbQuery;
            if (criteria.KeyIds != null)
                query = query.Where(k => criteria.KeyIds.Contains(k.KeyId));

            if (criteria.KeyStateIds != null)
                query = query.Where(k => criteria.KeyStateIds.Contains(k.KeyStateId));

            if (criteria.DateFrom != null)
                query = query.Where(k => k.FulfilledDateUtc >= criteria.DateFromUtc);

            if (criteria.DateTo != null)
                query = query.Where(k => k.FulfilledDateUtc <= criteria.DateToUtc);

            if (criteria.IsInProgress != null)
                query = query.Where(k => k.KeyInfoEx.IsInProgress == criteria.IsInProgress);

            if (criteria.IsAssign != null && !criteria.IsAssign.Value)
                query = query.Where(k => k.KeyInfoEx.SsId == null);

            if (criteria.SsId != null)
                query = query.Where(k => k.KeyInfoEx.SsId == criteria.SsId);

            if (criteria.HqId != null)
                query = query.Where(k => k.KeyInfoEx.HqId == criteria.HqId);

            if (!string.IsNullOrEmpty(criteria.ProductKeyID))
                query = query.Where(k => SqlFunctions.StringConvert((decimal)k.KeyId, 15).Contains(criteria.ProductKeyID));

            if (criteria.ProductKeyIDFrom.HasValue)
                query = query.Where(k => k.KeyId >= criteria.ProductKeyIDFrom.Value);

            if (criteria.ProductKeyIDTo.HasValue)
                query = query.Where(k => k.KeyId <= criteria.ProductKeyIDTo.Value);

            if (!string.IsNullOrEmpty(criteria.ProductKey))
                query = query.Where(k => k.ProductKey.Contains(criteria.ProductKey));

            if (criteria.HasHardwareHash)
                query = query.Where(k => k.HardwareHash != null && k.HardwareHash != string.Empty);

            if (!string.IsNullOrEmpty(criteria.HardwareHash))
                query = query.Where(k => k.HardwareHash.Contains(criteria.HardwareHash));

            if (!string.IsNullOrEmpty(criteria.MsOrderNumber))
                query = query.Where(k => k.MsOrderNumber.Contains(criteria.MsOrderNumber));

            if (!string.IsNullOrEmpty(criteria.MsPartNumber))
                query = query.Where(k => k.LicensablePartNumber.Contains(criteria.MsPartNumber));

            if (!string.IsNullOrEmpty(criteria.OemPoNumber))
                query = query.Where(k => k.OemPoNumber.Contains(criteria.OemPoNumber));

            if (criteria.HasOemPartNumberNull && string.IsNullOrEmpty(criteria.OemPartNumber))
                query = query.Where(k => k.OemPartNumber == null || k.OemPartNumber == string.Empty);

            if (!string.IsNullOrEmpty(criteria.OemPartNumber))
                query = query.Where(k => k.OemPartNumber.Contains(criteria.OemPartNumber));

            if (!string.IsNullOrEmpty(criteria.ReferenceNumber))
                query = query.Where(k => k.CallOffReferenceNumber.Contains(criteria.ReferenceNumber));

            if (!string.IsNullOrEmpty(criteria.TrakingInfo))
                query = query.Where(k => k.TrackingInfo == criteria.TrakingInfo);

            if (!string.IsNullOrEmpty(criteria.ZCHANNEL_REL_ID))
                query = query.Where(k => k.ZCHANNEL_REL_ID == criteria.ZCHANNEL_REL_ID);

            if (!string.IsNullOrEmpty(criteria.ZMAUF_GEO_LOC))
                query = query.Where(k => k.ZMANUF_GEO_LOC == criteria.ZMAUF_GEO_LOC);

            if (!string.IsNullOrEmpty(criteria.ZOEM_EXT_ID))
                query = query.Where(k => k.ZOEM_EXT_ID == criteria.ZOEM_EXT_ID);

            if (!string.IsNullOrEmpty(criteria.ZPC_MODEL_SKU))
                query = query.Where(k => k.ZPC_MODEL_SKU == criteria.ZPC_MODEL_SKU);

            if (!string.IsNullOrEmpty(criteria.ZPGM_ELIG_VALUES))
                query = query.Where(k => k.ZPGM_ELIG_VALUES == criteria.ZPGM_ELIG_VALUES);

            if (!string.IsNullOrEmpty(criteria.ZFRM_FACTOR_CL1))
                query = query.Where(k => k.ZFRM_FACTOR_CL1 == criteria.ZFRM_FACTOR_CL1);

            if (!string.IsNullOrEmpty(criteria.ZFRM_FACTOR_CL2))
                query = query.Where(k => k.ZFRM_FACTOR_CL2 == criteria.ZFRM_FACTOR_CL2);

            if (!string.IsNullOrEmpty(criteria.ZSCREEN_SIZE))
                query = query.Where(k => k.ZSCREEN_SIZE == criteria.ZSCREEN_SIZE);

            if (!string.IsNullOrEmpty(criteria.ZTOUCH_SCREEN))
                query = query.Where(k => k.ZTOUCH_SCREEN == criteria.ZTOUCH_SCREEN);

            if (criteria.HasOhrData != null)
                query = query.Where(k => (
                    k.ZFRM_FACTOR_CL1 != null && k.ZFRM_FACTOR_CL1 != string.Empty &&
                    k.ZFRM_FACTOR_CL2 != null && k.ZFRM_FACTOR_CL2 != string.Empty &&
                    k.ZSCREEN_SIZE != null && k.ZSCREEN_SIZE != string.Empty &&
                    k.ZTOUCH_SCREEN != null && k.ZTOUCH_SCREEN != string.Empty &&
                    k.ZPC_MODEL_SKU != null && k.ZPC_MODEL_SKU != string.Empty) == criteria.HasOhrData.Value);

            if (criteria.OemRmaDateFrom != null && criteria.OemRmaDateTo != null)
                query = query.Where(k => k.ReturnReportKeys.Any(r => r.ReturnReport.OemRmaDateUTC >= criteria.OemRmaDateFromUtc && r.ReturnReport.OemRmaDateUTC <= criteria.OemRmaDateToUtc));
            else
            {
                if (criteria.OemRmaDateFrom != null)
                    query = query.Where(k => k.ReturnReportKeys.Any(r => r.ReturnReport.OemRmaDateUTC >= criteria.OemRmaDateFromUtc));
                if (criteria.OemRmaDateTo != null)
                    query = query.Where(k => k.ReturnReportKeys.Any(r => r.ReturnReport.OemRmaDateUTC <= criteria.OemRmaDateToUtc));
            }

            //Add serial number suport - Rally Sept. 24, 2014
            if (!string.IsNullOrEmpty(criteria.SerialNumber))
            {
                query = query.Where(k => k.SerialNumber == criteria.SerialNumber);
            }
           
            if (Constants.InstallType == InstallType.Oem)
            {
                if (criteria.ShouldCarbonCopy == null)
                    query = query.Where(k => k.KeyInfoEx.ShouldCarbonCopy == null);
            }
            else
            {
                if (criteria.ShouldCarbonCopy != null)
                    query = query.Where(k => k.KeyInfoEx.ShouldCarbonCopy == criteria.ShouldCarbonCopy);
            }

            if (criteria.KeyType != null)
            {
                List<int> keyTypes = new List<int>();
                if (((int)criteria.KeyType & (int)KeyType.Standard) != 0)
                    keyTypes.Add((int)KeyType.Standard);
                if (((int)criteria.KeyType & (int)KeyType.MBR) != 0)
                    keyTypes.Add((int)KeyType.MBR);
                if (((int)criteria.KeyType & (int)KeyType.MAT) != 0)
                    keyTypes.Add((int)KeyType.MAT);
                query = query.Where(k => keyTypes.Contains(k.KeyInfoEx.KeyTypeId.Value));
            }

            if (!string.IsNullOrEmpty(criteria.OemRmaNumber) || criteria.HasNoCredit != null || criteria.ReturnReportStatus != null)
            {
                IQueryable<ReturnReport> returnQuery = context.ReturnReports.Include("ReturnReportKeys");
                if (criteria.ReturnReportStatus != null)
                    returnQuery = returnQuery.Where(r => r.ReturnReportStatusId == (int)criteria.ReturnReportStatus);
                if (!string.IsNullOrEmpty(criteria.OemRmaNumber))
                    returnQuery = returnQuery.Where(r => r.OemRmaNumber == criteria.OemRmaNumber);
                List<long> keyIds = null;
                if (criteria.HasNoCredit == null)
                    keyIds = returnQuery.ToList().SelectMany(r => r.ReturnReportKeys).Where(k => !string.IsNullOrEmpty(k.ReturnReasonCode) && (k.ReturnReasonCode.StartsWith("O") || k.ReturnReasonCode.StartsWith("Q"))).Select(rk => rk.KeyId).Distinct().ToList();
                else
                {
                    // returnQuery = returnQuery.Where(r => r.ReturnNoCredit == criteria.HasNoCredit.Value);
                    if (criteria.HasNoCredit.Value)
                        keyIds = returnQuery.ToList().SelectMany(r => r.ReturnReportKeys).Where(k => !string.IsNullOrEmpty(k.ReturnReasonCode) && k.ReturnReasonCode.StartsWith("Q")).Select(rk => rk.KeyId).Distinct().ToList();
                    else
                        keyIds = returnQuery.ToList().SelectMany(r => r.ReturnReportKeys).Where(k => !string.IsNullOrEmpty(k.ReturnReasonCode) && k.ReturnReasonCode.StartsWith("O")).Select(rk => rk.KeyId).Distinct().ToList();
                }
                query = query.Where(k => keyIds.Contains(k.KeyId));
            }

            return query;
        }


        public void InsertKey(KeyInfo key, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                InsertKey(context, key);
            });
        }

        public void InsertKeys(List<KeyInfo> keys, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                foreach (KeyInfo key in keys)
                {
                    KeyInfo keyInfo = key;
                    InsertKey(context, keyInfo);
                }
                context.Configuration.AutoDetectChangesEnabled = true;
            });
        }

        public void UpdateKey(KeyInfo key, bool? isInProgress, int? ssId, string hardwareId, OemOptionalInfo oemOptionalInfo, string trackingInfo = null, bool shouldUpdateTrackingInfoIfNull = false)
        {
            using (var context = GetContext())
            {
                KeyInfo dbKey = GetKey(context, key.KeyId);
                if (dbKey != null)
                {
                    if (key.KeyStateChanged)
                        dbKey.CopyKeyState(key);

                    if (isInProgress != null)
                        dbKey.KeyInfoEx.IsInProgress = isInProgress.Value;
                    if (ssId != null)
                        dbKey.KeyInfoEx.SsId = ssId;
                    if (hardwareId == string.Empty)
                        dbKey.HardwareHash = null;
                    if (!string.IsNullOrEmpty(hardwareId))
                        dbKey.HardwareHash = hardwareId;
                    if (oemOptionalInfo != null)
                        dbKey.OemOptionalInfo = oemOptionalInfo;
                    if (shouldUpdateTrackingInfoIfNull || trackingInfo != null)
                        dbKey.TrackingInfo = trackingInfo;

                    if (dbKey.KeyStateChanged)
                    {
                        InsertKeyHistory(context, key);
                        dbKey.ModifiedDate = DateTime.Now;
                    }
                }
                context.SaveChanges();
            }
        }

        //With serial number support - Rally
        public void UpdateKey(KeyInfo key, bool? isInProgress, int? ssId, string hardwareId, OemOptionalInfo oemOptionalInfo, string trackingInfo, string serialNumber, bool shouldUpdateTrackingInfoIfNull)
        {
            using (var context = GetContext())
            {
                KeyInfo dbKey = GetKey(context, key.KeyId);
                if (dbKey != null)
                {
                    if (key.KeyStateChanged)
                        dbKey.CopyKeyState(key);

                    if (isInProgress != null)
                        dbKey.KeyInfoEx.IsInProgress = isInProgress.Value;
                    if (ssId != null)
                        dbKey.KeyInfoEx.SsId = ssId;
                    if (hardwareId == string.Empty)
                        dbKey.HardwareHash = null;
                    if (!string.IsNullOrEmpty(hardwareId))
                        dbKey.HardwareHash = hardwareId;
                    if (oemOptionalInfo != null)
                        dbKey.OemOptionalInfo = oemOptionalInfo;
                    if (shouldUpdateTrackingInfoIfNull || trackingInfo != null)
                        dbKey.TrackingInfo = trackingInfo;

                    //SerialNumber support -Rally
                    if (!string.IsNullOrEmpty(serialNumber))
                    {
                        dbKey.SerialNumber = serialNumber;
                    }

                    if (dbKey.KeyStateChanged)
                    {
                        InsertKeyHistory(context, key);
                        dbKey.ModifiedDate = DateTime.Now;
                    }
                }
                context.SaveChanges();
            }
        }

        public void UpdateKeys(List<KeyInfo> keys, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                List<KeyInfo> dbKeys = GetKeys(context, keys.Select(k => k.KeyId).ToArray());
                Func<KeyInfo, KeyInfo, KeyInfo> updateKey = (dbKey, key) =>
                {
                    if (key.KeyStateChanged)
                        dbKey.CopyKeyState(key);

                    if (key.HardwareHash != null)
                        dbKey.HardwareHash = key.HardwareHash;
                    if (key.OemOptionalInfo != null)
                        dbKey.OemOptionalInfo = key.OemOptionalInfo;
                    if (key.TrackingInfo != null)
                        dbKey.TrackingInfo = key.TrackingInfo;
                    if (dbKey.KeyStateChanged)
                    {
                        InsertKeyHistory(context, key);
                        dbKey.ModifiedDate = DateTime.Now;
                    }

                    //Add serial number mapping support - Rally Sept. 23, 2014
                    if (key.SerialNumber != null)
                    {
                        dbKey.SerialNumber = key.SerialNumber;
                    }

                    context.Entry(dbKey).State = EntityState.Modified;
                    return dbKey;
                };

                var tmp = (from dbKey in dbKeys
                           join key in keys on dbKey.KeyId equals key.KeyId
                           select updateKey(dbKey, key)).ToList();

                context.Configuration.AutoDetectChangesEnabled = true;
            });
        }

        public void UpdateKeys(List<KeyInfo> keys, bool? isInProgress, int? ssId, bool shouldUpdateSsIdIfNull = false, bool? shouldCarbonCopy = null, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                SetupTempKeyIdTable(context, keys);
                string sql = @"
                        DECLARE @isInProgress bit
                        DECLARE @ssid int
                        DECLARE @shouldCarbonCopy bit
                        DECLARE @shouldUpdateSsIdIfNull bit

                        SET @isInProgress= {0}
                        SET @ssid= {1}
                        SET @shouldCarbonCopy= {2}
                        SET @shouldUpdateSsIdIfNull= {3}

                        DECLARE @shouldBeUpdateKeyState int
                        SELECT @shouldBeUpdateKeyState = COUNT(1) FROM TempKeyId WHERE KeyState IS NOT NULL

                        IF @shouldBeUpdateKeyState > 0
                        BEGIN
                            INSERT INTO KeyHistory(ProductKeyID, ProductKeyStateID, StateChangeDate)
                            SELECT k.ProductKeyID, tmp.KeyState, GETDATE() FROM ProductKeyInfo k JOIN TempKeyId tmp
                            ON k.ProductKeyId = tmp.KeyId AND k.ProductKeyStateID <> tmp.KeyState

                            Update ProductKeyInfo 
                            SET ProductKeyStateID = tmp.KeyState, ProductKeyState = s.KeyState, ModifiedDate = GETDATE()
                            FROM ProductKeyInfo k JOIN TempKeyId tmp ON k.ProductKeyId = tmp.KeyId
                            JOIN KeyState s ON tmp.KeyState = s.KeyStateId
                            AND k.ProductKeyStateID <> tmp.KeyState
                        END

                        UPDATE KeyInfoEx 
                        SET IsInProgress = ISNULL(@isInProgress, k.IsInProgress)
                        , SSID = CASE WHEN @shouldUpdateSsIdIfNull = 1 THEN @ssid ELSE ISNULL(@ssid, k.SSID) END
                        , ShouldCarbonCopy = ISNULL(@shouldCarbonCopy, k.ShouldCarbonCopy)
                        FROM KeyInfoEx k JOIN TempKeyId tmp ON k.ProductKeyId = tmp.KeyId

                        DECLARE @shouldBeUpdateKeyType int
                        SELECT @shouldBeUpdateKeyType = COUNT(1) FROM TempKeyId WHERE KeyType IS NOT NULL

                        IF @shouldBeUpdateKeyType > 0
                            UPDATE KeyInfoEx 
                            SET KeyType = ISNULL(tmp.KeyType, k.KeyType)
                            FROM KeyInfoEx k JOIN TempKeyId tmp ON k.ProductKeyId = tmp.KeyId";

                context.Database.ExecuteSqlCommand(sql
                    , (byte?)NullableConvert(isInProgress, Convert.ToByte)
                    , (int?)NullableConvert(ssId, Convert.ToInt32)
                    , (byte?)NullableConvert(shouldCarbonCopy, Convert.ToByte)
                    , Convert.ToByte(shouldUpdateSsIdIfNull));
            });
        }

        private Nullable<TTarget> NullableConvert<TSource, TTarget>(
                  Nullable<TSource> source, Func<TSource, TTarget> converter)
            where TTarget : struct
            where TSource : struct
        {
            return source.HasValue ?
                       (Nullable<TTarget>)converter(source.Value) :
                       null;
        }

        public void DeleteKeys(long[] keyIds)
        {
            string[] tableNames = new string[] {
                "DuplicatedKey",
                "KeyOperationHistory",
                "KeyHistory",
                "KeyInfoEx",
                "ProductKeyInfo",
            };

            using (var context = GetContext())
            {
                SetupTempKeyIdTable(context, keyIds);
                foreach (var table in tableNames)
                {
                    context.Database.ExecuteSqlCommand(
                        string.Format("DELETE {0} FROM {0} K JOIN {1} TMP ON K.PRODUCTKEYID = TMP.KEYID", table, tempKeyIdName));
                }
                context.SaveChanges();
            }
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }

        #region private method

        private List<KeyInfo> GetKeys(KeyStoreContext context, long[] keyIds)
        {
            SetupTempKeyIdTable(context, keyIds);
            return ((DbQuery<KeyInfo>)(from k in context.KeyInfoes
                                       join t in context.TempKeyId on k.KeyId equals t.KeyId
                                       select k)).Include("KeyInfoEx").Include("KeyHistories").ToList();
        }

        private KeyInfo GetKey(KeyStoreContext context, long keyId)
        {
            return GetKeysQuery(context).FirstOrDefault(k => k.KeyId == keyId);
        }

        private void InsertKey(KeyStoreContext context, KeyInfo key)
        {
            key.CreatedDate = DateTime.Now;
            context.KeyInfoes.Add(key);
            context.KeyInfoExes.Add(key.KeyInfoEx);
            InsertKeyHistory(context, key);
        }

        private void InsertKeyHistory(KeyStoreContext context, KeyInfo key)
        {
            KeyHistory history = new KeyHistory()
            {
                KeyId = key.KeyId,
                KeyStateId = key.KeyStateId,
                StateChangeDate = DateTime.Now
            };
            context.KeyHistories.Add(history);
        }

        private IQueryable<KeyInfo> GetKeysQuery(KeyStoreContext context)
        {
            return context.KeyInfoes.Include("KeyInfoEx").Include("KeyHistories");
        }

        private void ValidateUpdateKeys(int keysInDb, int keysToUpdate)
        {
            if (keysInDb < keysToUpdate)
                throw new ApplicationException("Some keys cannot be found.");
        }

        #endregion
    }
}
