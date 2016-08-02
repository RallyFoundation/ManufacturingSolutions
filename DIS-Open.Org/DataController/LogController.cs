using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using DIS.Data.DataContract;

namespace DIS.Data.DataController
{
    public class LogController : ODataController
    {
        public const int DefaultPageSize = 100;

        private LogRepository repository;

        public LogController()
        {
            string connectionString = HttpContext.Current.Request.Headers.Get("DAAS-DB-CONNECTION");

            //string resourceName = HttpContext.Current.Request.Headers.Get("DAAS-RESOURCE-NAME");

            this.repository = new LogRepository(connectionString);
        }


        [EnableQuery(PageSize = DefaultPageSize)]
        public IQueryable<Log> Get()
        {
            return this.repository.Get();
        }

        [EnableQuery]
        public SingleResult<Log> Get([FromODataUri]string id)
        {
            var item = this.repository.Get("LogId", long.Parse(id));

            return SingleResult.Create(item);
        }
    }
}
