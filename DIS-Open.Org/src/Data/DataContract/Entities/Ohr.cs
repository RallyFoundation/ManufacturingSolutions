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

namespace DIS.Data.DataContract
{
    [DataContract]
    public class Ohr
    {
        [DataMember]
        public Nullable<System.Guid> MsUpdateUniqueId { get; set; }
        [DataMember]
        public System.Guid CustomerUpdateUniqueId { get; set; }
        [DataMember]
        public Nullable<System.DateTime> MsReceivedDateUtc { get; set; }
        [DataMember]
        public string SoldToCustomerId { get; set; }
        [DataMember]
        public string ReceivedFromCustomerId { get; set; }
        [DataMember]
        public Nullable<int> TotalLineItems { get; set; }
        [DataMember]
        public int OhrStatusId { get; set; }
        [DataMember]
        public System.DateTime CreatedDateUtc { get; set; }
        [DataMember]
        public System.DateTime ModifiedDateUtc { get; set; }
        [DataMember]
        public ICollection<OhrKey> OhrKeys  { get; set; }

        [NotMapped]
        public OhrStatus OhrStatus
        {
            get { return (OhrStatus)OhrStatusId; }
            set { OhrStatusId = (int)value; }
        }
        [NotMapped]
        public DateTime ModifiedDate
        {
            get
            {
                DateTimeOffset offset = new DateTimeOffset(ModifiedDateUtc, TimeZoneInfo.Utc.GetUtcOffset(ModifiedDateUtc));
                return offset.LocalDateTime;
            }
        }

        [NotMapped]
        public DateTime? MsReceivedDate
        {
            get
            {
                DateTime? date = null;
                if (MsReceivedDateUtc.HasValue)
                    date = new DateTimeOffset(MsReceivedDateUtc.Value, TimeZoneInfo.Utc.GetUtcOffset(MsReceivedDateUtc.Value)).LocalDateTime;
                return date;
            }
        }

        [NotMapped]
        public List<KeyInfo> Keys
        {
            get
            {
                return OhrKeys.ToKeys();
            }

        }

        public Ohr()
        {
        }

        public Ohr(List<KeyInfo> keys)
        {
            this.OhrKeys = keys.ToOhrKeys();
        }

    }
}
