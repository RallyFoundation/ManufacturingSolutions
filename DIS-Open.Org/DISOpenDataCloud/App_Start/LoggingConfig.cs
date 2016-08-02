using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.DAAS.OData.Facade;

namespace DISOpenDataCloud
{
    public class LoggingConfig
    {
        public static void SetLoggingSettings()
        {
            Global.SetExceptionHandler("DISOpenDataCloudExceptionPolicy");
            Global.SetLogger("DISOpenDataCloudLogger");
            Global.SetTracer("DISOpenDataCloudTraceSource");
        }
    }
}