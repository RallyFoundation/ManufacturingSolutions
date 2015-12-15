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
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.Models {
    public class CategoryModel : IComparable<CategoryModel> {
        private static readonly Dictionary<string, string> categoryNames = new Dictionary<string, string>(){
            { MessageLogger.SystemCategoryName, MergedResources.Common_SystemLog },
            { MessageLogger.OperationCategoryName, MergedResources.Common_OperationLog },
        };

        public Category Category { get; set; }

        public string DisplayName {
            get { return categoryNames[Category.CategoryName]; }
        }

        public int Order {
            get {
                switch (Category.CategoryName) {
                    case MessageLogger.SystemCategoryName:
                        return 0;
                    case MessageLogger.OperationCategoryName:
                        return 1;
                    default:
                        return 2;
                }
            }
        }

        public int CompareTo(CategoryModel other) {
            return Order.CompareTo(other.Order);
        }

        public override string ToString() {
            return DisplayName;
        }
    }
}
