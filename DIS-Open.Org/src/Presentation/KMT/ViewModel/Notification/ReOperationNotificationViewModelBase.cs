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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReOperationNotificationViewModelBase<T> : ViewModelBase
    {
        private ObservableCollection<T> infoes;
        private DelegateCommand copyCommand;
        private string result = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoes"></param>
        protected ReOperationNotificationViewModelBase(List<T> infoes)
        {
            this.infoes = new ObservableCollection<T>(infoes);
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<T> Infoes
        {
            get
            {
                return infoes;
            }
            set
            {
                infoes = value;
                RaisePropertyChanged("Infoes");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if (copyCommand == null)
                    copyCommand = new DelegateCommand(Copy);
                return copyCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual string Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Copy()
        {
            Clipboard.SetText(Result);
            var messageResult = MessageBox.Show(MergedResources.Notification_CopiedToClipboard, MergedResources.Common_Information, MessageBoxButton.OK, MessageBoxImage.Information);
            if (messageResult == MessageBoxResult.OK)
                this.View.Close();
        }
    }
}
