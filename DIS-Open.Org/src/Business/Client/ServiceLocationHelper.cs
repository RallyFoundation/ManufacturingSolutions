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

namespace DIS.Business.Client
{
    /// <summary>
    /// Provides all web service URLs
    /// </summary>
    internal class ServiceLocationHelper
    {
        #region priviate & protected member variables

        private string hostUrl; 

        #endregion

        #region Constructors & Dispose

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostUrl"></param>
        public ServiceLocationHelper(string hostUrl)
        {
            this.hostUrl = hostUrl;
        } 

        #endregion

        #region public Methods

        public string TestInternal { get { return GetUrl("Diagnostic/Internal"); } }
        public string TestExternal { get { return GetUrl("Diagnostic/External"); } }
        public string TestDPS { get { return GetUrl("Diagnostic/DPS"); } }
        public string TestKPS { get { return GetUrl("Diagnostic/KPS"); } }
        public string DPSReport { get { return GetUrl("Diagnostic/DPS/Report"); } }
        public string KPSReport { get { return GetUrl("Diagnostic/KPS/Report"); } }
        public string TestDDF { get { return GetUrl("Diagnostic/DatabaseDiskFull"); } }
        public string DDFReport { get { return GetUrl("Diagnostic/DatabaseDiskFull/Report"); } }

        public string GetFulfilledKeysUrl { get { return GetUrl("fulfillments"); } }
        public string RetrieveFulfilmentUrl { get { return GetUrl("fulfillments/?status=ready"); } }
        public string ReportBindingUrl { get { return GetUrl("computerbuildreport/"); } }
        public string CBRAckUrl { get { return GetUrl("computerbuildreport/acknowledgements"); } }
        public string CBRSearchUrl { get { return GetUrl("computerbuildreport/royd/v1"); } }

        public string GetKeysUrl { get { return GetUrl("Keys/Get"); } }
        public string ReportKeysUrl { get { return GetUrl("Keys/Report"); } }
        public string SyncKeysUrl { get { return GetUrl("Keys/Sync"); } }
        public string RecallKeysUrl { get { return GetUrl("Keys/Recall"); } }

        public string CarbonCopyFulfilledKeysUrl { get { return GetUrl("Keys/CarbonCopy/Fulfilled"); } }
        public string CarbonCopyReportedKeysUrl { get { return GetUrl("Keys/CarbonCopy/Reported"); } }
        public string CarbonCopyReturnReportedKeysUrl { get { return GetUrl("Keys/CarbonCopy/Returned"); } }
        public string CarbonCopyReturnReportUrl { get { return GetUrl("/Keys/CarbonCopy/ReturnReport"); } }

        public string ReportReturnUrl { get { return GetUrl("return/"); } }
        public string ReturnAckUrl { get { return GetUrl("return/acknowledgements"); } }
        public string ReturnSearchUrl { get { return GetUrl("return/royd/v1"); } }

        public string SyncUrl { get { return GetUrl("Sync"); } }

        public string ReportOhrUrl { get { return GetUrl("oemhardwarereporting/royd/v1/"); } }
        public string OhrAckUrl { get { return GetUrl("oemhardwarereporting/royd/v1/acknowledgements"); } }

        #endregion

        #region Private & protected methods

        private string GetUrl(string location)
        {
            return string.Format("{0}/{1}", hostUrl, location);
        } 

        #endregion
    }
}
