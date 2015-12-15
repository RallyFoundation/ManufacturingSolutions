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
using System.Text;
using System.Timers;
using System.Windows;
using DIS.Business.Proxy;
using DIS.Presentation.KMT.Views.Key;
using DIS.Common.Utility;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        private ObservableCollection<Notification> notifications;

        /// <summary>
        /// 
        /// </summary>
        public NotificationEventArgs(ObservableCollection<Notification> notifications)
        {
            this.notifications = notifications;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Push(Notification notification)
        {
            notification.SetViewCommand();
            Notification old = notifications.FirstOrDefault(n => n.Category == notification.Category);
            if (old != null)
                notifications.Remove(old);
            notifications.Add(notification);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Pop(NotificationCategory category)
        {
            Notification old = notifications.FirstOrDefault(n => n.Category == category);
            if (old != null)
                notifications.Remove(old);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class NotificationViewModel : ViewModelBase, IDisposable
    {
        private Timer timer;
        private Timer systemCheckTimer;

        /// <summary>
        /// 
        /// </summary>
        public bool ShouldNotificationNotPopup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Notification> Notifications { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public event NotificationEventHandler Check;

        /// <summary>
        /// 
        /// </summary>
        public event NotificationEventHandler SystemCheck;

        /// <summary>
        /// 
        /// </summary>
        public NotificationViewModel()
        {
            Notifications = new ObservableCollection<Notification>();

            timer = new Timer(KmtConstants.NotificationCheckInterval);
            timer.AutoReset = false;
            timer.Elapsed += OnCheckNotification;
            timer.Start();

            systemCheckTimer = new Timer(60000);
            systemCheckTimer.AutoReset = false;
            systemCheckTimer.Elapsed += OnCheckNotification;
            systemCheckTimer.Start();

            new System.Threading.Thread(() =>
            {
                System.Threading.Thread.Sleep(5000);
                OnCheckNotification(timer, null);
            }).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnCheckNotification(object sender, ElapsedEventArgs e)
        {
            Timer tSender = (Timer)sender;
            NotificationEventArgs ea = new NotificationEventArgs(Notifications);
      
            if (tSender == timer && Check != null)
                Check(this, ea);
            else if (tSender == systemCheckTimer && SystemCheck != null)
                SystemCheck(this, ea);

            tSender.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            timer.Dispose();
            systemCheckTimer.Dispose();
        }
    }
}
