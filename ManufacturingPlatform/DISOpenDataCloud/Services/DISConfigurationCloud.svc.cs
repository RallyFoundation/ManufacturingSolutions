using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DISOpenDataCloud.Models;

namespace DISOpenDataCloud.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DISConfigurationCloud" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DISConfigurationCloud.svc or DISConfigurationCloud.svc.cs at the Solution Explorer and start debugging.
    public class DISConfigurationCloud : IDISConfigurationCloud
    {
        public string AddCustomer(Customer Customer)
        {
            throw new NotImplementedException();
        }

        public void DoWork()
        {
        }

        public ConfigurationModel GetConfiguration(string ConfigurationID)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(string CustomerID)
        {
            throw new NotImplementedException();
        }

        public ConfigurationModel GetCustomerConfiguration(string CustomerID, string ConfigurationType)
        {
            throw new NotImplementedException();
        }

        public Customer[] GetCustomers()
        {
            throw new NotImplementedException();
        }

        public string Test()
        {
            return "Hello!";
        }

        public string UpdateCustomer(string CustomerID, Customer Customer)
        {
            throw new NotImplementedException();
        }
    }
}
