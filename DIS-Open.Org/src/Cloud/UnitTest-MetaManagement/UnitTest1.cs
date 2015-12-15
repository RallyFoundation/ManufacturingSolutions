using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DISConfigurationCloud.MetaManagement;

namespace UnitTest_MetaManagement
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddCustomer()
        {
            MetaManager metaManager = new MetaManager();

            Customer customer = new Customer() 
            {
                 ID = Guid.NewGuid().ToString(),
                 Name = "Customer01",
                 Configurations = new Configuration[] 
                 {
                     new Configuration()
                     {
                         ID = Guid.NewGuid().ToString(),
                         ConfigurationType = ConfigurationType.OEM,
                         DbConnectionString = "Data Source=.\\ADK;Initial Catalog=OEMKeyStore_009;User ID=DIS;Password=Chin@soft!"  
                     },
                     new Configuration()
                     {
                         ID = Guid.NewGuid().ToString(),
                         ConfigurationType = ConfigurationType.FactoryFloor,
                         DbConnectionString = "Data Source=.\\ADK;Initial Catalog=OEMKeyStore_008;User ID=DIS;Password=Chin@soft!"  
                     },
                 }
            };

           string result = metaManager.AddCustomerConfiguration(customer);

           Console.WriteLine(result);
        }

        [TestMethod]
        public void TestUpdateCustomer()
        {
            MetaManager metaManager = new MetaManager();

            Customer customer = new Customer()
            {
                ID = "12a60e22-2960-4ac8-a3e3-dee806d6b990",
                Name = "Customer001",
                Configurations = new Configuration[] 
                {
                     new Configuration()
                     {
                         ID = "7ba30088-a526-4269-af9e-ba162bda3504",
                         ConfigurationType = ConfigurationType.OEM,
                         DbConnectionString = "Data Source=.\\ADK;Initial Catalog=OEMKeyStore_007;User ID=DIS;Password=Chin@soft!"  
                     },
                     new Configuration()
                     {
                         ID = Guid.NewGuid().ToString(),
                         ConfigurationType = ConfigurationType.TPI,
                         DbConnectionString = "Data Source=.\\ADK;Initial Catalog=OEMKeyStore_009;User ID=DIS;Password=Chin@soft!"  
                     }
                 }
            };

            int result = metaManager.UpdateCustomerConfiguration(customer);

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestListCustomers() 
        {
            MetaManager metaManager = new MetaManager();

            Customer[] customers = metaManager.ListCustomers();

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Customer[]), new Type[] {typeof(Customer), typeof(Configuration), typeof(ConfigurationType), typeof(Configuration[])});

            string result = "-9";

            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                serializer.Serialize(writer, customers);

                result = writer.ToString();
            }

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestGetCustomer()
        {
            MetaManager metaManager = new MetaManager();

            string customerID = "12a60e22-2960-4ac8-a3e3-dee806d6b990";

            Customer customers = metaManager.GetCustomer(customerID);

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Customer), new Type[] { typeof(Configuration), typeof(ConfigurationType), typeof(Configuration[])});

            string result = "-9";

            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                serializer.Serialize(writer, customers);

                result = writer.ToString();
            }

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestGetCustomerConfiguration()
        {
            MetaManager metaManager = new MetaManager();

            string customerID = "12a60e22-2960-4ac8-a3e3-dee806d6b990";

            ConfigurationType confType = ConfigurationType.OEM;

            Configuration configuration = metaManager.GetCustomerConfiguration(customerID, confType);

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Configuration), new Type[] {typeof(ConfigurationType) });

            string result = "-9";

            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                serializer.Serialize(writer, configuration);

                result = writer.ToString();
            }

            Console.WriteLine(result);
        }
    }
}
