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
using System.Windows;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyExportLogModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public KeyExportLog keyExportLog { get; set; }

        private bool isSelected = false;
       
        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        public string IsEncrypted
        {
            get 
            {
                if (keyExportLog.IsEncrypted)
                    return MergedResources.Common_EncryptedDisplay;
                else
                    return MergedResources.Common_UnEncryptedDisplay;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public Visibility IsEncryptedVisibility
        {
            get
            {
                if (keyExportLog.ExportType == Constants.ExportType.FulfilledKeys.ToString() || keyExportLog.ExportType == Constants.ExportType.ReportKeys.ToString())
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectColor
        {
            get 
            {
                if ((keyExportLog.ExportType == Constants.ExportType.FulfilledKeys.ToString() || keyExportLog.ExportType == Constants.ExportType.ReportKeys.ToString()) && keyExportLog.IsEncrypted==false)
                    return "Gray";
                else
                    return "CornflowerBlue";
            }
        }
    }
}
