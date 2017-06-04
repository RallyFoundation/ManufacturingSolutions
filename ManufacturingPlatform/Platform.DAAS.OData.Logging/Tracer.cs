using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Platform.DAAS.OData.Core.Logging;

namespace Platform.DAAS.OData.Logging
{
    public class Tracer : ITracer
    {
        public static string DefaultTraceSourceName = "ODataPlatformTraceSource";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="TraceSource"></param>
        public void Trace(object[] Data, string TraceSource)
        {
            try
            {
                string sourceName = !String.IsNullOrEmpty(TraceSource) ? TraceSource : DefaultTraceSourceName;

                //System.Diagnostics.TraceSource trace = new System.Diagnostics.TraceSource(sourceName);

                //trace.TraceData(System.Diagnostics.TraceEventType.Information, new Random().Next(), data);

                //trace.Flush();

                string message = "";

                if (Data != null)
                {
                    foreach (var item in Data)
                    {
                        if (item != null)
                        {
                            message += item.ToString();
                            message += "\r\n";
                        }
                    }
                }

                LogManager.GetLogger(sourceName).Trace(new LogItem() { Title = "Tracing", Message = message, Category = "TracingInfo", Level = LogLevel.Trace.ToString(), MachineName = Environment.MachineName, TimeStamp = DateTime.UtcNow }.ToString());
            }
            catch (Exception)
            {
                //If you want to handle this exception, add your exception handling code here, else you may uncomment the following line to throw this exception out.
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        public void Trace(object[] Data)
        {
            try
            {
                string sourceName = DefaultTraceSourceName;

                string message = "";

                if (Data != null)
                {
                    foreach (var item in Data)
                    {
                        if (item != null)
                        {
                            message += item.ToString();
                            message += "\r\n";
                        }
                    }
                }

                LogManager.GetLogger(sourceName).Trace(new LogItem() { Title = "Tracing", Message = message, Category = "TracingInfo", Level = LogLevel.Trace.ToString(), MachineName = Environment.MachineName, TimeStamp = DateTime.UtcNow }.ToString());
            }
            catch (Exception)
            {
                //If you want to handle this exception, add your exception handling code here, else you may uncomment the following line to throw this exception out.
                throw;
            }
        }
    }
}
