using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using ODataTest.Models;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace ODataTest.Controllers
{
    public class ProductsController : ODataController
    {
        //ProductsContext db = new ProductsContext();

        //ProductsContext db = new ProductsContext("Data Source=localhost;port=3306;Initial Catalog=test;user id=root;password=P@ssword!;");

        ProductsContext db = null; //new ProductsContext(new MySqlConnection("Data Source=localhost;port=3306;Initial Catalog=test;user id=root;password=P@ssword!;"), true);

        public ProductsController() 
        {
            string dbType = ""; 

            if (System.Web.HttpContext.Current.Request.QueryString["DBType"] != null)
            {
                dbType = System.Web.HttpContext.Current.Request.QueryString["DBType"].ToLower();
            }

            switch (dbType)
            {
                case "mysql":
                    {
                        this.db = new ProductsContext(new MySqlConnection("Data Source=localhost;port=3306;Initial Catalog=test;user id=root;password=P@ssword!;"), true);
                        break;
                    }
                case "mssqlserver":
                    {
                        this.db = new ProductsContext(new SqlConnection("Data Source=192.168.1.158;Initial Catalog=ODataTest;User ID=MES;Password=M(S@OMSG.msft"), true);
                        break;
                    }
                default:
                    {
                        this.db = new ProductsContext(new MySqlConnection("Data Source=localhost;port=3306;Initial Catalog=test;user id=root;password=P@ssword!;"), true);
                        break;
                    }

            }
        }
        private bool ProductExists(int key)
        {
            return db.Products.Any(p => p.Id == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing); 
        }

        [EnableQuery]
        public IQueryable<Product> Get()
        {
            return db.Products;
        }
        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            IQueryable<Product> result = db.Products.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        /// <summary>
        /// To enable clients to add a new product to the database, add the following method to ProductsController.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();

            if (this.db.Database.Connection.State != System.Data.ConnectionState.Closed)
            {
                this.db.Database.Connection.Close();
            }

            return Created(product);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Products.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            product.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Product update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        /// <summary>
        /// To enable clients to delete a product from the database, add the following method to ProductsController.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
