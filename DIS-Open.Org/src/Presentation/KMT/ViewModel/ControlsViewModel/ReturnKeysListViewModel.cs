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
using System.Linq;
using System.Windows;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.ViewModel.ControlsViewModel
{
    public class ReturnKeysListViewModel : ViewModelBase
    {

        #region Private Members

        private ObservableCollection<ReturnKeyModel> returnKeys = null;
        private string oemRmaNumber = string.Empty;
        private bool returnNoCredit = false;
        private DateTime? oemRmaDate = null;

        #endregion

        #region Public Properties

        public ObservableCollection<ReturnKeyModel> ReturnKeys
        {
            get { return returnKeys; }
            set
            {
                returnKeys = value;
                RaisePropertyChanged("ReturnKeys");
            }
        }

        public string OemRmaNumber
        {
            get { return oemRmaNumber; }
            set
            {
                oemRmaNumber = value;
                RaisePropertyChanged("OemRmaNumber");
            }
        }

        public DateTime? OemRmaDate
        {
            get { return oemRmaDate; }
            set
            {
                if (oemRmaDate == null)
                    oemRmaDate = DateTime.Now;
                else
                    oemRmaDate = value;
                RaisePropertyChanged("OemRmaDate");
            }
        }

        public bool IsAllChecked
        {
            get
            {
                if (ReturnKeys == null || ReturnKeys.Count <= 0)
                    return false;
                else
                    return ReturnKeys.All(k => k.ReturnReportKey.IsSelected);
            }
            set
            {
                if (ReturnKeys != null && ReturnKeys.Count > 0)
                {
                    foreach (var key in ReturnKeys)
                    {
                        key.ReturnReportKey.IsSelected = value;
                    }
                    RaisePropertyChanged("IsAllChecked");
                }
            }
        }

        public bool ReturnNoCredit
        {
            get { return returnNoCredit; }
            set
            {
                returnNoCredit = value;
                RaisePropertyChanged("ReturnNoCredit");
            }
        }

        public bool ValidateOemRmaNumberTxt()
        {
            if (string.IsNullOrEmpty(OemRmaNumber))
            {
                MessageBox.Show(ResourcesOfR6.ReturnKeysView_NoOemRmaNumberError, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (System.Text.UTF8Encoding.UTF8.GetBytes(OemRmaNumber).Length > 35)
            {
                MessageBox.Show(ResourcesOfR6.ReturnKeysView_OemRmaNumberError, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (this.OemRmaDate == null)
            {
                System.Windows.MessageBox.Show(ResourcesOfR6.Export_ReturnRMADateMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (this.OemRmaDate.Value > DateTime.Now.AddYears(5) || this.OemRmaDate.Value < DateTime.Now.AddYears(-5))
            {
                System.Windows.MessageBox.Show(ResourcesOfR6.Export_ReturnRMADateInvalid, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool ValidateReturnNoCredit()
        {
            if (ReturnNoCredit)
                return MessageBox.Show(ResourcesOfR6.ReturnKeysView_ReturnNoCreditWaring, MergedResources.Common_Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
            return true;
        }

        public bool ValidateKeyStateNotice()
        {
            if (this.ReturnKeys.Where(k => k.ReturnReportKey.IsSelected).Any(k => k.ReturnReportKey.keyInfo.KeyState == KeyState.ActivationEnabled ||
                                                      k.ReturnReportKey.keyInfo.KeyState == KeyState.ActivationDenied ||
                                                      k.ReturnReportKey.keyInfo.KeyState == KeyState.Bound))
                return MessageBox.Show(ResourcesOfR6.ReturnKeysView_ReturnNotice, MergedResources.Common_Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
            return true;
        }

        //add and remove return types 
        public void AddkeyReturnTypeByCredit()
        {
            if (this.ReturnNoCredit)
            {
                this.ReturnKeys.ToList().ForEach(k =>
                {
                    if (!k.ReturnRequestTypes.Contains(ResourcesOfR6.ReturnKeysView_ZOEDescription))
                        k.ReturnRequestTypes.Add(ResourcesOfR6.ReturnKeysView_ZOEDescription);
                    if (!k.ReturnRequestTypes.Contains(ResourcesOfR6.ReturnKeysView_ZOFDescription))
                        k.ReturnRequestTypes.Add(ResourcesOfR6.ReturnKeysView_ZOFDescription);
                });
            }
            else
            {
                this.ReturnKeys.ToList().ForEach(k =>
                {
                    if (k.ReturnRequestTypes.Contains(ResourcesOfR6.ReturnKeysView_ZOEDescription))
                        k.ReturnRequestTypes.Remove(ResourcesOfR6.ReturnKeysView_ZOEDescription);
                    if (k.ReturnRequestTypes.Contains(ResourcesOfR6.ReturnKeysView_ZOFDescription))
                        k.ReturnRequestTypes.Remove(ResourcesOfR6.ReturnKeysView_ZOFDescription);
                });
            }
        }

        #endregion
    }
}
