using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.ServiceManagement
{
    public interface IODataEntityModel<T>
    {
        IDictionary<string, object> GetDynamicProperties();
        void Register(T Builder);
    }
}
