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
        public string[] ListDatabases(SQLServerConnectionModel connection)
        {
            string connectionString = DBUtility.BuildConnectionString(connection.ServerAddress, connection.DatabaseName, connection.UserName, connection.Password);

            return Provider.SQLServerDatabaseManager().ListDatabases(connectionString);
        }

        [Route("Exists")]
        [HttpPost]
        public string CheckDatabaseExistency(SQLServerConnectionModel connection)
        {
            string connectionString = DBUtility.BuildConnectionString(connection.ServerAddress, "master", connection.UserName, connection.Password);

            string[] result = Provider.SQLServerDatabaseManager().ListDatabases(connectionString);

            foreach (var item in result)
            {
                if (item.ToLower() == connection.DatabaseName.ToLower())
                {
                    return "1";
                }
            }

            return "0";
        }

        [Route("Create")]
        [HttpPost]
        public string CreateDatabase(SQLServerConnectionModel connection)
        {
            //string connectionString = DBUtility.BuildConnectionString(connection.ServerAddress, "master", connection.UserName, connection.Password);

            string result = Provider.SQLServerDatabaseManager().CreateDatabase(connection.ServerAddress, connection.DatabaseName, connection.UserName, connection.Password);

            return result;
        }
    }
}
