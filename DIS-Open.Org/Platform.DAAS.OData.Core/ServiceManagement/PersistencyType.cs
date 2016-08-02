using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.ServiceManagement
{
    public enum PersistencyType
    {
        MySQL = 0,
        MongoDB = 1,
        Redis = 2,
        File = 3,
        SQLServer = 4,
        Oracle = 5,
        Other = 6
    }
}
