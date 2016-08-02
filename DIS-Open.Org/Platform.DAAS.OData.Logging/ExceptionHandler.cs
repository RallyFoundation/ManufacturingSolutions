using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NLog;
using Platform.DAAS.OData.Core.Logging;

namespace Platform.DAAS.OData.Logging
{
    public class ExceptionHandler : IExHandler
    {
        public static string DefaultPolicyName = "ODataPlatformExceptionPolicy";

        /// <summary>
        /// Handle an exception
        /// </summary>
        /// <param name="Ex"></param>
        public void HandleException(Exception Ex)
        {
            //ExceptionPolicy.HandleException(ex, DefaultPolicyName);

            LogManager.GetLogger(DefaultPolicyName).Fatal(Ex, new LogItem() { Title = "Exception", Message = Ex.ToString(), Category = "Exception", Level = LogLevel.Fatal.ToString(), MachineName = Environment.MachineName, TimeStamp = DateTime.UtcNow }.ToString());
        }

        public void HandleException(Exception Ex, string Policy)
        {
            //ExceptionPolicy.HandleException(ex, policy);

            LogManager.GetLogger(Policy).Fatal(Ex, new LogItem() { Title = "Exception", Message = Ex.ToString(), Category = "Exception", Level = LogLevel.Fatal.ToString(), MachineName = Environment.MachineName, TimeStamp = DateTime.UtcNow }.ToString());
        }
    }
}
