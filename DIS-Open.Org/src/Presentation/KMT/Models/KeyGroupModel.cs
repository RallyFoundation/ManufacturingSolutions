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

using System.ComponentModel;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyGroupModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public KeyGroup KeyGroup { get;set;}

        private string quantityString;
        
        /// <summary>
        /// 
        /// </summary>
        public string QuantityString
        {
            get
            {
                if (KeyGroup == null || KeyGroup.Quantity == -1)
                {
                    KeyGroup.Quantity = 0;
                    return "";
                }
                else
                    return quantityString;
            }
            set
            {
                int integer = 0;
                if (KeyGroup != null)
                {
                    if (int.TryParse(value, out integer))
                        KeyGroup.Quantity = integer;
                    else
                        KeyGroup.Quantity = -1;
                }
                quantityString = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnSelected()
        {
            if (string.IsNullOrEmpty(QuantityString))
            {
                KeyGroup.Quantity = (KeyGroup.AvailableKeysCount > Constants.BatchLimit) ? Constants.BatchLimit : KeyGroup.AvailableKeysCount;
                QuantityString = KeyGroup.Quantity.ToString();
            }

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("QuantityString"));
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
