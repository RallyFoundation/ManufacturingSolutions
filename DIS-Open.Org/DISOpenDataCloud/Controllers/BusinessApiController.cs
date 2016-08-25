using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Platform.DAAS.OData.Core.DomainModel;
using Platform.DAAS.OData.Core.BusinessManagement;
using Platform.DAAS.OData.Facade;
using Platform.DAAS.OData.Utility;
using DISOpenDataCloud.Models;

namespace DISOpenDataCloud.Controllers
{
    [RoutePrefix("api/Business")]
    public class BusinessApiController : ApiController
    {
        [Route("New")]
        [HttpPost]
        public string CreateBusiness(BusinessModel business)
        {
            Business biz = new Business()
            {
                ID = !String.IsNullOrEmpty(business.ID) ? business.ID : Guid.NewGuid().ToString(),
                Name = business.Name,
                BusinessType = (BusinessType)Enum.Parse(typeof(BusinessType), business.Type),
            };

            List<Configuration> confs = new List<Configuration>();

            foreach (var conf in business.Configurations)
            {
                confs.Add(new Configuration()
                {
                    ID = !String.IsNullOrEmpty(conf.ID) ? conf.ID : Guid.NewGuid().ToString(),
                    ConfigurationType = (ConfigurationType)Enum.Parse(typeof(ConfigurationType), conf.Type),
                    DbConnectionString = DBUtility.BuildConnectionString(conf.ServerAddress, conf.DatabaseName, conf.UserName, conf.Password)
                });
            }

            biz.Configurations = confs.ToArray();

            string bizID = Provider.BusinessManager().AddBusinessConfiguration(biz);

            return bizID;
        }
    }
}
