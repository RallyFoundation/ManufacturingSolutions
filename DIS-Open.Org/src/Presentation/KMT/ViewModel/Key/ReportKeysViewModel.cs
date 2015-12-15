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
using System.Linq;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;
using System.Windows;
using System;
using DIS.Common.Utility;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportKeysViewModel : TemplateViewModelBase
    {
        #region Contructor

        /// <summary>
        /// View Model class fo rReport Keys View
        /// </summary>
        /// <param name="keyProxy"></param>
        public ReportKeysViewModel(IKeyProxy keyProxy,IConfigProxy configProxy)
            : base(keyProxy)
        {
            base.WindowTitle = MergedResources.MainWindow_ReportKeys;
            this.configProxy = configProxy;
            Initialize();
            this.SearchControlVM.KeyTypesVisibility = Visibility.Collapsed;
            isRequireOHRData = configProxy.GetRequireOHRData();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public string SendFailedText
        {
            get
            {
                return sendFailedText;
            }
            set
            {
                sendFailedText = value;
                RaisePropertyChanged("SendFailedText");
            }
        }

        public bool IsReportingSerialNumbers { get; set; }

        #endregion

        #region Private Members

        private string sendFailedText;

        private IConfigProxy configProxy;

        private bool isRequireOHRData;

        private void Initialize()
        {
            base.InitView();
            base.SearchAllKeys();
        }

        #endregion

        #region Override Members

        /// <summary>
        /// Get default keys for report
        /// </summary>
        /// <returns></returns>
        protected override List<KeyInfoModel> SearchKeys()
        {
            List<KeyInfo> searchkeys = base.keyProxy.SearchBoundKeysToReport(KeySearchCriteria);
            if (searchkeys == null && searchkeys.Count <= 0)
                return null;
            else
                return searchkeys.ToKeyInfoModel().ToList();
        }

        /// <summary>
        /// Search key for report by groups
        /// </summary>
        protected override void SearchKeyGroups()
        {
            KeyGroups = new ObservableCollection<KeyGroupModel>(base.keyProxy.SearchBoundKeyGroupsToReport(KeySearchCriteria).ToKeyGroupModel());
        }

        protected override bool ValidateKeyGroups()
        {
            if (!base.ValidateKeyGroups())
                return false;
            if (KmtConstants.IsOemCorp || (KmtConstants.IsTpiCorp && (KmtConstants.CurrentHeadQuarter != null) && (!KmtConstants.CurrentHeadQuarter.IsCentralizedMode)))
            {
                if (keyProxy.SearchBoundKeysToReport(base.KeyGroups.Where(k => k.KeyGroup.Quantity > 0).Select(k => k.KeyGroup).ToList()).Any(k => !k.OemOptionalInfo.HasOHRData))
                {
                  
                    if (isRequireOHRData)
                    {
                        MessageBox.Show(ResourcesOfRTMv1_6.EditOptionalInfo_RequireOHRDataMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        MessageBoxResult confirm = System.Windows.MessageBox.Show(ResourcesOfRTMv1_6.EditOptionalInfo_MissOHRDataMsg, MergedResources.Common_Confirmation, MessageBoxButton.OKCancel);
                        if (confirm != MessageBoxResult.OK)
                            return false;
                    }
                }
            }
            return true;
        }

        private bool CheckCbrTouchScreenValue(KeyInfo key)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(key.ZTOUCH_SCREEN))
            {
                try
                {
                    OemOptionalInfo.ConvertTouchEnum(key.ZTOUCH_SCREEN);
                }
                catch(ApplicationException)
                {
                    result = false;
                }
            }

            return result;
        }

        protected override bool ValidateKeys()
        {
            if (!base.ValidateKeys())
                return false;
            if (KmtConstants.IsOemCorp || (KmtConstants.IsTpiCorp && (KmtConstants.CurrentHeadQuarter != null) && (!KmtConstants.CurrentHeadQuarter.IsCentralizedMode)))
            {
                if (base.Keys.Where(k => k.IsSelected).Any(k => !k.keyInfo.OemOptionalInfo.HasOHRData))
                {
                    if (isRequireOHRData)
                    {
                        MessageBox.Show(ResourcesOfRTMv1_6.EditOptionalInfo_RequireOHRDataMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        MessageBoxResult confirm = System.Windows.MessageBox.Show(ResourcesOfRTMv1_6.EditOptionalInfo_MissOHRDataMsg, MergedResources.Common_Confirmation, MessageBoxButton.OKCancel);
                        if (confirm != MessageBoxResult.OK)
                            return false;
                    }
                }
                if (base.Keys.Where(k => k.IsSelected).Any(k => CheckCbrTouchScreenValue(k.keyInfo)))
                {
                    MessageBox.Show(ResourcesOfRTMv1_8.ReportKeysViewModel_InvalidateTouchScreen, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Execute keys for report
        /// </summary>
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();

            if (this.IsReportingSerialNumbers)
            {
                base.keyProxy.SetKeyReportMessageTranformationXSLT(KmtConstants.XSLT_ULSKeyReportCompitable);
            }
            else
            {
                base.keyProxy.SetKeyReportMessageTranformationXSLT(String.Empty);
            }

            if (base.TabIndex == KmtConstants.FirstTab)
            {
                result = base.keyProxy.SendBoundKeys(base.KeyGroups.Select(k => k.KeyGroup).Where(k => k.Quantity > 0).ToList());
            }
            else
            {
                result = base.keyProxy.SendBoundKeys(base.Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList());
            }

            base.KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);
            base.SummaryText = string.Format(MergedResources.ReportKeysViewModel_ReportSuccess,
                    base.KeyOperationResults.Where(r => !r.Failed).Count(),
                    base.KeyOperationResults.Where(r => r.Failed).Count());
            if (base.KeyOperationResults.Any(r => r.Failed))
            {
                this.SendFailedText = ResourcesOfR6.Common_ReportFailed;
            }
        }

        #endregion
    }
}
