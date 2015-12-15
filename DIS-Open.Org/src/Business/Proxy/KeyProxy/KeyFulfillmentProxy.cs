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
using DIS.Common.Utility;
using DIS.Data.DataAccess;

namespace DIS.Business.Proxy
{
    public partial class KeyProxy
    {
        #region Fulfillment

        /// <summary>
        /// CKI/FKI downloads keys from Microsoft.
        /// Steps:
        /// 1. Download fulfillment list and save to database.
        /// 2. Download keys for each fulfillment number in the list and save to database.        
        /// 3. Update keys sync state to 'false', means these kind of keys from Micorosft are no need to sync.
        /// 4. If current installation instance is FKI with 'Carbon Copy' feature enabled, these keys need to send to CKI.
        /// </summary>
        /// <returns></returns>
        public int FulfillOrder()
        {
            if (!configManager.GetIsMsServiceEnabled())
                return -1;

            try
            {
                fulfillManager.RetrieveFulfilment(msClient.GetFulfilments(),
                    f => msClient.FulfillKeys(f.FulfillmentNumber));
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.GetType() == typeof(System.Data.SqlClient.SqlException))
                {
                    System.Data.SqlClient.SqlException sqlException = (System.Data.SqlClient.SqlException)ex.InnerException.InnerException;
                    if (sqlException.Number == 1105) //Could not allocate space for object
                    {
                        msClient.DatabaseDiskFullReport(true);
                        fulfillManager.UpdateFulfillmentFailedWhenDiskIsFull();
                        return -1;
                    }
                }
            }

            bool? shouldBeCarbonCopy = GetIsCarbonCopy() ? (bool?)true : null;

            int total = 0;
            while (true)
            {
                FulfillmentInfo info = fulfillManager.GetFirstFulfilledFulfillment();
                if (info == null)
                    break;

                total += info.Keys.Count;
                try
                {
                    using (KeyStoreContext context = KeyStoreContext.GetContext(this.dbConnectionStr))//using (KeyStoreContext context = KeyStoreContext.GetContext())
                    {
                        if (info.Keys.Count > 0)
                            base.SaveKeysAfterGetting(info.Keys, false, shouldBeCarbonCopy, context);

                        fulfillManager.UpdateFulfillmentToCompleted(info, context);

                        context.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return total;
        }

        #endregion

        #region Get and sync keys

        public int GetKeysFromUls()
        {
            if (!GetIsCentralizeMode())
                return -1;

            int count = base.GetAndSaveKeys(ulsClient.GetKeys, keysToSync => ulsClient.SyncKeys(keysToSync));
            return count;
        }

        public new void ReceiveSyncNotification(List<KeyInfo> keys)
        {
            var result = base.ReceiveSyncNotification(keys);
            if (GetIsCarbonCopy())
                base.UpdateKeysToCarbonCopy(
                    result.Where(r => !r.Failed && r.Key.KeyInfoEx.KeyType == KeyType.MBR).Select(r => r.Key).ToList(), true);
        }

        public void AutomaticGetKeys()
        {
            if (!configManager.GetCanAutoFulfill())
                return;

            if (Constants.InstallType == InstallType.Oem
                || (Constants.InstallType == InstallType.Tpi && !GetIsCentralizeMode()))
                FulfillOrder();

            if (Constants.InstallType == InstallType.FactoryFloor
                || (Constants.InstallType == InstallType.Tpi && GetIsCentralizeMode()))
                GetKeysFromUls();
        }

        public void SendCarbonCopyFulfilledKeys()
        {
            var keys = base.SearchCarbonCopyFulfilledKeys();
            if (keys.Count > 0)
            {
                ulsClient.CarbonCopyFulfilledKeys(keys);
                List<KeyInfo> keysToUpdate = keys.Where(k => (
                    k.KeyState != KeyState.ActivationEnabled &&
                    k.KeyState != KeyState.ActivationDenied &&
                    k.KeyState != KeyState.Returned)).ToList();
                if (keysToUpdate.Count > 0)
                    base.UpdateKeysToCarbonCopy(keysToUpdate, false);
            }
        }

        public void SendCarbonCopyReportedKeys()
        {
            var keys = base.SearchCarbonCopyReportedKeys();
            if (keys.Count > 0)
            {
                ulsClient.CarbonCopyReportedKeys(keys.Select(key =>
                    new KeyInfo(key.KeyState)
                    {
                        KeyId = key.KeyId,
                        OemOptionalInfo = key.OemOptionalInfo,
                        TrackingInfo = key.TrackingInfo,
                        HardwareHash = key.HardwareHash,
                        Tags = "Carbon Copy",
                    }).ToList());
                base.UpdateKeysToCarbonCopy(keys, false);
            }
        }

        public void SendCarbonCopyReturnReportedKeys()
        {
            var keys = base.SearchCarbonCopyReturnedKeys();
            if (keys.Count > 0)
            {
                ulsClient.CarbonCopyReturnReportedKeys(keys.Select(key =>
                    new KeyInfo(key.KeyState)
                    {
                        KeyId = key.KeyId,
                    }).ToList());
                base.UpdateKeysToCarbonCopy(keys, false);
            }
        }

        public void SendCarbonCopyReturnReport()
        {
            PreProcessCarbonCopyReturnReport();
            var returnReports = base.SearchCarbonCopyReturnReport();
            foreach (var returnReport in returnReports)
            {
                ulsClient.CarbonCopyReturnReport(returnReport);
                base.UpdateReturnReportToCarbonCopyCompleted(returnReport);
            }
        }

        private void PreProcessCarbonCopyReturnReport()
        {
            List<ReturnReport> toUpdateCarbonCopyReturnReport = new List<ReturnReport>();
            List<ReturnReport> completedReturnReport = base.GetCompletedReturnReports();

            foreach (var crReport in completedReturnReport)
            {
                List<KeyInfo> keysInDb = GetKeysInDb(crReport.ReturnReportKeys.Select(k => k.KeyId));
                if (keysInDb.All(k => k.KeyInfoEx.ShouldCarbonCopy != null &&
                    !k.KeyInfoEx.ShouldCarbonCopy.Value))
                {
                    toUpdateCarbonCopyReturnReport.Add(crReport);
                }

            }
            if (toUpdateCarbonCopyReturnReport.Any())
                base.UpdateReturnReportToCarbonCopy(toUpdateCarbonCopyReturnReport);
        }

        #endregion
    }
}
