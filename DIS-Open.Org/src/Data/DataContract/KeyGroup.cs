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

using System.ComponentModel;
using System;

namespace DIS.Data.DataContract
{
    public class KeyGroup
    {
        /// <summary>
        /// OEM PO Number
        /// </summary>
        public string OEMPONumber { get; set; }

        /// <summary>
        /// Ms Licensable PartNumber
        /// </summary>
        public string MsLicensablePartNumber { get; set; }

        /// <summary>
        /// OEM PartNumber
        /// </summary>
        public string OEMPartNumber { get; set; }

        public Nullable<int> KeyTypeId { get; set; }
        public KeyType? KeyType
        {
            get { return (KeyType?)KeyTypeId; }
            set { KeyTypeId = (int?)value; }
        }
        /// <summary>
        /// Available keys can export count
        /// </summary>
        public int AvailableKeysCount { get; set; }

        public int Quantity { get; set; }

        public KeySearchCriteria[] Criterias { get; set; }

        public KeySearchCriteria[] ToSearchCriterias()
        {
            foreach (KeySearchCriteria c in Criterias)
            {
                c.OemPoNumber = OEMPONumber;
                c.OemPartNumber = OEMPartNumber;
                c.MsPartNumber = MsLicensablePartNumber;
                c.KeyType = KeyType;
                c.PageSize = Quantity;
            }
            return Criterias;
        }
    }
}
