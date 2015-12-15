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
using System.Data.Objects;
using System.Linq;
using System.Text;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository
{
    public class SubsidiaryRepository : RepositoryBase, ISubsidiaryRepository
    {
        public SubsidiaryRepository() : base() 
        {

        }

        public SubsidiaryRepository(string ConnectionString) : base(ConnectionString)
        {

        }

        #region Public Method

        public Subsidiary GetSubsidiary(int ssId)
        {
            using (var context = GetContext())
            {
                return GetSubsidiaryById(context, ssId);
            }
        }

        public Subsidiary GetSubsidiary(string userName)
        {
            using (var context = GetContext())
            {
                return context.Subsidiaries.Single(ss => ss.UserName == userName);
            }
        }

        public List<Subsidiary> GetSubsidiaries(string userName, string userKey)
        {
            using (var context = GetContext())
            {
                return context.Subsidiaries.Where(ss => ss.UserName == userName && ss.AccessKey == userKey).ToList();
            }
        }

        public List<Subsidiary> GetSubsidiaries()
        {
            using (var context = GetContext())
            {
                return context.Subsidiaries.ToList();
            }
        }

        public void InsertSubsidiary(Subsidiary sub)
        {
            using (var context = GetContext())
            {
                context.Subsidiaries.Add(sub);
                context.SaveChanges();
            }
        }

        public void UpdateSubsidiary(Subsidiary subsidiary)
        {
            using (var context = GetContext())
            {
                Subsidiary subToUpdate = GetSubsidiaryById(context, subsidiary.SsId);
                if (subToUpdate == null)
                    throw new ApplicationException(string.Format("Subsidiary '{0}' cannot be found.", subsidiary.SsId));
                context.Entry(subToUpdate).CurrentValues.SetValues(subsidiary);
                context.SaveChanges();
            }
        }

        public void DeleteSubsidiary(int ssId)
        {
            using (var context = GetContext())
            {
                Subsidiary subToDelete = GetSubsidiaryById(context, ssId);
                if (subToDelete == null)
                    throw new ApplicationException(string.Format("Subsidiary '{0}' cannot be found.", ssId));
                context.Subsidiaries.Remove(subToDelete);
                context.SaveChanges();
            }
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }

        #endregion

        #region Private Method

        private Subsidiary GetSubsidiaryById(KeyStoreContext context, int ssId)
        {
            return context.Subsidiaries.SingleOrDefault(s => s.SsId == ssId);
        }

        #endregion
    }
}
