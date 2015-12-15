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
using System.Collections.ObjectModel;
using DIS.Data.DataContract;
using DIS.Business.Proxy;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ReAcknowledgementNotificationViewModel : ReOperationNotificationViewModelBase<Cbr>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cbrs"></param>
        public ReAcknowledgementNotificationViewModel(List<Cbr> cbrs)
            : base(cbrs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override string Result
        {
            get
            {
                return string.Join(Environment.NewLine, Infoes.Select(info => string.Format("Acknowledgement: MS Report Unique ID= {0},Customer Report Unique ID= {1}",
                      info.MsReportUniqueId, info.CbrUniqueId)));
            }
        }
    }
}
