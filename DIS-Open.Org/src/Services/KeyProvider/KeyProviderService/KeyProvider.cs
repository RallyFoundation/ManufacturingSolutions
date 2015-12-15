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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security;
using DIS.Business.Proxy;
using System.Timers;
using DIS.Data.DataContract; 

namespace DIS.Services.KeyProviderService
{ 
    partial class KeyProvider : ServiceBase
    {
        private IUserProxy userProxy;
        private System.Timers.Timer pulseTimer = null;

        public IUserProxy UserProxy
        {
            get
            {
                if (userProxy == null)
                    userProxy = new UserProxy();
                return userProxy;
            }
        }

        public IConfigProxy ConfigProxy
        {
            get
            {
                return new ConfigProxy(UserProxy.GetFirstManager());
            }
        }

        [DllImport("KeyProviderListener.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, BestFitMapping=false, ThrowOnUnmappableChar=true)]
        public static extern long RegisterRpc(string protocolSequence, string endpoint, string serviceClass, string networkAddress, string errorMessageBuffer, int errorMessageBufferLength);

        [DllImport("KeyProviderListener.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, BestFitMapping = false,ThrowOnUnmappableChar = true)]
        public static extern long UnregisterRpc(string errorMessageBuffer, int errorMessageBufferLength);

        // Returned message is has a C style null terminator that must be trimmed.
        protected static string LeftTrim(string errorMessageBuffer)
        {
            string errorMessage;
            int nullIndex = errorMessageBuffer.IndexOf('\0');
            if (nullIndex > -1)
                errorMessage = errorMessageBuffer.Substring(0, nullIndex);
            else
                errorMessage = errorMessageBuffer;

            return errorMessage;
        }

        /// <summary>
        /// construct
        /// </summary>
        public KeyProvider()
        {
            InitializeComponent();

            eventLog.Source = "KeyProviderLog";
            eventLog.Log = "KeyProviderLog";
        }

        /// <summary>
        /// start service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("KeyProvider.Service starting");

            string errorMessageBuffer = "                                                        ";

            string serviceClass = ConfigurationManager.AppSettings["ServiceClass"];
            string networkAddress = ConfigurationManager.AppSettings["NetworkAddress"];
            string protocolSequence = ConfigurationManager.AppSettings["ProtocolSequence"];
            string endpoint = ConfigurationManager.AppSettings["Endpoint"];

            long result = RegisterRpc(protocolSequence, endpoint, serviceClass, networkAddress, errorMessageBuffer, errorMessageBuffer.Length);
            if (result != 0)
                eventLog.WriteEntry("KeyProviderService error in RegisterRpc. Error: " + result.ToString() + " " + LeftTrim(errorMessageBuffer));
            else
                eventLog.WriteEntry("KeyProviderService started successfully");

            pulseTimer = new System.Timers.Timer();
            pulseTimer.Enabled = true;
            pulseTimer.Interval = Constants.PulseInterval;
            pulseTimer.Elapsed += new ElapsedEventHandler(IntervalTimerElapsed);
        }

        /// <summary>
        /// stop service
        /// </summary>
        protected override void OnStop()
        {
            eventLog.WriteEntry("KeyProvider.Service stopping");

            string errorMessageBuffer = "                                                         ";
            long result = UnregisterRpc(errorMessageBuffer, errorMessageBuffer.Length);
            if (result != 0)
                eventLog.WriteEntry("KeyProvider.Service error in UnregisterRpc. Error: " + result.ToString() + " " + LeftTrim(errorMessageBuffer));
            else
                eventLog.WriteEntry("KeyProvider.Service stopped successfully");
        }

        private void IntervalTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if ((ModuleConfiguration.DISCloudConfigurations!= null) && (ModuleConfiguration.DISCloudConfigurations.Count > 0))
                {
                    IUserProxy userProxy = null;
                    IConfigProxy configProxy = null;

                    string dbConnectionString = String.Empty;
                    string customerID = String.Empty;

                    foreach (string configurationID in ModuleConfiguration.DISCloudConfigurations.Keys)
                    {
                        dbConnectionString = ModuleConfiguration.DISCloudConfigurations[configurationID];

                       customerID = ModuleConfiguration.DISCloudCustomers[configurationID];

                        userProxy = new UserProxy(dbConnectionString);
                        configProxy = new ConfigProxy(userProxy.GetFirstManager(), dbConnectionString, configurationID, customerID);

                        configProxy.KeyProviderServiceReport();
                    }
                }
                else
                {
                    ConfigProxy.KeyProviderServiceReport();
                }
            }
            catch
            {
                //do nothing
            }
        }
    }
}
