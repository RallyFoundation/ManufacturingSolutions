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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.Models
{
    public class ReturnKeyModel : INotifyPropertyChanged
    {
        private string selectReturnRequestType;
        private ObservableCollection<string> returnRequestTypes = null;
        public KeyInfoModel ReturnReportKey { get; set; }

        public string SelectReturnRequestType
        {
            get
            {
                if (string.IsNullOrEmpty(selectReturnRequestType))
                    return ReturnRequestTypes.First();
                else
                    return selectReturnRequestType;
            }
            set
            {
                selectReturnRequestType = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectReturnRequestType"));
            }
        }

        public ObservableCollection<string> ReturnRequestTypes
        {
            get
            {
                if (returnRequestTypes == null)
                {
                    returnRequestTypes = new ObservableCollection<string>();
                    if (ReturnReportKey.keyInfo.KeyState != KeyState.ActivationDenied || ReturnReportKey.keyInfo.KeyState != KeyState.ActivationEnabled)
                        returnRequestTypes.Add(ResourcesOfR6.ReturnKeysView_ZOADescription);
                    returnRequestTypes.Add(ResourcesOfR6.ReturnKeysView_ZOBDescription);
                    returnRequestTypes.Add(ResourcesOfR6.ReturnKeysView_ZOCDescription);
                    returnRequestTypes.Add(ResourcesOfR6.ReturnKeysView_ZODDescription);
                }
                return returnRequestTypes;
            }
            set
            {
                returnRequestTypes = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ReturnRequestTypes"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
