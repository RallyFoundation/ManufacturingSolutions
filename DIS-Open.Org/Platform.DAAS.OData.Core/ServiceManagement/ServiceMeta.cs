using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.ServiceManagement
{
    public class ServiceMeta
    {
        public string ID { get; set; }
        //public Service Service { get; set; }
        public PersistencyType PersistencyType { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DBConnectionString { get; set; }
        public string Parameters { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set;}
        public string Charset { get; set; }
        public string DomainName { get; set; }
        public string ResourceName { get; set; }
        public long Size { get; set; }
        public string ModelMeta { get; set; }
        public string ServiceCode { get; set; }
        public byte[] ServiceBinary { get; set; }
    }
}
