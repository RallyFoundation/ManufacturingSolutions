using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISOpenDataCloud
{
    public class BusinessConfig
    {
        public static void SetStoreConnectionName()
        {
            Platform.DAAS.OData.Facade.Global.SetBusinessStoreConnectionName("DefaultConnection");
        }
    }
}