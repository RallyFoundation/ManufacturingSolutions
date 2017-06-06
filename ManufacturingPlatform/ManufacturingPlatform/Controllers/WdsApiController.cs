using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Platform.DAAS.OData.Core.WdsManagement;
using Platform.DAAS.OData.Facade;
using ManufacturingPlatform.Models;

namespace ManufacturingPlatform.Controllers
{
    [RoutePrefix("api/Wds")]
    public class WdsApiController : ApiController
    {
        [Route("ImageGroup")]
        [HttpPost]
        public object AddWdsInstallImageGroup(string name)
        {
            return Provider.WdsManager().AddInstallImageGroup(name);
        }

        [Route("BootImage")]
        [HttpPost]
        public object ImportWdsBootImage(WdsImageModel image)
        {
            return Provider.WdsManager().ImportBootImage(image.WdsImage);
        }

        [Route("InstallImage")]
        [HttpPost]
        public object ImportWdsInstallImage(WdsImageModel image)
        {
            IWdsManager wdsMananger = Provider.WdsManager();
            object imageObj = wdsMananger.ImportInstallImage(image.WdsImage);
            string wdsImageNamespace = wdsMananger.GetWdsImageNamespace(image.WdsImage);

            Provider.GetWdsCacheManager().SetCache(image.Key, wdsImageNamespace);

            return imageObj;
        }

        [Route("RawImage/{imagePath}")]
        [HttpGet]
        public object ListRawImageContent(string imagePath)
        {
            return Provider.WdsManager().GetRawImageContent(imagePath);
        }

        [Route("ImageLookup")]
        [HttpPost]
        public object SetInstallImageLookupCache(WdsImageModel image)
        {
            string wdsImageNamespace = Provider.WdsManager().GetWdsImageNamespace(image.WdsImage);

            Provider.GetWdsCacheManager().SetCache(image.Key, wdsImageNamespace);

            return wdsImageNamespace;
        }
    }
}
