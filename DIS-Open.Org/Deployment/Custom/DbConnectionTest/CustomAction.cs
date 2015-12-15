//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DIS.Common.Utility;
using Microsoft.Deployment.WindowsInstaller;
using System.ServiceProcess;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using DISConfigurationCloud.Client;

namespace DIS.Deployment.Custom.DbConnectionTest
{
    public enum ApplicationType
    {
        [DescriptionAttribute("DIS.Services.DataPolling.exe.config")]
        DataPolling = 1,

        [DescriptionAttribute("DIS.Services.KeyProviderService.exe.config")]
        KeyProvider = 2,

        [DescriptionAttribute("DIS.Presentation.KMT.exe.config")]
        Kmt = 3,

        [DescriptionAttribute("Web.config")]
        Web = 4,

        Other = 0
    }

    public enum InstallType
    {
        Oem = 1,
        Tpi = 2,
        FactoryFloor = 3,

    }

    public static class CustomActions
    {
        private const string connectionStringSessionName = "CONNECTIONSTRING";
        private const string connectionStringSessionOnInitName = "CONNECTIONSTRINGONINIT";
        private const string connectSuccessSessionName = "CONNECTSUCCESS";
        private const string instanceSessionName = "INSTANCENAME";
        private const string databaseNameName = "DATABASENAME";
        private const string defaultInstanceName = "MSSQLSERVER";
        private const string internalServicePortName = "INPORT";
        private const string chinasoftSubject = "CN=Erich-PC";
        private const string everyone = "EVERYONE";
        private const string backupPath = "BACKUPPATH";
        private const string r4Installed = "R4INSTALLED";
        private const string InstallPath = "INSTALLLOCATION";
        private const string KMTToolPath = "Key Management Tool";
        private const string KMTName = "KMT";
        private const string DataPollingName = "DATAPOLLING";
        private const string installType = "INSTALLTYPE";
        private const string configFileSearchPattern = "*.config";
        private const string certThumbPrint = "6C849AB1CECAFE6AB81280CFA0B6AE229917F9BF";
        private const string exceptionInformationSessionName = "EXCEPTIONINFO";

        private const string DISConfigurationCloudServicePoint = "CLOUDSERVICEPOINT";
        private const string DISConfigurationCloudUsername = "CLOUDUSERNAME";
        private const string DISConfigurationCloudPassword = "CLOUDPASSWORD";

        [CustomAction]
        public static ActionResult TestCloudConnection(Session session)
        {
            IManager mananger = new Manager(false, null);

            string servicePoint = session[DISConfigurationCloudServicePoint];
            string username = session[DISConfigurationCloudUsername];
            string password = session[DISConfigurationCloudPassword];

            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = DISConfigurationCloud.Client.ModuleConfiguration.GetServicePoint(servicePoint, "/Services/DISConfigurationCloud.svc"); ;

            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = string.Format("{0}:{1}", username, password);

            string result = "";

            try
            {
                result = mananger.Test();

                if (result.ToLower() == "hello!")
                {
                    session[connectSuccessSessionName] = "1";  
                }
                else
                {
                    session[connectSuccessSessionName] = "0";
                }
            }
            catch (Exception ex)
            {
                session[connectSuccessSessionName] = "-1";
                session[exceptionInformationSessionName] = ex.ToString();
            }

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult UpdateCertificate(Session session)
        {
            string connectionString = session[connectionStringSessionName];
            try
            {
                string getSubjectSql = string.Empty;
                string updateSql = string.Empty;
                string subjectName = string.Empty;
                string thumbPrint = string.Empty;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    getSubjectSql = string.Format("SELECT [Value] FROM [Configuration] where [Name]='CertificateSubject'");
                    using (SqlCommand cmd = new SqlCommand(getSubjectSql, connection))
                    {
                        object value = cmd.ExecuteScalar();
                        if (value != null && value != DBNull.Value)
                        {
                            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToString()));
                            using (XmlReader reader = XmlReader.Create(stream))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(string));
                                subjectName = (string)serializer.Deserialize(reader);
                            }
                        }
                    }
                    thumbPrint = GetCertThumbPrint(subjectName);

                    updateSql = string.Format(
                        "UPDATE [Configuration] SET [Value] = " +
                        "'<DisCert xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                        "<Subject>{0}</Subject>" +
                        "<ThumbPrint>{1}</ThumbPrint>" +
                        "</DisCert>', [Type] ='DIS.Data.DataContract.DisCert' " +
                        "where [Name]='CertificateSubject'", subjectName, thumbPrint);
                    using (SqlCommand cmd = new SqlCommand(updateSql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    getSubjectSql = string.Format("SELECT TOP 1 [CertSubject] FROM [HeadQuarter]");

                    using (SqlCommand cmd = new SqlCommand(getSubjectSql, connection))
                    {
                        object value = cmd.ExecuteScalar();
                        if (value != null && value != DBNull.Value)
                            subjectName = value.ToString();
                    }

                    thumbPrint = GetCertThumbPrint(subjectName);
                    updateSql = string.Format(
                        "UPDATE [HeadQuarter] SET [CertThumbPrint] = '{1}' WHERE [CertSubject] = '{0}'", subjectName, thumbPrint);

                    using (SqlCommand cmd = new SqlCommand(updateSql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult ConnectDatabase(Session session)
        {
            string connectionString = session[connectionStringSessionName];
            if (TestConnection(session, connectionString) && 
                AddConstraintForKeyTypeConfiguration(connectionString) && 
                AddNewOptionInfoForDB(connectionString) &&
                AddDataUpdateReport(connectionString))
                session[connectSuccessSessionName] = "0";
            else
                session[connectSuccessSessionName] = "-1";
            return ActionResult.Success;
        }


        [CustomAction]
        public static ActionResult ConnectCloudDatabase(Session session)
        {
            string connectionString = session[connectionStringSessionName];
            if (TestConnection(session, connectionString))
                session[connectSuccessSessionName] = "0";
            else
                session[connectSuccessSessionName] = "-1";
            return ActionResult.Success;
        }

        private static bool AddDataUpdateReport(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = string.Join(Environment.NewLine, new string[] {
                        "IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataUpdateReport]') AND type in (N'U'))",
                        "BEGIN",
                        "CREATE TABLE [dbo].[DataUpdateReport](",
                        "	[MSUpdateUniqueID] [uniqueidentifier] NULL,",
                        "	[CustomerUpdateUniqueID] [uniqueidentifier] NOT NULL,",
                        "	[MSReceivedDateUTC] [datetime] NULL,",
                        "	[SoldToCustomerID] [nvarchar](10) NOT NULL,",
                        "	[ReceivedFromCustomerID] [nvarchar](10) NOT NULL,",
                        "	[TotalLineItems] [int] NULL,",
                        "	[OHRStatus] [int] NOT NULL,",
                        "	[CreatedDateUTC] [datetime] NOT NULL,",
                        "	[ModifiedDateUTC] [datetime] NOT NULL,",
                        "	CONSTRAINT [PK_ OHRDataUpdate] PRIMARY KEY CLUSTERED ",
                        "(",
                        "	[CustomerUpdateUniqueID] ASC",
                        ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]",
                        ") ON [PRIMARY]",
                        "END",
                        "",
                        "IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataUpdateReportKey]') AND type in (N'U'))",
                        "BEGIN",
                        "CREATE TABLE [dbo].[DataUpdateReportKey](",
                        "	[CustomerUpdateUniqueID] [uniqueidentifier] NOT NULL,",
                        "	[ProductKeyID] [bigint] NOT NULL,",
                        "	[Name] [nvarchar](50) NOT NULL,",
                        "	[Value] [nvarchar](80) NOT NULL,",
                        "	[ReasonCode] [nvarchar](4) NULL,",
                        "	[ReasonCodeDescription] [nvarchar](160) NULL,",
                        " CONSTRAINT [PK_OHRDataUpdateKey] PRIMARY KEY CLUSTERED ",
                        "(",
                        "	[CustomerUpdateUniqueID] ASC,",
                        "	[ProductKeyID] ASC,",
                        "	[Name] ASC",
                        ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]",
                        ") ON [PRIMARY]",
                        "",
                        "",
                        "",
                        "ALTER TABLE [dbo].[DataUpdateReportKey]  WITH NOCHECK ADD  CONSTRAINT [FK_OHRDataUpdateKey_OHRDataUpdate] FOREIGN KEY([CustomerUpdateUniqueID])",
                        "REFERENCES [dbo].[DataUpdateReport] ([CustomerUpdateUniqueID])",
                        "",
                        "",
                        "ALTER TABLE [dbo].[DataUpdateReportKey] CHECK CONSTRAINT [FK_OHRDataUpdateKey_OHRDataUpdate]",
                        "",
                        "",
                        "ALTER TABLE [dbo].[DataUpdateReportKey]  WITH NOCHECK ADD  CONSTRAINT [FK_OHRDataUpdateKey_ProductKeyInfo] FOREIGN KEY([ProductKeyID])",
                        "REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])",
                        "",
                        "",
                        "ALTER TABLE [dbo].[DataUpdateReportKey] CHECK CONSTRAINT [FK_OHRDataUpdateKey_ProductKeyInfo]",
                        "END",
                        "IF COLUMNPROPERTY(OBJECT_ID('dbo.KeyState'),'KeyState','PRECISION') = 20",
                        "BEGIN",
                        "        ALTER TABLE KeyState  ",
                        "        ALTER COLUMN [KeyState] nvarchar(30) NOT NULL",
                        "		INSERT [dbo].[KeyState] ([KeyStateId],[KeyState])  VALUES (13, N'ActivationEnabledPendingUpdate')",
                        "END",
                        "IF COLUMNPROPERTY(OBJECT_ID('dbo.ProductKeyInfo'),'ProductKeyState','PRECISION') = 20",
                        "BEGIN",
                        "        ALTER TABLE ProductKeyInfo  ",
                        "        ALTER COLUMN [ProductKeyState] nvarchar(30) NULL",
                        "END",
                         });
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static bool AddConstraintForKeyTypeConfiguration(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = string.Join(Environment.NewLine, new string[] {
                        "IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = N'PK_KeyTypeConfiguration')",
                        "BEGIN",
                        "	ALTER TABLE [dbo].[KeyTypeConfiguration] ADD  CONSTRAINT [PK_KeyTypeConfiguration] PRIMARY KEY CLUSTERED ([KeyTypeConfigurationId] ASC) ON [PRIMARY]",
                        "END",
                        "IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_KeyTypeConfiguration_HQID_PartNumber')",
                        "BEGIN",
                        "	CREATE UNIQUE NONCLUSTERED INDEX [IX_KeyTypeConfiguration_HQID_PartNumber] ON [dbo].[KeyTypeConfiguration]",
                        "	([HeadQuarterId] ASC, [LicensablePartNumber] ASC) ON [PRIMARY]",
                        "END",
                        "IF OBJECT_ID('dbo.TempKeyId', 'U') IS NULL",
                        "BEGIN",
                        "    CREATE TABLE [dbo].[TempKeyId](",
                        "        [KeyId] [bigint] NOT NULL,",
                        "        [KeyState] [tinyint] NULL,",
                        "        [KeyType] [int] NULL,",
                        "     CONSTRAINT [PK_KeyIds] PRIMARY KEY CLUSTERED ",
                        "     (",
                        "        [KeyId] ASC",
                         "    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]",
                         "   ) ON [PRIMARY]",
                        "END",
                        "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyState]') AND type in (N'U'))",
                        "BEGIN",
                        "    CREATE TABLE [dbo].[KeyState](",
                        "        [KeyStateId] [tinyint] NOT NULL,",
                        "        [KeyState] [nvarchar](20) NOT NULL,",
                        "     CONSTRAINT [PK_KeyState] PRIMARY KEY CLUSTERED  ",
                        "     (",
                        "        [KeyStateId] ASC",
                        "    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]",
                        "   ) ON [PRIMARY]",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (1, N'Fulfilled')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (2, N'Consumed')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (3, N'Bound')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (4, N'NotifiedBound')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (5, N'Returned')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (7, N'ReportedBound')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (8, N'ReportedReturn')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (9, N'ActivationEnabled')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (10, N'ActivationDenied')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (11, N'Assigned')",
                        " INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (12, N'Retrieved')",
                        "END",
                        "IF COLUMNPROPERTY( OBJECT_ID('dbo.FulfillmentInfo'),'Tags','PRECISION') IS NULL ",
                        "BEGIN",
                        "      ALTER TABLE FulfillmentInfo  ",
                        "      ADD [Tags] [nvarchar](200) NULL",
                        "END",
                        "IF COLUMNPROPERTY( OBJECT_ID('dbo.ProductKeyInfo'),'Tags','PRECISION') IS NULL ",
                        "BEGIN",
                        "      ALTER TABLE ProductKeyInfo  ",
                        "      ADD [Tags] [nvarchar](200) NULL,",
                        "          [Description] [nvarchar](500) NULL",
                        "END",
                    });
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [CustomAction]
        public static ActionResult ConnectDatabaseOnInit(Session session)
        {
            string connectionString = session[connectionStringSessionOnInitName];
            string databaseName = session[databaseNameName];
            if (!TestConnection(session, connectionString) || !TestDatabaseName(databaseName))
                session[connectSuccessSessionName] = "-1";
            else if (TestDatabaseIsExist(databaseName, connectionString))
                session[connectSuccessSessionName] = "-2";
            else
                session[connectSuccessSessionName] = "0";

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult InsertInternalWebService(Session session)
        {
            string connectionString = session[connectionStringSessionName];
            string inport = session[internalServicePortName];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = string.Format(
                        "IF NOT EXISTS(SELECT TOP 1 1 FROM [Configuration] where [Name]='InternalServiceConfig')" +
                        " insert Configuration(Name,Value,[Type]) values(N'InternalServiceConfig', '<ServiceConfig xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><UserName /><UserKey /><ServiceHostUrl>http://localhost:{0}/KeyBinding.svc</ServiceHostUrl></ServiceConfig>', N'System.String')" +
                        " else update Configuration set Value='<ServiceConfig xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><UserName /><UserKey /><ServiceHostUrl>https://localhost:{0}/KeyBinding.svc</ServiceHostUrl></ServiceConfig>' where [Name]='InternalServiceConfig'", inport);
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    session[connectSuccessSessionName] = "1";
                }
            }
            catch (Exception)
            {
                session[connectSuccessSessionName] = "0";
            }
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult ProtectConfigFiles(Session session)
        {
            try
            {
                string[] filePaths = Directory.GetFiles(session[InstallPath], configFileSearchPattern, SearchOption.AllDirectories);

                string connectionString = session[connectionStringSessionName];
                foreach (string configFile in filePaths)
                {
                    ApplicationType applicationType = GetConfigFileApplicationType(configFile);
                    switch (applicationType)
                    {
                        case ApplicationType.Web:
                            ProtectWebConfigFile(configFile, connectionString);
                            break;
                        case ApplicationType.Kmt:
                            ProtectApplicationConfigFile(configFile, connectionString);
                            break;
                        case ApplicationType.DataPolling:
                            ProtectDataPollingServiceConfigFile(session, configFile, connectionString);
                            break;
                        case ApplicationType.KeyProvider:
                            ProtectKeyProviderServiceConfigFile(configFile, connectionString);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                return ActionResult.Failure;
            }
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult AddAccessToCertificateByEveryone(Session session)
        {
            AddAccessToCertificate(GetChinasoftCertificate(), everyone);
            AddAccessToCertificate(GetChinasoftConfigCertificate(), everyone);
            return ActionResult.Success;
        }

        /// <summary>
        /// uninstall:delete ExportKeys and keyshare folder of KMT(create by customer)
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [CustomAction]
        public static ActionResult DeleteKeyShareFolder(Session session)
        {
            string defaultInstanceFolder = RemoveLastDirectoryChar(session[InstallPath]);
            string kmtPath = RemoveLastDirectoryChar(session[KMTName]);
            DirectoryInfo kmtDirectory = new DirectoryInfo(kmtPath);
            if (kmtDirectory.Exists)
            {
                string instanceFolder = kmtDirectory.Parent.FullName;
                DeleteFolder(instanceFolder);
                if (kmtPath.Equals(Path.Combine(defaultInstanceFolder, KMTToolPath), StringComparison.OrdinalIgnoreCase))
                {
                    string disFolder = Path.GetDirectoryName(defaultInstanceFolder);
                    string[] strSubFiles = Directory.GetFileSystemEntries(disFolder);
                    if (strSubFiles.Length == 0)
                        Directory.Delete(disFolder);
                }
            }
            return ActionResult.Success;
        }

        private static bool AddNewOptionInfoForDB(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = string.Join(Environment.NewLine, new string[] {
                
                        "IF COLUMNPROPERTY( OBJECT_ID('dbo.ProductKeyInfo'),'ZFRM_FACTOR_CL1','PRECISION') IS NULL ",
                        "BEGIN",
                        "      ALTER TABLE ProductKeyInfo  ",
                        "      ADD [ZFRM_FACTOR_CL1] [nvarchar](64) NULL,",
                        "          [ZFRM_FACTOR_CL2] [nvarchar](64) NULL,",
                        "          [ZSCREEN_SIZE] [nvarchar](32) NULL,",
                         "         [ZTOUCH_SCREEN] [nvarchar](32) NULL",
                        "END",
                        "   if (select name from sys.indexes where object_name(object_id)='ProductKeyInfo' and name='IX_ProductKeyInfo_ZFRM_FACTOR_CL1') is NUll   ",
                        "BEGIN",
                        "  CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZFRM_FACTOR_CL1] ON [dbo].[ProductKeyInfo](	[ZFRM_FACTOR_CL1] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]   ",
                        "END",
                         "   if (select name from sys.indexes where object_name(object_id)='ProductKeyInfo' and name='IX_ProductKeyInfo_ZFRM_FACTOR_CL2') is NUll   ",
                        "BEGIN",
                        "  CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZFRM_FACTOR_CL2] ON [dbo].[ProductKeyInfo](	[ZFRM_FACTOR_CL2] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]   ",
                        "END",
                         "   if (select name from sys.indexes where object_name(object_id)='ProductKeyInfo' and name='IX_ProductKeyInfo_ZSCREEN_SIZE') is NUll   ",
                        "BEGIN",
                        "  CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZSCREEN_SIZE] ON [dbo].[ProductKeyInfo](	[ZSCREEN_SIZE] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]   ",
                        "END",
                         "   if (select name from sys.indexes where object_name(object_id)='ProductKeyInfo' and name='IX_ProductKeyInfo_ZTOUCH_SCREEN') is NUll   ",
                        "BEGIN",
                        "  CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZTOUCH_SCREEN] ON [dbo].[ProductKeyInfo](	[ZTOUCH_SCREEN] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]   ",
                        "END",
                        "   if not exists (select *from [dbo].[Configuration] where name='IsRequireOHRData' )  ",
                           "BEGIN",
                        "    insert Configuration(Name,Value,[Type]) values(N'IsRequireOHRData', '<boolean>false</boolean>', N'System.Boolean')   ",
                           " END",
                    });
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Private Functions

        private static string RemoveLastDirectoryChar(string path)
        {
            while (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }

        private static string GetCertThumbPrint(string subject)
        {
            string thumbPrint = string.Empty;
            if (!string.IsNullOrEmpty(subject))
            {
                try
                {
                    X509Certificate2 cert = GetCertificate(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadOnly, X509FindType.FindBySubjectDistinguishedName, subject);
                    thumbPrint = cert.Thumbprint;
                }
                catch (Exception) { }
            }
            return thumbPrint;
        }

        private static void DeleteFolder(string deleteDirectory)
        {
            if (Directory.Exists(deleteDirectory))
            {
                foreach (string deleteFile in Directory.GetFileSystemEntries(deleteDirectory))
                {
                    if (File.Exists(deleteFile))
                        File.Delete(deleteFile);
                    else
                        DeleteFolder(deleteFile);
                }
                Directory.Delete(deleteDirectory);
            }
        }

        private static X509Certificate2 GetChinasoftCertificate()
        {
            return GetCertificate(StoreName.My, StoreLocation.LocalMachine, OpenFlags.OpenExistingOnly,
                X509FindType.FindBySubjectDistinguishedName, chinasoftSubject);
        }

        private static X509Certificate2 GetChinasoftConfigCertificate()
        {
            return EncryptionHelper.GetCertificate(
                        StoreName.My, StoreLocation.LocalMachine, X509FindType.FindByThumbprint, certThumbPrint);
        }

        private static X509Certificate2 GetCertificate(StoreName storeName, StoreLocation storeLocation,
            OpenFlags openFlags, X509FindType findType, string subject)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(openFlags);
            X509Certificate2Collection certs = store.Certificates.Find(findType, subject, false);
            if (certs.Count == 0)
                throw new FileNotFoundException("Certificate cannot be found.");
            else
                return certs[0];
        }

        private static void AddAccessToCertificate(X509Certificate2 cert, string user)
        {
            RSACryptoServiceProvider rsa = cert.PrivateKey as RSACryptoServiceProvider;
            if (rsa == null)
                throw new ArgumentNullException("Certificate private key is null.");

            string keyPath = FindKeyLocation(rsa.CspKeyContainerInfo.UniqueKeyContainerName);
            FileInfo file = new FileInfo(Path.Combine(keyPath, rsa.CspKeyContainerInfo.UniqueKeyContainerName));
            FileSecurity fs = file.GetAccessControl();
            NTAccount account = new NTAccount(user);
            fs.AddAccessRule(new FileSystemAccessRule(account, FileSystemRights.FullControl, AccessControlType.Allow));
            file.SetAccessControl(fs);
        }

        private static string FindKeyLocation(string keyFileName)
        {
            string keysFolder;
            string[] keys;

            keysFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"Microsoft\Crypto\RSA\MachineKeys");
            keys = Directory.GetFiles(keysFolder, keyFileName);
            if (keys.Length > 0)
                return keysFolder;

            keysFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"Microsoft\Crypto\RSA\");
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string[] keyFolders = Directory.GetDirectories(keysFolder);
            if (keyFolders.Length > 0)
                foreach (string folder in keyFolders)
                {
                    keys = Directory.GetFiles(folder, keyFileName);
                    if (keys.Length > 0)
                        return folder;
                }

            throw new FileNotFoundException("Certificate cannot be found.");
        }

        private static bool TestConnection(Session session, string connectionString)
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
                        string instanceName = reader.HasRows ? reader[0] as string : null;
                        session[instanceSessionName] = string.IsNullOrEmpty(instanceName) ?
                            defaultInstanceName : instanceName;
                        reader.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                session[exceptionInformationSessionName] = ex.ToString();

                return false;
            }
        }

        private static bool TestDatabaseName(string databaseName)
        {
            var pattern = @"^[a-zA-Z]+[\w]*$";
            if (System.Text.RegularExpressions.Regex.IsMatch(databaseName, pattern))
                return true;
            else
                return false;
        }

        private static bool TestDatabaseIsExist(string databaseName, string connectionString)
        {
            bool result = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(string.Format("SELECT name FROM sys.databases WHERE name = '{0}'", databaseName), connection))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        if (reader.HasRows)
                            result = true;
                        reader.Close();
                    }
                }
            }
            catch (Exception ex) 
            {
                
            }

            return result;
        }

        private static ApplicationType GetConfigFileApplicationType(string configFile)
        {
            return GetEnumValueFromDescription(Path.GetFileName(configFile));
        }

        private static void ProtectWebConfigFile(string configFile, string connectionString)
        {
            Configuration config = Common.Utility.ProtectConfigurationSection.OpenWebConfiguration(configFile);
            if (config != null)
            {
                Common.Utility.ProtectConfigurationSection.ProtectConfigFile(config, connectionString);
            }
        }

        private static void ProtectDataPollingServiceConfigFile(Session session, string configFile, string connectionString)
        {
            switch ((InstallType)Enum.Parse(typeof(InstallType), session[installType]))
            {
                case InstallType.Oem:
                    ProtectServiceConfigFile("DataPollingService - OEMCorp", configFile, connectionString);
                    break;
                case InstallType.Tpi:
                    ProtectServiceConfigFile("DataPollingService - TPICorp", configFile, connectionString);
                    break;
                case InstallType.FactoryFloor:
                    ProtectServiceConfigFile("DataPollingService - FactoryFloor", configFile, connectionString);
                    break;
                default:
                    break;
            }
        }

        private static void ProtectKeyProviderServiceConfigFile(string configFile, string connectionString)
        {
            if (!System.Diagnostics.EventLog.SourceExists("KeyProviderSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource("KeyProviderSource", "KeyProviderLog");
            }
            ProtectServiceConfigFile("KeyProviderService - Default", configFile, connectionString);
        }

        private static void ProtectApplicationConfigFile(string configFile, string connectionString)
        {
            Configuration config = Common.Utility.ProtectConfigurationSection.OpenExeConfiguration(configFile);
            if (config != null)
            {
                Common.Utility.ProtectConfigurationSection.ProtectConfigFile(config, connectionString);
            }
        }

        private static void ProtectServiceConfigFile(string serviceName, string configFile, string connectionString)
        {
            ServiceController service = new ServiceController(serviceName);
            int timeoutMilliseconds = 60000;
            int millisec1 = Environment.TickCount;
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

            if (service.Status != ServiceControllerStatus.Stopped)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }

            ProtectApplicationConfigFile(configFile, connectionString);

            // count the rest of the timeout
            int millisec2 = Environment.TickCount;
            timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }

        private static ApplicationType GetEnumValueFromDescription(string description)
        {
            MemberInfo[] fis = typeof(ApplicationType).GetFields();

            foreach (var fi in fis)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == description)
                    return (ApplicationType)Enum.Parse(typeof(ApplicationType), fi.Name);
            }

            return ApplicationType.Other;
        }
        #endregion
    }
}
