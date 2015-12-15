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
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfService.Contracts;
using WcfService.Contracts.Fulfillment;
using DomainData = WcfService.Contracts.DomainData;

namespace WcfService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/computerbuildreport/", Method = "POST")]
        ComputerBuildReportResponse ReportBindings(ComputerBuildReportRequest request);

        [OperationContract]
        [WebGet(UriTemplate = "/computerbuildreport/acknowledgements")]
        string[] RetrieveComputerBuildReportAcknowledge();

        [OperationContract]
        [WebGet(UriTemplate = "/computerbuildreport/acknowledgements/{reportUniqueID}")]
        ComputerBuildReportAckResponse RetrieveReportBindings(string reportUniqueId);

        [OperationContract]
        [WebGet(UriTemplate = "/computerbuildreport/royd/v1/SearchSubmitted?CustomerReportUniqueID={customerReportUniqueID}")]
        CbrSearchSubmittedResponse[] SearchSubmittedCbr(string customerReportUniqueID);

        [OperationContract]
        [WebGet(UriTemplate = "/fulfillments/?status=ready")]
        FulfillmentInfoResponse RetrieveFulfillment();

        [OperationContract]
        [WebGet(UriTemplate = "/fulfillments/Order/{orderuniqueid}")]
        FulfillmentInfoResponse RetrieveSpecFulfillment(string orderUniqueId);

        [OperationContract]
        [WebGet(UriTemplate = "/fulfillments/{fulfillmentid}")]
        FulfillmentResponse FulfillOrder(string fulfillmentId);

        [OperationContract]
        [WebGet(UriTemplate = "/Order?soldto={soldTo}&quantity={quantity}")]
        OrderResponse CreateOrder(string soldTo, string quantity);

        [OperationContract]
        [WebGet(UriTemplate = "/fulfillments?fulfillmentid={fulfillmentID}&Status={status}")]
        FulfillmentInfoResponse SetFulfillmentStatus(string fulfillmentId,string status);

        [OperationContract]
        [WebGet(UriTemplate = "/acknowledgements?reportUniqueid={reportUniqueID}&Status={status}")]
        ComputerBuildReportAckResponse SetAcknowledgementStatus(string reportUniqueId, string status);

        [OperationContract]
        [WebInvoke(UriTemplate = "/return/", Method = "POST")]
        ReturnResponse ReportReturn(ReturnRequest request);

        [OperationContract]
        [WebGet(UriTemplate = "/return/acknowledgements")]
        string[] RetrieveReturnAcknowledge();

        [OperationContract]
        [WebGet(UriTemplate = "/return/acknowledgements/{returnUniqueID}")]
        ReturnAck RetrieveReportReturn(string returnUniqueID);

        [OperationContract]
        [WebGet(UriTemplate = "/return/royd/v1/SearchSubmitted?OEMRMANumber={oemRMANumber}&OEMRMADateUTC={oemRMADateUTC}")]
        ReturnSearchSubmittedResponse[] SearchSubmittedReturn(string oemRMANumber, DateTime oemRMADateUTC);

        [OperationContract]
        [WebInvoke(UriTemplate = "/oemhardwarereporting/royd/v1/", Method = "POST")]
        DataUpdateResponse ReportOhr(DataUpdateRequest request);

        [OperationContract]
        [WebGet(UriTemplate = "/oemhardwarereporting/royd/v1/acknowledgements")]
        string[] RetrieveOhrAcks();

        [OperationContract]
        [WebGet(UriTemplate = "/oemhardwarereporting/royd/v1/acknowledgements/{MSUpdateUniqueID}")]
        DataUpdateAck RetrieveOhrAck(string msUpdateUniqueID);
    }
}
