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
using System.Security.Cryptography;
using DIS.Common.Utility;
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataContract;
using DIS.Business.Library.Properties;

namespace DIS.Business.Library {
    /// <summary>
    /// UserManager class
    /// </summary>
    public class UserManager : IUserManager {
        #region Private Members

        private IUserRepository userRepository;
        private PasswordVersionResolver resolver = new PasswordVersionResolver();

        #endregion

        #region Constrcutor

        public UserManager()
            : this(new UserRepository()) {
        }

        public UserManager(string dbConnectionString) 
        {
            this.userRepository = new UserRepository(dbConnectionString);
        }

        public UserManager(IUserRepository userRepository) {
            if (userRepository == null) 
                this.userRepository = new UserRepository();
            else
                this.userRepository = userRepository;
        }

        #endregion

        public bool ValidateUser(string loginId, string hashedPassword) {
            User user = userRepository.GetUser(loginId);
            return (user != null) && (user.Password == hashedPassword);
        }

        public User Login(string loginId, string password) {
            User user = userRepository.GetUser(loginId);
            bool shouldUpdate;
            if ((user == null) || !resolver.ValidateUserPassword(user, password, out shouldUpdate))
                throw new DisException(Resources.UserProxy_LoginIDOrPasswordIncorrect);
            if (shouldUpdate) {
                user.Password = password;
                ChangeProfile(user);
            }
            return user;
        }

        public User GetFirstManager() {
            return userRepository.GetFirstManager();
        }

        public List<User> GetUsers(string loginId, int? roleId) {
            return userRepository.SearchUsers(loginId, roleId);
        }

        public void AddUser(User user) {
            if (user == null)
                throw new ArgumentNullException("User is null");
            TrimInformation(user);
            if (!ValidateLoginId(user.LoginId, user.UserId))
                throw new DisException(Resources.UserProxy_UserAlreadyExist);
            resolver.SetUserPassword(user);
            userRepository.InsertUser(user);
        }

        public void EditUser(User user) {
            if (user == null)
                throw new ArgumentNullException("User is null");

            if (!string.IsNullOrEmpty(user.Password))
                resolver.SetUserPassword(user);
            TrimInformation(user);
            if (!ValidateAdminCount(user))
                throw new DisException(Resources.UserProxy_CannotRemoveLastAdmin);
            userRepository.UpdateUser(user);
        }

        public void ChangeProfile(User user) {
            if (user == null)
                throw new ArgumentNullException("User is null");

            if (!string.IsNullOrEmpty(user.Password))
                resolver.SetUserPassword(user);
            TrimInformation(user);
            userRepository.UpdateUser(user);
        }

        public void DeleteUser(User user) {
            if (user == null)
                throw new ArgumentNullException("User is null");

            if ((user.RoleName == Constants.ManagerRoleName) &&
                userRepository.SearchUsers(null, userRepository.GetRoles().SingleOrDefault(r => r.RoleName == Constants.ManagerRoleName).RoleId).Count <= 1)
                throw new DisException(Resources.UserProxy_CannotRemoveLastAdmin);
            userRepository.DeleteUser(user.UserId);
        }

        public List<Role> GetRoles() {
            return userRepository.GetRoles();
        }

        #region Private Methods

        private void TrimInformation(User user) {
            if (!string.IsNullOrEmpty(user.LoginId))
                user.LoginId = user.LoginId.Trim();
            if (!string.IsNullOrEmpty(user.Phone))
                user.Phone = user.Phone.Trim();
            if (!string.IsNullOrEmpty(user.Position))
                user.Position = user.Position.Trim();
            if (!string.IsNullOrEmpty(user.FirstName))
                user.FirstName = user.FirstName.Trim();
            if (!string.IsNullOrEmpty(user.SecondName))
                user.SecondName = user.SecondName.Trim();
            if (!string.IsNullOrEmpty(user.Email))
                user.Email = user.Email.Trim();
            if (!string.IsNullOrEmpty(user.Department))
                user.Department = user.Department.Trim();
            if (!string.IsNullOrEmpty(user.Language))
                user.Language = user.Language.Trim();
        }

        private bool ValidateAdminCount(User user) {
            bool isValid = user.Roles.SingleOrDefault().RoleName == Constants.ManagerRoleName
                || user.Roles.SingleOrDefault().RoleName == userRepository.GetUser(user.LoginId).Roles.SingleOrDefault().RoleName;
            return isValid ||
                userRepository.SearchUsers(null, userRepository.GetRoles().Single(r => r.RoleName == Constants.ManagerRoleName).RoleId).Count > 1;
        }

        public bool ValidateCurrentPassword(User user) {
            User currentUser = this.Login(user.LoginId, user.Password);
            return currentUser != null;
        }

        private bool ValidateLoginId(string loginId, int userId) {
            User user = userRepository.GetUser(loginId);
            return user == null || user.UserId == userId;
        }

        #endregion
    }
}
