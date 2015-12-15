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
	public class KeyOperationHistory
	{
	    public KeyOperationHistory()
		{
			this.KeysDuplicated = new List<KeyDuplicated>();
		}

		public int OperationId { get; set; }
		public long KeyId { get; set; }
		public string ProductKey { get; set; }
		public string HardwareHash { get; set; }
		public byte KeyStateFrom { get; set; }
		public byte KeyStateTo { get; set; }
		public string Operator { get; set; }
		public string Message { get; set; }
		public Nullable<System.DateTime> CreatedDate { get; set; }
		public ICollection<KeyDuplicated> KeysDuplicated { get; set; }
		public KeyInfo KeyInfo { get; set; }
	}
}

