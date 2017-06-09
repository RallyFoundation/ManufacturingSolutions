using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DISAdapter;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string isToContinue = "Y";

            while (isToContinue.ToLower() == "y")
            {
                Console.WriteLine("Product Key ID: ");

                long productKeyID = long.Parse(Console.ReadLine());

                Console.WriteLine("Serial Number: ");

                string serialNumber = Console.ReadLine();

                Console.WriteLine("Persistency Mode: ");

                int persistencyMode = int.Parse(Console.ReadLine());

                Console.WriteLine("DB Connection String: ");

                string connectionString = Console.ReadLine();

                Console.WriteLine("File Path: ");

                string filePath = Console.ReadLine();

                OA3DPKIDSNManager.ProductKeySerialBinder binder = new OA3DPKIDSNManager.ProductKeySerialBinder();

                if (persistencyMode == 0)
                {
                    binder.SetDBConnectionString(connectionString);
                }
                else
                {
                    binder.SetFilePath(filePath);
                }

                binder.SetPersistencyMode(persistencyMode);

                string result = binder.Bind(productKeyID, serialNumber);

                Console.WriteLine(result);

                Console.WriteLine("Continue(Y/N)? ");

                isToContinue = Console.ReadLine();
            }

            Console.Read();

            OA3ServerBased oa3s = new OA3ServerBased();

            oa3s.Parameters = new OA3ServerBasedParameters[]
            {
                new OA3ServerBasedParameters
                {
                    Parameter = new OA3ServerBasedParametersParameter[]
                    {
                        new OA3ServerBasedParametersParameter()
                        {
                            name = "", 
                            value = ""
                        },
                    },

                    OEMOptionalInfo = new OA3ServerBasedParametersOEMOptionalInfoField[][]
                    {
                        new OA3ServerBasedParametersOEMOptionalInfoField[]
                        {
                            new OA3ServerBasedParametersOEMOptionalInfoField()
                            { 
                                Name = "", 
                                Value = ""
                            }
                        },
                    },

                    CloudConfigurationID = ""
                }
            };
            
        }
    }
}
