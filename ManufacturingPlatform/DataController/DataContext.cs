using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;
using Platform.DAAS.OData.Framework;
using DIS.Data.DataContract;

namespace DIS.Data.DataController
{
    public class DataContext : ODataEntityContext
    {
        public DataContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public DbSet<KeyInfo> ProductKeys;

        public DbSet<Log> Logs;
    }
}
