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
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Security.Cryptography.Xml;

namespace DIS.Common.Utility
{
    /// <summary>
    /// This provider will perform encryption and decryption using the keys from an X509 Certificate. 
    /// </summary>
    public class X509ProtectedConfigProvider : ProtectedConfigurationProvider
    {
        //The certificate that provider try to encrypt/decrypt
        private X509Certificate2 cert;

        /// <summary>
        /// The key thing to note is that this provider expects CertSubjectThumbPrint to be defined in the [configProtectedData] section of the config
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            string certThumbPrint = config["CertSubjectThumbPrint"];

            cert = EncryptionHelper.GetCertificate(
                        StoreName.My, StoreLocation.LocalMachine, X509FindType.FindByThumbprint, certThumbPrint);
        }

        /// <summary>
        /// Encrypt data with X509 certificate
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override System.Xml.XmlNode Encrypt(System.Xml.XmlNode node)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(node.OuterXml);

            EncryptedXml eXml = new EncryptedXml();
            EncryptedData eData = eXml.Encrypt(doc.DocumentElement, cert);
            return eData.GetXml();
        }

        /// <summary>
        /// Decrypt data with X509 certificate
        /// </summary>
        /// <param name="encryptedNode"></param>
        /// <returns></returns>
        public override System.Xml.XmlNode Decrypt(System.Xml.XmlNode encryptedNode)
        {
            XmlDocument doc = encryptedNode.OwnerDocument;
            EncryptedXml eXml = new EncryptedXml(doc);
            eXml.DecryptDocument();
            return doc.DocumentElement;
        }
    }
}
