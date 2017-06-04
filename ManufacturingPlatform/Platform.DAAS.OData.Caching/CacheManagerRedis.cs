using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using Platform.DAAS.OData.Core.Caching;

namespace Platform.DAAS.OData.Caching
{
    public class CacheManagerRedis : ICacheManager
    {
        private RedisClient redisClient;

        public CacheManagerRedis(string ServerAddress, int Port, string Password, long DBIndex)
        {
            this.redisClient = new RedisClient(ServerAddress, Port);
            this.redisClient.Password = Password;
            this.redisClient.ChangeDb(DBIndex);
        }

        public bool HasConnected()
        {
            return this.redisClient.HasConnected;
        }

        public bool HadExceptions()
        {
            return this.redisClient.HadExceptions;
        }

        public object ClearCache(string ID)
        {
            return this.redisClient.Del(ID);
        }

        public object GetCache(string ID)
        {
            return this.redisClient.GetValue(ID);
        }

        public void SetCache(string ID, object ObjectToCache)
        {
            this.redisClient.SetValue(ID, (string)(ObjectToCache));
            //this.redisClient.Save();
        }

        public void SetCache(string ID, object ObjectToCache, TimeSpan ExpireIn)
        {
            this.redisClient.SetValue(ID, (string)(ObjectToCache), ExpireIn);
        }

        public object StoreObject<T>(T Entity)
        {
            return redisClient.Store(Entity);
        }

        public List<T> GetObjectList<T>(string Name, string Value)
        {
            var entities = redisClient.As<T>();
            return entities.GetAll().Where(x => x.GetType().GetProperty(Name).GetValue(x).ToString().Contains(Value)).ToList();
        }
    }
}
