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
using System.Text;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository
{
    public class FulfillmentRepository : RepositoryBase, IFulfillmentRepository
    {
        public FulfillmentRepository() : base() 
        {
            
        }

        public FulfillmentRepository(string ConnectionString) : base(ConnectionString)
        {

        }
        public List<FulfillmentInfo> GetFulfillments(List<string> fulfillmentNumbers)
        {
            using (var context = GetContext())
            {
                return context.FulfillmentInfoes.Where(f => fulfillmentNumbers.Contains(f.FulfillmentNumber)).ToList();
            }
        }

        public FulfillmentInfo GetFirstFulfillment(FulfillmentStatus status)
        {
            using (var context = GetContext())
            {
                return context.FulfillmentInfoes.FirstOrDefault(i => i.FulfillmentStatusId == (byte)status);
            }
        }

        public List<FulfillmentInfo> GetFulfillments(FulfillmentStatus status, DateTime? modifiedFromUtc = null)
        {
            using (var context = GetContext())
            {
                IQueryable<FulfillmentInfo> query = context.FulfillmentInfoes
                    .Where(info => info.FulfillmentStatusId == (byte)status);
                if (modifiedFromUtc != null)
                    query = query.Where(f => f.ModifiedDateUtc > modifiedFromUtc.Value);
                return query.ToList();
            }
        }

        public void InsertFulfillments(List<FulfillmentInfo> infoes)
        {
            using (var context = GetContext())
            {
                foreach (FulfillmentInfo info in infoes)
                {
                    info.CreatedDateUtc = info.ModifiedDateUtc = DateTime.UtcNow;
                    context.FulfillmentInfoes.Add(info);
                }
                context.SaveChanges();
            }
        }

        public void UpdateFulfillment(FulfillmentInfo fulfillmentInfo, KeyStoreContext context = null)
        {
            UsingContext(ref context, () =>
            {
                FulfillmentInfo info = context.FulfillmentInfoes.Single(i => i.FulfillmentNumber == fulfillmentInfo.FulfillmentNumber);
                if (!ValidateStatusTransition(info.FulfillmentStatus, fulfillmentInfo.FulfillmentStatus))
                    throw new ApplicationException(string.Format("Cannot change fulfillment {0} status from {1} to {2}.",
                        info.FulfillmentNumber, info.FulfillmentStatus.ToString(), fulfillmentInfo.FulfillmentStatus.ToString()));

                context.Entry(info).CurrentValues.SetValues(fulfillmentInfo);
                info.ModifiedDateUtc = DateTime.UtcNow;
            });
        }

        private bool ValidateStatusTransition(FulfillmentStatus statusInDb, FulfillmentStatus statusToUpdate)
        {
            switch (statusInDb)
            {
                case FulfillmentStatus.Ready:
                    if (statusToUpdate == FulfillmentStatus.Failed
                        || statusToUpdate == FulfillmentStatus.InProgress)
                        return true;
                    break;
                case FulfillmentStatus.Fulfilled:
                    if (statusToUpdate == FulfillmentStatus.Completed)
                        return true;
                    break;
                case FulfillmentStatus.Failed:
                    if (statusToUpdate == FulfillmentStatus.Ready
                        || statusToUpdate == FulfillmentStatus.Fulfilled)
                        return true;
                    break;
                case FulfillmentStatus.InProgress:
                    if (statusToUpdate == FulfillmentStatus.Fulfilled
                        || statusToUpdate == FulfillmentStatus.Failed
                        || statusToUpdate == FulfillmentStatus.Ready)
                        return true;
                    break;
            }
            return false;
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }
    }
}
