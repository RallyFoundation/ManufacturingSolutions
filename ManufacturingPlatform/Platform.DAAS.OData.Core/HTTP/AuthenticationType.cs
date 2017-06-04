using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform.DAAS.OData.Core.HTTP
{
    public enum AuthenticationType
    {
        PlainText = 0,
        X509Certificate = 1,
        Kerberos = 2,
        NTLM = 3,
        Negociate = 4,
        Custom = 5
    }
}
