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
    public class SearchControlViewModel : ViewModelBase
    {
        private DelegateCommand searchCommand;

        private string searchMSNumber;
        private string searchPONumber;
        private string searchOEMNumber;
        private DateTime? beginMsFulfiledDate;
        private DateTime? endMsFulfiledDate;
        private DateTime? startOemRMADate;
        private DateTime? endOemRMADate;
        private bool isSearchBtnDefault;
        public ObservableCollection<string> keyTypes;
        private Visibility keyTypesVisibility = Visibility.Visible;

        private string productKey;
        private string productKeyID;
        private string productKeyIDFrom;
        private string productKeyIDTo;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, List<string>> keyStates = null;
        private string selectedKeyState = null;
        private string selectedKeyType = null;

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
        public DateTime? StartOemRMADate
        {
            get
            {
                return startOemRMADate;
            }
            set
            {
                startOemRMADate = value;
                RaisePropertyChanged("StartOemRMADate");
            }
        }

        /// <summary>
        /// Search content to search part data
        /// </summary>
        public DateTime? EndOemRMADate
        {
            get
            {
                return endOemRMADate;
            }
            set
            {
                endOemRMADate = value;
                RaisePropertyChanged("EndOemRMADate");
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
        public ObservableCollection<string> KeyStates
        {
            get
            {
                if (selectedKeyType == null || selectedKeyType == MergedResources.Common_All)
                    return new ObservableCollection<string>(new List<string> { MergedResources.Common_All });
                else
                {
                    if (keyStates != null && keyStates.Count > 0)
                        return new ObservableCollection<string>(keyStates.Where(k => k.Key == selectedKeyType).FirstOrDefault().Value);
                }
                return null;
            }
            set { }
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

        public string SelectedKeyType
        {
            get
            {
                if (selectedKeyType == null)
                    return MergedResources.Common_All;
                else
                    return this.selectedKeyType;
            }
            set
            {
                selectedKeyType = value;
                RaisePropertyChanged("SelectedKeyType");
                if (keyStates != null && keyStates.Count > 0)
                {
                    if (selectedKeyType == null || selectedKeyType == MergedResources.Common_All)
                        KeyStates = new ObservableCollection<string>(new List<string> { MergedResources.Common_All });
                    else
                        KeyStates = new ObservableCollection<string>(keyStates.Where(k => k.Key == selectedKeyType).FirstOrDefault().Value);
                    this.SelectedKeyState = KeyStates.FirstOrDefault();
                    RaisePropertyChanged("KeyStates");
                    RaisePropertyChanged("SelectedKeyState");
                }
            }
        }

        public Visibility KeyTypesVisibility
        {
            get
            {
                return keyTypesVisibility;
            }
            set
            {
                keyTypesVisibility = value;
                RaisePropertyChanged("KeyTypesVisibility");
            }
        }

        public ObservableCollection<string> KeyTypes
        {
            get
            {
                if (keyTypes == null)
                {
                    keyTypes = new ObservableCollection<string>();
                    keyTypes.Add(MergedResources.Common_All);
                    keyTypes.Add(KeyType.Standard.ToString());
                    keyTypes.Add(KeyType.MBR.ToString());
                    keyTypes.Add(KeyType.MAT.ToString());
                }
                return keyTypes;
            }
            set
            {
                keyTypes = value;
                RaisePropertyChanged("keyTypes");
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
            KeySearchCriteria.OemRmaDateFrom = StartOemRMADate;
            KeySearchCriteria.OemRmaDateTo = EndOemRMADate;
            if (SelectedKeyType == MergedResources.Common_All)
                KeySearchCriteria.KeyType = KeyType.All;
            else
                KeySearchCriteria.KeyType = (KeyType)Enum.Parse(typeof(KeyType), SelectedKeyType, true);

            if (this.SelectedKeyState != null && this.SelectedKeyState != MergedResources.Common_All)
                KeySearchCriteria.KeyStates = new List<KeyState>() { (KeyState)Enum.Parse(typeof(KeyState), this.SelectedKeyState, true) };

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
