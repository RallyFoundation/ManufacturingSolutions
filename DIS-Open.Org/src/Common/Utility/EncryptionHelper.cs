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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Collections.Generic;

namespace DIS.Common.Utility
{
    /// <summary>
    /// Provides encryption methods
    /// </summary>
    public static class EncryptionHelper
    {
        /// <summary>
        /// Encrypt data with AES algorithm
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] AesEncrypt(byte[] data, out byte[] key, out byte[] iv)
        {
            if (data == null)
                throw new ArgumentNullException("No data to encrypt.");

            using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider())
            {
                provider.GenerateKey();
                provider.GenerateIV();
                key = provider.Key;
                iv = provider.IV;
                return provider.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Decrypt data with AES algorithm
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] AesDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data == null)
                throw new ArgumentNullException("No data to encrypt.");

            using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider())
            {
                provider.Key = key;
                provider.IV = iv;
                return provider.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Encrypt data with RSA algorithm
        /// </summary>
        /// <param name="data"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static byte[] RsaEncrypt(byte[] data, RSACryptoServiceProvider provider)
        {
            if (data == null)
                throw new ArgumentNullException("No data to encrypt.");

            return provider.Encrypt(data, false);
        }

        /// <summary>
        /// Decrypt data with RSA algorithm
        /// </summary>
        /// <param name="data"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static byte[] RsaDecrypt(byte[] data, RSACryptoServiceProvider provider)
        {
            if (data == null)
                throw new ArgumentNullException("No data to decrypt.");

            return provider.Decrypt(data, false);
        }

        /// <summary>
        /// Get an X.509 certificate from windows
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="storeLocation"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public static X509Certificate2 GetCertificate(string subject,
            StoreLocation storeLocation, StoreName storeName = StoreName.My)
        {
            return GetCertificate(storeName, storeLocation,
                X509FindType.FindBySubjectDistinguishedName, subject);
        }

        public static X509Certificate2Collection GetCertificates(StoreLocation storeLocation, StoreName storeName = StoreName.My)
        {
            X509Store store = new X509Store(StoreName.My, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            return store.Certificates;
        }

        public static X509Certificate2 GetCertificate(StoreName storeName, StoreLocation storeLocation,
             X509FindType findType, string findValue)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs = store.Certificates.Find(findType, findValue, false);
            if (certs.Count == 0)
                throw new FileNotFoundException("Certificate cannot be found.");
            else
                return certs[0];
        }

        public static X509Certificate2Collection GetMSCertificates()
        {
            X509Certificate2Collection collection = EncryptionHelper.GetCertificates(StoreLocation.CurrentUser);
            collection = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, true);
            collection = collection.Find(X509FindType.FindByExtension, "2.5.29.37", true);

            List<X509Certificate2> collection2 = new List<X509Certificate2>();
            foreach (X509Certificate2 cert in collection)
            {
                collection2.Add(cert);
            }
            collection2.Sort(delegate(X509Certificate2 x1, X509Certificate2 x2) { return x2.Subject.CompareTo(x1.Subject); });
            return new X509Certificate2Collection(collection2.ToArray());
        }
    }
}
