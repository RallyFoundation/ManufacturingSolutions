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
using System.Collections.ObjectModel;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;


namespace DIS.Presentation.KMT.ViewModel
{
    public class ReportMsFailedNotificationViewModel : ViewModelBase
    {
        #region Private Members

        private string title = string.Empty;
        private string summaryText = string.Empty;
        private ObservableCollection<KeyInfo> keys = null;

        #endregion

        #region Public Properties

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        public string SummaryText
        {
            get
            {
                return summaryText;
            }
            set
            {
                summaryText = value;
                RaisePropertyChanged("SummaryText");
            }
        }

        public ObservableCollection<KeyInfo> Keys
        {
            get { return keys; }
            set
            {
                keys = value;
                RaisePropertyChanged("Keys");
            }
        }

        #endregion

        #region Constructor
        
        public ReportMsFailedNotificationViewModel(NotificationCategory category ,List<KeyInfo> keys)
        {
            this.keys = new ObservableCollection<KeyInfo>(keys);

            if (category == NotificationCategory.ReportedKeysFaild)
                SetReportedNotice();
            else if (category == NotificationCategory.ReturnedKeysFaild)
                SetReturnedNotice();

        }
        
        #endregion

        #region private Members

        private void SetReportedNotice()
        {
            Title = ResourcesOfR6.Common_ReportedKeyFailedTitle;
            SummaryText = string.Format(ResourcesOfR6.Common_ReportedKeyFailed, keys.Count);
        }

        private void SetReturnedNotice()
        {
            Title = ResourcesOfR6.Common_ReturnedKeyFailedTitle;
            SummaryText = string.Format(ResourcesOfR6.Common_ReturnedKeyFailed, keys.Count);
        }

        #endregion
    }
}