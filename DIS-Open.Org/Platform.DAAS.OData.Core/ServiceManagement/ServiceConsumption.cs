using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.ServiceManagement
{
    public class ServiceConsumption
    {
        public string ID { get; set; }

        public Service Service { get; set; }

        public string Consumer { get; set; }

        public string UrlReferrer { get; set; }

        public string Result { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
