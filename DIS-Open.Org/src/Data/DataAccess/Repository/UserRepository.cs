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
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using System.Data.Entity;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {

        public UserRepository() : base() 
        {

        }

        public UserRepository(string ConnectionString) : base(ConnectionString) 
        {

        }

        #region Public Methods

        public List<Role> GetRoles()
        {
            using (var context = GetContext())
            {
                return context.Roles.ToList();
            }
        }

        public User GetFirstManager() {
            using (var context = GetContext()) {
                return context.Roles.Include("Users").Single(
                    r => r.RoleName == Constants.ManagerRoleName).Users.First();
            }
        }

        public User GetUser(string loginId)
        {
            using (var context = GetContext())
            {
                return GetQuery(context).SingleOrDefault(q => q.LoginId == loginId);
            }
        }

        public List<User> SearchUsers(string loginId, int? roleId)
        {
            using (var context = GetContext())
            {
                var query = GetQuery(context);

                if (!string.IsNullOrEmpty(loginId))
                    query = query.Where(u => u.LoginId.Contains(loginId));

                if (roleId != null)
                    query = query.Where(u => u.Roles.Select(user => user.RoleId).Contains(roleId.Value));

                return query.ToList();
            }
        }

        public void InsertUser(User user)
        {
            using (var context = GetContext())
            {
                InsertUser(user, context);
                InsertRole(user, context);
                InsertHeadQuarter(user, context);
                context.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            RenewUser(user);
            UpdateRole(user);
        }

        public void DeleteUser(int userId)
        {
            using (var context = GetContext())
            {
                User userToDelete = GetQuery(context).FirstOrDefault(q => q.UserId == userId);
                if (userToDelete == null)
                    throw new ApplicationException(string.Format("User '{0}' cannot be found.", userToDelete.UserId));

                DeleteUserHeadQuarter(userId, context);
                DeleteUser(context, userToDelete);
                context.SaveChanges();
            }
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }

        #endregion

        #region Private Methods

        private static IQueryable<User> GetQuery(KeyStoreContext context)
        {
            return context.Users.Include("Roles");
        }

        private void InsertRole(User user, KeyStoreContext context)
        {
            user.Roles.ToList().ForEach(r => context.Roles.Attach(r));
        }

        private void InsertUser(User user, KeyStoreContext context)
        {
            context.Users.Add(user);
        }

        private void InsertHeadQuarter(User user, KeyStoreContext context)
        {
            if (Constants.IsMultipleEnabled)
                user.UserHeadQuarters.ToList().ForEach(u => context.UserHeadQuarters.Add(u));
            else
            {
                if (context.HeadQuarters.Count() > 0)
                {
                    var map = new UserHeadQuarter()
                    {
                        User = user,
                        HeadQuarterId = context.HeadQuarters.First().HeadQuarterId,
                        IsDefault = true
                    };
                    context.UserHeadQuarters.Add(map);
                }
            }
        }

        private void DeleteUserHeadQuarter(int userId, KeyStoreContext context)
        {
            context.UserHeadQuarters
                .Where(hq => hq.UserId == userId).ToList()
                .ForEach(h => context.UserHeadQuarters.Remove(h));
        }

        private void DeleteUser(KeyStoreContext context, User userToDelete)
        {
            if (context.Entry(userToDelete).State == EntityState.Detached)
                context.Users.Attach(userToDelete);
            context.Users.Remove(userToDelete);
        }

        private void RenewUser(User user)
        {
            using (var context = GetContext())
            {
                User userToUpdate = GetQuery(context).FirstOrDefault(q => q.UserId == user.UserId);
                if (userToUpdate == null)
                    throw new ApplicationException(string.Format("User '{0}' cannot be found.", user.UserId));                
                if (string.IsNullOrEmpty(user.Password))
                    user.Password = userToUpdate.Password;
                context.Entry(userToUpdate).CurrentValues.SetValues(user);
                context.SaveChanges();
            }
        }

        private void UpdateRole(User user)
        {
            using (var context = GetContext())
            {
                var userToUpdate = GetQuery(context).FirstOrDefault(q => q.UserId == user.UserId);
                if (userToUpdate == null)
                    throw new ApplicationException(string.Format("Update failed, user id: {0} didn't exist.", user.LoginId));
                if (userToUpdate.Role.RoleId != user.Role.RoleId)
                {
                    userToUpdate.Roles.Clear();
                    userToUpdate.Roles.Add(GetRole(context,user.Role.RoleId));
                }
                context.SaveChanges();
            }
        }

        private Role GetRole(KeyStoreContext context, int roleId) 
        {
            return context.Roles.SingleOrDefault(r=>r.RoleId == roleId);
        }

        #endregion
    }
}
