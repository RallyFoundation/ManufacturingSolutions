using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_DISConfigurationCloudServices
{
    class Program
    {
        static void Main(string[] args)
        {
            DISConfigurationCloudServiceReference.DISConfigurationCloudClient client = new DISConfigurationCloudServiceReference.DISConfigurationCloudClient("WSHttpBinding_IDISConfigurationCloud");

            client.ClientCredentials.UserName.UserName = "DIS";
            client.ClientCredentials.UserName.Password = "D!S@OMSG.msft";

            DISConfigurationCloudServiceReference.Customer[] customers = client.GetCustomers();

            Console.WriteLine(customers.Length);

            for (int i = 0; i < customers.Length; i++)
            {
                Console.WriteLine(customers[i].ID);
            }

            client.Close();

            Console.Read();
        }
    }
}
