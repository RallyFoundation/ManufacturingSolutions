using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Platform.DAAS.OData.Core.ServiceManagement;

namespace Platform.DAAS.OData.Framework
{
    public class ODataMongoDBRepository<TEntity> : IODataRepository<TEntity>
    {
        private string connectionString;

        private string schema;

        private MongoServer mongoServer;
        private MongoDatabase mongoDatabase;

        public virtual void Initialize(string Server, int Port, string UserName, string Password, string Catalog, string Schema)
        {
            this.mongoServer = new MongoServer(new MongoServerSettings() {  Server = new MongoServerAddress(Server, Port)});

            this.mongoServer.Connect();

            this.mongoDatabase = this.mongoServer.GetDatabase(Catalog);

            this.schema = Schema;
        }

        public virtual string GetConnectionString()
        {
            return this.connectionString;
        }

        public virtual void SetConnectionString(string ConnectionString)
        {
            this.connectionString = ConnectionString;
        }

        public virtual bool Delete(string KeyName, object KeyValue)
        {
            MongoCollection<TEntity> items = this.mongoDatabase.GetCollection<TEntity>(this.schema);

            //IMongoQuery query = Query.EQ(KeyName, BsonValue.Create(KeyValue));

            IMongoQuery query;

            if (KeyValue is ObjectId)
            {
                query = Query.EQ(KeyName, (ObjectId)(KeyValue));
            }
            else
            {
                query = Query.EQ(KeyName, BsonValue.Create(KeyValue));
            }

            WriteConcernResult result = items.Remove(query);

            return result.UpdatedExisting;
        }

        public virtual IQueryable<TEntity> Get()
        {
            MongoCollection<TEntity> items = this.mongoDatabase.GetCollection<TEntity>(this.schema);

            return items.FindAll().AsQueryable<TEntity>();
        }

        public virtual IQueryable<TEntity> Get(string KeyName, object KeyValue)
        {
            MongoCollection<TEntity> items = this.mongoDatabase.GetCollection<TEntity>(this.schema);

            //IMongoQuery query = Query.EQ(KeyName, BsonValue.Create(KeyValue));

            IMongoQuery query;

            if (KeyValue is ObjectId)
            {
                query = Query.EQ(KeyName, (ObjectId)(KeyValue));
            }
            else
            {
                query = Query.EQ(KeyName, BsonValue.Create(KeyValue));
            }

            return items.Find(query).AsQueryable<TEntity>();
        }

        public virtual TEntity Post(TEntity Item)
        {
            MongoCollection<TEntity> items = this.mongoDatabase.GetCollection<TEntity>(this.schema);

            WriteConcernResult result = items.Insert(Item);

            if (result.UpdatedExisting)
            {
                return Item;
            }

            return default(TEntity);
        }

        public virtual TEntity Put(string KeyName, object KeyValue, TEntity Item)
        {
            MongoCollection<TEntity> items = this.mongoDatabase.GetCollection<TEntity>(this.schema);

            //IMongoQuery query = Query.EQ(KeyName, BsonValue.Create(KeyValue));

            IMongoQuery query;

            if (KeyValue is ObjectId)
            {
                query = Query.EQ(KeyName, (ObjectId)(KeyValue));
            }
            else
            {
                query = Query.EQ(KeyName, BsonValue.Create(KeyValue));
            }

            IMongoUpdate mongoUpdate = new UpdateBuilder();

            System.Reflection.PropertyInfo[] props = Item.GetType().GetProperties(System.Reflection.BindingFlags.Public);

            if (props != null)
            {
                foreach (var prop in props)
                {
                    if ((prop.PropertyType != typeof(IDictionary<string, object>)))
                    {
                        mongoUpdate = Update.Set(prop.Name, BsonValue.Create(prop.GetValue(Item)));
                    }
                }
            }

            if (Item is ODataEntityModel)
            {
                IDictionary<string, object> dynamicProperties = (Item as ODataEntityModel).GetDynamicProperties();

                if (dynamicProperties != null)
                {
                    foreach (var key in dynamicProperties.Keys)
                    {
                        mongoUpdate = Update.Set(key, BsonValue.Create(dynamicProperties[key]));
                    }
                }
            }

            WriteConcernResult result = items.Update(query, mongoUpdate);

            if (result.UpdatedExisting)
            {
                return Item;
            }

            return default(TEntity);
        }

        public virtual TEntity Patch(string KeyName, object KeyValue, TEntity Item)
        {
            throw new NotImplementedException();
        }
    }
}
