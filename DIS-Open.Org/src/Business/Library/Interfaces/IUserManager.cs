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

using System.Collections.Generic;
using DIS.Data.DataContract;

namespace DIS.Business.Library
{
    /// <summary>
    /// Interface of User Management class
    /// </summary>
    public interface IUserManager
    {
        bool ValidateUser(string loginId, string hashedPassword);

        User Login(string loginId, string password);

        User GetFirstManager();

        List<User> GetUsers(string loginId, int? roleId);

        void AddUser(User user);

        void EditUser(User user);

        void ChangeProfile(User user);

        void DeleteUser(User user);
        
        bool ValidateCurrentPassword(User user);

        List<Role> GetRoles();
    }
}
