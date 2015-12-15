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
using System.Collections.ObjectModel;
using System.Linq;
using DIS.Business.Proxy;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ReFulfillmentNotificationViewModel : ReOperationNotificationViewModelBase<FulfillmentInfo>
    {
        private ObservableCollection<FulfillmentInfo> allInfoes;
        private ObservableCollection<FulfillmentInfo> oldInfoes;
        private IKeyProxy keyProxy;
        private bool isShowAllChecked = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fulfillmentInfoes"></param>
        /// <param name="keyProxy"></param>
        public ReFulfillmentNotificationViewModel(List<FulfillmentInfo> fulfillmentInfoes, IKeyProxy keyProxy)
            : base(fulfillmentInfoes)
        {
            oldInfoes = new ObservableCollection<FulfillmentInfo>(fulfillmentInfoes);
            this.keyProxy = keyProxy;
        }

        private ObservableCollection<FulfillmentInfo> AllInfoes
        {
            get 
            {
                if (allInfoes == null)
                {
                    allInfoes = new ObservableCollection<FulfillmentInfo>(keyProxy.GetFailedFulfillments(true));
                }
                return allInfoes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsShowAllChecked
        {
            get
            {
                return isShowAllChecked;
            }
            set
            {
                isShowAllChecked = value;
                if (isShowAllChecked)
                {
                    Infoes = AllInfoes;
                }
                else
                {
                    Infoes = oldInfoes;
                }
                RaisePropertyChanged("IsShowAllChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override string Result
        {
            get
            {
                return string.Join(Environment.NewLine, Infoes.Select(info => 
                    string.Format("FulfilmentInfo: Order Unique ID= {0},Fulfillment Number= {1}",
                      info.OrderUniqueId, info.FulfillmentNumber)));
            }
        }
    }
}
