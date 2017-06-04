using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.DomainModel
{
    public class Application
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Owner { get; set; }

        public int Status { get; set; }

        public List<Service> Services { get; set; }
    }
}
