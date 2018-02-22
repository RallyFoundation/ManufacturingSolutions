using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
//using DISConfigurationCloud.Client;
//using DISConfigurationCloud.Contract;
using Contract;
using ResourceIntegrator;
using Utility;

namespace OA3ToolConfGen
{
    public class ModuleConfiguration
    {
        public const string AppStateKey_DBConnectionString = "DBConnectionString";
        public const string AppStateKey_CloudConfigurationID = "CloudConfigurationID";
        public const string AppStateKey_CloudServicePoint = "CloudServicePoint";
        public const string AppStateKey_CloudUserName = "CloudUserName";
        public const string AppStateKey_CloudPassword = "CloudPassword";
        public const string AppStateKey_OA3ToolConfiguration = "OA3ToolConfiguration";
        public const string AppStateKey_CloudConfigurationSets = "CloudConfigurationSets";
        public const string AppStateKey_CloudClientDBName = "CloudClientDBName";
        public const string AppStateKey_KeyTypeID = "KeyTypeID";

        public static string ParameterKey_LicensablePartNumber = "LicensablePartNumber";
        public static string ParameterKey_OEMPartNumber = "OEMPartNumber";
        public static string ParameterKey_OEMPONumber = "OEMPONumber";
        public static string ParameterKey_SerialNumber = "SerialNumber";
        public static string ParameterKey_ProductKeyIDFrom = "ProductKeyIDFrom";
        public static string ParameterKey_ProductKeyIDTo = "ProductKeyIDTo";

        public static string OHRKey_ZPC_MODEL_SKU = "ZPC_MODEL_SKU";
        public static string OHRKey_ZFRM_FACTOR_CL1 = "ZFRM_FACTOR_CL1";
        public static string OHRKey_ZFRM_FACTOR_CL2 = "ZFRM_FACTOR_CL2";
        public static string OHRKey_ZSCREEN_SIZE = "ZSCREEN_SIZE";
        public static string OHRKey_ZTOUCH_SCREEN = "ZTOUCH_SCREEN";

        public static string OHRValue_ZFRM_FACTOR_CL1_Desktop = "Desktop";
        public static string OHRValue_ZFRM_FACTOR_CL1_Notebook = "Notebook";
        public static string OHRValue_ZFRM_FACTOR_CL1_Tablet = "Tablet";

        public static string OHRValue_ZFRM_FACTOR_CL2_Standard = "Standard";
        public static string OHRValue_ZFRM_FACTOR_CL2_AIO = "AIO";
        public static string OHRValue_ZFRM_FACTOR_CL2_Ultraslim = "Ultraslim";
        public static string OHRValue_ZFRM_FACTOR_CL2_Convertible = "Convertible";
        public static string OHRValue_ZFRM_FACTOR_CL2_Detachable = "Detachable";

        public static string OHRValue_ZTOUCH_SCREEN_Touch = "Touch";
        public static string OHRValue_ZTOUCH_SCREEN_Nontouch = "Non-touch";

        public static string OA3ToolConfigurationValue_ProtocolSequence = "ncacn_ip_tcp";
        public static string OA3ToolConfigurationValue_Options_HardwareHashVersion = "1";
        public static int OA3ToolConfigurationValue_Options_HardwareHashPadding = 4000;

        public static string Configuration_Database_Name = "MDOS_FFKI_KeyStore_Test";//"MDOSKeyStore_CloudOA";
        public static string Configuration_Database_Username = "";//"MDOS";
        public static string Configuration_Database_Password = "";//"D!S@OMSG.msft";
        public static string SQL_GetConfigurationsAll = "SELECT Value.query('/CloudOAConfiguration/BusinessSettings[./CloudOABusinessSetting/IsActive=\"true\"]') AS BusinessSettings FROM Configuration WHERE Name = 'CloudOASettingVersion2'";//"SELECT BusinessID, BusinessName, Status, DatabaseType, HostName, UserName, Password, DatabaseName, TrustConnection FROM Business WHERE Status = 1 AND DatabaseType = 1";
        //public static string SQL_GetConfigurationByID = "SELECT BusinessID, BusinessName, Status, DatabaseType, HostName, UserName, Password, DatabaseName, TrustConnection FROM Business WHERE Status = 1 AND DatabaseType = 1 AND BusinessID = @BusinessID";

        public static string OEMOptionalInfoKey_ZPGM_ELIG_VAL = "ZPGM_ELIG_VAL";
        public static int OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxCount = 11;
        public static int OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxLength = 4;
        public static string OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueSeparator = "|";

        public static int KeyTypeID = 1;

        public static bool ShowConfigurationsFromServer = true;//false;

        public static Customer[] GetFactoryFloorConfigurationSets(string ServicePoint, string UserName, string Password) 
        {
            Customer[] returnValue = null;

            //string url = ServicePoint;
            //string authHeader = String.Format("{0}:{1}", UserName, Password);

            //if (url.EndsWith("/"))
            //{
            //    url = url.Substring(0, (url.Length - 1));
            //}

            //url += "/Services/DISConfigurationCloud.svc";

            //DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = url;
            //DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = authHeader;
            //DISConfigurationCloud.Client.ModuleConfiguration.CachingPolicy = CachingPolicy.RemoteOnly;

            //Manager manager = new Manager(false, null);

            //returnValue = manager.GetCustomers();

            //returnValue = (returnValue != null) ? (returnValue.Where((o) => (o.Configurations.FirstOrDefault((c) => (c.ConfigurationType == ConfigurationType.FactoryFloor)) != null)).ToArray()) : null;

            //string dbConnectionString = Utility.BuildConnectionString(ServicePoint, Configuration_Database_Name, UserName, Password);

            //string selectCmdText = SQL_GetConfigurationsAll;

            List<Customer> customers = new List<Customer>();

            string configXmlValue = "", bizID, bizName; //, server = ServicePoint, database = Configuration_Database_Name, userName = UserName, password = Password;

            //using (SqlConnection connection = new SqlConnection(dbConnectionString))
            //{
            //    SqlCommand command = connection.CreateCommand();
            //    command.CommandText = selectCmdText;
            //    command.CommandType = CommandType.Text;

            //    if (connection.State != ConnectionState.Open)
            //    {
            //        connection.Open();
            //    }

            //    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            //    {
            //        //string bizID, bizName, server, database, userName, password;

            //        while (reader.Read())
            //        {
            //            //bizID = (string)reader["BusinessID"];
            //            //bizName = (string)reader["BusinessName"];
            //            //server = (string)reader["HostName"];
            //            //database = (string)reader["DatabaseName"];
            //            //userName = (string)reader["UserName"];
            //            //password = (string)reader["Password"];
            //            //password = Utility.Decrypt(password, bizID);

            //            //customers.Add(new Customer()
            //            //{
            //            //    ID = bizID,
            //            //    Name = bizName,
            //            //    Configurations = new Configuration[] { new Configuration()
            //            //    {
            //            //        ConfigurationType = ConfigurationType.FactoryFloor,
            //            //        ID = bizID,
            //            //        DbConnectionString = Utility.BuildConnectionString(server, database, userName, password)
            //            //    }}
            //            // });

            //            configXmlValue = (string)reader["BusinessSettings"];
            //         }
            //    }
            //}

            IResourceRouter router = new ResourceRouter();

            string url = ServicePoint;

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, (url.Length - 1));
            }

            url += "/oa3/business/all";

            object result = router.Get(url, new ResourceIntegrator.Authentication() { Type = AuthenticationType.Custom }, null);

            if (result != null)
            {
                configXmlValue = result.ToString();
            }
                 
            if (!String.IsNullOrEmpty(configXmlValue))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXmlValue);

                XmlNodeList nodes = document.DocumentElement.SelectNodes("//CloudOABusinessSetting");

                for (int i = 0; i < nodes.Count; i++)
                {
                   bizID = nodes[i].SelectSingleNode("BusinessId").InnerText;
                   bizName = nodes[i].SelectSingleNode("BusinessName").InnerText;

                    customers.Add(new Customer()
                    {
                        ID = bizID,
                        Name = bizName,
                        Configurations = new Configuration[] { new Configuration()
                        {
                            ConfigurationType = ConfigurationType.FactoryFloor,
                            ID = bizID,
                            //DbConnectionString = Utility.BuildConnectionString(server, database, userName, password)
                        }}
                    });
                }
            }

            returnValue = customers.ToArray();

            return returnValue;
        }


        public static List<String> GetParameterValues(string ServicePoint, string UserName, string Password, string BusinessID, string ParameterName)
        {
            List<string> returnValue = null;

            IResourceRouter router = new ResourceRouter();

            string url = ServicePoint;

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, (url.Length - 1));
            }

            url += String.Format("/oa3/parameter/{0}/{1}/{2}", BusinessID, ParameterName, ModuleConfiguration.KeyTypeID);

            object result = router.Get(url, new ResourceIntegrator.Authentication() { Type = AuthenticationType.Custom }, null);

            if (result != null)
            {
                string paramValueJsonString = result.ToString();

                byte[] paramValueJsonBytes = Encoding.UTF8.GetBytes(paramValueJsonString);

                string[] paramValues = JsonUtility.JsonDeserialize(paramValueJsonBytes, typeof(string[]), new Type[] { typeof(string[]) }, "root") as string[];

                if (paramValues != null)
                {
                    returnValue = new List<string>(paramValues);
                }
            }

            return returnValue;
        }

        public static List<long> GetProductKeyIDRange(string ServicePoint, string UserName, string Password, int KeyCount, string BusinessID, QueryItem[] QueryItems)
        {
            List<long> returnValue = null;

            IResourceRouter router = new ResourceRouter();

            string url = ServicePoint;

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, (url.Length - 1));
            }

            url += String.Format("/oa3/keys/query/{0}/{1}/{2}", KeyCount, BusinessID, ModuleConfiguration.KeyTypeID);

            byte[] jsonBytes = JsonUtility.JsonSerialize(QueryItems, new Type[] { typeof(QueryItem[]) }, "root");

            string jsonString = Encoding.UTF8.GetString(jsonBytes);

            object result = router.Post(url, jsonString, new ResourceIntegrator.Authentication() { Type = AuthenticationType.Custom }, new Dictionary<string, string>() { { "Content-Type", "application/json;charset=utf-8" }, { "Accept", "application/json" } });

            if (result != null)
            {
                string paramValueJsonString = result.ToString();

                byte[] paramValueJsonBytes = Encoding.UTF8.GetBytes(paramValueJsonString);

                long[] paramValues = JsonUtility.JsonDeserialize(paramValueJsonBytes, typeof(long[]), new Type[] { typeof(long[]) }, "root") as long[];

                if (paramValues != null)
                {
                    returnValue = new List<long>(paramValues);
                }
            }

            return returnValue;
        }

        public static Configuration GetFactoryFloorConfigurationByBusinessID(Customer[] ConfigurationSets, string BusinessID) 
        {
            if ((ConfigurationSets != null) && (!String.IsNullOrEmpty(BusinessID)))
            {
                Customer configSet = ConfigurationSets.FirstOrDefault((o) => (o.ID == BusinessID));

                if ((configSet != null) && (configSet.Configurations != null))
	            {
                    return configSet.Configurations.FirstOrDefault((c) => (c.ConfigurationType == ConfigurationType.FactoryFloor));
	            }
            }

            return null;
        }

        public static Customer GetConfigurationSetByConfigurationID(Customer[] ConfigurationSets, string ConfigurationID) 
        {
            if ((ConfigurationSets != null) && (!String.IsNullOrEmpty(ConfigurationID))) 
            {
                return ConfigurationSets.FirstOrDefault((o) => (o.Configurations.FirstOrDefault((c) => (c.ID == ConfigurationID)) != null));
            }

            return null;
        }

        public static string GetDBConnectionString(string ServicePoint, string UserName, string Password, string ConfigurationID)
        {
            string returnValue = "";

            //string url = ServicePoint;
            //string authHeader = String.Format("{0}:{1}", UserName, Password);

            //if (url.EndsWith("/"))
            //{
            //    url = url.Substring(0, (url.Length - 1));
            //}

            //url += "/Services/DISConfigurationCloud.svc";

            //DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = url;
            //DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = authHeader;
            //DISConfigurationCloud.Client.ModuleConfiguration.CachingPolicy = CachingPolicy.RemoteOnly;

            //Manager manager = new Manager(false, null);

            //returnValue = manager.GetDBConnectionString(ConfigurationID);

            string dbConnectionString = Utility.BuildConnectionString(ServicePoint, Configuration_Database_Name, UserName, Password);

            //string selectCmdText = SQL_GetConfigurationByID;

            //using (SqlConnection connection = new SqlConnection(dbConnectionString))
            //{
            //    SqlCommand command = connection.CreateCommand();
            //    command.CommandText = selectCmdText;
            //    command.CommandType = CommandType.Text;
            //    command.Parameters.Add(new SqlParameter()
            //    {
            //        ParameterName = "@BusinessID",
            //        Value = ConfigurationID,
            //        Direction = ParameterDirection.Input,
            //        DbType = DbType.String
            //    });

            //    if (connection.State != ConnectionState.Open)
            //    {
            //        connection.Open();
            //    }

            //    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            //    {
            //        string bizID, bizName, server, database, userName, password;

            //        while (reader.Read())
            //        {
            //            bizID = (string)reader["BusinessID"];
            //            bizName = (string)reader["BusinessName"];
            //            server = (string)reader["HostName"];
            //            database = (string)reader["DatabaseName"];
            //            userName = (string)reader["UserName"];
            //            password = (string)reader["Password"];
            //            password = Utility.Decrypt(password, bizID);

            //            returnValue = Utility.BuildConnectionString(server, database, userName, password);
            //        }
            //    }
            //}

            returnValue = dbConnectionString;

            return returnValue;

        }
    }
}
