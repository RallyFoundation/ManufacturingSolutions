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
using System.Security.Cryptography;
using DIS.Data.DataContract;
using DIS.Common.Utility;

namespace DIS.Business.Library {
    public class PasswordVersionResolver {
        private const int saltLength = 10;
        public const PasswordVersion CurrentPasswordVersion = PasswordVersion.Sha512WithSalt;

        public bool ValidateUserPassword(User userInDb, string inputPassword, out bool shouldUpdate) {
            shouldUpdate = (userInDb.PasswordVersion != CurrentPasswordVersion);
            switch (userInDb.PasswordVersion) {
                case PasswordVersion.Sha1:
                    if (HashHelper.CreateHash<SHA1CryptoServiceProvider>(inputPassword) != userInDb.Password)
                        return false;
                    break;
                case PasswordVersion.Sha512WithSalt:
                    if (HashHelper.CreateHash<SHA512CryptoServiceProvider>(MixWithSalt(inputPassword, userInDb.Salt)) != userInDb.Password)
                        return false;
                    break;
                default:
                    throw new NotSupportedException();
            }
            if (shouldUpdate) {
                userInDb.PasswordVersion = CurrentPasswordVersion;
                if (string.IsNullOrEmpty(userInDb.Salt))
                    userInDb.Salt = HashHelper.GenerateRandomString(saltLength);
            }
            return true;
        }

        public void SetUserPassword(User user) {
            if (user.PasswordVersion != CurrentPasswordVersion)
                throw new ApplicationException(string.Format("Password version of user '{0}' has expired.", user.LoginId));

            user.Password = HashHelper.CreateHash<SHA512CryptoServiceProvider>(MixWithSalt(user.Password, user.Salt));
        }

        private string MixWithSalt(string password, string salt) {
            return string.Format("{0}{1}", salt, password);
        }
    }
}
