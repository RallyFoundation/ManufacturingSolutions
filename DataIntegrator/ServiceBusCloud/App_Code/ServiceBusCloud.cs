using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DataIntegrator
{
    /// <summary>
    /// Summary description for ServiceBusCloud
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceBusCloud : System.Web.Services.WebService
    {

        public ServiceBusCloud()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        //[WebMethod]
        //public string HelloWorld() {
        //    return "Hello World";
        //}

        IManager manager;

        [WebMethod]
        public bool InitializeAdapters()
        {
            this.manager = new DataIntegrator.Manager();

            string location = System.Configuration.ConfigurationManager.AppSettings.Get("DataIntegrator.AdapterConfig.Location");

            string pattern = System.Configuration.ConfigurationManager.AppSettings.Get("DataIntegrator.AdapterConfig.SearchPattern");

            string encodingName = System.Configuration.ConfigurationManager.AppSettings.Get("DataIntegrator.AdapterConfig.EncodingName");

            this.manager.InitializeAdapters(location, pattern, encodingName);

            return true;
        }

        [WebMethod]
        public string[] ListAdapters()
        {
            if (this.manager == null)
            {
                this.manager = new DataIntegrator.Manager();
            }

            return this.manager.ListAdapters();
        }

        [WebMethod]
        [System.Xml.Serialization.XmlInclude(typeof(HTTPEndPoint))]
        [System.Xml.Serialization.XmlInclude(typeof(SOAPEndPoint))]
        [System.Xml.Serialization.XmlInclude(typeof(RDBMSEndPoint))]
        [System.Xml.Serialization.XmlInclude(typeof(FileSystemEndPoint))]
        [System.Xml.Serialization.XmlInclude(typeof(FTPEndPoint))]
        [System.Xml.Serialization.XmlInclude(typeof(XSLTEndPoint))]
        public Adapter GetAdapter(string AdapterName)
        {
            if (this.manager == null)
            {
                this.manager = new DataIntegrator.Manager();
            }

            return manager.GetAdapter(AdapterName) as Adapter;
        }

        [WebMethod]
        public bool SetAdapter(string AdapterName) 
        {
            if (this.manager == null)
            {
                this.manager = new DataIntegrator.Manager();
            }

            string encodingName = System.Configuration.ConfigurationManager.AppSettings.Get("DataIntegrator.AdapterConfig.EncodingName");

            return manager.SetAdapter(AdapterName, encodingName) != null;
        }

        [WebMethod]
        public string Adapt(string AdapterName)
        {
            if (this.manager == null)
            {
                this.manager = new DataIntegrator.Manager();
            }

            IAdapter adapter = this.manager.GetAdapter(AdapterName);

            object returnValue = adapter.Adapt(null);

            if (returnValue is object[])
            {
                return ((object[])returnValue)[0].ToString();
            }
            else if(returnValue != null)
            {
                return returnValue.ToString();
            }

            return "";
        }

        [WebMethod]
        public string SetDefaultTraceSource(string TraceSourceName) 
        {
            if (String.IsNullOrEmpty(TraceSourceName))
            {
                throw new Exception("Trace source name should NOT be empty!");
            }

            if (this.manager == null)
            {
                this.manager = new DataIntegrator.Manager();
            }

            return manager.SetDefaultTraceSource(TraceSourceName);
        }
    }
}