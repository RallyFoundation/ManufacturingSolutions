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
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Contract;

namespace DIS.Services.DataPolling {
    /// <summary>
    /// OEMDataPollingService class for Service code behind
    /// </summary>
    public partial class DataPollingService : ServiceBase {
        #region Private fields

        private IUserProxy userProxy;
        private IConfigProxy configProxy;
        private IHeadQuarterProxy hqProxy;

        private readonly int searchInterval;
        private readonly int defaultInterval;

        private Timer pulseTimer = null;
        private Timer intervalTimer = null;
        private Timer fulfillmentTimer = null;
        private Timer reportTimer = null;
        private Timer miscTimer = null;
        private Timer searchTimer = null;

        private object lockingObject = new object();

        private bool isFulfilling = false;
        private bool isReporting = false;
        private bool isMiscRunning = false;
        private bool isSearching = false;

        #endregion Private fields

        #region DIS Cloud fields

        private Dictionary<string, DISTimer> pulseTimers;
        private Dictionary<string, DISTimer> internalTimers;
        private Dictionary<string, DISTimer> fulfillmentTimers;
        private Dictionary<string, DISTimer> reportTimers;
        private Dictionary<string, DISTimer> miscTimers;
        private Dictionary<string, DISTimer> searchTimers;

        #endregion

        public IUserProxy UserProxy {
            get {
                if (userProxy == null)
                    userProxy = new UserProxy();
                return userProxy;
            }
        }

        public IConfigProxy ConfigProxy {
            get {
                if (configProxy == null)
                    configProxy = new ConfigProxy(UserProxy.GetFirstManager());
                return configProxy;
            }
        }

        public IHeadQuarterProxy HqProxy {
            get {
                if (hqProxy == null)
                    hqProxy = new HeadQuarterProxy();
                return hqProxy;
            }
        }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DataPollingService()
        {
            InitializeComponent();
            AutoLog = true;

            defaultInterval = Math.Max(int.Parse(ConfigurationManager.AppSettings["interval"]), 60000);
            searchInterval = Math.Max(int.Parse(ConfigurationManager.AppSettings["searchInterval"]), 60000);
        }

        #endregion Consturctor

        #region Overriden Methods

        /// <summary>
        /// OnStart
        /// </summary>
        /// <param name="args">string[]</param>
        protected override void OnStart(string[] args) 
        {
            bool isCloudConfigSyncSuccessful = false;

            try
            {
                isCloudConfigSyncSuccessful = this.syncCloudConfigurations();
            }
            catch (Exception ex)
            {
                isCloudConfigSyncSuccessful = false;

                //At this time, there is not a specific customer context to log the errors occurred, so a global log is used - Rally Dec. 19, 2014
                TracingHelper.Trace(new object[] { ex }, "DataPollingServiceTraceSource");
            }

            try
            {
                if (!isCloudConfigSyncSuccessful)
                {
                    try
                    {
                        this.intervalTimer = new Timer();
                        this.intervalTimer.Enabled = true;
                        this.intervalTimer.Interval = defaultInterval;
                        this.intervalTimer.Elapsed += new ElapsedEventHandler(IntervalTimerElapsed);

                        this.fulfillmentTimer = new Timer();
                        this.fulfillmentTimer.Enabled = true;
                        this.fulfillmentTimer.Interval = defaultInterval;
                        this.fulfillmentTimer.Elapsed += new ElapsedEventHandler(FulfillmentTimerElapsed);

                        this.reportTimer = new Timer();
                        this.reportTimer.Enabled = true;
                        this.reportTimer.Interval = defaultInterval;
                        this.reportTimer.Elapsed += new ElapsedEventHandler(ReportTimerElapsed);

                        this.miscTimer = new Timer();
                        this.miscTimer.Enabled = true;
                        this.miscTimer.Interval = defaultInterval;
                        this.miscTimer.Elapsed += new ElapsedEventHandler(MiscTimerElapsed);

                        this.pulseTimer = new Timer();
                        this.pulseTimer.Enabled = true;
                        this.pulseTimer.Interval = Constants.PulseInterval;
                        this.pulseTimer.Elapsed += new ElapsedEventHandler(PulseTimerElapsed);

                        this.searchTimer = new Timer();
                        this.searchTimer.Enabled = true;
                        this.searchTimer.Interval = searchInterval;
                        this.searchTimer.Elapsed += new ElapsedEventHandler(SearchTimerElapsed);
                    }
                    catch (Exception ex)
                    {
                        //ExceptionHandler.HandleException(ex);
                        //At this time, there is not a specific customer context to log the errors occurred, so a global log is used - Rally Sept. 23, 2014
                        TracingHelper.Trace(new object[] { ex }, "DataPollingServiceTraceSource");
                    }
                }
                else
                {
                    foreach (var pulseTimer in this.pulseTimers.Values)
                    {
                        pulseTimer.Enabled = true;
                        pulseTimer.Interval = Constants.PulseInterval;
                        pulseTimer.Elapsed += pulseTimer_Elapsed;
                    }

                    foreach (var intervalTimer in this.internalTimers.Values)
                    {
                        intervalTimer.Enabled = true;
                        intervalTimer.Interval = this.defaultInterval;
                        intervalTimer.Elapsed += intervalTimer_Elapsed;
                    }

                    foreach (var fulfillmentTimer in this.fulfillmentTimers.Values)
                    {
                        fulfillmentTimer.Enabled = true;
                        fulfillmentTimer.Interval = this.defaultInterval;
                        fulfillmentTimer.Elapsed += fulfillmentTimer_Elapsed;
                    }

                    foreach (var reportTimer in this.reportTimers.Values)
                    {
                        reportTimer.Enabled = true;
                        reportTimer.Interval = this.defaultInterval;
                        reportTimer.Elapsed += reportTimer_Elapsed;
                    }

                    foreach (var miscTimer in this.miscTimers.Values)
                    {
                        miscTimer.Enabled = true;
                        miscTimer.Interval = this.defaultInterval;
                        miscTimer.Elapsed += miscTimer_Elapsed;
                    }

                    foreach (var searchTimer in this.searchTimers.Values)
                    {
                        searchTimer.Enabled = true;
                        searchTimer.Interval = this.searchInterval;
                        searchTimer.Elapsed += searchTimer_Elapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                //At this time, there is not a specific customer context to log the errors occurred, so a global log is used - Rally Sept. 11, 2014
                TracingHelper.Trace(new object[] { ex }, "DataPollingServiceTraceSource");
            }
        }

        void searchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.lockingObject)
            {
                DISTimer timer = sender as DISTimer;

                if (timer != null)
                {
                    this.doTimerBusinessActions(timer, (p) => { p.SearchSubmitted(); });
                }
            } 
        }

        void miscTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.lockingObject)
            {
                DISTimer timer = sender as DISTimer;

                if (timer != null)
                {
                    this.doTimerBusinessActions(timer, (p) => { p.DoRecurringTasks(); });
                }
            }
        }

        void reportTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.lockingObject)
            {
                DISTimer timer = sender as DISTimer;

                if (timer != null)
                {
                    this.doTimerBusinessActions(timer, (p) => { p.AutomaticReportKeys(); });
                }
            }
        }

        void fulfillmentTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.lockingObject)
            {
                DISTimer timer = sender as DISTimer;

                if (timer != null)
                {
                    this.doTimerBusinessActions(timer, (p) => { p.AutomaticGetKeys(); });
                }
            }
        }

        void intervalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.lockingObject)
            {
                DISTimer timer = sender as DISTimer;

                if (timer != null)
                {
                    try
                    {
                        int fulfillmentInterval = this.GetTimerInterval(timer.ConfigProxy.GetFulfillmentInterval());

                        if (timer.Interval != fulfillmentInterval)
                        {
                            timer.Interval = fulfillmentInterval;
                        }

                        int reportInterval = GetTimerInterval(timer.ConfigProxy.GetReportInterval());

                        if (timer.Interval != reportInterval)
                        {
                            timer.Interval = reportInterval;
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageLogger.ResetLoggingConfiguration("KeyStoreContext", timer.DBConnectionString);
                        ExceptionHandler.HandleException(ex, timer.DBConnectionString);
                    }
                }
            }
        }

        void pulseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.lockingObject)
            {
                DISTimer timer = sender as DISTimer;

                if (timer != null)
                {
                    try
                    {
                        timer.ConfigProxy.DataPollingServiceReport();
                    }
                    catch (Exception ex)
                    {
                        //MessageLogger.ResetLoggingConfiguration("KeyStoreContext", timer.DBConnectionString);
                        ExceptionHandler.HandleException(ex, timer.DBConnectionString);
                    }

                }
            }
        }

        /// <summary>
        /// OnShutdown
        /// </summary>
        protected override void OnShutdown() 
        {
            if (this.internalTimers != null)
            {
                foreach (var timer in this.internalTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.intervalTimer_Elapsed);
                    timer.Dispose();
                }
            }
            else
            {
                this.intervalTimer.Elapsed -= new ElapsedEventHandler(this.IntervalTimerElapsed);
                this.intervalTimer.Dispose();
            }

            if (this.fulfillmentTimers != null)
            {
                foreach (var timer in this.fulfillmentTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.fulfillmentTimer_Elapsed);
                    timer.Dispose();
                }
            }
            else
            {
                this.fulfillmentTimer.Elapsed -= new ElapsedEventHandler(this.FulfillmentTimerElapsed);
                this.fulfillmentTimer.Dispose();
            }

            if (this.reportTimers != null)
            {
                foreach (var timer in this.reportTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.reportTimer_Elapsed);
                    timer.Dispose();
                }
            }
            else 
            { 
                this.reportTimer.Elapsed -= new ElapsedEventHandler(this.ReportTimerElapsed);
                this.reportTimer.Dispose();
            }

            if (this.miscTimers != null)
            {
                foreach (var timer in this.miscTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.miscTimer_Elapsed);
                    timer.Dispose();
                }
            }
            else
            {
                this.miscTimer.Elapsed -= new ElapsedEventHandler(this.MiscTimerElapsed);
                this.miscTimer.Dispose();
            }

            if (this.pulseTimers != null)
            {
                foreach (var timer in this.pulseTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.pulseTimer_Elapsed);
                    timer.Dispose();
                }
            }
            else
            {
                this.pulseTimer.Elapsed -= new ElapsedEventHandler(this.PulseTimerElapsed);
                this.pulseTimer.Dispose();
            }

            if (this.searchTimers != null)
            {
                foreach (var timer in this.searchTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.searchTimer_Elapsed);
                    timer.Dispose();
                }
            }
            else
            {
                this.searchTimer.Elapsed -= new ElapsedEventHandler(this.SearchTimerElapsed);
                this.searchTimer.Dispose();
            }
            
            base.OnShutdown();
        }

        /// <summary>
        /// OnStop
        /// </summary>
        protected override void OnStop() 
        {
            if (this.internalTimers != null)
            {
                foreach (var timer in this.internalTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.intervalTimer_Elapsed);
                    timer.Enabled = false;
                }
            }
            else
            {
                this.intervalTimer.Elapsed -= new ElapsedEventHandler(this.IntervalTimerElapsed);
                this.intervalTimer.Enabled = false;
            }

            if (this.fulfillmentTimers != null)
            {
                foreach (var timer in this.fulfillmentTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.fulfillmentTimer_Elapsed);
                    timer.Enabled = false;
                }
            }
            else
            {
                this.fulfillmentTimer.Elapsed -= new ElapsedEventHandler(this.FulfillmentTimerElapsed);
                this.fulfillmentTimer.Enabled = false;
            }

            if (this.reportTimers != null)
            {
                foreach (var timer in this.reportTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.reportTimer_Elapsed);
                    timer.Enabled = false;
                }
            }
            else
            {
                this.reportTimer.Elapsed -= new ElapsedEventHandler(this.ReportTimerElapsed);
                this.reportTimer.Enabled = false;
            }

            if (this.miscTimers != null)
            {
                foreach (var timer in this.miscTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.miscTimer_Elapsed);
                    timer.Enabled = false;
                }
            }
            else
            {
                this.miscTimer.Elapsed -= new ElapsedEventHandler(this.MiscTimerElapsed);
                this.miscTimer.Enabled = false;
            }

            if (this.pulseTimers != null)
            {
                foreach (var timer in this.pulseTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.pulseTimer_Elapsed);
                    timer.Enabled = false;
                }
            }
            else
            {
                this.pulseTimer.Elapsed -= new ElapsedEventHandler(this.PulseTimerElapsed);
                this.pulseTimer.Enabled = false;
            }

            if (this.searchTimers != null)
            {
                foreach (var timer in this.searchTimers.Values)
                {
                    timer.Elapsed -= new ElapsedEventHandler(this.searchTimer_Elapsed);
                    timer.Enabled = false;
                }
            }
            else
            {
                this.searchTimer.Elapsed -= new ElapsedEventHandler(this.SearchTimerElapsed);
                this.searchTimer.Enabled = false;
            }

            base.OnStop();
        }

        #endregion Overriden Methods

        #region Timer Events

        private void IntervalTimerElapsed(object sender, ElapsedEventArgs e) {
            try {
                int fulfillmentInterval = GetTimerInterval(ConfigProxy.GetFulfillmentInterval());
                if (fulfillmentTimer.Interval != fulfillmentInterval)
                    fulfillmentTimer.Interval = fulfillmentInterval;

                int reportInterval = GetTimerInterval(ConfigProxy.GetReportInterval());
                if (reportTimer.Interval != reportInterval)
                    reportTimer.Interval = reportInterval;
            }
            catch (Exception ex) {
                ExceptionHandler.HandleException(ex, ConfigurationManager.ConnectionStrings["KeyStoreContext"].ConnectionString);
            }
        }

        private void PulseTimerElapsed(object sender, ElapsedEventArgs e) {
            try {
                ConfigProxy.DataPollingServiceReport();
            }
            catch {
                //do nothing
            }
        }

        /// <summary>
        /// Invoked when the time is elapsed for the OrderFulfillment Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FulfillmentTimerElapsed(object sender, ElapsedEventArgs e) {
            if (!isFulfilling) {
                isFulfilling = true;
                LoopForHeadQuarters((p) => {
                    p.AutomaticGetKeys();
                });
                isFulfilling = false;
            }
        }

        /// <summary>
        /// Invoked when the time is elapsed for the Getproductkeys Timer
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ElapsedEventArgs</param>
        private void ReportTimerElapsed(object sender, ElapsedEventArgs e) {
            if (!isReporting) {
                isReporting = true;
                LoopForHeadQuarters((p) => {
                    p.AutomaticReportKeys();
                });
                isReporting = false;
            }
        }

        private void MiscTimerElapsed(object sender, ElapsedEventArgs e) {
            if (!isMiscRunning) {
                isMiscRunning = true;
                LoopForHeadQuarters((p) => {
                    p.DoRecurringTasks();
                });
                isMiscRunning = false;
            }
        }

        private void SearchTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!isSearching)
            {
                isSearching = true;
                LoopForHeadQuarters(p =>
                {
                    p.SearchSubmitted();
                });
                isSearching = false;
            }
        }

        private void LoopForHeadQuarters(Action<IKeyProxy> action) {
            try {
                List<HeadQuarter> hqs = HqProxy.GetHeadQuarters();
                if (hqs == null || hqs.Count == 0)
                    action(new KeyProxy(UserProxy.GetFirstManager(), null));
                else
                    foreach (HeadQuarter hq in hqs) {
                        action(new KeyProxy(UserProxy.GetFirstManager(), hq.HeadQuarterId));
                    }
            }
            catch (Exception ex) {
                ExceptionHandler.HandleException(ex, ConfigurationManager.ConnectionStrings["KeyStoreContext"].ConnectionString);
            }
        }

        #endregion Timer Events

        private int GetTimerInterval(double intervalInMinute) 
        {
            return (int)(intervalInMinute * 60 * 1000);
        }

        private void doTimerBusinessActions(DISTimer timer, Action<IKeyProxy> action)
        {
            try
            {
                List<HeadQuarter> hqs = timer.HeadQuaterProxy.GetHeadQuarters();

                if (hqs == null || hqs.Count == 0)
                {
                    action(new KeyProxy(timer.UserProxy.GetFirstManager(), null, timer.DBConnectionString, timer.ConfigurationID, timer.CustomerID));
                }
                else 
                { 
                    foreach (HeadQuarter hq in hqs)
                    {
                       action(new KeyProxy(timer.UserProxy.GetFirstManager(), hq.HeadQuarterId, timer.DBConnectionString, timer.ConfigurationID, timer.CustomerID));
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageLogger.ResetLoggingConfiguration("KeyStoreContext", timer.DBConnectionString);
                ExceptionHandler.HandleException(ex, timer.DBConnectionString);
            }
        }

        private bool syncCloudConfigurations()
        {
            IManager manager = new Manager(DISConfigurationCloud.Client.ModuleConfiguration.IsTracingEnabled, DISConfigurationCloud.Client.ModuleConfiguration.TraceSourceName);

            Customer[] customers = manager.GetCustomers();

            if (customers != null)
            {
                this.pulseTimers = new Dictionary<string, DISTimer>();
                this.internalTimers = new Dictionary<string, DISTimer>();
                this.fulfillmentTimers = new Dictionary<string, DISTimer>();
                this.reportTimers = new Dictionary<string, DISTimer>();
                this.miscTimers = new Dictionary<string, DISTimer>();
                this.searchTimers = new Dictionary<string, DISTimer>();

                string ignoredCustomers = System.Configuration.ConfigurationManager.AppSettings.Get("IgnoredCustomers");

                string[] ignoredCustomerArray = String.IsNullOrEmpty(ignoredCustomers) ? null : ignoredCustomers.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                List<string> ignoredCustomerList = ignoredCustomerArray == null ? new List<string>() : new List<string>(ignoredCustomerArray);

                foreach (var customer in customers)
                {
                    if ((ignoredCustomerList == null)||((!ignoredCustomerList.Contains(customer.ID))&&(customer.Configurations != null)))
                    {
                        foreach (var configuration in customer.Configurations)
                        {
                            if (Data.DataContract.Constants.InstallType.ToString().ToLower() == configuration.ConfigurationType.ToString().ToLower())
                            {
                                if (!this.pulseTimers.ContainsKey(customer.ID))
                                {
                                    this.pulseTimers.Add(customer.ID, new DISTimer() 
                                    { 
                                        CustomerID = customer.ID, 
                                        ConfigurationID = configuration.ID, 
                                        DBConnectionString = configuration.DbConnectionString
                                    });
                                }

                                if (!this.internalTimers.ContainsKey(customer.ID))
                                {
                                    this.internalTimers.Add(customer.ID, new DISTimer() 
                                    { 
                                        CustomerID = customer.ID, 
                                        ConfigurationID = configuration.ID, 
                                        DBConnectionString = configuration.DbConnectionString 
                                    });
                                }

                                if (!this.fulfillmentTimers.ContainsKey(customer.ID))
                                {
                                    this.fulfillmentTimers.Add(customer.ID, new DISTimer() 
                                    { 
                                        CustomerID = customer.ID, 
                                        ConfigurationID = configuration.ID, 
                                        DBConnectionString = configuration.DbConnectionString 
                                    });
                                }

                                if (!this.reportTimers.ContainsKey(customer.ID))
                                {
                                    this.reportTimers.Add(customer.ID, new DISTimer() 
                                    { 
                                        CustomerID = customer.ID, 
                                        ConfigurationID = configuration.ID, 
                                        DBConnectionString = configuration.DbConnectionString 
                                    });
                                }

                                if (!this.miscTimers.ContainsKey(customer.ID))
                                {
                                    this.miscTimers.Add(customer.ID, new DISTimer() 
                                    { 
                                        CustomerID = customer.ID, 
                                        ConfigurationID = configuration.ID, 
                                        DBConnectionString = configuration.DbConnectionString 
                                    });
                                }

                                if (!this.searchTimers.ContainsKey(customer.ID))
                                {
                                    this.searchTimers.Add(customer.ID, new DISTimer() 
                                    { 
                                        CustomerID = customer.ID, 
                                        ConfigurationID = configuration.ID, 
                                        DBConnectionString = configuration.DbConnectionString 
                                    });
                                }
                            }
                        }
                    }
                }

                return true;
            }

            return false;
        }

    }
}
