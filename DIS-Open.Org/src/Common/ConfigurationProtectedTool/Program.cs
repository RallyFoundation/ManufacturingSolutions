using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using System.Reflection;

namespace DIS.Common.ConfigurationProtectedTool
{
    public enum AutherType
    {
        WinAuth = 1,

        SQLAuth = 2
    }

    public enum InstallType
    {
        [DescriptionAttribute("OEM")]
        Oem = 1,

        [DescriptionAttribute("TPI")]
        Tpi = 2,

        [DescriptionAttribute("Factory Floor")]
        FactoryFloor = 4,

    }

    /// <summary>
    /// The tool provide user to encrypt the configuration connection string
    /// </summary>
    public class Program
    {
        public static InstallType InstallType
        {
            get { return (InstallType)Enum.Parse(typeof(InstallType), ConfigurationManager.AppSettings["InstallType"]); }
        }

        static void Main(string[] args)
        {
            Console.Title = GetConsoleTitle();

            AppDomain.CurrentDomain.UnhandledException +=
            new UnhandledExceptionEventHandler(
                OnUnhandledException);

            Console.WriteLine("Please choose the task to complete : (1: Configuring DIS Configuration Cloud local cache (default); 2: Validate an existing DIS Configuration Cloud local cache).");

            string option = Console.ReadLine();

            if (option == "2")
            {
                CheckDISConfigurationCloudLocalCache(args);
            }
            else if (option.ToLower() != "exit")
            {
                SetDISConfigurationCloudLocalCache(args);
            }

            //SetDISConfigurationCloudLocalCache(args);

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        
        static void SetProtectedConfiguration(string[] args) 
        {
            AutherType autherType = AutherType.WinAuth;
            string server = string.Empty;
            string database = string.Empty;
            string userName = string.Empty;
            string password = string.Empty;

            Arguments commandLine = new Arguments(args);
            string configFile = commandLine["configfile"];
            string connectionString = commandLine["connectionstring"];
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Enter your SQL instance name:");
                server = Console.ReadLine();

                Console.WriteLine("Enter your database name:");
                database = Console.ReadLine();

                Console.WriteLine("Enter your Authentication Type:");
                Console.WriteLine("1: Windows Authentication");
                Console.WriteLine("2: SQL Authentication");
                autherType = (AutherType)Enum.Parse(typeof(AutherType), Console.ReadLine());

                switch (autherType)
                {
                    case AutherType.SQLAuth:
                        Console.WriteLine("Enter your database user name:");
                        userName = Console.ReadLine();

                        Console.WriteLine("Enter your database password:");
                        ConsolePasswordInput cpi = new ConsolePasswordInput();
                        cpi.PasswordInput(ref password, 128);
                        Console.WriteLine();
                        break;
                    case AutherType.WinAuth:
                        break;
                    default:
                        throw new NotSupportedException();
                }
                if (autherType == AutherType.WinAuth)
                    connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Pooling=False;MultipleActiveResultSets=True", server, database);
                else
                    connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=False;User ID={2};Password={3};Pooling=False;MultipleActiveResultSets=True", server, database, userName, password);
            }
            if (TestConnection(connectionString))
            {
                Console.WriteLine("Connect your database successfully.");
                Console.WriteLine("Start encrypting...");
                RegistryKey installer = GetRegistryKey();
                if (string.IsNullOrEmpty(configFile))
                {
                    ProteectConfigFiles(GetPaths(installer), connectionString);
                    UpdateRegdit(installer, autherType, server, database, userName, password, connectionString);
                }
                else
                {
                    if (System.IO.File.Exists(configFile))
                    {
                        ProteectConfigFile(configFile, connectionString);
                    }
                    else
                    {
                        Console.WriteLine("The path is incorrect.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Connect database is failed.");
            }
        }

        static void SetDISConfigurationCloudLocalCache(string[] args) 
        {
            string configStore, isUsingDefaultValue = "N", isCreatingNew = "N";

            do
            {
                Console.WriteLine("Please enter DIS home location, or press Enter to use default value:");
                configStore = Console.ReadLine();

                if ((!String.IsNullOrEmpty(configStore)) && (!Directory.Exists(configStore)))
                {
                    Console.WriteLine(String.Format("DIS home location \"{0}\" does NOT exist! Create a new one (Y: Yes (default); N: No)?", configStore));

                    isCreatingNew = Console.ReadLine();

                    if (isCreatingNew.ToUpper() == "N")
                    {
                        isUsingDefaultValue = "N";
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(configStore);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error(s) occurred creating new directory, please retry.");
                            isUsingDefaultValue = "N";
                        }
                    }

                    //isUsingDefaultValue = Console.ReadLine();
                    //isUsingDefaultValue = isUsingDefaultValue.ToUpper();

                    //if (isUsingDefaultValue != "N")
                    //{
                    //    isUsingDefaultValue = "Y";
                    //}
                }
                else
                {
                    if (String.IsNullOrEmpty(configStore))
                    {
                        isUsingDefaultValue = "Y";
                    }
                    else
                    {
                        isUsingDefaultValue = "";
                    }
                }
            } 
            while (isUsingDefaultValue == "N");

            if (String.IsNullOrEmpty(configStore) || (isUsingDefaultValue == "Y"))
            {
                //try
                //{
                //    configStore = GetRegistryKey().GetValue("InstallLocation").ToString();
                //}
                //catch (Exception ex)
                //{
                    configStore = AppDomain.CurrentDomain.BaseDirectory;

                    if (configStore.EndsWith("\\"))
                    {
                        configStore = configStore.Substring(0, (configStore.Length - 1));
                    }

                    configStore = configStore.Substring(0, configStore.LastIndexOf("\\"));
                //}
            }

            configStore += (!configStore.EndsWith("\\")) ? "\\Cloud-Configs.xml" : "Cloud-Configs.xml";

            Console.WriteLine(configStore);

            CloudConfigurationHelper.BuildConfigurationSets(args, configStore);
        }

        static void CheckDISConfigurationCloudLocalCache(string[] args)
        {
            string configStore, isUsingDefaultValue = "N";

            do
            {
                Console.WriteLine("Please enter DIS home location, or press Enter to use default value:");
                configStore = Console.ReadLine();

                if ((!String.IsNullOrEmpty(configStore)) && (!Directory.Exists(configStore)))
                {
                    Console.WriteLine(String.Format("DIS home location \"{0}\" does NOT exist! Use default value? (Y: Yes (Default); N: No.)", configStore));

                    isUsingDefaultValue = Console.ReadLine();
                    isUsingDefaultValue = isUsingDefaultValue.ToUpper();

                    if (isUsingDefaultValue != "N")
                    {
                        isUsingDefaultValue = "Y";
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(configStore))
                    {
                        isUsingDefaultValue = "Y";
                    }
                    else
                    {
                        isUsingDefaultValue = "";
                    }
                }
            }
            while (isUsingDefaultValue == "N");

            if (String.IsNullOrEmpty(configStore) || (isUsingDefaultValue == "Y"))
            {
                //try
                //{
                //    configStore = GetRegistryKey().GetValue("InstallLocation").ToString();
                //}
                //catch (Exception ex)
                //{
                configStore = AppDomain.CurrentDomain.BaseDirectory;

                if (configStore.EndsWith("\\"))
                {
                    configStore = configStore.Substring(0, (configStore.Length - 1));
                }

                configStore = configStore.Substring(0, configStore.LastIndexOf("\\"));
                //}
            }

            configStore += (!configStore.EndsWith("\\")) ? "\\Cloud-Configs.xml" : "Cloud-Configs.xml";

            Console.WriteLine(configStore);

            if (CloudConfigurationHelper.ValidateConfigurationSets(CloudConfigurationHelper.LoadConfigurationSets(configStore))) 
            {
                Console.WriteLine(String.Format("Validation passed! The configuration store of \"{0}\" is OK.", configStore));
            }
            else
            {
                Console.WriteLine(String.Format("Validation failed! The configuration store of \"{0}\" is having problems, please check it.", configStore));
            }
        }

        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //here's how you get the exception   
            Exception exception = (Exception)e.ExceptionObject;

            //bail out in a tidy way and perform your logging 
            Console.Error.WriteLine(exception.ToString());
            Console.Error.WriteLine(exception.StackTrace);
            if (exception.InnerException != null)
            {
                Console.Error.WriteLine(exception.InnerException.ToString());
                Console.Error.WriteLine(exception.InnerException.StackTrace);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            //Environment.Exit(666); 
        }

        private static string GetConsoleTitle()
        {
            return "Configuration Protected Tool - " + InstallTypeToString();
        }

        private static string InstallTypeToString()
        {
            switch (InstallType)
            {
                case InstallType.Oem:
                    return "OEM";
                case InstallType.Tpi:
                    return "TPI";
                case InstallType.FactoryFloor:
                    return "Factory Floor";
            }
            return string.Empty;
        }

        private static RegistryKey GetRegistryKey()
        {
            RegistryKey installer = Registry.CurrentUser
                .OpenSubKey("Software")
                .OpenSubKey("Microsoft")
                .OpenSubKey("DIS Solution");
            if (installer != null)
            {
                return installer.OpenSubKey(InstallTypeToString(), true);
            }
            return null;
        }

        private static string[] GetPaths(RegistryKey installer)
        {
            string[] filePaths = new string[] { };

            if (installer != null)
            {
                string dir = installer.GetValue("InstallLocation").ToString();
                filePaths = Directory.GetFiles(dir, "*.config", SearchOption.AllDirectories);
            }
            return filePaths;
        }

        private static void UpdateRegdit(RegistryKey installer,
            AutherType autherType,
            string server,
            string database,
            string userName,
            string password,
            string connectionString)
        {
            SetValue(installer, "AuthType", ((int)autherType).ToString());
            SetValue(installer, "ServerName", server);
            SetValue(installer, "DatabaseName", database);
            SetValue(installer, "SqlUserName", userName);
            SetValue(installer, "SqlPassword", password);
            SetValue(installer, "ConnectionString", connectionString);
        }

        private static void SetValue(RegistryKey installer, string name, string value)
        {
            installer.SetValue(name, value);
        }

        private static void ProteectConfigFiles(string[] filePaths, string connectionString)
        {
            if (filePaths.Length == 0)
                throw new ApplicationException("There is no DIS solution product installed.");

            foreach (string configFile in filePaths)
            {
                ProteectConfigFile(configFile, connectionString);
            }
        }

        private static void ProteectConfigFile(string filePath, string connectionString)
        {
            Configuration config = null;

            if (DIS.Common.Utility.ProtectConfigurationSection.IsWebConfiguration(filePath))
            {
                config = DIS.Common.Utility.ProtectConfigurationSection.OpenWebConfiguration(filePath);
            }
            else
            {
                config = DIS.Common.Utility.ProtectConfigurationSection.OpenExeConfiguration(filePath);
            }
            if (config != null)
            {
                DIS.Common.Utility.ProtectConfigurationSection.ProtectConfigFile(config, connectionString);
                Console.WriteLine(string.Format("Encrypt {0} file succesfully.", filePath));
            }
        }

        private static bool TestConnection(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("select SERVERPROPERTY('InstanceName')", connection))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        string instanceName = reader[0] as string;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static InstallType GetEnumValueFromDescription(string description)
        {
            MemberInfo[] fis = typeof(InstallType).GetFields();

            foreach (var fi in fis)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == description)
                    return (InstallType)Enum.Parse(typeof(InstallType), fi.Name);
            }

            throw new Exception("Not found");
        }
    }
}
