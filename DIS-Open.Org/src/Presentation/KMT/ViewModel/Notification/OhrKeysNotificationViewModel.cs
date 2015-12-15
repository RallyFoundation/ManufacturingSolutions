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
using DIS.Presentation.KMT.ViewModel;
using System.Linq;
using DIS.Business.Proxy;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class OhrKeysNotificationViewModel : ViewModelBase
    {
        private bool isDesc = false;
        private string sortColumn = string.Empty;
        private string summary;
        private ObservableCollection<OhrKeyInfo> keys = null;
        private IKeyProxy keyProxy;
        private KeySearchCriteria keySearchCriteria = null;
        private List<OhrKey> ohrKeys;
        private long[] keyIds;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysExpired"></param>
        /// <param name="timeLineDays"></param>
        public OhrKeysNotificationViewModel(List<Ohr> ohrs, IKeyProxy keyProxy)
        {            
            this.keyProxy = keyProxy;
            this.ohrKeys = ohrs.SelectMany(o => o.OhrKeys).ToList();
            SetKeyIds();
            Summary = string.Format(ResourcesOfRTMv1_8.OhrKeysNotificationViewModel_Summary, this.keyIds.Count());
            SearchKeys();
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<OhrKeyInfo> Keys
        {
            get { return keys; }
            set
            {
                keys = value;
                RaisePropertyChanged("Keys");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Summary
        {
            get { return summary; }
            set
            {
                summary = value;
                RaisePropertyChanged("Summary");
            }
        }

        public void SortingByColumn(string sortColumn)
        {
            this.sortColumn = sortColumn;
            isDesc = !isDesc;
            keys = null;
            SearchKeys();
        }

        public void LoadUpKeys()
        {
            SearchKeys(Keys.Count);
        }

        private void SetKeyIds()
        {
            var failedOhrKeys = this.ohrKeys.Where(k => !string.IsNullOrEmpty(k.ReasonCode) && k.ReasonCode != Constants.CBRAckReasonCode.ActivationEnabled);
            this.keyIds = failedOhrKeys.Select(k => k.KeyId).Distinct().ToArray();
        }

        private void SearchKeys(int startIndex = 0)
        {
            if (keySearchCriteria == null)
                keySearchCriteria = FillSearchCriteria();
            keySearchCriteria.StartIndex = startIndex;
            if (!string.IsNullOrEmpty(sortColumn))
            {
                keySearchCriteria.SortBy = sortColumn;
                keySearchCriteria.SortByDesc = isDesc;
            }

            var nextPageKeys = keyProxy.SearchKeys(keySearchCriteria);
            var nextPageOhrKeys = GetOhrKeyInfoes(nextPageKeys);
            if (this.Keys == null)
                this.Keys = new ObservableCollection<OhrKeyInfo>(nextPageOhrKeys);
            else
            {
                nextPageOhrKeys.ForEach(k => this.keys.Add(k));
                RaisePropertyChanged("Keys");
            }

        }

        private KeySearchCriteria FillSearchCriteria()
        {
            KeySearchCriteria result = new KeySearchCriteria();
            result.KeyIds = this.keyIds.ToList();
            result.PageSize = 10;
            return result;
        }

        private List<OhrKeyInfo> GetOhrKeyInfoes(List<KeyInfo> keys)
        {
            List<OhrKeyInfo> result = keys.Select(k => new OhrKeyInfo()
                {
                    KeyId = k.KeyId,
                    KeyInfoId = new KeyDescription(k.KeyId.ToString()),
                    ProductKey = k.ProductKey,
                    ZFRM_FACTOR_CL1 = new KeyDescription(k.ZFRM_FACTOR_CL1),
                    ZFRM_FACTOR_CL2 = new KeyDescription(k.ZFRM_FACTOR_CL2),
                    ZTOUCH_SCREEN = new KeyDescription(k.ZTOUCH_SCREEN),
                    ZSCREEN_SIZE = new KeyDescription(k.ZSCREEN_SIZE),
                    ZPC_MODEL_SKU = new KeyDescription(k.ZPC_MODEL_SKU),
                }).ToList();

            foreach (var key in result)
            {
                List<OhrKey> info = ohrKeys.Where(k => k.KeyId == key.KeyId).ToList();
                foreach (OhrKey ohrKey in info)
                {
                    key.SetValue(ohrKey);                
                }
            }

            return result;
        }
    }
}
