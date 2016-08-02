using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Core.ServiceManagement;

namespace Platform.DAAS.OData.Framework
{
    public class ODataMySQLDBRepository<TEntity> : IODataRepository<TEntity>
    {
        public virtual bool Delete(string KeyName, object KeyValue)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> Get()
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> Get(string KeyName, object KeyValue)
        {
            throw new NotImplementedException();
        }

        public virtual string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public virtual void Initialize(string Server, int Port, string UserName, string Password, string Catalog, string Schema)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Patch(string KeyName, object KeyValue, TEntity Item)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Post(TEntity Item)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Put(string KeyName, object KeyValue, TEntity Item)
        {
            throw new NotImplementedException();
        }

        public virtual void SetConnectionString(string ConnectionString)
        {
            throw new NotImplementedException();
        }
    }
}
