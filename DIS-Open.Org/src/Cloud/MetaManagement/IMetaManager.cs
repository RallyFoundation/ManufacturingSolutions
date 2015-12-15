using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DISConfigurationCloud.MetaManagement
{
    public interface IMetaManager
    {
        string AddCustomerConfiguration(Customer Customer);

        int UpdateCustomerConfiguration(Customer Customer);

        Customer[] ListCustomers();

        Customer[] ListCustomers(bool IsIncludingConfigurations);

        Customer GetCustomer(string CustomerID);

        Configuration GetCustomerConfiguration(string CustomerID, ConfigurationType ConfigurationType);

        Configuration GetCustomerConfiguration(string ConfigurationID);
    }
}
