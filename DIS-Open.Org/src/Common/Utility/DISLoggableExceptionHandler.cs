using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace DIS.Common.Utility
{
    public class DISLoggableException : Exception
    {
        public string DBConnectionString { get; set; }

        public DISLoggableException(string message, Exception innerException)
            : base(message, innerException) 
        {

        }
    }

    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class DISLoggableExceptionHandler : IExceptionHandler 
    {
        private NameValueCollection attributes;

        public DISLoggableExceptionHandler(NameValueCollection configAttributes) 
        {
            this.attributes = configAttributes;
        }
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            if (exception is DISLoggableException)
            {
                string title = (this.attributes != null) ? this.attributes.Get("title") : "Unknown DIS Exception";

                MessageLogger.LogSystemError(title, exception.InnerException.ToString(), (exception as DISLoggableException).DBConnectionString);

                return exception.InnerException;
            }

            return exception;
        }
    }
}
