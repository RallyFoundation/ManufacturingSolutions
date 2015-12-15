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
using System.Text;
using System.Xml;
using System.Xml.Linq;
using DIS.Business.Library.Properties;
using DIS.Common.Utility;
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataContract;
using DIS.Data.ServiceContract;
using DC = DIS.Data.DataContract;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    public class CbrManager : ICbrManager
    {
        private ICbrRepository cbrRepository;

        public CbrManager()
            : this(new CbrRepository())
        {
        }

        public CbrManager(string dbConnectionString)
        {
            this.cbrRepository = new CbrRepository(dbConnectionString);
        }

        public CbrManager(ICbrRepository cbrRepository)
        {
            if (cbrRepository == null)
                this.cbrRepository = new CbrRepository();
            else
                this.cbrRepository = cbrRepository;
        }

        public List<Cbr> GetCbrsNotBeenSent(bool includeKeyInfo = false)
        {
            CbrSearchCriteria criteria = new CbrSearchCriteria()
            {
                CbrStatus = CbrStatus.Generated,
                IncludeKeyInfo = includeKeyInfo
            };
            return cbrRepository.SearchCbrs(criteria);
        }

        public Cbr GetFirstSentCbr()
        {
            CbrSearchCriteria criteria = new CbrSearchCriteria()
            {
                CbrStatus = CbrStatus.Sent,
            };
            return cbrRepository.SearchCbrs(criteria).FirstOrDefault();
        }

        public List<Cbr> GetReportedCbrs()
        {
            CbrSearchCriteria criteria = new CbrSearchCriteria()
            {
                CbrStatus = CbrStatus.Reported
            };
            return cbrRepository.SearchCbrs(criteria);
        }

        public List<Cbr> GetReadyCbrs()
        {
            CbrSearchCriteria criteria = new CbrSearchCriteria()
            {
                CbrStatus = CbrStatus.Ready
            };
            return cbrRepository.SearchCbrs(criteria);
        }

        public List<Cbr> GetFailedCbrs()
        {
            CbrSearchCriteria criteria = new CbrSearchCriteria()
            {
                CbrStatus = CbrStatus.Failed,
            };
            return cbrRepository.SearchCbrs(criteria);
        }

        public Cbr GenerateCbr(List<KeyInfo> keys, bool isExport = false, KeyStoreContext context = null)
        {
            if (keys.Count == 0 || keys.Count > Constants.BatchLimit)
                throw new ArgumentOutOfRangeException("Keys are invalid to generate CBR.");
            string soldTo = keys.First().SoldToCustomerId;
            if (keys.Any(k => k.SoldToCustomerId != soldTo))
                throw new ApplicationException("Keys are not sold to the same customer.");
            string shipTo = keys.First().ShipToCustomerId;
            if (keys.Any(k => k.ShipToCustomerId != shipTo))
                throw new ApplicationException("Keys are not shipped to the same customer.");
            
            keys.ForEach(k => CheckCbrTouchScreenValue(k));

            Guid customerReportId = Guid.NewGuid();
            Cbr cbr = new Cbr()
            {
                CbrUniqueId = customerReportId,
                CbrStatus = (isExport ? CbrStatus.Reported : CbrStatus.Generated),
                SoldToCustomerId = soldTo,
                ReceivedFromCustomerId = shipTo,
                CbrKeys = keys.Select(k => new CbrKey()
                {
                    CustomerReportUniqueId = customerReportId,
                    KeyId = k.KeyId,
                    HardwareHash = k.HardwareHash,
                    OemOptionalInfo = k.OemOptionalInfo.ToString(),
                }).ToList()
            };
            cbrRepository.InsertCbrAndCbrKeys(cbr, context);
            return cbr;
        }

        public string GenerateCbrToFile(List<KeyInfo> keys, string outputPath)
        {
            var cbr = GenerateCbr(keys, true);
            return KeyManagerHelper.SaveServiceContractToFile(cbr.ToBindingReport(), outputPath);
        }

        public void UpdateCbrIfSendingFailed(Cbr cbr)
        {
            cbr.CbrStatus = CbrStatus.Sent;
            cbrRepository.UpdateCbr(cbr);
        }

        public void UpdateCbrIfSearchResultEmpty(Cbr cbr)
        {
            cbr.CbrStatus = CbrStatus.Generated;
            cbrRepository.UpdateCbr(cbr);
        }

        public void UpdateCbrAfterReported(Cbr cbr, KeyStoreContext context)
        {
            if (!cbr.MsReportUniqueId.HasValue || cbr.MsReportUniqueId == Guid.Empty)
                throw new ArgumentException("CBR is invalid.");

            cbr.CbrStatus = CbrStatus.Reported;
            cbrRepository.UpdateCbr(cbr, context);
        }

        public void UpdateCbrsAfterAckReady(List<Cbr> cbrs)
        {
            if (cbrs.Any(c => c.CbrStatus != CbrStatus.Reported && c.CbrStatus != CbrStatus.Failed))
                throw new ArgumentException("CBRs are invalid.");

            foreach (Cbr c in cbrs)
            {
                c.CbrStatus = CbrStatus.Ready;
                cbrRepository.UpdateCbr(c);
            }
        }

        public void UpdateCbrAfterAckRetrieved(Cbr cbr, bool isDuplicated = false, KeyStoreContext context = null)
        {
            Cbr dbCbr = cbrRepository.GetCbr(cbr.CbrUniqueId);

            if (dbCbr == null)
                throw new DisException("Failed to get data from database to match the contents of this file!");
            if (dbCbr.CbrStatus == CbrStatus.Completed && !isDuplicated)
                throw new DisException("CBRs ack has got and completed!");
            if (isDuplicated)
            {
                var duplicatedLog = cbrRepository.GetDuplicatedCbr(cbr.CbrUniqueId);
                if (duplicatedLog == null)
                    throw new DisException("This duplicated cbr is not logged in the system !");
            }

            dbCbr.MsReportUniqueId = cbr.MsReportUniqueId;
            dbCbr.MsReceivedDateUtc = cbr.MsReceivedDateUtc;
            dbCbr.CbrAckFileTotal = cbr.CbrAckFileTotal;
            dbCbr.CbrAckFileNumber = cbr.CbrAckFileNumber;
            dbCbr.ModifiedDateUtc = DateTime.UtcNow;
            dbCbr.CbrStatus = CbrStatus.Completed;
            if (dbCbr.CbrKeys != null && dbCbr.CbrKeys.Count > 0)
            {
                Func<CbrKey, CbrKey, CbrKey> updateCbrKey = (k1, k2) =>
                {
                    k1.ReasonCode = k2.ReasonCode;
                    k1.ReasonCodeDescription = k2.ReasonCodeDescription;
                    return k1;
                };
                var update = (from db in dbCbr.CbrKeys
                              join key in cbr.CbrKeys on db.KeyId equals key.KeyId
                              select updateCbrKey(db, key)).ToList();
            }
            cbrRepository.UpdateCbrAck(dbCbr, isDuplicated, context);
        }

        public void UpdateCbrWhenAckFailed(Cbr cbr)
        {
            cbr.CbrStatus = CbrStatus.Failed;
            cbrRepository.UpdateCbr(cbr);
        }

        public List<Cbr> GetCbrsDuplicated()
        {
            CbrSearchCriteria criteria = new CbrSearchCriteria()
            {
                CbrStatus = CbrStatus.Completed,
                IncludeKeyInfo = true,
                IncludeCbrDuplicated = true
            };
            return cbrRepository.SearchCbrs(criteria);
        }

        public void UpdateCbrsAfterExported(Cbr cbr)
        {
            if (cbr == null)
                throw new ArgumentNullException("CBR cannot be empty.");

            cbrRepository.UpdateCbrsDuplicated(cbr);
        }

        public void UpdateCbrsAfterImported(Cbr cbr)
        {
            if (cbr == null)
                throw new ArgumentNullException("Key IDs cannot be empty.");

            cbrRepository.DeleteCbrsDuplicated(cbr);
        }

        public Cbr RetrieveCbrAck(string path)
        {

            var cbr = RetrieveCbrFromFile(path);
            return cbr;
        }

        private Cbr RetrieveCbrFromFile(string path)
        {
            try
            {
                XElement doc = XElement.Load(path);
                string xmldoc = ConvertCBRFileFormat(doc.ToString());
                var response = Serializer.FromDataContract<ComputerBuildReportAck>(xmldoc);
                return response.FromServiceContract();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.cbrRepository.GetDBConnectionString());
                throw new DisException(Resources.Exception_ImportFileInvalid);
            }
        }

        private string ConvertCBRFileFormat(string xmldoc)
        {
            if (xmldoc.Contains("ComputerBuildReportAckResponse"))
            {
                if (xmldoc.Contains("ComputerBuildReportAcks"))
                {
                    xmldoc = xmldoc.Replace("<ComputerBuildReportAcks>", "");
                    xmldoc = xmldoc.Replace("</ComputerBuildReportAcks>", "");
                }
                if (xmldoc.Contains("ComputerBuildReportAck"))
                {
                    xmldoc = xmldoc.Replace("<ComputerBuildReportAck>", "");
                    xmldoc = xmldoc.Replace("</ComputerBuildReportAck>", "");
                }
                xmldoc = xmldoc.Replace("ComputerBuildReportAckResponse", "ComputerBuildReportAck");
            }
            return xmldoc;
        }

        private void CheckCbrTouchScreenValue(KeyInfo key)
        {
            if (!string.IsNullOrEmpty(key.ZTOUCH_SCREEN))
            {
                try
                {
                    OemOptionalInfo.ConvertTouchEnum(key.ZTOUCH_SCREEN);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
