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
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class KeysExpiredNotificationViewModel : ViewModelBase
    {
        private string timeLineDaysSummary;
        private ObservableCollection<KeyExpired> keyExpiredNotifications = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysExpired"></param>
        /// <param name="timeLineDays"></param>
        public KeysExpiredNotificationViewModel(List<KeyInfo> keysExpired, int timeLineDays)
        {
            this.TimeLineDaysSummary = string.Format(MergedResources.KeysExpiredNotificationViewModel_TimeLineDays, timeLineDays);

            if (this.KeyExpiredNotifications == null)
                this.KeyExpiredNotifications = new ObservableCollection<KeyExpired>();

            foreach (var key in keysExpired)
            {
                this.KeyExpiredNotifications.Add(new KeyExpired() 
                { 
                    keyInfo = key, 
                    OverDays = (DateTime.UtcNow - key.FulfilledDateUtc.Value).Days - timeLineDays >0 ?(DateTime.UtcNow - key.FulfilledDateUtc.Value).Days - timeLineDays : 1 
                });
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<KeyExpired> KeyExpiredNotifications
        {
            get { return keyExpiredNotifications; }
            set
            {
                keyExpiredNotifications = value;
                RaisePropertyChanged("KeyExpiredNotifications");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TimeLineDaysSummary
        {
            get { return timeLineDaysSummary; }
            set
            {
                timeLineDaysSummary = value;
                RaisePropertyChanged("TimeLineDaysSummary");
            }
        }
    }
}
