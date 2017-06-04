using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Core;

namespace DISOpenDataCloud.Models
{
    public class BusinessModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public string[] ReferenceID{get;set;}

        public ConfigurationModel[] Configurations { get; set; }
    }

    public class ConfigurationModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ServerAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
    }

    public class BusinessListModel
    {
        public BusinessModel[] BusinessList { get; set; }

        public PagingArgument PagingArgument { get; set; }

        public SearchingArgument[] SearchingArguments { get; set; }

        public int TotalRecords
        {
            get
            {
                return PagingArgument == null ? -1 : PagingArgument.GetTotalAffectedRecordCount();
            }
        }
    }
}
