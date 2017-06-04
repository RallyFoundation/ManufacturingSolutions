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
    public class ExportLogSearchCriteria : SearchCriteriaBase
    {
        public ExportLogSearchCriteria()
        {
            SortBy = "CreateDate";
            SortByDesc = true;
        }

        public string ExportFrom { get; set; }
        public string ExportTo { get; set; }
        public List<string> ExportTypes { get; set; }
        public string FileName { get; set; }
        public string CreateBy { get; set; }
        public bool? IsEncrypted { get; set; }
    }
}
