using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using DIS.Data.DataContract;
using System.Net;

namespace DIS.Data.DataController
{
    public class ProductKeyController : ODataController
    {
        public const int DefaultPageSize = 100;

        private ProductKeyRepository repository;

        public ProductKeyController()
        {
            string connectionString = HttpContext.Current.Request.Headers.Get("DAAS-DB-CONNECTION");

            //string resourceName = HttpContext.Current.Request.Headers.Get("DAAS-RESOURCE-NAME");

            this.repository = new ProductKeyRepository(connectionString);
        }

        [EnableQuery(PageSize = DefaultPageSize)]
        public IQueryable<KeyInfo> Get()
        {
            return this.repository.Get();
        }

        [EnableQuery]
        public SingleResult<KeyInfo> Get([FromODataUri]string id)
        {
            var item = this.repository.Get("KeyId", long.Parse(id));

            return SingleResult.Create(item);
        }

        public IHttpActionResult Post(KeyInfo productKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = this.repository.Post(productKey);

            return Created(result);
        }

        public IHttpActionResult Patch([FromODataUri] string id, KeyInfo productKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = this.repository.Patch("KeyId", long.Parse(id), productKey);

            return Updated(result);
        }

        [HttpPost]
        public IHttpActionResult AddKeys(KeyInfo[] ProductKeys)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.AddKeys(ProductKeys);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
