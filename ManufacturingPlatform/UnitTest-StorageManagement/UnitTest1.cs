using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DISConfigurationCloud.StorageManagement;

namespace UnitTest_StorageManagement
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateDatabase()
        {
            ModuleConfiguration.DefaulDatabasePhysicalFileLocation = "E:\\DIS2.0\\DataTest\\";

            DatabaseManager dbManager = new DatabaseManager();
            
            //string connectionString = dbManager.GetConnectionString(".\\ADK", "master", "DIS", "Chin@soft!");

            //Console.WriteLine(connectionString);
                
            //int result = dbManager.CreateDatabase("OEMKeyStore_001", connectionString);

            string result = dbManager.CreateDatabase(".\\ADK", "OEMKeyStore_009", "DIS", "Chin@soft!");

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestCreateBackupDevice() 
        {
            ModuleConfiguration.DefaultDatabaseBackupLocation = "E:\\DIS2.0\\DataTest\\";

            DatabaseManager dbManager = new DatabaseManager();

            string connectionString = dbManager.GetConnectionString(".\\ADK", "master", "DIS", "Chin@soft!");

            Console.WriteLine(connectionString);

            int result = dbManager.CreateBackupDevice("BackupDevice008", connectionString);

            Console.WriteLine(result);   
        }

        [TestMethod]
        public void TestCreateBackups()
        {
            DatabaseManager dbManager = new DatabaseManager();

            string connectionString = dbManager.GetConnectionString(".\\ADK", "master", "DIS", "Chin@soft!");

            Console.WriteLine(connectionString);

            int result = dbManager.BackupDatabase("BackupDevice008", "OEMKeyStore_008", connectionString);

            Console.WriteLine(result);   
        }

        [TestMethod]
        public void TestListingBackupHeader() 
        {
            DatabaseManager dbManager = new DatabaseManager();

            string connectionString = dbManager.GetConnectionString(".\\ADK", "master", "DIS", "Chin@soft!");

            Console.WriteLine(connectionString);

            System.Data.DataTable table = dbManager.GetBackupInformation("BackupDevice008", connectionString) as System.Data.DataTable;

            string result = "";

            using(System.IO.StringWriter writer = new System.IO.StringWriter())
            {
               table.WriteXml(writer, true);

               result = writer.ToString();
            }

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestListingBackupDevices()
        {
            DatabaseManager dbManager = new DatabaseManager();

            string connectionString = dbManager.GetConnectionString(".\\ADK", "master", "DIS", "Chin@soft!");

            Console.WriteLine(connectionString);

            string[] results = dbManager.ListBackupDevices(connectionString);

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        [TestMethod]
        public void TestListingDatabases()
        {
            DatabaseManager dbManager = new DatabaseManager();

            string connectionString = dbManager.GetConnectionString(".\\ADK", "master", "DIS", "Chin@soft!");

            Console.WriteLine(connectionString);

            string[] results = dbManager.ListDatabases(connectionString);

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        [TestMethod]
        public void TestRestoreDatabaseFromBackup()
        {
            DatabaseManager dbManager = new DatabaseManager();

            string connectionString = dbManager.GetConnectionString(".\\ADK", "master", "DIS", "Chin@soft!");

            Console.WriteLine(connectionString);

            int result = dbManager.RestoreDatabase("BackupDevice008", "2", "OEMKeyStore_008", connectionString);

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestTestingDBConnection()
        {
            DatabaseManager dbManager = new DatabaseManager();

            string connectionString = dbManager.GetConnectionString(".\\ADK", "OEMKeyStore_008", "DIS", "Chin@soft!");

            Console.WriteLine(connectionString);

            bool result = dbManager.TestConnection(connectionString);

            Console.WriteLine(result);
        }
    }
}
