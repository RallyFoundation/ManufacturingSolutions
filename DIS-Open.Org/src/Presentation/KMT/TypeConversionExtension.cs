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

using System.Collections.Generic;
using System.Linq;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Models;
using System;

namespace DIS.Presentation.KMT
{
    internal static class TypeConversionExtension
    {
        public static List<KeyInfoModel> ToKeyInfoModel(this IEnumerable<KeyInfo> keys)
        {
            return keys.Select(k => new KeyInfoModel() { keyInfo = k }).ToList();
        }

        public static List<KeyGroupModel> ToKeyGroupModel(this IEnumerable<KeyGroup> keyGroups)
        {
            return keyGroups.Select(k => new KeyGroupModel() { KeyGroup = k }).ToList();
        }

        public static ReturnReport ToReturnReport(this IEnumerable<ReturnKeyModel> returnKeys, string OemRmaNumber,DateTime OemRmaDate, bool ReturnNoCredit)
        {         
            int i = 0;
            ReturnReport request = new ReturnReport()
            {
                CustomerReturnUniqueId = System.Guid.NewGuid(),
                OemRmaNumber = OemRmaNumber,
                OemRmaDate = OemRmaDate,
                OemRmaDateUTC = OemRmaDate.ToUniversalTime(),
                SoldToCustomerId = returnKeys.First().ReturnReportKey.keyInfo.SoldToCustomerId,
                ReturnNoCredit = ReturnNoCredit,
                ReturnReportStatusId = (int)ReturnReportStatus.Reported,
                ReturnReportKeys = returnKeys.Where(k => k.ReturnReportKey.IsSelected).Select(k => new ReturnReportKey()
                {
                    OemRmaLineNumber = i + 1,
                    ReturnTypeId = ToReturKeyType(k.SelectReturnRequestType),
                    KeyId = k.ReturnReportKey.keyInfo.KeyId,
                    PreProductKeyStateId = k.ReturnReportKey.keyInfo.KeyStateId,
                }).ToList()
            };
            return request;
        }

        public static DateTime ToUTCTime(this DateTime vDate)
        {
            DateTimeOffset offset = new DateTimeOffset(vDate, TimeZoneInfo.Utc.GetUtcOffset(vDate));
            return offset.UtcDateTime;
        }

        private static string ToReturKeyType(string selectReturnRequestType)
        {
            string type = string.Empty;
            if (selectReturnRequestType == DIS.Presentation.KMT.Properties.ResourcesOfR6.ReturnKeysView_ZOADescription)
                type = ReturnRequestType.ZOA.ToString();

            if (selectReturnRequestType == DIS.Presentation.KMT.Properties.ResourcesOfR6.ReturnKeysView_ZOBDescription)
                type = ReturnRequestType.ZOB.ToString();

            if (selectReturnRequestType == DIS.Presentation.KMT.Properties.ResourcesOfR6.ReturnKeysView_ZOCDescription)
                type = ReturnRequestType.ZOC.ToString();

            if (selectReturnRequestType == DIS.Presentation.KMT.Properties.ResourcesOfR6.ReturnKeysView_ZODDescription)
                type = ReturnRequestType.ZOD.ToString();

            if (selectReturnRequestType == DIS.Presentation.KMT.Properties.ResourcesOfR6.ReturnKeysView_ZOEDescription)
                type = ReturnRequestType.ZOE.ToString();

            if (selectReturnRequestType == DIS.Presentation.KMT.Properties.ResourcesOfR6.ReturnKeysView_ZOFDescription)
                type = ReturnRequestType.ZOF.ToString();
            return type;
        }
    }
}
