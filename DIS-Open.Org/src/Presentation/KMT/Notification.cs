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
using System.Windows;
using System.Windows.Input;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.ViewModel;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// 
    /// </summary>
    public class Notification
    {
        public string ButtonContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommand ButtonCommand { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Visibility ButtonVisibility { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message"></param>
        /// <param name="windowType"></param>
        /// <param name="callback"></param>
        /// <param name="windowParameters"></param>
        public Notification(NotificationCategory category, string message, Type windowType, Action callback, params object[] windowParameters)
        {
            Category = category;
            Message = message;
            WindowType = windowType;
            WindowParameters = windowParameters;
            Callback = callback;

            Timestamp = DateTime.Now;
            if (WindowType == null && Callback == null)
                ButtonVisibility = Visibility.Hidden;
            ButtonContent = WindowType == null ? MergedResources.Common_Ok : MergedResources.Common_View;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetViewCommand() {
            ButtonCommand = new DelegateCommand(() => {
                if (WindowType != null)
                {
                    Window window = null;
                    WindowCollection windows = App.Current.Windows;
                    for (int i = 0; i < windows.Count; i++)
                    {
                        if (windows[i].GetType() == WindowType)
                        {
                            window = windows[i];
                            break;
                        }
                    }
                    if (window != null)
                        window.Close();

                    window = (Window)Activator.CreateInstance(WindowType, WindowParameters);
                    window.ShowDialog();
                }
                if (Callback != null)
                    Callback();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public NotificationCategory Category { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Type WindowType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public object[] WindowParameters { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Action Callback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Timestamp { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum NotificationCategory
    {
        /// <summary>
        /// 
        /// </summary>
        ReFulfillment,
        
        /// <summary>
        /// 
        /// </summary>
        ReAcknowledgement,
       
        /// <summary>
        /// 
        /// </summary>
        DuplicatedCbr,
        
        /// <summary>
        /// 
        /// </summary>
        QuantityOutOfRange,

        /// <summary>
        /// 
        /// </summary>
        KeyTypeUnmapped,
        
        /// <summary>
        /// 
        /// </summary>
        OldTimelineExceed,

        SystemError_DatePolling,

        SystemError_DownLevelSystem,

        SystemError_Internal,

        SystemError_MSConnection,

        SystemError_UpLevelSystem,

        SystemError_Unknow,

        SystemError_DataBaseError,

        SystemError_KeyProviderServiceError,

        ReportedKeysFaild,

        ReturnedKeysFaild,

        OhrDataMissed,

        ConfirmedOhrs,

        SystemError_DabaseDiskFull,
    }
}
