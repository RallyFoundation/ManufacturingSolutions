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
using System.Linq;
using System.Text;
using DIS.Data.DataContract;
using System.Data.Entity.Infrastructure;

namespace DIS.Data.DataAccess.Repository
{
    public class MiscRepository : RepositoryBase, IMiscRepository
    {
        public MiscRepository() : base() 
        {

        }

        public MiscRepository(string ConnectionString) : base(ConnectionString) 
        {

        }

        private const string table = "KeySyncNotification";
        #region key operation history

        public void InsertKeyOperationHistories(List<KeyInfo> keys, DataContract.KeyState targetKeyState, string @operator, string message)
        {
            using (var context = GetContext())
            {
                foreach (var keyInfo in keys)
                {
                    KeyOperationHistory koh = new KeyOperationHistory()
                    {
                        KeyId = keyInfo.KeyId,
                        ProductKey = keyInfo.ProductKey,
                        HardwareHash = keyInfo.HardwareHash,
                        KeyStateFrom = (byte)keyInfo.KeyState,
                        KeyStateTo = (byte)targetKeyState,
                        Message = message,
                        Operator = @operator,
                        CreatedDate = DateTime.UtcNow
                    };
                    context.KeyOperationHistories.Add(koh);
                }
                context.SaveChanges();
            }
        }

        #endregion

        #region  key duplicated

        public List<KeyDuplicated> GetKeysDuplicated()
        {
            using (var context = GetContext())
            {
                return context.KeysDuplicated.Where(k => !k.Handled).ToList();
            }
        }

        public void InsertKeysDuplicated(List<DataContract.KeyInfo> keys)
        {
            using (var context = GetContext())
            {
                foreach (var keyInfo in keys)
                {
                    KeyDuplicated key = new KeyDuplicated()
                    {
                        KeyId = keyInfo.KeyId,
                        ProductKey = keyInfo.ProductKey,
                        Handled = false,
                        OperationId = null
                    };
                    if (context.KeysDuplicated.Where(k => !k.Handled && k.KeyId == keyInfo.KeyId).Count() <= 0)
                        context.KeysDuplicated.Add(key);
                }
                context.SaveChanges();
            }
        }

        public void UpdateKeysDuplicated(List<KeyDuplicated> keys)
        {
            using (var context = GetContext())
            {
                foreach (KeyDuplicated key in keys)
                {
                    context.KeysDuplicated.Attach(key);
                    context.Entry(key).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        #endregion

        #region key export log

        public PagedList<KeyExportLog> SearchExportLogs(ExportLogSearchCriteria criteria)
        {
            using (var context = GetContext())
            {
                var query = context.KeyExportLogs.Where(log => log.ExportLogId >= 0);
                if (criteria != null)
                {
                    if (criteria.DateFrom != null)
                        query = query.Where(k => k.CreateDate >= criteria.DateFrom);
                    if (criteria.DateTo != null)
                        query = query.Where(k => k.CreateDate <= criteria.DateTo);
                    if (!string.IsNullOrEmpty(criteria.ExportTo))
                        query = query.Where(k => k.ExportTo.Contains(criteria.ExportTo));
                    if (criteria.ExportTypes != null && criteria.ExportTypes.Count() > 0)
                        query = query.Where(k => criteria.ExportTypes.Contains(k.ExportType));
                    if (!string.IsNullOrEmpty(criteria.CreateBy))
                        query = query.Where(k => k.CreateBy == criteria.CreateBy);
                    if (criteria.IsEncrypted != null && criteria.IsEncrypted.HasValue)
                        query = query.Where(k => k.IsEncrypted == criteria.IsEncrypted);
                    if (!string.IsNullOrEmpty(criteria.FileName))
                        query = query.Where(k => k.FileName.Contains(criteria.FileName));
                    query = query.SortBy(criteria.SortBy, criteria.SortByDesc);
                }
                PagedList<int> logIds = new PagedList<int>(query.Select(l => l.ExportLogId), criteria.StartIndex, criteria.PageSize);
                Dictionary<int, KeyExportLog> dic = context.KeyExportLogs
                    .Where(l => logIds.Contains(l.ExportLogId)).ToDictionary(l => l.ExportLogId, l => l);
                return logIds.Transform(id => dic[id]);
            }
        }

        public KeyExportLog GetExportLog(int logId)
        {
            using (var context = GetContext())
            {
                var query = context.KeyExportLogs.Where(log => log.ExportLogId == logId);
                return query.SingleOrDefault();
            }
        }

        public void InsertExportLog(KeyExportLog exportlog)
        {
            using (var context = GetContext())
            {
                exportlog.CreateDate = DateTime.Now;
                context.KeyExportLogs.Add(exportlog);
                context.SaveChanges();
            }
        }

        #endregion

        #region Key Sync Notification

        public void InsertOrUpdateKeySyncNotifiction(List<KeyInfo> keys, KeyStoreContext context)
        {
            UsingContext(ref context, () =>
            {
                List<KeySyncNotification> dbSyncs = GetKeySyncNotifications(context, keys.Select(k => k.KeyId).ToArray());
                context.Configuration.AutoDetectChangesEnabled = false;
                foreach (var keyInfo in keys)
                {
                    KeySyncNotification dbSync = dbSyncs.SingleOrDefault(s => s.KeyId == keyInfo.KeyId);
                    if (dbSync == null)
                    {
                        context.KeySyncNotifications.Add(new KeySyncNotification()
                        {
                            KeyId = keyInfo.KeyId,
                            KeyState = keyInfo.KeyState,
                            CreateDate = DateTime.UtcNow,
                        });
                    }
                    else
                    {
                        dbSync.KeyState = keyInfo.KeyState;
                        dbSync.CreateDate = DateTime.UtcNow;
                        context.Entry(dbSync).State = EntityState.Modified;
                    }
                }
                context.Configuration.AutoDetectChangesEnabled = true;
            });
        }

        public void DeleteKeySyncNotification(long[] keyIds)
        {
            using (var context = GetContext())
            {
                SetupTempKeyIdTable(context, keyIds);
                context.Database.ExecuteSqlCommand(
                        string.Format("DELETE {0} FROM {0} K JOIN {1} TMP ON K.PRODUCTKEYID = TMP.KEYID", table, tempKeyIdName));
                context.SaveChanges();
            };
        }

        public List<KeySyncNotification> SearchKeySyncNotification(int? hqId)
        {
            using (var context = GetContext())
            {
                var query = GetSyncQuery(context);
                var queryKey = GetKeyQuery(context);
                if (hqId != null)
                    queryKey = queryKey.Where(k => k.HqId == hqId.Value);
                return query.Join(queryKey, sync => sync.KeyId, key => key.KeyId, (sync, key) => sync).ToList();
            };
        }

        private List<KeySyncNotification> GetKeySyncNotifications(KeyStoreContext context, long[] keyIds)
        {
            SetupTempKeyIdTable(context, keyIds);
            return (from k in context.KeySyncNotifications
                    join t in context.TempKeyId on k.KeyId equals t.KeyId
                    select k).ToList();

        }

        private IQueryable<KeySyncNotification> GetSyncQuery(KeyStoreContext context)
        {
            return (from sync in context.KeySyncNotifications
                    select sync);
        }

        private IQueryable<KeyInfoEx> GetKeyQuery(KeyStoreContext context)
        {
            return (from key in context.KeyInfoExes
                    select key);
        }
        #endregion

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }
    }
}
