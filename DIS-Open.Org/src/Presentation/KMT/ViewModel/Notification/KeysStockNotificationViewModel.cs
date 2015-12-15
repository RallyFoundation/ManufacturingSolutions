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

namespace DIS.Presentation.KMT.ViewModel
{
    public class KeysStockNotificationViewModel : ViewModelBase
    {
        private ObservableCollection<KeyTypeConfiguration> keysStockNotifications;

        public ObservableCollection<KeyTypeConfiguration> KeysStockNotifications
        {
            get { return keysStockNotifications; }
            set
            {
                keysStockNotifications = value;
                RaisePropertyChanged("KeysStockNotifications");
            }
        }

        public KeysStockNotificationViewModel(List<KeyTypeConfiguration> configs)
        {
            KeysStockNotifications = new ObservableCollection<KeyTypeConfiguration>(configs);
        }
    }
}
