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
    public interface IMiscRepository
    {
        #region Key Operation History

        void InsertKeyOperationHistories(List<KeyInfo> keys, KeyState targetKeyState, string @operator, string message);

        #endregion

        #region Key Duplicated

        List<KeyDuplicated> GetKeysDuplicated();
        
        void InsertKeysDuplicated(List<KeyInfo> keys);

        void UpdateKeysDuplicated(List<KeyDuplicated> keys);

        #endregion

        #region Key Export Log

        // TODO: add criteria
        PagedList<KeyExportLog> SearchExportLogs(ExportLogSearchCriteria criteria);

        KeyExportLog GetExportLog(int logId);

        void InsertExportLog(KeyExportLog exportlog);

        #endregion

        #region

        void InsertOrUpdateKeySyncNotifiction(List<KeyInfo> keys, KeyStoreContext context);

        void DeleteKeySyncNotification(long[] keyIds);

        List<KeySyncNotification> SearchKeySyncNotification(int? hqId);

        #endregion

        string GetDBConnectionString();
    }
}
