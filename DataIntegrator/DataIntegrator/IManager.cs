using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrator
{
    public interface IManager
    {
        void InitializeAdapters(string ConfigurationLocation, string SearchPattern, string EncodingName);

        string[] ListAdapters();

        IAdapter GetAdapter(string AdapterName);

        IAdapter SetAdapter(string AdapterName, string EncodingName);

        string SetDefaultTraceSource(string TraceSourceName);
    }
}
