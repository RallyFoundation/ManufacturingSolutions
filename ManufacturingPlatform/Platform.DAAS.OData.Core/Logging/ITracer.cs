using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.Logging
{
    public interface ITracer
    {
        void Trace(object[] Data, string TraceSourceName);
    }
}
