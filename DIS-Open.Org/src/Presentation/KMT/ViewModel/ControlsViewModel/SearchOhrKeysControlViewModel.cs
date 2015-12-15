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
using System.Windows;
using System.Windows.Input;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel.ControlsViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchOhrKeysControlViewModel : ViewModelBase
    {
        private DelegateCommand searchCommand;

        private string searchMSNumber;
        private string searchPONumber;
        private string searchOEMNumber;
        private DateTime? beginMsFulfiledDate;
        private DateTime? endMsFulfiledDate;
        private bool isSearchBtnDefault;

        private string productKey;
        private string productKeyID;
        private string productKeyIDFrom;
        private string productKeyIDTo;

        /// <summary>
        /// 
        /// </summary>
        private string selectedKeyState = null;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler SearchKeys;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler GotSearchFouce;

        /// <summary>
        /// Search content to search part data
        /// </summary>
        public string SearchMSNumber
        {
            get
            {
                return searchMSNumber;
            }
            set
            {
                searchMSNumber = value.Trim();
                RaisePropertyChanged("SearchOEMNumber");
            }
        }
        /// <summary>
        /// Search content to search part data
        /// </summary>
        public string SearchOEMNumber
        {
            get
            {
                return searchOEMNumber;
            }
            set
            {
                searchOEMNumber = value.Trim();
                RaisePropertyChanged("SearchOEMNumber");
            }
        }
        /// <summary>
        /// Search content to search part data
        /// </summary>
        public DateTime? BeginMsFulfiledDate
        {
            get
            {
                return beginMsFulfiledDate;
            }
            set
            {
                beginMsFulfiledDate = value;
                RaisePropertyChanged("BeginMsFulfiledDate");
            }
        }

        /// <summary>
        /// Search content to search part data
        /// </summary>
        public DateTime? EndMsFulfiledDate
        {
            get
            {
                return endMsFulfiledDate;
            }
            set
            {
                endMsFulfiledDate = value;
                RaisePropertyChanged("EndMsFulfiledDate");
            }
        }

        /// <summary>
        /// Search content to search part data
        /// </summary>
        public string SearchPONumber
        {
            get
            {
                return searchPONumber;
            }
            set
            {
                searchPONumber = value.Trim();
                RaisePropertyChanged("SearchPONumber");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSearchBtnDefault
        {
            get
            {
                return isSearchBtnDefault;
            }
            set
            {
                isSearchBtnDefault = value;
                RaisePropertyChanged("IsSearchBtnDefault");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<string> KeyStates { get; private set; }


        public SearchOhrKeysControlViewModel()
        {
            this.KeyStates = new ObservableCollection<string> { MergedResources.Common_All, KeyState.ActivationEnabled.ToString(), KeyState.ActivationDenied.ToString() };
            this.SelectedKeyState = this.KeyStates.First();
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedKeyState
        {
            get { return this.selectedKeyState; }
            set
            {
                selectedKeyState = value;
                RaisePropertyChanged("SelectedKeyState");
            }

        }

        public string ProductKey
        {
            get
            {
                return productKey;
            }
            set
            {
                productKey = value.Trim();
                RaisePropertyChanged("ProductKey");
            }
        }

        public string ProductKeyID
        {
            get
            {
                return productKeyID;
            }
            set
            {
                productKeyID = value.Trim();
                RaisePropertyChanged("ProductKeyID");
            }
        }

        public string ProductKeyIDFrom
        {
            get
            {
                return productKeyIDFrom;
            }
            set
            {

                long temp = -1;
                if (long.TryParse(value, out temp) && temp > 0)
                    productKeyIDFrom = value.Trim();
                else
                    productKeyIDFrom = string.Empty;
                RaisePropertyChanged("ProductKeyIDFrom");
            }
        }

        public string ProductKeyIDTo
        {
            get
            {
                return productKeyIDTo;
            }
            set
            {
                long temp = -1;
                if (long.TryParse(value, out temp) && temp > 0)
                    productKeyIDTo = value.Trim();
                else
                    productKeyIDTo = string.Empty;
                RaisePropertyChanged("ProductKeyIDTo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new DelegateCommand(OnSearchKeys);
                }
                return searchCommand;
            }
        }

        private void OnSearchKeys()
        {
            if (SearchKeys != null)
                SearchKeys(this, new EventArgs());
        }

        internal void SetSearchFocus(object sender, RoutedEventArgs e)
        {
            if (GotSearchFouce != null)
                GotSearchFouce(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        public KeySearchCriteria FillSearchCriteria()
        {
            KeySearchCriteria KeySearchCriteria = new KeySearchCriteria();
            KeySearchCriteria.PageSize = KeySearchCriteria.DefaultPageSize;
            KeySearchCriteria.MsPartNumber = SearchMSNumber;
            KeySearchCriteria.OemPartNumber = SearchOEMNumber;
            KeySearchCriteria.OemPoNumber = SearchPONumber;
            KeySearchCriteria.DateFrom = BeginMsFulfiledDate;
            KeySearchCriteria.DateTo = EndMsFulfiledDate;
            KeySearchCriteria.KeyType = KeyType.Standard;
            //if (this.SelectedKeyState != null && this.SelectedKeyState != MergedResources.Common_All)
            //    KeySearchCriteria.KeyStates = new List<KeyState>() { (KeyState)Enum.Parse(typeof(KeyState), this.SelectedKeyState, true) };

            KeySearchCriteria.ProductKey = ProductKey;
            KeySearchCriteria.ProductKeyID = ProductKeyID;
            if (!string.IsNullOrEmpty(ProductKeyIDFrom))
                KeySearchCriteria.ProductKeyIDFrom = long.Parse(ProductKeyIDFrom);
            if (!string.IsNullOrEmpty(ProductKeyIDTo))
                KeySearchCriteria.ProductKeyIDTo = long.Parse(ProductKeyIDTo);

            return KeySearchCriteria;
        }
    }
}
