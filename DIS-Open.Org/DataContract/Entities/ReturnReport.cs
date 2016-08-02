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
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIS.Data.DataContract
{
    [DataContract]
    public class ReturnReport
    {
        public ReturnReport()
        {
            this.ReturnReportKeys = new List<ReturnReportKey>();
        }
        [DataMember]
        public System.Guid CustomerReturnUniqueId { get; set; }
        [DataMember]
        public Nullable<System.Guid> ReturnUniqueId { get; set; }
        [DataMember]
        public string MsReturnNumber { get; set; }
        [DataMember]
        public Nullable<DateTime> ReturnDateUTC { get; set; }
        [DataMember]
        public Nullable<DateTime> OemRmaDateUTC { get; set; }
        [DataMember]
        public string OemRmaNumber { get; set; }
        [DataMember]
        public DateTime OemRmaDate { get; set; }
        [DataMember]
        public string SoldToCustomerId { get; set; }
        [DataMember]
        public bool ReturnNoCredit { get; set; }
        [DataMember]
        public string SoldToCustomerName { get; set; }
        public int ReturnReportStatusId { get; set; }
        [DataMember]
        public List<ReturnReportKey> ReturnReportKeys { get; set; }

        [NotMapped]
        public ReturnReportStatus ReturnReportStatus
        {
            get { return (ReturnReportStatus)ReturnReportStatusId; }
            set { ReturnReportStatusId = (int)value; }
        }
    }
}
