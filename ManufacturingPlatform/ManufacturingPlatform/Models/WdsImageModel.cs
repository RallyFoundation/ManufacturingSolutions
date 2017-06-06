using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.DAAS.OData.Core.DomainModel;

namespace ManufacturingPlatform.Models
{
    public class WdsImageModel
    {
        public string Key { get; set; }
        public OSImage WdsImage { get; set; }
    }
}