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

namespace DIS.Common.Utility {
    /// <summary>
    /// Provides hash methods
    /// </summary>
    public static class HashHelper {
        private const string charRange = @"abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$";
        private static readonly Encoding defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Generates a random string.
        /// </summary>
        public static string GenerateRandomString(int length) {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++) {
                chars[i] = charRange[RollDice(charRange.Length)];
            }
            return new string(chars);
        }

        /// <summary>
        /// Creates hash value of data with specified hash provider.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateHash<T>(byte[] data) where T : HashAlgorithm {
            if (data == null)
                throw new ArgumentNullException("No data to hash.");

            using (HashAlgorithm hashProvider = Activator.CreateInstance<T>()) {
                return ToHexString(hashProvider.ComputeHash(data));
            }
        }

        /// <summary>
        /// Creates hash value of string with specified hash provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CreateHash<T>(string str) where T : HashAlgorithm {
            return CreateHash<T>(str, defaultEncoding);
        }

        /// <summary>
        /// Creates HMAC value of data with specified HMAC provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateHmac<T>(string key, byte[] data) where T : HMAC {
            if (data == null)
                throw new ArgumentNullException("No data to hash.");

            using (HMAC hmac = (HMAC)Activator.CreateInstance(typeof(T), defaultEncoding.GetBytes(key))) {
                return ToHexString(hmac.ComputeHash(data));
            }
        }

        private static string CreateHash<T>(string str, Encoding encoding) where T : HashAlgorithm {
            return CreateHash<T>(encoding.GetBytes(str));
        }

        private static string ToHexString(byte[] bytes) {
            StringBuilder sb = new StringBuilder();
            foreach (var b in bytes) {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        private static int RollDice(int length) {
            if (length > byte.MaxValue)
                throw new NotImplementedException();
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider()) {
                byte[] randomNumberBuffer = new byte[1];
                do {
                    rngCsp.GetBytes(randomNumberBuffer);
                }
                while (randomNumberBuffer[0] >= ((byte.MaxValue / length) * length));
                return randomNumberBuffer[0] % length;
            }
        }
    }
}
