using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.Logging
{
    public interface ILogger
    {
        string GetMethodName();

        string GetOperationLogTitle(string Prefix, string UserName);

        void LogUserOperation(string UserName, string Message);

        void LogServiceOperation(string UserName, string Message);

        void LogSystemError(string Title, string Message);

        void LogSystemRunning(string Title, string Message);
    }
}
