using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Platform.DAAS.OData.Core.Security;

namespace Platform.DAAS.OData.Security
{
    public class EncryptionManager : IEncryptionManager
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] AesEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data == null)
            {
                throw new ArgumentNullException("No data to encrypt.");
            }

            using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider())
            {
                provider.Key = key;
                provider.IV = iv;
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

        public static X509Certificate2Collection GetX509Certificate2CertificateCollection(StoreLocation storeLocation, IDictionary<X509FindType, object> filters)
        {
            X509Certificate2Collection collection = GetCertificates(storeLocation);

            foreach (var findType in filters.Keys)
            {
                collection = collection.Find(findType, filters[findType], true); 
            }

            List<X509Certificate2> collection2 = new List<X509Certificate2>();

            foreach (X509Certificate2 cert in collection)
            {
                collection2.Add(cert);
            }

            collection2.Sort(delegate (X509Certificate2 x1, X509Certificate2 x2) { return x2.Subject.CompareTo(x1.Subject); });

            return new X509Certificate2Collection(collection2.ToArray());
        }

        public byte[] AesEncryption(byte[] Data, byte[] Key, byte[] IV)
        {
            return EncryptionManager.AesEncrypt(Data, Key, IV);
        }

        public byte[] AesDecryption(byte[] Data, byte[] Key, byte[] IV)
        {
            return EncryptionManager.AesDecrypt(Data, Key, IV);
        }
    }
}
