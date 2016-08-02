using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using Platform.DAAS.OData.Framework;
using DIS.Data.DataContract;

namespace DIS.Data.DataController
{
    public class LogRepository : ODataSQLServerDBRepository<Log>
    {
        private DataContext context = null;
        private string connectionString = null;

        public LogRepository(string DBConnectionString)
        {
            this.connectionString = DBConnectionString;
        }

        public override string GetConnectionString()
        {
            return this.connectionString;
        }

        public override void SetConnectionString(string ConnectionString)
        {
            this.connectionString = ConnectionString;
        }

        public override IQueryable<Log> Get()
        {
            IQueryable<Log> result;

            using (this.context = new DataContext(new SqlConnection(this.connectionString), false))
            {
                result = this.context.Logs.AsQueryable();
            }

            return result;
        }


        public override IQueryable<Log> Get(string KeyName, object KeyValue)
        {
            IQueryable<Log> result;

            using (this.context = new DataContext(new SqlConnection(this.connectionString), false))
            {
                result = this.context.Logs.Where(l => l.LogId == (int)(KeyValue)).AsQueryable();
            }

            return result;
        }

    }
}
