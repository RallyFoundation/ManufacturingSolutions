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

using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using DIS.Data.DataContract;
using DIS.Services.WebServiceLibrary;

namespace DIS.Services.ProviderWebService
{
    /// <summary>
    /// The service hosted on Internet
    /// </summary>
    [ServiceContract]
    public interface IProviderService
    {
        /// <summary>
        /// Return OK for ULS/ULS
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate = "/Diagnostic/External")]
        void TestExternal();

        /// <summary>
        /// Return keys for DLS
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/Keys/Get")]
        List<KeyInfo> GetKeys();

        /// <summary>
        /// Get sync notification after keys retrieved by DLS
        /// </summary>
        /// <param name="request"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/Sync", Method = "POST")]
        void SyncKeys(List<KeyInfo> request);

        /// <summary>
        /// Get CBR from DLS
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/Report", Method = "POST")]
        List<KeyInfo> ReportKeys(List<KeyInfo> request);

        /// <summary>
        /// Get recall request from DLS
        /// </summary>
        /// <param name="request"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/Recall", Method = "POST")]
        void RecallKeys(List<KeyInfo> request);

        /// <summary>
        /// Get Fulfilled CC request from DLS
        /// </summary>
        /// <param name="request"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/CarbonCopy/Fulfilled", Method = "POST")]
        void CarbonCopyFulfilledKeys(List<KeyInfo> request);

        /// <summary>
        /// Get Reported CC request from DLS
        /// </summary>
        /// <param name="request"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/CarbonCopy/Reported", Method = "POST")]
        void CarbonCopyReportedKeys(List<KeyInfo> request);

        /// <summary>
        /// Get Returned CC request from DLS
        /// </summary>
        /// <param name="request"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/CarbonCopy/Returned", Method = "POST")]
        void CarbonCopyReturnReportedKeys(List<KeyInfo> request);

        /// <summary>
        /// Get Return report CC request from DLS
        /// </summary>
        /// <param name="request"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/CarbonCopy/ReturnReport", Method = "POST")]
        void CarbonCopyReturnReport(ReturnReport request);
        /// <summary>
        /// Get key state sync request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Sync", Method = "POST")]
        long[] UpdateKeyStateAfterRecieveSyncNotification(List<KeySyncNotification> request);
    }
}
