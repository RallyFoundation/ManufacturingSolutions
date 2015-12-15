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
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using DIS.Business.KeyProviderLibrary.Parameters;
using DIS.Business.Library;
using DIS.Business.Operation;
using DIS.Data.DataContract;
using DIS.Business.Proxy;

namespace DIS.Business.KeyProviderLibrary
{
    public class KeyStoreProvider : IKeyStoreProvider
    {
        #region Variables

        private const string parameterSchema = "Parameters.xsd";
        private const string productKeyInfoSchema = "ProductKeyInfo.xsd";

        private static NameValueCollection section =
            ConfigurationManager.GetSection("Parameters") as NameValueCollection;
        private IKeyProxy keyProxy;

        #endregion

        #region Constructors

        /// <summary>
        /// KeyStoreProvider constructor
        /// </summary>
        public KeyStoreProvider()
        {
            try
            {
                MessageLogger.LogSystemRunning("KeyStoreProvider",
                    "Initializing KeyStoreProvider with app setting provided connect string.");

                keyProxy = new KeyProxy(null);

                MessageLogger.LogSystemRunning("KeyStoreProvider",
                    "KeyStoreProvider initialization completed.");
            }
            catch (Exception ex) { ExceptionHandler.HandleException(ex); }
        }

        public KeyStoreProvider(IKeyProxy keyProxy)
        {
            try
            {
                MessageLogger.LogSystemRunning("KeyStoreProvider", "Initializing KeyStoreProvider with provided repository.");

                this.keyProxy = keyProxy;

                MessageLogger.LogSystemRunning("KeyStoreProvider", "KeyStoreProvider initialization completed.");
            }
            catch (Exception ex) { ExceptionHandler.HandleException(ex); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get product key info from key store, called from KeyProviderListener.
        /// </summary>
        /// <param name="parameters">Optional parameter for use by OEMs</param>
        /// <param name="productKeyInfo">product key info BLOB</param>
        /// <returns></returns>
        public int GetKey(string parameters, ref string productKeyInfo)
        {
            try
            {
                MessageLogger.LogSystemRunning("GetKey", "Parameters = " + parameters);

                XDocument param = ParseAndValidateXML(parameters, parameterSchema);

                if (param == null)
                {
                    return Convert.ToInt32(
                        ReturnValue.MSG_KEYPROVIDER_XML_SCHEMA_FORMAT_VIOLATION);
                }
                KeyInfo key = null;

                // Get allocated key in store.
                KeySearchCriteria query = new KeySearchCriteria();
                query.KeyStates = new List<KeyState> { KeyState.Fulfilled };
                query.SortBy = "FulfilledDateUTC";
                query.SortByDesc = false;
                query.PageSize = 1;

                query = AttachParameters(query, param);
                if (query == null)
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_XML_INVALID_PARAMETER);
                }
                var keys = keyProxy.SearchKeys(query);


                if (keys == null || keys.Count <= 0)
                {
                    return Convert.ToInt32(
                       ReturnValue.MSG_KEYPROVIDER_NO_KEYS_AVAILABLE_FOR_SPECIFIED_PARAMETERS);
                }
                key = keys.FirstOrDefault();

                keyProxy.UpdateKeyState(key, KeyState.Consumed);

                var updatedKey = keyProxy.SearchKeys(new KeySearchCriteria() { PageSize = 1, KeyId = key.ProductKeyId }).Keys.FirstOrDefault();

                // Compose and return XML to DMTool.
                XElement dm = new XElement("Key",
                    new XElement("ProductKey", updatedKey.ProductKey),
                    new XElement("ProductKeyID", updatedKey.ProductKeyId.ToString()),
                    new XElement("ProductKeyState", (byte)updatedKey.ProductKeyState),
                    new XElement("ProductKeyPartNumber", updatedKey.SKUID)
                );

                productKeyInfo = dm.ToString();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);

                if (ex.GetType() == typeof(EntityException))
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED_DB_CONNECTION);
                }

                return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED);
            }

            return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_SUCCESS);
        }

        private KeySearchCriteria AttachParameters(KeySearchCriteria searchCriteria, XDocument parameterXml)
        {
            var query = searchCriteria;
            foreach (XElement element in parameterXml.Elements("Parameters").Descendants())
            {
                string parameterName = element.Attribute("name").Value;

                if (!section.AllKeys.Contains(parameterName))
                {
                    MessageLogger.LogSystemRunning("AttachParameters", "Invalid Parameters = " +
                        parameterXml.ToString(), TraceEventType.Warning);
                    return null;
                }

                string value = element.Attribute("value").Value;

                IParameter processor = Activator.CreateInstance(System.Type.GetType(section[parameterName])) as IParameter;
                processor.Attach(query, value);
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
                MessageLogger.LogSystemRunning("UpdateKey", "Parameters = " + parameters);

                XDocument param = ParseAndValidateXML(parameters, parameterSchema);

                if (param == null)
                {
                    return Convert.ToInt32(
                        ReturnValue.MSG_KEYPROVIDER_XML_SCHEMA_FORMAT_VIOLATION);
                }

                XDocument dm = ParseAndValidateXML(productKeyInfo, productKeyInfoSchema);

                if (dm == null)
                {
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

                    query = AttachParameters(query, param);

                    var keys = keyProxy.SearchKeys(query);

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

                    var newHardwareId = GetXElementValue(element, "HardwareHash");
                    var newOEMOptionalInfo = new OemOptionalInfo(GetXElementValue(element, "OEMOptionalInfo"));
                    var newKeyState = (KeyState)Convert.ToByte(GetXElementValue(element, "ProductKeyState"));
                    if (!keyProxy.UpdateReportKey(key, newKeyState, newHardwareId, newOEMOptionalInfo))
                        return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_INVALID_PRODUCT_KEY_STATE_TRANSITION);
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);

                if (ex.GetType() == typeof(EntityException))
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED_DB_CONNECTION);
                }

                return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_FAILED);
            }

            return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_SUCCESS);
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

                MessageLogger.LogSystemRunning("Ping", "Parameters = " + parameters);

                XDocument param = ParseAndValidateXML(parameters, parameterSchema);

                // Get allocated key in store.
                KeySearchCriteria query = new KeySearchCriteria();
                query.PageSize = 1;
                query.KeyStates = new List<KeyState> { KeyState.Fulfilled };

                query = AttachParameters(query, param);
                if (query == null)
                {
                    return Convert.ToInt32(ReturnValue.MSG_KEYPROVIDER_XML_INVALID_PARAMETER);
                }

                var keys = keyProxy.SearchKeys(query);


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
                ExceptionHandler.HandleException(ex);

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
        private XDocument ParseAndValidateXML(string parameters, string schema)
        {
            XDocument doc = null;

            try
            {
                doc = XDocument.Parse(parameters);
            }
            catch
            {
                return null;
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            XmlTextReader xtr = new XmlTextReader(
                assembly.GetManifestResourceStream(
                "DIS.Business.KeyProviderLibrary.Schemas." + schema));

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, xtr);

            bool errors = false;
            doc.Validate(schemas, (o, e) =>
            {
                errors = true;
            });

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
}
