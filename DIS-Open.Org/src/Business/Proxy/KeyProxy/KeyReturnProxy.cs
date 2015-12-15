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
using Microsoft.ServiceModel.Web;
using DIS.Data.DataAccess;

namespace DIS.Business.Proxy
{
    public partial class KeyProxy
    {
        private void SearchSubmittedReturnReport()
        {
            ReturnReport returnReport = GetFirstSentReturnReport();
            if (returnReport != null)
            {
                ReturnReport submittedReturnReport = msClient.SearchSubmittedReturn(returnReport);
                if (submittedReturnReport == null)
                {
                    UpdateReturnReportIfSearchResultEmpty(returnReport);
                }
                else
                {
                    returnReport.ReturnUniqueId = submittedReturnReport.ReturnUniqueId;
                    returnReport.ReturnDateUTC = submittedReturnReport.ReturnDateUTC;
                    UpdateReturnReportAfterReported(returnReport);
                }
            }
        }

        //re-send return failed keys in the background
        private void SendGeneratedReturnReports()
        {
            List<ReturnReport> returnReports = GetReturnReportsNotSent();
            foreach (ReturnReport returnReport in returnReports)
            {
                try
                {
                    returnReport.ReturnUniqueId = msClient.ReportReturn(returnReport);
                    UpdateReturnReportAfterReported(returnReport);
                }
                catch (Exception ex)
                {
                    UpdateReturnReportIfSendingFailed(returnReport);
                    ExceptionHandler.HandleException(ex, this.dbConnectionStr);
                }
            }
        }

        private void UpdateReturnReportAfterReported(ReturnReport returnReport)
        {
            using (KeyStoreContext context = KeyStoreContext.GetContext(this.dbConnectionStr))//using (KeyStoreContext context = KeyStoreContext.GetContext())
            {
                UpdateReturnAfterReported(returnReport, context);
                UpdateKeysAfterReturnReport(returnReport.ReturnReportKeys, context);
                context.SaveChanges();
            }
        }

        //get Return.Ack from Ms 
        private void RetrieveReturnReportAcks()
        {
            List<ReturnReport> returnReports = GetReportedReturnReports()
                .Where(c => c.ReturnUniqueId.HasValue).ToList();
            if (returnReports.Count > 0)
            {
                Guid[] readyReturnReportIds = msClient.RetrieveReturnReportAcks();
                returnReports = returnReports.Where(c => readyReturnReportIds.Contains(c.ReturnUniqueId.Value))
                    .Union(GetFailedReturnReports()).ToList();
                UpdateReturnsAfterAckReady(returnReports);
            }
            returnReports = GetReadyReturnReports();
            foreach (var returnReport in returnReports)
            {
                try
                {
                    ReturnReport returnWithAck = msClient.RetrievReturnReportAck(returnReport);
                    using (KeyStoreContext context = KeyStoreContext.GetContext(this.dbConnectionStr))//using (KeyStoreContext context = KeyStoreContext.GetContext())
                    {
                        ReturnReport dbReturnReport = UpdateReturnAfterAckRetrieved(returnWithAck, context);
                        var result = base.UpdateKeysAfterRetrieveReturnReportAck(dbReturnReport, context);
                        if (GetIsCarbonCopy())
                            base.UpdateKeysToCarbonCopy(result.Where(r => !r.Failed && r.KeyInDb.KeyState == KeyState.Returned).Select(r => r.Key).ToList(), true, context);
                        context.SaveChanges();
                    }
                }
                catch (WebProtocolException)
                {
                    UpdateReturnWhenAckFailed(returnReport);
                }
            }
        }
    }
}
