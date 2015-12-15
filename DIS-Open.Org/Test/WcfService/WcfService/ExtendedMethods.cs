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
using System.Web;
using DomainData = WcfService.Contracts.DomainData;
using System.Data.Objects;
using WcfService.Contracts;

namespace WcfService
{
    public static class ExtendedMethods
    {
        public static Key[] GetDomainData(this List<WcfService.ProductKeyInfo> source)
        {
            return (from p in source
                    select new Key
                    {
                        ProductKey = p.ProductKey,
                        ProductKeyID = p.ProductKeyID,
                        SKUID = p.SKUID,
                    }).ToArray();
        }

        public static Range[] GetRanges(this List<WcfService.ProductKeyInfo> source)
        {
            var beginningProductKeyID = source.ElementAt(0).ProductKeyID;
            var endingProductKeyID = source.ElementAt(source.Count - 1).ProductKeyID;
            return new Range[]{new Range
                   {
                       BeginningProductKeyID = beginningProductKeyID,
                       EndingProductKeyID = endingProductKeyID,
                   }};
        }

        public static KeyFulfillment[] GetDomainData(this List<WcfService.ProductKeyInfo> source, string fulfillmentId)
        {
            List<KeyFulfillment> lt = new List<KeyFulfillment>();
            if ((source != null) && (source.Count > 0))
            {
                var group = from p in source
                            group p by p.LicensablePartNumber into g
                            select new { Remainder = g.Key, Items = g };

                foreach (var grp in group)
                {
                    var p = grp.Items.ElementAt(0);
                    lt.Add(new KeyFulfillment
                    {
                        OrderUniqueID = p.OrderUniqueID.Value,
                        SoldToCustomerName = p.SoldToCustomerName,
                        SoldToCustomerID = p.SoldToCustomerID,
                        ShipToCustomerName = p.ShipToCustomerName,
                        ShipToCustomerID = p.ShipToCustomerID,
                        CallOffReferenceNumber = p.CallOffReferenceNumber,
                        OEMPODateUTC = p.OEMPODateUTC.HasValue ? p.OEMPODateUTC.Value : DateTime.MinValue,
                        OEMPONumber = p.OEMPONumber,
                        LicensablePartNumber = grp.Remainder,
                        FulfillmentNumber = p.FulfillmentNumber,
                        OEMPartNumber = p.OEMPartNumber,
                        FulfillmentCreateDateUTC = p.FulfillmentCreateDateUTC.HasValue ? p.FulfillmentCreateDateUTC.Value : DateTime.MinValue,
                        FulfilledDateUTC = p.FulfilledDateUTC.Value,
                        Quantity = p.Quantity.HasValue ? p.Quantity.Value : 0,
                        EndItemPartNumber = p.EndItemPartNumber,
                        MSOrderNumber = p.MSOrderNumber,
                        MSOrderLineNumber = p.MSOrderLineNumber.HasValue ? p.MSOrderLineNumber.Value : 0,
                        Keys = grp.Items.ToList().GetDomainData(),
                        Ranges = grp.Items.ToList().GetRanges(),
                    });
                }
            }
            return lt.ToArray();
        }

        public static ComputerBuildReport GetDomainData(this ComputerBuildReportRequest source)
        {
            return new ComputerBuildReport()
            {
                CustomerReportUniqueID = source.CustomerReportUniqueID,
                SoldToCustomerID = source.SoldToCustomerID,
                ReceivedFromCustomerID = source.ReceivedFromCustomerID,
                CBRAckFileNumber = source.TotalLineItems,
                CBRAckFileTotal = source.TotalLineItems,
                HardwareBindingReports = source.Bindings.GetDomainData(),
            };
        }

        public static List<HardwareBindingReport> GetDomainData(this IEnumerable<Binding> source)
        {
            return source.Select(s => new HardwareBindingReport()
            {
                ProductKeyID = s.ProductKeyID,
                HardwareHash = s.HardwareHash,
                //OEMOptionalInfo = s.OEMOptionalInfo != null ? s.OEMOptionalInfo.ToString() : null,
            }).ToList();
        }

        public static ComputerBuildReportAckResponse GetDomainData(this ComputerBuildReport computerBuildReport)
        {
            return new ComputerBuildReportAckResponse()
            {
                ComputerBuildReportAcks = new ComputerBuildReportAck[] { 
                    new ComputerBuildReportAck() {
                        MSReportUniqueID = computerBuildReport.MSReportUniqueID.Value,
                        CustomerReportUniqueID = computerBuildReport.CustomerReportUniqueID,
                        MSReceivedDateUTC = computerBuildReport.MSReceivedDateUTC.Value,
                        SoldToCustomerID = computerBuildReport.SoldToCustomerID,
                        ReceivedFromCustomerID = computerBuildReport.ReceivedFromCustomerID,
                        CBRAckFileTotal = computerBuildReport.CBRAckFileTotal.Value,
                        CBRAckFileNumber = computerBuildReport.CBRAckFileNumber.Value,
                        FailedValidations = computerBuildReport.HardwareBindingReports.GetFailedResults(),
                        SuccessfulValidations = computerBuildReport.HardwareBindingReports.GetSuccessfulResults(),
                    }
                }
            };
        }

        public static FailedValidationResult[] GetFailedResults(this IEnumerable<HardwareBindingReport> computerBuildReports)
        {
            return (from r in computerBuildReports
                    where r.ReasonCode != "00"
                    select new FailedValidationResult
                    {
                        ProductKeyID = r.ProductKeyID,
                        HardwareHash = r.HardwareHash,
                        ReasonCode = r.ReasonCode,
                        ReasonCodeDescription = r.ReasonCodeDescription,
                        OEMOptionalInfoErrors = string.Empty,
                    }).ToArray();
        }

        public static SuccessfulValidationResult[] GetSuccessfulResults(this IEnumerable<HardwareBindingReport> computerBuildReports)
        {
            return (from r in computerBuildReports
                    where r.ReasonCode == "00"
                    select new SuccessfulValidationResult
                    {
                        ProductKeyID = r.ProductKeyID,
                        HardwareHash = r.HardwareHash,
                        OEMOptionalInfoErrors = string.Empty,
                    }).ToArray();
        }

        public static ReturnReport GetDomainData(this ReturnRequest request)
        {
            return new ReturnReport()
            {
                SoldToCustomerID = request.SoldToCustomerID,
                OEMRMANumber = request.OEMRMANumber,
                OEMRMADate = request.OEMRMADate,
                ReturnNoCredit = request.ReturnNoCredit,
                ReturnReportKeys = request.ReturnLineItems.GetDomainData(),
            };
        }

        public static List<ReturnReportKey> GetDomainData(this IEnumerable<ReturnLineItem> source)
        {
            return source.Select(s => new ReturnReportKey()
            {
                ProductKeyID = s.ProductKeyID,
                OEMRMALineNumber = s.OEMRMALineNumber,
                ReturnTypeID = s.ReturnTypeID,
            }).ToList();
        }

        public static ReturnAck GetDomainData(this ReturnReport source)
        {
            return new ReturnAck()
            {
                ReturnUniqueID = source.ReturnUniqueID,
                MSReturnNumber = source.MSReturnNumber,
                ReturnDateUTC = source.ReturnDateUTC.Value,
                OEMRMANumber = source.OEMRMANumber,
                OEMRMADateUTC = source.OEMRMADateUTC,
                SoldToCustomerID = source.SoldToCustomerID,
                SoldToCustomerName = source.SoldToCustomerName,
                ReturnAckLineItems = source.ReturnReportKeys.GetDomainData(),
            };
        }

        public static ReturnAckLineItem[] GetDomainData(this IEnumerable<ReturnReportKey> source)
        {
            return source.Select(s => new ReturnAckLineItem()
            {
                MSReturnLineNumber = s.MSReturnLineNumber.Value,
                OEMRMALineNumber = s.OEMRMALineNumber.ToString(),
                ReturnTypeID = s.ReturnTypeID,
                ProductKeyID = s.ProductKeyID,
                LicensablePartNumber = s.LicensablePartNumber,
                ReturnReasonCode = s.ReturnReasonCode,
                ReturnReasonCodeDescription = s.ReturnReasonCodeDescription,
            }).ToArray();
        }
    }
}