using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Platform.DAAS.OData.Core.StorageManagement;
using Platform.DAAS.OData.Facade;
using Platform.DAAS.OData.Utility;
using DISOpenDataCloud.Models;

namespace DISOpenDataCloud.Controllers
{
    [RoutePrefix("api/Database/SQLServer")]
    public class DatabaseApiController : ApiController
    {
        [Route("All")]
        [HttpPost]
        public string[] ListDatabases(SQLServerConnectionModel model)
        {
            string connectionString = DBUtility.BuildConnectionString(model.ServerAddress, model.DatabaseName, model.UserName, model.Password);

            return Provider.SQLServerDatabaseManager().ListDatabases(connectionString);
        }
    }
}
