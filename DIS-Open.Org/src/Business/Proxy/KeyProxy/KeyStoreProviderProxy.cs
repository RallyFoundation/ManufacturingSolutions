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
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Collections.Generic;
using DIS.Business.Library;
using DIS.Business.Proxy.KeyProvider.Parameters;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Data.DataAccess;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Common;

namespace DIS.Business.Proxy
{

    public class KeyStoreProviderProxy : IKeyStoreProviderProxy
    {
        #region Variables

        private const string parameterSchema = "Parameters.xsd";
        private const string productKeyInfoSchema = "ProductKeyInfo.xsd";

        private const string paramNameCloudConfigurationID = "CloudConfigurationID";
        private const string paramNameSerialNumber = "SerialNumber";

        private const string globalTraceSourceName = "DISKeyProviderServiceTraceSource";

        private static NameValueCollection section =
            ConfigurationManager.GetSection("Parameters") as NameValueCollection;
        private IKeyManager keyManager;

        private string databaseConnectionString;

        #endregion

        public static IDictionary<string, string> DISCloudConfigurations;

        #region Constructors

        /// <summary>
        /// KeyStoreProvider constructor
        /// </summary>
        public KeyStoreProviderProxy()
        {
            try
            {
                keyManager = new KeyManager(null);
            }
            catch (Exception ex) { ExceptionHandler.HandleException(ex, this.databaseConnectionString); }
        }

        public KeyStoreProviderProxy(string dbConnectionString)
        {
            try
            {
                keyManager = new KeyManager(null, dbConnectionString);

                this.databaseConnectionString = dbConnectionString;
            }
            catch (Exception ex) 
            { 
               // ExceptionHandler.HandleException(ex); 

                //At this time, there is not a specific customer context to log the errors occurred, so a global log is used - Rally Sept. 11, 2014
                DIS.Common.Utility.TracingHelper.Trace(new object[] { ex }, globalTraceSourceName);
            }
        }

        public KeyStoreProviderProxy(IKeyManager keyManager)
        {
            try
            {
                this.keyManager = keyManager;
            }
            catch (Exception ex) { ExceptionHandler.HandleException(ex, this.databaseConnectionString); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get product key info from key store, called from KeyProviderListener.
        /// </summary>
        /// <param name="productKeyInfo">product key info BLOB</param>
        /// <returns></returns>
        public int GetKey(string parameters, ref string productKeyInfo)
        {
            try
            {
                int errorCount = 0;
                string errorInfo = "";
                string logMsg = "";

                XDocument param = ParseAndValidateXML(parameters, parameterSchema, out errorCount, out errorInfo);

                if (param == null)
                {
                    logMsg = string.Format("Original Document: {0}; Error Count: {1}; Error Detial: {2}", System.Environment.NewLine + parameters + System.Environment.NewLine, errorCount, System.Environment.NewLine + errorInfo);

                    //MessageLogger.LogSystemError("XML Schema Validation Failed", logMsg, this.databaseConnectionString);

                    DIS.Common.Utility.TracingHelper.Trace(new object[] { logMsg }, "DISKeyProviderServiceTraceSource");

                    return Convert.ToInt32(
                        ReturnValue.MSG_KEYPROVIDER_XML_SCHEMA_FORMAT_VIOLATION);
                }

                this.ResetDBConnection(param);

                // Get allocated key in store.
                KeySearchCriteria query = new KeySearchCriteria();
                query.KeyState = KeyState.Fulfilled;
                query.SortBy = "FulfilledDateUTC";
                query.SortByDesc = false;
                query.PageSize = 1;

                query = AttachParameters(query, param);
                if (query == null)
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_XML_INVALID_PARAMETER);
                }

                KeyInfo key = null;
                try
                {
                    key = GetKeyTransaction(query);
                    MessageLogger.LogSystemRunning("GetKey", string.Format("{0} key {1}/{2} was consumed to {3}.\r\n{4}",
                        key.KeyInfoEx.KeyType.ToString(), key.KeyId, key.ProductKey, key.KeyStateName, parameters), this.databaseConnectionString);
                }
                catch (ApplicationException)
                {
                    return Convert.ToInt32(
                        ReturnValue.MSG_KEYPROVIDER_NO_KEYS_AVAILABLE_FOR_SPECIFIED_PARAMETERS);
                }

                // Compose and return XML to DMTool.
                XElement dm = new XElement("Key",
                    new XElement("ProductKey", key.ProductKey),
                    new XElement("ProductKeyID", key.KeyId.ToString()),
                    new XElement("ProductKeyState", (byte)KeyState.Consumed),
                    new XElement("ProductKeyPartNumber", key.SkuId)
                );
                productKeyInfo = dm.ToString();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.databaseConnectionString);

                if (ex.GetType() == typeof(EntityException))
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED_DB_CONNECTION);
                }

                return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED);
            }

            return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_SUCCESS);
        }

        private KeyInfo GetKeyTransaction(KeySearchCriteria criteria)
        {
            KeyInfo key = null;

            bool failed = false;
            using (KeyStoreContext context = new KeyStoreContext(this.databaseConnectionString, false))
            {
                ObjectContext oCtx = ((IObjectContextAdapter)context).ObjectContext;
                oCtx.Connection.Open();
                DbTransaction trans = oCtx.Connection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                try
                {
                    IQueryable<KeyInfo> query = context.KeyInfoes.Include("KeyInfoEx").OrderBy(k => k.FulfilledDateUtc);
                    if (!string.IsNullOrEmpty(criteria.MsPartNumber))
                        query = query.Where(k => k.LicensablePartNumber.Contains(criteria.MsPartNumber));
                    if (!string.IsNullOrEmpty(criteria.OemPoNumber))
                        query = query.Where(k => k.OemPoNumber.Contains(criteria.OemPoNumber));
                    if (criteria.HasOemPartNumberNull && string.IsNullOrEmpty(criteria.OemPartNumber))
                        query = query.Where(k => k.OemPartNumber == null || k.OemPartNumber == string.Empty);
                    if (!string.IsNullOrEmpty(criteria.OemPartNumber))
                        query = query.Where(k => k.OemPartNumber.Contains(criteria.OemPartNumber));

                    //SerialNumber support
                    if (!string.IsNullOrEmpty(criteria.SerialNumber)) 
                    {
                        query = query.Where(k => k.SerialNumber.Contains(criteria.SerialNumber));
                    }

                    key = query.FirstOrDefault(k => k.KeyStateId == (int)KeyState.Fulfilled && !k.KeyInfoEx.IsInProgress);

                    if (key == null)
                        throw new ApplicationException("No key available.");

                    key.FactoryFloorAssembleKey();
                    if (key.KeyStateChanged)
                        context.KeyHistories.Add(new KeyHistory()
                        {
                            KeyId = key.KeyId,
                            KeyStateId = key.KeyStateId,
                            StateChangeDate = DateTime.Now
                        });
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    failed = true;
                }
                if (!failed)
                    trans.Commit();

                oCtx.Connection.Close();
            }
            if (failed)
                key = GetKeyTransaction(criteria);

            return key;
        }

        //Multiple customer support - Rally
        private string ExtractCloudConfigurationID(XDocument parameterXml) 
        {
            string configurationID = "";

            foreach (XElement element in parameterXml.Elements("Parameters").Descendants())
            {
                if (element.Name == paramNameCloudConfigurationID)
                {
                    configurationID = element.Value;
                    break;
                }
            }

            return configurationID;
        }

        //Serial number support - Rally
        private string ExtractSerialNumber(XDocument parameterXml)
        {
            string serialNumber = "";

            foreach (XElement element in parameterXml.Elements("Parameters").Descendants())
            {
                if (element.Name == paramNameSerialNumber)
                {
                    serialNumber = element.Value;
                    break;
                }
            }

            if (String.IsNullOrEmpty(serialNumber))
            {
                foreach (XElement element in parameterXml.Elements("Parameters").Descendants())
                {
                    if ((element.Name == "Parameter") && (element.Attribute("name").Value == paramNameSerialNumber))
                    {
                        serialNumber = element.Attribute("value").Value;
                        break;
                    }
                }
            }

            return serialNumber;
        }

        private void ResetDBConnection(XDocument parameterXml) 
        {
            string configurationID = this.ExtractCloudConfigurationID(parameterXml);

            string dbConnectionString = String.Empty;

            if (!String.IsNullOrEmpty(configurationID))
            {
                if ((KeyStoreProviderProxy.DISCloudConfigurations != null) && (KeyStoreProviderProxy.DISCloudConfigurations.ContainsKey(configurationID))) 
                {
                    dbConnectionString = KeyStoreProviderProxy.DISCloudConfigurations[configurationID];
                }
                else
                {
                    throw new EntityException("Could NOT find a DB connection string matching the specified cloud configration ID! Try restarting KPS to refresh local cache.");
                }
            }

            if (!String.IsNullOrEmpty(dbConnectionString))
            {
                this.databaseConnectionString = dbConnectionString;

                this.keyManager = new KeyManager(null, dbConnectionString);

                //MessageLogger.ResetLoggingConfiguration("KeyStoreContext", dbConnectionString);
            }
        }

        private KeySearchCriteria AttachParameters(KeySearchCriteria searchCriteria, XDocument parameterXml)
        {
            var query = searchCriteria;
            foreach (XElement element in parameterXml.Elements("Parameters").Descendants())
            {
                if (element.Name == "Parameter")
                {
                    string parameterName = element.Attribute("name").Value;

                    if (!section.AllKeys.Contains(parameterName))
                    {
                        MessageLogger.LogSystemRunning("AttachParameters", "Invalid Parameters = " +
                            parameterXml.ToString(), this.databaseConnectionString, TraceEventType.Warning);
                        return null;
                    }

                    string value = element.Attribute("value").Value;

                    IParameter processor = Activator.CreateInstance(System.Type.GetType(section[parameterName])) as IParameter;
                    processor.Attach(query, value);
                }
            }
            return query;
        }

        /// <summary>
        /// Set product key info from key store, called from KeyProviderListener.
        /// </summary>
        /// <param name="parameters">Optional parameter for use by OEMs</param>
        /// <param name="productKeyInfo">XML with product key info to store</param>
        /// <returns></returns>
        public int UpdateKey(string parameters, string productKeyInfo)
        {
            try
            {
                int errorCount = 0;
                string errorInfo = "";
                string logMsg = "";

                XDocument param = ParseAndValidateXML(parameters, parameterSchema, out errorCount, out errorInfo);

                if (param == null)
                {
                    logMsg = string.Format("Original Document: {0}; Error Count: {1}; Error Detial: {2}", System.Environment.NewLine + parameters + System.Environment.NewLine, errorCount, System.Environment.NewLine + errorInfo);

                    //MessageLogger.LogSystemError("XML Schema Validation Failed", logMsg, this.databaseConnectionString);

                    DIS.Common.Utility.TracingHelper.Trace(new object[] { logMsg }, "DISKeyProviderServiceTraceSource");

                    return Convert.ToInt32(
                        ReturnValue.MSG_KEYPROVIDER_XML_SCHEMA_FORMAT_VIOLATION);
                }

                this.ResetDBConnection(param);

                XDocument dm = ParseAndValidateXML(productKeyInfo, productKeyInfoSchema, out errorCount, out errorInfo);

                if (dm == null)
                {
                    logMsg = string.Format("Original Document: {0}; Error Count: {1}; Error Detial: {2}", System.Environment.NewLine + productKeyInfo + System.Environment.NewLine, errorCount, System.Environment.NewLine + errorInfo);

                    MessageLogger.LogSystemRunning("XML Schema Validation Failed", logMsg, this.databaseConnectionString);

                    return Convert.ToInt32(
                        ReturnValue.MSG_KEYPROVIDER_XML_SCHEMA_FORMAT_VIOLATION);
                }

                // Get XML bound by DMTool.
                foreach (XElement element in dm.Descendants("Key"))
                {
                    long productKeyId = Convert.ToInt64(GetXElementValue(element, "ProductKeyID"));

                    KeySearchCriteria query = new KeySearchCriteria();
                    query.PageSize = 1;
                    query.KeyId = productKeyId;

                    var keys = keyManager.SearchKeys(query);

                    if (keys == null || keys.Count <= 0)
                    {
                        return Convert.ToInt32(
                           ReturnValue.MSG_KEYPROVIDER_NO_KEYS_AVAILABLE_FOR_SPECIFIED_PARAMETERS);
                    }
                    var key = keys.FirstOrDefault();

                    if (key == null)
                    {
                        return Convert.ToInt32(
                           ReturnValue.MSG_KEYPROVIDER_NO_KEYS_AVAILABLE_FOR_SPECIFIED_PARAMETERS);
                    }

                    var newTrackingInfo = GetTrackingInfo(param);
                    var newHardwareId = GetXElementValue(element, "HardwareHash");
                    var newOEMOptionalInfo = new OemOptionalInfo(GetOEMOptionalInfo(param));
                    var newKeyState = (KeyState)Convert.ToByte(GetXElementValue(element, "ProductKeyState"));

                    //Serial number support - Rally
                    string serialNumber = this.ExtractSerialNumber(param);

                    //Serial number support - Rally
                    if (!keyManager.OaToolReportKey(key, newKeyState, newHardwareId, newOEMOptionalInfo, newTrackingInfo, serialNumber)) //if (!keyManager.OaToolReportKey(key, newKeyState, newHardwareId, newOEMOptionalInfo, newTrackingInfo)
                    {
                        return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_INVALID_PRODUCT_KEY_STATE_TRANSITION);
                    }

                    MessageLogger.LogSystemRunning("UpdateKey", string.Format("{0} key {1}/{2} was reported as {3}.\r\n{4}",
                        key.KeyInfoEx.KeyType.ToString(), key.KeyId, key.ProductKey, newKeyState.ToString(), parameters), this.databaseConnectionString);
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.databaseConnectionString);

                if (ex.GetType() == typeof(EntityException))
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED_DB_CONNECTION);
                }

                return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED);
            }

            return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_SUCCESS);
        }

        private string GetOEMOptionalInfo(XDocument param)
        {
            var result = (from c in param.Elements("Parameters").Descendants()
                          where c.Name == "OEMOptionalInfo"
                          select c).SingleOrDefault();
            return result == null ? null : result.ToString();
        }

        private string GetTrackingInfo(XDocument param)
        {
            var result = (from c in param.Elements("Parameters").Descendants()
                          where c.Name == "TrackingInfo"
                          select c).SingleOrDefault();
            return result == null ? null : result.Value;
        }

        /// <summary>
        /// Test function to validate the client to server connection, called from KeyProviderListener.
        /// </summary>
        /// <param name="message">Text output</param>
        /// <returns></returns>
        public int Ping(ref string productKeyInfo)
        {
            try
            {
                string parameters = ConfigurationManager.AppSettings["PingTestParameters"];

                //MessageLogger.LogSystemRunning("Ping", "Parameters = " + parameters, this.databaseConnectionString);

                DIS.Common.Utility.TracingHelper.Trace(new object[] { "Ping", "Parameters = " + parameters }, "DISKeyProviderServiceTraceSource");

                int errorCount = 0;
                string errorInfo = "";
                string logMsg = "";

                XDocument param = ParseAndValidateXML(parameters, parameterSchema, out errorCount, out errorInfo);

                if (param == null)
                {
                    logMsg = string.Format("Original Document: {0}; Error Count: {1}; Error Detial: {2}", System.Environment.NewLine + productKeyInfo + System.Environment.NewLine, errorCount, System.Environment.NewLine + errorInfo);
                    //MessageLogger.LogSystemError("XML Schema Validation Failed", logMsg, this.databaseConnectionString);

                    DIS.Common.Utility.TracingHelper.Trace(new object[] {logMsg}, "DISKeyProviderServiceTraceSource");
                }

                // Get allocated key in store.
                KeySearchCriteria query = new KeySearchCriteria();
                query.PageSize = 1;
                query.KeyState = KeyState.Fulfilled;

                query = AttachParameters(query, param);
                if (query == null)
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_XML_INVALID_PARAMETER);
                }

                var keys = keyManager.SearchKeys(query);


                if (keys == null || keys.Count <= 0)
                {
                    return Convert.ToInt32(
                       ReturnValue.MSG_KEYPROVIDER_NO_KEYS_AVAILABLE_FOR_SPECIFIED_PARAMETERS);
                }
                KeyInfo key = keys.FirstOrDefault();

                XElement dm = new XElement("Key",
                    new XElement("ProductKey", key.ProductKey.ToString()),
                    new XElement("ProductKeyID", key.KeyId.ToString()),
                    new XElement("ProductKeyState", key.KeyState.ToString()),
                    new XElement("ProductKeyPartNumber", key.SkuId)
                );

                productKeyInfo = dm.ToString();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.databaseConnectionString);

                if (ex.GetType() == typeof(EntityException))
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED_DB_CONNECTION);
                }

                return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED);
            }

            return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_SUCCESS);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Parse parameters as XLinq Document
        /// </summary>
        /// <param name="parameters">Parameters passed from DMTool</param>
        /// <returns>XML document</returns>
        private XDocument ParseAndValidateXML(string parameters, string schema, out int errorCount, out string errorInfo)
        {
            XDocument doc = null;

            try
            {
                doc = XDocument.Parse(parameters);
            }
            catch
            {
                errorCount = -9;
                errorInfo = "";
                return null;
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            XmlTextReader xtr = new XmlTextReader(
                assembly.GetManifestResourceStream(
                "DIS.Business.Proxy.Schema." + schema));

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, xtr);

            bool errors = false;
            int errorCnt = 0;
            string errorMsg = "";

            doc.Validate(schemas, (o, e) =>
            {
                errors = true;
                errorCnt++; //Rally - Dec. 22, 2014
                errorMsg += System.Environment.NewLine;
                errorMsg += e.Message;
                errorMsg += System.Environment.NewLine;
            });

            errorCount = errorCnt;
            errorInfo = errorMsg;

            return errors ? null : doc;
        }

        /// <summary>
        /// Get value of a specific XElement.
        /// </summary>
        /// <param name="xml">XML element</param>
        /// <param name="name">Element name</param>
        /// <returns>Value of element</returns>
        private string GetXElementValue(XElement xml, string name)
        {
            XElement element = xml.Element(name);
            return element != null ? element.Value : string.Empty;
        }
        #endregion
    }

    /// <summary>
    /// Return value and key state enumerators
    /// </summary>
    public enum ReturnValue
    {
        MSG_KEYPROVIDER_SUCCESS = 300,
        MSG_KEYPROVIDER_FAILED = -1073741523,
        MSG_KEYPROVIDER_FAILED_DB_CONNECTION = -1073741522,
        MSG_KEYPROVIDER_XML_SCHEMA_FORMAT_VIOLATION = -1073741521,
        MSG_KEYPROVIDER_XML_INVALID_PARAMETER = -1073741520,
        MSG_KEYPROVIDER_XML_SCHEMA_MISSING_PRODUCT_KEY_TAG = -1073741519,
        MSG_KEYPROVIDER_XML_SCHEMA_MISSING_PRODUCT_KEY_STATE_TAG = -1073741518,
        MSG_KEYPROVIDER_NO_KEYS_AVAILABLE_FOR_SPECIFIED_PARAMETERS = -1073741517,
        MSG_KEYPROVIDER_INVALID_PRODUCT_KEY_STATE_TRANSITION = -1073741516
    };
}
