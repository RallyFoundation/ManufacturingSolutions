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
using System.Windows;
using System.Windows.Input;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    public sealed class SystemStateNotificationViewModel : ViewModelBase
    {
        private string errorTitle = string.Empty;
        private DelegateCommand copyCommand;
        private string result = string.Empty;

        public string ErrorTitle
        {
            get
            {
                return errorTitle;
            }
            set
            {
                errorTitle = value;
                RaisePropertyChanged("ErrorTitle");
            }
        }

        public SystemStateNotificationViewModel(string errorTitle, string result)
        {
            this.ErrorTitle = errorTitle;
            this.Result = result;
        }

        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        public ICommand CopyCommand
        {
            get
            {
                if (copyCommand == null)
                    copyCommand = new DelegateCommand(Copy);
                return copyCommand;
            }
        }

        private void Copy()
        {
            Clipboard.SetText(Result);
            var messageResult = MessageBox.Show(MergedResources.Notification_CopiedToClipboard, MergedResources.Common_Information, MessageBoxButton.OK, MessageBoxImage.Information);
            if (messageResult == MessageBoxResult.OK)
                this.View.Close();
        }
    }
}
