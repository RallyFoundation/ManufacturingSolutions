using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.ServiceManagement
{
    public class ServiceSubscription
    {
        public string ID { get; set; }

        public string Subscriber{ get; set; }

        public Application Application { get; set; }

        public Service Service { get; set; }

        public string Token { get; set; }

        public int Status { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
