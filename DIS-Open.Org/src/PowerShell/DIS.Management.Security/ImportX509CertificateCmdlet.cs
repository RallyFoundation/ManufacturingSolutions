using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Management.Automation;

namespace DIS.Management.Security
{
    [Cmdlet("Import", "X509Certificate")]
    public class ImportX509CertificateCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The full path to the certificate file.")]
        public string Path { get; set; }

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "The password of the certificate.")]
        public string Password { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The location of the store where the certificate is to be imported to.")]
        public StoreLocation StoreLocation { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The name of the store where the certificate is to be imported to.")]
        public StoreName StoreName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            byte[] certBytes = null;

            using (FileStream stream = new FileStream(this.Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int size = ((int)(stream.Length));

                certBytes = new byte[size];

                stream.Read(certBytes, 0, size);
            }

            X509Certificate2 certificate = new X509Certificate2();

            switch (this.StoreLocation)
            {
                case StoreLocation.CurrentUser:
                    certificate.Import(certBytes, this.Password, X509KeyStorageFlags.UserKeySet|X509KeyStorageFlags.Exportable);
                    break;
                case StoreLocation.LocalMachine:
                    certificate.Import(certBytes, this.Password, X509KeyStorageFlags.MachineKeySet|X509KeyStorageFlags.Exportable);
                    break;
                default:
                    certificate.Import(certBytes, this.Password, X509KeyStorageFlags.UserProtected|X509KeyStorageFlags.Exportable);
                    break;
            }

            this.WriteObject(certificate);

            X509Store store = new X509Store(this.StoreName, this.StoreLocation);

            store.Open(OpenFlags.MaxAllowed);

            store.Add(certificate);

            store.Close();

            this.WriteObject(store);
        }
    }
}
