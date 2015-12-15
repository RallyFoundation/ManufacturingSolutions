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

using System.Runtime.Serialization;
using System;

namespace DIS.Data.ServiceContract
{
    /// <summary>
    /// TransferKey class is used to populate the TransferKey relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class TransferKey
    {
        #region Public fields

        /// <summary>
        /// Product Key Id
        /// </summary>
        [DataMember(Order = 1)]
        public long ProductKeyId { get; set; }

        /// <summary>
        /// Product Key State id
        /// </summary>
        [DataMember(Order = 2)]
        public byte ProductKeyStateId { get; set; }

        /// <summary>
        /// Product Key State Name
        /// </summary>
        [DataMember(Order = 3)]
        public string ProductKeyState { get; set; }

        /// <summary>
        /// Product Key
        /// </summary>
        [DataMember(Order = 4)]
        public string ProductKey { get; set; }

        /// <summary>
        /// OEm Additional Info
        /// </summary>
        [DataMember(Order = 5)]
        public string OEMOptionalInfo { get; set; }

        /// <summary>
        /// Hardware Id
        /// </summary>
        [DataMember(Order = 6)]
        public string HardwareId { get; set; }

        /// <summary>
        /// OEM Part number
        /// </summary>
        [DataMember(Order = 7)]
        public string OEMPartNumber { get; set; }

        /// <summary>
        /// Business Name
        /// </summary>
        [DataMember(Order = 8)]
        public string SoldToCustomerName { get; set; }

        /// <summary>
        /// Order unique Id from MS
        /// </summary>
        [DataMember(Order = 9)]
        public System.Guid OrderUniqueID { get; set; }

        /// <summary>
        /// Customer Number
        /// </summary>
        [DataMember(Order = 10)]
        public string SoldToCustomerID { get; set; }

        /// <summary>
        /// Reference Number
        /// </summary>
        [DataMember(Order = 11)]
        public string CallOffReferenceNumber { get; set; }

        /// <summary>
        ///OEM Part Number
        /// </summary>
        [DataMember(Order = 12)]
        public string OEMPONumber { get; set; }

        /// <summary>
        /// MS Order Number
        /// </summary>
        [DataMember(Order = 13)]
        public string MSOrderNumber { get; set; }

        /// <summary>
        ///MS Line Item Number
        /// </summary>
        [DataMember(Order = 14)]
        public int MSOrderLineNumber { get; set; }

        /// <summary>
        /// Key Quantity
        /// </summary>
        [DataMember(Order = 15)]
        public int Quantity { get; set; }

        /// <summary>
        /// Licensable Part Number
        /// </summary>
        [DataMember(Order = 16)]
        public string LicensablePartNumber { get; set; }

        /// <summary>
        /// SKU ID
        /// </summary>
        [DataMember(Order = 17)]
        public string SKUID { get; set; }

        [DataMember(Order = 17)]
        public DateTime? OEMPODateUTC { get; set; }

        [DataMember(Order = 18)]
        public string ShipToCustomerID { get; set; }

        [DataMember(Order = 19)]
        public string ShipToCustomerName { get; set; }

        [DataMember(Order = 20)]
        public string LicensableName { get; set; }

        [DataMember(Order = 21)]
        public string OEMPOLineNumber { get; set; }

        [DataMember(Order = 22)]
        public string CallOffLineNumber { get; set; }

        [DataMember(Order = 23)]
        public bool? FulfillmentResendIndicator { get; set; }

        [DataMember(Order = 24)]
        public string FulfillmentNumber { get; set; }

        [DataMember(Order = 25)]
        public DateTime? FulfilledDateUTC { get; set; }

        [DataMember(Order = 26)]
        public DateTime? FulfillmentCreateDateUTC { get; set; }

        [DataMember(Order = 27)]
        public string EndItemPartNumber { get; set; }

        #endregion
    }
}
