using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.DomainModel
{
    public class Service
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public ServiceMeta ServiceMeta { get; set; }
        public ServiceType ServiceType { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Version { get; set; }
        public int Status { get; set; }
        public DateTime CreationTime { get; set; }

        public Application Application { get; set; }
    }
}
