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
	public class KeyExportLog
	{
		public int ExportLogId { get; set; }
		public string ExportTo { get; set; }
		public string ExportType { get; set; }
		public int KeyCount { get; set; }
		public string FileName { get; set; }
		public bool IsEncrypted { get; set; }
		public string FileContent { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
	}
}

