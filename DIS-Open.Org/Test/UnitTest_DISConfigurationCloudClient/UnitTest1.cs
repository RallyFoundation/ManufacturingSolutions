using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Contract;

namespace UnitTest_DISConfigurationCloudClient
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetCustomers()
        {
            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = "http://192.168.0.103:8818/Services/DISConfigurationCloud.svc";
            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = "DIS:D!S@OMSG.msft";

            Manager manager = new Manager(false, "");

            Customer[] customers =  manager.GetCustomers();

            if (customers != null)
            {
                foreach (var customer in customers)
                {
                    Console.WriteLine(customer.ID);
                    Console.WriteLine(customer.Name);

                    foreach (var configuration in customer.Configurations)
                    {
                        Console.WriteLine(configuration.ID);
                        Console.WriteLine(configuration.DbConnectionString);
                        Console.WriteLine(configuration.ConfigurationType);
                    }
                }
            }
            else 
            {
                Console.WriteLine("No customers available!");
            }
        }

        [TestMethod]
        public void TestGetConfiguration() 
        {
            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = "http://192.168.0.103:8818/Services/DISConfigurationCloud.svc";
            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = "DIS:D!S@OMSG.msft";

            Manager manager = new Manager(false, "");

            string configurationID = "9c0d6d0a-53b1-4c1c-8316-7ba6f7ce5f09";

            Configuration configuration = manager.GetConfiguration(configurationID);

            Console.WriteLine(configuration.ID);
            Console.WriteLine(configuration.DbConnectionString);
            Console.WriteLine(configuration.ConfigurationType);
        }
    }
}
