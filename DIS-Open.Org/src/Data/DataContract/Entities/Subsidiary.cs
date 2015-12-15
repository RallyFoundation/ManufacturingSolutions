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
using System.ComponentModel.DataAnnotations;

namespace DIS.Data.DataContract
{
	public  class Subsidiary
	{
	    public Subsidiary()
		{
			this.KeyInfoExes = new List<KeyInfoEx>();
		}

		public int SsId { get; set; }
		public string DisplayName { get; set; }
        public string ServiceHostUrl { get; set; }
        public string UserName { get; set; }
		public string AccessKey { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public ICollection<KeyInfoEx> KeyInfoExes { get; set; }

        [NotMapped]
        public string Host
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ServiceHostUrl))
                {
                    Uri uri = new Uri(this.ServiceHostUrl);
                    return uri.Host;
                }
                return string.Empty;
            }
        }

        [NotMapped]
        public string Port
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ServiceHostUrl))
                {
                    Uri uri = new Uri(this.ServiceHostUrl);
                    return uri.Host;
                }
                return string.Empty;
            }
        }
	}
}

