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
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;

namespace DIS.Data.DataContract
{
    public static class Constants
    {
        public const int BatchLimit = 5000;
        public const int RefulfillmentTimeLimit = 72;
        public const int PulseInterval = 20000;

        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        public static readonly bool IsMultipleEnabled = false;
        public static InstallType InstallType
        {
            get { return (InstallType)Enum.Parse(typeof(InstallType), ConfigurationManager.AppSettings["InstallType"]); }
        }

        public const string ManagerRoleName = "Manager";
        public const string OperatorRoleName = "Operator";

        public const string SystemCategoryName = "System";

        public static class CBRAckReasonCode
        {
            public const string ActivationEnabled = "00";
            public const string DuplicateProductKeyId = "01";
        }

        public enum ActionCode
        {
            Preserve,
            Inserted,
            Updated,
            Deleted
        }

        public enum ExportType
        {
            FulfilledKeys,
            ReFulfilledKeys,
            ReportKeys,
            ReReportKeys,
            ToolKeys,
            ReToolKeys,
            CBR,
            ReCBR,
            DuplicateCBR,
            ReDuplicateCBR,
            ReturnKeys,
            ReReturnKeys,
            ReturnAck

        }

        public enum ImportFileType
        {
            Encrypted,
            R6_NonEncrypted,
            R6_Encrypted,
            R5_NonEncrypted,
            R5_Encrypted,
            R4_NonEncrypted,
            R4_Encrypted
        }
    }
}
