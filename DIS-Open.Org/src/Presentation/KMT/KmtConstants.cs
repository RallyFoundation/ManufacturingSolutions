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
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using DIS.Data.DataContract;
using DISConfigurationCloud.Contract;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// Configuration class for reading config information
    /// </summary>
    internal class KmtConstants
    {
        private static User loginUser;

        #region Public Properties

        public static User LoginUser
        {
            get
            {
                return loginUser;
            }
            set
            {

                loginUser = value;
                if (loginUser != null)
                {
                    if (!string.IsNullOrEmpty(loginUser.Language))
                    {
                        CurrentCulture =
                            Thread.CurrentThread.CurrentCulture =
                            Thread.CurrentThread.CurrentUICulture =
                            new CultureInfo(loginUser.Language);
                    }
                }
            }
        }
        public static bool IsManager
        {
            get
            {
                return LoginUser.RoleName == Constants.ManagerRoleName;
            }
        }
        public static CultureInfo CurrentCulture { get; set; }

        public static int? HeadQuarterId
        {
            get
            {
                return CurrentHeadQuarter == null ? (int?)null : CurrentHeadQuarter.HeadQuarterId;
            }
        }

        public static HeadQuarter CurrentHeadQuarter { get; set; }

        public static string HttpsUrlFormat
        {
            get
            {
                return string.Format(ServiceUrlFormat, "https");
            }
        }

        public static bool IsOemCorp
        {
            get { return Constants.InstallType == InstallType.Oem; }
        }

        public static bool IsTpiCorp
        {
            get { return Constants.InstallType == InstallType.Tpi; }
        }

        public static bool IsFactoryFloor
        {
            get { return Constants.InstallType == InstallType.FactoryFloor; }
        }

        public static string InventoryName
        {
            get
            {
                switch (Constants.InstallType)
                {
                    case InstallType.Oem:
                        return "Corporate Key Inventory";
                    case InstallType.Tpi:
                        return "Factory Key Inventory";
                    case InstallType.FactoryFloor:
                        return "Factory Floor Key Inventory";
                    default:
                        throw new ApplicationException("Unsupported InstallType");
                }
            }
        }

        public static int OldTimeline { get; set; }

        public const int MinInterval = 1;
        public const int MaxInterval = 9999;
        public const int DefaultPageSize = 10;
        public static readonly string[] PageSizeList;
        public static double NotificationCheckInterval
        {
            get { return double.Parse(ConfigurationManager.AppSettings["NotificationCheckInterval"]); }
        }

        public const int FirstTab = 0;
        public const int SecondTab = 1;
        public const int MinLifeDays = 1;
        public const int MaxLifeDays = 3600;//Fix for Bug#125 - Rally, Dec.16, 2014

        public static string CurrentCustomerID;

        public static string CurrentConfigurationID;

        public static string CurrentDBConnectionString;

        public static List<Customer> CloudCustomers;

        public static Dictionary<string, string> CloudConfigurations;

        public static string XSLT_ULSKeyReportCompitable = @".\XSLT\TransformULSKeyReportCompitable.xslt";

        public static string XSLT_ULSKeyExportCompitable = @".\XSLT\TransformULSKeyExportCompitable.xslt";

        public static string XSLT_DLSKeyImportCompitable = @".\XSLT\TransformDLSKeyImportCompitable.xslt";

        #endregion

        #region Constructors & Dispose

        static KmtConstants()
        {
            CurrentCulture = CultureInfo.CurrentUICulture;

            string pageSize = ConfigurationManager.AppSettings["PageSize"];
            if (string.IsNullOrEmpty(pageSize))
            {
                PageSizeList = new string[] { DefaultPageSize.ToString() };
            }
            else
            {
                PageSizeList = pageSize.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        #endregion

        private const string ServiceUrlFormat = "{0}://{{0}}:{{1}}/KeyBinding.svc";
    }

    /// <summary>
    /// Add,Edit or Change User's Password operation
    /// </summary>
    public enum UserOperation
    {
        /// <summary>
        /// 
        /// </summary>
        Add = 0,

        /// <summary>
        /// 
        /// </summary>
        Edit = 1,

        /// <summary>
        /// 
        /// </summary>
        SetAccount = 2,
    }
}
