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

namespace DIS.Data.DataAccess.Repository
{
    public interface IUserRepository
    {
        List<Role> GetRoles();

        User GetFirstManager();

        User GetUser(string loginId);

        List<User> SearchUsers(string loginId, int? roleId);

        void InsertUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int userId);

        string GetDBConnectionString();
    }
}
