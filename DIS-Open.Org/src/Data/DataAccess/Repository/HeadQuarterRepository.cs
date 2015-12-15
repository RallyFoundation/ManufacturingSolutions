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
    public class HeadQuarterRepository : RepositoryBase, IHeadQuarterRepository
    {
        public HeadQuarterRepository() : base() 
        {

        }

        public HeadQuarterRepository(string ConnectionString) : base(ConnectionString) 
        {

        }

        public List<HeadQuarter> GetHeadQuarters()
        {
            using (var context = GetContext())
            {
                return context.HeadQuarters.ToList();
            }
        }

        public List<HeadQuarter> GetHeadQuarters(User user)
        {
            using (var context = GetContext())
            {
                return context.UserHeadQuarters.Include("HeadQuarter").Where(m => m.UserId == user.UserId)
                    .ToList().Select(m => m.HeadQuarter).ToList();
            }
        }

        public List<HeadQuarter> GetHeadQuarters(string userName, string userKey)
        {
            using (var context = GetContext())
            {
                return context.HeadQuarters.Where(hq => hq.UserName == userName && hq.AccessKey == userKey).ToList();
            }
        }

        public HeadQuarter GetHeadQuarter(int hqId)
        {
            using (var context = GetContext())
            {
                return context.HeadQuarters.SingleOrDefault(h => h.HeadQuarterId == hqId);
            }
        }

        public HeadQuarter GetHeadQuarter(string userName)
        {
            using (var context = GetContext())
            {
                return context.HeadQuarters.Single(h => h.UserName == userName);
            }
        }

        public void InsertHeadQuarter(HeadQuarter headQuarter)
        {
            using (var context = GetContext())
            {
                context.HeadQuarters.Add(headQuarter);
                if (Constants.IsMultipleEnabled)
                {
                    var users = context.Roles.Single(r => r.RoleName == Constants.ManagerRoleName).Users.ToList();
                    foreach (User u in users)
                    {
                        context.UserHeadQuarters.Add(new UserHeadQuarter()
                        {
                            HeadQuarter = headQuarter,
                            UserId = u.UserId,
                            IsDefault = (u.UserHeadQuarters.Count() == 0)
                        });
                    }
                }
                else
                {
                    foreach (User u in context.Users)
                    {
                        context.UserHeadQuarters.Add(new UserHeadQuarter()
                        {
                            HeadQuarter = headQuarter,
                            User = u,
                            IsDefault = true
                        });
                    }
                }
                context.SaveChanges();
            }
        }

        public void UpdateHeadQuarter(HeadQuarter headQuarter)
        {
            using (var context = GetContext())
            {
                var hq = GetHeadQuarter(context, headQuarter.HeadQuarterId);
                if (hq == null)
                    throw new ApplicationException(string.Format("Head quarter '{0}' cannot be found.", headQuarter.HeadQuarterId));
                context.Entry(hq).CurrentValues.SetValues(headQuarter);
                context.SaveChanges();
            }
        }

        public void DeleteHeadQuarter(int headQuarterId)
        {
            using (var context = GetContext())
            {
                var hq = GetHeadQuarter(context, headQuarterId);
                if (hq == null)
                    throw new ApplicationException(string.Format("Head quarter '{0}' cannot be found.", headQuarterId));

                context.UserHeadQuarters
                    .Where(m => m.HeadQuarterId == headQuarterId).ToList()
                    .ForEach(h => context.UserHeadQuarters.Remove(h));
                context.HeadQuarters.Remove(hq);
                context.SaveChanges();
            }
        }

        public void UpdateUserHeadQuarter(User user, HeadQuarter headQuarter)
        {
            using (var context = GetContext())
            {
                context.UserHeadQuarters
                    .Where(u => u.UserId == user.UserId).ToList()
                    .ForEach(hq => hq.IsDefault = (hq.HeadQuarterId == headQuarter.HeadQuarterId));
                context.SaveChanges();
            }
        }

        private HeadQuarter GetHeadQuarter(KeyStoreContext context, int id)
        {
            return context.HeadQuarters.SingleOrDefault(hq => hq.HeadQuarterId == id);
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }
    }
}
