using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DataIntegrator.Helpers;

namespace DataIntegrator
{
    public class Manager : IManager
    {
        static IDictionary<string, IAdapter> AdapterCache;

        public void InitializeAdapters(string ConfigurationLocation, string SearchPattern, string EncodingName) 
        {
            Manager.AdapterCache = new Dictionary<string, IAdapter>();

            string searchPattern = String.IsNullOrEmpty(SearchPattern) ? "*.xml" : SearchPattern;

            string[] adapterFileNames = Directory.GetFiles(ConfigurationLocation, SearchPattern);

            XmlDocument document = null;

            IAdapter adapter = null;

            if ((adapterFileNames != null) && (adapterFileNames.Length > 0))
            {
                foreach (string adapterFileName in adapterFileNames)
                {
                    document = new XmlDocument();

                    document.Load(adapterFileName);

                    adapter = Utility.XmlDeserialize(document.InnerXml, typeof(Adapter), new Type[] { typeof(Authentication), typeof(Operation), typeof(Protocol), typeof(AuthenticationType), typeof(OperationMethod), typeof(List<Operation>), typeof(List<Argument>), typeof(Argument), typeof(HTTPEndPoint), typeof(SOAPEndPoint), typeof(RDBMSEndPoint), typeof(XSLTEndPoint), typeof(LDAPEndPoint), typeof(FileSystemEndPoint), typeof(FTPEndPoint) }, EncodingName) as Adapter;

                    Manager.AdapterCache.Add(adapterFileName, adapter);
                }
            }
        }

        public string[] ListAdapters() 
        {
            if (Manager.AdapterCache != null)
            {
                return Manager.AdapterCache.Keys.ToArray();
            }

            return null;
        }

        public IAdapter GetAdapter(string AdapterName) 
        {
            if ((Manager.AdapterCache != null) && Manager.AdapterCache.ContainsKey(AdapterName))
            {
                return Manager.AdapterCache[AdapterName];
            }

            return null;
        }

        public IAdapter SetAdapter(string AdapterName, string EncodingName) 
        {
            if (Manager.AdapterCache == null)
            {
                Manager.AdapterCache = new Dictionary<string, IAdapter>();
            }

            XmlDocument document = new XmlDocument();

            IAdapter adapter = null;

            document.Load(AdapterName);

            adapter = Utility.XmlDeserialize(document.InnerXml, typeof(Adapter), new Type[] { typeof(Authentication), typeof(Operation), typeof(Protocol), typeof(AuthenticationType), typeof(OperationMethod), typeof(List<Operation>), typeof(List<Argument>), typeof(Argument), typeof(HTTPEndPoint), typeof(SOAPEndPoint), typeof(RDBMSEndPoint), typeof(XSLTEndPoint), typeof(LDAPEndPoint), typeof(FileSystemEndPoint), typeof(FTPEndPoint) }, EncodingName) as Adapter;

            if (!Manager.AdapterCache.ContainsKey(AdapterName))
            {
                Manager.AdapterCache.Add(AdapterName, adapter);
            }
            else
            {
                Manager.AdapterCache[AdapterName] = adapter;
            }

            return adapter;
        }

        public string SetDefaultTraceSource(string TraceSourceName) 
        {
            DataIntegrator.Helpers.Tracing.TracingHelper.DefaultTraceSourceName = TraceSourceName;

            return DataIntegrator.Helpers.Tracing.TracingHelper.DefaultTraceSourceName;
        }
    }
}
