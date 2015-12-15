using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Management.Automation;

namespace DIS.Management.Security
{
    [Cmdlet(VerbsCommon.Find, "X509Certificate")]
    public class FindX509CertificateCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The location of the store where the certificate to find is at.")]
        public StoreLocation StoreLocation { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The name of the store where the certificate to find is at.")]
        public StoreName StoreName { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The property to find the certificate with.")]
        public X509FindType FindType { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The value corresponding to property to find the certificate with.")]
        public object FindValue { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            X509Store store = new X509Store(this.StoreName, this.StoreLocation);

            store.Open(OpenFlags.MaxAllowed);

            X509Certificate2Collection certificates = store.Certificates;

            X509Certificate2Collection results = null;

            X509Certificate2[] certs = null;

            if (certificates != null)
            {
               results = certificates.Find(this.FindType, this.FindValue, false);

               if ((results != null) && (results.Count > 0))
               {
                   certs = new X509Certificate2[results.Count];
                   results.CopyTo(certs, 0);
               }
            }

            this.WriteObject(certs);

            store.Close();
        }
    }
}
