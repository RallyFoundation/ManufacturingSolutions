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
using DIS.Data.DataAccess.Repository;
using DIS.Data.ServiceContract;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    public partial class LocalKeyManager
    {

        #region Return Keys

        public List<KeyInfo> SearchToReturnKeys(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeys(GetToReturnKeysSearchCriteria(searchCriteria));
        }

        public ReturnReport GetFirstSentReturnReport()
        {
            ReturnKeySearchCriteria criteria = new ReturnKeySearchCriteria()
            {
                ReturnReportStatus = ReturnReportStatus.Sent
            };

            return returnKeyRepository.SearchReturnKeys(criteria).FirstOrDefault();
        }

        public List<ReturnReport> GetReportedReturnReports()
        {
            ReturnKeySearchCriteria criteria = new ReturnKeySearchCriteria()
            {
                ReturnReportStatus = ReturnReportStatus.Reported
            };

            return returnKeyRepository.SearchReturnKeys(criteria);
        }

        public List<ReturnReport> GetReturnReportsNotSent()
        {
            ReturnKeySearchCriteria criteria = new ReturnKeySearchCriteria()
            {
                ReturnReportStatus = ReturnReportStatus.Generated
            };

            return returnKeyRepository.SearchReturnKeys(criteria);
        }

        public List<ReturnReport> GetReadyReturnReports()
        {
            ReturnKeySearchCriteria criteria = new ReturnKeySearchCriteria()
            {
                ReturnReportStatus = ReturnReportStatus.Ready
            };

            return returnKeyRepository.SearchReturnKeys(criteria);
        }

        public List<ReturnReport> GetFailedReturnReports()
        {
            ReturnKeySearchCriteria criteria = new ReturnKeySearchCriteria()
            {
                ReturnReportStatus = ReturnReportStatus.Failed
            };

            return returnKeyRepository.SearchReturnKeys(criteria);
        }

        public List<ReturnReport> GetCompletedReturnReports()
        {
            ReturnKeySearchCriteria criteria = new ReturnKeySearchCriteria()
            {
                ReturnReportStatus = ReturnReportStatus.Completed
            };

            return returnKeyRepository.SearchReturnKeys(criteria);
        }

        public void UpdateReturnReportIfSendingFailed(ReturnReport returnReport)
        {
            returnReport.ReturnReportStatus = ReturnReportStatus.Sent;
            returnKeyRepository.UpdateReturnReport(returnReport);
        }

        public void UpdateReturnReportIfSearchResultEmpty(ReturnReport returnReport)
        {
            returnReport.ReturnReportStatus = ReturnReportStatus.Generated;
            returnKeyRepository.UpdateReturnReport(returnReport);
        }

        public void UpdateReturnsAfterAckReady(List<ReturnReport> returnReports)
        {
            if (returnReports.Any(r => r.ReturnReportStatus != ReturnReportStatus.Reported && r.ReturnReportStatus != ReturnReportStatus.Failed))
                throw new ArgumentException("Returns are invalid.");

            foreach (var returnReport in returnReports)
            {
                returnReport.ReturnReportStatus = ReturnReportStatus.Ready;
                returnKeyRepository.UpdateReturnReport(returnReport);
            }
        }

        public ReturnReport UpdateReturnAfterAckRetrieved(ReturnReport returnReport, KeyStoreContext context)
        {
            ReturnReport dbReturnReport = returnKeyRepository.GetReturnKey(returnReport.ReturnUniqueId.Value, context);
            UpdateReturnByAck(returnReport, dbReturnReport, context);
            return dbReturnReport;
        }

        public void UpdateReturnWhenAckFailed(ReturnReport returnReport)
        {
            returnReport.ReturnReportStatus = ReturnReportStatus.Failed;
            returnKeyRepository.UpdateReturnReport(returnReport);
        }

        protected void UpdateReturnAfterReported(ReturnReport returnReport, KeyStoreContext context)
        {
            if (returnReport.CustomerReturnUniqueId == null)
                throw new ArgumentException("Return is invalid.");

            returnReport.ReturnReportStatus = ReturnReportStatus.Reported;
            returnKeyRepository.UpdateReturnReport(returnReport, context);
        }

        public List<KeyOperationResult> SaveGeneratedReturnReport(ReturnReport returnReport)
        {
            List<KeyOperationResult> results = VerificationReturnReport(returnReport);
            keyRepository.UpdateKeys(results.Where(k => !k.Failed).Select(k => k.Key).ToList(), true, null);
            SaveReturnReport(returnReport);
            return results;
        }

        private void UpdateReturnByAck(ReturnReport returnReport, ReturnReport dbReturnReport, KeyStoreContext context = null)
        {
            if (dbReturnReport == null)
                throw new DisException("Failed to get data from database to match the contents of this file!");
            if (dbReturnReport.ReturnReportStatus == ReturnReportStatus.Completed)
                throw new DisException("Return.ack has got and completed!");

            // validate the count of return ack keys equals the db keys - Merged from v1.9 - Rally Spet. 26, 2014
            var validateReturnAck = (from db in dbReturnReport.ReturnReportKeys
                                     join ack in returnReport.ReturnReportKeys on db.KeyId equals ack.KeyId
                                     select ack).ToList();
            if (dbReturnReport.ReturnReportKeys.Count != validateReturnAck.Count)
                throw new DisException("Return.ack keys not equal the db!");

            dbReturnReport.ReturnUniqueId =(returnReport.ReturnUniqueId==System.Guid.Empty?null: returnReport.ReturnUniqueId);
            dbReturnReport.MsReturnNumber = returnReport.MsReturnNumber;
            dbReturnReport.ReturnDateUTC = returnReport.ReturnDateUTC;
            dbReturnReport.OemRmaNumber = returnReport.OemRmaNumber;       
            dbReturnReport.OemRmaDateUTC = returnReport.OemRmaDateUTC;
            dbReturnReport.SoldToCustomerName = returnReport.SoldToCustomerName;
            dbReturnReport.ReturnReportStatus = ReturnReportStatus.Completed;
            if (dbReturnReport.ReturnReportKeys != null && dbReturnReport.ReturnReportKeys.Count > 0)
            {
                Func<ReturnReportKey, ReturnReportKey, ReturnReportKey> updateReturnReportAck =
                (k1, k2) =>
                {
                    k1.MsReturnLineNumber = k2.MsReturnLineNumber;
                    k1.OemRmaLineNumber = k2.OemRmaLineNumber;
                    k1.ReturnTypeId = k2.ReturnTypeId;
                    k1.LicensablePartNumber = k2.LicensablePartNumber;
                    k1.ReturnReasonCode = k2.ReturnReasonCode;
                    k1.ReturnReasonCodeDescription = k2.ReturnReasonCodeDescription;
                    return k1;
                };
                var update = (from db in dbReturnReport.ReturnReportKeys
                              join ack in returnReport.ReturnReportKeys on db.KeyId equals ack.KeyId
                              select updateReturnReportAck(db, ack)).ToList();
                if (dbReturnReport.ReturnReportKeys.All(k => k.ReturnReasonCode.StartsWith("Q")))
                    dbReturnReport.ReturnNoCredit = true;
                else if (dbReturnReport.ReturnReportKeys.All(k => k.ReturnReasonCode.StartsWith("O")))
                    dbReturnReport.ReturnNoCredit = false;
            }
            returnKeyRepository.UpdateReturnKeyAck(dbReturnReport, context);
        }

        private List<KeyOperationResult> ImportReturnAck(ReturnAck returnAck, KeyStoreContext context)
        {
            if (returnAck == null || returnAck.ReturnAckLineItems.Count() <= 0)
                throw new ApplicationException("Passed in keys list is empty.");
            ReturnReport returnReport = returnAck.FromServiceContract();
            ReturnReport ReturnInDB = null;
            if (returnAck.ReturnUniqueID == System.Guid.Empty)
                ReturnInDB = returnKeyRepository.GetReturnKeyByOneKeyID(returnReport.ReturnReportKeys.Select(k => k.KeyId).FirstOrDefault(), context);
            else
                ReturnInDB = returnKeyRepository.GetReturnKey(returnAck.ReturnUniqueID, context);
            UpdateReturnByAck(returnReport, ReturnInDB, context);
            List<KeyOperationResult> result = UpdateKeysAfterRetrieveReturnReportAck(ReturnInDB, context);
            return result;
        }

        public void UpdateReturnReportToCarbonCopy(List<ReturnReport> returnReports)
        {
            foreach (var returnReport in returnReports)
            {
                returnReport.ReturnReportStatus = ReturnReportStatus.ShouldCarbonCopy;
                returnKeyRepository.UpdateReturnReport(returnReport);
            }
        }

        public void UpdateReturnReportToCarbonCopyCompleted(ReturnReport returnReport)
        {
            returnReport.ReturnReportStatus = ReturnReportStatus.CarbonCopyCompleted;
            returnKeyRepository.UpdateReturnReport(returnReport);
        }

        public List<ReturnReport> SearchCarbonCopyReturnReport()
        {
            ReturnKeySearchCriteria criteria = new ReturnKeySearchCriteria()
            {
                ReturnReportStatus = ReturnReportStatus.ShouldCarbonCopy
            };

            return returnKeyRepository.SearchReturnKeys(criteria);
        }

        private string GenerateReturnKeysToFile(ReturnReport report, ExportParameters exportParameters)
        {
            ReturnRequest request = new ReturnRequest()
            {
                OEMRMANumber = report.OemRmaNumber,
                OEMRMADate = report.OemRmaDate,
                SoldToCustomerID = report.SoldToCustomerId,
                ReturnNoCredit = report.ReturnNoCredit,
                ReturnLineItems = report.ReturnReportKeys.Select(k => new ReturnLineItem()
                {
                    OEMRMALineNumber = k.OemRmaLineNumber,
                    ProductKeyID = k.KeyId,
                    ReturnTypeID = k.ReturnTypeId
                }).ToArray()
            };
            KeyManagerHelper.CreateFilePath(exportParameters.OutputPath, this.keyRepository.GetDBConnectionString());
            Serializer.WriteToXml(request, exportParameters.OutputPath);
            return KeyManagerHelper.GetXmlResult(request);
        }

        #endregion

        #region Private Methods

        private void SaveReturnReport(ReturnReport returnReport)
        {
            returnReport.OemRmaDate = DateTime.Now;
            returnReport.ReturnReportStatus = ReturnReportStatus.Generated;
            returnReport.CustomerReturnUniqueId = Guid.NewGuid();
            returnKeyRepository.InsertReturnReportAndKeys(returnReport);
        }

        private List<KeyOperationResult> VerificationReturnReport(ReturnReport returnReport)
        {
            List<KeyOperationResult> results = new List<KeyOperationResult>();
            List<KeyInfo> keysInDb = GetKeysInDb(returnReport.ReturnReportKeys.Select(k => k.KeyId));
            foreach (var reportKey in returnReport.ReturnReportKeys)
            {
                KeyInfo key = GetKey(reportKey.KeyId, keysInDb);
                if (!returnReport.ReturnNoCredit &&
                    (reportKey.ReturnTypeId == ReturnRequestType.ZOE.ToString() || reportKey.ReturnTypeId == ReturnRequestType.ZOF.ToString()))
                {
                    results.Add(new KeyOperationResult() { Failed = true, FailedType = KeyErrorType.Invalid, Key = key });
                    returnReport.ReturnReportKeys.Remove(reportKey);
                }

                if (key.KeyState == KeyState.Fulfilled
                    && key.KeyState == KeyState.Bound && reportKey.ReturnTypeId == ReturnRequestType.ZOA.ToString())
                {
                    results.Add(new KeyOperationResult() { Failed = true, FailedType = KeyErrorType.Invalid, Key = key });
                    returnReport.ReturnReportKeys.Remove(reportKey);
                }

                if (key.KeyState == KeyState.ActivationDenied
                    && key.KeyState == KeyState.ActivationEnabled && reportKey.ReturnTypeId != ReturnRequestType.ZOA.ToString())
                {
                    results.Add(new KeyOperationResult() { Failed = true, FailedType = KeyErrorType.Invalid, Key = key });
                    returnReport.ReturnReportKeys.Remove(reportKey);
                }

                results.Add(new KeyOperationResult() { Failed = false, Key = key });
            }
            return results;
        }

        /// <summary>
        /// get keys search criteria to return
        /// </summary>
        /// <param name="searchCriteria">KeySearchCriteria</param>
        /// <returns></returns>
        private KeySearchCriteria[] GetToReturnKeysSearchCriteria(KeySearchCriteria searchCriteria)
        {
            if (searchCriteria.KeyType == null)
                throw new NotSupportedException();

            searchCriteria.HqId = CurrentHeadQuarterId;
            searchCriteria.IsInProgress = false;
            
            //Merged from v1.9 - Rally Sept. 26, 2014
            //if (searchCriteria.KeyType == KeyType.All)
            //{
            //    if (searchCriteria.KeyStateIds != null)
            //        throw new DisException("SearchKey_InvalidKeyType");
            //    else
            //    {
            //        var searchCriteriaStandard = ConvertSearchCriteria(searchCriteria);
            //        var searchCriteriaMBR = ConvertSearchCriteria(searchCriteria);
            //        var searchCriteriaMAT = ConvertSearchCriteria(searchCriteria);

            //        searchCriteriaStandard.KeyType = KeyType.Standard;
            //        searchCriteriaStandard.KeyStates = new List<KeyState> { KeyState.Fulfilled, KeyState.Bound, KeyState.ActivationEnabled, KeyState.ActivationDenied };
            //        searchCriteriaMBR.KeyType = KeyType.MBR;
            //        searchCriteriaMBR.KeyStates = new List<KeyState> { KeyState.Fulfilled, KeyState.ActivationEnabled };
            //        searchCriteriaMAT.KeyType = KeyType.MAT;
            //        searchCriteriaMAT.KeyStates = new List<KeyState> { KeyState.Fulfilled };
            //        return new KeySearchCriteria[] { searchCriteriaStandard, searchCriteriaMBR, searchCriteriaMAT };
            //    }
            //}
            //else
            //{
                switch (searchCriteria.KeyType)
                {
                    case KeyType.Standard:
                        if (searchCriteria.KeyStateIds == null || searchCriteria.KeyStateIds.Count <= 0)
                            searchCriteria.KeyStates = new List<KeyState> { KeyState.Fulfilled, KeyState.Bound, KeyState.ActivationEnabled, KeyState.ActivationDenied };
                        break;
                    case KeyType.MBR:
                        if (searchCriteria.KeyStateIds == null || searchCriteria.KeyStateIds.Count <= 0)
                            searchCriteria.KeyStates = new List<KeyState> { KeyState.Fulfilled, KeyState.ActivationEnabled };
                        break;
                    case KeyType.MAT:
                        if (searchCriteria.KeyStateIds == null || searchCriteria.KeyStateIds.Count <= 0)
                            searchCriteria.KeyStates = new List<KeyState> { KeyState.Fulfilled };
                        break;
                    default:
                        break;
                }
                return new KeySearchCriteria[] { searchCriteria };
            //}
        }

        private KeySearchCriteria[] GetNotBeenSendReturnKeysSearchCriteria()
        {
            KeySearchCriteria searchCriteria = new KeySearchCriteria()
            {
                HqId = CurrentHeadQuarterId,
                ShouldIncludeReturnReport = true,
                IsInProgress = true,
                ReturnReportStatus = ReturnReportStatus.Generated
            };
            var searchCriteriaStandard = ConvertSearchCriteria(searchCriteria);
            var searchCriteriaMBR = ConvertSearchCriteria(searchCriteria);
            var searchCriteriaMAT = ConvertSearchCriteria(searchCriteria);

            searchCriteriaStandard.KeyType = KeyType.Standard;
            searchCriteriaStandard.KeyStates = new List<KeyState> { KeyState.Fulfilled, KeyState.Bound, KeyState.ActivationEnabled, KeyState.ActivationDenied };
            searchCriteriaMBR.KeyType = KeyType.MBR;
            searchCriteriaMBR.KeyStates = new List<KeyState> { KeyState.Fulfilled, KeyState.ActivationEnabled };
            searchCriteriaMAT.KeyType = KeyType.MAT;
            searchCriteriaMAT.KeyStates = new List<KeyState> { KeyState.Fulfilled };
            return new KeySearchCriteria[] { searchCriteriaStandard, searchCriteriaMBR, searchCriteriaMAT };
        }

        #endregion

    }
}
