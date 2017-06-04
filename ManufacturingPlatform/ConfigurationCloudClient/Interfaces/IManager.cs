using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DISConfigurationCloud.Contract;

namespace DISConfigurationCloud.Client
{
    public interface IManager
    {
        Customer[] GetCustomers();

        Configuration GetConfiguration(string ConfigurationID);

        string GetDBConnectionString(string ConfigurationID);

        string Test();
    }
}
