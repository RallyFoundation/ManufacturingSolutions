using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.Caching
{
    public interface ICacheManager
    {
        void SetCache(string ID, object ObjectToCache);

        void SetCache(string ID, object ObjectToCache, TimeSpan ExpireIn);

        object GetCache(string ID);

        object ClearCache(string ID);

        object StoreObject<T>(T Entity);

        List<T> GetObjectList<T>(string Name, string Value);

        bool HasConnected();

        bool HadExceptions();
    }
}
