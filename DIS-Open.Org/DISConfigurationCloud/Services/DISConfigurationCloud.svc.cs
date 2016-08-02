using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DISConfigurationCloud.MetaManagement;
//using DISConfigurationCloud.Utility;
using Platform.DAAS.OData.Security.Authorization;
using Platform.DAAS.OData.Facade;

namespace DISConfigurationCloud.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DISConfigurationCloud" in code, svc and config file together.
    public class DISConfigurationCloud : IDISConfigurationCloud
    {
        private MetaManager metaManager;

        public DISConfigurationCloud() 
        {
            this.metaManager = new MetaManager();
        }

        [Authorization(IsRequiringAuthentication = true)]
        public string Test()
        {
            return "Hello!";
        }

        [Authorization(IsRequiringAuthentication = true)]
        public string AddCustomer(Customer Customer) 
        {
            try
            {
                return this.metaManager.AddCustomerConfiguration(Customer);
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        [Authorization(IsRequiringAuthentication = true)]
        public Customer GetCustomer(string CustomerID) 
        {
            try
            {
                return this.metaManager.GetCustomer(CustomerID);
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }  
        }

        [Authorization(IsRequiringAuthentication = true)]
        public Customer[] GetCustomers()
        {
            try
            {
                return this.metaManager.ListCustomers();
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }

        [Authorization(IsRequiringAuthentication = true)]
        public Configuration GetCustomerConfiguration(string CustomerID, string ConfigurationType)
        {
            try
            {
                return this.metaManager.GetCustomerConfiguration(CustomerID, ((MetaManagement.ConfigurationType)(int.Parse(ConfigurationType))));
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }
        }


        [Authorization(IsRequiringAuthentication = true)]
        public Configuration GetConfiguration(string ConfigurationID)
        {
            try
            {
                return this.metaManager.GetCustomerConfiguration(ConfigurationID);
            }
            catch (Exception ex)
            {
                //TracingUtility.Trace(new object[] { ex.ToString() }, TracingUtility.DefaultTraceSourceName);

                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);

                throw;
            }	
        }

        [Authorization(IsRequiringAuthentication = true)]
        public string UpdateCustomer(string CustomerID, Customer Customer)
        {
            try
            {
                Customer.ID = CustomerID;

                return this.metaManager.UpdateCustomerConfiguration(Customer).ToString();
            }
            catch (Exception ex)
            {
                Provider.Tracer().Trace(new object[] { ex.ToString() }, null);
                throw;
            }
        }
    }
}
