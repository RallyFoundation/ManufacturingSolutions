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
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database;
using Microsoft.Practices.Unity;


namespace DIS.Common.Utility
{
    /// <summary>
    /// Message logger
    /// </summary>
    public class MessageLogger
    {
        /// <summary>
        /// Log count per file
        /// </summary>
        public const int LogsPerFile = 500;
        /// <summary>
        /// Category name of system logs
        /// </summary>
        public const string SystemCategoryName = "System";
        /// <summary>
        /// Category name of operation logs
        /// </summary>
        public const string OperationCategoryName = "Operation";

        //public static void ResetLoggingConfiguration(string dbInstanceNameInConfiguration, string dbConnectionString) 
        //{
        //    var builder = new ConfigurationSourceBuilder();

        //    builder.ConfigureData()
        //            .ForDatabaseNamed(dbInstanceNameInConfiguration)
        //                .ThatIs.ASqlDatabase()
        //                .WithConnectionString(dbConnectionString)
        //                .AsDefault();

        //    var configSource = new DictionaryConfigurationSource();
        //    builder.UpdateConfigurationWithReplace(configSource);

        //    IUnityContainer container = new UnityContainer();

        //    // Load the default configuration information from the config file
        //    container.AddNewExtension<EnterpriseLibraryCoreExtension>();

        //    var configurator = new UnityContainerConfigurator(container);
        //    EnterpriseLibraryContainer.ConfigureContainer(configurator, configSource);

        //    EnterpriseLibraryContainer.Current = new UnityServiceLocator(container);
        //}

        /// <summary>
        /// Get current method name
        /// </summary>
        /// <returns></returns>
        public static string GetMethodName()
        {
            return new StackTrace().GetFrame(1).GetMethod().Name;
        }

        /// <summary>
        /// Get the title of operation logs
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetOperationLogTitle(string userName)
        {
            string title = "KMT";
            if (!string.IsNullOrEmpty(userName))
                title = string.Format("{0} - {1}", title, userName);
            return title;
        }

        ///// <summary>
        ///// Log user operations
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="message"></param>
        //public static void LogOperation(string userName, string message)
        //{
        //    LogMessage(GetOperationLogTitle(userName), message, OperationCategoryName,
        //        TraceEventType.Information);
        //}

        /// <summary>
        /// Log user operations
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public static void LogOperation(string userName, string message, string dbConnectionString)
        {
            LogMessage(GetOperationLogTitle(userName), message, OperationCategoryName,
                TraceEventType.Information, dbConnectionString);
        }

        ///// <summary>
        ///// Log system errors
        ///// </summary>
        ///// <param name="title"></param>
        ///// <param name="message"></param>
        //public static void LogSystemError(string title, string message)
        //{
        //    LogSystemRunning(title, message, TraceEventType.Error);
        //}

        /// <summary>
        /// Log system errors
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void LogSystemError(string title, string message, string dbConnectionString)
        {
            LogSystemRunning(title, message, dbConnectionString, TraceEventType.Error);
        }

        ///// <summary>
        ///// Log running infomation of system
        ///// </summary>
        ///// <param name="title"></param>
        ///// <param name="message"></param>
        ///// <param name="eventType"></param>
        //public static void LogSystemRunning(string title, string message, TraceEventType eventType = TraceEventType.Information)
        //{
        //    LogMessage(title, message, SystemCategoryName, eventType);
        //}

        /// <summary>
        /// Log running infomation of system
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="eventType"></param>
        public static void LogSystemRunning(string title, string message, string dbConnectionString, TraceEventType eventType = TraceEventType.Information)
        {
            LogMessage(title, message, SystemCategoryName, eventType, dbConnectionString);
        }

        //private static void LogMessage(string title, string message, string category, 
        //    TraceEventType eventType)
        //{
        //    LogEntry logEntry = new LogEntry();
        //    logEntry.Title = title;
        //    logEntry.Message = message;
        //    logEntry.Categories.Add(category);
        //    logEntry.Severity = eventType;
        //    logEntry.EventId = 100;
        //    logEntry.Priority = 0;
        //    logEntry.TimeStamp = DateTime.UtcNow;
        //    logEntry.MachineName = Environment.MachineName;
            
        //    Logger.Write(logEntry);
        //}

        private static void parseConnectionString(string ConnectionString, out string ServerName, out string DatabaseName, out string UserName, out string Password)
        {
            string[] fields = ConnectionString.Split(new string[] { ";" }, StringSplitOptions.None);

            ServerName = null;
            DatabaseName = null;
            UserName = null;
            Password = null;

            if ((fields != null) && (fields.Length == 4))
            {
                string[] pair = null;

                for (int i = 0; i < fields.Length; i++)
                {
                    pair = fields[i].Split(new string[] { "=" }, StringSplitOptions.None);

                    switch (i)
                    {
                        case 0:
                            {
                                ServerName = pair[1];
                                break;
                            }
                        case 1:
                            {
                                DatabaseName = pair[1];
                                break;
                            }
                        case 2:
                            {
                                UserName = pair[1];
                                break;
                            }
                        case 3:
                            {
                                Password = pair[1];
                                break;
                            }
                    }
                }
            }
        } 

        private static void LogMessage(string title, string message, string category, TraceEventType eventType, string dbConnectionString)
        {
            DISLogEntry logEntry = new DISLogEntry();
            logEntry.Title = title;
            logEntry.Message = message;
            logEntry.Categories.Add(category);
            logEntry.Severity = eventType;
            logEntry.EventId = 100;
            logEntry.Priority = 0;
            logEntry.TimeStamp = DateTime.UtcNow;
            logEntry.MachineName = Environment.MachineName;
            logEntry.ManagedThreadName = "";

            logEntry.DbConnectionString = dbConnectionString;

            string dbServerName, dbUserName, dbPassword, dbName;

            parseConnectionString(dbConnectionString, out dbServerName, out dbName, out dbUserName, out dbPassword);

            //logEntry.ExtendedProperties.Add("DbConnectionString", dbConnectionString);

            logEntry.ExtendedProperties.Add("DbServer", dbServerName);

            logEntry.ExtendedProperties.Add("DbName", dbName);

            Logger.Write(logEntry);
        } 
    }
}
