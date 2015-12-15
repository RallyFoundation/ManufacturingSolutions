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
using System.Data;
using System.Linq;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository {
    public class OhrRepository : RepositoryBase, IOhrRepository
    {
        public OhrRepository() : base()
        {

        }

        public OhrRepository(string ConnectionString) : base(ConnectionString)
        {

        }
        public List<Ohr> SearchConfirmedKeys()
        {
            using (var context = GetContext())
            {
                IQueryable<Ohr> query =
                    context.Ohrs.Include("OhrKeys");

                query = query.Where(c => c.OhrStatusId == (int)OhrStatus.Confirmed);
                query = query.Where(c => c.OhrKeys.Any(k => k.ReasonCode != "00"));   

                return query.ToList();
            }
        }

        public List<Ohr> SearchOhr(OhrStatus? status)
        {
            using (var context = GetContext()) {
                IQueryable<Ohr> query =
                    context.Ohrs.Include("OhrKeys");

                if (status != null)
                    query = query.Where(c => c.OhrStatusId == (int)status);

                return query.ToList();
            }
        }

        public void InsertOhr(Ohr ohr, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                ohr.CreatedDateUtc = ohr.ModifiedDateUtc = DateTime.UtcNow;
                context.Ohrs.Add(ohr);
                foreach (var key in ohr.OhrKeys)
                {
                    context.OhrKeys.Add(key);
                }
            });
        }

        public void UpdateOhr(Ohr ohr, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                ohr.ModifiedDateUtc = DateTime.UtcNow;
                UpdateOhr(context, ohr);
            });
        }

        public void UpdateOhrAck(Ohr ohr, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                UpdateOhr(context, ohr);

                foreach (OhrKey key in ohr.OhrKeys)
                {
                    context.OhrKeys.Attach(key);
                    context.Entry(key).State = EntityState.Modified;
                }
                
                context.Configuration.AutoDetectChangesEnabled = true;
            });
        }

        public Ohr GetOhrByCustomerUniqueId(Guid customerUniqueId)
        {
            using (var context = GetContext()) {
                return context.Ohrs.Include("OhrKeys").Where(r => r.CustomerUpdateUniqueId == customerUniqueId).SingleOrDefault();
            }
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }

        private void UpdateOhr(KeyStoreContext context, Ohr ohr)
        {
            context.Ohrs.Attach(ohr);
            context.Entry(ohr).State = EntityState.Modified;
        }
    }
}
