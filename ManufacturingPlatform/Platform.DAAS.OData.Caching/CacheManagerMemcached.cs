using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching;
using ServiceStack.Caching.Memcached;
using Platform.DAAS.OData.Core.Caching;

namespace Platform.DAAS.OData.Caching
{
    public class CacheManagerMemcached : ICacheManager
    {
        private MemcachedClient memcachedClient;

        public CacheManagerMemcached() 
        {
            memcachedClient = new MemcachedClient();
        }

        public void SetCache(string ID, object ObjectToCache)
        {
            //throw new NotImplementedException();

            this.memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Set, ID, ObjectToCache);
        }

        public void SetCache(string ID, object ObjectToCache, TimeSpan ExpireIn)
        {
            //throw new NotImplementedException();

            this.memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Set, ID, ObjectToCache, ExpireIn);
        }

        public object GetCache(string ID)
        {
            //throw new NotImplementedException();

            return this.memcachedClient.Get(ID);
        }

        public object ClearCache(string ID)
        {
            //throw new NotImplementedException();

            return this.memcachedClient.Remove(ID);
        }

        public object StoreObject<T>(T Entity)
        {
            throw new NotImplementedException();
        }

        public List<T> GetObjectList<T>(string Name, string Value)
        {
            //throw new NotImplementedException();

            T t = this.memcachedClient.Get<T>(Name);

            if (t != null)
            {
                return new List<T>() { t };
            }
            return null;
        }

        public bool HasConnected()
        {
           // throw new NotImplementedException();

            return true;
        }

        public bool HadExceptions()
        {
            //throw new NotImplementedException();

            return false;
        }
    }
}
