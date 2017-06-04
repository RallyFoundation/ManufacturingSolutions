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

namespace DIS.Data.DataContract
{
	public class Role
	{
	    public Role()
		{
			this.Users = new List<User>();
		}

		public int RoleId { get; set; }
		public string RoleName { get; set; }
		public string Description { get; set; }
		public ICollection<User> Users { get; set; }
        public Constants.ActionCode ActionCode { get; set; }
	}
}

