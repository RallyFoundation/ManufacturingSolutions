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
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Data.ServiceContract;
using OA3ToolKeyInfo = DIS.Data.DataContract.OA3ToolKeyInfo;
using System.Text;

namespace DIS.Business.Library
{
    public partial class LocalKeyManager : ILocalKeyManager
    {
        #region Export log

        public List<KeyExportLog> SearchExportLogs(ExportLogSearchCriteria exportLogSearchCriteria)
        {
            return miscRepository.SearchExportLogs(exportLogSearchCriteria);
        }

        public void InsertExportLog(KeyExportLog exportlog)
        {
            miscRepository.InsertExportLog(exportlog);
        }

        #endregion

        #region Export offline-CBR

        public List<KeyOperationResult> ExportCbr(ExportParameters exportParameters, Func<List<KeyInfo>, string> generateCbrToFile)
        {
            return Execute(exportParameters,
                k => generateCbrToFile(k),
                (k1, k2) => ValidateReportedBoundKey(k1, k2),
                k => keyRepository.UpdateKeys(k, false, null));
        }

        public List<KeyOperationResult> ExportOHRData(ExportParameters exportParameters, Func<List<KeyInfo>, string> generateOHRDataToFile)
        {
            var keys = ConvertKeyGroupsToKeys(exportParameters);
            var result = ValidateKeys(keys, (k1, k2) => ValidateOHRDataKey(k1, k2));
            if (result.All(r => !r.Failed))
            {
                var fileContent = generateOHRDataToFile(keys);
            }
            return result;
        }

        private KeyErrorType ValidateReportedBoundKey(KeyInfo key, KeyInfo keyInDb)
        {
            if (keyInDb == null)
                return KeyErrorType.NotFound;
            else if (!ValidateKeyState(keyInDb.UlsReportingBoundKeyToMs))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

        private KeyErrorType ValidateOHRDataKey(KeyInfo key, KeyInfo keyInDb)
        {
            if (keyInDb == null)
                return KeyErrorType.NotFound;
            else if (!ValidateKeyState(keyInDb.UlsExportOHRData))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

        #endregion

        #region Export fulfilled keys

        public List<KeyInfo> SearchFulfilledKeys(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeys(GetFulfilledkeysSearchCriteria(searchCriteria));
        }

        public List<KeyGroup> SearchFulfilledKeyGroups(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeyGroups(GetFulfilledkeysSearchCriteria(searchCriteria));
        }

        public List<KeyInfo> SearchFulfilledKeys(List<KeyGroup> keyGroups)
        {
            return keyRepository.SearchKeys(keyGroups);
        }

        //Export Fulfilled Keys to DLS
        public List<KeyOperationResult> ExportFulfilledKeys(ExportParameters exportParameters)
        {
            return Execute(exportParameters,
                (k1, k2) => ValidExportFulfilledKeyInUls(k1, k2),
                k => keyRepository.UpdateKeys(k, false, GetExportToSsId(exportParameters)));
        }

        private KeyErrorType ValidExportFulfilledKeyInUls(KeyInfo key, KeyInfo keyInDb)
        {
            if (keyInDb == null)
                return KeyErrorType.NotFound;
            else if (!ValidateKeyState(keyInDb.UlsExportingFulfilledKey))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

        private KeySearchCriteria[] GetFulfilledkeysSearchCriteria(KeySearchCriteria searchCriteria)
        {
            KeySearchCriteria searchCriteriaUp = ConvertSearchCriteria(searchCriteria);
            searchCriteriaUp.KeyState = KeyState.Fulfilled;
            searchCriteriaUp.IsInProgress = false;
            searchCriteriaUp.IsAssign = false;
            searchCriteriaUp.SsId = null;
            searchCriteriaUp.HqId = CurrentHeadQuarterId;
            return new KeySearchCriteria[] { searchCriteriaUp };
        }

        #endregion

        #region Export bound keys

        public List<KeyInfo> SearchBoundKeys(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeys(GetBoundKeysSearchCriteria(searchCriteria));
        }

        public List<KeyGroup> SearchBoundKeyGroups(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeyGroups(GetBoundKeysSearchCriteria(searchCriteria));
        }

        public List<KeyInfo> SearchBoundKeys(List<KeyGroup> keyGroups)
        {
            return keyRepository.SearchKeys(keyGroups);
        }

        //Export Bound Report Keys to ULS
        public List<KeyOperationResult> ExportBoundKeys(ExportParameters exportParameters)
        {
            return Execute(exportParameters,
                (k1, k2) => ValidateExportBoundKey(k1, k2),
                k => keyRepository.UpdateKeys(k, false, null));
        }

        private KeyErrorType ValidateExportBoundKey(KeyInfo key, KeyInfo keyInDb)
        {
            if (keyInDb == null)
                return KeyErrorType.NotFound;
            else if (!ValidateKeyState(keyInDb.DlsReportingBoundKeyToUls))
                return KeyErrorType.StateInvalid;
            else if (string.IsNullOrEmpty(keyInDb.HardwareHash))
                return KeyErrorType.Invalid;
            else
                return KeyErrorType.None;
        }

        private KeySearchCriteria GetBoundKeysSearchCriteria(KeySearchCriteria searchCriteria)
        {
            var searchCriteriaUp = ConvertSearchCriteria(searchCriteria);
            searchCriteriaUp.KeyType = KeyType.Standard;
            searchCriteriaUp.KeyState = KeyState.Bound;
            searchCriteriaUp.HqId = CurrentHeadQuarterId;
            searchCriteriaUp.IsInProgress = false;
            searchCriteriaUp.HasHardwareHash = true;
            return searchCriteriaUp;
        }

        #endregion

        #region Export tool keys

        //Export Tool Key in the Factoryfloor
        public List<KeyOperationResult> ExportToolKeys(ExportParameters exportParameters)
        {
            return Execute(exportParameters,
                 (k1, k2) => ValidExportConsumedKey(k1, k2),
                 k => keyRepository.UpdateKey(k, false, null, null, null));
        }

        private KeyErrorType ValidExportConsumedKey(KeyInfo key, KeyInfo keyInDb)
        {
            if (keyInDb == null)
                return KeyErrorType.NotFound;
            else if (!ValidateKeyState(keyInDb.FactoryFloorAssembleKey))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

        #endregion

        #region Export Return Keys

        //Export Return Keys to MS
        public List<KeyOperationResult> ExportReturnKeys(ReturnReport request, ExportParameters exportParameters)
        {
            if (request == null || request.ReturnReportKeys.Count() <= 0)
                throw new ArgumentException("export keys is null!");
            var keys = request.ReturnReportKeys.Select(k => new KeyInfo()
            {
                KeyId = k.KeyId
            }).ToList();

            var result = ValidateKeys(keys, ValidExportReturnKey);
            if (result.All(r => !r.Failed))
            {
                string fileContent = GenerateReturnKeysToFile(request, exportParameters);
                List<KeyInfo> keysToUpdate = result.Select(r => r.KeyInDb).ToList();
                returnKeyRepository.InsertReturnReportAndKeys(request);
                keyRepository.UpdateKeys(keysToUpdate, false, null);
                RecordExportLog(keysToUpdate, exportParameters, fileContent, exportParameters.OutputPath);
            }
            result.ForEach(r =>
            {
                r.Key = r.KeyInDb;
            });
            return result;
        }

        private KeyErrorType ValidExportReturnKey(KeyInfo key, KeyInfo keyInDb)
        {
            if (keyInDb == null)
                return KeyErrorType.NotFound;
            else if (!ValidateKeyState(keyInDb.ULsReturningKey))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

        #endregion

        #region Re-export keys

        //re-export keys from the log
        public List<KeyOperationResult> ReExportKeys(int logId, string filePath)
        {
            var keyLog = miscRepository.GetExportLog(logId);
            if (keyLog != null && keyLog.KeyCount > 0)
            {
                KeyManagerHelper.CreateFilePath(filePath, this.keyRepository.GetDBConnectionString());
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(keyLog.FileContent);
                }
            }

            return GetFileKeys(keyLog).Select(key => new KeyOperationResult()
            {
                Failed = false,
                Key = key
            }).ToList();
        }

        public List<KeyInfo> GetLogFileKeys(int logId)
        {
            return GetFileKeys(miscRepository.GetExportLog(logId));
        }

        #endregion

        #region private method

        private List<KeyOperationResult> Execute(ExportParameters exportParameters,
            Func<List<KeyInfo>, string> generateKeysToFile,
            Func<KeyInfo, KeyInfo, KeyErrorType> validateKey,
            Action<List<KeyInfo>> updateKeysState)
        {
            var keys = ConvertKeyGroupsToKeys(exportParameters);
            var result = ValidateKeys(keys, validateKey);
            if (result.All(r => !r.Failed))
            {
                var fileContent = generateKeysToFile(keys);
                List<KeyInfo> keysToUpdate = result.Select(r => r.KeyInDb).ToList();
                updateKeysState(keysToUpdate);
                RecordExportLog(keysToUpdate, exportParameters, fileContent, exportParameters.OutputPath);
            }
            return result;
        }

        private List<KeyOperationResult> Execute(ExportParameters exportParameters,
            Func<KeyInfo, KeyInfo, KeyErrorType> validateKey,
            Action<List<KeyInfo>> updateKeysState)
        {
            return Execute(exportParameters,
                k => GenerateKeysToFile(k, exportParameters),
                (k1, k2) => validateKey(k1, k2),
                k => updateKeysState(k));
        }

        private List<KeyOperationResult> Execute(ExportParameters exportParameters,
            Func<KeyInfo, KeyInfo, KeyErrorType> validateKey,
            Action<KeyInfo> updateKeysState)
        {
            var keys = ConvertKeyGroupsToKeys(exportParameters);
            var keysInDb = GetKeysInDb(keys);
            return keys.Select(key =>
                {
                    var subFullFillName = exportParameters.OutputPath;
                    var result = GenerateKeyOperationResult(key, keysInDb, validateKey);

                    if (!result.Failed)
                    {
                        var fileContent = GenerateKeysToFile(key, subFullFillName, out subFullFillName);
                        KeyInfo keyToUpdate = result.KeyInDb;
                        updateKeysState(keyToUpdate);
                        RecordExportLog(keyToUpdate, exportParameters, fileContent, subFullFillName);
                    }
                    return result;
                }).ToList();
        }

        private List<KeyInfo> ConvertKeyGroupsToKeys(ExportParameters exportParameters)
        {
            List<KeyInfo> keys = null;

            if (exportParameters.Keys.GetType() == typeof(List<KeyGroup>))
            {
                var keyGroups = (List<KeyGroup>)exportParameters.Keys;
                if (keyGroups == null || keyGroups.Count < 0)
                    throw new ArgumentException("export keys is null!");

                keys = keyRepository.SearchKeys(keyGroups);
                if (keys == null || keys.Count <= 0 || keys.Count != keyGroups.Sum(k => k.Quantity))
                    throw new DisException("ExportKeys_DataChangeError");
            }
            else
                keys = (List<KeyInfo>)exportParameters.Keys;

            if (keys == null || keys.Count <= 0)
                throw new ArgumentException("export keys is null!");
            return keys;
        }

        protected KeyOperationResult GenerateKeyOperationResult(KeyInfo key, List<KeyInfo> keysInDb, Func<KeyInfo, KeyInfo, KeyErrorType> validateFunc)
        {
            var keyInDb = GetKey(key.KeyId, keysInDb);
            KeyErrorType errorType = validateFunc(key, keyInDb);
            return new KeyOperationResult()
            {
                Failed = errorType == KeyErrorType.None ? false : true,
                FailedType = errorType,
                Key = key,
                KeyInDb = keyInDb,
            };
        }

        protected List<KeyOperationResult> ValidateKeys(List<KeyInfo> keys, Func<KeyInfo, KeyInfo, KeyErrorType> validateFunc)
        {
            var keysInDb = GetKeysInDb(keys);
            return keys.Select(k => GenerateKeyOperationResult(k, keysInDb, validateFunc)).ToList();
        }

        private string GetExportTo(ExportParameters exportParameters)
        {
            if (exportParameters.ExportType == Constants.ExportType.CBR || exportParameters.ExportType == Constants.ExportType.ReturnKeys)
                return "MS";
            if (exportParameters.ExportType == Constants.ExportType.ToolKeys)
                return "OA3Tool";
            if (exportParameters.To == null)
                throw new ArgumentNullException("Argument ExportParameters:To is null");

            if (exportParameters.To.GetType() == typeof(Subsidiary))
            {
                var target = (Subsidiary)exportParameters.To;
                return target.DisplayName;
            }
            else if (exportParameters.To.GetType() == typeof(HeadQuarter))
            {
                var target = (HeadQuarter)exportParameters.To;
                return target.DisplayName;
            }
            else
                throw new ArgumentException("Argument ExportParameters:To is not valid");
        }

        private int GetExportToSsId(ExportParameters exportParameters)
        {
            if (exportParameters.To == null || (exportParameters.To.GetType() != typeof(Subsidiary)))
                throw new ArgumentException("Argument subsidiary not valid");

            return ((Subsidiary)exportParameters.To).SsId;
        }

        private string GenerateSubDirectory(KeyInfo key, string path, bool isDefaultPath)
        {
            var subDirectory = path;
            var partNumberName = string.IsNullOrEmpty(key.OemPartNumber) ? string.Empty : key.OemPartNumber;
            subDirectory = isDefaultPath ? Path.Combine(subDirectory, partNumberName, KeyManagerHelper.DefaultToolKeyAllocateFolderName) : subDirectory;
            KeyManagerHelper.CreateDirectory(subDirectory, this.keyRepository.GetDBConnectionString());
            return subDirectory;
        }

        private string GenerateKeysToFile(KeyInfo key, string path, out string subFullFileName)
        {
            var directoryName = path;
            var isDefaultPath = directoryName == Path.Combine(Directory.GetCurrentDirectory(), KeyManagerHelper.DefaultToolKeyFolderName) ? true : false;

            var subDirectoryName = GenerateSubDirectory(key, path, isDefaultPath);
            var subFileName = string.Format("{0}.xml", key.KeyId);
            subFullFileName = Path.Combine(subDirectoryName, subFileName);

            var toolKeys = key.ToToolKey();
            Serializer.WriteToXml(toolKeys, subFullFileName);
            return KeyManagerHelper.GetXmlResult(toolKeys);
        }

        private string GenerateKeysToFile(List<KeyInfo> keys, ExportParameters exportParameters)
        {
            KeyManagerHelper.CreateFilePath(exportParameters.OutputPath, this.keyRepository.GetDBConnectionString());
            List<ExportKeyInfo> exportKeys = keys.ToExportFulfilledKey();

            if (exportParameters.IsInCompatibleMode)
            {
                foreach (var key in exportKeys)
                {
                    key.SerialNumber = null;
                    key.CanBindPbr = null;//For DIS 1.95 Accommodations - Rally Aug. 20, 2015
                    key.PbrBindingKeyId = null;//For DIS 1.95 Accommodations - Rally Aug. 20, 2015
                    key.PbrStateId = null;//For DIS 1.95 Accommodations - Rally Aug. 20, 2015
                }
            }

            var exportKeyList = new ExportKeyList()
            {
                Keys = exportKeys,
                UserName = exportParameters.UserName,
                AccessKey = GenerateHmacValue(exportParameters.AccessKey, exportKeys), //exportParameters.IsInCompatibleMode ? this.GenerateHmacValue(exportParameters.AccessKey, exportKeys, exportParameters.TransformationXSLT) : GenerateHmacValue(exportParameters.AccessKey, exportKeys), //Fix for Bug#126, Rally - Dec.16, 2014
                CreatedDate = DateTime.UtcNow
            };

            string result;

            //result = exportKeyList.ToXml(true, true);//Rally - Dec. 19, 2014

            //MessageLogger.LogSystemRunning("Key Export - Before Transformation", result, this.keyRepository.GetDBConnectionString());

            //Rally - Dec. 19, 2014
            //if (exportParameters.IsInCompatibleMode)
            //{
            //    XSLTHelper xsltHelper = new XSLTHelper();

            //    result = result.Substring(result.IndexOf("<ExportKeyList>"));

            //    result = xsltHelper.GetTransformedXmlStringByXsltDocument(result, exportParameters.TransformationXSLT, new Dictionary<string, object>(){{"mode", 1}}, null, "utf-8");
            //}

            //MessageLogger.LogSystemRunning("Key Export - After Transformation", result, this.keyRepository.GetDBConnectionString());

            //Rally - Dec. 22, 2014
            if (exportParameters.IsEncrypted)
            {
                //EncryptedExportKeyList export = KeyManagerHelper.EncryptExportFile(result);

                EncryptedExportKeyList export = KeyManagerHelper.EncryptExportFile(exportKeyList.ToXml(true, true));
                Serializer.WriteToXml(export, exportParameters.OutputPath);
                result = KeyManagerHelper.GetXmlResult(export);
            }
            else
            {
                Serializer.WriteToXml(exportKeyList, exportParameters.OutputPath);
                result = KeyManagerHelper.GetXmlResult(exportKeyList);

                //Rally - Dec. 19, 2014
                //using (FileStream stream = new FileStream(exportParameters.OutputPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                //{
                //    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                //    {
                //        writer.Write(result);
                //    }
                //}
            }

            //Rally - Dec. 19, 2014
            //if (!String.IsNullOrEmpty(result))
            //{
            //    if (result.IndexOf("encoding=\"utf-8\"") > 0)
            //    {
            //        result = result.Replace("encoding=\"utf-8\"", "encoding=\"utf-16\"");
            //    }
            //}

            return result;
        }

        private string GenerateHmacValue(string accessKey, List<ExportKeyInfo> keys)
        {
            byte[] data = Constants.DefaultEncoding.GetBytes(keys.ToXml(true,true));
            return HashHelper.CreateHmac<HMACSHA256>(accessKey, data);
        }

        /// <summary>
        /// For DIS 1.95 Accommodations - Rally Aug. 20, 2015
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="keys"></param>
        /// <param name="shouldIncludePbrProperties"></param>
        /// <returns></returns>
        private string GenerateHmacValue(string accessKey, List<ExportKeyInfo> keys, bool shouldIncludePbrProperties)
        {
            foreach (var key in keys)
            {
                key.ShouldIncludePbrProperties(shouldIncludePbrProperties);
            }

            byte[] data = Constants.DefaultEncoding.GetBytes(keys.ToXml(true, true));
            return HashHelper.CreateHmac<HMACSHA256>(accessKey, data);
        }

        //Fix for Bug#126, Rally - Dec.16, 2014
        //private string GenerateHmacValue(string accessKey, List<ExportKeyInfo> keys, string transformationXslt)
        //{
        //    string keyXml = keys.ToXml(true, true);

        //    MessageLogger.LogSystemRunning("Key Export Access Key Computing- Before Transformation", keyXml, this.keyRepository.GetDBConnectionString());

        //    keyXml = keyXml.Substring(keyXml.IndexOf("<ArrayOfExportKeyInfo>"));

        //    XSLTHelper xsltHelper = new XSLTHelper();

        //    keyXml = xsltHelper.GetTransformedXmlStringByXsltDocument(keyXml, transformationXslt, new Dictionary<string, object>() {{"mode", 0}}, null, "utf-8");

        //    MessageLogger.LogSystemRunning("Key Export Access Key Computing- After Transformation", keyXml, this.keyRepository.GetDBConnectionString());

        //    byte[] data = Constants.DefaultEncoding.GetBytes(keyXml);
        //    return HashHelper.CreateHmac<HMACSHA256>(accessKey, data);
        //}

        private List<KeyInfo> GetFileKeys(KeyExportLog keyLog)
        {
            if (keyLog.ExportType == Constants.ExportType.ToolKeys.ToString())
                return GetToolKeysFromFile(keyLog.FileContent);
            else if (keyLog.ExportType == Constants.ExportType.CBR.ToString())
                return GetKeysFromCBRFile(keyLog.FileContent);
            else if (keyLog.ExportType == Constants.ExportType.ReturnKeys.ToString())
                return GetKeysFromReturnFile(keyLog.FileContent);
            else
                if (keyLog.IsEncrypted)
                    return GetCryptKeysFromFile(keyLog.FileContent);
                else
                    return GetKeysFromFile(keyLog.FileContent);
        }

        private List<KeyInfo> GetToolKeysFromFile(string content)
        {
            return new List<KeyInfo> { content.FromXml<OA3ToolKeyInfo.Key>().ToKeyInfo() };
        }

        private List<KeyInfo> GetKeysFromFile(string content)
        {
            var exportKeys = content.FromXml<ExportKeyList>();
            if (exportKeys != null)
                return exportKeys.Keys.ToKeyInfo();
            return new List<KeyInfo>();
        }

        private List<KeyInfo> GetCryptKeysFromFile(string content)
        {
            var cryptExport = Serializer.FromXml<EncryptedExportKeyList>(content);
            var exportKeys = GetDecryptedFileKeys(cryptExport);
            if (exportKeys != null)
                return exportKeys.Keys.ToKeyInfo();
            return new List<KeyInfo>();
        }

        private List<KeyInfo> GetKeysFromCBRFile(string content)
        {
            var request = Serializer.FromXml<ComputerBuildReportRequest>(content);
            Cbr cbr = request.FromServiceContract();
            return cbr.CbrKeys.Select(k => keyRepository.GetKey(k.KeyId)).ToList();
        }

        private List<KeyInfo> GetKeysFromReturnFile(string content)
        {
            var request = Serializer.FromXml<ReturnRequest>(content);
            return GetKeysInDb(request.ReturnLineItems.Select(k => new KeyInfo()
            {
                KeyId = k.ProductKeyID
            }).ToList());
        }

        private void RecordExportLog(List<KeyInfo> keys, ExportParameters exportParameters, string fileContent, string fileName)
        {
            KeyExportLog exportLog = new KeyExportLog()
            {
                ExportTo = GetExportTo(exportParameters),
                KeyCount = keys.Count,
                IsEncrypted = exportParameters.IsEncrypted,
                ExportType = exportParameters.ExportType.ToString(),
                FileName = Path.GetFileName(fileName),
                FileContent = fileContent,
                CreateBy = exportParameters.CreateBy.LoginId,
                CreateDate = DateTime.Now
            };
            miscRepository.InsertExportLog(exportLog);
        }

        private void RecordExportLog(KeyInfo key, ExportParameters exportParameters, string fileContent, string fileName)
        {
            RecordExportLog(new List<KeyInfo>() { key }, exportParameters, fileContent, fileName);
        }


        #endregion

        private string keyExportMessageTranformationXSLT;
        public string GetKeyExportMessageTranformationXSLT() 
        {
            return this.keyExportMessageTranformationXSLT;
        }

        public void SetKeyExportMessageTranformationXSLT(string value) 
        {
            this.keyExportMessageTranformationXSLT = value;
        }

    }
}
