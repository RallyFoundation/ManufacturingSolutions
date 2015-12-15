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
using DIS.Common.Utility;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using DIS.Data.ServiceContract;
using DIS.Business.Library.Properties;

namespace DIS.Business.Library
{
    public partial class LocalKeyManager : ILocalKeyManager
    {
        private const string XML = "XML";

        public void HandleKeysDuplicated(List<KeyDuplicated> keys, string @operator, string message)
        {
            foreach (KeyDuplicated keyDup in keys)
            {
                KeyInfo keyInDb = keyRepository.GetKey(keyDup.KeyId);
                if (keyDup.ReuseOperation == ReuseOperation.Reuse)
                {
                    if ((KeyState)keyInDb.KeyStateId != KeyState.NotifiedBound)
                    {
                        MessageLogger.LogSystemError(MessageLogger.GetMethodName(), "DuplicatedKey state is invalid.", this.keyRepository.GetDBConnectionString());
                        continue;
                    }
                    keyRepository.UpdateKey(keyInDb, null, null, null, null);
                }
                keyDup.Handled = true;
                miscRepository.InsertKeyOperationHistories(new List<KeyInfo> { keyInDb }, keyDup.NewState, @operator, message);
                miscRepository.UpdateKeysDuplicated(new List<KeyDuplicated> { keyDup });
            }
        }

        public List<KeyDuplicated> GetKeysDuplicated()
        {
            return miscRepository.GetKeysDuplicated();
        }

        public List<KeyOperationResult> ExportDuplicatedCbr(Cbr cbr, string outputPath, string @operator)
        {
            var fileExt = Path.GetExtension(outputPath);
            if (!fileExt.EndsWith(XML, StringComparison.OrdinalIgnoreCase))
                throw new Exception("KeyProxy_CBRExportFormatNotSupported");
            try
            {
                string doc = SaveDuplicatedCbrToFile(cbr, outputPath);
                InsertExportLog(new KeyExportLog()
                {
                    ExportTo = string.Empty,
                    ExportType = Constants.ExportType.DuplicateCBR.ToString(),
                    FileName = Path.GetFileName(outputPath),
                    FileContent = doc,
                    KeyCount = cbr.CbrKeys.Count(
                        k => k.ReasonCode == Constants.CBRAckReasonCode.DuplicateProductKeyId),
                    IsEncrypted = false,
                    CreateBy = @operator
                });
                return cbr.CbrKeys.Select(k => new KeyOperationResult()
                {
                    Failed = false,
                    Key = k.KeyInfo ?? new KeyInfo() { KeyId = k.KeyId }
                }).ToList();
            }
            catch (DirectoryNotFoundException ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                throw new DisException("Exception_FilePathNotFound");
            }
        }

        public List<KeyOperationResult> ImportDuplicatedCbr(string outputPath, Action<long[]> action)
        {
            string fileExt = Path.GetExtension(outputPath);
            if (!fileExt.EndsWith(XML, StringComparison.OrdinalIgnoreCase))
                throw new Exception("KeyProxy_CBRImportFormatNotSupported");
            else
            {         

                long[] keyIds = GetKeyIdFromDuplicatedCbrFile(outputPath);
                bool failed = false;
                try
                {
                    action(keyIds);
                }
                catch (Exception)
                {
                    failed = true;
                }
                return keyIds.Select(k => new KeyOperationResult()
                {
                    Failed = failed,
                    Key = new KeyInfo() { KeyId = k }
                }).ToList();
            }
        }

        public List<KeyOperationHistory> SearchOperationHistories(KeySearchCriteria criteria)
        {
            throw new NotImplementedException();
        }

        private long[] GetKeyIdFromDuplicatedCbrFile(string outputPath)
        {
            try
            {
                XNamespace ms = "http://schemas.ms.it.oem/digitaldistribution/2010/10";
                XElement doc = XElement.Load(outputPath);
                long[] keyIds = (from x in doc.Element(ms + "Bindings").Elements(ms + "Binding")
                                 select long.Parse(x.Element(ms + "ProductKeyID").Value)).ToArray();
                return keyIds;
            }
            catch
            {
                throw new Exception("Exception_ImportFileInvalid");
            }
        }

        private string SaveDuplicatedCbrToFile(Cbr cbr, string outputPath)
        {
            //XNamespace ms = "http://schemas.ms.it.oem/digitaldistribution/2010/10";
            //XElement doc = new XElement(ms + "ComputerBuildReportAckResponse",
            //    new XElement(ms + "ComputerBuildReportAcks",
            //        from cbr in cbrs
            //        select new XElement(ms + "ComputerBuildReportAck",
            //            new XElement(ms + "MSReportUniqueID", cbr.CbrUniqueId),
            //            new XElement(ms + "CustomerReportUniqueID", cbr.CbrUniqueId),
            //            new XElement(ms + "MSReceivedDateUTC", cbr.MsReceivedDateUtc),
            //            new XElement(ms + "SoldToCustomerID", cbr.SoldToCustomerId),
            //            new XElement(ms + "ReceivedFromCustomerID", cbr.ReceivedFromCustomerId),
            //            new XElement(ms + "CBRAckFileTotal", cbr.CbrAckFileTotal),
            //            new XElement(ms + "CBRAckFileNumber", cbr.CbrAckFileNumber),
            //            new XElement(ms + "FailedValidations",
            //                from failedKey in cbr.CbrKeys.Where(k => k.ReasonCode == Constants.CBRAckReasonCode.DuplicateProductKeyId)
            //                select new XElement(ms + "FailedValidationResult",
            //                    new XElement(ms + "ProductKeyID", failedKey.KeyId),
            //                    new XElement(ms + "HardwareHash", failedKey.HardwareHash),
            //                    new XElement(ms + "ReasonCode", failedKey.ReasonCode),
            //                    new XElement(ms + "ReasonCodeDescription", failedKey.ReasonCodeDescription))))));
            //doc.Save(outputPath);
            Serializer.WriteToXml(cbr.ToBindingReport(), outputPath);
            string fileName = Path.GetFileName(outputPath);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(outputPath);
            return xDoc.DocumentElement.OuterXml;
        }
    }
}
