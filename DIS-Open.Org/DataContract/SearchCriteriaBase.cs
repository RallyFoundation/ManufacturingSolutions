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
    public class SearchCriteriaBase
    {
        public const int DefaultPageSize = 10;

        public int PageSize { get; set; }

        public int StartIndex { get; set; }

        public int PageNumber
        {
            set
            {
                StartIndex = (value - 1) * PageSize;
            }
        }

        /// <summary>
        /// Date from
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Date to
        /// </summary>
        public DateTime? DateTo { get; set; }

        public DateTime? DateFromUtc
        {
            get
            {
                if (DateFrom == null)
                    return null;
                else
                    return DateFrom.Value.ToUniversalTime();
            }
        }

        public DateTime? DateToUtc
        {
            get
            {
                if (DateTo == null)
                    return null;
                else
                    return DateTo.Value.ToUniversalTime();
            }
        }

        public string SortBy { get; set; }

        public bool SortByDesc { get; set; }

    }
}
