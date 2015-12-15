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
using DIS.Data.DataContract;

namespace DIS.Business.Client
{    
    /// <summary>
    /// Defines behaviors of ServiceClient
    /// </summary>
    public interface IServiceClient
    {
        /// <summary>
        /// Test if internal web service can be connected
        /// </summary>
        void TestInternal();

        /// <summary>
        /// Test if external web service can be connected
        /// </summary>
        void TestExternal();

        /// <summary>
        /// Test if Data Polling Service can be connected
        /// </summary>
        void TestDataPollingService();

        /// <summary>
        /// Test if Key Provider Service can be connected
        /// </summary>
        void TestKeyProviderService();

        /// <summary>
        /// Data Polling Service report self state
        /// </summary>
        void DataPollingServiceReport();

        /// <summary>
        /// Key Provider Service report self state
        /// </summary>
        void KeyProviderServiceReport();

        bool TestDatabaseDiskFull();

        void DatabaseDiskFullReport(bool isFull);

        #region Microsoft services

        /// <summary>
        /// Invoke fulfillments API of Microsoft
        /// </summary>
        /// <returns></returns>
        List<FulfillmentInfo> GetFulfilments();

        /// <summary>
        /// Invoke fulfillments API of Microsoft with specified fulfillment number to get keys
        /// </summary>
        /// <param name="fulfillmentId"></param>
        /// <returns></returns>
        List<KeyInfo> FulfillKeys(string fulfillmentId);

        /// <summary>
        /// Invoke computerbuildreport API of Microsoft with CBR data to report
        /// </summary>
        /// <param name="cbr"></param>
        /// <returns></returns>
        Guid ReportCbr(Cbr cbr);

        Cbr SearchSubmittedCbr(Cbr cbr);

        /// <summary>
        /// Invoke computerbuildreport/acknowledgements API of Microsoft to get a list of available CBR ACKs
        /// </summary>
        /// <returns></returns>
        Guid[] RetrieveCbrAcks();

        /// <summary>
        /// Invoke computerbuildreport/acknowledgements API of Microsoft with specified CBR to retrieve its ACK
        /// </summary>
        /// <returns></returns>
        Cbr RetrieveCbrAck(Cbr cbr);

        /// <summary>
        /// Invoke ReturnReport API of Microsoft with CBR data to report
        /// </summary>
        /// <param name="returnReport"></param>
        /// <returns></returns>
        Guid ReportReturn(ReturnReport returnReport);

        ReturnReport SearchSubmittedReturn(ReturnReport returnReport);

        /// <summary>
        /// Invoke ReturnReport/acknowledgements API of Microsoft to get a list of available ReturnReport ACKs
        /// </summary>
        /// <returns></returns>
        Guid[] RetrieveReturnReportAcks();

        /// <summary>
        /// Invoke ReturnReport/acknowledgements API of Microsoft with specified CBR to retrieve its ACK
        /// </summary>
        /// <returns></returns>
        ReturnReport RetrievReturnReportAck(ReturnReport returnReport);

        Guid ReportOhr(Ohr ohr);

        Guid[] RetrieveOhrAcks();

        Ohr RetrieveOhrAck(Ohr ohr);

        #endregion

        #region Up level system services

        /// <summary>
        /// Get keys from up level system
        /// </summary>
        /// <returns></returns>
        List<KeyInfo> GetKeys();

        /// <summary>
        /// Send sync notification after keys gotten
        /// </summary>
        /// <param name="keys"></param>
        void SyncKeys(List<KeyInfo> keys);

        /// <summary>
        /// Report keys to up level system
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        List<KeyInfo> ReportKeys(List<KeyInfo> keys);

        /// <summary>
        /// Report keys to up level system
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="xsltTransformation"></param>
        /// <returns></returns>
        List<KeyInfo> ReportKeys(List<KeyInfo> keys, string xsltTransformation);

        /// <summary>
        /// Recall keys to up level system
        /// </summary>
        /// <param name="keys"></param>
        void RecallKeys(List<KeyInfo> keys);

        /// <summary>
        /// Copy fulfilled keys to up level system which got from Microsoft
        /// </summary>
        /// <param name="keys"></param>
        void CarbonCopyFulfilledKeys(List<KeyInfo> keys);

        /// <summary>
        /// Copy reported keys to up level system which sent to Microsoft
        /// </summary>
        /// <param name="keys"></param>
        void CarbonCopyReportedKeys(List<KeyInfo> keys);

        /// <summary>
        /// Copy return reported keys to up level system which sent to Microsoft
        /// </summary>
        /// <param name="keys"></param>
        void CarbonCopyReturnReportedKeys(List<KeyInfo> keys);

        /// <summary>
        /// Copy return report to up level system which sent to Microsoft
        /// </summary>
        /// <param name="keys"></param>
        void CarbonCopyReturnReport(ReturnReport request);

        /// <summary>
        /// Sync keys to down level system
        /// </summary>
        /// <param name="syncs"></param>
        long[] SendKeySyncNotifications(List<KeySyncNotification> syncs);

        #endregion

    }
}
