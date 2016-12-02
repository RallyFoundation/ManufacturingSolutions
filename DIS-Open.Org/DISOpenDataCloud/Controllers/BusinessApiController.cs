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
        [Route("{id}")]
        [HttpGet]
        public BusinessModel GetBusiness(string id)
        {
            Business biz = Provider.BusinessManager().GetBusiness(id);

            if (biz != null)
            {
                BusinessModel business = new BusinessModel()
                {
                     ID = biz.ID,
                     Name = biz.Name,
                     Type = biz.BusinessType.ToString(),
                     ReferenceID = biz.ReferenceID
                };

                if (biz.Configurations != null && biz.Configurations.Length > 0)
                {
                    List<ConfigurationModel> configurations = new List<ConfigurationModel>();

                    string host = "", username = "", password = "", dbname = "";

                    foreach (var conf in biz.Configurations)
                    {
                        DBUtility.ParseConnectionString(conf.DbConnectionString, out host, out dbname, out username, out password);

                        configurations.Add(new ConfigurationModel()
                        {
                             ID = conf.ID,
                             Type = conf.ConfigurationType.ToString(),
                             DatabaseName = dbname,
                             Password = password, 
                             ServerAddress = host,
                             UserName = username
                        });
                    }

                    business.Configurations = configurations.ToArray();
                }

                return business;
            }

            return null;
        }

        [Route("{id}/Configurations")]
        [HttpGet]
        public ConfigurationModel[] GetBusinessConfigurations(string id)
        {
            List<ConfigurationModel> configurations = null;

            var confs = Provider.BusinessManager().GetConfigurations(id);

            if (confs != null && confs.Length > 0)
            {
                configurations = new List<ConfigurationModel>();

                string host = "", username = "", password = "", dbname = "";

                foreach (var conf in confs)
                {
                    DBUtility.ParseConnectionString(conf.DbConnectionString, out host, out dbname, out username, out password);

                    configurations.Add(new ConfigurationModel()
                    {
                        ID = conf.ID,
                        Type = conf.ConfigurationType.ToString(),
                        DatabaseName = dbname,
                        Password = password,
                        ServerAddress = host,
                        UserName = username
                    });
                }
            }

            return configurations != null ? configurations.ToArray() : null;
        }

        [Route("Query")]
        [HttpPost]
        public BusinessListModel QueryBusiness(BusinessListModel businessQuery)
        {
            var bizManager = Provider.BusinessManager();

            var bizList = bizManager.SearchBusiness(bizManager.CreateBusinessQueryExpression, businessQuery.SearchingArguments, businessQuery.PagingArgument, false);

            if (bizList != null && bizList.Length > 0)
            {
                List<BusinessModel> bizModels = new List<BusinessModel>();

                foreach (var biz in bizList)
                {
                    bizModels.Add(new BusinessModel()
                    {
                         ID = biz.ID,
                         Name = biz.Name,
                         Type =  biz.BusinessType.ToString()
                    });

                    businessQuery.BusinessList = bizModels.ToArray();
                }
            }

            return businessQuery;
        }

        [Route("")]
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

            if (business.ReferenceID != null && business.ReferenceID.Length > 0)
            {
                biz.ReferenceID = business.ReferenceID.ToArray();
            }

            string bizID = Provider.BusinessManager().AddBusinessConfiguration(biz);

            return bizID;
        }

        [Route("")]
        [HttpPatch]
        public string UpdateBusiness(BusinessModel business)
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

            if (business.ReferenceID != null && business.ReferenceID.Length > 0)
            {
                biz.ReferenceID = business.ReferenceID.ToArray();
            }

            int result = Provider.BusinessManager().UpdateBusinessConfiguration(biz);

            return result.ToString();
        }
    }
}
