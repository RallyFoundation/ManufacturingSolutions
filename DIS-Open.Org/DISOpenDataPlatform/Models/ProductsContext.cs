using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Common;

namespace ODataTest.Models
{
    public class ProductsContext : DbContext
    {
        //public ProductsContext(): base("name=ProductsContext")
        //{
        //}

        //public ProductsContext(string nameOrConnectionString) : base(nameOrConnectionString)
        //{
        //    base.Database.Connection.ConnectionString = nameOrConnectionString;
        //}

        public ProductsContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public DbSet<Product> Products { get; set; }

    }
}
