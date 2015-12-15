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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.Validation.Interfaces;
using DIS.Common.Utility;
using System.Threading;
using System.Globalization;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// Base class for View Model classes
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Priviate & Protected member variables

        private readonly Dispatcher uiDispatcher;

        #endregion

        #region Public fields

        /// <summary>
        /// 
        /// </summary>
        public Window View { get; set; }

        /// <summary>
        ///     Occurs when a property value changes.
        /// 
        ///     anprakas: Leveraging from WPF MVVM Toolkit
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors & Dispose

        /// <summary>
        /// 
        /// </summary>
        public ViewModelBase()
            : this(Dispatcher.CurrentDispatcher)
        {
        }

        private ViewModelBase(Dispatcher uiDispatcher)
        {
            this.uiDispatcher = uiDispatcher;
        }

        #endregion

        #region Private & Protected methods

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected  void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Executes the work in background to avoid DIS.Presentation.KMT hangs
        /// </summary>
        protected void WorkInBackground(DoWorkEventHandler work, RunWorkerCompletedEventHandler onCompleted = null)
        {
            //Getting called from DIS.Presentation.KMT, execute on background worker
            if (uiDispatcher != null)
            {
                using (BackgroundWorker worker = new BackgroundWorker())
                {
                    worker.DoWork += (s, e) =>
                    {
                        try
                        {
                            Thread.CurrentThread.CurrentUICulture = KmtConstants.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = KmtConstants.CurrentCulture;
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                        }
                        work(s, e);
                    };
                    if (onCompleted == null)
                        onCompleted = new RunWorkerCompletedEventHandler((s, e) =>
                        {
                            if (e.Result != null)
                            {
                                if (!(e.Result is Message))
                                    throw new ApplicationException("Background worker result is invalid.");

                                Message msg = e.Result as Message;
                                if (msg != null)
                                {
                                   ValidationHelper.ShowMessageBox(msg.Content, msg.Title);
                                }
                            }
                        });
                    worker.RunWorkerCompleted += onCompleted;
                    worker.RunWorkerAsync();
                }
            }
            //Getting called from unit test, execute on main thread
            else
            {
                work.Invoke(null, new DoWorkEventArgs(null));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        protected void Dispatch(Action action)
        {
            //Getting called from DIS.Presentation.KMT, execute on backgroud Dispatcher
            if (uiDispatcher != null)
            {
                uiDispatcher.BeginInvoke(DispatcherPriority.Normal, action);
            }
            //Getting called from unit test, execute on main thread
            else
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Window GetCurrentWindow() 
        {
            Window parent = null;
            for (int i = 0; i < App.Current.Windows.Count; i++)
            {
                Window current = App.Current.Windows[App.Current.Windows.Count - i - 1];
                if (!(current is NotificationWindow))
                {
                    parent = current;
                    break;
                }
            }
            return parent;
        }

        #endregion
    }
}
