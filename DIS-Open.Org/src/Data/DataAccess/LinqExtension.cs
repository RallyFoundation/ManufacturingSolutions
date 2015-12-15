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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Reflection;

namespace DIS.Data.DataAccess
{
    internal static class LinqExtension
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, string columnName, bool orderByDesc)
        {
            return query.OrderBy(string.Format("{0} {1}", columnName, orderByDesc ? "desc" : "asc"));
        }
    }
}
