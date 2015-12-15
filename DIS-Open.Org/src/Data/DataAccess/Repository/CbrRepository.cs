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
    public class CbrRepository : RepositoryBase, ICbrRepository 
    {
        public CbrRepository() : base() 
        {

        }

        public CbrRepository(string ConnectionString) : base(ConnectionString) 
        {

        }
        public List<Cbr> SearchCbrs(CbrSearchCriteria criteria) {
            using (var context = GetContext()) {
                IQueryable<Cbr> query = !criteria.IncludeKeyInfo ?
                    context.Cbrs.Include("CbrKeys") : (!criteria.IncludeCbrDuplicated ?
                        context.Cbrs.Include("CbrKeys.KeyInfo.KeyInfoEx") :
                        context.Cbrs.Include("CbrKeys.KeyInfo.KeyInfoEx").Include("CbrDuplicated"));

                if (criteria.CbrStatus != null)
                    query = query.Where(c => c.CbrStatusId == (int)criteria.CbrStatus);

                if (criteria.IsExported != null)
                    query = query.Where(c => c.CbrDuplicated.IsExported == criteria.IsExported);

                if (criteria.IncludeCbrDuplicated)
                    query = query.Where(c => c.CbrDuplicated != null);
                return query.ToList();
            }
        }

        public void InsertCbrAndCbrKeys(Cbr cbr, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                cbr.CreatedDateUtc = cbr.ModifiedDateUtc = DateTime.UtcNow;
                context.Cbrs.Add(cbr);
                foreach (var key in cbr.CbrKeys)
                {
                    context.CbrKeys.Add(key);
                }
            });
        }

        public void UpdateCbr(Cbr cbr, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                cbr.ModifiedDateUtc = DateTime.UtcNow;
                UpdateCbr(context, cbr);
            });
        }

        public void UpdateCbrAck(Cbr cbr, bool IsDuplicateImport = false, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                UpdateCbr(context, cbr);

                foreach (CbrKey key in cbr.CbrKeys)
                {
                    context.CbrKeys.Attach(key);
                    context.Entry(key).State = EntityState.Modified;
                }
                if (cbr.CbrKeys.Any(k => k.ReasonCode == Constants.CBRAckReasonCode.DuplicateProductKeyId) && !IsDuplicateImport)
                {
                    context.CbrsDuplicated.Add(new CbrDuplicated()
                    {
                        CbrUniqueId = cbr.CbrUniqueId,
                        IsExported = false
                    });
                }
                if (IsDuplicateImport)
                {
                    var delCbr = context.CbrsDuplicated.FirstOrDefault(c => c.CbrUniqueId == cbr.CbrUniqueId);
                    context.CbrsDuplicated.Remove(delCbr);
                }
                context.Configuration.AutoDetectChangesEnabled = true;
            });
        }

        public void UpdateCbrsDuplicated(Cbr cbr) {
            using (var context = GetContext()) {
                context.CbrsDuplicated.Where(c => c.CbrUniqueId == cbr.CbrUniqueId).ToList()
                    .ForEach(c => c.IsExported = true);
                context.SaveChanges();
            }
        }

        public void DeleteCbrsDuplicated(Cbr cbr)
        {
            using (var context = GetContext())
            {
                var delCbr = context.CbrsDuplicated.FirstOrDefault(c => c.CbrUniqueId == cbr.CbrUniqueId);
                context.CbrsDuplicated.Remove(delCbr);
                context.SaveChanges();
            }
        }

        public void UpdateCbrKeys(long[] keyIds) {
            using (var context = GetContext()) {
                List<long> ids = keyIds.ToList();
                context.CbrKeys.Where(k => ids.Contains(k.KeyId)).ToList()
                    .ForEach(k => k.ReasonCode = Constants.CBRAckReasonCode.ActivationEnabled);
                context.SaveChanges();
            }
        }

        public Cbr GetCbr(Guid customerUniqueId) {
            using (var context = GetContext()) {
                return context.Cbrs.Include("CbrKeys").Where(r => r.CbrUniqueId == customerUniqueId).SingleOrDefault();
            }
        }

        public CbrDuplicated GetDuplicatedCbr(Guid customerUniqueId)
        {
            using (var context = GetContext())
            {
                return context.CbrsDuplicated.FirstOrDefault(c => c.CbrUniqueId == customerUniqueId);
            }
        }

        private void UpdateCbr(KeyStoreContext context, Cbr cbr) {
            context.Cbrs.Attach(cbr);
            context.Entry(cbr).State = EntityState.Modified;
        }


        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }
    }
}
