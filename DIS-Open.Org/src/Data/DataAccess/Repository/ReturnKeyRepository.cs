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
using System.Data;

namespace DIS.Data.DataAccess.Repository
{
    public class ReturnKeyRepository : RepositoryBase, IReturnKeyRepository
    {
        public ReturnKeyRepository() : base() 
        {

        }

        public ReturnKeyRepository(string ConnectionString) : base(ConnectionString)
        {

        }
        public List<ReturnReport> SearchReturnKeys(ReturnKeySearchCriteria criteria)
        {
            using (var context = GetContext())
            {
                IQueryable<ReturnReport> query = context.ReturnReports.Include("ReturnReportKeys");
                if (criteria.ReturnUniqueID != null)
                    query = query.Where(c => c.ReturnUniqueId == criteria.ReturnUniqueID);
                if (!string.IsNullOrEmpty(criteria.OEMRMANumber))
                    query = query.Where(c => c.OemRmaNumber == criteria.OEMRMANumber);
                if (criteria.ReturnReportStatus != null)
                    query = query.Where(c => c.ReturnReportStatusId == (int)criteria.ReturnReportStatus);
                return query.ToList();
            }
        }

        public void InsertReturnReportAndKeys(ReturnReport returnKey)
        {
            using (var context = GetContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.ReturnReports.Add(returnKey);
                foreach (var key in returnKey.ReturnReportKeys)
                {
                    context.ReturnReportKeys.Add(key);
                }
                context.SaveChanges();
            }
        }

        public void UpdateReturnReport(ReturnReport returnKey, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                UpdateReturnKeyAck(context, returnKey);
            });
        }

        public void UpdateReturnKeyAck(ReturnReport returnKey, KeyStoreContext context)
        {
            UsingContext(ref context, () =>
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                UpdateReturnKeyAck(context, returnKey);

                if (returnKey.ReturnReportKeys != null)
                {
                    foreach (ReturnReportKey key in returnKey.ReturnReportKeys)
                    {
                        context.ReturnReportKeys.Attach(key);
                        context.Entry(key).State = EntityState.Modified;
                    }
                }
            });
        }

        public ReturnReport GetReturnKey(Guid customerReportUniqueId, KeyStoreContext context)
        {
            return context.ReturnReports.Include("ReturnReportKeys").Where(r => r.ReturnUniqueId == customerReportUniqueId).SingleOrDefault();
        }

        public ReturnReport GetReturnKeyByCustomerId(Guid customerReportUniqueId)
        {
            using (var context = GetContext())
            {
                return context.ReturnReports.Include("ReturnReportKeys").Where(r => r.CustomerReturnUniqueId == customerReportUniqueId).SingleOrDefault();
            }
        }

        public ReturnReport GetReturnKeyByOneKeyID(long keyId, KeyStoreContext context)
        {
            return context.ReturnReports.Include("ReturnReportKeys").Where(r => r.ReturnReportKeys.Where(s => s.KeyId == keyId && s.ReturnReasonCode == null).Count() > 0).SingleOrDefault();
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }

        private void UpdateReturnKeyAck(KeyStoreContext context, ReturnReport returnKey)
        {
            context.ReturnReports.Attach(returnKey);
            context.Entry(returnKey).State = EntityState.Modified;
        }
    }
}
