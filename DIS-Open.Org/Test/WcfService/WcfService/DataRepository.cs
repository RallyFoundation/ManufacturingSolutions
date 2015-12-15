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
using System.Linq;
using WcfService.Contracts;
using WcfService.Contracts.DomainData;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Specialized;
using System.Configuration;

namespace WcfService
{
    public class DataRepository
    {
        #region Private members
        private static readonly Random r = new Random();
        private static NameValueCollection section = ConfigurationManager.GetSection("ExtendedProperty") as NameValueCollection;
        private const string parameterSchema = "OEMOptionalInfo.xsd";
        private const int keyLimit = 5000;
        #endregion

        #region Fulfillment
        public KeyFulfillment[] GetKeyFulfillment(string fulfillmentId)
        {
            using (var db = GetContext())
            {
                var query = (from p in db.ProductKeyInfoes
                             where p.FulfillmentNumber == fulfillmentId && p.Status == true
                             select p).ToList();
                if (query.Count > 0)
                {
                    query.ForEach(p => { p.FulfilledDateUTC = DateTime.UtcNow; p.Status = false; p.ProductKeyStateID = 1; });
                    db.SaveChanges();
                    return query.GetDomainData(fulfillmentId);
                }
                else
                    return null;
            }
        }

        public OrderResponse CreateOrder(string soldTo, string shipTo, int totalQuantity)
        {
            OrderResponse response = new OrderResponse();
            Guid orderUniqueID = Guid.NewGuid();
            response.OrderUniqueID = orderUniqueID;
            using (var db = GetContext())
            {
                var quantity = totalQuantity > keyLimit ? keyLimit : totalQuantity;
                var maxFulfillmentNumber = GetMaxFulfillmentNumber(db);
                while (quantity > 0)
                {
                    var query = (from p in db.ProductKeyInfoes
                                 where p.Status == null
                                 select p).Take(quantity).ToList();
                    foreach (var p in query)
                    {
                        p.FulfillmentCreateDateUTC = DateTime.UtcNow;
                        p.FulfillmentNumber = maxFulfillmentNumber.ToString();
                        p.OrderUniqueID = orderUniqueID;
                        p.Quantity = quantity;
                        p.SoldToCustomerID = soldTo;
                        p.ShipToCustomerID = shipTo;
                        p.Status = true;
                    }
                    db.SaveChanges();
                    maxFulfillmentNumber++;
                    totalQuantity -= keyLimit;
                    quantity = totalQuantity > keyLimit ? keyLimit : totalQuantity;
                }
            }
            return response;
        }

        public Contracts.Fulfillment.FulfillmentInfo[] GetFulfillmentInfo(string customerNumber)
        {
            using (var db = GetContext())
            {
                var group = (from p in db.ProductKeyInfoes
                             where p.Status == true && p.ShipToCustomerID == customerNumber
                             group p by new { p.FulfillmentNumber, p.OrderUniqueID, p.SoldToCustomerID } into g
                             select new Contracts.Fulfillment.FulfillmentInfo
                             {
                                 FulfillmentNumber = g.Key.FulfillmentNumber,
                                 OrderUniqueID = g.Key.OrderUniqueID.Value,
                                 SoldToCustomerID = g.Key.SoldToCustomerID,
                             }).Take(10).ToArray();

                return group;
            }
        }

        public Contracts.Fulfillment.FulfillmentInfo SetFulfillmentStatus(string fulfillmentId, bool isBeenSent)
        {
            using (var db = GetContext())
            {
                var query = (from p in db.ProductKeyInfoes
                             where p.FulfillmentNumber == fulfillmentId
                             select p).ToList();


                query.ForEach(q => q.Status = isBeenSent);
                db.SaveChanges();
                return new Contracts.Fulfillment.FulfillmentInfo
                             {
                                 FulfillmentNumber = query.ElementAt(0).FulfillmentNumber,
                                 OrderUniqueID = query.ElementAt(0).OrderUniqueID.Value,
                                 SoldToCustomerID = query.ElementAt(0).SoldToCustomerID,
                             };
            }
        }

        public Contracts.Fulfillment.FulfillmentInfo[] GetSpecFulfillmentInfo(Guid orderUniqueId)
        {
            using (var db = GetContext())
            {
                var group = (from p in db.ProductKeyInfoes
                             where p.OrderUniqueID == orderUniqueId && p.Status == true
                             group p by new { p.FulfillmentNumber, p.OrderUniqueID, p.SoldToCustomerID } into g
                             select new Contracts.Fulfillment.FulfillmentInfo
                             {
                                 FulfillmentNumber = g.Key.FulfillmentNumber,
                                 OrderUniqueID = g.Key.OrderUniqueID.Value,
                                 SoldToCustomerID = g.Key.SoldToCustomerID,
                             }).ToArray();

                var update = (from p in db.ProductKeyInfoes
                              where p.OrderUniqueID == orderUniqueId && p.Status == true
                              select p).ToList();

                update.ForEach(p => p.FulfillmentResendIndicator = false);
                db.SaveChanges();
                return group;
            }
        }
        #endregion

        #region Computer build report
        public Guid ReportBindings(ComputerBuildReport cbr)
        {
            var msReportUniqueId = Guid.NewGuid();
            var cbrKeys = cbr.HardwareBindingReports.ToList();
            using (var db = GetContext())
            {
                var dbKeys = GetProductKeyInfoes(db, cbrKeys.Select(k => k.ProductKeyID).ToList());
                cbrKeys.ForEach(cbrKey =>
                {
                    var reasonCode = Validate(dbKeys, cbrKey);
                    cbrKey.CustomerReportUniqueID = cbr.CustomerReportUniqueID;
                    cbrKey.ReasonCode = reasonCode;
                    cbrKey.ReasonCodeDescription = GetReasonCodeDescription(reasonCode);

                    var productKeyInfo = dbKeys.Single(k => k.ProductKeyID == cbrKey.ProductKeyID);
                    productKeyInfo.ProductKeyStateID = 3;
                    productKeyInfo.HardwareID = cbrKey.HardwareHash;
                    productKeyInfo.OEMAdditionalInfo = cbrKey.OEMOptionalInfo;
                });
                cbr.MSReportUniqueID = msReportUniqueId;
                cbr.MSReceivedDateUTC = DateTime.UtcNow;
                cbr.Status = true;
                db.ComputerBuildReports.AddObject(cbr);
                db.SaveChanges();
                return msReportUniqueId;
            }
        }

        public List<Guid> RetrieveComputerBuildReportAcknowledge()
        {
            using (var db = GetContext())
            {
                var result= db.ComputerBuildReports.Where(c => c.Status == true).ToList();
                foreach (var cbr in result)
                {
                    cbr.Status = false;
                }
                db.SaveChanges();
                return result.Select(r => r.MSReportUniqueID.Value).ToList();
            }
        }

        public ComputerBuildReportAckResponse RetrieveReportBindings(Guid reportUniqueId)
        {
            using (var db = GetContext())
            {
                var computerBuildReportAck = db.ComputerBuildReports.Include("HardwareBindingReports").Where(c => c.MSReportUniqueID == reportUniqueId).SingleOrDefault();
                if (computerBuildReportAck != null)
                {
                    return computerBuildReportAck.GetDomainData();
                }
                else
                    return null;
            }
        }

        public CbrSearchSubmittedResponse[] SearchSubmittedCbr(Guid customerReportUniqueID)
        {
            using (var db = GetContext())
            {
                var ack = db.ComputerBuildReports.Where(c => c.CustomerReportUniqueID == customerReportUniqueID).SingleOrDefault();
                if (ack != null)
                {
                    return new []{new CbrSearchSubmittedResponse()
                    {
                        MSReportUniqueID = ack.MSReportUniqueID.Value,
                        CustomerReportUniqueID = ack.CustomerReportUniqueID,
                        ReportReceiptDateUTC = ack.MSReceivedDateUTC.Value,
                    }};
                }

                return null;
            }
        }

        public ComputerBuildReportAckResponse SetComputerBuildReportAckStatus(string reportUniqueId, bool isBeenSent)
        {
            using (var db = GetContext())
            {
                var reportUniqueIdGUID = Guid.Parse(reportUniqueId);
                var query = db.ComputerBuildReports.Include("HardwareBindingReports").Where(c => c.MSReportUniqueID == reportUniqueIdGUID).Single();
                query.Status = isBeenSent;
                db.SaveChanges();
                return query.GetDomainData();
            }
        }

        #endregion

        #region Return report

        public Guid ReportReturn(ReturnReport returnReport)
        {
            Guid returnUniqueID = Guid.NewGuid();
            List<ReturnReportKey> returnReportKeys = returnReport.ReturnReportKeys.ToList();
            using (var db = GetContext())
            {
                List<ProductKeyInfo> dbKeys = GetProductKeyInfoes(db, returnReportKeys.Select(k => k.ProductKeyID).ToList());
                int i = 1;
                returnReportKeys.ForEach(returnReportKey =>
                {
                    ProductKeyInfo productKeyInfo = dbKeys.Single(k => k.ProductKeyID == returnReportKey.ProductKeyID);
                    productKeyInfo.ProductKeyStateID = 5;

                    string reasonCode = ValidateReturn(productKeyInfo, returnReport, returnReportKey);
                    returnReportKey.ReturnReasonCode = reasonCode;
                    returnReportKey.ReturnReasonCodeDescription = GetReturnReasonCodeDescription(reasonCode);
                    returnReportKey.LicensablePartNumber = productKeyInfo.LicensableName;
                    returnReportKey.MSReturnLineNumber = i++;
                });
                returnReport.ReturnUniqueID = returnUniqueID;
                returnReport.MSReturnNumber = GetMaxMSReturnNumber(db).ToString();
                returnReport.ReturnDateUTC = DateTime.UtcNow;
                returnReport.OEMRMADateUTC = DateTime.UtcNow;
                returnReport.status = true;
                db.ReturnReports.AddObject(returnReport);
                db.SaveChanges();
                return returnUniqueID;
            }
        }

        public ReturnAck RetrieveReportReturn(Guid returnReportUniqueID)
        {
            using (var db = GetContext())
            {
                ReturnReport returnReportAck = db.ReturnReports.Include("ReturnReportKeys").Where(c => c.ReturnUniqueID == returnReportUniqueID).SingleOrDefault();
                if (returnReportAck != null)
                {
                    return returnReportAck.GetDomainData();
                }
                else
                    return null;
            }
        }

        public ReturnSearchSubmittedResponse[] SearchSubmittedReturn(string oemRMANumber, DateTime oemRMADateUTC)
        {
            using (var db = GetContext())
            {
                ReturnReport  ack= db.ReturnReports.Where(c => c.OEMRMANumber == oemRMANumber && c.OEMRMADateUTC == oemRMADateUTC).SingleOrDefault();
                if (ack != null)
                {
                    return new [] {new ReturnSearchSubmittedResponse()
                    {
                        OEMRMANumber = ack.OEMRMANumber,
                        OEMRMADateUTC = ack.OEMRMADateUTC,
                        ReturnReceiptDateUTC = ack.ReturnDateUTC.Value,
                        ReturnUniqueID = ack.ReturnUniqueID,
                    }};
                }

                return null;
            }
        }

        public Guid[] RetrieveReturnAcknowledge()
        {
            using (var db = GetContext())
            {
                var result= db.ReturnReports.Where(c => c.status == true).ToList();
                foreach (ReturnReport report in result)
                {
                    report.status = false;
                }
                db.SaveChanges();
                return result.Select(r => r.ReturnUniqueID).ToArray();
            }
        }

        #endregion

        #region Private methods

        private WCFDataEntities GetContext()
        {
            return new WCFDataEntities();
        }

        private int GetMaxFulfillmentNumber(WCFDataEntities context)
        {
            List<int> stringList = (from p in context.ProductKeyInfoes
                                    where p.FulfillmentNumber != null
                                    select p.FulfillmentNumber).Cast<int>().ToList();

            if (stringList == null || stringList.Count == 0)
                return 1;
            else
            {
                int result = stringList.Max();
                result++;
                return result;
            }
        }

        private int GetMaxMSReturnNumber(WCFDataEntities context)
        {
            List<int> stringList = (from p in context.ReturnReports
                                    where p.MSReturnNumber != null
                                    select p.MSReturnNumber).Cast<int>().ToList();

            if (stringList == null || stringList.Count == 0)
                return 1;
            else
            {
                int result = stringList.Max();
                result++;
                return result;
            }
        }

        private List<ProductKeyInfo> GetProductKeyInfoes(WCFDataEntities context, List<long> productKeyInfoIds)
        {
            return context.ProductKeyInfoes.Where(k => productKeyInfoIds.Contains(k.ProductKeyID)).ToList();
        }

        private string Validate(List<ProductKeyInfo> context, HardwareBindingReport hardwareBindingReport)
        {
            string reasonCode;
            var productKeyInfo = context.SingleOrDefault(k => k.ProductKeyID == hardwareBindingReport.ProductKeyID);
            if (!ValidateDuplicateProductKey(context, hardwareBindingReport.ProductKeyID))
                reasonCode = "01";
            else 
                reasonCode = GetRandomCbrReasonCode();
            return reasonCode;
        }

        private string GetRandomCbrReasonCode() {
            string reasonCode = null;
            while (reasonCode == null || reasonCode == "01") {
                reasonCode = r.Next(0, 8).ToString("d2");
            }
            return reasonCode;
        }

        private string GetReasonCodeDescription(string reasonCode)
        {
            switch (reasonCode)
            {
                case "00":
                    return "Activation Enabled";
                case "01":
                    return "Duplicate product key ID";
                case "02":
                    return "Product key ID has been activation-enabled through the activation override process";
                case "03":
                    return "Product key ID has not been delivered or fulfilled";
                case "04":
                    return "Product key ID not fulfilled to the Sold-To location on the Computer Build Report";
                case "05":
                    return "Product key ID has already been returned";
                case "06":
                    return "Product key ID has been reported as lost or stolen";
                case "07":
                    return "Product key ID has already been activation-enabled";
                case "08":
                    return "Invalid Optional Info attribute name";
                default:
                    return "Not Invalid reason Code";
            }
        }

        private string GetReturnReasonCodeDescription(string reasonCode)
        {
            switch (reasonCode)
            {
                case "OJ":
                    return "Accepted/Credit/End user return ";
                case "OK":
                    return "Accepted/Credit/Agmt terminated";
                case "OL":
                    return "Accepted/Credit/Agmt expired";
                case "OM":
                    return "Accepted/Credit/Product EOL";
                case "OO":
                    return "Accepted/Credit/Stock balancing";
                case "OP":
                    return "Accepted/Credit/Manufacturing damage";
                case "OQ":
                    return "Accepted/Credit/Key used for testing";
                case "PA":
                    return "Rejected/No Credit/Invalid sold-to";
                case "PB":
                    return "Rejected/No Credit/Invalid DPK";
                case "PC":
                    return "Rejected/No Credit/MBR DPK";
                case "PD":
                    return "Rejected/No Credit/TKEY DPK";
                case "PE":
                    return "Rejected/No Credit/Duplicate override ";
                case "PF":
                    return "Rejected/No Credit/Previously returned ";
                case "PG":
                    return "Rejected/No Credit/Duplicate DPK";
                case "PI":
                    return "Rejected/No Credit/Over 365 days";
                case "PJ":
                    return "Rejected/No Credit/Invalid return type";
                case "PK":
                    return "Rejected/No Credit/Agmt terminated";
                case "PL":
                    return "Rejected/No Credit/Agmt expired";
                case "PM":
                    return "Rejected/No Credit/Product EOL";
                case "PN":
                    return "Rejected/No Credit/Over stck bal limit";
                case "PO":
                    return "Rejected/No Credit/Over stck bal date";
                case "PP":
                    return "Rejected/No Credit/Invalid return type";
                case "QC":
                    return "Accepted/No Credit/MBR DPK";
                case "QD":
                    return "Accepted/No Credit/TKEY DPK";
                case "QH":
                    return "Accepted/No Credit/Lost-stolen-other";
                case "QI":
                    return "Accepted/No Credit/Over 365 days";
                case "QK":
                    return "Accepted/No Credit/Agmt terminated";
                case "QL":
                    return "Accepted/No Credit/Agmt expired";
                case "QM":
                    return "Accepted/No Credit/Product EOL";
                case "QN":
                    return "Accepted/No Credit/Over stck bal limit";
                case "QO":
                    return "Accepted/No Credit/Over stck bal date";
                default:
                    return "Not Invalid reason Code";
            }
        }

        private string ValidateReturn(ProductKeyInfo key, ReturnReport returnReport, ReturnReportKey returnReportKey)
        {
            if (!returnReport.ReturnNoCredit)
            {
                return "OJ";
            }
            else
            {
                if (key == null)
                    return "PB";
                else if (key.ProductKeyStateID == 5)
                    return "PF";
                else
                    return "QK";
            }
        }

        private bool ValidateOptionalInfo(HardwareBindingReport hardwareBindingReport)
        {
            bool result = true;
            XDocument param = ParseAndValidateXML(hardwareBindingReport.OEMOptionalInfo, parameterSchema);
            if (param == null)
                result = true;
            else
                result = ValidateOptionalInfoName(param);
            return result;
        }

        private bool ValidateDuplicateProductKey(List<ProductKeyInfo> keys, long productKeyId)
        {
            return keys.Where(k => k.ProductKeyID == productKeyId).Count() == 1 ? true : false;
        }

        /// <summary>
        /// Parse parameters as XLinq Document
        /// </summary>
        /// <param name="parameters">Parameters passed from DMTool</param>
        /// <returns>XML document</returns>
        private XDocument ParseAndValidateXML(string parameters, string schema)
        {
            XDocument doc = null;

            try
            {
                doc = XDocument.Parse(parameters);
            }
            catch
            {
                return null;
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            XmlTextReader xtr = new XmlTextReader(
                assembly.GetManifestResourceStream(
               "WcfService.Schemas." + schema));

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, xtr);

            bool errors = false;
            doc.Validate(schemas, (o, e) =>
            {
                errors = true;
            });

            return errors ? null : doc;
        }

        private bool ValidateOptionalInfoName(XDocument parameterXml)
        {
            bool result = true;
            foreach (XElement element in parameterXml.Elements("ExtendedProperty").Descendants())
            {
                string parameterName = element.Attribute("Name").Value;

                if (!section.AllKeys.Contains(parameterName))
                {
                    //MessageLogger.LogSystemRunning("AttachParameters","Invalid Parameters =" +
                    //    parameterXml.ToString(), TraceEventType.Warning);
                    result = false;
                }
            }
            return result;
        }

        #endregion

    }
}