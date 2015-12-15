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
    public class OhrManager : IOhrManager
    {
        private IOhrRepository ohrRepository;

        public OhrManager()
            : this(new OhrRepository())
        {
        }

        public OhrManager(string dbConnectionString) 
        {
            this.ohrRepository = new OhrRepository(dbConnectionString);
        }

        public OhrManager(IOhrRepository ohrRepository)
        {
            if (ohrRepository == null)
                this.ohrRepository = new OhrRepository();
            else
                this.ohrRepository = ohrRepository;
        }

        public List<Ohr> GetConfirmedOhrs()
        {
            return ohrRepository.SearchConfirmedKeys();
        }

        public void UpdateOhrAfterNotification(List<Ohr> ohrs)
        {
            foreach (Ohr o in ohrs)
            {
                o.OhrStatus = OhrStatus.ReadMark;
                ohrRepository.UpdateOhr(o);
            }
        }

        public List<Ohr> GetOhrsNotBeenSent()
        {
            return ohrRepository.SearchOhr(OhrStatus.Generated);
        }

        public List<Ohr> GetReportedOhrs()
        {
            return ohrRepository.SearchOhr(OhrStatus.Unconfirmed);
        }

        public List<Ohr> GetReadyOhrs()
        {
            return ohrRepository.SearchOhr(OhrStatus.Ready);
        }

        public List<Ohr> GetFailedOhrs()
        {
            return ohrRepository.SearchOhr(OhrStatus.Failed);
        }

        public Ohr GenerateOhr(List<KeyInfo> keys, KeyStoreContext context = null)
        {
            if (keys.Count == 0 || keys.Count > Constants.BatchLimit)
                throw new ArgumentOutOfRangeException("Keys are invalid to generate OHR.");
            string soldTo = keys.First().SoldToCustomerId;
            if (keys.Any(k => k.SoldToCustomerId != soldTo))
                throw new ApplicationException("Keys are not sold to the same customer.");
            string shipTo = keys.First().ShipToCustomerId;
            if (keys.Any(k => k.ShipToCustomerId != shipTo))
                throw new ApplicationException("Keys are not shipped to the same customer.");

            Guid customerReportId = Guid.NewGuid();
            Ohr ohr = new Ohr(keys)
            {
                CustomerUpdateUniqueId = customerReportId,
                OhrStatus = OhrStatus.Generated,
                SoldToCustomerId = soldTo,
                ReceivedFromCustomerId = shipTo,
            };
            ohrRepository.InsertOhr(ohr, context);
            return ohr;
        }

        public void UpdateOhrAfterReported(Ohr ohr, KeyStoreContext context)
        {
            if (!ohr.MsUpdateUniqueId.HasValue || ohr.MsUpdateUniqueId == Guid.Empty)
                throw new ArgumentException("OHR is invalid.");

            ohr.OhrStatus = OhrStatus.Unconfirmed;
            ohrRepository.UpdateOhr(ohr, context);
        }

        public void UpdateOhrsAfterAckReady(List<Ohr> ohrs)
        {
            if (ohrs.Any(c => c.OhrStatus != OhrStatus.Unconfirmed && c.OhrStatus != OhrStatus.Failed))
                throw new ArgumentException("OHRs are invalid.");

            foreach (Ohr o in ohrs)
            {
                o.OhrStatus = OhrStatus.Ready;
                ohrRepository.UpdateOhr(o);
            }
        }

        public string GenerateOhrToFile(List<KeyInfo> keys, string outputPath)
        {
            var ohr = GenerateOhr(keys);
            return KeyManagerHelper.SaveServiceContractToFile(ohr.ToDataUpdateRequest(), outputPath);
        }

        public Ohr UpdateOhrAfterAckRetrieved(Ohr ohr, KeyStoreContext context = null)
        {
            Ohr dbOhr = ohrRepository.GetOhrByCustomerUniqueId(ohr.CustomerUpdateUniqueId);

            if (dbOhr == null)
                throw new DisException("Failed to get data from database to match the contents of this file!");
            if (dbOhr.OhrStatus == OhrStatus.Confirmed)
                throw new DisException("OHRs ack has got and completed!");

            dbOhr.MsUpdateUniqueId = ohr.MsUpdateUniqueId;
            dbOhr.MsReceivedDateUtc = ohr.MsReceivedDateUtc;
            dbOhr.TotalLineItems = ohr.TotalLineItems;
            dbOhr.ModifiedDateUtc = DateTime.UtcNow;
            dbOhr.OhrStatus = OhrStatus.Confirmed;
            if (dbOhr.OhrKeys != null && ohr.OhrKeys != null)
            {
                foreach (var ohrKey in ohr.OhrKeys)
                {
                    foreach (var dbKey in dbOhr.OhrKeys)
                    {
                        if (ohrKey.KeyId == dbKey.KeyId && ohrKey.Name == dbKey.Name)
                        {
                            dbKey.ReasonCode = ohrKey.ReasonCode;
                            dbKey.ReasonCodeDescription = ohrKey.ReasonCodeDescription;
                        }
                    }
                }
            }

            ohrRepository.UpdateOhrAck(dbOhr);
            return dbOhr;
        }

        public Ohr RetrieveOhrAck(string path)
        {
            return RetrieveOhrFromFile(path);
        }

        public void UpdateOhrWhenAckFailed(Ohr ohr)
        {
            ohr.OhrStatus = OhrStatus.Failed;
            ohrRepository.UpdateOhr(ohr);
        }
   
        private Ohr RetrieveOhrFromFile(string path)
        {
            try
            {
                XElement doc = XElement.Load(path);
                var response = Serializer.FromDataContract<DataUpdateAck>(doc.ToString());
                return response.FromServiceContract();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.ohrRepository.GetDBConnectionString());
                throw new DisException(Resources.Exception_ImportFileInvalid);
            }
        }
    }
}
