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
using System.Net;
using Microsoft.ServiceModel.Web;
using WcfService.Contracts;
using WcfService.Contracts.Fulfillment;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel;
using System.Security.Principal;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace WcfService
{
    [ServiceBehavior(MaxItemsInObjectGraph = int.MaxValue)]
    public class Service : IService
    {
        #region Private members and contructor
        private DataRepository db;
        private string customerNumber;
        private const string issuerName = @"CN=";

        public Service()
        {
            db = new DataRepository();
            customerNumber = GetCertificateName();
        }

        #endregion

        #region Fulfillment
        public FulfillmentResponse FulfillOrder(string fulfillmentId)
        {
            try
            {
                var fulfillments = db.GetKeyFulfillment(fulfillmentId);
                if (fulfillments == null)
                    throw new WebProtocolException(HttpStatusCode.BadRequest);

                return new FulfillmentResponse
                {
                    Fulfillments = fulfillments,
                };
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("fulfillments/{0}", fulfillmentId), ex);
            }
        }

        public FulfillmentInfoResponse RetrieveFulfillment()
        {
            try
            {
                return new FulfillmentInfoResponse
                {
                    FulfillmentInfos = db.GetFulfillmentInfo(customerNumber)
                };
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("fulfillments?status=ready"), ex);
            }
        }

        public FulfillmentInfoResponse RetrieveSpecFulfillment(string orderUniqueId)
        {
            try
            {
                var order = new Guid(orderUniqueId);
                return new FulfillmentInfoResponse
                {
                    FulfillmentInfos = db.GetSpecFulfillmentInfo(order)
                };
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("fulfillments?orderuniqueid={0}", orderUniqueId), ex);
            }
        }

        public OrderResponse CreateOrder(string soldTo, string quantity)
        {
            try
            {
                int quant = int.Parse(quantity);
                return db.CreateOrder(soldTo, customerNumber, quant);
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("Order/{0}", quantity), ex);
            }
        }

        public FulfillmentInfoResponse SetFulfillmentStatus(string fulfillmentId, string status)
        {
            try
            {
                if (
                    !status.Equals("Sent", StringComparison.CurrentCultureIgnoreCase) &&
                    !status.Equals("Resend", StringComparison.CurrentCultureIgnoreCase))
                    throw new ArgumentException("The status must be Sent or Resend");

                bool isBeenSent = status.Equals("Resend", StringComparison.CurrentCultureIgnoreCase) ? true : false;
                return new FulfillmentInfoResponse
                {
                    FulfillmentInfos = new FulfillmentInfo[] { db.SetFulfillmentStatus(fulfillmentId, isBeenSent) },
                };
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("fulfillments?fulfillmentid={fulfillmentID}&Status={status}"), ex);
            }
        }
        #endregion

        #region Computer build report

        public ComputerBuildReportResponse ReportBindings(ComputerBuildReportRequest request)
        {
            try
            {
                return new ComputerBuildReportResponse { ReportUniqueID = db.ReportBindings(request.GetDomainData()) };
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("BindingReport"), ex);
            }
        }

        public string[] RetrieveComputerBuildReportAcknowledge()
        {
            try
            {
                return db.RetrieveComputerBuildReportAcknowledge().Select(k => k.ToString()).ToArray();
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("BindingReport/acknowledgements"), ex);
            }
        }

        public ComputerBuildReportAckResponse RetrieveReportBindings(string reportUniqueId)
        {
            try
            {
                Guid guidReportUniqueId = Guid.Parse(reportUniqueId);
                var ack = db.RetrieveReportBindings(guidReportUniqueId);
                if (ack == null)
                    throw new WebProtocolException(HttpStatusCode.BadRequest);
                return ack;
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("BindingReport/acknowledgements"), ex);
            }
        }

        public ComputerBuildReportAckResponse SetAcknowledgementStatus(string reportUniqueId, string status)
        {
            try
            {
                if (
                    !status.Equals("Sent", StringComparison.CurrentCultureIgnoreCase) &&
                    !status.Equals("Resend", StringComparison.CurrentCultureIgnoreCase))
                    throw new ArgumentException("The status must be Sent or Resend");

                bool isBeenSent = status.Equals("Resend", StringComparison.CurrentCultureIgnoreCase) ? true : false;
                return db.SetComputerBuildReportAckStatus(reportUniqueId, isBeenSent);
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("acknowledgements?reportUniqueid={reportUniqueID}&Status={status}"), ex);
            }
        }

        public CbrSearchSubmittedResponse[] SearchSubmittedCbr(string customerReportUniqueID)
        {
            try
            {
                Guid guid = Guid.Parse(customerReportUniqueID);
                var result = db.SearchSubmittedCbr(guid);

                if (result == null)
                {
                    var err = new ClientError();
                    err.ErrorCode = null;
                    err.ErrorMessage = "No result with given input search parameter(s)";
                    throw new WebProtocolException(HttpStatusCode.BadRequest, "", err, null);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("acknowledgements?reportUniqueid={reportUniqueID}&Status={status}"), ex);
            }
        }

        #endregion

        #region Return report

        public ReturnResponse ReportReturn(ReturnRequest request)
        {
            try
            {
                return new ReturnResponse { ReturnUniqueID = db.ReportReturn(request.GetDomainData()) };
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("ReportReturn"), ex);
            }
        }

        public string[] RetrieveReturnAcknowledge()
        {
            try
            {
                return db.RetrieveReturnAcknowledge().Select(k => k.ToString()).ToArray();
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("RetrieveReturnAcknowledge"), ex);
            }
        }

        public ReturnAck RetrieveReportReturn(string returnUniqueID)
        {
            try
            {
                Guid returnReportUniqueID = Guid.Parse(returnUniqueID);
                ReturnAck ack = db.RetrieveReportReturn(returnReportUniqueID);
                if (ack == null)
                    throw new WebProtocolException(HttpStatusCode.BadRequest);
                return ack;
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("RetrieveReportReturn"), ex);
            }
        }

        public ReturnSearchSubmittedResponse[] SearchSubmittedReturn(string oemRMANumber, DateTime oemRMADateUTC)
        {
            try
            {
                var ack = db.SearchSubmittedReturn(oemRMANumber, oemRMADateUTC);
                if (ack == null)
                    throw new WebProtocolException(HttpStatusCode.BadRequest, 
                        "", 
                        new ClientError { ErrorCode = null, ErrorMessage = "No result with given input search parameter(s)" }, 
                        null);
                return ack;
            }
            catch (Exception ex)
            {
                if (ex is WebProtocolException)
                    throw;
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, string.Format("BindingReport/acknowledgements"), ex);
            }
        }

        #endregion

        #region OHR Data Update
        public DataUpdateResponse ReportOhr(DataUpdateRequest request)
        {
            Guid msId = Guid.NewGuid();
            DataUpdateAck ack = new DataUpdateAck()
            {
                MSUpdateUniqueID = msId,
                CustomerUpdateUniqueID = request.CustomerUpdateUniqueID,
                MSReceivedDateUTC = DateTime.UtcNow,
                SoldToCustomerID = request.SoldToCustomerID,
                ReceivedFromCustomerID = request.ReceivedFromCustomerID,
                TotalLineItems = 0,
                DataUpdateResults = null
            };
            SaveOHR(ack);
            return new DataUpdateResponse() { MSUpdateUniqueID = msId };
        }

        public string[] RetrieveOhrAcks()
        {
            string dir = Helper.GetLocalDirectory(ohrFolder);
            string file = Path.Combine(dir, "OhrList");
            file += ".txt";
            string[] result = File.ReadAllLines(file);
            return result;
        }

        public DataUpdateAck RetrieveOhrAck(string msUpdateUniqueID)
        {
            return GetOHR(msUpdateUniqueID);
        }

        #endregion

        private string GetCertificateName()
        {
            ServiceSecurityContext ssContext = ServiceSecurityContext.Current;
            if (ssContext != null)
            {
                string name = ssContext.PrimaryIdentity.Name;
                if (name.Length > issuerName.Length)
                    return name.Substring(issuerName.Length);
                else
                    return name;
            }
            else
                return string.Empty;
        }

        private string ohrFolder = "Ohr";
        private void SaveOHR(DataUpdateAck ack)
        {
            string dir = Helper.GetLocalDirectory(ohrFolder);
            string file = Path.Combine(dir, ack.MSUpdateUniqueID.ToString());
            file += ".xml";
            Helper.SaveServiceContractToFile(ack, file);
        }

        private DataUpdateAck GetOHR(string msUpdateUniqueID)
        {
            string dir = Helper.GetLocalDirectory(ohrFolder);
            string file = Path.Combine(dir, msUpdateUniqueID);
            file += ".xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string xmlContent = doc.DocumentElement.OuterXml;
            //File.Delete(file);
            return xmlContent.FromDataContract<DataUpdateAck>();
        }
       
       
    }
}