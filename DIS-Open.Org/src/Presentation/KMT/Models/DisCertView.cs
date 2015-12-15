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
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.Models
{
    public class DisCertView
    {
        private X509Certificate2 cert;
        public X509Certificate2 Cert { get; set; }

        public DisCertView(X509Certificate2 certParam)
        {
            this.cert = certParam;
            this.InitialDetails();
        }

        public string FriendlyName { get { return cert.FriendlyName; } }
        
        public string Version { get { return "v" + cert.Version; } }

        public string SerialNumber { get { return cert.SerialNumber; } }

        public string SignatureAlgorithm { get { return cert.SignatureAlgorithm.FriendlyName; } }

        public string SignatureHashAlgorithm { get { return cert.SignatureAlgorithm.Value; } }

        public string Issuer { get { return cert.Issuer; } }

        public string ValidFrom { get { return cert.GetEffectiveDateString(); } }

        public string ValidTo { get { return cert.GetExpirationDateString(); } }

        public string Subject { get { return cert.Subject; } }

        public string PublicKey { get { return cert.PublicKey.Oid.FriendlyName+"("+cert.PublicKey.Key.KeySize+" bits)"; } }

        public string DnsFromAlternativeName { get { return cert.GetNameInfo(X509NameType.DnsFromAlternativeName, true); } }

        public string IssuerAlternativeName { get { return "DNS Name= "+ cert.GetNameInfo(X509NameType.DnsName, true); } }

        public string EmailName { get { return cert.GetNameInfo(X509NameType.EmailName, true); } }

        public string SimpleName { get { return cert.GetNameInfo(X509NameType.SimpleName,true); } }

        public string UpnName { get { return cert.GetNameInfo(X509NameType.UpnName,true); } }

        public string UrlName { get { return cert.GetNameInfo(X509NameType.UrlName,true);} }
        
        public string ThumbPrint { get { return cert.Thumbprint; } }
        
        public string ThumbPrintAlgorithm { get {return "sha1"; } }

        public Dictionary<string, string> CertDetails = new Dictionary<string, string>();

        private void InitialDetails()
        {
            this.CertDetails.Clear();
            this.CertDetails.Add(ResourcesOfR6.CertVM_Version, this.Version);
            this.CertDetails.Add(ResourcesOfR6.CertVM_SerialNumber, this.SerialNumber);
            this.CertDetails.Add(ResourcesOfR6.CertVM_SignatureAlgorithm, this.SignatureAlgorithm);
            //this.CertDetails.Add(ResourcesOfR6.CertVM_SignatureHashAlgorithm, "sha1");
            this.CertDetails.Add(ResourcesOfR6.CertVM_Issuer, this.Issuer);
            this.CertDetails.Add(ResourcesOfR6.CertVM_ValidFrom, this.ValidFrom);
            this.CertDetails.Add(ResourcesOfR6.CertVM_ValidTo, this.ValidTo);
            this.CertDetails.Add(ResourcesOfR6.CertVM_Subject, this.Subject);
            this.CertDetails.Add(ResourcesOfR6.CertVM_PublicKey, this.PublicKey);
            this.CertDetails.Add(ResourcesOfR6.CertVM_ThumbPrint, this.ThumbPrint);
            this.CertDetails.Add(ResourcesOfR6.CertVM_ThumbPrintAlgorithm,this.ThumbPrintAlgorithm);
            this.CertDetails.Add(ResourcesOfR6.CertVM_FriendlyName, this.FriendlyName);
            foreach (X509Extension extension in cert.Extensions)
            {
                if (extension.Oid.FriendlyName == "Key Usage")
                {
                    X509KeyUsageExtension ext = (X509KeyUsageExtension)extension;
                    
                    CertDetails.Add(ResourcesOfR6.CertVM_KeyUsage, ext.KeyUsages.ToString());
                }
                if (extension.Oid.FriendlyName == "Basic Constraints")
                {
                    X509BasicConstraintsExtension ext = (X509BasicConstraintsExtension)extension;
                    string value = ext.CertificateAuthority + "\r\n"
                                   + ext.HasPathLengthConstraint + "\r\n"
                                   + ext.PathLengthConstraint;
                    CertDetails.Add(ResourcesOfR6.CertVM_BasicConstrants, value);
                }

                if (extension.Oid.FriendlyName == "Subject Key Identifier")
                {
                    X509SubjectKeyIdentifierExtension ext = (X509SubjectKeyIdentifierExtension)extension;
                    CertDetails.Add(ResourcesOfR6.CertVM_SubjectkeyID, ext.SubjectKeyIdentifier);
                }

                if (extension.Oid.FriendlyName == "Enhanced Key Usage")
                {
                    X509EnhancedKeyUsageExtension ext = (X509EnhancedKeyUsageExtension)extension;
                    string value = string.Empty;
                    OidCollection oids = ext.EnhancedKeyUsages;
                    foreach (Oid oid in oids)
                    {
                        value += oid.FriendlyName + "(" + oid.Value + ")" + "\r\n";
                    }
                    CertDetails.Add(ResourcesOfR6.CertVM_EnhancedKeyUsage, value);
                }
            }
        }

    }
}
