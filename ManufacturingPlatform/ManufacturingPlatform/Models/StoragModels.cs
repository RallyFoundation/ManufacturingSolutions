using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DISOpenDataCloud.Models
{
    public class SQLServerConnectionModel
    {
        public string ServerAddress { get; set; }

        public string DatabaseName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
