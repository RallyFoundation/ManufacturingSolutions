using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.ServiceManagement
{
    public interface IODataRepository<TEntity>
    {
        IQueryable<TEntity> Get();

        IQueryable<TEntity> Get(string KeyName,object KeyValue);

        TEntity Post(TEntity Item);

        TEntity Patch(string KeyName, object KeyValue, TEntity Item);

        TEntity Put(string KeyName, object KeyValue, TEntity Item);

        bool Delete(string KeyName, object KeyValue);

        string GetConnectionString();

        void SetConnectionString(string ConnectionString);

        void Initialize(string Server, int Port, string UserName, string Password, string Catalog, string Schema);
    }
}
