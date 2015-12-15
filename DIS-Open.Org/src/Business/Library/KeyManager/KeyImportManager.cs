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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Data.ServiceContract;
using OA3ToolReportKeyInfo = DIS.Data.DataContract.OA3ToolReportKeyInfo;
using System.Reflection;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    public partial class LocalKeyManager : ILocalKeyManager
    {
        #region import ULS Fulfilled keys

        //Import Fulfilled Keys File from ULS
        public List<KeyOperationResult> ImportULSFulfilledKeys(string filePath, HeadQuarter currentHeadQuarter, bool isCheckFileSignature)
        {
            Constants.ImportFileType fileType = ValidateImportKeysFile(filePath);
            try
            {
                ExportKeyList keys = GetKeysByImpotFile(filePath, fileType);
                if (fileType == Constants.ImportFileType.R6_Encrypted || fileType == Constants.ImportFileType.R6_NonEncrypted)
                {
                    if (keys.UserName != currentHeadQuarter.UserName)
                        throw new DisException("ImportKey_InvalidUserName");
                    if (fileType == Constants.ImportFileType.R6_NonEncrypted && (!isCheckFileSignature))
                    {
                        if (keys.AccessKey != currentHeadQuarter.AccessKey)
                            throw new DisException("ImportKey_InvalidPsdOrFile");
                    }
                    else
                    {
                        //if (keys.AccessKey != GenerateHmacValue(currentHeadQuarter.AccessKey, keys.Keys))//For DIS 1.95 Accommodations - Rally Aug. 20, 2015
                        if ((keys.AccessKey != GenerateHmacValue(currentHeadQuarter.AccessKey, keys.Keys)) && (keys.AccessKey != GenerateHmacValue(currentHeadQuarter.AccessKey, keys.Keys, true)))//For DIS 1.95 Accommodations - Rally Aug. 20, 2015
                        {
                            throw new DisException("ImportKey_InvalidPsdOrFile");
                        }
                    }
                }
                else
                {
                    if (keys.UserName != currentHeadQuarter.DisplayName)
                        throw new DisException("ImportKey_InvalidUserName");
                }
                return ImportFulfilledKeys(keys.Keys.ToKeyInfo());
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_ImportFileInvalid");
            }
        }

        private List<KeyOperationResult> ImportFulfilledKeys(List<KeyInfo> keys)
        {
            var results = ValidateKeys(keys, ValidImportFulfilledKeyInDls);
            keys = results.Where(k => !k.Failed).Select(k => k.Key).ToList();

            if (keys.Count > 0)
                SaveKeysAfterGetting(keys, false);

            return results;
        }

        private KeyErrorType ValidImportFulfilledKeyInDls(KeyInfo key, KeyInfo keyInDb)
        {
            if (key.KeyId <= 0 || key.KeyState != KeyState.Fulfilled)
                return KeyErrorType.Invalid;
            if (keyInDb != null && keyInDb.ModifiedDate >= key.ModifiedDate)
                return KeyErrorType.DuplicateImport;
            if (keyInDb != null)
                return KeyErrorType.Duplicate;
            else
                return KeyErrorType.None;
        }

        //Get Keys from Key File
        private ExportKeyList GetKeysByImpotFile(string filePath, Constants.ImportFileType fileType)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("file path invalid!");
            ExportKeyList keys = GetFileKeysByVersion(filePath, fileType);
            if (keys == null || keys.Keys.Count <= 0)
                throw new ArgumentException("file keys is null!");
            return keys;
        }

        //Get Keys from File by Version(R4/R5/R6) 
        private ExportKeyList GetFileKeysByVersion(string filePath, Constants.ImportFileType fileType)
        {
            try
            {
                if (fileType == Constants.ImportFileType.R6_NonEncrypted || fileType == Constants.ImportFileType.R5_NonEncrypted || fileType == Constants.ImportFileType.R4_NonEncrypted)
                    return GetNonEncryptedFileKeysByVersion(filePath, fileType);
                else
                    return GetEncryptedFileKeysByVersion(filePath, fileType);
            }
            catch (AccessViolationException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_PermissionsMsg");
            }
            catch (InvalidOperationException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_ImportFileInvalid");
            }
        }

        private ExportKeyList GetNonEncryptedFileKeysByVersion(string filePath, Constants.ImportFileType fileType)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            string xmlContent = doc.DocumentElement.OuterXml;

            if (fileType == Constants.ImportFileType.R5_NonEncrypted)
                xmlContent = ConvertFileContentR5toR6(xmlContent);
            else if (fileType == Constants.ImportFileType.R4_NonEncrypted)
                xmlContent = ConvertFileContentR4toR6(xmlContent);
            return Serializer.FromXml<ExportKeyList>(xmlContent);
        }

        private ExportKeyList GetEncryptedFileKeysByVersion(string filePath, Constants.ImportFileType fileType)
        {
            try
            {
                EncryptedExportKeyList export = Serializer.ReadFromXml<EncryptedExportKeyList>(filePath);
                X509Certificate2 certificate = certificate = KeyManagerHelper.GetInternalCertificate();
                using (RSACryptoServiceProvider provider = (RSACryptoServiceProvider)certificate.PrivateKey)
                {
                    byte[] key = EncryptionHelper.RsaDecrypt(Convert.FromBase64String(export.Key), provider);
                    byte[] iv = EncryptionHelper.RsaDecrypt(Convert.FromBase64String(export.IV), provider);
                    byte[] exportKeysData = EncryptionHelper.AesDecrypt(Convert.FromBase64String(export.ProductKeys), key, iv);
                    string strKeysData = Constants.DefaultEncoding.GetString(exportKeysData);
                    if (fileType == Constants.ImportFileType.R5_Encrypted)
                        strKeysData = ConvertFileContentR5toR6(strKeysData);
                    else if (fileType == Constants.ImportFileType.R4_Encrypted)
                        strKeysData = ConvertFileContentR4toR6(strKeysData);
                    return strKeysData.FromXml<ExportKeyList>();
                }
            }
            catch (CryptographicException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_GetprivateKeyError");
            }
        }

        //R5 File Format Convert to R6 Format
        private string ConvertFileContentR5toR6(string xmlContent)
        {
            xmlContent = xmlContent.Replace("<From>", "<UserName>").Replace("</From>", "</UserName>");
            xmlContent = xmlContent.Replace("<To>", "<AccessKey>").Replace("</To>", "</AccessKey>");
            return xmlContent;
        }

        //R4 File Format Convert to R6 Format
        private string ConvertFileContentR4toR6(string xmlContent)
        {
            xmlContent = xmlContent.Replace("<From>", "<UserName>").Replace("</From>", "</UserName>");
            xmlContent = xmlContent.Replace("<To>", "<AccessKey>").Replace("</To>", "</AccessKey>");
            xmlContent = xmlContent.Replace("<ExportFulFilledKey>", "<ExportKeyInfo>").Replace("</ExportFulFilledKey>", "</ExportKeyInfo>");
            if (xmlContent.Contains("<OEMOptionalInfo>&lt;OEMOptionalInfo /&gt;</OEMOptionalInfo>"))
                xmlContent = xmlContent.Replace("<OEMOptionalInfo>&lt;OEMOptionalInfo /&gt;</OEMOptionalInfo>", "<OEMOptionalInfo/>");
            else if (xmlContent.Contains("<OEMOptionalInfo>"))
                throw new DisException("ImportKey_R4OptionMsg");
            return xmlContent;
        }

        #endregion

        #region Import DLS Bound Keys

        //Import Bound Report Keys From DLS
        public List<KeyOperationResult> ImportDLSBoundKeys(string filePath, List<Subsidiary> subsidiaries, bool IsCheckFileSignature)
        {
            if (subsidiaries == null || subsidiaries.Count() <= 0)
                throw new ApplicationException("subsidiaries is null!");

            Constants.ImportFileType fileType = ValidateImportKeysFile(filePath);
            try
            {
                ExportKeyList keys = GetKeysByImpotFile(filePath, fileType);
                int ssId = 0;
                if (fileType == Constants.ImportFileType.R6_Encrypted || fileType == Constants.ImportFileType.R6_NonEncrypted)
                {
                    if (fileType == Constants.ImportFileType.R6_NonEncrypted && (!IsCheckFileSignature))
                    {
                        Subsidiary subs = subsidiaries.Where(s => s.UserName == keys.UserName && s.AccessKey == keys.AccessKey).FirstOrDefault();
                        if (subs == null)
                            throw new DisException("ImportKey_InvalidPsdOrFile");
                        ssId = subs.SsId;
                    }
                    else
                    {
                        Subsidiary subs = subsidiaries.Where(s => s.UserName == keys.UserName && GenerateHmacValue(s.AccessKey, keys.Keys) == keys.AccessKey).FirstOrDefault();
                        if (subs == null)
                            throw new DisException("ImportKey_InvalidPsdOrFile");
                        ssId = subs.SsId;
                    }
                }
                else
                {
                    Subsidiary subs = subsidiaries.Where(s => s.DisplayName == keys.UserName).FirstOrDefault();
                    if (subs == null)
                        throw new DisException("ImportKey_InvalidUserName");
                    ssId = subs.SsId;
                }
                return ImportBoundKeys(keys.Keys.ToKeyInfo(), ssId);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_ImportFileInvalid");
            }
        }

        private List<KeyOperationResult> ImportBoundKeys(List<KeyInfo> keys, int ssId)
        {
            var results = ValidateKeys(keys, ValidImportBoundKeyInUls);
            keys = results.Where(k => !k.Failed).Select(k => k.Key).ToList();

            if (keys.Count > 0)
            {
                List<KeyInfo> succeedKeys = UpdateKeysAfterBeingReported(keys, ssId);
                List<KeyInfo> failedKeys = keys.Where(k => !succeedKeys.Select(s => s.KeyId).ToList().Contains(k.KeyId)).ToList();
                foreach (KeyOperationResult r in results.Where(r => failedKeys.Contains(r.Key)))
                {
                    r.Failed = true;
                    r.FailedType = KeyErrorType.SsIdInvalid;
                }
            }
            return results;
        }

        private KeyErrorType ValidImportBoundKeyInUls(KeyInfo key, KeyInfo keyInDb)
        {
            if (key.KeyId <= 0 || key.KeyState != KeyState.Bound || string.IsNullOrEmpty(key.HardwareHash))
                return KeyErrorType.Invalid;
            if (keyInDb == null)
                return KeyErrorType.NotFound;
            if (keyInDb != null && keyInDb.ModifiedDate >= key.ModifiedDate && keyInDb.KeyState == KeyState.Bound)
                return KeyErrorType.DuplicateImport;
            if (!ValidateKeyState(keyInDb.UlsReceivingBoundKey))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

        #endregion

        #region import tool key

        //Import Tool Report Key in the Factoryfloor
        public List<KeyOperationResult> ImportToolKey(string importFilePath)
        {
            if (!File.Exists(importFilePath))
            { 
                //throw new FileNotFoundException("Import file cannot be found.");

                ExceptionHandler.HandleException(new FileNotFoundException(String.Format("Import file cannot be found: {0}", importFilePath)), this.keyRepository.GetDBConnectionString());

                return new List<KeyOperationResult>(new KeyOperationResult[]{new KeyOperationResult() 
                {
                     Failed = true,
                     FailedType = KeyErrorType.OriginalFileNotFound,
                     OriginalFileName = importFilePath
                }});
            }

            XmlDocument xmldoc = new XmlDocument();

            try
            {
                xmldoc.Load(importFilePath);

                int errorCount = 0;
                string errorInfo = "";
                string logMsg = "";

                if (KeyManagerHelper.ParseAndValidateXML(xmldoc.DocumentElement.OuterXml, KeyManagerHelper.ToolReportkeyFormat, out errorCount, out errorInfo) == null)
                {    
                    //throw new DisException("Exception_ImportFileInvalid");

                    logMsg = string.Format("Original Document: {0}; Error Count: {1}; Error Detial: {2}", System.Environment.NewLine + xmldoc.DocumentElement.OuterXml + System.Environment.NewLine, errorCount, System.Environment.NewLine + errorInfo);

                    ExceptionHandler.HandleException(new DisException(String.Format("Exception_ImportFileInvalid: {0} {1}", importFilePath, System.Environment.NewLine + logMsg)), this.keyRepository.GetDBConnectionString());

                    return new List<KeyOperationResult>(new KeyOperationResult[]{new KeyOperationResult() 
                    {
                         Failed = true,
                         FailedType = KeyErrorType.InvalidOriginalFile,
                         OriginalFileName = importFilePath
                    }});
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                //throw new DisException("Exception_ImportFileInvalid");

                ExceptionHandler.HandleException(new DisException(String.Format("Exception_ImportFileInvalid: {0}", importFilePath)), this.keyRepository.GetDBConnectionString());

                return new List<KeyOperationResult>(new KeyOperationResult[]{new KeyOperationResult() 
                {
                        Failed = true,
                        FailedType = KeyErrorType.InvalidOriginalFile,
                        OriginalFileName = importFilePath
                }});
            }

            //OA3ToolReportKeyInfo.Key key = new OA3ToolReportKeyInfo.Key(); -Rally, Nov 25, 2014
            //key = Serializer.FromXml<OA3ToolReportKeyInfo.Key>(xmldoc.OuterXml);

            try
            {
                OA3ToolReportKeyInfo.Key key = new OA3ToolReportKeyInfo.Key();
                key = Serializer.FromXml<OA3ToolReportKeyInfo.Key>(xmldoc.OuterXml);

                List<KeyOperationResult> result = UpdateImportToolKey(key);
                if (result.Count(r => !r.Failed) > 0) 
                {
                    BackUpToolKeyFile(importFilePath, key.ProductKeyID);
                }
                
                if(result.Count > 0) //Add original file name to the result for batch import tracking - Rally, Nov 24, 2014
                {
                    foreach (var item in result)
                    {
                        item.OriginalFileName = importFilePath;
                    }
                }

                return result;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                //throw new DisException("Exception_ImportFileInvalid");-Rally, Nov 25, 2014
                ExceptionHandler.HandleException(new DisException(String.Format("Exception_ImportFileInvalid: {0}", importFilePath)), this.keyRepository.GetDBConnectionString());//-Rally, Nov 25, 2014

                return new List<KeyOperationResult>(new KeyOperationResult[]{new KeyOperationResult() //-Rally, Nov 25, 2014
                {
                        Failed = true,
                        FailedType = KeyErrorType.InvalidOriginalFile,
                        OriginalFileName = importFilePath
                }});
            }
            catch (System.Exception ex) //-Rally, Nov 25, 2014
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                //throw new DisException("Exception_ImportFileInvalid");-Rally, Nov 25, 2014
                ExceptionHandler.HandleException(new DisException(String.Format("Exception_ImportFileInvalid: {0}", importFilePath)), this.keyRepository.GetDBConnectionString());//-Rally, Nov 25, 2014

                return new List<KeyOperationResult>(new KeyOperationResult[]{new KeyOperationResult() //-Rally, Nov 25, 2014
                {
                        Failed = true,
                        FailedType = KeyErrorType.InvalidOriginalFile,
                        OriginalFileName = importFilePath
                }});
            }
        }

        private List<KeyOperationResult> UpdateImportToolKey(OA3ToolReportKeyInfo.Key key)
        {
            List<KeyOperationResult> results = new List<KeyOperationResult>();
            KeyInfo keyinfo = keyRepository.GetKey(key.ProductKeyID);
            KeyErrorType FailedType = ValidImportToolReportKey(key, keyinfo);
            if (FailedType == KeyErrorType.None)
            {
                OemOptionalInfo oemOptionInfo = null;
                oemOptionInfo = (key.OEMOptionalInfo.Count <= 0 ? new OemOptionalInfo() : new OemOptionalInfo(key.OEMOptionalInfo));
                
               // keyRepository.UpdateKey(keyinfo, null, null, key.HardwareHash, oemOptionInfo, key.TrackingInfo, true);

                //Add serial number suppoort - Rally Sept. 22, 2014
                keyRepository.UpdateKey(keyinfo, null, null, key.HardwareHash, oemOptionInfo, key.TrackingInfo, key.SerialNumber, true);
            }
            results.Add(new KeyOperationResult()
                {
                    Failed = FailedType != KeyErrorType.None,
                    FailedType = FailedType,
                    Key = new KeyInfo()
                    {
                        KeyId = key.ProductKeyID,
                        ProductKey = (keyinfo != null ? keyinfo.ProductKey : string.Empty),
                        KeyState = (KeyState)key.ProductKeyState,
                        HardwareHash = key.HardwareHash,
                        //Add serial number suppoort - Rally Sept. 22, 2014
                        SerialNumber = key.SerialNumber
                    },
                    KeyInDb = keyinfo
                });
            return results;
        }

        private KeyErrorType ValidImportToolReportKey(OA3ToolReportKeyInfo.Key key, KeyInfo keyinfo)
        {
            if (key.ProductKeyID <= 0)
                return KeyErrorType.Invalid;
            else if (key.ProductKeyState != (byte)KeyState.Bound)
                return KeyErrorType.FileStateInvalid;
            else if (keyinfo == null)
                return KeyErrorType.NotFound;
            else if (keyinfo.KeyInfoEx.KeyType != KeyType.Standard)
                return KeyErrorType.KeyTypeInvalid;
            else if (!ValidateKeyState(() => keyinfo.FactoryFloorBoundKey(true)))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

        private bool validateKeyFileXMLSchema(string keyFileName, out string exceptionInfo) 
        {
            XmlDocument xmlDoc = new XmlDocument();

            exceptionInfo = "";

            try
            {
                xmlDoc.Load(keyFileName);

                int errorCount = 0;
                string errorInfo = "";

                if (KeyManagerHelper.ParseAndValidateXML(xmlDoc.DocumentElement.OuterXml, KeyManagerHelper.ToolReportkeyFormat, out errorCount, out errorInfo) != null) 
                {
                    return true;
                }
                else
                {
                    exceptionInfo = string.Format("Original Document: {0}; Error Count: {1}; Error Detial: {2}", System.Environment.NewLine + keyFileName + System.Environment.NewLine, errorCount, System.Environment.NewLine + errorInfo);
                    return false;
                }
            }
            catch (Exception ex)
            {
                exceptionInfo = ex.ToString();
            }

            return false;
        }

        private OA3ToolReportKeyInfo.Key getToolKeyFromKeyFile(string keyFileName, out string exceptionInfo) 
        {
            XmlDocument xmlDoc = new XmlDocument();

            exceptionInfo = "";

            try
            {
                xmlDoc.Load(keyFileName);

                return Serializer.FromXml<OA3ToolReportKeyInfo.Key>(xmlDoc.OuterXml);
            }
            catch (Exception ex)
            {
                exceptionInfo = ex.ToString();
            }

            return null;
        }

        /// <summary>
        /// Validate key files in batch - Rally Dec 1, 2014
        /// </summary>
        /// <param name="originalFiles"></param>
        /// <returns>Validation results</returns>
        public List<KeyOperationResult> ValidateKeyImportOriginalFiles(string[] originalFiles) 
        {
            List<KeyOperationResult> results = new List<KeyOperationResult>();

            List<string> healthyKeyFiles = new List<string>();

            string exceptionInfo = "";

            Dictionary<string, string> keyFileValidationErrors = new Dictionary<string, string>();

            Dictionary<long, List<string>> DPKIDAndFileDictionary = new Dictionary<long, List<string>>();

            Dictionary<string, OA3ToolReportKeyInfo.Key> keyAndFileDictionary = new Dictionary<string, OA3ToolReportKeyInfo.Key>();

            foreach (string originalFile in originalFiles)
            {
                if (!File.Exists(originalFile))
                {
                    results.Add(new KeyOperationResult() 
                    {
                         Failed = true,
                         FailedType = KeyErrorType.OriginalFileNotFound,
                         OriginalFileName = originalFile
                    });

                    keyFileValidationErrors.Add(originalFile, "NOT_FOUND");
                }
                else if(!this.validateKeyFileXMLSchema(originalFile, out exceptionInfo))
                {
                    results.Add(new KeyOperationResult()
                    {
                        Failed = true,
                        FailedType = KeyErrorType.InvalidOriginalFile,
                        OriginalFileName = originalFile,
                        FailureDetail = exceptionInfo
                    });

                    keyFileValidationErrors.Add(originalFile, exceptionInfo);
                }
                else
                {
                    healthyKeyFiles.Add(originalFile);
                }
            }

            if (healthyKeyFiles.Count > 0)
            {
                OA3ToolReportKeyInfo.Key key = null;

                for (int i = 0; i < healthyKeyFiles.Count; i++)
                {
                    key = this.getToolKeyFromKeyFile(healthyKeyFiles[i], out exceptionInfo);

                    if ((key != null) && (String.IsNullOrEmpty(exceptionInfo)))
                    {
                        if (!DPKIDAndFileDictionary.ContainsKey(key.ProductKeyID))
                        {
                            DPKIDAndFileDictionary.Add(key.ProductKeyID, new List<string>(new string[] { healthyKeyFiles[i] }));
                        }
                        else
                        {
                            DPKIDAndFileDictionary[key.ProductKeyID].Add(healthyKeyFiles[i]);
                        }

                        keyAndFileDictionary.Add(healthyKeyFiles[i], key);
                    }
                    else
                    {
                        results.Add(new KeyOperationResult()
                        {
                            Failed = true,
                            FailedType = KeyErrorType.InvalidOriginalFile,
                            OriginalFileName = healthyKeyFiles[i],
                            FailureDetail = exceptionInfo
                        });

                        keyFileValidationErrors.Add(healthyKeyFiles[i], exceptionInfo);

                        healthyKeyFiles.RemoveAt(i);

                        i--;
                    }
                }
            }

            foreach (long productKeyID in DPKIDAndFileDictionary.Keys)
            {
                if (DPKIDAndFileDictionary[productKeyID].Count > 1)
                {
                    foreach (string originalFile in DPKIDAndFileDictionary[productKeyID])
                    {
                        results.Add(new KeyOperationResult()
                        {
                            Failed = true,
                            FailedType = KeyErrorType.Duplicate,
                            OriginalFileName = originalFile,
                            Key = new KeyInfo() 
                            {
                                KeyId = productKeyID,
                                HardwareHash = keyAndFileDictionary[originalFile].HardwareHash,
                                SerialNumber = keyAndFileDictionary[originalFile].SerialNumber,
                                TrackingInfo = keyAndFileDictionary[originalFile].TrackingInfo
                            }
                        });

                        healthyKeyFiles.Remove(originalFile);
                    }
                }
            }

            if (healthyKeyFiles.Count > 0)
            {
                for (int i = 0; i < healthyKeyFiles.Count; i++)
                {
                    results.Add(new KeyOperationResult() 
                    {
                        Failed = false,
                        FailedType = KeyErrorType.None,
                        OriginalFileName = healthyKeyFiles[i],
                        Key = new KeyInfo()
                        {
                            KeyId = keyAndFileDictionary[healthyKeyFiles[i]].ProductKeyID,
                            HardwareHash = keyAndFileDictionary[healthyKeyFiles[i]].HardwareHash,
                            SerialNumber = keyAndFileDictionary[healthyKeyFiles[i]].SerialNumber,
                            TrackingInfo = keyAndFileDictionary[healthyKeyFiles[i]].TrackingInfo
                        }
                    });
                }
            }

            return results;
        }

        #endregion

        #region Import ReturnAck Keys

        //Import Return.Ack From MS 
        public List<KeyOperationResult> ImportReturnAckKeys(string filePath, KeyStoreContext context = null)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("file path invalid!");

            ReturnAck returnAck = GetReturnKeyAckByFile(filePath);
            try
            {
                return ImportReturnAck(returnAck, context);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_ImportFileInvalid");
            }
        }

        private ReturnAck GetReturnKeyAckByFile(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            string xmlContent = doc.DocumentElement.OuterXml;
            xmlContent = RemoveXmlNullNode(filePath);
            try
            {
                return Serializer.FromXml<ReturnAck>(xmlContent);
            }
            catch (AccessViolationException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_PermissionsMsg");
            }
            catch (InvalidOperationException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_ImportFileInvalid");
            }
        }

        private string RemoveXmlNullNode(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            string xmlContent = doc.DocumentElement.OuterXml;
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("ns", "http://schemas.ms.it.oem/digitaldistribution/2010/10");
            XmlNode xn = doc.SelectSingleNode("/ns:ReturnAck", nsMgr);
            XmlNode xnItems = doc.SelectSingleNode("/ns:ReturnAck/ns:ReturnAckLineItems", nsMgr);

            if (xn == null || xnItems == null)
                throw new DisException("Exception_ImportFileInvalid");
            for (int i = 0; i < xn.ChildNodes.Count; i++)
            {
                XmlNode node = xn.ChildNodes[i];
                if (string.IsNullOrEmpty(node.InnerXml) || (!node.HasChildNodes))
                    xn.RemoveChild(node);
            }

            for (int i = 0; i < xnItems.ChildNodes.Count; i++)
            {
                XmlNode node = xnItems.ChildNodes[i];
                for (int j = 0; j < node.ChildNodes.Count; j++)
                {
                    XmlNode node1 = node.ChildNodes[j];
                    if (string.IsNullOrEmpty(node1.InnerXml) || (!node1.HasChildNodes))
                        node.RemoveChild(node1);
                }
            }
            return doc.OuterXml;
        }

        private KeyErrorType ValidImportReturnAckKey(KeyInfo key)
        {
            KeyInfo keyInDb = keyRepository.GetKey(key.KeyId);

            if (key.KeyId <= 0 || key.KeyState != KeyState.Fulfilled)
                return KeyErrorType.Invalid;
            if (keyInDb != null && keyInDb.ModifiedDate >= key.ModifiedDate)
                return KeyErrorType.DuplicateImport;
            if (keyInDb != null)
                return KeyErrorType.Duplicate;
            else
                return KeyErrorType.None;
        }

        #endregion


        #region private methods

        //Validate and Get Key File Version(R4/R5/R6)
        private Constants.ImportFileType ValidateImportKeysFile(string importFilePath)
        {
            if (!File.Exists(importFilePath))
                throw new FileNotFoundException("Import file cannot be found.");
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(importFilePath);
                string xmlContent = doc.DocumentElement.OuterXml;

                int errorCount = 0;
                string errorInfo = "";

                if (KeyManagerHelper.ParseAndValidateXML(xmlContent, KeyManagerHelper.EncryptedExportKeyFormat, out errorCount, out errorInfo) != null)
                    return ValidateEncryptedFileType(importFilePath);
                else if (KeyManagerHelper.ParseAndValidateXML(xmlContent, KeyManagerHelper.NonEncryptedExportKeyFormat, out errorCount, out errorInfo) != null)
                    return Constants.ImportFileType.R6_NonEncrypted;
                else if (KeyManagerHelper.ParseAndValidateXML(xmlContent, KeyManagerHelper.EncryptedExportKeyFormat_R4, out errorCount, out errorInfo) != null)
                    return Constants.ImportFileType.R4_Encrypted;
                else if (KeyManagerHelper.ParseAndValidateXML(xmlContent, KeyManagerHelper.NonEncryptedExportKeyFormat_R5, out errorCount, out errorInfo) != null)
                {
                    if (xmlContent.Contains("<ExportKeyInfo>"))
                        return Constants.ImportFileType.R5_NonEncrypted;
                    else
                        return Constants.ImportFileType.R4_NonEncrypted;
                }
                else
                    //throw new Exception();
                    throw new Exception(string.Format("Original Document: {0}; Error Count: {1}; Error Detial: {2}", System.Environment.NewLine + xmlContent + System.Environment.NewLine, errorCount, System.Environment.NewLine + errorInfo)); //Rally - Dec. 12, 2014
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_ImportFileInvalid");
            }
        }

        private Constants.ImportFileType ValidateEncryptedFileType(string filePath)
        {
            EncryptedExportKeyList export = Serializer.ReadFromXml<EncryptedExportKeyList>(filePath);
            try
            {
                X509Certificate2 certificate = KeyManagerHelper.GetInternalCertificate();
                using (RSACryptoServiceProvider provider = (RSACryptoServiceProvider)certificate.PrivateKey)
                {
                    byte[] key = EncryptionHelper.RsaDecrypt(Convert.FromBase64String(export.Key), provider);
                    byte[] iv = EncryptionHelper.RsaDecrypt(Convert.FromBase64String(export.IV), provider);
                    byte[] exportKeysData = EncryptionHelper.AesDecrypt(Convert.FromBase64String(export.ProductKeys), key, iv);
                    if (Constants.DefaultEncoding.GetString(exportKeysData).Contains("<UserName>"))
                        return Constants.ImportFileType.R6_Encrypted;
                    else
                        return Constants.ImportFileType.R5_Encrypted;
                }
            }
            catch (CryptographicException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_GetprivateKeyError");
            }
        }

        private ExportKeyList GetDecryptedFileKeys(EncryptedExportKeyList export)
        {
            try
            {
                X509Certificate2 certificate = KeyManagerHelper.GetInternalCertificate();
                using (RSACryptoServiceProvider provider = (RSACryptoServiceProvider)certificate.PrivateKey)
                {
                    byte[] key = EncryptionHelper.RsaDecrypt(Convert.FromBase64String(export.Key), provider);
                    byte[] iv = EncryptionHelper.RsaDecrypt(Convert.FromBase64String(export.IV), provider);
                    byte[] exportKeysData = EncryptionHelper.AesDecrypt(Convert.FromBase64String(export.ProductKeys), key, iv);
                    return Constants.DefaultEncoding.GetString(exportKeysData).FromXml<ExportKeyList>();
                }
            }
            catch (CryptographicException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_GetprivateKeyError");
            }
        }

        private void BackUpToolKeyFile(string filepath, long keyId)
        {
            string filename = Path.GetFileName(filepath);
            KeyInfo dbkey = keyRepository.GetKey(keyId);
            string backFilePath = !string.IsNullOrEmpty(dbkey.OemPartNumber) ? Path.Combine(Directory.GetCurrentDirectory(), KeyManagerHelper.DefaultToolKeyFolderName, dbkey.OemPartNumber, KeyManagerHelper.DefaultToolKeyUsedFolderName) : Path.Combine(Directory.GetCurrentDirectory(), KeyManagerHelper.DefaultToolKeyFolderName, KeyManagerHelper.DefaultToolKeyUsedFolderName);
            string backfulPath = Path.Combine(backFilePath, filename);
            KeyManagerHelper.CreateFilePath(backfulPath, this.keyRepository.GetDBConnectionString());
            if (!File.Exists(backfulPath))
                File.Copy(filepath, Path.Combine(backFilePath, filename));
        }

        #endregion

        private string keyImportMessageTranformationXSLT;
        public string GetKeyImportMessageTranformationXSLT() 
        {
            return this.keyImportMessageTranformationXSLT;
        }

        public void SetKeyImportMessageTranformationXSLT(string value)
        {
            this.keyImportMessageTranformationXSLT = value;
        }

    }
}
