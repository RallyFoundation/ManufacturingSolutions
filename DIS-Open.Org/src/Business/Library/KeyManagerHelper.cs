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
using System.Linq;
using System.Text;
using DIS.Data.DataContract;
using DIS.Data.ServiceContract;
using OA3ToolKeyInfo = DIS.Data.DataContract.OA3ToolKeyInfo;
using OA3ToolReportKeyInfo = DIS.Data.DataContract.OA3ToolReportKeyInfo;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography.X509Certificates;
using DIS.Common.Utility;
using System.Configuration;
using System.Security.Cryptography;
using DIS.Business.Library.Properties;
using System.Runtime.Serialization;

namespace DIS.Business.Library
{
    internal static class KeyManagerHelper
    {
        public static string DefaultToolKeyFolderName = "keyShare";
        public static string DefaultToolKeyAllocateFolderName = "Allocated";
        public static string DefaultToolKeyUsedFolderName = "Used";
        public static string ToolReportkeyFormat = "OA3ToolReportKey2.xsd";
        public static string EncryptedExportKeyFormat = "ExportKey_Encrypted.xsd";
        public static string EncryptedExportKeyFormat_R4 = "ExportKey_Encrypted_R4.xsd";
        public static string NonEncryptedExportKeyFormat = "ExportKeys_nonEncrypted.xsd";
        public static string NonEncryptedExportKeyFormat_R5 = "ExportKeys_nonEncrypted_R5.xsd";

        public static string InternalCertThumbPrint
        {
            get { return ConfigurationManager.AppSettings["CertificateThumbprint"]; }
        }

        public static List<KeyInfo> ToKeyInfo(this List<ExportKeyInfo> keys)
        {
            return keys.Select(k => k.ToKeyInfo()).ToList();
        }

        public static KeyInfo ToKeyInfo(this ExportKeyInfo key)
        {
            return new KeyInfo(key.ProductKeyState)
            {
                KeyId = key.ProductKeyId,
                ProductKey = key.ProductKey,
                HardwareHash = key.HardwareHash,
                OemOptionalInfo = (key.OEMOptionalInfo == null || key.OEMOptionalInfo.Count <= 0) ? null : new OemOptionalInfo(key.OEMOptionalInfo),
                SkuId = key.SKUID,
                OrderUniqueId = key.OrderUniqueID,
                MsOrderNumber = key.MSOrderNumber,
                MsOrderLineNumber = key.MSOrderLineNumber,
                OemPartNumber = key.OEMPartNumber,
                OemPoNumber = key.OEMPONumber,
                OemPoDateUtc = key.OEMPODateUTC,
                SoldToCustomerId = key.SoldToCustomerID,
                SoldToCustomerName = key.SoldToCustomerName,
                ShipToCustomerId = key.ShipToCustomerID,
                ShipToCustomerName = key.ShipToCustomerName,
                CallOffReferenceNumber = key.CallOffReferenceNumber,
                LicensablePartNumber = key.LicensablePartNumber,
                LicensableName = key.LicensableName,
                OemPoLineNumber = key.OEMPOLineNumber,
                CallOffLineNumber = key.CallOffLineNumber,
                FulfillmentResendIndicator = key.FulfillmentResendIndicator,
                FulfillmentNumber = key.FulfillmentNumber,
                FulfilledDateUtc = key.FulfilledDateUTC,
                FulfillmentCreateDateUtc = key.FulfillmentCreateDateUTC,
                EndItemPartNumber = key.EndItemPartNumber,
                TrackingInfo = key.TrackingInfo,
                SerialNumber = key.SerialNumber,
                KeyInfoEx = new KeyInfoEx() { KeyType = (key.KeyType == null ? KeyType.Standard : (KeyType)key.KeyType) }
            };
        }

        public static List<ExportKeyInfo> ToExportFulfilledKey(this List<KeyInfo> keys)
        {
            return keys.Select(k => k.ToExportFulfilledKey()).ToList();
        }

        public static ExportKeyInfo ToExportFulfilledKey(this KeyInfo key)
        {
            return new ExportKeyInfo()
            {
                ProductKeyId = key.KeyId,
                ProductKey = key.ProductKey,
                ProductKeyState = key.KeyState,
                HardwareHash = key.HardwareHash,
                OEMOptionalInfo = key.OemOptionalInfo == null ? null : key.OemOptionalInfo.ToFields(),
                SKUID = key.SkuId,
                OrderUniqueID = key.OrderUniqueId,
                MSOrderNumber = key.MsOrderNumber,
                MSOrderLineNumber = key.MsOrderLineNumber,
                OEMPartNumber = key.OemPartNumber,
                OEMPONumber = key.OemPoNumber,
                OEMPODateUTC = key.OemPoDateUtc,
                SoldToCustomerID = key.SoldToCustomerId,
                SoldToCustomerName = key.SoldToCustomerName,
                ShipToCustomerID = key.ShipToCustomerId,
                ShipToCustomerName = key.ShipToCustomerName,
                CallOffReferenceNumber = key.CallOffReferenceNumber,
                LicensablePartNumber = key.LicensablePartNumber,
                LicensableName = key.LicensableName,
                OEMPOLineNumber = key.OemPoLineNumber,
                CallOffLineNumber = key.CallOffLineNumber,
                FulfillmentResendIndicator = key.FulfillmentResendIndicator,
                FulfillmentNumber = key.FulfillmentNumber,
                FulfilledDateUTC = key.FulfilledDateUtc,
                FulfillmentCreateDateUTC = key.FulfillmentCreateDateUtc,
                EndItemPartNumber = key.EndItemPartNumber,
                TrackingInfo=key.TrackingInfo,
                SerialNumber = key.SerialNumber,
                KeyType =key.KeyInfoEx.KeyTypeId
            };
        }

        public static KeyInfo ToKeyInfo(this OA3ToolKeyInfo.Key key)
        {
            return new KeyInfo((KeyState)key.ProductKeyState)
            {
                KeyId = key.ProductKeyID,
                ProductKey = key.ProductKey,
                OemPartNumber = key.ProductKeyPartNumber
            };
        }

        public static OA3ToolKeyInfo.Key ToToolKey(this KeyInfo key)
        {
            return new OA3ToolKeyInfo.Key()
            {
                ProductKey = key.ProductKey,
                ProductKeyID = key.KeyId,
                ProductKeyState = (byte)KeyState.Consumed,
                ProductKeyPartNumber = key.OemPartNumber,
                //To supoort serial number mapping - Rally Sept. 22, 2014
                SerialNumber = key.SerialNumber
            };
        }

        public static void CreateFilePath(string filePath, string dbConnectionString)
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
            catch (AccessViolationException ex)
            {
                ExceptionHandler.HandleException(ex, dbConnectionString);
                throw new DisException("Exception_PermissionsMsg");
            }
            catch (DirectoryNotFoundException ex)
            {
                ExceptionHandler.HandleException(ex, dbConnectionString);
                throw new DisException("Exception_FilePathNotFound");
            }
            catch (ArgumentException ex)
            {
                ExceptionHandler.HandleException(ex, dbConnectionString);
                throw new DisException("Exception_InvalidPath");
            }
        }

        public static void CreateDirectory(string directory, string dbConnectionString)
        {
            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
            catch (AccessViolationException ex)
            {
                ExceptionHandler.HandleException(ex, dbConnectionString);
                throw new DisException("Exception_PermissionsMsg");
            }
            catch (DirectoryNotFoundException ex)
            {
                ExceptionHandler.HandleException(ex, dbConnectionString);
                throw new DisException("Exception_FilePathNotFound");
            }
            catch (ArgumentException ex)
            {
                ExceptionHandler.HandleException(ex, dbConnectionString);
                throw new DisException("Exception_InvalidPath");
            }

        }

        public static XDocument ParseAndValidateXML(string parameters, string schema, out int errorCount, out string errorInfo) //Rally Dec. 22, 2014
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
                "DIS.Business.Library.Schema." + schema));
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

        public static X509Certificate2 GetInternalCertificate()
        {
            try
            {
                return EncryptionHelper.GetCertificate(StoreName.My,StoreLocation.LocalMachine,X509FindType.FindByThumbprint, InternalCertThumbPrint);
            }
            catch (FileNotFoundException)
            {
                throw new DisException("Exception_CertificateNotFound");
            }
        }

        public static EncryptedExportKeyList EncryptExportFile(string productKeysXml)
        {
            X509Certificate2 certificate = GetInternalCertificate();
            byte[] productKeysData = Constants.DefaultEncoding.GetBytes(productKeysXml);
            byte[] key;
            byte[] iv;
            byte[] encryptedData = EncryptionHelper.AesEncrypt(productKeysData, out key, out iv);
            using (RSACryptoServiceProvider provider = (RSACryptoServiceProvider)certificate.PublicKey.Key)
            {
                EncryptedExportKeyList export = new EncryptedExportKeyList()
                {
                    Key = Convert.ToBase64String(EncryptionHelper.RsaEncrypt(key, provider)),
                    IV = Convert.ToBase64String(EncryptionHelper.RsaEncrypt(iv, provider)),
                    ProductKeys = Convert.ToBase64String(encryptedData)
                };
                return export;
            }
        }

        public static string GetXmlResult(object export)
        {
            return Serializer.ToXml(export).Replace("encoding=\"utf-8\"", "encoding=\"utf-16\"");
        }

        public static List<KeyInfo> ToSyncServiceContract(this List<KeyInfo> keys)
        {
            return keys.Select(key => new KeyInfo() { KeyId = key.KeyId }).ToList();
        }

        public static string SaveServiceContractToFile(object obj, string outputPath)
        {
            string xml = obj.ToDataContract();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            xDoc.Save(outputPath);
            return xDoc.DocumentElement.OuterXml;
        }
    }
}
