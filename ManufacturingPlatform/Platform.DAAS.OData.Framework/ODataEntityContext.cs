using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;

namespace Platform.DAAS.OData.Framework
{
    public abstract class ODataEntityContext : DbContext
    {
        public ODataEntityContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {

        }
    }
}
