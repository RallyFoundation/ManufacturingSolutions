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

namespace DIS.Data.DataContract
{
    public class ExportKeyInfo
    {
        public long ProductKeyId { get; set; }     

        public string ProductKey { get; set; }

        public KeyState ProductKeyState { get; set; }

        public string HardwareHash { get; set; }

        //public string OEMOptionalInfo { get; set; }
        //public OemOptionalInfo OEMOptionalInfo { get; set; }
        public List<Field> OEMOptionalInfo { get; set; }

        public string OEMPartNumber { get; set; }

        public string SoldToCustomerName { get; set; }

        public System.Guid? OrderUniqueID { get; set; }

        public string SoldToCustomerID { get; set; }

        public string CallOffReferenceNumber { get; set; }

        public string OEMPONumber { get; set; }

        public string MSOrderNumber { get; set; }

        public int? MSOrderLineNumber { get; set; }

        public string LicensablePartNumber { get; set; }

        public string SKUID { get; set; }

        public DateTime? OEMPODateUTC { get; set; }

        public string ShipToCustomerID { get; set; }

        public string ShipToCustomerName { get; set; }

        public string LicensableName { get; set; }

        public string OEMPOLineNumber { get; set; }

        public string CallOffLineNumber { get; set; }

        public bool? FulfillmentResendIndicator { get; set; }

        public string FulfillmentNumber { get; set; }

        public DateTime? FulfilledDateUTC { get; set; }

        public DateTime? FulfillmentCreateDateUTC { get; set; }

        public string EndItemPartNumber { get; set; }

        public string TrackingInfo { get; set; }

        public int? KeyType { get; set; }

        public string SerialNumber { get; set; }

        #region For DIS 1.95 Accommodations - Rally Aug. 20, 2015
        public long? PbrBindingKeyId { get; set; }

        public int? PbrStateId { get; set; }

        public bool? CanBindPbr { get; set; }

        public bool ShouldSerializePbrBindingKeyId() { return PbrBindingKeyId.HasValue || this.shouldIncludePbrProperties; }

        public bool ShouldSerializePbrStateId() { return PbrStateId.HasValue || this.shouldIncludePbrProperties; }

        public bool ShouldSerializeCanBindPbr() { return CanBindPbr.HasValue || this.shouldIncludePbrProperties; }

        private bool shouldIncludePbrProperties = false;

        public void ShouldIncludePbrProperties(bool shouldOrNot) 
        {
            this.shouldIncludePbrProperties = shouldOrNot;
        }

        #endregion
    }
}
