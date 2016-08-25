using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DISOpenDataCloud.Models
{
    public class BusinessModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public ConfigurationModel[] Configurations { get; set; }
    }

    public class ConfigurationModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ServerAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
    }
}
