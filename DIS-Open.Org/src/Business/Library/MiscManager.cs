using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIS.Data.DataContract;
using DIS.Data.DataAccess.Repository;

namespace DIS.Business.Library
{
   public class MiscManager
    {
       private IMiscRepository miscRepository;

       public MiscManager()
       {
           if (miscRepository == null)
               this.miscRepository = new MiscRepository();
       }
       public void InsertKeyOperationHistories(List<KeyInfo> keys, KeyState targetKeyState, string @operator, string message)
       {
           miscRepository.InsertKeyOperationHistories(keys,targetKeyState,@operator,message);
       }

       public List<KeyDuplicated> GetKeysDuplicated()
       {
           return miscRepository.GetKeysDuplicated();
       }

       public void InsertKeysDuplicated(List<KeyInfo> keys)
       {
           miscRepository.InsertKeysDuplicated(keys);
       }

       public void UpdateKeysDuplicated(List<KeyDuplicated> keys, string @operator, string message)
       {
           miscRepository.UpdateKeysDuplicated(keys,@operator,message);
       }

       public PagedList<KeyExportLog> SearchExportLogs(ExportLogSearchCriteria criteria)
       {
           return miscRepository.SearchExportLogs(criteria);
       }

       public KeyExportLog GetExportLog(int logId)
       {
           return miscRepository.GetExportLog(logId);
       }

       public void InsertExportLog(KeyExportLog exportlog)
       {
           miscRepository.InsertExportLog(exportlog);
       }
        //public void HandleDuplicatedKeys(List<KeyDuplicated> keys, string @operator, string message)
        //{
        //    using (var context = GetContext())
        //    {
        //        List<long> keyIds = keys.Select(k => k.KeyId).ToList();
        //        List<KeyDuplicated> keysToHandle = context.DuplicatedKeys.Where(k => keyIds.Contains(k.KeyId)).ToList();
        //        foreach (KeyDuplicated key in keys)
        //        {
        //            KeyInfo dbKey = GetKey(context, key.KeyId);
        //            if (key.ReuseOperation == ReuseOperation.Reuse)
        //            {
        //                if ((KeyState)dbKey.KeyStateId != KeyState.NotifiedBound)
        //                {
        //                    MessageLogger.LogSystemError(MessageLogger.GetMethodName(), "DuplicatedKey state is invalid.");
        //                    continue;
        //                }
        //                dbKey.KeyStateId = (byte)key.NewState;
        //                dbKey.KeyState = key.NewState.ToString();
        //                dbKey.KeyInfoEx.SsId = null;
        //            }

        //            KeyDuplicated dupKeyInDb = keysToHandle.Single(k => k.KeyId == key.KeyId);
        //            KeyOperationHistory koh = new KeyOperationHistory()
        //            {
        //                KeyId = key.KeyId,
        //                ProductKey = key.ProductKey,
        //                HardwareHash = dbKey.HardwareHash,
        //                KeyStateFrom = (byte)key.CurrentState,
        //                KeyStateTo = (byte)key.NewState,
        //                Message = message,
        //                Operator = @operator
        //            };
        //            context.KeyOperationHistories.Add(koh);
        //            dupKeyInDb.KeyOperationHistory = koh;
        //            dupKeyInDb.Handled = true;
        //        }
        //        context.SaveChanges();
        //    }
        //}
    }
}
