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
    public class LogSearchCriteria : SearchCriteriaBase
    {
        public bool ShouldIncludeMessage { get; set; }
        public int MessageLength { get; set; }

        public string Category { get; set; }
        public int Severity { get; set; }
        public string UserName { private get; set; }

        public string Title
        {
            get
            {
                if (Category == Constants.SystemCategoryName || string.IsNullOrEmpty(UserName))
                    return null;
                return string.Format("{0} - {1}", "KMT", UserName);
            }
        }

        public LogSearchCriteria()
        {
            MessageLength = 200;

            SortBy = "TimestampUtc";
            SortByDesc = true;
        }
    }
}
