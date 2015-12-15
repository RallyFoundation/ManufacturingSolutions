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
using System.Linq;
using System.Text;
using DIS.Data.DataContract;
using System.ComponentModel;

namespace DIS.Presentation.KMT.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyInfoModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public KeyInfo keyInfo { get; set; }
        
        /// <summary>
        /// selection for KMT.UI
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// Only Item Selection
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }

        public string ZFRM_FACTOR_CL1
        {
            get 
            {
                return keyInfo.ZFRM_FACTOR_CL1; 
            }
            set
            {
                keyInfo.ZFRM_FACTOR_CL1 = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ZFRM_FACTOR_CL1"));
                if (ZfrmFactorCl1PropertyChanged != null)
                    ZfrmFactorCl1PropertyChanged(this, new ZfrmFactorCl1PropertyChangedEventArgs(value));
            }
        }

        public string ZFRM_FACTOR_CL2
        {
            get { return keyInfo.ZFRM_FACTOR_CL2; }
            set
            {
                keyInfo.ZFRM_FACTOR_CL2 = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ZFRM_FACTOR_CL2"));
            }
        }
        public string ZTOUCH_SCREEN
        {
            get { return keyInfo.ZTOUCH_SCREEN; }
            set
            {
                keyInfo.ZTOUCH_SCREEN = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ZTOUCH_SCREEN"));
            }
        }

        public string ZSCREEN_SIZE
        {
            get { return keyInfo.ZSCREEN_SIZE; }
            set
            {
                keyInfo.ZSCREEN_SIZE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ZSCREEN_SIZE"));
            }
        }

        public string ZPC_MODEL_SKU
        {
            get { return keyInfo.ZPC_MODEL_SKU; }
            set
            {
                keyInfo.ZPC_MODEL_SKU = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ZPC_MODEL_SKU"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ZfrmFactorCl1PropertyChangedEventHandle ZfrmFactorCl1PropertyChanged;

    }
}
