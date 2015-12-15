using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using DIS.Common.Utility;
using System.Collections.Specialized;
using System.Configuration;

namespace EmulatorService
{
    partial class EmulatorService : ServiceBase
    {
        private EmulatorManager emulatorManager = new EmulatorManager();
        private static NameValueCollection runtimeSection=ConfigurationManager.GetSection("TestRuntime") as NameValueCollection;
        private static int defaultInterval = int.Parse(runtimeSection["DefaultInterval"]);
        private static int defaultGenerateAssembleKeysInterval = int.Parse(runtimeSection["DefaultGenerateAssembleKeysInterval"]);
        private Timer intervalTimer = null;
        private Timer intervalGenerateAssembleKeysTimer = null;
        private bool isExecute = false;
        private bool isExecuteGenerateAssembleKeys = false;
        private static bool isAutoGenerateAssembleKey = bool.Parse(runtimeSection["IsAutoGenerateAssembleKey"]);

        public EmulatorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Debugger.Launch();
            try
            {
                intervalTimer = new Timer();
                intervalTimer.Enabled = true;
                intervalTimer.Interval = defaultInterval;
                intervalTimer.Elapsed += new ElapsedEventHandler(IntervalTimerElapsed);

                if (isAutoGenerateAssembleKey)
                {
                    intervalGenerateAssembleKeysTimer = new Timer();
                    intervalGenerateAssembleKeysTimer.Enabled = true;
                    intervalGenerateAssembleKeysTimer.Interval = defaultGenerateAssembleKeysInterval;
                    intervalGenerateAssembleKeysTimer.Elapsed += new ElapsedEventHandler(IntervalAssembleKeysTimerElapsed);
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        protected override void OnShutdown()
        {
            intervalTimer.Elapsed -= new ElapsedEventHandler(IntervalTimerElapsed);
            intervalTimer.Dispose();

            if (isAutoGenerateAssembleKey)
            {
                intervalGenerateAssembleKeysTimer.Elapsed -= new ElapsedEventHandler(IntervalAssembleKeysTimerElapsed);
                intervalGenerateAssembleKeysTimer.Dispose();
            }

            base.OnShutdown();
        }

        protected override void OnStop()
        {
            intervalTimer.Elapsed -= new ElapsedEventHandler(IntervalTimerElapsed);
            intervalTimer.Enabled = false;

            if (isAutoGenerateAssembleKey)
            {
                intervalGenerateAssembleKeysTimer.Elapsed -= new ElapsedEventHandler(IntervalAssembleKeysTimerElapsed);
                intervalGenerateAssembleKeysTimer.Enabled = false;
            }
        }

        private void IntervalTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!isExecute)
            {
                isExecute = true;
                try
                {
                    emulatorManager.ExecuteTest();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
                isExecute = false;
            }
        }

        private void IntervalAssembleKeysTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!isExecuteGenerateAssembleKeys)
            {
                isExecuteGenerateAssembleKeys = true;
                try
                {
                    emulatorManager.GenerateAssembleKeys();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
                isExecuteGenerateAssembleKeys = false;
            }
        }
    }
}
